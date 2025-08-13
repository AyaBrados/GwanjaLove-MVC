namespace GwanjaLoveProto.Models
{
	public class GeneralLog : BaseModel
	{
		public required string UserId { get; set; }
		public AppUser User { get; set; }
	}
}
