namespace GwanjaLoveProto.Models
{
    public class ShopSpecial : BaseModel
    {
        public required int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal SpecialPrice { get; set; }
        public DateTime SpecialEndDate { get; set; }
    }
}
