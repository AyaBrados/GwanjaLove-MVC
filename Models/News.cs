using System.ComponentModel.DataAnnotations;

namespace GwanjaLoveProto.Models
{
    public class News : BaseModel
    {
        [Required]
        public required string Link { get; set; }
    }
}
