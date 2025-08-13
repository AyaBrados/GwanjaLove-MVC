namespace GwanjaLoveProto.Models
{
	public class Cart : BaseModel
	{
		public required string UserId { get; set; }
		public AppUser User { get; set; }
		public List<Product> Products { get; set; } = new List<Product>();
		public int? OrderId { get; set; }
		public Order Order { get; set; }
		public List<ShopSpecial> Specials { get; set; } = new List<ShopSpecial>();
	}
}
