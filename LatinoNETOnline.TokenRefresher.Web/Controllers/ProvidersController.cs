
using System.Linq;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Business.Interfaces;
using LatinoNETOnline.TokenRefresher.Web.Models.Enums;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LatinoNETOnline.TokenRefresher.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProvidersController : ControllerBase
    {
        private readonly ITokenBusiness _tokenBusiness;

        public ProvidersController(ITokenBusiness tokenBusiness)
        {
            _tokenBusiness = tokenBusiness;
        }

        [HttpGet]
        public async Task<IActionResult> DoChallenge(Provider provider, string tokenName, string clientId, string clientSecret)
        {
            var latinoClientId = User.Claims.First(x => x.Type == "client_id").Value;

            var response = await _tokenBusiness.ExistTokenByName(tokenName, latinoClientId);
            if (response.Success && response.Result)
            {
                return BadRequest(response.Message);
            }
            else
            {
                AuthenticationProperties authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = "/"
                };



                authenticationProperties.Items.Add(nameof(OAuthOptions.ClientId), clientId);
                authenticationProperties.Items.Add(nameof(OAuthOptions.ClientSecret), clientSecret);
                authenticationProperties.Items.Add(nameof(OAuthOptions.SignInScheme), ((int)provider).ToString());
                authenticationProperties.Items.Add("LatinoNETOnlineClientId", latinoClientId);
                authenticationProperties.Items.Add("TokenName", tokenName);

                return Challenge(authenticationProperties, provider.ToString());
            }
        }
    }
}
