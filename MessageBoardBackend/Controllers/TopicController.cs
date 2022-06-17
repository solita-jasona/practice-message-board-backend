using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoardBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TopicController : Controller
    {
        public static Topic topic = new Topic();
        private readonly ITopicService _topicService;

        public TopicController(ITopicService topicService)
        {
            _topicService = topicService;
        }

        [HttpGet("all")]
        public async Task<IEnumerable<Topic>> GetAll()
        {
            return await _topicService.GetAll();
        }

        [HttpGet("single/{id}")]
        public async Task<Topic> GetSingle(int id)
        {
            return await _topicService.GetTopic(id);
        }

        [HttpPost("add"), Authorize()]
        public async Task<ActionResult<bool>> AddTopic(TopicDto request)
        {
            string title = request.Title;
            if (String.IsNullOrEmpty(title))
            {
                return BadRequest("missing parameters");
            }
            var topic = new Topic();
            var userIdString = User.Identity.Name;
            int userId = Int32.Parse(userIdString);
            topic.Title = title;
            topic.UserId = (int)userId;
            return await _topicService.AddTopic(topic);
        }

        [HttpPut("update"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> UpdateTitle(TopicDto request)
        {
            var user = User;
            int topicId = request.Id;
            string title = request.Title;
            if (String.IsNullOrEmpty(title) || topicId < 1)
            {
                return BadRequest("missing or invalid parameters");
            }
            return await _topicService.UpdateTopic(topicId, title);
        }

        [HttpDelete("delete/{id}"), Authorize(Roles = "Admin")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("invalid id");
            }
            return await _topicService.DeleteTopic(id);
        }



    }
}
