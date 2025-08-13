namespace GwanjaLoveProto.Models
{
    public class Order : BaseModel
    {
        public required string UserId { get; set; }
        public int PaymentMethodId { get; set; }
        public int OrderRecieveMethodId { get; set; } 
        public PaymentMethod PaymentMethod { get; set; }
        public OrderReceiveMethod OrderReceiveMethod { get; set; }
        public bool OrderReceived { get; set; }
        public bool PaymentReceived { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime OrderReceivedDate { get; set; }
        public decimal OrderAmount { get; set; }
        public List<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
    }
}
