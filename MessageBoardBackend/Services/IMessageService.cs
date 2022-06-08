using System;
namespace MessageBoardBackend.Services
{
	public interface IMessageService
	{
		Task<bool> AddMessage(Message message);

		Task<bool> DeleteMessage(int id);

		Task<bool> UpdateMessage(int id, string contents, int userId, bool admin);

		Task<IEnumerable<Message>> GetByTopic(int topicId);

		Task<bool> UpdateTopicLastMessage(Message message);
	}
}

