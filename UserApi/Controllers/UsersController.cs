using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Models;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private static List<UserModel> Users = new()
        {
            new UserModel { Id = 1, Username = "admin", Password = "123456" },
            new UserModel { Id = 2, Username = "user1", Password = "password" }
        };

        // إضافة مستخدم جديد
        [HttpPost]
        public IActionResult AddUser([FromBody] UserModel newUser)
        {
            if (Users.Any(u => u.Username == newUser.Username))
                return Conflict("Username already exists");

            newUser.Id = Users.Max(u => u.Id) + 1;
            Users.Add(newUser);
            return Ok(newUser);
        }

        // عرض المستخدمين (محمي بالتوكن)
        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users.Select(u => new { u.Id, u.Username }));
        }

       
    }
}
