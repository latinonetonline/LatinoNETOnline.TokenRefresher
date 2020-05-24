using System.Text.Encodings.Web;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Entities;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication
{
    public class DynamicOAuthHandler : OAuthHandler<OAuthOptions>
    {
        public DynamicOAuthHandler(IOptionsMonitor<OAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        { }


        protected override async Task<OAuthTokenResponse> ExchangeCodeAsync(OAuthCodeExchangeContext context)
        {
            Options.ClientId = context.Properties.Items[nameof(Token.ProviderClientId)];
            Options.ClientSecret = context.Properties.Items[nameof(Token.ProviderClientSecret)];

            var response = await base.ExchangeCodeAsync(context);

            return response;
        }


        //https://github.com/dotnet/aspnetcore/blob/master/src/Security/Authentication/OAuth/src/OAuthHandler.cs#L263
        protected override string BuildChallengeUrl(AuthenticationProperties properties, string redirectUri)
        {
            Options.ClientId = properties.Items[nameof(Token.ProviderClientId)];
            Options.ClientSecret = properties.Items[nameof(Token.ProviderClientSecret)];

            string url = base.BuildChallengeUrl(properties, redirectUri);
            return url;
        }
    }
}
