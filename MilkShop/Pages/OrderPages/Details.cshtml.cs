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
    public class DetailsModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;
        private readonly MilkShop.Business.OrderBusinesses.IOrderBusiness _orderBusiness;

        public DetailsModel(MilkShop.Data.Models.MilkShopContext context, Business.OrderBusinesses.IOrderBusiness orderBusiness)
        {
            _context = context;
            _orderBusiness = orderBusiness;
        }

        public Order Order { get; set; } = default!;
        public IEnumerable<OrderDetail> OrderDetails { get; set; } = default!;
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var order = await _context.Orders.FirstOrDefaultAsync(m => m.OrderId == id);
            //var order = (Order)(await _orderBusiness.GetById(id.Value)).Data;
            var order = (Order)(await _orderBusiness.GetById(id.Value)).Data;

            if (order == null)
            {
                return NotFound();
            }
            else
            {
                Order = order;
                OrderDetails = await _context.OrderDetails.Where(od => od.OrderId == order.OrderId).ToListAsync();
            }
            return Page();
        }
    }
}
