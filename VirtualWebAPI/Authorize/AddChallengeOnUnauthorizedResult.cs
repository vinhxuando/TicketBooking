using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VirtualWebAPI.Authorize
{
    internal class AddChallengeOnUnauthorizedResult : IHttpActionResult
    {
        public AuthenticationHeaderValue challenge { get; private set; }
        public IHttpActionResult result { get; private set; }

        public AddChallengeOnUnauthorizedResult(AuthenticationHeaderValue challenge, IHttpActionResult result)
        {
            this.challenge = challenge;
            this.result = result;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage respone = await result.ExecuteAsync(cancellationToken);

            if (respone.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                if (!respone.Headers.WwwAuthenticate.Any(h => h.Scheme == challenge.Scheme))
                    respone.Headers.WwwAuthenticate.Add(challenge);
            }

            return respone;
        }
    }
}