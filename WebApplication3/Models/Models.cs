using WebApplication3.DB.DB_Models;

namespace WebApplication3.Models;


public partial class RegisterParam
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Phone { get; set; }
    public required int Gender { get; set; }
}

