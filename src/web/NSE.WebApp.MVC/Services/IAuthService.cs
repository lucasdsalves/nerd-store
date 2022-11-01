using NSE.WebApp.MVC.Models;

namespace NSE.WebApp.MVC.Services
{
    public interface IAuthService
    {
        Task<UserResponseLogin> Login(UserLogin userLogin);
        Task<UserResponseLogin> Register(UserRegister userRegister);
    }
}
