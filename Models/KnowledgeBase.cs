namespace GwanjaLoveProto.Models
{
    public class KnowledgeBase : BaseModel
    {
        public List<byte[]>? TermImages { get; set; }
        public required int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
