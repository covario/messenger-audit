using System;

namespace Telegram.Governor.Exceptions
{
    public class TelegramCreateGroupException : TelegramException
    {
        public TelegramCreateGroupException()
        {
        }

        public TelegramCreateGroupException(string message) : base(message)
        {
        }

        public TelegramCreateGroupException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}