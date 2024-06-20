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

namespace MilkShop.Pages.ProductPages
{
    public class CreateModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;

        public CreateModel(MilkShop.Data.Models.MilkShopContext context, IProductBusiness productBusiness, IProductBrandBusiness productBrandBusiness)
        {
            _context = context;
            _productBusiness = productBusiness;
            _productBrandBusiness = productBrandBusiness;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var productBrand = await _productBrandBusiness.GetAll();
            ViewData["ProductBrandId"] = new SelectList((System.Collections.IEnumerable)productBrand.Data, "ProductBrandId", "ProductBrandName");
            ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "ProductCategoryId", "ProductCategoryName");
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
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
