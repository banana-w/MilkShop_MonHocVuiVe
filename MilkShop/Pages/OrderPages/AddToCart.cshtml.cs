using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.Helper;
using MilkShop.Business.OrderBusinesses;
using MilkShop.Data.CartModel;
using MilkShop.Data.Models;

namespace MilkShop.Pages.OrderPages
{
    public class AddToCartModel : PageModel
    {
        private readonly MilkShopContext _context;

        public AddToCartModel(MilkShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int id, int pageIndex, string searchTerm)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            if (pageIndex == 0) pageIndex = 1;

            var cart = HttpContext.Session.GetObjectFromJson<ShoppingCart>("Cart") ?? new ShoppingCart();

            cart.AddToCart(new CartItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductImage = product.ProductImage,
                ProductPrice = product.ProductPrice,
                Quantity = 1 // Default quantity to add to cart
            });
            TempData["SuccessMessage"] = $"{product.ProductName} is added to cart";
            HttpContext.Session.SetInt32("cartQuantity", cart.Items.Count);
            HttpContext.Session.SetObjectAsJson("Cart", cart);

            return RedirectToPage("/ProductPages/Index", new {PageIndex = pageIndex, SearchTerm = searchTerm});
        }
    }
}