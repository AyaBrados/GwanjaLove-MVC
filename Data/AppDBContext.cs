using GwanjaLoveProto.Data.Implementations;
using GwanjaLoveProto.Models;
using Microsoft.EntityFrameworkCore;

namespace GwanjaLoveProto.Data
{
	public class AppDBContext : DbContext
	{
		public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}
