using Newtonsoft.Json;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace API.DTO
{
    public class LoginDto
    {
        [Required]
        [JsonProperty("email")]
        public string Email { get; set; }
        [Required]
        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("fcm_token")]
        [DefaultValue("")]
        [AllowNull]
        public string? FCMToken { get; set; }
    }
}
