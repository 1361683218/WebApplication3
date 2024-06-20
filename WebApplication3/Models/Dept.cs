using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Dept
{
    public int Did { get; set; }

    public string Dname { get; set; } = null!;

    public string? Dtel { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
