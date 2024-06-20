using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductCategoryPages
{
    public class DeleteModel : PageModel
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public DeleteModel(IProductCategoryBusiness productCategoryBusiness)
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

            var productcategory = await _productCategoryBusiness.GetById(id);

            if(productcategory.Status > 0)
            {
                ProductCategory = productcategory.Data as ProductCategory;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productcategory = await _productCategoryBusiness.DeleteAsync(id);
            if (productcategory.Status > 0)
            {
                ViewData["SuccessMessage"] = productcategory.Message;
                return RedirectToPage("./Index");
            }
            else
            {
                ViewData["ErrorMessage"] = productcategory.Message;
                return Page();
            }
        }
    }
}
