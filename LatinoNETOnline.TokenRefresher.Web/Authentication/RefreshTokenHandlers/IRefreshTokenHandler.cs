using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;

namespace LatinoNETOnline.TokenRefresher.Web.Authentication.RefreshTokenHandlers
{
    public interface IRefreshTokenHandler<TOptions> : IRefreshTokenHandler where TOptions : class, new()
    {
        TOptions Options { get; set; }
    }

    public interface IRefreshTokenHandler
    {
        Task<Response<Token>> Handle(Token token);
    }
}
