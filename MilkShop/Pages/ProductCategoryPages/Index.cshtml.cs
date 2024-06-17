using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;

namespace MilkShop.Pages.ProductCategoryPages
{
    public class IndexModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;

        public IndexModel(MilkShop.Data.Models.MilkShopContext context)
        {
            _context = context;
        }

        public IList<ProductCategory> ProductCategory { get;set; } = default!;

        public async Task OnGetAsync()
        {
            ProductCategory = await _context.ProductCategories.ToListAsync();
        }
    }
}
