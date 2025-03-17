using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CodeChallenging.CustomHealthChecks
{
    public class CustomHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            // Example logic to check if a service is alive
            bool isHealthy = true;

            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Service is running."));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Service is down."));
        }
    }
}
