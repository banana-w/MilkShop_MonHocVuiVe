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
using MilkShopBusiness.ProductBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class EditModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrand;

        public EditModel(IProductBusiness productBusiness, IProductBrandBusiness productBrandBusiness)
        {
            _productBusiness = productBusiness;
            _productBrand = productBrandBusiness;
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product =  await _productBusiness.GetById((int)id);
            if (product == null)
            {
                return NotFound();
            }
            Product = (Product)product.Data;
            var productBrand = await _productBrand.GetAll();
            ViewData["ProductBrandId"] = new SelectList((System.Collections.IEnumerable)productBrand.Data, "ProductBrandId", "ProductBrandName");
            //ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryId");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _productBusiness.UpdateAsync(Product);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_productBusiness.GetById(Product.ProductId) == null)
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
