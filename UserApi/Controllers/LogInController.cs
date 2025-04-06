using Microsoft.AspNetCore.Authorization;
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
    public class LogInController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LogInController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Post([FromBody] UserModel model)
        {
            var response = Unauthorized();
            UserModel user = AuthorizeUser(model);

            if (user != null)
            {
                var token = GenerateToken(model);
                return Ok(new { token= token });    
            }

            return response;
        }

        private UserModel AuthorizeUser(UserModel model)
        {
            // هذا مثال بسيط. في الحالة الحقيقية، ستتأكد من المستخدم من قاعدة بيانات
            if (model.Username == "Emad" && model.Password == "123")
            {
                return new UserModel()
                {
                    Username = "Emad",
                    Email = "Walid30189@gmail.com"
                };
            }
            return null;
        }

        private string GenerateToken(UserModel user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
