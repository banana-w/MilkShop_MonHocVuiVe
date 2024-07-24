using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MilkShop.Data.Models;

public partial class ProductCategory
{
    public int ProductCategoryId { get; set; }

    [Required(ErrorMessage = "Product Category Name is required.")]
    public string? ProductCategoryName { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    public string? Status { get; set; }

    [Required(ErrorMessage = "Created Date is required.")]
    public DateOnly? CreatedDate { get; set; }

    [Required(ErrorMessage = "Product Category Description is required.")]
    public string? ProductCategoryDescription { get; set; }

    [Required(ErrorMessage = "Updated Date is required.")]
    public DateOnly? UpdatedDate { get; set; }

    [Required(ErrorMessage = "Thumbnail Image is required.")]
    public string? ThumbnailImage { get; set; }

    [Required(ErrorMessage = "Category Level is required.")]
    public int? CategoryLevel { get; set; }

    [Required(ErrorMessage = "Meta Keywords are required.")]
    public string? MetaKeywords { get; set; }

    [Required(ErrorMessage = "Category Code is required.")]
    public string? CategoryCode { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
