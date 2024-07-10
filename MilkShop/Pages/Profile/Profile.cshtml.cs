using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;

namespace MilkShop.Pages.Profile
{
    public class ProfileModel : PageModel
    {
        private readonly ICustomerBusiness _customerBusiness;

        public ProfileModel(ICustomerBusiness customerBusiness)
        {
            _customerBusiness = customerBusiness;
        }
        [BindProperty]
        public Customer Customer { get; set; } = default!;

        public async Task GetProfileUserAsync()
        {
            int? userId = HttpContext.Session.GetInt32("UserID");
            if (userId.HasValue)
            {
                var customerResponse = await _customerBusiness.GetByIdAsync(userId.Value);
                if (customerResponse.Status > 0 && customerResponse.Data != null)
                {
                    Customer = customerResponse.Data as Customer;
                }
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            await GetProfileUserAsync();
            if (Customer == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var result = await _customerBusiness.UpdateAsync(Customer);

                if (result.Status > 0)
                {
                    ViewData["SuccessMessage"] = result.Message;
                    return RedirectToPage("./Profile");
                }
                else
                {
                    ViewData["ErrorMessage"] = result.Message;
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return Page();
            }
        }

    }
}

