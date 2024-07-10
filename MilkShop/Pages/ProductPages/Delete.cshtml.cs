using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;

        public DeleteModel(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productBusiness.GetById((int)id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = (Product)product.Data;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _productBusiness.FindId((int)id);
            if (product != null)
            {
                Product = (Product)product.Data;
                Product.Status = "Deactive";
                await _productBusiness.UpdateAsync(Product);
            }

            return RedirectToPage("./Index");
        }
    }
}
