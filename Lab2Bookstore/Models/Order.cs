using System;
using System.Collections.Generic;

namespace Lab2Bookstore.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? CustomerId { get; set; }

    public int? StoreId { get; set; }

    public string? BookIspn { get; set; }

    public virtual Book? BookIspnNavigation { get; set; }

    public virtual Customer? Customer { get; set; }

    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();

    public virtual Store? Store { get; set; }
}
