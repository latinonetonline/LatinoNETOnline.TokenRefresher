
using LatinoNETOnline.TokenRefresher.Web.Authentication.Providers.Mixer;
using LatinoNETOnline.TokenRefresher.Web.Authentication.RefreshTokenHandlers;

using Microsoft.Extensions.DependencyInjection;

namespace LatinoNETOnline.TokenRefresher.Web.Extensions
{
    public static class RefreshTokenHandlerExtensions
    {
        public static IServiceCollection AddRefreshTokenHandlers(this IServiceCollection services)
        {
            services.AddTransient<IRefreshTokenHandler<MixerOptions>, MixerRefreshTokenHandler>();

            return services;
        }
    }
}
