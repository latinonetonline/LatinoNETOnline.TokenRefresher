using System;
using System.Data;
using System.Threading.Tasks;

using Dapper;
using Dapper.Contrib.Extensions;

using LatinoNETOnline.TokenRefresher.Web.Entities;
using LatinoNETOnline.TokenRefresher.Web.Models;
using LatinoNETOnline.TokenRefresher.Web.Services.Interfaces;

using Microsoft.Extensions.Configuration;

using Npgsql;

namespace LatinoNETOnline.TokenRefresher.Web.Services
{
    public class TokenService : ITokenService
    {
        private readonly IDbConnection _db;
        public TokenService(IConfiguration configuration)
        {
            _db = new NpgsqlConnection(configuration.GetConnectionString("Default"));
            SqlMapperExtensions.TableNameMapper = (type) => $"\"{type.Name}s\"";
        }

        public async Task<Response<Token>> Create(Token token)
        {
            await _db.InsertAsync(token);

            return new Response<Token>(token);
        }

        public async Task<Response<Token>> Update(Token token)
        {
            await _db.UpdateAsync(token);

            return new Response<Token>(token);
        }

        public async Task<Response> Delete(Guid id)
        {
            Token token = new Token
            {
                Id = id
            };

            bool wasDeleted = await _db.DeleteAsync(token);

            return new Response() { Success = wasDeleted };
        }

        public async Task<ResponseEnumerable<Token>> GetAll()
        {
            var tokens = await _db.GetAllAsync<Token>();

            return new ResponseEnumerable<Token>(tokens);
        }

        public async Task<Response<Token>> Get(Guid id)
        {
            var token = await _db.QuerySingleOrDefaultAsync<Token>($"SELECT * FROM \"public\".\"Tokens\" WHERE \"{nameof(Token.Id)}\" = '{id}'");
            return new Response<Token>(token);
        }

        public async Task<Response<Token>> Get(string name, string clientId)
        {
            var token = await _db.QuerySingleOrDefaultAsync<Token>($"SELECT * FROM \"public\".\"Tokens\" WHERE Lower(\"{nameof(Token.Name)}\") = '{name.ToLower()}' AND \"{nameof(Token.ClientId)}\" = '{clientId}'");
            return new Response<Token>(token);
        }

        public async Task<Response<Token>> GetByNameOrToken(string name, string tokenValue, string clientId)
        {
            var token = await _db.QuerySingleOrDefaultAsync<Token>($"SELECT * FROM \"public\".\"Tokens\" WHERE ( \"{nameof(Token.Value)}\" = '{tokenValue}' OR Lower(\"{nameof(Token.Name)}\") = '{name}' ) AND \"{nameof(Token.ClientId)}\" = '{clientId}'");
            return new Response<Token>(token);
        }

        public async Task<ResponseEnumerable<Token>> GetExpireSoon(int seconds)
        {
            var tokens = await _db.QueryAsync<Token>($"SELECT * FROM \"public\".\"Tokens\" WHERE NOW() + {seconds} * interval '1 second' >= \"Expires\" ");

            return new ResponseEnumerable<Token>(tokens);
        }
    }
}
