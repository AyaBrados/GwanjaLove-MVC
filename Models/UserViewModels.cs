using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GwanjaLoveProto.Models
{
    public class LoginModel
    {
        [Required]
        [UIHint("email")]
        public required string Email { get; set; }

        [Required]
        [UIHint("password")]
        public required string Password { get; set; }

        public string ReturnUrl { get; set; } = "/";
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User name")]
        public required string UserName { get; set; }

        [Required]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //Indicate that password options specified in Program.cs must be used.
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
        public required IdentityRole Role { get; set; }
    }
}
