namespace Telegram.Governor.Models
{
    public class TelegramChat
    {
        public TelegramChat()
        {
            Members = new TelegramContact[] { };
        }

        public long LastMessageId { get; set; }

        public long ChatId { get; set; }
        
        public string Id
        {
            get
            {
                var hex = ChatId.ToString("X");
                if (hex.Length < 16)
                    hex = (new string('0', 16-hex.Length)) + hex;
                return hex;
            }
        }

        public int BasicGroupId { get; set; }

        public string Title { get; set; }

        public TelegramContact[] Members { get; set; }

        public string SupportContact => Members.Length > 0 ? Members[0].DisplayName : string.Empty;
        public string ClientContact => Members.Length > 1 ? Members[1].DisplayName : string.Empty;
    }
}
