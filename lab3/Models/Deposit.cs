using System;
using System.Collections.Generic;
using lab3.Models;

namespace lab3;

public partial class Deposit
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public double Term { get; set; }

    public decimal Mindepositamount { get; set; }

    public int Currencyid { get; set; }

    public decimal Rate { get; set; }

    public string? Additionalconditions { get; set; }

    public virtual Currency Currency { get; set; } = null!;

    public virtual ICollection<Operation> Operations { get; set; } = new List<Operation>();
}
