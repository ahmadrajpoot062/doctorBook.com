using Core.Interfaces;
using doctorBook_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace doctorBook_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly string _key;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IRepository<ApiUsers> _userRepository;

        public AuthController(IConfiguration config, IRepository<ApiUsers> userRepository)
        {
            _key = config["Jwt:Key"];
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
            _userRepository = userRepository;
        }

        [HttpPost("LoginUser")]
        public async Task<ActionResult<ApiUsers>> LoginUser([FromBody] UserCredentials credentials)
        {
            var users = await _userRepository.GetAll();

            if (users == null || users.Count == 0)
            {
                return NotFound("No users found.");
            }

            // Find a user that matches the provided email and password
            var user = users.FirstOrDefault(u =>
                u.Email == credentials.Email &&
                u.Password == credentials.Password);

            if (user == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_key);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, credentials.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256),
                Issuer = _issuer,
                Audience = _audience
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new { Token = tokenHandler.WriteToken(token),  user.Id, user.FirstName, user.LastName, user.Email, user.Role });

        }

    }
}
