using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Governor.Models;

namespace Telegram.Governor.Services
{
    public interface ITelegramSession: IDisposable
    {
        void StartSession();

        TelegramSessionState State { get; }

        IObservable<TelegramSessionState> StateChange { get; }

        IObservable<TelegramMessage> MessageFeed { get; }
        IObservable<TelegramChat> ChatFeed { get; }

        Task Connect(string governorAccountPhoneNumber = null, string responseCode = null, string mfaPassword = null);

        IAsyncEnumerable<TelegramChat> GetChats();
        
        Task<TelegramChat> GetGroup(long chatId);

        IAsyncEnumerable<TelegramContact> GetContacts();
        
        
        Task CreateGroup(TelegramContact serviceContact, TelegramContact clientContact, string title);

        Task<TelegramContact> GetContactForNumber(string phoneNumber);
        
        IAsyncEnumerable<TelegramMessage> GetMessageHistory(long chatId, long fromMessageId = 0);

        Task Disconnect();
    }
}