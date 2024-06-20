using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;

namespace MilkShop.Pages.OrderPages
{
    public class IndexModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;
        private readonly MilkShop.Business.OrderBusinesses.IOrderBusiness _orderBusiness;

        public IndexModel(MilkShop.Data.Models.MilkShopContext context, Business.OrderBusinesses.IOrderBusiness orderBusiness)
        {
            _context = context;
            _orderBusiness = orderBusiness;
        }

        public IList<Order> Order { get;set; } = default!;

        public async Task OnGetAsync()
        {
            //Order = await _context.Orders
            //    .Include(o => o.User).ToListAsync();
            //Order = (IList<Order>)await _orderBusiness.GetAll();
            var temp = await _orderBusiness.GetAll();
            if(temp != null && temp.Data != null)
            {
                Order = (IList<Order>)temp.Data;
            }
            else
            {
                Order = new List<Order>();
            }
        }
    }
}
