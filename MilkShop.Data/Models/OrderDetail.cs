using System;
using System.Collections.Generic;

namespace MilkShop.Data.Models;

public partial class OrderDetail
{
    public int OrderDetailId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int ProductQuantity { get; set; }

    public decimal? ProductPrice { get; set; }

    public string? Status { get; set; }

    public DateOnly? CreateDate { get; set; }

    public decimal? OrderdetailPrice { get; set; }

    public decimal? ProductDiscount { get; set; }

    public decimal? ProductWeight { get; set; }

    public decimal? ShippingCost { get; set; }

    public decimal? TaxAmount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
