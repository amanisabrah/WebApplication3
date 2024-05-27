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
        #region Users Actions

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<USE_User> GetUserList()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;//string
            var userId = int.Parse(userIdClaim);//converts the string to an int

            return Ok(_context.USE_User.Where(usr=>usr.USE_User_ID == userId).Single());
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
                    //new Claim("ClaimTypes.Name", user.USE_User_Name ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.USE_User_ID.ToString()),//Extracts the User_ID from token claims
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
                USE_User_Gender = param.Gender,
            };
            _context.USE_User.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered. Waiting for admin approval.");
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "admin")]
        public ActionResult UpdateUserByID(int id,UpdateUserBYIDParam param)
        {
            var user = _context.USE_User.SingleOrDefault(x => x.USE_User_ID == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user.USE_User_ID != 1)
            {
                user.USE_User_Name = param.Name;
                user.USE_User_Password = param.Password;
                user.USE_User_Phone = param.Phone;
                user.USE_User_Email = param.Email;
                user.USE_User_Gender = param.Gender;
            }
            _context.SaveChanges();

            return Ok("User information updated successfully.");
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public ActionResult DeleteUserByID(int id)
        {
            var user = _context.USE_User.SingleOrDefault(x => x.USE_User_ID == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var userInformation = new
            {
                ID = user.USE_User_ID,
                Name = user.USE_User_Name,
                Email = user.USE_User_Email,
                Phone = user.USE_User_Phone,
                Gender = user.USE_User_Gender
            };

            if (user.USE_User_ID != 1)
            {
                _context.USE_User.Remove(user);
            }
            _context.SaveChanges();
            return Ok(new { Message = "User deleted successfully.", User = userInformation });

        }
        #endregion

        #region Req Actions

        [HttpGet]
        public ActionResult<List<USE_TYP_TypeOfRequset>> GetReqList()
        {
            return _context.USE_TYP_TypeOfRequset.ToList();
        }


        [HttpPost]
        public ActionResult CreateRequest(string message, int userID)
        {
            var newRequest = new USE_TYP_TypeOfRequset
            {
                USE_TYP_MEssage = message,
                USE_TYP_UserID = userID
            };

            _context.USE_TYP_TypeOfRequset.Add(newRequest);
            _context.SaveChanges();

            return Ok("Request created successfully.");
        }

        [HttpPut("{id}")]
        public ActionResult UpdateRequest(int id, UpdateReq param)
        {
            var existingRequest = _context.USE_TYP_TypeOfRequset.Find(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            existingRequest.USE_TYP_MEssage = param.USE_TYP_MEssage;
            existingRequest.USE_TYP_UserID = param.USE_TYP_UserID;

            _context.SaveChanges();

            return Ok("Request updated successfully.");
        }
        #endregion
    }
}
