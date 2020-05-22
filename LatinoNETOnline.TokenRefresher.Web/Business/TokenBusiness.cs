using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Authentication.Providers.Mixer;
using LatinoNETOnline.TokenRefresher.Web.Authentication.RefreshTokenHandlers;
using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;
using LatinoNETOnline.TokenRefresher.Web.Models.Enums;
using LatinoNETOnline.TokenRefresher.Web.Services.Interfaces;

namespace LatinoNETOnline.TokenRefresher.Web.Business
{
    public class TokenBusiness : TokenBusinessCreate, ITokenBusiness
    {
        private readonly ITokenService _tokenService;
        private readonly IServiceProvider _serviceProvider;

        public TokenBusiness(ITokenService tokenService, IServiceProvider serviceProvider) : base(tokenService)
        {
            _tokenService = tokenService;
            _serviceProvider = serviceProvider;
        }

        public async Task<Response> Delete(Guid id, string ClientId)
        {
            var responseToken = await _tokenService.Get(id);
            if (responseToken.Result is null || responseToken.Result.ClientId != ClientId)
            {
                return new Response
                {
                    Message = "No existe o no tiene permisos para alterar este Token."
                };
            }
            else
                return await _tokenService.Delete(id);
        }

        public async Task<Response<bool>> ExistTokenByName(string name, string clientId)
        {
            var responseToken = await _tokenService.Get(name, clientId);

            if (responseToken?.Result is null)
            {
                return new Response<bool>
                {
                    Result = false,
                    Success = true
                };
            }
            else
            {
                return new Response<bool>
                {
                    Result = true,
                    Success = true,
                    Message = $"Ya existe un Token con el nombre '{name}'."
                };
            }
        }

        public async Task<Response<Token>> Get(Guid id)
        {
            return await _tokenService.Get(id);
        }

        public async Task<Response<Token>> Get(string name, string clientId)
        {
            return await _tokenService.Get(name, clientId);
        }

        public async Task<ResponseEnumerable<Token>> GetAll()
        {
            return await _tokenService.GetAll();
        }

        public Task<Response<Token>> RefreshToken(Token token)
        {
            Type optionsType = token.Provider switch
            {
                Provider.Mixer => typeof(MixerOptions),
                _ => throw new InvalidEnumArgumentException(nameof(token.Provider), (int)token.Provider, typeof(Provider))
            };

            var genericIRefreshTokenHandlerType = typeof(IRefreshTokenHandler<>);
            var specificIRefreshTokenHandlerType = genericIRefreshTokenHandlerType.MakeGenericType(optionsType);

            var service = (IRefreshTokenHandler)_serviceProvider.GetService(specificIRefreshTokenHandlerType);
            return service.Handle(token);
        }

        public async Task<Response> RefreshTokens()
        {
            var tokens = await _tokenService.GetExpireSoon(2400);

            var tasks = new List<Task<Response<Token>>>();

            foreach (var token in tokens.Result)
            {
                tasks.Add(RefreshToken(token));
            }

            Task.WaitAll(tasks.ToArray());

            return new Response
            {
                Success = true
            };
        }

        public async Task<Response<Token>> Update(Token token)
        {
            return await _tokenService.Update(token);
        }
    }
}
