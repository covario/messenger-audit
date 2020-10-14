using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covario.ChatApp.models;
using Telegram.Governor;
using Telegram.Governor.Models;

namespace Covario.ChatApp.hub
{
    public interface IAdminHub
    {
        #region "Server side actions"

        //Task<Contact> Login(string username, string password);
        Task Login(string username, string password);
        
        Task GetTelegramSessionState();

        Task Connect(string governorAccountPhoneNumber = null, string responseCode = null, string mfaPassword = null);
        
        Task Disconnect();

        Task ChatWatch(long chatId);

        Task AddClientContact(string phoneNumber);

        Task AddServiceContact(string phoneNumber);

        //Task<IEnumerable<TelegramContact>> GetClientContacts();
        Task GetClientContacts();

        //Task<IEnumerable<TelegramContact>> GetServiceContacts(); 
        Task GetServiceContacts();

        Task CreateGroup(TelegramContact serviceContact, TelegramContact clientContact, string groupName);

        Task CreateGroupByNumber(string serivePhoneNumber, string clientPhoneNumber, string groupName);

        #endregion

        #region "Client side events"

        Task OnStateUpdated(TelegramSessionState state);
        
        Task OnClientContactAdded(TelegramContact contact);
        
        Task OnServiceContactAdded(TelegramContact contact);
        
        Task OnGroupAdded(TelegramContact clientContact, TelegramContact serviceContact);

        Task OnNewMessage(TelegramMessage message);

        #endregion
    }
}
