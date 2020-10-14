using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TdLib;
using Telegram.Governor.Exceptions;
using Telegram.Governor.Models;

namespace Telegram.Governor.Services
{
    public class TelegramSession: ITelegramSession
    {
        private readonly ILogger<TelegramSession> _logger;
        private readonly TelegramConfiguration _configuration;
        private TdClient _client;
        private HashSet<long> _pendingPins = new HashSet<long>();
        public TelegramSessionState State { get; private set; } = TelegramSessionState.Uninitialized;
        public TelegramSessionState AuthState { get; private set; } = TelegramSessionState.Connected;
        private Subject<TelegramSessionState> _stateChanges = new Subject<TelegramSessionState>();
        public IObservable<TelegramSessionState> StateChange => (IObservable<TelegramSessionState>)_stateChanges;
        private Subject<TelegramMessage> _messageFeed = new Subject<TelegramMessage>();
        public IObservable<TelegramMessage> MessageFeed => (IObservable<TelegramMessage>)_messageFeed;
        private Subject<TelegramContact> _contactFeed = new Subject<TelegramContact>();
        public IObservable<TelegramContact> ContactFeed => (IObservable<TelegramContact>)_contactFeed;
        private Subject<TelegramChat> _chatFeed = new Subject<TelegramChat>();
        public IObservable<TelegramChat> ChatFeed => (IObservable<TelegramChat>)_chatFeed;

        private readonly ConcurrentDictionary<string, TelegramContact> _contactList = new ConcurrentDictionary<string, TelegramContact>();
        private readonly ConcurrentDictionary<int, TelegramContact> _userList = new ConcurrentDictionary<int, TelegramContact>();
        private int _serviceUserId;
        private readonly ConcurrentDictionary<long, TelegramChat> _chats = new ConcurrentDictionary<long, TelegramChat>();

        public TelegramSession(ILogger<TelegramSession> logger, 
            IOptions<TelegramConfiguration> configuration)
        {
            _logger = logger;
            _configuration = configuration.Value;
        }

        public void StartSession()
        {
            TdLog.SetVerbosityLevel(0);
            _client = new TdClient();
            _client.UpdateReceived += client_UpdateReceived;
        }
        
        private void ResetSession()
        {
            _client.UpdateReceived -= client_UpdateReceived;
            _client = new TdClient();
            _client.UpdateReceived += client_UpdateReceived;
        }

        private void ChangeState(TelegramSessionState state)
        {
            if (State != state)
            {
                _logger.LogInformation("Telegram State changed from {from} to {to}", State, state);
                State = state;
                _stateChanges.OnNext(state);
            }
        }
        
        public async Task Connect(string governorAccountPhoneNumber = null, string responseCode = null, string mfaPassword = null)
        {
            if (State == TelegramSessionState.PendingServiceAccount && ! string.IsNullOrEmpty(governorAccountPhoneNumber))
            {
                ChangeState(TelegramSessionState.Unauthorized);
                await _client.ExecuteAsync(new TdApi.SetAuthenticationPhoneNumber
                {
                    PhoneNumber = governorAccountPhoneNumber
                });
            }
            if (State == TelegramSessionState.PendingCode && !string.IsNullOrEmpty(responseCode))
            {
                ChangeState(TelegramSessionState.Unauthorized);
                await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
                {
                    Code = responseCode
                });
            }
            if (State == TelegramSessionState.PendingPassword && !string.IsNullOrEmpty(mfaPassword))
            {
                ChangeState(TelegramSessionState.Unauthorized);
                await _client.ExecuteAsync(new TdApi.CheckAuthenticationCode
                {
                    Code = responseCode
                });
            }
        }

        public async IAsyncEnumerable<TelegramChat> GetChats()
        {
            foreach (var chat in _chats.Values)
            {
                yield return chat;
            }
        }

        public async Task<TelegramChat> GetGroup(long chatId)
        {
            return _chats[chatId];
        }

        public async IAsyncEnumerable<TelegramContact> GetContacts()
        {
            foreach (var contact in _contactList.Values)
            {
                yield return contact;
            }
        }

        private async void client_UpdateReceived(object sender, TdApi.Update e)
        {
            switch (e)
            {
                case TdApi.Update.UpdateConnectionState connState:

                    _logger.LogDebug("Telegram Connection State Update: {COnnectionState}", connState.State.GetType().Name);

                    switch (connState.State)
                    {
                        case TdApi.ConnectionState.ConnectionStateUpdating s1:
                        case TdApi.ConnectionState.ConnectionStateReady s2:
                        case TdApi.ConnectionState.ConnectionStateConnecting s3:
                            
                            ChangeState(AuthState);
                            break;
                            
                        default:
                            
                            ChangeState(TelegramSessionState.Disconnected);
                            break;
                    }
                    
                    break;

                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateWaitTdlibParameters:

                    ChangeState(TelegramSessionState.Initializing);
                    
                    await _client.ExecuteAsync(new TdApi.SetTdlibParameters
                    {
                        Parameters = new TdApi.TdlibParameters
                        {
                            ApiId = _configuration.ApiId,
                            ApiHash = _configuration.ApiHash,
                            ApplicationVersion = "1.3.0",
                            DeviceModel = "PC",
                            SystemLanguageCode = "en",
                            SystemVersion = RuntimeInformation.OSDescription,
                            DatabaseDirectory = System.IO.Path.Combine(_configuration.Data,"db"),
                            FilesDirectory = System.IO.Path.Combine(_configuration.Data, "files")
                        }
                    });
                    ChangeState(TelegramSessionState.Initialized);
                    break;

                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateWaitEncryptionKey:
                {
                    ChangeState(TelegramSessionState.SettingEncryptionKey);
                    
                    var key = _configuration.EncryptionKey;
                    var bkey = new byte[key.Length / 2];
                    for (int i = 0; i < key.Length / 2; i++)
                    {
                        bkey[i] = Convert.ToByte(key.Substring(i * 2, 2), 16);
                    }

                    await _client.ExecuteAsync(new TdApi.CheckDatabaseEncryptionKey() {EncryptionKey = bkey });

                    break;
                }
                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateWaitPhoneNumber:

                    ChangeState(AuthState = TelegramSessionState.PendingServiceAccount);
                    break;

                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateWaitCode:

                    ChangeState(AuthState = TelegramSessionState.PendingCode);
                    break;

                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateWaitPassword:

                    ChangeState(AuthState = TelegramSessionState.PendingPassword);
                    break;

                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateReady:

                    ChangeState(AuthState = TelegramSessionState.Connected);
                    LoadContactsAndChats();
                    break;

                case TdApi.Update.UpdateAuthorizationState authState
                    when authState.AuthorizationState is TdApi.AuthorizationState.AuthorizationStateClosed:

                    ResetSession();
                    break;
                
                case TdApi.Update.UpdateAuthorizationState authState:

                    _logger.LogInformation("Unexpected Telegram State {state}", authState.AuthorizationState.DataType);
                    ChangeState(AuthState = TelegramSessionState.Unauthorized);
                    break;

                case TdApi.Update.UpdateUser updateUser:

                    var user = await GetTelegramContact(updateUser.User.Id);

                    user.FirstName = updateUser.User.FirstName;
                    user.LastName = updateUser.User.LastName;
                    user.Username = updateUser.User.Username;
                    break;

                case TdApi.Update.UpdateOption option
                    when option.Value is TdApi.OptionValue.OptionValueString value:

                    _logger.LogDebug("Option: {Name} = {Value}", option.Name, value.Value);
                    break;

                case TdApi.Update.UpdateOption option
                    when option.Value is TdApi.OptionValue.OptionValueBoolean value:

                    _logger.LogDebug("Option: {Name} = {Value}", option.Name, value.Value);
                    break;

                case TdApi.Update.UpdateOption option
                    when option.Value is TdApi.OptionValue.OptionValueInteger value:

                    _logger.LogDebug("Option: {Name} = {Value}", option.Name, value.Value);
                    break;

                case TdApi.Update.UpdateOption option
                    when option.Value is TdApi.OptionValue.OptionValueEmpty value:

                    _logger.LogDebug("Option: {Name} is Empty", option.Name, null);
                    break;

                case TdApi.Update.UpdateMessageSendSucceeded sentMessage:
                    
                    if (_pendingPins.Contains(sentMessage.OldMessageId))
                    {
                        await _client.ExecuteAsync(new TdApi.PinChatMessage()
                        {
                            ChatId = sentMessage.Message.ChatId,
                            MessageId = sentMessage.Message.Id,
                            DisableNotification = true
                        });
                        _pendingPins.Remove(sentMessage.OldMessageId);
                    }

                    break;

                case TdApi.Update.UpdateNewMessage newMessage:
                    if (!(newMessage.Message.IsOutgoing &&
                          newMessage.Message.SendingState is TdApi.MessageSendingState.MessageSendingStatePending))
                    {
                        if (newMessage.Message.Content is TdApi.MessageContent.MessageChatChangeTitle newTitle)
                        {
                            var chatChanged = await GetChat(newMessage.Message.ChatId);
                            _logger.LogInformation("Chat title changed for {chatId} from {oldTitle} to {chatTitle}", chatChanged.ChatId, chatChanged.Title, newTitle.Title);
                            chatChanged.Title = newTitle.Title;
                        }
                        else
                        {
                            var msg = await CreateMessage(newMessage.Message);

                            if (msg != null)
                                _messageFeed.OnNext(msg);
                        }
                    } 
                    else
                    {
                        _logger.LogInformation("Unknown message type received {chatId} {chatTitle}", newMessage.Message.ChatId, newMessage.DataType);
                    }

                    break;

                case TdApi.Update.UpdateNewChat newChat:

                    var chat = await GetChat(newChat.Chat.Id, newChat.Chat);
                    _logger.LogInformation("Loading chat {chatId} {chatTitle}", chat.Id, chat.Title);

                    break;

                case TdApi.Update.UpdateUserStatus userStatus:
                
                    break;

                default:

                    _logger.LogDebug("Unhandled Telegram Event {eventType}", e.DataType);
                    break;
            }
        }

        private async Task<TelegramMessage> CreateMessage(TdApi.Message tdMessage)
        {
            if (tdMessage.Content is TdApi.MessageContent.MessageText text)
            {
                var sent = DateTimeOffset.FromUnixTimeSeconds((long)tdMessage.Date).UtcDateTime;
                var chat = await GetChat(tdMessage.ChatId);
                var msgSender = await GetTelegramContact(tdMessage.SenderUserId);
                if (_chats.ContainsKey(tdMessage.ChatId))
                {
                    var msg = new TelegramMessage
                    {
                        MessageId = tdMessage.Id,
                        ChatId = tdMessage.ChatId,
                        ChatName = chat.Title,
                        Text = text.Text.Text,
                        Sent = sent,
                        SenderContact = msgSender
                    };
                    return msg;
                }
            }

            return null;
        }


        private async void LoadContactsAndChats()
        {
            var myUserIdRes = await _client.ExecuteAsync(new TdApi.GetOption { Name = "my_id" });
            _serviceUserId = ((TdApi.OptionValue.OptionValueInteger)myUserIdRes).Value;
            long offsetOrder = long.MaxValue;
            long offsetId = 0;
            int limit = 1000;

            var chats = await _client.ExecuteAsync(new TdApi.GetChats { OffsetOrder = offsetOrder, Limit = limit, OffsetChatId = offsetId });
            foreach (var chatId in chats.ChatIds)
            {
                var chat = await GetChat(chatId);

                if (chat != null)
                {
                    _logger.LogInformation("Auditing {chatId} {chatTitle}", chat.Id, chat.Title);
                }
            }
        }

        private readonly ConcurrentDictionary<long, AutoResetEvent> _loadingChats = new ConcurrentDictionary<long, AutoResetEvent>();
        
        private async Task<TelegramChat> GetChat(long chatId, TdApi.Chat chat = null)
        {
            if (_chats.TryGetValue(chatId, out var tg))
            {
                return tg;
            }

            AutoResetEvent wait;

            if (_loadingChats.TryGetValue(chatId, out wait))
            {
                wait.WaitOne();
                return _chats[chatId];
            }

            wait = new AutoResetEvent(false);
            if (!_loadingChats.TryAdd(chatId, wait))
            {
                wait.WaitOne();
                return _chats[chatId];
            }

            int tryIndex = 0;
            while (tryIndex < 5)
            {
                tryIndex++;
                const string tdLibThrottlePrefix = "Too Many Requests: retry after";
                
                try
                {
                    if (chat == null)
                    {
                        chat = await _client.ExecuteAsync(new TdApi.GetChat {ChatId = chatId});
                    } 

                    switch (chat.Type)
                    {
                        case TdApi.ChatType.ChatTypeBasicGroup existingBasicGroup:
                        {
                            if (chat.LastReadInboxMessageId != 0)
                            {
                                _logger.LogInformation("Known last read for chat {chat} {messageId}", chat.Id, chat.LastReadInboxMessageId);
                            }

                            tg = new TelegramChat()
                            {
                                ChatId = chatId,
                                BasicGroupId = existingBasicGroup.BasicGroupId,
                                Title = chat.Title
                            };
                            var basicGroupFullInfo = await _client.ExecuteAsync(new TdApi.GetBasicGroupFullInfo()
                                {BasicGroupId = existingBasicGroup.BasicGroupId});
                            var users = basicGroupFullInfo.Members.Where(m => m.UserId != _serviceUserId).Select(m => m.UserId);
                            var members = new List<TelegramContact>();
                            foreach (var user in users)
                            {
                                var contact = await GetTelegramContact(user);
                                members.Add(contact);
                            }

                            tg.Members = members.ToArray();

                            break;
                        }
                        case TdApi.ChatType.ChatTypePrivate privateChat:
                        {
                            tg = new TelegramChat()
                            {
                                ChatId = chatId,
                                Title = chat.Title
                            };
                            var members = new List<TelegramContact>();
                            var contact = await GetTelegramContact(privateChat.UserId);
                            members.Add(contact);

                            tg.Members = members.ToArray();

                            break;
                        }
                        case TdApi.ChatType.ChatTypeSupergroup superGroup:
                        {
                            tg = new TelegramChat()
                            {
                                ChatId = chatId,
                                Title = chat.Title
                            };
                            var group = await _client.ExecuteAsync(new TdApi.GetSupergroup()
                                { SupergroupId = superGroup.SupergroupId});
                            try
                            {
                                var groupMember = await _client.ExecuteAsync(new TdApi.GetSupergroupMembers()
                                    {SupergroupId = superGroup.SupergroupId, Limit = 1000, Offset = 0});
                                var users = groupMember.Members.Where(m => m.UserId != _serviceUserId).Select(m => m.UserId);
                                var members = new List<TelegramContact>();
                                foreach (var user in users)
                                {
                                    var contact = await GetTelegramContact(user);
                                    members.Add(contact);
                                }

                                tg.Members = members.ToArray();
                            }
                            catch (Exception ex)
                            {
                                _logger.LogInformation("Error loading supergroup members {group}, {message}", tg.Title, ex.Message);
                            }

                            break;
                        }
                        default:
                        {
                            tg = new TelegramChat()
                            {
                                ChatId = chatId,
                                BasicGroupId = 0,
                                Title = chat.Title,
                            };
                            break;
                        }
                    }

                    _chats.TryAdd(tg.ChatId, tg);
                    _loadingChats.Remove(chatId, out wait);
                    break;
                }
                catch (Exception ex) when (ex.Message.StartsWith(tdLibThrottlePrefix))
                {
                    var howLong = int.Parse(ex.Message.Substring(tdLibThrottlePrefix.Length));
                    _logger.LogWarning("TDLib throttling in progress waiting {howLong} seconds", howLong);
                    Thread.Sleep((howLong + 5) * 1000);
                }
            }
            wait.Set();
            _chatFeed.OnNext(tg);
            return tg;
        }

        public async Task CreateGroup(TelegramContact serviceContact, TelegramContact clientContact, string title)
        {
            try
            {
                string prefix = "Covario - ";
                if (!title.StartsWith(prefix))
                    title = prefix + title;

                TelegramChat tc = _chats.Values.FirstOrDefault(c => c.Title == title);
                if (tc == null)
                {
                    var basicGroup = await _client.ExecuteAsync(new TdApi.CreateNewBasicGroupChat()
                    {
                        Title = title,
                        UserIds = new[]
                        {
                            _serviceUserId,
                            serviceContact.UserId,
                            clientContact.UserId
                        }
                    });
                    tc = new TelegramChat()
                    {
                        ChatId = basicGroup.Id,
                        BasicGroupId = (basicGroup.Type as TdApi.ChatType.ChatTypeBasicGroup)?.BasicGroupId ?? 0,
                        Title = title,
                        Members = new[] {serviceContact, clientContact}
                    };
                    _chats.TryAdd(tc.ChatId, tc);
                }

                await _client.ExecuteAsync(new TdApi.SetChatPermissions()
                {
                    ChatId = tc.ChatId,
                    Permissions = new TdApi.ChatPermissions
                    {
                        CanSendMessages = true,
                        CanSendMediaMessages = false,
                        CanSendPolls = false,
                        CanSendOtherMessages = false,
                        CanAddWebPagePreviews = true,
                        CanChangeInfo = false,
                        CanInviteUsers = false,
                        CanPinMessages = false
                    }
                });
                await _client.ExecuteAsync(new TdApi.SetChatDescription()
                {
                    ChatId = tc.ChatId,
                    Description = "Covar.io Client Support Chat"
                });
                await _client.ExecuteAsync(new TdApi.SetChatDescription()
                {
                    ChatId = tc.ChatId,
                    Description = "Covar.io Client Support Chat"
                });

                var parts = new[]
                {
                    @"All communication in this conversation is logged by our systems for auditing and compliance mandates, as per our Terms of Service: ",
                    @"https://messenger.covar.io/terms",
                    @". Communicating here acknowledges your acceptance of these terms."
                };

                var tosMessage = await _client.ExecuteAsync(new TdApi.SendMessage()
                {
                    ChatId = tc.ChatId,
                    Options = new TdApi.SendMessageOptions {FromBackground = false, DisableNotification = true},
                    InputMessageContent = new TdApi.InputMessageContent.InputMessageText()
                    {
                        Text = new TdApi.FormattedText()
                        {
                            Text = string.Join("", parts),
                            Entities = new TdApi.TextEntity[]
                            {
                                new TdApi.TextEntity()
                                {
                                    Type = new TdApi.TextEntityType.TextEntityTypeUrl(),
                                    Offset = parts[0].Length,
                                    Length = parts[1].Length
                                },
                            }
                        },
                    }
                });

                _pendingPins.Add(tosMessage.Id);
            }
            catch (Exception ex)
            {
                throw new TelegramCreateGroupException(ex.Message, ex);
            }
        }


        public async Task Disconnect()
        {
            await _client.ExecuteAsync(new TdApi.LogOut());
            _contactList.Clear();
            _userList.Clear();
            _serviceUserId = 0;
            _chats.Clear();
        }

        public void Dispose()
        {
            
        }

        async Task<TelegramContact> ITelegramSession.GetContactForNumber(string phoneNumber)
        {
            phoneNumber = phoneNumber.Trim().Replace(" ", "");
            if (phoneNumber.StartsWith("+"))
                phoneNumber = phoneNumber.Remove(0, 1);

            TelegramContact contact;
            if (phoneNumber.StartsWith("@"))
            {
                phoneNumber = phoneNumber.Remove(0,1).Trim();
                contact = _contactList.Values.FirstOrDefault(c => c.Username == phoneNumber);
                if (contact == null)
                {
                    var q = await _client.ExecuteAsync(new TdApi.SearchPublicChats() { Query = "@" + phoneNumber });
                    if (q.ChatIds.Length == 1)
                    {
                        var chat = await _client.ExecuteAsync(new TdApi.GetChat() { ChatId = q.ChatIds[0] });
                        if (chat.Type is TdApi.ChatType.ChatTypePrivate privateChat)
                        {
                            return await GetTelegramContact(privateChat.UserId);
                        }
                    }
                }
                else
                {
                    return contact;
                }
            }

            if (!_contactList.TryGetValue(phoneNumber, out contact))
            {
                var importedContacts = await _client.ExecuteAsync(new TdApi.ImportContacts()
                {
                    Contacts = new[]
                    {
                        new TdApi.Contact()
                        {
                            PhoneNumber = phoneNumber,
                        },
                    }
                });
                if (importedContacts.UserIds[0] == 0)
                    return null;

                return await GetTelegramContact(importedContacts.UserIds[0]);
            }
            return contact;
        }
        
        async Task<TelegramContact> GetTelegramContact(int userId)
        {
            if (_userList.TryGetValue(userId, out var contact))
            {
                return contact;
            }
            //var user2 = await _client.ExecuteAsync(new TdApi.GetUserFullInfo() { UserId = userId });
            var user = await _client.ExecuteAsync(new TdApi.GetUser {UserId = userId});
            contact = new TelegramContact(userId, user.PhoneNumber)
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username
            };
            _userList.TryAdd(userId, contact);
            _contactList.TryAdd(user.PhoneNumber, contact);
            return contact;
        }

        public async IAsyncEnumerable<TelegramMessage> GetMessageHistory(long chatId, long fromMessageId = 0)
        {
            var count = await _client.ExecuteAsync(new TdApi.GetChat() { ChatId = chatId });

            var left = 1000;
            var offset = -99;
            while (left > 0)
            {
                TdApi.Messages messages = await _client.ExecuteAsync(new TdApi.GetChatHistory() { ChatId = chatId, FromMessageId = fromMessageId, Offset = offset, Limit = 1000, OnlyLocal = false });
                offset = 0;
                if (messages.Messages_.Length == 0)
                    break;
                
                left -= messages.Messages_.Length;
                foreach (var message in messages.Messages_)
                {
                    if (message != null)
                    {
                        fromMessageId = message.Id;
                        var msg = await CreateMessage(message);
                        
                        if (msg != null)
                            yield return msg;
                    }
                }
            }
        }
    }
}
