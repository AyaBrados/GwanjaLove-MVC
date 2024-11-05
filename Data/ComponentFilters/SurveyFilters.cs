using GwanjaLoveProto.Models;

namespace GwanjaLoveProto.Data.ComponentFilters
{
    public class SurveyFilters : BaseFilters
    {
        public Survey? Survey { get; set; }
        public Question? Question { get; set; }
    }
}
