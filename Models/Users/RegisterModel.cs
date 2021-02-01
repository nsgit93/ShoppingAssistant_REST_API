using System.ComponentModel.DataAnnotations;

namespace ShoppingAssistantServer.Models.Users
{
    public class RegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Type { get; set; }
    }
}