using System;
using System.Collections.Generic;

namespace DB;

public partial class USE_TYP_TypeOfRequset
{
    public int USE_TYP_ID { get; set; }

    public string? USE_TYP_MEssage { get; set; }

    public int? USE_TYP_UserID { get; set; }

    public virtual USE_User? USE_TYP_User { get; set; }
}
