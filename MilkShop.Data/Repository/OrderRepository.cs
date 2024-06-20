using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Base;
using MilkShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Data.Repository
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository() { }
        public async Task<List<Order>> GetListOrder()
        {
            return await _context.Orders.Include(x => x.User).ToListAsync();
        }
    }
}
