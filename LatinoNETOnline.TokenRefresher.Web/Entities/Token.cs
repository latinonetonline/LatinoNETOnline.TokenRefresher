using System;

using Dapper.Contrib.Extensions;

namespace LatinoNETOnline.TokenRefresher.Web.Entities
{
    [Table("Tokens")]
    public class Token
    {
        [ExplicitKey]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public DateTime Expires { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
    }
}
