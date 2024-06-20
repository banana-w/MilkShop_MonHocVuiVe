using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilkShop.Data.Models;

public partial class Customer
{
    [Required]
    public int UserId { get; set; }

    [Required]
    [EmailAddress]
    public string? UserName { get; set; }

    public string? UserEmail { get; set; }

    public string? Status { get; set; }

    [Required]
    public DateOnly? CreatedDate { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }


    [Required]
    [DataType(DataType.Password)]
    public string? Password { get; set; }

    public string? PreferredLanguage { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
