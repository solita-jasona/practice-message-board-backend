using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessageBoardBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public static User user = new User();

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("current"), Authorize()]
        public async Task<User?> GetCurrent()
        {
            var userIdString = User.Identity.Name;
            int userId = Int32.Parse(userIdString);
            return await _userService.GetUser(userId);
        }
    }
}
