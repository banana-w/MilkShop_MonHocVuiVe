using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkShop.Business.Helper;
using MilkShop.Business.OrderBusinesses;
using MilkShop.Data.CartModel;
using MilkShop.Data.Models;
using Newtonsoft.Json;

namespace MilkShop.Pages.OrderPages
{
    public class CreateModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;
        private readonly MilkShop.Business.OrderBusinesses.IOrderBusiness _orderBusiness;
        private readonly MilkShop.Business.OrderDetailBusinesses.IOrderDetailBusiness _orderDetailBusiness;
        public int UserID { get; set; }

        public CreateModel(MilkShop.Data.Models.MilkShopContext context, MilkShop.Business.OrderBusinesses.IOrderBusiness orderBusiness,
            MilkShop.Business.OrderDetailBusinesses.IOrderDetailBusiness orderDetailBusiness)
        {
            _context = context;
            _orderBusiness = orderBusiness;
            _orderDetailBusiness = orderDetailBusiness;
        }

        public IActionResult OnGet()
        {
            var cartData = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            var totalAmount = HttpContext.Session.GetObjectFromJson<decimal>("TotalAmount");
            var userIdFromSession = HttpContext.Session.GetInt32("UserID");
            if (userIdFromSession.HasValue)
            {
                UserID = userIdFromSession.Value;
            }
            TempData["CartData"] = cartData;
            TempData["TotalAmount"] = totalAmount;


            //TempData["Cart"] = JsonConvert.SerializeObject(cartData);

            ViewData["UserId"] = new SelectList(_context.Customers, "UserId", "UserId");
            return Page();
        }

        [BindProperty]
        public Order Order { get; set; } = default!;
        [BindProperty]
        public string Cart { get; set; } = string.Empty;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //List<CartItem> cartItems = JsonConvert.DeserializeObject<List<CartItem>>(Cart);
            var cartData = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart");
            var totalAmount = HttpContext.Session.GetObjectFromJson<decimal>("TotalAmount");
            var userIdFromSession = HttpContext.Session.GetInt32("UserID");
            if (userIdFromSession.HasValue)
            {
                UserID = userIdFromSession.Value;
            }


            TempData["CartData"] = cartData;
            TempData["TotalAmount"] = totalAmount;
            
            //if (!ModelState.IsValid)
            //{
            //    return Page();
            //}

            // Giải mã dữ liệu giỏ hàng từ Cart

            var x = UserID;
            // Tạo đối tượng Order mới
            Order newOrder = new Order
            {
                UserId = UserID,
                OrderDate = DateTime.Now,
                OrderTotalAmount = totalAmount,
                OrderStatus = Order.Status,
                PaymentMethodId = Order.PaymentMethodId,
                PaymentStatus = Order.PaymentStatus,
                Status = Order.Status,
                CreatedDate = Order.CreatedDate,
                ShippingAddress = Order.ShippingAddress,
                BillingAddress = Order.BillingAddress,
                ShippingMethod = Order.ShippingMethod,
                // Thiết lập các thuộc tính khác của Order nếu có
            };

            // Lưu đơn hàng mới vào cơ sở dữ liệu
            await _orderBusiness.Save(newOrder);

            // Tạo các OrderDetail từ cartItems
            List<OrderDetail> orderDetails = new List<OrderDetail>();
            foreach (var item in cartData.Items)
            {
                orderDetails.Add(new OrderDetail
                {
                    OrderId = newOrder.OrderId,
                    ProductId = item.ProductId,
                    ProductQuantity = item.Quantity,
                    ProductPrice = item.ProductPrice,
                    Status = "Pending",
                    CreateDate = DateOnly.FromDateTime(DateTime.Now),
                    OrderdetailPrice = item.ProductPrice * item.Quantity,
                    ProductDiscount = 0,
                    ProductWeight = 0,
                    ShippingCost = 0,
                    TaxAmount = 0,
                    
                });
            }

            // Lưu các OrderDetail vào cơ sở dữ liệu
            foreach (var orderDetail in orderDetails)
            {
                await _orderDetailBusiness.Add(orderDetail);
            }

            // Xóa giỏ hàng sau khi tạo đơn hàng thành công
            HttpContext.Session.Remove("Cart");
            HttpContext.Session.Remove("cartQuantity");

            TempData.Remove("CartData");
            TempData.Remove("TotalAmount");
            return RedirectToPage("./Index");
        }
    }
}
