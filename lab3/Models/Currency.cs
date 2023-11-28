using System;
using System.Collections.Generic;

namespace lab3.Models;

public partial class Currency
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Country { get; set; } = null!;

    public virtual ICollection<Deposit> Deposits { get; set; } = new List<Deposit>();

    public virtual ICollection<Exchangerate> Exchangerates { get; set; } = new List<Exchangerate>();
}
