using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Mpay
{
    public int Mid { get; set; }

    public string Mlevel { get; set; } = null!;

    public int Mpay1 { get; set; }

    public int? Pid { get; set; }

    public virtual Post? PidNavigation { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
