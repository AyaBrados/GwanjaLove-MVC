namespace GwanjaLoveProto.Models
{
    public class Product : BaseModel
	{
		public required int CategoryId { get; set; }
		public int? StrainTypeId { get; set; }
		public int? StrainStickinessId { get; set; }
		public bool IsInStock { get; set; }
		public required decimal Price { get; set; }
		public string? Aroma { get; set; }
		public string? Flavour { get; set; }
		public StrainType? StrainType { get; set; }
		public StrainStickiness? StrainStickiness { get; set; }
		public required Category Category { get; set; }
        public List<OrderProduct>? OrderProducts { get; set; }
    }
}
