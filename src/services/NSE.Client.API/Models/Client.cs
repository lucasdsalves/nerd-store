using NSE.Core.DomainObjects;

namespace NSE.Client.API.Models
{
    public class Client : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool IsDeleted { get; private set; }
        public Address Address { get; private set; }

        // EF Relation
        protected Client() { }

        public Client(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            IsDeleted = false;
        }

        public void ChangeEmail(string email)
        {
            Email = new Email(email);
        }

        public void SetAddress(Address address)
        {
            Address = address;
        }
    }
}
