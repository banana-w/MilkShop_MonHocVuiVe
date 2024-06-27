using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CompanyBusiness;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;

namespace MilkShop.Pages.CompanyPages
{
    public class EditModel : PageModel
    {
        private readonly ICompanyBusiness _companyBusiness;

        public EditModel(ICompanyBusiness companyBusiness)
        {
            _companyBusiness = companyBusiness;
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var company = await _companyBusiness.GetById(id);
            if (company == null)
            {
                return NotFound();
            }
            Company = company.Data as Company;
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
                var result = await _companyBusiness.UpdateAsync(Company);
                if (result.Status > 0)
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
                if (!await CompanyExits(Company.CompanyId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
        }

        private async Task<bool> CompanyExits(int id)
        {
            var customer = await _companyBusiness.GetById(id);
            return customer != null && customer.Data != null;
        }
    }
}
