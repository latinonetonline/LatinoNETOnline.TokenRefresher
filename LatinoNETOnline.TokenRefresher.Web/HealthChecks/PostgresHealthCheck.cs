using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

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
                //TODO: There is probably a much better way to do this.
                using (var connection = new NpgsqlConnection(_configuration.GetConnectionString("Default")))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = "SELECT 1";
                        var result = (int)await command.ExecuteScalarAsync().ConfigureAwait(false);
                        if (result == 1)
                        {
                            return HealthCheckResult.Healthy();
                        }

                        return HealthCheckResult.Unhealthy();
                    }
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"Exception during check: {ex.GetType().FullName}");
            }
        }
    }
}
