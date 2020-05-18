
using FluentMigrator.Runner;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNETOnline.TokenRefresher.Web.Extensions
{
    public static class FluentMigrationExtensions
    {
        public static IServiceCollection AddFluentMigrator(this IServiceCollection services, IConfiguration configuration)
        {
            return services.AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    // Add SQLite support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(configuration.GetConnectionString("Default"))
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Startup).Assembly).For.Migrations())
                // Enable logging to console in the FluentMigrator way
                .AddLogging(lb => lb.AddFluentMigratorConsole());
        }
    }
}
