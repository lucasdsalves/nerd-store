using System.Security.Claims;

namespace NSE.WebApp.MVC.Extensions
{
    public interface IUser
    {
        string UserName { get; }
        Guid GetUserId();
        string GetUserEmail();
        string GetUserToken();
        bool IsUserAuthenticated();
        bool HasRoles(string role);
        IEnumerable<Claim> GetClaims();
        HttpContext GetHttpContext();
    }

    public class AspNetUser : IUser
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public AspNetUser(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public string UserName => _contextAccessor.HttpContext.User.Identity.Name;

        public IEnumerable<Claim> GetClaims()
        {
            return _contextAccessor.HttpContext.User.Claims;
        }

        public HttpContext GetHttpContext()
        {
            return _contextAccessor.HttpContext;
        }

        public string GetUserEmail()
        {
            return IsUserAuthenticated() ? _contextAccessor.HttpContext.User.GetUserEmail() : "";
        }

        public Guid GetUserId()
        {
            return IsUserAuthenticated() ? Guid.Parse(_contextAccessor.HttpContext.User.GetUserId()) : Guid.Empty;
        }

        public string GetUserToken()
        {
            return IsUserAuthenticated() ? _contextAccessor.HttpContext.User.GetUserToken() : "";
        }

        public bool HasRoles(string role)
        {
            return _contextAccessor.HttpContext.User.IsInRole(role);
        }

        public bool IsUserAuthenticated()
        {
            return _contextAccessor.HttpContext.User.Identity.IsAuthenticated;
        }
    }

    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("sub");

            return claim?.Value;
        }

        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("email");

            return claim?.Value;
        }

        public static string GetUserToken(this ClaimsPrincipal principal)
        {
            if (principal == null)
            {
                throw new ArgumentException(nameof(principal));
            }

            var claim = principal.FindFirst("JWT");

            return claim?.Value;
        }
    }
}
