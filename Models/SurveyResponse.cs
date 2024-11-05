namespace GwanjaLoveProto.Models
{
    public class SurveyResponse : BaseModel
    {
        public required string UserId { get; set; }
        public required int SurveyId { get; set; }
        public List<string> Answer { get; set; } = new List<string>();
        public Survey Survey { get; set; }
    }
}
