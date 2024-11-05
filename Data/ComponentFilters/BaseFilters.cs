using GwanjaLoveProto.Models;

namespace GwanjaLoveProto.Data.ComponentFilters
{
    public class BaseFilters
    {
        public bool? Active { get; set; }
        public bool? SuccessfulPersistence { get; set; } = null;
        public string? Name { get; set; }
        public Category? Category { get; set; }
        public AppUser? User { get; set; }
        public int? PkId { get; set; }
    }
}
