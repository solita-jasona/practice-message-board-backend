namespace MessageBoardBackend
{
	public class Message
	{
		public int Id { get; set; }

		public string Contents { get; set; } = string.Empty;

		public DateTime TimeStamp { get; set; } = DateTime.Now;

		public int UserId { get; set; }

		public virtual User User { get; set; }

		
	}
}

