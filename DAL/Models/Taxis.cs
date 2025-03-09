using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Taxis
{
    public int TaxId { get; set; }

    public string TaxName { get; set; } = null!;

    public decimal TaxAmount { get; set; }

    public bool? IsEnabled { get; set; }

    public bool? IsDefault { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedAt { get; set; }

    public string[]? TaxType { get; set; }

    public virtual ICollection<Order> Orders { get; } = new List<Order>();
}
