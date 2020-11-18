﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.MicroA.Api.HealthChecks
{
    public class RandomHealthCheck : IHealthCheck
    {
        private readonly IRandomHealthCheckResultGenerator generator;

        public RandomHealthCheck(IRandomHealthCheckResultGenerator generator)
        {
            this.generator = generator;
        }

        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(generator.GenerateRandomResult());
        }

    }

    public interface IRandomHealthCheckResultGenerator
    {
        HealthCheckResult GenerateRandomResult();
    }

    public class TimeBasedRandomHealthCheckResultGenerator : IRandomHealthCheckResultGenerator
    {
        public HealthCheckResult GenerateRandomResult()
        {
            if (DateTime.UtcNow.Minute % 2 == 0)
            {
                return HealthCheckResult.Healthy();
            }

            return HealthCheckResult.Unhealthy(description: "failed");
        }
    }

    public class TotallyRandomHealthCheckResultGenerator : IRandomHealthCheckResultGenerator
    {
        private readonly Random random;

        public TotallyRandomHealthCheckResultGenerator()
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            byte[] byteArray = new byte[4];
            provider.GetBytes(byteArray);
            int seed = BitConverter.ToInt32(byteArray, 0);
            random = new Random(seed);
        }

        public HealthCheckResult GenerateRandomResult()
        {
            int value = random.Next(100);
            switch (value)
            {
                case int n when (n >= 80):
                    return HealthCheckResult.Unhealthy();
                case int n when (n >= 50):
                    return HealthCheckResult.Degraded();
                default:
                    return HealthCheckResult.Healthy();
            }
        }
    }
}
