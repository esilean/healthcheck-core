using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.MicroA.Api.HealthChecks
{
    public class TripwireHealthCheck : IHealthCheck
    {
        private static int trippedCount = 0;

        public int Trip()
        {
            return Interlocked.Increment(ref trippedCount);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return (trippedCount % 3) switch
            {
                2 => Task.FromResult(HealthCheckResult.Unhealthy("Boom!")),
                1 => Task.FromResult(HealthCheckResult.Degraded("About to explode")),
                _ => Task.FromResult(HealthCheckResult.Healthy("Still doing okay")),
            };
            ;
        }
    }
}
