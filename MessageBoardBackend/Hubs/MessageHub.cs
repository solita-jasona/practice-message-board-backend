using Microsoft.AspNetCore.SignalR;

namespace MessageBoardBackend.Hubs
{
	public class MessageHub: Hub
	{
        public async Task SendMessage(int topicId)
        {
            await Clients.Others.SendAsync("ReceiveMessage", topicId);
        }
    }
}

