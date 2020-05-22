using System;

using Dapper.Contrib.Extensions;

using LatinoNETOnline.TokenRefresher.Web.Models.Enums;

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
        public Provider Provider { get; set; }
        public string ProviderClientId { get; set; }
        public string ProviderClientSecret { get; set; }
    }
}
