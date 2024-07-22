using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;
using MilkShopBusiness.ProductBrandBusiness;
using MilkShopBusiness.ProductBusiness;
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public IndexModel(IProductBusiness productBusiness, IProductCategoryBusiness productCategoryBusiness, IProductBrandBusiness productBrandBusiness)
        {
            _productBusiness = productBusiness;
            _productCategoryBusiness = productCategoryBusiness;
            _productBrandBusiness = productBrandBusiness;

        }
        public Paginate<Product> Product { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int SortPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int BrandId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CateId { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal Price { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 6;

        private async Task<Paginate<Product>> GetProduct()
        {
            var result = await _productBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var product = result.Data;
                return (Paginate<Product>)product;
            }
            return null;
        }
        private async Task<Paginate<Product>> Search()
        {
            var result = await _productBusiness.Search(SearchTerm, BrandId, CateId, Price, PageIndex, Size, SortPrice);
            if (result.Status > 0 && result.Data != null)
            {
                var product = result.Data;
                return (Paginate<Product>)product;
            }
            return null;
        }
        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm) || BrandId != 0 || CateId != 0 || Price != default || SortPrice != 0)
            {
                Product = await Search();
            }
            else
            {
                Product = await GetProduct();
            }
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            var productBrand = await _productBrandBusiness.GetAll();
            var productCate = await _productCategoryBusiness.GetCategories();
            ViewData["ProductBrandId"] = new SelectList((System.Collections.IEnumerable)productBrand.Data, "ProductBrandId", "ProductBrandName");
            ViewData["ProductCategoryId"] = new SelectList((System.Collections.IEnumerable)productCate.Data, "ProductCategoryId", "ProductCategoryName");
            ViewData["SortEnum"] = new SelectList(new[] {
                                        new { Value = "1", Text = "Ascending Price" },
                                        new { Value = "2", Text = "Descending Price" }}, "Value", "Text");
        }
    }
}
