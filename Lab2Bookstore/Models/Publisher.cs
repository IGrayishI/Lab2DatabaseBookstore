using System;
using System.Collections.Generic;

namespace Lab2Bookstore.Models;

public partial class Publisher
{
    public int PublisherId { get; set; }

    public string? Name { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }
}
