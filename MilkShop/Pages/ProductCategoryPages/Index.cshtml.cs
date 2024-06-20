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
    public class IndexModel : PageModel
    {
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public IndexModel(IProductCategoryBusiness productCategoryBusiness)
        {
            _productCategoryBusiness = productCategoryBusiness;
        }

        [BindProperty]
        public ProductCategory ProductCategory { get; set; } = default!;

        public List<ProductCategory> ProductCategories { get;set; } = default!;

        private List<ProductCategory> GetProductCategories()
        {
            var result = _productCategoryBusiness.GetAll();
            if (result.Status > 0 && result.Result.Data != null)
            {
                var productCate = (List<ProductCategory>)result.Result.Data;
                return productCate;
            }
            return null;
        }

        public async Task OnGetAsync()
        {
            ProductCategories = GetProductCategories();
        }
    }
}
