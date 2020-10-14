using System;
using Covario.Diagnostics.HealthChecks;
using Covario.Diagnostics.HealthChecks.DependencyInjection;
using Covario.Logging;
using Covario.AuditAdminApp.Configuration;
using Covario.AuditAdminApp.Hubs;
using Covario.AuditAdminApp.Models;
using Covario.AuditAdminApp.Pipeline;
using Covario.AuditAdminApp.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Telegram.Governor.Models;
using Telegram.Governor.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Covario.AuditAdminApp
{
    public class Startup
    {
        private readonly string _corsPolicy = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IdentityServerOptions>(Configuration.GetSection("IdentityServer"));

            services.Configure<TelegramConfiguration>(Configuration.GetSection(TelegramConfiguration.SettingKey));
            services.Configure<AuditConfiguration>(Configuration.GetSection(AuditConfiguration.SettingKey));
            services.Configure<IdentityServerOptions>(Configuration.GetSection(IdentityServerOptions.SettingKey));
            services.AddSingleton<ITelegramSession, TelegramSession>();
            services.AddSingleton<IMessageAuditService, MessageAuditService>();
            services.AddSingleton<ITelegramService, TelegramService>();
            services.AddHostedService<TelegramServiceHost>();

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            var authConfig = Configuration.GetSection(IdentityServerOptions.SettingKey).Get<IdentityServerOptions>();

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = "Cookies";
                    options.DefaultSignInScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = authConfig.Authority; 
                    options.ClientId = authConfig.ApiName; 
                    options.ClientSecret = authConfig.ApiSecret;
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.ResponseMode = OpenIdConnectResponseMode.FormPost;
                    options.SignedOutCallbackPath = "/index";
                    options.RequireHttpsMetadata = true;
                    options.SignedOutRedirectUri = @"";
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.UsePkce = true;
                    options.TokenValidationParameters.NameClaimType = "name"; // ClaimTypes.Name;
                });

            services
                .AddAuthorization(options =>
                {
                    options.AddPolicy(IdentityServerOptions.AdminPolicyName, builder =>
                    {
                        builder.RequireAuthenticatedUser();
                        builder.RequireScope(IdentityServerOptions.AdminScopeName);
                    });
                });
            
            services.AddCors(options =>
            {
                options.AddPolicy(name: _corsPolicy,
                    builder =>
                    {
                        builder.WithOrigins(
                            "http://localhost");
                    });
            });
            
            services.AddResponseCompression();

            var key = $"DataProtection.AuditAdministration";
            services
                .AddDataProtection()
                .SetApplicationName(key);

            services
                .AddHealthChecks()
                .AddSelfHealthCheck();

            services.AddControllers()
                    .AddControllersAsServices();
            services.AddSignalR();
            
            services.AddRazorPages(options => {
                options.Conventions.AuthorizeFolder("/admin");
                options.Conventions.AddPageRoute("/admin/messageLog", "Get/{chatId}");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseForwardedHeaders(new ForwardedHeadersOptions {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseHsts(options => options.MaxAge(days: 30));
            }

            app.UseStaticFiles();
            app.UseRouting();
            // app.UsePageLogging();
            app.UseHttpsSaver();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseHealthChecksEndpoints();

            app.UseHttpRequestLogging(o =>
            {
                o.EnableRequestBody = true;
                o.EnableRequestForm = true;
                o.EnableRequestHeaders = true;
                o.EnableRequestQuery = true;
                o.EnableResponseHeaders = true;
            });
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapHub<AdminHub>("/admin-hub");
            });
        }
    }
}
