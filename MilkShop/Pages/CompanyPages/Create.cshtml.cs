using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MilkShop.Business.CompanyBusiness;
using MilkShop.Common;
using MilkShop.Data.Models;

namespace MilkShop.Pages.CompanyPages
{
    public class CreateModel : PageModel
    {
        private readonly ICompanyBusiness _companyBusiness;

        public CreateModel(ICompanyBusiness companyBusiness)
        {
            _companyBusiness = companyBusiness;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Company Company { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            var result = await _companyBusiness.Save(Company);
            if (result.Status != Const.SUCCESS_CREATE_CODE)
            {
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
