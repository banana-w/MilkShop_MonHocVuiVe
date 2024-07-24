using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;

namespace MilkShop.Pages.CustomerPages
{
    public class EditCustomer : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public EditCustomer(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer =  await _customerBusiness.GetByIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            Customer = customer.Data as Customer;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                var result = await _customerBusiness.UpdateAsync(Customer);
                if(result.Status > 0)
                {
                    ViewData["SuccessMessage"] = result.Message;
                    return RedirectToPage("./Index");
                }
                else
                {
                    ViewData["ErrorMessage"] = result.Message;
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await CustomerExists(Customer.UserId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<bool> CustomerExists(int id)
        {
            var customer = await _customerBusiness.GetByIdAsync(id);
            return customer != null && customer.Data != null;
        }
    }
}
