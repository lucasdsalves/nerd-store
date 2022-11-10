using NSE.WebApp.MVC.Extensions;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NSE.WebApp.MVC.Models
{
    public class UserRegister
    {
        [Required(ErrorMessage = "This field {0} is required")]
        [DisplayName("Fullname")]
        public string Fullname { get; set; }

        [Required(ErrorMessage = "This field {0} is required")]
        [DisplayName("CPF")]
        [Cpf]
        public string Cpf { get; set; }

        [Required(ErrorMessage = "This field {0} is required")]
        [EmailAddress(ErrorMessage = "This field {0} has an invalid format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field {0} is required")]
        [StringLength(100, ErrorMessage = "This field {0} need to have between {2} and {1} caracters", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords didn't match")]
        public string PasswordConfirmation { get; set; }
    }

    public class UserLogin
    {
        [Required(ErrorMessage = "This field {0} is required")]
        [EmailAddress(ErrorMessage = "This field {0} has an invalid format")]
        public string Email { get; set; }

        [Required(ErrorMessage = "This field {0} is required")]
        [StringLength(100, ErrorMessage = "This field {0} need to have between {2} and {1} caracters", MinimumLength = 6)]
        public string Password { get; set; }
    }
    public class UserResponseLogin
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public UserToken UserToken { get; set; }
        public ResponseResult ResponseResult { get; set; }
    }

    public class UserToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> Claims { get; set; }
    }

    public class UserClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }
}
