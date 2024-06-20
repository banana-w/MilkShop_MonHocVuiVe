using System;
using System.Collections.Generic;

namespace MilkShop.Data.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateTime? OrderDate { get; set; }

    public string? OrderStatus { get; set; }

    public decimal? OrderTotalAmount { get; set; }

    public int? UserId { get; set; }

    public string? PaymentMethodId { get; set; }

    public string? PaymentStatus { get; set; }

    public string? Status { get; set; }

    public string? CreatedDate { get; set; }

    public string? ShippingAddress { get; set; }

    public string? BillingAddress { get; set; }

    public string? ShippingMethod { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual Customer? User { get; set; }
}
