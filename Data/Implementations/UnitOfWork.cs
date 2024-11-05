using GwanjaLoveProto.Data.Interfaces;
using GwanjaLoveProto.Models;

namespace GwanjaLoveProto.Data.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _appDbContext;

        public UnitOfWork(AppDBContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IRepositoryBase<Product> ProductRepository => new RepositoryBase<Product>(_appDbContext);

        public IRepositoryBase<Category> CategoryRepository => new RepositoryBase<Category>(_appDbContext);

        public IRepositoryBase<CustomerLoyalty> CustomerLoyaltyRepository => new RepositoryBase<CustomerLoyalty>(_appDbContext);

        public IRepositoryBase<KnowledgeBase> KnowledgeBaseRepository => new RepositoryBase<KnowledgeBase>(_appDbContext);

        public IRepositoryBase<News> NewsRepository => new RepositoryBase<News>(_appDbContext);

        public IRepositoryBase<Order> OrderRepository => new RepositoryBase<Order>(_appDbContext);

        public IRepositoryBase<OrderReceiveMethod> OrderReceiveMethodRepository => new RepositoryBase<OrderReceiveMethod>(_appDbContext);

        public IRepositoryBase<PaymentMethod> PaymentMethodRepository => new RepositoryBase<PaymentMethod>(_appDbContext);

        public IRepositoryBase<Question> QuestionRepository => new RepositoryBase<Question>(_appDbContext);

        public IRepositoryBase<SensamilliaService> SensamilliaServiceRepository => new RepositoryBase<SensamilliaService>(_appDbContext);

        public IRepositoryBase<StrainStickiness> StrainStickinessRepository => new RepositoryBase<StrainStickiness>(_appDbContext);

        public IRepositoryBase<StrainType> StrainTypeRepository => new RepositoryBase<StrainType>(_appDbContext);

        public IRepositoryBase<Survey> SurveyRepository => new RepositoryBase<Survey>(_appDbContext);
        public IRepositoryBase<OrderProduct> OrderProductRepository => new RepositoryBase<OrderProduct>(_appDbContext);
        public IRepositoryBase<ShopSpecial> ShopSpecialRepository => new RepositoryBase<ShopSpecial>(_appDbContext);
        public IRepositoryBase<SurveyResponse> SurveyResponseRepository => new RepositoryBase<SurveyResponse>(_appDbContext);
        public IRepositoryBase<UserFavourite> UserFavouriteRepository => new RepositoryBase<UserFavourite>(_appDbContext);

        public bool Save()
        {
            return _appDbContext.SaveChanges() > -1;
        }
    }
}
