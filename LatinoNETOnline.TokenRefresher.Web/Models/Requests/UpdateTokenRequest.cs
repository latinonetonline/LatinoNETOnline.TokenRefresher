using System;

namespace LatinoNETOnline.TokenRefresher.Web.Models.Requests
{
    public class UpdateTokenRequest
    {
        public Guid Id { get; set; }
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}
