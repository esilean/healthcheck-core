using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.MicroA.Api.HealthChecks
{
    public class CertificateExpiryHealthCheck : IHealthCheck
    {
        //public CertificateExpiryHealthCheck(KeyVaultOptions options)
        //{

        //}

        //private async Task<X509Certificate2> GetCertificate(string certName)
        //{
        //    var client = new KeyVaultClient(async (authority, resource, scope) =>
        //    {
        //        var authContext = new AuthenticationContext(authority);
        //        var clientCred = new ClientCredential("", "");
        //        var result = await authContext.AcquireTokenAsync(resource, clientCred);

        //        if (result == null)
        //            throw new InvalidOperationException("Failed to obtain the JWT token");

        //        return result.AccessToken;
        //    });

        //    var certificateSecret = await client.GetSecretAsync($"https://{"YOUR_VAULT_NAME"}.vault.azure.net/", "YOUR_VAULT_CERTIFICATE_NAME");
        //    var privateKeyBytes = Convert.FromBase64String(certificateSecret.Value);
        //    var certificate = new X509Certificate2(privateKeyBytes, (string)null);
        //    return certificate;
        //}

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            //var certificate = await GetCertificate("");
            //return
            //    certificate.NotAfter < DateTime.Now.AddDays(-30) ?
            //    HealthCheckResult.Degraded() : HealthCheckResult.Healthy();
            throw new NotImplementedException();
        }

    }
}
