using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using MilkShop.Business.Helper;
using MilkShop.Business.OrderBusinesses;
using MilkShop.Data.CartModel;
using MilkShop.Data.Models;
using MilkShop.Data.VNPay;
using Newtonsoft.Json;
using WebAPI.Util;

namespace MilkShop.Pages.OrderPages
{
    public class CreateModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;
        private readonly MilkShop.Business.OrderBusinesses.IOrderBusiness _orderBusiness;
        private readonly MilkShop.Business.OrderDetailBusinesses.IOrderDetailBusiness _orderDetailBusiness;
        private readonly VNPaySettings _vnPaySettings;

        public int UserID { get; set; }

        public CreateModel(MilkShop.Data.Models.MilkShopContext context, MilkShop.Business.OrderBusinesses.IOrderBusiness orderBusiness,
            MilkShop.Business.OrderDetailBusinesses.IOrderDetailBusiness orderDetailBusiness, IOptions<VNPaySettings> vnPaySettings)
        {
            _context = context;
            _orderBusiness = orderBusiness;
            _orderDetailBusiness = orderDetailBusiness;
            _vnPaySettings = vnPaySettings.Value;
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
            ViewData["Message"] = TempData["Message"];
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
            var url = await Payment("100000", "thanh toan");
            // Xóa giỏ hàng sau khi tạo đơn hàng thành công
            //HttpContext.Session.Remove("Cart");
            //HttpContext.Session.Remove("cartQuantity");

            TempData.Remove("CartData");
            TempData.Remove("TotalAmount");
            return Redirect(url);
        }
        public async Task<string> Payment(string amount, string infor)
        {
            // find order in table order and checking exits

            string orderinfor = DateTime.Now.Ticks.ToString();
            string hostName = System.Net.Dns.GetHostName();
            string clientIPAddress = System.Net.Dns.GetHostAddresses(hostName).GetValue(0).ToString();
            VNPayHelper pay = new VNPayHelper();
            amount += "00";
            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode",
                _vnPaySettings
                    .TmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount",
                amount); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            // pay.AddRequestData("vnp_BankCode",
            //     "");
            //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate",
                DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", clientIPAddress); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", infor); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType",
                "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl",
                _vnPaySettings.ReturnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", orderinfor); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(_vnPaySettings.Url, _vnPaySettings.HashSecret);
            return paymentUrl;
        }

    }
}
