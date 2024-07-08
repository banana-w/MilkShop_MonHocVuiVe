using System;
using System.Collections.Generic;

namespace MilkShop.Data.Models;

public partial class Customer
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    public string? UserEmail { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Password { get; set; }

    public string? PreferredLanguage { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
