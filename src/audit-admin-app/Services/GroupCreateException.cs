using System;

namespace Covario.AuditAdminApp.Services
{
    public class TelegramCreateGroupException : ApplicationException
    {
        public TelegramCreateGroupException()
        {
        }

        public TelegramCreateGroupException(string message) : base(message)
        {
        }

        public TelegramCreateGroupException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}