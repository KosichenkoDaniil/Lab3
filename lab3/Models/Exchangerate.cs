using System;
using System.Collections.Generic;

namespace lab3.Models;

public partial class Exchangerate
{
    public int Id { get; set; }

    public DateTime Date { get; set; }

    public int Currencyid { get; set; }

    public decimal Cost { get; set; }

    public virtual Currency Currency { get; set; } = null!;
}
