using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class User
{
    public int Uid { get; set; }

    public string Uname { get; set; } = null!;

    public string Usex { get; set; } = null!;

    public string Urzsj { get; set; } = null!;

    public string? Utel { get; set; }

    public string Ustatus { get; set; } = null!;

    public string? Uadress { get; set; }

    public int? Pid { get; set; }

    public int? Did { get; set; }

    public int? Mid { get; set; }

    public string Usfzh { get; set; } = null!;

    public string? Umail { get; set; }

    public virtual Dept? DidNavigation { get; set; }

    public virtual Dnotice? Dnotice { get; set; }

    public virtual Mpay? MidNavigation { get; set; }

    public virtual Post? PidNavigation { get; set; }
}
