using System;
using System.ComponentModel.DataAnnotations;

namespace LatinoNETOnline.TokenRefresher.Web.Models.Requests
{
    public class CreateTokenRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}
