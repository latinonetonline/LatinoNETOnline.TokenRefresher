using System.Threading.Tasks;

using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;

namespace LatinoNETOnline.TokenRefresher.Web.Business.Interfaces
{
    public interface ITokenBusinessCreate
    {
        Task<Response<Token>> Create(Token token);
    }
}
