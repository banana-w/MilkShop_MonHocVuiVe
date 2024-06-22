using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CompanyBusiness;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;

namespace MilkShop.Pages.CompanyPages
{
    public class DetailsModel : PageModel
    {
        private readonly ICompanyBusiness _companyBusiness;

        public DetailsModel(ICompanyBusiness companyBusiness)
        {
            _companyBusiness = companyBusiness;
        }

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
            else
            {
                Company = company.Data as Company;
            }
            return Page();
        }
    }
}
