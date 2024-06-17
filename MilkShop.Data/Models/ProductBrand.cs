using System;
using System.Collections.Generic;

namespace MilkShop.Data.Models;

public partial class ProductBrand
{
    public int ProductBrandId { get; set; }

    public string? ProductBrandName { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? Description { get; set; }

    public string? LogoUrl { get; set; }

    public string? WebsiteUrl { get; set; }

    public string? CountryOfOrigin { get; set; }

    public string? ContactEmail { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
