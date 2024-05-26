using System;
using System.Collections.Generic;

namespace DB;

public partial class USE_User
{
    public int USE_User_ID { get; set; }

    public string? USE_User_Name { get; set; }

    public string? USE_User_Email { get; set; }

    public int? USE_User_Gender { get; set; }

    public string? USE_User_Phone { get; set; }

    public string USE_User_Password { get; set; } = null!;

    public bool USE_User_IsApproved { get; set; }
}
