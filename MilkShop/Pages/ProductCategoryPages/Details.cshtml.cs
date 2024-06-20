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
    public class DetailsModel : PageModel
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public DetailsModel(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }

        public ProductCategory ProductCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productcategory = await _productCategoryBusiness.GetById(id);
            if (productcategory == null)
            {
                return NotFound();
            }
            else
            {
                ProductCategory = productcategory.Data as ProductCategory;
            }
            return Page();
        }
    }
}
