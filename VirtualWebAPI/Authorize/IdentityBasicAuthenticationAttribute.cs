using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Claims;

namespace VirtualWebAPI.Authorize
{
    public class IdentityBasicAuthenticationAttribute : BasicAuthenticationAttribute
    {
        protected override async Task<IPrincipal> AuthenticateAsync(string username, string password, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            //code for checking from db

            Claim nameClaim = new Claim(ClaimTypes.Name, username);
            List<Claim> claims = new List<Claim> { nameClaim };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Basic");

            IPrincipal principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
