using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;

namespace MilkShop.Pages.ProductBrandPages
{
    public class EditModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public EditModel(MilkShop.Data.Models.MilkShopContext context)
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

            var productbrand =  await _context.ProductBrands.FirstOrDefaultAsync(m => m.ProductBrandId == id);
            if (productbrand == null)
            {
                return NotFound();
            }
            ProductBrand = productbrand;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(ProductBrand).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductBrandExists(ProductBrand.ProductBrandId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ProductBrandExists(int id)
        {
            return _context.ProductBrands.Any(e => e.ProductBrandId == id);
        }
    }
}
