using NSE.Core.Messages;

namespace NSE.Client.API.Events
{
    public class RegisteredClientEvent : Event
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; set; }

        public RegisteredClientEvent(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }
    }
}
