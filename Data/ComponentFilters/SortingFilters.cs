namespace GwanjaLoveProto.Data.ComponentFilters
{
    public class SortingAndPagingFilters
    {
        public string SortBy { get; set; }
        public int PageSize { get; set; } = 25;
        public int PageCount { get; set; }
        public string SortDirection { get; set; } = "asc";
    }

    public enum SortBy
    {
        None = 0,
        Favourites = 1,
        LowToHighPrice = 2,
        HighToLowPrice = 3,
        IsInStock = 4
    }

    public enum SortDirection
    {
        Asc = 0, Desc = 1
    }

    public enum PageSize
    {
        Small = 0,
        Medium = 1,
        Large = 2
    }
}
