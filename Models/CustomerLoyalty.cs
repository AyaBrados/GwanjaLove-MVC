namespace GwanjaLoveProto.Models
{
    public class CustomerLoyalty : BaseModel
    {
        public required int UserId { get; set; }
        public double LoyaltyPoints { get; set; }
    }
}
