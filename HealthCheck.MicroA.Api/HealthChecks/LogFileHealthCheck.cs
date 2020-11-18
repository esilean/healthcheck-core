using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.MicroA.Api.HealthChecks
{
    public class LogFileHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {

            var basePath = AppDomain.CurrentDomain.BaseDirectory;
            var logFileExists = File.Exists($"{basePath}logs\\nlog-microa.log");

            if (logFileExists)
                return Task.FromResult(HealthCheckResult.Healthy("Log file found"));

            return Task.FromResult(HealthCheckResult.Unhealthy("Cannot find log file 'logs\\nlog-microa.log'"));
        }
    }
}
