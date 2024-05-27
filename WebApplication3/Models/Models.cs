using DB;
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

public partial class UpdateUserBYIDParam
{
    public required int USEID { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Phone { get; set; }
    public required int Gender { get; set; }
}

public partial class UpdateReqParm
{
    public int ID { get; set; }

    public string? Message { get; set; }

    public int? UserID { get; set; }

}

public partial class CreatReqParm
{

    public string? Message { get; set; }

    public int? UserEntry { get; set; }//admin
    public int? USRID_Update { get; set; }


}

