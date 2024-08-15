using GwanjaLoveProto.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GwanjaLoveProto.Data
{
    public class AppDBContext(DbContextOptions<AppDBContext> options) : IdentityDbContext<IdentityUser>(options)
	{
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Category>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("CategoryNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
			modelBuilder.Entity<Product>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("ProductNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
			modelBuilder.Entity<Product>().HasIndex("CategoryId").IsUnique().IsDescending(false)
                        .HasDatabaseName("ProductCategoryIndex")
                        .HasFilter("[CategoryId] IS NOT NULL");
			modelBuilder.Entity<CustomerLoyalty>().HasIndex("UserId").IsUnique().IsDescending(false)
                        .HasDatabaseName("CustomerLoyaltyUserIndex")
                        .HasFilter("[UserId] IS NOT NULL");
			modelBuilder.Entity<News>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("NewsNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<Order>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("OrderNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<Order>().HasIndex("UserId").IsUnique().IsDescending(false)
                        .HasDatabaseName("OrderUserIndex")
                        .HasFilter("[UserId] IS NOT NULL");
            modelBuilder.Entity<OrderReceiveMethod>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("OrderReceiveMethodNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<PaymentMethod>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("PaymentMethodNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<Question>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("QuestionNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<SensamilliaService>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("SensamilliaServiceNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<StrainStickiness>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("StrainStickinessNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<StrainType>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("StrainTypeNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<Survey>().HasIndex("Name").IsUnique().IsDescending(false)
                        .HasDatabaseName("SurveyNameIndex")
                        .HasFilter("[Name] IS NOT NULL");
            modelBuilder.Entity<Survey>()
        }
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
	}
}
