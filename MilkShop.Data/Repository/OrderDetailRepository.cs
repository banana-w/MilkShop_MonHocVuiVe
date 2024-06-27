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
    public class OrderDetailRepository : GenericRepository<OrderDetail>
    {
        public OrderDetailRepository() { }

        public async Task<List<OrderDetail>> GetByOrderId(int id)
        {
            return await _context.OrderDetails.Where(x => x.OrderId == id).ToListAsync();
        }
    }
}
