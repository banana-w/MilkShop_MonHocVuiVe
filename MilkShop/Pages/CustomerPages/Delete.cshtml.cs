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
    public class DeleteCustomerModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public DeleteCustomerModel(ICustomerBusiness customerBusiness)
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

            var result = await _customerBusiness.GetByIdAsync(id);

            if (result.Status > 0)
            {
                Customer = result.Data as Customer;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteResult = await _customerBusiness.DeleteAsync(id);
            if (deleteResult.Status > 0)
            {
                ViewData["SuccessMessage"] = deleteResult.Message;
                return RedirectToPage("./Index");
            }
            else
            {
                ViewData["ErrorMessage"] = $"Error: {deleteResult.Message}";
                return Page();

            }
        }
    }
}
