namespace MessageBoardBackend
{
	public class Topic
	{
		public int Id { get; set; }

		public int Title { get; set; }

		public int MessageCount { get; set; } = 0;

		public virtual Message? LastMessage { get; set; }

		public int? LastMessageId { get; set; }

		public virtual User? User { get; set; }

		public int? UserId { get; set; }
	}
}

