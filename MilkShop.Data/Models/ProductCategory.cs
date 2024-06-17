using System;
using System.Collections.Generic;

namespace MilkShop.Data.Models;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    public string? ProductCategoryName { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    public string? ProductCategoryDescription { get; set; }

    public DateOnly? UpdatedDate { get; set; }

    public string? ThumbnailImage { get; set; }

    public int? CategoryLevel { get; set; }

    public string? MetaKeywords { get; set; }

    public string? CategoryCode { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
