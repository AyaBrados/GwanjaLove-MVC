namespace GwanjaLoveProto.Models
{
    public class OrderProduct : BaseModel
    {
        public required int OrderId { get; set; }
        public int? ProductId { get; set; }
        public int? ShopSpecialId { get; set; }
        public double ProductCount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
        public ShopSpecial Special { get; set; }
    }
}
