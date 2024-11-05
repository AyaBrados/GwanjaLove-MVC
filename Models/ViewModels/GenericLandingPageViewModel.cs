namespace GwanjaLoveProto.Models.ViewModels
{
    public class GenericLandingPageViewModel<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public bool? SuccessfullPersistence { get; set; }
        public object? Filters { get; set; }
    }
}
