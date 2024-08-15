namespace GwanjaLoveProto.Models
{
    public class Question : BaseModel
    {
        public required int SurveyId { get; set; }
        public required Survey Survey { get; set; }
    }
}
