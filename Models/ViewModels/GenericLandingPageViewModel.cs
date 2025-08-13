using GwanjaLoveProto.Data.ComponentFilters;

namespace GwanjaLoveProto.Models.ViewModels
{
    public class GenericLandingPageViewModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public SuccessfullPersistenceViewModel? SuccessfullPersistence { get; set; } = null;
        public BaseFilters Filters { get; set; } = new BaseFilters();
    }
}
