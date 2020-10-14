using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Covario.AuditAdminApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization.Internal;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Telegram.Governor.Models;
using Telegram.Governor.Services;

namespace Covario.AuditAdminApp.Services
{
    public class MessageAuditService: IMessageAuditService
    {
        private readonly ILogger<MessageAuditService> _logger;
        private readonly AuditConfiguration _configuration;
        private ConcurrentDictionary<long, object> _fileLocks = new ConcurrentDictionary<long, object>();

        public MessageAuditService(
            ILogger<MessageAuditService> logger,
            IOptions<AuditConfiguration> configuration)
        {
            _logger = logger;
            _configuration = configuration.Value;
        }

        public void LogMessage(IList<TelegramMessage> messages)
        {
            if (!messages.Any())
                return;
            
            var chatId = messages.First().ChatId;
            var fileSync = _fileLocks.GetOrAdd(chatId, _ => new object());

            lock (fileSync)
            {
                var hex = chatId.ToString("X");
                if (hex.Length < 16)
                    hex = (new string('0', 16 - hex.Length)) + hex;

                var logFile = new FileInfo(Path.Combine(_configuration.LogPath, $"{hex}.dat"));
                if (!logFile.Directory.Exists)
                    logFile.Directory.Create();
                
                var path = logFile.FullName;

                if (!File.Exists(path))
                    _logger.LogInformation("Creating {log}", path);

                using var log = File.AppendText(path);
                foreach (var message in messages)
                {
                    string json = System.Text.Json.JsonSerializer.Serialize(message);
                    string base64String = Convert.ToBase64String(Encoding.UTF8.GetBytes(json));
                    log.WriteLine(base64String);
                }
            }
        }
        
        private string GetTagForId(long id)
        {
            var tag = id.ToString("X");
            if (tag.Length < 16)
                tag = (new string('0', 16 - tag.Length)) + tag;

            return tag;
        }

        public IEnumerable<TelegramMessage> ReadLog(long chatId)
        {
            return ReadLog(GetTagForId(chatId), chatId);
        }

        public IEnumerable<TelegramMessage> ReadLog(string chatTag)
        {
            return ReadLog(chatTag, Convert.ToInt64(chatTag, 16));
        }
        public bool LogExists(long chatId)
        {
            return LogExists(GetTagForId(chatId));
        }

        public bool LogExists(string chatTag)
        {
            var log = Convert.ToInt64(chatTag, 16);
            
            var fileSync = _fileLocks.GetOrAdd(log, _ => new object());
            lock (fileSync)
            {
                var logFile = new FileInfo(Path.Combine(_configuration.LogPath, $"{chatTag}.dat"));
                
                return logFile.Exists;
            }
        }

        public IEnumerable<TelegramMessage> ReadLog(string chatTag, long chatId)
        {
            var fileSync = _fileLocks.GetOrAdd(chatId, _ => new object());

            lock (fileSync)
            {
                var logFile = new FileInfo(Path.Combine(_configuration.LogPath, $"{chatTag}.dat"));
                if (!logFile.Directory.Exists)
                    logFile.Directory.Create();

                var path = logFile.FullName;
                if (!File.Exists(path))
                    yield break;

                using var log = File.OpenText(path);
                while (! log.EndOfStream)
                {
                    var base64Line = log.ReadLine();
                    var bytes = Convert.FromBase64String(base64Line);
                    string line = Encoding.UTF8.GetString(bytes);
                    var message = System.Text.Json.JsonSerializer.Deserialize<TelegramMessage>(line);
                    yield return message;
                }
            }
        }
    }
}
