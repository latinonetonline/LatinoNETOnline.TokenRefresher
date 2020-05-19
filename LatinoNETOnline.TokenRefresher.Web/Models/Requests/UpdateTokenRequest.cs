using System;
using System.ComponentModel.DataAnnotations;

namespace LatinoNETOnline.TokenRefresher.Web.Models.Requests
{
    public class UpdateTokenRequest
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Token { get; set; }
        public DateTime? Expires { get; set; }
        public string TokenType { get; set; }
        public string RefreshToken { get; set; }
    }
}
