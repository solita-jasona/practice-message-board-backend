using System;

namespace MessageBoardBackend.Services
{
	public class MessageService : IMessageService
	{
		private readonly DataContext _context;

		public MessageService(DataContext context)
		{
			_context = context;
		}

        public async Task<bool> AddMessage(Message message)
        {
                await _context.Message.AddAsync(message);
                await _context.SaveChangesAsync();
                return await UpdateTopicLastMessage(message);
        }

        public async Task<bool> DeleteMessage(int id)
        {
            try
            {
                var message = await _context.Message.FindAsync(id);
                if (message == null)
                {
                    return false;
                }
                _context.Message.Remove(message);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Message>> GetByTopic(int topicId)
        {
            return await _context.Message.Where(t => t.TopicId == topicId).Include(t => t.User).OrderByDescending(t => t.TimeStamp).ToListAsync();
        }

        public async Task<bool> UpdateMessage(int id, string contents, int userId, bool admin)
        {
            try
            {
                var message = await _context.Message.FindAsync(id);
                if (message == null || (message.UserId != userId && !admin))
                {
                    return false;
                }
                message.Contents = contents;
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateTopicLastMessage(Message message)
        {
            try
            {
                var topic = await _context.Topic.FindAsync(message.Id);
                topic.LastMessageTimeStamp = message.TimeStamp;
                topic.MessageCount = await _context.Message.Where(t => t.TopicId == message.TopicId).CountAsync();
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}

