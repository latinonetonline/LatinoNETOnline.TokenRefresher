using System;
using System.Linq;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;
using LatinoNETOnline.TokenRefresher.Web.Models.Requests;
using LatinoNETOnline.TokenRefresher.Web.Services.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinoNETOnline.TokenRefresher.Web.Controllers
{
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public TokensController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tokenService.GetAll());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _tokenService.Get(id);
            if (response.Result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("[action]/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            string clientId = User.Claims.First(c => c.Type == "client_id").Value;
            var response = await _tokenService.Get(name, clientId);
            if (response.Result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTokenRequest request)
        {
            string clientId = User.Claims.First(c => c.Type == "client_id").Value;
            var responseToken = await _tokenService.Get(request.Name, clientId);
            if (responseToken?.Result is null)
            {
                Token token = new Token
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Value = request.Token,
                    Expires = request.Expires.GetValueOrDefault(),
                    RefreshToken = request.RefreshToken,
                    TokenType = request.TokenType,
                    ClientId = clientId
                };

                return Ok(await _tokenService.Create(token));
            }
            else
            {
                Response response = new Response
                {
                    Message = $"Ya existe un token con el nombre '{request.Name}'"
                };
                return BadRequest(response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTokenRequest request)
        {
            var responseToken = await _tokenService.Get(request.Id);

            Token token = responseToken.Result;

            token.Value = string.IsNullOrWhiteSpace(request.Token) ? token.Value : request.Token;
            token.RefreshToken = string.IsNullOrWhiteSpace(request.RefreshToken) ? token.RefreshToken : request.RefreshToken;
            token.Expires = request.Expires.HasValue ? token.Expires : request.Expires.Value;
            token.TokenType = string.IsNullOrWhiteSpace(request.TokenType) ? token.TokenType : request.TokenType;

            return Ok(await _tokenService.Update(token));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var responseToken = await _tokenService.Get(id);
            if (responseToken.Result.ClientId == User.Claims.First(c => c.Type == "client_id").Value)
            {
                return Ok(await _tokenService.Delete(id));
            }
            else
            {
                return Forbid();
            }
        }
    }
}
