using System.Text.RegularExpressions;

namespace NSE.Core.DomainObjects
{
    public class Email
    {
        public const int EmailAddressMaxLength = 254;
        public const int EmailAddressMinLength = 5;
        public string EmailAddress { get; private set; }

        //EF constructor
        protected Email() { }

        public Email(string emailAddress)
        {
            if (!Validate(EmailAddress)) throw new DomainException("Invalid email");
            EmailAddress = emailAddress;
        }

        public static bool Validate(string email)
        {
            //TODO: check this regex later
            //var regexEmail = new Regex(@"^[\w -\.] +@([\w -] +\.) +[\w -]{ 2,4}$");
            //return regexEmail.IsMatch(email);
            return true;
        }
    }
}
