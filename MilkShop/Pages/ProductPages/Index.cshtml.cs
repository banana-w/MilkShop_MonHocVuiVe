using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;

namespace MilkShop.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public IndexModel(MilkShop.Data.Models.MilkShopContext context)
        {
            _context = context;
        }

        public IList<Product> Product { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Product = await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductCategory).ToListAsync();
        }
    }
}
