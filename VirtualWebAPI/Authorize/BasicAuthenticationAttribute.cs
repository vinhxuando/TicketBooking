using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Filters;

namespace VirtualWebAPI.Authorize
{
    public abstract class BasicAuthenticationAttribute : Attribute, IAuthenticationFilter
    {
        public string Realm { get; set; }

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }

        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpRequestMessage request = context.Request;
            AuthenticationHeaderValue authorization = request.Headers.Authorization;

            if (authorization == null) return;
            if (authorization.Scheme != "Basic") return;

            if (String.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationFailureResult("Missing credentials", request);
                return;
            }

            Tuple<string, string> usernameAndPassword = ExtractUsernameAndPassword(authorization.Parameter);

            if(usernameAndPassword== null)
            {
                context.ErrorResult = new AuthenticationFailureResult("Invalid Credentials", request);
                return;
            }

            string username = usernameAndPassword.Item1;
            string password = usernameAndPassword.Item2;

            IPrincipal principal = await AuthenticateAsync(username, password, cancellationToken);

            if (principal == null)
                context.ErrorResult = new AuthenticationFailureResult("Invalid username or password", request);
            else context.Principal = principal;
        }

        protected abstract Task<IPrincipal> AuthenticateAsync(string username, string password, CancellationToken cancellationToken);

        private static Tuple<string, string> ExtractUsernameAndPassword(string authorizationParameter)
        {
            byte[] credentialBytes;

            try
            {
                credentialBytes = Convert.FromBase64String(authorizationParameter);
            }
            catch (FormatException)
            {
                return null;
            }

            Encoding encoding = Encoding.ASCII;
            encoding = (Encoding)encoding.Clone();
            encoding.DecoderFallback = DecoderFallback.ExceptionFallback;

            string decodedCredentials;

            try
            {
                decodedCredentials = encoding.GetString(credentialBytes);
            }
            catch (DecoderFallbackException)
            {
                return null;
            }

            if (String.IsNullOrEmpty(decodedCredentials)) return null;

            int colonIndex = decodedCredentials.IndexOf(":");

            if (colonIndex == -1) return null;

            string username = decodedCredentials.Substring(0, colonIndex);
            string password = decodedCredentials.Substring(colonIndex + 1);

            return new Tuple<string, string>(username, password);
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            Challenge(context);
            return Task.FromResult(0);
        }

        private void Challenge(HttpAuthenticationChallengeContext context)
        {
            string parameter;

            if (String.IsNullOrEmpty(Realm)) parameter = null;
            else
                parameter = "realm=\"" + Realm + "\"";

            context.ChallengeWith("Basic", parameter);
        }
    }
}