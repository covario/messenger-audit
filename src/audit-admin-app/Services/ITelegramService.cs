using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Governor.Models;

namespace Covario.AuditAdminApp.Services
{
    public interface ITelegramService
    {
        void Start();
        TelegramSessionState Connect(string governorAccountPhoneNumber = null, string responseCode = null, string mfaPassword = null);
        void Disconnect();
        void ChatWatchSubscribe(string connectionId, long chatId);
        void ChatWatcherDisconnect(string connectionId);

        void NewMessage(TelegramMessage message);
        Task CreateGroupByNumber(string supportPhoneNumber, string clientPhoneNumber, string groupName);
        TelegramSessionState GetTelegramSessionState();
        IAsyncEnumerable<TelegramChat> GetGroups();
        Task<TelegramChat> GetGroup(long chatId);
        IAsyncEnumerable<TelegramContact> GetContacts();
        IAsyncEnumerable<TelegramMessage> GetMessageHistory(long chatId);
    }
}