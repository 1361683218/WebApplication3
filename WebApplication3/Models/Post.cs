using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Post
{
    public int Pid { get; set; }

    public string Pname { get; set; } = null!;

    public int? Did { get; set; }

    public virtual Dept? DidNavigation { get; set; }

    public virtual ICollection<Dnotice> DnoticeNpostNavigations { get; set; } = new List<Dnotice>();

    public virtual ICollection<Dnotice> DnoticeNpostoNavigations { get; set; } = new List<Dnotice>();

    public virtual ICollection<Mpay> Mpays { get; set; } = new List<Mpay>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
