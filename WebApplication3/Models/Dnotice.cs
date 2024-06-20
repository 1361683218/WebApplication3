using System;
using System.Collections.Generic;

namespace WebApplication3.Models;

public partial class Dnotice
{
    public int Nuid { get; set; }

    public int Npost { get; set; }

    public int Nposto { get; set; }

    public string Naddtime { get; set; } = null!;

    public string Ncontime { get; set; } = null!;

    public virtual Post NpostNavigation { get; set; } = null!;

    public virtual Post NpostoNavigation { get; set; } = null!;

    public virtual User Nu { get; set; } = null!;
}
