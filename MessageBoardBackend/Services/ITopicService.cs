using System;
namespace MessageBoardBackend.Services
{
	public interface ITopicService
	{
		Task<bool> AddTopic(Topic topic);

		Task<bool> UpdateTopic(int id, string title);

		Task<bool> DeleteTopic(int id);

		Task<IEnumerable<Topic>> GetAll();

		Task<Topic> GetTopic(int id);
	}
}

