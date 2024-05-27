using DB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
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

        private int GetUSRID()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;//string
            return int.Parse(userIdClaim);//converts the string to an int
        }

        public UserLogInController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #region Users Actions

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<AAA_USR_User> GetUserList()
        {
            var userId = GetUSRID();

            return Ok(_context.AAA_USR_User.Where(usr => usr.AAA_USR_ID == userId).Single());
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var user = _context.AAA_USR_User.SingleOrDefault(x => x.AAA_USR_Email == email);
            if (user == null || user.AAA_USR_Password != password)
            {
                return Unauthorized("Invalid email or password.");
            }
            if (user.AAA_USR_IsApproved == false)
            {
                return Conflict("User is not approved Yet.");
            }
            var isAdmin = user.AAA_USR_Email == "admin@gmail.com";
            var tokenHandler = new JwtSecurityTokenHandler();
            //_configuration["JWt:Secret"]
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"] ?? "");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    //new Claim("ClaimTypes.Name", user.AAA_USR_Name ?? ""),
                    new Claim(ClaimTypes.NameIdentifier, user.AAA_USR_ID.ToString()),//Extracts the User_ID from token claims
                    new Claim(ClaimTypes.Name, user.AAA_USR_Name ?? ""),
                    new Claim(ClaimTypes.Email, user.AAA_USR_Email ?? ""),
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
            if (_context.AAA_USR_User.Any(x => x.AAA_USR_Email == param.Email))
            {
                return Conflict("User already exists.");
            }

            var newUser = new AAA_USR_User
            {
                AAA_USR_Name = param.Name,
                AAA_USR_Email = param.Email,
                AAA_USR_Password = param.Password,
                AAA_USR_Phone = param.Phone,
                AAA_USR_Gender = param.Gender,
            };
            _context.AAA_USR_User.Add(newUser);
            _context.SaveChanges();

            return Ok("User registered. Waiting for admin approval.");
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "admin")]
        public ActionResult UpdateUserByID(int id, UpdateUserBYIDParam param)
        {
            var user = _context.AAA_USR_User.SingleOrDefault(x => x.AAA_USR_ID == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            if (user.AAA_USR_ID != 1)
            {
                user.AAA_USR_Name = param.Name;
                user.AAA_USR_Password = param.Password;
                user.AAA_USR_Phone = param.Phone;
                user.AAA_USR_Email = param.Email;
                user.AAA_USR_Gender = param.Gender;
            }
            _context.SaveChanges();

            return Ok("User information updated successfully.");
        }

        [HttpDelete("{id}")]
        //[Authorize(Roles = "admin")]
        public ActionResult DeleteUserByID(int id)
        {
            var user = _context.AAA_USR_User.SingleOrDefault(x => x.AAA_USR_ID == id);
            if (user == null)
            {
                return NotFound("User not found.");
            }
            var userInformation = new
            {
                ID = user.AAA_USR_ID,
                Name = user.AAA_USR_Name,
                Email = user.AAA_USR_Email,
                Phone = user.AAA_USR_Phone,
                Gender = user.AAA_USR_Gender
            };

            if (user.AAA_USR_ID != 1)
            {
                _context.AAA_USR_User.Remove(user);
            }
            _context.SaveChanges();
            return Ok(new { Message = "User deleted successfully.", User = userInformation });

        }
        #endregion

        #region Req Actions

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<AAA_REQ_Requset> GetAdminList()
        {
            var userId = GetUSRID();
            var req = _context.AAA_REQ_Requset.Where(req => req.AAA_REQ_USRID_Update == userId).ToList();
            return Ok(req);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<List<AAA_REQ_Requset>> GetReqListForAdmin()
        {
            var userId = GetUSRID();
            return Ok(_context.AAA_REQ_Requset.ToList());
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult CreateRequest(CreatReqParm parm)
        {
            var userId = GetUSRID();
            var newRequest = new AAA_REQ_Requset
            {
                AAA_REQ_Message = parm.Message,
                AAA_REQ_USRID_Entry = userId,
                AAA_REQ_USRID_Update = parm.USRID_Update,
            };

            _context.AAA_REQ_Requset.Add(newRequest);
            _context.SaveChanges();

            return Ok("Request created successfully.");
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public ActionResult UpdateRequest(int id, UpdateReqParm param)
        {
            var userId = GetUSRID();
            var existingRequest = _context.AAA_REQ_Requset.Find(id);
            if (existingRequest == null)
            {
                return NotFound("Request not found.");
            }

            existingRequest.AAA_REQ_Message = param.Message;
            existingRequest.AAA_REQ_USRID_Entry = userId;
            existingRequest.AAA_REQ_USRID_Update = param.UserID;

            _context.SaveChanges();

            return Ok("Request updated successfully.");
        }

        [HttpGet]
        // [Authorize(Roles = "admin")]
        public ActionResult<List<AAA_REQ_Requset>> Test1()
        {
            var todayDate = DateTime.Today;
            var entrydate = todayDate.AddDays(-3);
            var req = _context.AAA_REQ_Requset.Where(u => u.AAA_REQ_Message != null && u.AAA_REQ_Message.Contains("Hello") &&
                      u.AAA_REQ_EntryDate <= todayDate &&
                      entrydate < u.AAA_REQ_EntryDate).ToList();

            return Ok(req);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult<List<AAA_REQ_Requset>> Test2()
        {
            var aliID = _context.AAA_USR_User.Where(u => u.AAA_USR_Name == "Ali").Select(u => u.AAA_USR_ID).ToList();
            if (!aliID.Any())
            {
                return NotFound("No records found for user Ali.");
            }

            var aliReq = _context.AAA_REQ_Requset.Where(r => aliID.Contains(r.AAA_REQ_USRID_Update.GetValueOrDefault())).ToList();
            return Ok(aliReq);
        }
        #endregion
    }
}
