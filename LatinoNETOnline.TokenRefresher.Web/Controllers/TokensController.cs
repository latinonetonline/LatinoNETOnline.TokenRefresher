using System;
using System.Linq;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
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
        private readonly ITokenBusiness _tokenBusiness;

        public TokensController(ITokenService tokenService, ITokenBusiness tokenBusiness)
        {
            _tokenService = tokenService;
            _tokenBusiness = tokenBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _tokenBusiness.GetAll());
        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _tokenBusiness.Get(id);
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
            var response = await _tokenBusiness.Get(name, clientId);
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

            Response<Token> response = await _tokenBusiness.Create(token);

            if (response.Result is null)
            {
                return BadRequest(response);
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateTokenRequest request)
        {
            if (request.Id == Guid.Empty)
            {
                return BadRequest("El Id no puede tener un valor vacio.");
            }

            if (string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest("El Token no puede estar vacio.");
            }

            if (string.IsNullOrWhiteSpace(request.RefreshToken))
            {
                return BadRequest("El RefreshToken no puede estar vacio.");
            }

            if (!request.Expires.HasValue)
            {
                return BadRequest("Debe ingresar una fecha de expiración.");
            }

            if (string.IsNullOrWhiteSpace(request.TokenType))
            {
                return BadRequest("El TokenType no puede estar vacio.");
            }

            var responseToken = await _tokenService.Get(request.Id);

            Token token = responseToken.Result;

            token.Value = request.Token;
            token.RefreshToken = request.RefreshToken;
            token.Expires = request.Expires.Value;
            token.TokenType = request.TokenType;

            return Ok(await _tokenService.Update(token));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            string clientId = User.Claims.First(c => c.Type == "client_id").Value;

            Response response = await _tokenBusiness.Delete(id, clientId);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response);
            }

        }

        [HttpPost("[action]/{name}")]
        public async Task<IActionResult> RefreshToken(string name)
        {
            string clientId = User.Claims.First(c => c.Type == "client_id").Value;
            var response = await _tokenBusiness.Get(name, clientId);
            if (response.Result is null)
            {
                return NotFound();
            }
            else
            {
                return Ok(await _tokenBusiness.RefreshToken(response.Result));
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> RefreshToken()
        {
            return Ok(await _tokenBusiness.RefreshTokens());
        }
    }
}
