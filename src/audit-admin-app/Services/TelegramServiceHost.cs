using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Covario.AuditAdminApp.Services
{
    public class TelegramServiceHost : IHostedService
    {
        private readonly ITelegramService _telegramService;

        public TelegramServiceHost(ITelegramService telegramService)
        {
            _telegramService = telegramService;
        }

        #region IHostedService

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _telegramService.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        #endregion
    }
}