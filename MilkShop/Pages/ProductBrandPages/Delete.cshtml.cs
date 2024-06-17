using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;

namespace MilkShop.Pages.ProductBrandPages
{
    public class DeleteModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public DeleteModel(MilkShop.Data.Models.MilkShopContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductBrand ProductBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productbrand = await _context.ProductBrands.FirstOrDefaultAsync(m => m.ProductBrandId == id);

            if (productbrand == null)
            {
                return NotFound();
            }
            else
            {
                ProductBrand = productbrand;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productbrand = await _context.ProductBrands.FindAsync(id);
            if (productbrand != null)
            {
                ProductBrand = productbrand;
                _context.ProductBrands.Remove(ProductBrand);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
