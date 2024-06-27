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
using MilkShopBusiness.ProductBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;

        public IndexModel(IProductBusiness productBusiness)
        {
            _productBusiness = productBusiness;
        }
        public Paginate<Product> Product { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
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
            var result = await _productBusiness.Search(SearchTerm, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var product = result.Data;
                return (Paginate<Product>)product;
            }
            return null;
        }
        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Product = await Search();
            }
            else
            {
                Product = await GetProduct();
            }
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
        }
    }
}
