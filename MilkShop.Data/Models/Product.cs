using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilkShop.Data.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Price must be greater than zero.")]
    public decimal ProductPrice { get; set; }

    public string? ProductImage { get; set; }

    public int? ProductCategoryId { get; set; }

    public int? ProductBrandId { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreatedDate { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Stock must be greater than zero.")]
    public int StockQuantity { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ProductBrand? ProductBrand { get; set; }

    public virtual ProductCategory? ProductCategory { get; set; }
}
