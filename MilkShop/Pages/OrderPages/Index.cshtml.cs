using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;

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

        public Paginate<Order> Order { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]

        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;


        private async Task<Paginate<Order>> GetOrder()
        {
            var result = await _orderBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var order = result.Data;
                return (Paginate<Order>)order;
            }
            return null;
        }

        private async Task<Paginate<Order>> Search()
        {
            var result = await _orderBusiness.Search(SearchTerm, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var order = result.Data;
                return (Paginate<Order>)order;
            }
            return null;
        }

        public async Task OnGetAsync()
        {
           if(!string.IsNullOrEmpty(SearchTerm))
            {
                Order = await Search();
            }
            else
            {
                Order = await GetOrder();
            }
        }
    }
}
