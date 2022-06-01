namespace MessageBoardBackend
{
	public class User
	{
		public int Id { get; set; }

		public string Username { get; set; } = string.Empty;

		public byte[] PasswordHash { get; set; }

		public byte[] PasswordSalt { get; set; }

		public string UserEmail { get; set; }

		public int? RefreshTokenId { get; set; }

		public virtual RefreshToken? RefreshToken { get; set; }

		public int RoleId { get; set; }

		public virtual Role Role { get; set; }
	}
}

