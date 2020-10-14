namespace Telegram.Governor.Models
{
    public class TelegramConfiguration
    {
        public const string SettingKey = "Telegram";

        public int ApiId { get; set; }
        public string ApiHash { get; set; }
        public string PhoneNumber { get; set; }
        public string EncryptionKey { get; set; }
        public string Data { get; set; }
    }
}
