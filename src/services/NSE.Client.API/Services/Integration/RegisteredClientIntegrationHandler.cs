using EasyNetQ;
using FluentValidation.Results;
using Microsoft.OpenApi.Writers;
using NSE.Client.API.Application.Commands;
using NSE.Core.Mediator;
using NSE.Core.Messages.Integration;

namespace NSE.Client.API.Services.Integration
{
    public class RegisteredClientIntegrationHandler : BackgroundService
    {
        private IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RegisteredClientIntegrationHandler(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _bus = RabbitHutch.CreateBus("host=localhost:5672"); //RabbitMQ port

            _bus.Rpc.RespondAsync<RegisteredUserIntegrationEvent, ResponseMessage>(async request => new ResponseMessage(await RegisterClient(request)));

            return Task.CompletedTask;
        }

        private async Task<ValidationResult> RegisterClient(RegisteredUserIntegrationEvent message)
        {
            var clientCommand = new RegisterClientCommand(message.Id, message.Name, message.Email, message.Cpf);

            ValidationResult success;

            // HostedService is Singleton. Mediator is Scoped.
            // This is a service rotator. Doing it to solve different lifescycles communicating.
            using (var scope = _serviceProvider.CreateScope())
            {
                var mediator = scope.ServiceProvider.GetRequiredService<IMediatorHandler>();

                success = await mediator.SendCommand(clientCommand);
            }

            return success;
        }
    }
}
