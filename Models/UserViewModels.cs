using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace GwanjaLoveProto.Models
{
    public class LoginModel
    {
        public string Email { get; set; } = "";

        [Required]
        [UIHint("password")]
        public required string Password { get; set; }

        [Display(Name = "User Name")]
        public string UserName { get; set; } = "";

        public string ReturnUrl { get; set; } = "/";
        public bool SignInWithUserName { get; set; } = false;

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;
    }

    public class RegisterModel
    {
        [Required]
        [Display(Name = "User Name")]
        public required string UserName { get; set; }

        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)] //Indicate that password options specified in Program.cs must be used.
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords must match")]
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }

        [Display(Name = "Role")]
        public required string Role { get; set; }

        [DisplayName("Avatar Image")]
        public IFormFile AvatarImage { get; set; }
    }
}
