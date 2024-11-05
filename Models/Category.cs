namespace GwanjaLoveProto.Models
{
	public class Category : BaseModel
	{
		public List<Product> Products { get; set; } = new List<Product>();
		public byte[]? Image { get; set; }
	}
}
