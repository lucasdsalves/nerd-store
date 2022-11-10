using MediatR;

namespace NSE.Client.API.Events
{
    public class ClientEventHandler : INotificationHandler<RegisteredClientEvent>
    {
        public Task Handle(RegisteredClientEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
