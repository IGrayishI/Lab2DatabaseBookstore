using System;
using System.Collections.Generic;

namespace Lab2Bookstore.Models;

public partial class Book
{
    public string Isbn13 { get; set; } = null!;

    public string? Catagory { get; set; }

    public string? Title { get; set; }

    public string? Language { get; set; }

    public decimal? Price { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public int? AuthorId { get; set; }

    public virtual Author? Author { get; set; }

    public virtual ICollection<Inventory> Inventories { get; set; } = new List<Inventory>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
