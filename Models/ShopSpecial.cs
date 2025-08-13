using System.ComponentModel.DataAnnotations;

namespace GwanjaLoveProto.Models
{
    public class ShopSpecial : BaseModel
    {
        public required int SpecialProductId { get; set; }
        public SpecialProduct Product { get; set; }
        public decimal SpecialPrice { get; set; }
        public DateTime SpecialEndDate { get; set; }
        [Display(Name = "Special Image")]
        public byte[] Image { get; set; }
    }
}
