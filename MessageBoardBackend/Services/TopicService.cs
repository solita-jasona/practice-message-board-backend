using System;

namespace MessageBoardBackend.Services
{
	public class TopicService : ITopicService
	{
        private readonly DataContext _context;

        public TopicService(DataContext context)
		{
            _context = context;
        }

        public async Task<bool> AddTopic(Topic topic)
        {
            try
            {
                _context.Topic.Add(topic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public async Task<bool> DeleteTopic(int id)
        {
            try
            {
                var topic = await _context.Topic.FindAsync(id);
                if (topic == null)
                {
                    return false;
                }
                _context.Topic.Remove(topic);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await _context.Topic.Include(t => t.LastMessage).OrderByDescending(t => t.LastMessage.TimeStamp).ToListAsync();
        }

        public async Task<bool> UpdateTopic(int id, string title)
        {
            try
            {
                var topic = await _context.Topic.FindAsync(id);
                if (topic == null || topic.MessageCount > 0)
                {
                    return false;
                }
                topic.Title = title;
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

