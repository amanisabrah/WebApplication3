using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WebApplication3.Models;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication3.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserLogInController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UserLogInController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<List<USE_User>> GetUserList()
        {
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
            return Ok("Login successful");
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
