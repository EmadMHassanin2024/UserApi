using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserApi.Models;

namespace UserApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //add IConfiguration
        private readonly IConfiguration _config;
        //
        private static readonly List<UserModel> Users = new()
        {
            new UserModel { Id = 1, Username = "admin", Password = "123456" },
            new UserModel { Id = 2, Username = "user1", Password = "password" }
        };

        //

        public AuthController(IConfiguration config)
        {
            _config = config;
        }

        //enerateJwtToken
        [HttpPost("login")]
        public IActionResult Login([FromBody] UserModel user)
        {
            var validUser = Users.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
            if (validUser == null)
                return Unauthorized(new { message = "Invalid username or password" });

            var token = GenerateJwtToken(validUser);
            return Ok(new { token });
        }

        //GenerateJwtToken
        private string GenerateJwtToken(UserModel user)
        {
            //Secret Key
            var key = Encoding.ASCII.GetBytes(_config["JwtSettings:SecretKey"]);
            // new JWT Tokens.
            var tokenHandler = new JwtSecurityTokenHandler();
            //Token settings.
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }),
                Expires = DateTime.UtcNow.AddHours(1),
               // Sign
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //Create
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //return string
            return tokenHandler.WriteToken(token);
        }
    }
}
