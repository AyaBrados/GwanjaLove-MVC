namespace GwanjaLoveProto.Models
{
    public class SurveyResponse : BaseModel
    {
        public required string UserId { get; set; }
        public required int SurveyId { get; set; }
        public List<string> Answer { get; set; } = new List<string>();
        public Survey Survey { get; set; }
        public double Rating { get; set; }
        public int? ProductId { get; set; }
        public Product? Product { get; set; }
        public int? UserFavouriteId { get; set; }
        public UserFavourite? UserFavourite { get; set; }
        public bool Approved { get; set; }
    }
}
