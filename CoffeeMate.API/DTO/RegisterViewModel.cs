using System.ComponentModel.DataAnnotations;

namespace CoffeeMate.API.DTO
{
    public class RegisterViewModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage="You must specify password between 4 and 8 characters.")]
        public string Password { get; set; }
    }

    public class LoginViewModel 
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}