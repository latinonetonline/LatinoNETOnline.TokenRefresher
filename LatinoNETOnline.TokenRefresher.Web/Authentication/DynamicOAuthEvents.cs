using System;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Authentication.Exceptions;
using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;
using LatinoNETOnline.TokenRefresher.Web.Models.Enums;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication
{
    public class DynamicOAuthEvents : OAuthEvents
    {
        private readonly ITokenBusinessCreate _tokenBusiness;

        public DynamicOAuthEvents(ITokenBusinessCreate tokenBusiness)
        {
            _tokenBusiness = tokenBusiness;
        }

        public override async Task CreatingTicket(OAuthCreatingTicketContext context)
        {
            var latinoClinetId = context.Properties.Items[nameof(Token.ClientId)];
            var tokenName = context.Properties.Items[nameof(Token) + nameof(Token.Name)];
            var providerClientId = context.Properties.Items[nameof(Token.ProviderClientId)];
            var providerSecret = context.Properties.Items[nameof(Token.ProviderClientSecret)];

            var provider = (Provider)int.Parse(context.Properties.Items[nameof(Token.Provider)]);
            Response<Token> response = await _tokenBusiness.Create(new Token
            {
                Expires = DateTime.Now.ToUniversalTime().AddSeconds(context.ExpiresIn.Value.TotalSeconds),
                Value = context.AccessToken,
                RefreshToken = context.RefreshToken,
                TokenType = context.TokenType,
                ClientId = latinoClinetId,
                Provider = provider,
                Id = Guid.NewGuid(),
                Name = tokenName,
                ProviderClientId = providerClientId,
                ProviderClientSecret = providerSecret
            });

            if (response.Success)
            {
                throw new ResponseSuccessException();
            }
            else
            {
                throw new ResponseFailedException(response);
            }
        }
        public override Task RemoteFailure(RemoteFailureContext context)
        {
            context.HandleResponse();
            if (context.Failure is ResponseSuccessException)
            {
                context.Response.Redirect("/Responses/Success?message=El Token se guardo de manera exitosa!!");
            }
            else if (context.Failure is ResponseFailedException)
            {
                ResponseFailedException responseFailedException = context.Failure as ResponseFailedException;
                context.Response.Redirect($"/Responses/Error?message={responseFailedException.Message}");
            }
            else
            {
                context.Response.Redirect($"/Responses/Error?message=Hubo un error interno: {context.Failure.Message}");
            }

            return Task.FromResult(0);
        }
    }
}
