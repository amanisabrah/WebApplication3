using System;
using System.Collections.Generic;

namespace DB;

public partial class AAA_USR_User
{
    public int AAA_USR_ID { get; set; }

    public string? AAA_USR_Name { get; set; }

    public string? AAA_USR_Email { get; set; }

    public int? AAA_USR_Gender { get; set; }

    public string? AAA_USR_Phone { get; set; }

    public string AAA_USR_Password { get; set; } = null!;

    public bool AAA_USR_IsApproved { get; set; }

    public virtual ICollection<AAA_REQ_Requset> AAA_REQ_RequsetAAA_REQ_USRID_EntryNavigation { get; set; } = new List<AAA_REQ_Requset>();

    public virtual ICollection<AAA_REQ_Requset> AAA_REQ_RequsetAAA_REQ_USRID_UpdateNavigation { get; set; } = new List<AAA_REQ_Requset>();
}
