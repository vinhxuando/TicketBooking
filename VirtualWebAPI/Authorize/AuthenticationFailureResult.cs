using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace VirtualWebAPI.Authorize
{
    internal class AuthenticationFailureResult : IHttpActionResult
    {
        private HttpRequestMessage request;
        private string errorMessage;

        public AuthenticationFailureResult(string errorMessage, HttpRequestMessage request)
        {
            this.errorMessage = errorMessage;
            this.request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(Execute());
        }

        private HttpResponseMessage Execute()
        {
            HttpResponseMessage response = new HttpResponseMessage(System.Net.HttpStatusCode.Unauthorized);
            response.RequestMessage = request;
            response.ReasonPhrase = errorMessage;
            return response;
        }
    }
}