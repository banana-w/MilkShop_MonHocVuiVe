using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CompanyBusiness;
using MilkShop.Data.Models;

namespace MilkShop.Pages.CompanyPages
{
    public class DeleteModel : PageModel
    {
        private readonly ICompanyBusiness _companyBusiness;

        public DeleteModel(ICompanyBusiness companyBusiness)
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

            if (company.Status > 0)
            {
                Company = company.Data as Company;
            }
            else
            {
                TempData["ErrorMessage"] = company.Message;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deleteResult = await _companyBusiness.DeleteAsync(id);
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
