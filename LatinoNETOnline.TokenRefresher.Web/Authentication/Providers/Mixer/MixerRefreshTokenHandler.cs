using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Authentication.RefreshTokenHandlers;
using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Json;
using LatinoNETOnline.TokenRefresher.Web.Models;

using Microsoft.Extensions.Options;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication.Providers.Mixer
{
    public class MixerRefreshTokenHandler : IRefreshTokenHandler<MixerOptions>
    {
        private readonly HttpClient _httpClient;
        private readonly ITokenBusiness _tokenBusiness;
        public MixerOptions Options { get; set; }

        public MixerRefreshTokenHandler(IHttpClientFactory httpClientFactory, ITokenBusiness tokenBusiness, IOptions<MixerOptions> options)
        {
            _httpClient = httpClientFactory.CreateClient();
            _tokenBusiness = tokenBusiness;
            Options = options.Value;
        }


        public async Task<Response<Token>> Handle(Token token)
        {
            MixerRefreshToken mixerRefreshToken = new MixerRefreshToken
            {
                Grant_Type = "refresh_token",
                Client_Id = token.ProviderClientId,
                Client_Secret = token.ProviderClientSecret,
                Refresh_Token = token.RefreshToken
            };

            JsonSerializerOptions jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCaseNamingPolicy(),
                WriteIndented = true
            };

            var httpResponse = await _httpClient.PostAsJsonAsync(Options.TokenEndpoint, mixerRefreshToken, jsonOptions);

            httpResponse.EnsureSuccessStatusCode();

            var mixerToken = await httpResponse.Content.ReadFromJsonAsync<MixerToken>();

            token.Expires = DateTime.Now.AddSeconds(mixerToken.Expires_In);
            token.Value = mixerToken.Access_Token;
            token.RefreshToken = mixerToken.Refresh_Token;
            token.TokenType = mixerToken.Token_Type;

            return await _tokenBusiness.Update(token);
        }
    }
}
