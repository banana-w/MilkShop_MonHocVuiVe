using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;

namespace MilkShop.Pages.ProductBrandPages
{
    public class IndexModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public IndexModel(MilkShop.Data.Models.MilkShopContext context)
        {
            _context = context;
        }

        public IList<ProductBrand> ProductBrand { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductBrand = await _context.ProductBrands.ToListAsync();
        }
    }
}
