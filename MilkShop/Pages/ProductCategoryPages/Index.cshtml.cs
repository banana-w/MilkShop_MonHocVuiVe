using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;
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
        public Paginate<ProductCategory> ProductCategory { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;

        private async Task<Paginate<ProductCategory>> GetProductCategories()
        {
            var result = await _productCategoryBusiness.GetAll(Size, PageIndex);
            if (result.Status > 0 && result.Data != null)
            {
                var productCate = (Paginate<ProductCategory>)result.Data;                
                return productCate;
            }
            return null;
        }

        private async Task<Paginate<ProductCategory>> SearchProductCategories()
        {
            var result = await _productCategoryBusiness.Search(SearchTerm, Size, PageIndex);
            if (result.Status > 0 && result.Data != null)
            {
                var productCate = (Paginate<ProductCategory>)result.Data;
                return productCate;
            }
            return null;
        }

        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                ProductCategory = await SearchProductCategories();
            }
            else
            {
                ProductCategory = await GetProductCategories();
            }
        }
    }
}
