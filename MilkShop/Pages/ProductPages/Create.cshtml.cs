using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkShop.Data.Models;

namespace MilkShop.Pages.ProductPages
{
    public class CreateModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public CreateModel(MilkShop.Data.Models.MilkShopContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["ProductBrandId"] = new SelectList(_context.ProductBrands, "ProductBrandId", "ProductBrandId");
        ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryId");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Products.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
