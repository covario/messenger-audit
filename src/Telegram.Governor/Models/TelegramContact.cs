namespace Telegram.Governor.Models
{
    public class TelegramContact
    {
        public TelegramContact(int userId, string phoneNumber)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
        }

        public TelegramContact()
        {
        }

        public int UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string DisplayName => ! string.IsNullOrEmpty(Username) ? "@" + Username : !string.IsNullOrEmpty(PhoneNumber) ? PhoneNumber : FirstName + " " + LastName;
    }
}