using System;
using System.Collections.Generic;

namespace DAL.Models;

public partial class Table
{
    public int TableId { get; set; }

    public string TableName { get; set; } = null!;

    public int? SectionId { get; set; }

    public int Capacity { get; set; }

    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public int ModifiedBy { get; set; }

    public DateTime ModifiedAt { get; set; }

    public string[]? Status { get; set; }

    public virtual ICollection<CustomerTable> CustomerTables { get; } = new List<CustomerTable>();

    public virtual ICollection<Customer> Customers { get; } = new List<Customer>();

    public virtual ICollection<OrderTable> OrderTables { get; } = new List<OrderTable>();

    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    public virtual Section? Section { get; set; }

    public virtual ICollection<WaitingList> WaitingLists { get; } = new List<WaitingList>();
}
