using System;
using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;

namespace LatinoNETOnline.TokenRefresher.Web.Services.Interfaces
{
    public interface ITokenService
    {
        Task<Response<Token>> Create(Token token);
        Task<Response> Delete(Guid id);
        Task<ResponseEnumerable<Token>> GetAll();
        Task<Response<Token>> Get(Guid id);
        Task<Response<Token>> Get(string name, string clientId);
        Task<Response<Token>> Update(Token token);
    }
}