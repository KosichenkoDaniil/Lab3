using System;
using System.Collections.Generic;
using lab3.Models;
using Microsoft.EntityFrameworkCore;

namespace lab3;

public partial class BankDeposits1Context : DbContext
{
    public BankDeposits1Context()
    {
    }

    public BankDeposits1Context(DbContextOptions<BankDeposits1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Currency> Currencies { get; set; }

    public virtual DbSet<Deposit> Deposits { get; set; }

    public virtual DbSet<Emploee> Emploees { get; set; }

    public virtual DbSet<Exchangerate> Exchangerates { get; set; }

    public virtual DbSet<Investor> Investors { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }
}
