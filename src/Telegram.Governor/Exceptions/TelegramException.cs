using System;

namespace Telegram.Governor.Exceptions
{
    public class TelegramException : ApplicationException
    {
        public TelegramException()
        {
        }

        public TelegramException(string message) : base(message)
        {
        }

        public TelegramException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}