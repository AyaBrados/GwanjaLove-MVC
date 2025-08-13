namespace GwanjaLoveProto.Models.ViewModels
{
	public class ProductViewModel
	{
		public Product Product { get; set; }
		public List<SurveyResponse> Reviews { get; set; }
		public List<UserFavourite> Favourites { get; set; }
		public SurveyResponse? CurrentReview { get; set; }
	}
}
