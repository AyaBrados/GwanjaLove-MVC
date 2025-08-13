using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace GwanjaLoveProto.Infrastructure
{
    [HtmlTargetElement("img", Attributes = "entity-image")]
    public class EntityImageTagHelper : TagHelper
    {
        public EntityImageTagHelper()
        {
        }

        [HtmlAttributeName("entity-image")]
        public byte[]? Image { get; set; }

        public override async Task ProcessAsync(TagHelperContext context,
            TagHelperOutput output)
        {
            if (Image != null)
            {
                string base64 = "";
                string mimeType = "image/jpeg";
                if (Image.Length > 0)
                    base64 = Convert.ToBase64String(Image);
                string filename = string.Format("data:{0};base64,{1}", mimeType, base64);
                output.Attributes.SetAttribute("src", $"{filename}");
            }
        }
    }
}
