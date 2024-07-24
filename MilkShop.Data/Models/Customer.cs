using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilkShop.Data.Models;

public partial class Customer
{
    public int UserId { get; set; }

    public string? UserName { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email format.")]

    public string? UserEmail { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    [RegularExpression(@"^\+?[0-9]\d{1,14}$", ErrorMessage = "Invalid phone number format.")]

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
    public string? Password { get; set; }

    public string? PreferredLanguage { get; set; }

    public string? ImageUrl { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
