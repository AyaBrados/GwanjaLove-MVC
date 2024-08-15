namespace GwanjaLoveProto.Models
{
    public class OrderProduct : BaseModel
    {
        public required int OrderId { get; set; }
        public required int  ProductId { get; set; }
        public int ProductCount { get; set; }
        public required Order Order { get; set; }
        public required Product Product { get; set; }
    }
}
