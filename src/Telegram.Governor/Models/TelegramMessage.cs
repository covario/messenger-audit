using System;

namespace Telegram.Governor.Models
{
    public class TelegramMessage
    {
        public TelegramMessage()
        {
        }

        public long MessageId { get; set; }

        public string Text { get; set; }
        
        public DateTime Sent { get; set; }
        
        public TelegramContact SenderContact { get; set; }
        
        public string ChatName { get; set; }

        public string Sender => SenderContact?.DisplayName;

        public long ChatId { get; set; }
    }
}