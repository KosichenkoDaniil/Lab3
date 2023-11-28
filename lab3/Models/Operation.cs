using System;
using System.Collections.Generic;

namespace lab3.Models;

public partial class Operation
{
    public int Id { get; set; }

    public int Investorsid { get; set; }

    public DateTime Depositdate { get; set; }

    public DateTime Returndate { get; set; }

    public int Depositid { get; set; }

    public decimal Depositamount { get; set; }

    public decimal Refundamount { get; set; }

    public bool Returnstamp { get; set; }

    public int Emploeeid { get; set; }

    public virtual Deposit Deposit { get; set; } = null!;

    public virtual Emploee Emploee { get; set; } = null!;

    public virtual Investor Investors { get; set; } = null!;
}
