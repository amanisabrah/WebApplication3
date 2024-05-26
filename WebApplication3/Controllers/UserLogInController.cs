using DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserLogInController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserLogInController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<List<USE_User>> GetUserList()
        {

            var userId = User.FindFirst("ClaimTypes.Name")?.Value;
            return _context.USE_User.ToList();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _context.USE_User.SingleOrDefault(x => x.USE_User_Email == email);
            if (user == null || user.USE_User_Password != password)
            {
                return Unauthorized("Invalid email or password.");
            }
            if (user.USE_User_IsApproved == false)
            {
                return Conflict("User is not approved Yet.");
            }

            var isAdmin = user.USE_User_Email == "admin@gmail.com";
            var tokenHandler = new JwtSecurityTokenHandler();
            //_configuration["JWt:Secret"]
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("ClaimTypes.Name", user.USE_User_Name ?? ""),
                    new Claim(ClaimTypes.Name, user.USE_User_Name ?? ""),
                    new Claim(ClaimTypes.Email, user.USE_User_Email ?? ""),
                    new Claim(ClaimTypes.Role, isAdmin ? "admin" : "user")
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new { Token = "Bearer " + tokenString });
        }

        [HttpPost]
        public ActionResult Register(RegisterParam param)
        {
            if (_context.USE_User.Any(x => x.USE_User_Email == param.Email))
            {
                return Conflict("User already exists.");
            }

            var newUser = new USE_User
            {
                USE_User_Name = param.Name,
                USE_User_Email = param.Email,
                USE_User_Password = param.Password,
                USE_User_Phone = param.Phone,
                USE_User_Gender = param.Gender
            };
            _context.USE_User.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered. Waiting for admin approval.");
        }
    }
}
