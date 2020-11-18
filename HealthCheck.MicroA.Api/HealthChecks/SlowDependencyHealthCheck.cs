using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.MicroA.Api.HealthChecks
{
    public class SlowDependencyHealthCheck : IHealthCheck
    {
        public static readonly string HealthCheckName = "SlowDependency";

        private readonly Task _task;

        public SlowDependencyHealthCheck()
        {
            _task = Task.Delay(10 * 1000);
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            if (_task.IsCompleted)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Dependency is ready"));
            }

            return Task.FromResult(new HealthCheckResult(
                status: context.Registration.FailureStatus,
                description: "Dependency is still initializing"));
        }
    }
}
