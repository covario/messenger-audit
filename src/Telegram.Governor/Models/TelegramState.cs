namespace Telegram.Governor.Models
{
    public enum TelegramSessionState
    {
        Uninitialized,
        Initializing,
        Initialized,
        SettingEncryptionKey,
        Connected,
        InError,
        Unauthorized,
        PendingServiceAccount,
        PendingCode,
        PendingPassword,
        Disconnected
    }
}