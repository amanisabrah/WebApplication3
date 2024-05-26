using DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WebApplication3.Models;

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
        public ActionResult<List<USE_User>> GetUserList()
        {
            return _context.USE_User.ToList();
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _context.USE_User.SingleOrDefault(x=>x.USE_User_Email == email);
            if (user == null || user.USE_User_Password != password)
            {
                return Unauthorized("Invalid email or password.");
            }
            return Ok("Login successful");
        }

        [HttpPost]
        public ActionResult Register(RegisterParam param)
        {
            var existingUser = _context.USE_User.FirstOrDefault(x => x.USE_User_Email == param.Email);

            if (existingUser != null)
            {
                if (existingUser.USE_User_IsApproved.HasValue && existingUser.USE_User_IsApproved.Value)
                    return Conflict("User already exists and is approved.");

                existingUser.USE_User_IsApproved = true;
                _context.SaveChanges();

                return Ok($"User with email {param.Email} has been approved.");
            }

            var newUser = new USE_User
            {
                USE_User_Name = param.Name,
                USE_User_Email = param.Email,
                USE_User_Password = param.Password,
                USE_User_Phone = param.Phone,
                USE_User_Gender = param.Gender,
                USE_User_IsApproved = true
            };

            _context.USE_User.Add(newUser);
            _context.SaveChanges();

            return Ok($"New user with email {param.Email} has been registered and approved.");
        }




    }


}
