namespace GwanjaLoveProto.Models
{
    public class Survey : BaseModel
    {
        public required List<Question> Questions { get; set; }
    }
}
