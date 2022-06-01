﻿using Microsoft.EntityFrameworkCore;
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
	}
}
