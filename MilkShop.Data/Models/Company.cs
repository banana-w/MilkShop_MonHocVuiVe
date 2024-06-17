using System;
using System.Collections.Generic;

namespace MilkShop.Data.Models;

public partial class Company
{
    public int CompanyId { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyAddress { get; set; }

    public string? CompanyPhone { get; set; }

    public string? CompanyEmail { get; set; }

    public string? Status { get; set; }

    public DateTime? CreateDated { get; set; }

    public int? ProductId { get; set; }

    public string? CompanyFirstName { get; set; }

    public string? CompanyLastName { get; set; }
}
