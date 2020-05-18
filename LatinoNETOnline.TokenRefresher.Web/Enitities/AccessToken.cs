using System;

namespace LatinoNETOnline.TokenRefresher.Web.Enitities
{
    public class AccessToken
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Token { get; set; }
        public DateTime Expires { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}
