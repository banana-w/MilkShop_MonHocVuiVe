using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using MilkShop.Data.VNPay;
using System.Net.Mail;
using System.Net;
using System.Web;
using WebAPI.Util;

namespace MilkShop.Pages.OrderPages
{
    public class VnPayModel : PageModel
    {
        private readonly VNPaySettings _vnPaySettings;
        public VnPayModel(IOptions<VNPaySettings> vnPaySettings)
        {
            _vnPaySettings = vnPaySettings.Value;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            string status = await PaymentConfirm();
            if (status == "200Ok")
            {
                TempData["Message"] = "Thanh toán thành công";
                // Xóa giỏ hàng sau khi tạo đơn hàng thành công
                HttpContext.Session.Remove("Cart");
                HttpContext.Session.Remove("cartQuantity");

                TempData.Remove("CartData");
                TempData.Remove("TotalAmount");
                return RedirectToPage("/OrderPages/Index");
            } else
            {
                TempData["Message"] = "Bạn đã hủy thanh toán";
               return RedirectToPage("/OrderPages/Create");
            }
        }
        public async Task<string> PaymentConfirm()
        {
            if (Request.QueryString.HasValue)
            {
                //lấy toàn bộ dữ liệu trả về
                var queryString = Request.QueryString.Value;
                var json = HttpUtility.ParseQueryString(queryString);

                long orderId = Convert.ToInt64(json["vnp_TxnRef"]); //mã hóa đơn
                string orderInfor = json["vnp_OrderInfo"].ToString(); //Thông tin giao dịch
                long vnpayTranId = Convert.ToInt64(json["vnp_TransactionNo"]); //mã giao dịch tại hệ thống VNPAY
                string
                    vnp_ResponseCode =
                        json["vnp_ResponseCode"]
                            .ToString(); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = json["vnp_SecureHash"].ToString(); //hash của dữ liệu trả về
                var pos = Request.QueryString.Value.IndexOf("&vnp_SecureHash");

                bool checkSignature = ValidateSignature(Request.QueryString.Value.Substring(1, pos - 1), vnp_SecureHash,
                    _vnPaySettings.HashSecret); //check chữ ký đúng hay không?
                if (checkSignature && _vnPaySettings.TmnCode == json["vnp_TmnCode"].ToString())
                {
                    // Sử dụng orderInfo chứ k phải orderId vì nó là id của vnpay
                    // Demo xử lý Order
                    // Order order = await _orderRepository.GetByOrderIdAsync((int)orderInfor);
                    if (vnp_ResponseCode == "00")
                    {
                        
                            using (var client = new SmtpClient("smtp.gmail.com"))
                            {
                                client.Port = 587;
                                client.Credentials = new NetworkCredential("dokhoa031103@gmail.com", "eint cjww oxel jzxq");
                                client.EnableSsl = true;

                                var message = new MailMessage
                                {
                                    From = new MailAddress("dokhoa031103@gmail.com"),
                                    Subject = "Confirm Order",
                                    Body = "Thank you for your order !",
                                    IsBodyHtml = false,
                                };
                                message.To.Add("phuonghiepthuan56@gmail.com");

                                client.Send(message);
                            }

                            
                        
                        
                        // Payment successful
                        // var transaction = await _transactionRepository.GetByIdAsync((int)orderInfor);
                        // transaction.Status = true;
                        // await _transactionRepository.UpdateAsync(transaction);
                        // order.Status = 1; // assuming '1' is the status code for successful payment
                        // await _orderRepository.UpdateOrderAsync(order);

                        return "200Ok";
                        // return Redirect("localhosst");
                    }
                    else
                    {
                        // Payment failed
                        // order.Status = 3; // assuming '3' is the status code for failed payment
                        // await _orderRepository.UpdateOrderAsync(order);
                        return $"Payment Required. Error Code: {vnp_ResponseCode}";
                    }
                }
                else
                {
                    return "đường dẫn nếu phản hồi ko hợp lệ";
                }
            }

            //phản hồi không hợp lệ
            return "An error occurred while processing your request.";
        }

        private bool ValidateSignature(string rspraw, string inputHash, string secretKey)
        {
            string myChecksum = VNPayHelper.HmacSHA512(secretKey, rspraw);
            return myChecksum.Equals(inputHash, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
