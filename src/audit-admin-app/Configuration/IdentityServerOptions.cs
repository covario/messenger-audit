using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Covario.AuditAdminApp.Configuration
{
    public class IdentityServerOptions
    {
        public const string SettingKey = "IdentityServer";
        public const string AdminPolicyName = "AuditPolicy";
        public const string AdminScopeName = "audit.groupmanagement";

        public string Authority { get; set; }
        public string ApiName { get; set; }
        public string ApiSecret { get; set; }
        public string PolicyName { get; set; }
        public string Scope { get; set; }
    }
}
