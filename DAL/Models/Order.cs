using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? TableId { get; set; }

    public int? TaxId { get; set; }

    public DateOnly Date { get; set; }

    public int? ReviewId { get; set; }

    public string? Comment { get; set; }

    public decimal SubTotal { get; set; }

    public int NoOfPerson { get; set; }

    public decimal TotalAmount { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedAt { get; set; }

    public string[]? Status { get; set; }

    public string[]? PaymentMode { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<OrderTable> OrderTables { get; } = new List<OrderTable>();

    public virtual Review? Review { get; set; }

    public virtual Table? Table { get; set; }

    public virtual Taxis? Tax { get; set; }
}
