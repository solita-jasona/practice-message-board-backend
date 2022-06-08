using System;
namespace MessageBoardBackend
{
	public class MessageDto
	{
		public int Id { get; set; }
		public string Contents { get; set; } = string.Empty;
		public int TopicId { get; set; }
	}
}

