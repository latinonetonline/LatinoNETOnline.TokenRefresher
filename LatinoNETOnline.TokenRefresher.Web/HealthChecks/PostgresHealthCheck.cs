using System;
using System.Threading;
using System.Threading.Tasks;

using Dapper;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;

using Npgsql;

namespace LatinoNETOnline.TokenRefresher.Web.HealthChecks
{
    public class PostgresHealthCheck : IHealthCheck
    {
        private readonly IConfiguration _configuration;

        public PostgresHealthCheck(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = new NpgsqlConnection(_configuration.GetConnectionString("Default"));
                await connection.QueryAsync("SELECT 1");

                return HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Exception during check: {ex.GetType().FullName}");
            }
        }
    }
}
