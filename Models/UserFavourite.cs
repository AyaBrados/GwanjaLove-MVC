namespace GwanjaLoveProto.Models
{
    public class UserFavourite : BaseModel
    {
        public required string UserId { get; set; }
        public required int ProductId { get; set; }
        public Product Product { get; set; }
        public AppUser User { get; set; }
    }
}
