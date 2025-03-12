using Polly.Extensions.Http;
using Polly;
using Polly.Contrib.WaitAndRetry;

namespace CodeChallenging.Extensions
{
    public static class PollyExtensions
    {
        //old code without Jitter-Enabled
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

        /// <summary>
        /// enhancing resilience against transient faults.
        /// A regular Retry policy can affect your system in cases of high concurrency 
        /// and scalability and under high contention.
        /// </summary>
        /// <returns></returns>
        public static IAsyncPolicy<HttpResponseMessage> GetJitterRetryPolicy()
        {
            // Define the delay sequence with decorrelated jitter
            var delay = Backoff.DecorrelatedJitterBackoffV2(
                medianFirstRetryDelay: TimeSpan.FromSeconds(1),
                retryCount: 6);

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(delay);
        }
    }
}
