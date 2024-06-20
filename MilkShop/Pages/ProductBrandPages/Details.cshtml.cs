using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductBrandBusiness;

namespace MilkShop.Pages.ProductBrandPages
{
    public class DetailsModel : PageModel
    {
        private readonly IProductBrandBusiness _productBrandBusiness;

        public DetailsModel(IProductBrandBusiness productBrandBusiness)
        {
            _productBrandBusiness = productBrandBusiness;
        }

        public ProductBrand ProductBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productbrand = await _productBrandBusiness.GetById((int)id);
            if (productbrand == null)
            {
                return NotFound();
            }
            else
            {
                ProductBrand = (ProductBrand)productbrand.Data;
            }
            return Page();
        }
    }
}
