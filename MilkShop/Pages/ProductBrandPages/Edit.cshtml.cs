using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductBrandBusiness;

namespace MilkShop.Pages.ProductBrandPages
{
    public class EditModel : PageModel
    {
        private readonly IProductBrandBusiness _productBrandBusiness;

        public EditModel(IProductBrandBusiness productBrandBusiness)
        {
           _productBrandBusiness = productBrandBusiness;
        }

        [BindProperty]
        public ProductBrand ProductBrand { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productbrand =  await _productBrandBusiness.GetById((int)id);
            if (productbrand == null)
            {
                return NotFound();
            }
            ProductBrand = (ProductBrand)productbrand.Data;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productBrandBusiness.UpdateAsync(ProductBrand);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_productBrandBusiness.GetById(ProductBrand.ProductBrandId) == null)
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

    }
}
