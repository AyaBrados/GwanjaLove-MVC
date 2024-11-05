using GwanjaLoveProto.Models;

namespace GwanjaLoveProto.Data.Interfaces
{
    public interface IUnitOfWork
    {
        IRepositoryBase<Product> ProductRepository { get; }
        IRepositoryBase<Category> CategoryRepository { get; }
        IRepositoryBase<CustomerLoyalty> CustomerLoyaltyRepository { get; }
        IRepositoryBase<KnowledgeBase> KnowledgeBaseRepository { get; }
        IRepositoryBase<News> NewsRepository { get; }
        IRepositoryBase<Order> OrderRepository { get; }
        IRepositoryBase<OrderReceiveMethod> OrderReceiveMethodRepository { get; }
        IRepositoryBase<PaymentMethod> PaymentMethodRepository { get; }
        IRepositoryBase<Question> QuestionRepository { get; }
        IRepositoryBase<SensamilliaService> SensamilliaServiceRepository { get; }
        IRepositoryBase<StrainStickiness> StrainStickinessRepository { get; }
        IRepositoryBase<StrainType> StrainTypeRepository { get; }
        IRepositoryBase<Survey> SurveyRepository { get; }
        IRepositoryBase<OrderProduct> OrderProductRepository { get; }
        IRepositoryBase<ShopSpecial> ShopSpecialRepository { get; }
        IRepositoryBase<SurveyResponse> SurveyResponseRepository { get; }
        IRepositoryBase<UserFavourite> UserFavouriteRepository { get; }
        bool Save();
    }
}
