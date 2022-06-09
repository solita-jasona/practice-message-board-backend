using Microsoft.EntityFrameworkCore;
namespace MessageBoardBackend.Data
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options) : base(options) { }

		public DbSet<User> User { get; set; }

		public DbSet<RefreshToken> RefreshToken { get; set; }

		public DbSet<Role> Role { get; set; }

		public DbSet<Topic> Topic { get; set; }

		public DbSet<Message> Message{ get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Message>()
				.HasOne(p => p.Topic)
				.WithMany(b => b.Messages)
				.HasForeignKey(p => p.TopicId);

			modelBuilder.Entity<Role>().HasData(
				new Role { Id = 1, Name = "Admin" },
				new Role { Id = 2, Name = "Poster"});
		}

	}
}

