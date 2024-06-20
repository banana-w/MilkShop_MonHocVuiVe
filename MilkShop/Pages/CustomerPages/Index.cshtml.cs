using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;

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

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public List<Customer> Customers { get; set; } = default!;

        private List<Customer> GetCustomers()
        {
            var result = _customerBusiness.GetAll();
            if(result.Status > 0 && result.Result.Data != null)
            {
                var customer = (List<Customer>)result.Result.Data;
                return customer;
            }
            return null;
        }
        public void OnGet()
        {
            Customers = GetCustomers();
        }
    }
}
