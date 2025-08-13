namespace GwanjaLoveProto.Models
{
	public class SpecialProduct : BaseModel
	{
		public required int ProductId { get; set; }
		public Product Product { get; set; }
		public int ProductCount { get; set; }
	}
}
