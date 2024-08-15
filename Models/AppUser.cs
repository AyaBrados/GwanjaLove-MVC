using Microsoft.AspNetCore.Identity;

namespace GwanjaLoveProto.Models
{
    public class AppUser : IdentityUser
    {
        public byte[]? AvatarImage { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
