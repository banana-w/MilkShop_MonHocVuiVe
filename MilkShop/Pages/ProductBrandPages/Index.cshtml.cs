using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;
using MilkShopBusiness.ProductBrandBusiness;

namespace MilkShop.Pages.ProductBrandPages
{
    public class IndexModel : PageModel
    {
        private readonly IProductBrandBusiness _productBrandBusiness;

        public IndexModel(IProductBrandBusiness productBrandBusiness)
        {
            _productBrandBusiness = productBrandBusiness;
        }
        public Paginate<ProductBrand> ProductBrand { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;

        private async Task<Paginate<ProductBrand>> GetProductBrands()
        {
            var result = await _productBrandBusiness.GetPagingList(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var productBrands = result.Data;
                return (Paginate<ProductBrand>)productBrands;
            }
            return null;
        }
        private async Task<Paginate<ProductBrand>> Search()
        {
            var result = await _productBrandBusiness.Search(SearchTerm, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var productBrands = result.Data;
                return (Paginate<ProductBrand>)productBrands;
            }
            return null;
        }
        public async Task OnGetAsync()
        {   
            if(!string.IsNullOrEmpty(SearchTerm))
            {
                ProductBrand = await Search();
            }
            else
            {
                ProductBrand = await GetProductBrands();
            }
        }
    }
}
