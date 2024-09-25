using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GwanjaLoveProto.Infrastructure
{
    [HtmlTargetElement("img", Attributes = "profile-user")]
    public class ProfileImageTagHelper : TagHelper
    {
        private readonly UserManager<AppUser> _userManager;

        public ProfileImageTagHelper(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        [HtmlAttributeName("profile-user")]
        public string UserName { get; set; }
        public string UserImage { get; set; }

        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            AppUser user = await _userManager.FindByNameAsync(UserName);

            if (user != null)
            {
                string base64 = "";
                string mimeType = "image/jpeg";
                if (user.AvatarImage?.Length > 0)
                    base64 = Convert.ToBase64String(user.AvatarImage);
                string filename = string.Format("data:{0};base64,{1}", mimeType, base64);
                output.Attributes.SetAttribute("src", $"{filename}");
                output.Attributes.SetAttribute("class", "img-thumbnail rounded-circle");
                output.Attributes.SetAttribute("style", "height:30px");
            }

        }
    }
}
