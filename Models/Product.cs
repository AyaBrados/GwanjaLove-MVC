namespace GwanjaLoveProto.Models
{
	public class Product : BaseModel
	{
		public int CategoryId { get; set; }
		public bool IsInStock { get; set; }
		public Category Category { get; set; }
	}
}
