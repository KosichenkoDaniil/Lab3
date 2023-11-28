using System;
using System.Collections.Generic;

namespace lab3.Models;

public partial class Emploee
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Middlename { get; set; } = null!;

    public string Post { get; set; } = null!;

    public DateTime Dob { get; set; }

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
