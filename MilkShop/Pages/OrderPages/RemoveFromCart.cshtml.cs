using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkShop.Business.Helper;
using MilkShop.Business.OrderBusinesses;
using MilkShop.Data.CartModel;

namespace MilkShop.Pages.OrderPages
{
    public class RemoveFromCartModel : PageModel
    {
        public IActionResult OnGet(int id)
        {
            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            // Ki?m tra xem s?n ph?m có trong gi? hàng không
            var cartItem = cart.Items.Find(item => item.ProductId == id);
            if (cartItem != null)
            {
                cart.RemoveFromCart(id);
                HttpContext.Session.SetObjectAsJson("Cart", cart);
            }

            return RedirectToPage("ViewCart");
        }
    }
}