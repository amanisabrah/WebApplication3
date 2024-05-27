using System;
using System.Collections.Generic;

namespace DB;

public partial class AAA_REQ_Requset
{
    public int AAA_REQ_ID { get; set; }

    public string? AAA_REQ_Message { get; set; }

    public int? AAA_REQ_USRID_Entry { get; set; }

    public int? AAA_REQ_USRID_Update { get; set; }

    public DateTime? AAA_REQ_EntryDate { get; set; }

    public virtual AAA_USR_User? AAA_REQ_USRID_EntryNavigation { get; set; }

    public virtual AAA_USR_User? AAA_REQ_USRID_UpdateNavigation { get; set; }
}
