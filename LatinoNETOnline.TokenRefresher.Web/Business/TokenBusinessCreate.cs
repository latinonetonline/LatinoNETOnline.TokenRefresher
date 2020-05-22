using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;
using LatinoNETOnline.TokenRefresher.Web.Services.Interfaces;

namespace LatinoNETOnline.TokenRefresher.Web.Business
{
    public class TokenBusinessCreate : ITokenBusinessCreate
    {
        private readonly ITokenService _tokenService;

        public TokenBusinessCreate(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        public async Task<Response<Token>> Create(Token token)
        {
            var responseToken = await _tokenService.GetByNameOrToken(token.Name, token.Value, token.ClientId);

            if (responseToken?.Result is null)
            {
                return await _tokenService.Create(token);
            }
            else
            {
                Response<Token> response = new Response<Token>
                {
                    Message = $"Ya existe un Token con el nombre '{token.Name}' o ya existe un Token con ese token."
                };
                return response;
            }
        }
    }
}
