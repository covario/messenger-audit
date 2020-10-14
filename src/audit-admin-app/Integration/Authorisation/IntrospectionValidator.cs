using System;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Covario.AuditAdminApp.Configuration;
using Microsoft.Extensions.Options;

namespace Covario.AuditAdminApp.Integration.Authorisation
{
    public class IntrospectionValidator : IIntrospectionValidator
    {
        private readonly HttpClient _client;
        private readonly IdentityServerOptions _options;

        public IntrospectionValidator(HttpClient client, IOptions<IdentityServerOptions> options)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _options = options?.Value ?? throw new ArgumentNullException(nameof (options));
        }

        public async Task<bool> ValidateToken(string token)
        {
            var request = new TokenIntrospectionRequest
            {
                Address = $"{_options.Authority}/connect/introspect",
                ClientId = _options.ApiName,
                ClientSecret = _options.ApiSecret,
                Token = token
            };

            var response = await _client.IntrospectTokenAsync(request);

            if (response.IsError) 
                throw new Exception(response.Error);

            // You might want to validate the scope here

            return response.IsActive;
        }
    }
}
