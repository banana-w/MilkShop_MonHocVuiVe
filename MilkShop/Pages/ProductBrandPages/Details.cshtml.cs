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
    public class DetailsModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public DetailsModel(MilkShop.Data.Models.MilkShopContext context)
        {
            _context = context;
        }

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
    }
}
