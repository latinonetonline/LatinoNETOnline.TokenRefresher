using System.Security.Claims;


using LatinoNETOnline.TokenRefresher.Web.Business;
using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Models.Enums;
using LatinoNETOnline.TokenRefresher.Web.Services;
using LatinoNETOnline.TokenRefresher.Web.Services.Interfaces;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication.Providers.Mixer
{
    public static class MixerOAuthExtensions
    {
        public static AuthenticationBuilder AddMixerOAuth(this AuthenticationBuilder builder, IConfiguration configuration)
        {
            ITokenService tokenService = new TokenService(configuration);
            ITokenBusinessCreate tokenBusiness = new TokenBusinessCreate(tokenService);
            var mixerOptions = new MixerOptions();
            configuration.GetSection($"Providers:{nameof(MixerOptions)}").Bind(mixerOptions);

            builder.AddOAuth<OAuthOptions, DynamicOAuthHandler>(Provider.Mixer.ToString(), options =>
            {
                options.ClientId = "N/A";
                options.ClientSecret = "N/A";
                options.CallbackPath = new PathString($"/{Provider.Mixer}LoginCallback");

                options.AuthorizationEndpoint = mixerOptions.AuthorizationEndpoint;
                options.TokenEndpoint = mixerOptions.TokenEndpoint;
                options.UserInformationEndpoint = mixerOptions.UserInformationEndpoint;

                options.Scope.Add("channel:update:self");
                options.Scope.Add("channel:details:self");
                options.Scope.Add("user:details:self");
                options.Scope.Add("user:update:self");
                options.Scope.Add("interactive:manage:self");
                options.Scope.Add("interactive:robot:self");
                options.Scope.Add("user:notification:self");
                options.Scope.Add("user:log:self");

                options.SaveTokens = true;

                options.ClaimActions.MapJsonSubKey(ClaimTypes.NameIdentifier, "channel", "id", "int32");
                options.ClaimActions.MapJsonKey(ClaimTypes.Name, "username", "string");

                options.Events = new DynamicOAuthEvents(tokenBusiness);
            });

            return builder;
        }
    }
}
