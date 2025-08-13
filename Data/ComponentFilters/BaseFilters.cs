using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;

namespace GwanjaLoveProto.Data.ComponentFilters
{
    public class BaseFilters
    {
        public bool? Active { get; set; } = true;
        public SuccessfullPersistenceViewModel SuccessfullPersistence { get; set; }
        public string? Name { get; set; }
        public Category? Category { get; set; }
        public AppUser? User { get; set; }
        public int? PkId { get; set; }
        public SortingAndPagingFilters SortingAndPagingFilters { get; set; }
    }
}
