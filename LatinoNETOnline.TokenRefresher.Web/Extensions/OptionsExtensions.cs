
using LatinoNETOnline.TokenRefresher.Web.Authentication.Providers.Mixer;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNETOnline.TokenRefresher.Web.Extensions
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MixerOptions>(configuration.GetSection($"Providers:{nameof(MixerOptions)}"));

            return services;
        }
    }
}
