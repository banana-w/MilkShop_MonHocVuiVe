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
using MilkShopBusiness.ProductBusiness;
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class CreateModel : PageModel
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;

        public CreateModel(IProductCategoryBusiness productCategoryBusiness, IProductBusiness productBusiness, 
            IProductBrandBusiness productBrandBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
            _productBusiness = productBusiness;
            _productBrandBusiness = productBrandBusiness;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var productBrand = await _productBrandBusiness.GetAll();
            var productCate = await _productCategoryBusiness.GetCategories();
            ViewData["ProductBrandId"] = new SelectList((System.Collections.IEnumerable)productBrand.Data, "ProductBrandId", "ProductBrandName");
            ViewData["ProductCategoryId"] = new SelectList((System.Collections.IEnumerable)productCate.Data, "ProductCategoryId", "ProductCategoryName");
            return Page();
        }

        [BindProperty]
        public Product Product { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Product.CreatedDate = DateOnly.FromDateTime(DateTime.Now);
            var result = await _productBusiness.Save(Product);
            if (result.Status != Const.SUCCESS_CREATE_CODE)
            {
                TempData["ErrorMessage"] = result.Message;
                return Page();
            }
            TempData["SuccessMessage"] = result.Message;

            return RedirectToPage("./Index");
        }
    }
}
