namespace GwanjaLoveProto.Models
{
    public class OrderProduct : BaseModel
    {
        public required int OrderId { get; set; }
        public required int  ProductId { get; set; }
        public double ProductCount { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
