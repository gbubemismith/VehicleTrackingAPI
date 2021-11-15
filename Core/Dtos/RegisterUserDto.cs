using System.ComponentModel.DataAnnotations;

namespace Core.Dtos
{
    public class RegisterUserDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
}