using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CompanyBusiness;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;
using MilkShopBusiness.ProductBusiness;

namespace MilkShop.Pages.CompanyPages
{
    public class IndexModel : PageModel
    {
        private readonly ICompanyBusiness _companyBusiness;

        public IndexModel(ICompanyBusiness companyBusiness)
        {
            _companyBusiness = companyBusiness;
        }
        public Paginate<Company> Company { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;

        private async Task<Paginate<Company>> GetCompany()
        {
            var result = await _companyBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var company = result.Data;
                return (Paginate<Company>)company;
            }
            return null;
        }
        private async Task<Paginate<Company>> Search()
        {
            var result = await _companyBusiness.Search(SearchTerm, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var company = result.Data;
                return (Paginate<Company>)company;
            }
            return null;
        }
        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Company = await Search();
            }
            else
            {
                Company = await GetCompany();
            }
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
        }


    }
}
