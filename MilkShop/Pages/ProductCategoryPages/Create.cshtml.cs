using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductCategoryPages
{
    public class CreateProductCategory : PageModel
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public CreateProductCategory(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var result = await _productCategoryBusiness.Save(ProductCategory);
            if(result != null)
            {
                ViewData["SuccessMessage"] = result.Message;
                return RedirectToPage("./Index");
            }
            else
            {
                ViewData["ErrorMessage"] = $"Error: {result.Message}";
                return Page();
            }
        }
    }
}
