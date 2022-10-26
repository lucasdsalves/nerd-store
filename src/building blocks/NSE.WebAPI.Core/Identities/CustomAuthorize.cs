using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace NSE.WebAPI.Core.Identities
{
    public class CustomAuthorization
    {
        public static bool ValidateUserClaims(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(c => c.Type == claimName && c.Value.Contains(claimValue));
        }

        public class ClaimsAuthorizeAttribute : TypeFilterAttribute
        {
            public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(RequisitClaimFilter))
            {
                Arguments = new object[] { new Claim(claimName, claimValue) };
            }
        }

        public class RequisitClaimFilter : IAuthorizationFilter
        {
            private readonly Claim _claim;

            public RequisitClaimFilter(Claim claim)
            {
                _claim = claim;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    context.Result = new StatusCodeResult(401);
                    return;
                }

                if (!CustomAuthorization.ValidateUserClaims(context.HttpContext, _claim.Type, _claim.Value))
                {
                    context.Result = new StatusCodeResult(403);
                }
            }
        }
    }
}
