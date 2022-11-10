using FluentValidation;
using NSE.Core.DomainObjects;
using NSE.Core.Messages;

namespace NSE.Client.API.Application.Commands
{
    public class RegisterClientCommand : Command
    {
        public Guid Id { get; set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public string Cpf { get; set; }

        public RegisterClientCommand(Guid id, string name, string email, string cpf)
        {
            AggregateId = id;
            Id = id;
            Name = name;
            Email = email;
            Cpf = cpf;
        }

        public override bool IsValid()
        {
            ValidationResult = new RegisterClientValidation().Validate(this);

            return ValidationResult.IsValid;
        }
    }

    public class RegisterClientValidation : AbstractValidator<RegisterClientCommand>
    {
        public RegisterClientValidation()
        {
            RuleFor(c => c.Id)
                .NotEqual(Guid.Empty)
                .WithMessage("Client id invalid");

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage("Client name wasn't informed");

            RuleFor(c => c.Cpf)
                .Must(HasValidCpf)
                .WithMessage("This cpf is invalid");

            RuleFor(c => c.Email)
                .Must(HasValidEmail)
                .WithMessage("This email is invalid");
        }

        protected static bool HasValidCpf(string cpf)
        {
            return Cpf.Validate(cpf);
        }

        protected static bool HasValidEmail(string email)
        {
            return Email.Validate(email);
        }
    }
}
