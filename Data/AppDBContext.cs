using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GwanjaLoveProto.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : IdentityDbContext<IdentityUser>(options)
	{
        public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CustomerLoyalty> CustomerLoyalties { get; set; }
		public DbSet<News> News { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderProduct> OrderProducts { get; set; }
		public DbSet<OrderReceiveMethod> OrderReceiveMethods { get; set; }
		public DbSet<PaymentMethod> PaymentMethods { get; set; }
		public DbSet<Question> Questions { get; set; }
		public DbSet<SensamilliaService> SensamilliaServices { get; set; }
		public DbSet<Survey> Surveys { get; set; }
		public DbSet<StrainType> StrainTypes { get; set; }
		public DbSet<StrainStickiness> StrainStickiness { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<KnowledgeBase> KnowledgeTerms { get; set; }
	}
}
