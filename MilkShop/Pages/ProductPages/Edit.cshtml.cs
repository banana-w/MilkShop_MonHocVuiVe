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
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class EditModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public EditModel(IProductBusiness productBusiness, IProductBrandBusiness productBrandBusiness, 
            IProductCategoryBusiness productCategoryBusiness)
        {
            _productBusiness = productBusiness;
            _productBrandBusiness = productBrandBusiness;
            _productCategoryBusiness = productCategoryBusiness;
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
            var productBrand = await _productBrandBusiness.GetAll();
            var productCate = await _productCategoryBusiness.GetCategories();
            ViewData["ProductBrandId"] = new SelectList((System.Collections.IEnumerable)productBrand.Data, "ProductBrandId", "ProductBrandName");
            ViewData["ProductCategoryId"] = new SelectList((System.Collections.IEnumerable)productCate.Data, "ProductCategoryId", "ProductCategoryName");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var result = await _productBusiness.UpdateAsync(Product);
                if (result.Status > 0)
                {
                    TempData["SuccessMessage"] = result.Message;
                    return RedirectToPage("./Index");
                }
                else
                {
                    TempData["ErrorMessage"] = result.Message;
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_productBusiness.FindId(Product.ProductId) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
       }
    }
}
