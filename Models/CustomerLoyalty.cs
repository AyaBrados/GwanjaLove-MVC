﻿namespace GwanjaLoveProto.Models
{
    public class CustomerLoyalty : BaseModel
    {
        public required string UserId { get; set; }
        public double LoyaltyPoints { get; set; }
    }
}
