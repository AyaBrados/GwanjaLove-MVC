namespace GwanjaLoveProto.Models
{
    public class PaymentMethod : BaseModel
    {
        public List<Order>? Orders { get; set; }
    }
}
