namespace MessageBoardBackend
{
	public class Topic
	{
		public int Id { get; set; }

		public string Title { get; set; } = string.Empty;

		public int MessageCount { get; set; } = 0;

		public DateTime LastMessageTimeStamp { get; set; }

		public virtual User? User { get; set; }

		public int? UserId { get; set; }

		public virtual List<Message>? Messages { get; set; }
	}
}

