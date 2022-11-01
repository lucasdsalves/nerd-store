using NSE.WebApp.MVC.Models;
using System.Text;
using System.Text.Json;

namespace NSE.WebApp.MVC.Services
{
    public class AuthService : ErrorsService, IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponseLogin> Login(UserLogin userLogin)
        {
            var loginContent = new StringContent(JsonSerializer.Serialize(userLogin), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7269/login", loginContent);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!DealWithResponseErrors(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };
            }

            return JsonSerializer.Deserialize<UserResponseLogin>(await response.Content.ReadAsStringAsync(), options);
        }

        public async Task<UserResponseLogin> Register(UserRegister userRegister)
        {
            var registerContent = new StringContent(JsonSerializer.Serialize(userRegister), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7269/register", registerContent);

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            if (!DealWithResponseErrors(response))
            {
                return new UserResponseLogin
                {
                    ResponseResult = JsonSerializer.Deserialize<ResponseResult>(await response.Content.ReadAsStringAsync(), options)
                };
            }

            return JsonSerializer.Deserialize<UserResponseLogin>(await response.Content.ReadAsStringAsync(), options);
        }
    }
}
