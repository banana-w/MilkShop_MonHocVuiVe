using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkShop.Common;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductBrandBusiness;

namespace MilkShop.Pages.ProductBrandPages
{
    public class CreateModel : PageModel
    {
        private readonly IProductBrandBusiness _productBrandBusiness;

        public CreateModel(IProductBrandBusiness productBrandBusiness)
        {
            _productBrandBusiness = productBrandBusiness;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ProductBrand ProductBrand { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _productBrandBusiness.Save(ProductBrand);
            if (result.Status != Const.SUCCESS_CREATE_CODE)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
