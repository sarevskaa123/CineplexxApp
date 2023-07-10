﻿using Cineplexx.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Cineplexx.Services
{
    public class ConsumeScopedHostedService : IHostedService
    {
        private readonly IServiceProvider _service;
        public ConsumeScopedHostedService(IServiceProvider service) {
            _service = service;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await SendEmail();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
           return Task.CompletedTask;
        }

        private async Task SendEmail()
        {
            using(var scope=_service.CreateScope())
            {
                var scopedProcessingService = scope.ServiceProvider.GetRequiredService<IEmailSender>();
                await scopedProcessingService.SendEmail();
            }
        }
    }
}
