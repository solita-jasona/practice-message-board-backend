using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoardBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController: Controller
    {
        public static Message topic = new Message();
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("byTopic/{id}")]
        public async Task<IEnumerable<Message>> GetAll(int id)
        {
            return await _messageService.GetByTopic(id);
        }

        [HttpPost("add"), Authorize()]
        public async Task<ActionResult<bool>> AddTopic(MessageDto request)
        {
            string content = request.Contents;
            if (String.IsNullOrEmpty(content))
            {
                return BadRequest("missing parameters");
            }
            int topicId = request.TopicId;
         
            var message = new Message();
            var userIdString = User.Identity.Name;
            int userId = Int32.Parse(userIdString);
            message.UserId = userId;
            message.Contents = content;
            message.TopicId = topicId;
            return await _messageService.AddMessage(message);
        }

        [HttpPut("update"), Authorize()]
        public async Task<ActionResult<bool>> UpdateMessage(MessageDto request)
        {
            var user = User;
            int messageId = request.Id;
            string contents = request.Contents;
            if (String.IsNullOrEmpty(contents) || messageId < 1)
            {
                return BadRequest("missing or invalid parameters");
            }
            var userIdString = user.Identity.Name;
            int userId = Int32.Parse(userIdString);
            var admin = user.IsInRole("Admin");
            return await _messageService.UpdateMessage(messageId, contents, userId, admin);
        }

        [HttpDelete("delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid id");
            }
            return await _messageService.DeleteMessage(id);
        }
    }
    

}
