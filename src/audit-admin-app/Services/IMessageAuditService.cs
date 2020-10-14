using System.Collections.Generic;
using Telegram.Governor.Models;

namespace Covario.AuditAdminApp.Services
{
    public interface IMessageAuditService
    {
        void LogMessage(IList<TelegramMessage> messages);

        IEnumerable<TelegramMessage> ReadLog(long chatId);
        
        IEnumerable<TelegramMessage> ReadLog(string chatTag);
        bool LogExists(string chatTag);
        bool LogExists(long chatId);
    }
}