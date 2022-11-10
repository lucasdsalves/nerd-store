using FluentValidation.Results;
using MediatR;
using NSE.Client.API.Events;
using NSE.Client.API.Models;
using NSE.Core.Messages;

namespace NSE.Client.API.Application.Commands
{
    public class ClientCommandHandler : CommandHandler, IRequestHandler<RegisterClientCommand, ValidationResult>
    {
        private readonly IClientsRepository _clientsRepository;

        public ClientCommandHandler(IClientsRepository clientsRepository)
        {
            _clientsRepository = clientsRepository;
        }

        public async Task<ValidationResult> Handle(RegisterClientCommand message, CancellationToken cancellationToken)
        {
            if (!message.IsValid()) return message.ValidationResult;

            var client = new Clients(message.Id, message.Name, message.Email, message.Cpf);

            var isExistantClient = await _clientsRepository.GetByCpf(message.Cpf);

            if (isExistantClient != null)
            {
                AddError("This CPF is already in use.");
                return ValidationResult;
            }

            _clientsRepository.Add(client);

            client.AddEvent(new RegisteredClientEvent(message.Id, message.Name, message.Email, message.Cpf));

            return await PersistData(_clientsRepository.UnitOfWork);
        }
    }
}
