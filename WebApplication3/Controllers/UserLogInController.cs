using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserLogInController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserLogInController(AppDbContext context)
        { 
            _context = context;
        }

        [HttpPost("login")]
        public ActionResult Login(string email, string password)
        {
            var user = _context.USE_User.SingleOrDefault(x=>x.USE_User_Email == email);
            if (user == null || user.USE_User_Password != password)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok("Login successful");
        }
    }
}
