using System.ComponentModel.DataAnnotations;

namespace LoginAPI.DTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}