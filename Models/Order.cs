namespace GwanjaLoveProto.Models
{
    public class Order : BaseModel
    {
        public required string UserId { get; set; }
        public required int PaymentMethodId { get; set; }
        public required int OrderRecieveMethodId { get; set; } 
        public required PaymentMethod PaymentMethod { get; set; }
        public required OrderReceiveMethod OrderReceiveMethod { get; set; }
        public bool OrderReceived { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderReceivedDate { get; set; }
        public decimal OrderAmount { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }
    }
}
