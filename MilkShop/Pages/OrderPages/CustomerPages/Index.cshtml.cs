using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;

namespace MilkShop.Pages.CustomerPages
{
    public class CustomerModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public CustomerModel(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }
        public string Message { get; set; } = default!;
        public Paginate<Customer> Customer { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;

        private async Task<Paginate<Customer>> GetCustomers()
        {
            var result = await _customerBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var customer = result.Data;
                return (Paginate<Customer>)customer;
            }
            return null;
        }

        private async Task<Paginate<Customer>> Search()
        {
            var result = await _customerBusiness.Search(SearchTerm, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var customer = result.Data;
                return (Paginate<Customer>)customer;
            }
            return null;
        }
        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Customer = await Search();
            }
            else
            {
                Customer = await GetCustomers();
            }
        }
    }
}
