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
    public class DetailsModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public DetailsModel(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }

        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _customerBusiness.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            else
            {
                Customer = customer.Data as Customer;
            }
            return Page();
        }
    }
}
