using Polly.Extensions.Http;
using Polly;

namespace CodeChallenging.Extensions
{
    public static class PollyExtensions
    {
        public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                // Handles transient failures like HTTP 5xx errors or network failures
                .HandleTransientHttpError()
                // Also handle NotFound (HTTP 404) as a transient error in this policy
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                // Retries 6 times with an exponential back-off (2^retryAttempt seconds)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
