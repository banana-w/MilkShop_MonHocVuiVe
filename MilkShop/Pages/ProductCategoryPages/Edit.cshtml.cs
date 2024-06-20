using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductCategoryPages
{
    public class EditModel : PageModel
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public EditModel(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productcategory =  await _productCategoryBusiness.GetById(id);
            if (productcategory == null)
            {
                return NotFound();
            }
            ProductCategory = productcategory.Data as ProductCategory;
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

            try
            {
                await _productCategoryBusiness.UpdateAsync(ProductCategory);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ProductCategoryExists(ProductCategory.ProductCategoryId))
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

        private async Task<bool> ProductCategoryExists(int id)
        {
            var productCategory = await _productCategoryBusiness.GetById(id);
            return productCategory != null && productCategory.Data != null;
        }
    }
}
