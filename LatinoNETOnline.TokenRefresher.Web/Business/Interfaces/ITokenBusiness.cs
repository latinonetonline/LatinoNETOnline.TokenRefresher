using System;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;

namespace LatinoNETOnline.TokenRefresher.Web.Business.Interfaces
{
    public interface ITokenBusiness : ITokenBusinessCreate
    {
        Task<Response> Delete(Guid id, string ClientId);
        Task<ResponseEnumerable<Token>> GetAll();
        Task<Response<Token>> Get(Guid id);
        Task<Response<Token>> Get(string name, string clientId);
        Task<Response<Token>> Update(Token token);
        Task<Response<bool>> ExistTokenByName(string name, string clientId);
        Task<Response<Token>> RefreshToken(Token token);
        Task<Response> RefreshTokens();

    }
}
