using System.Collections.Generic;

namespace Covario.AuditAdminApp.Models
{
    public class AuditConfiguration
    {
        public const string SettingKey = "Audit";

        public string LogPath { get; set; }
    }
}
