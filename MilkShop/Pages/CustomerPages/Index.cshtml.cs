using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;

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
        public Paginate<Customer> Customer { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;

        private async Task<Paginate<Customer>> GetCustomers()
        {
            var result = await _customerBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var customer = result.Data;
                return (Paginate<Customer>)customer;
            }
            return null;
        }

        private async Task<Paginate<Customer>> Search()
        {
            var result = await _customerBusiness.Search(SearchTerm, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var customer = result.Data;
                return (Paginate<Customer>)customer;
            }
            return null;
        }
        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Customer = await Search();
            }
            else
            {
                Customer = await GetCustomers();
            }
        }

        public async Task<IActionResult> OnGetGeneratePdf()
        {
            Customer = await GetCustomers();
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            document.Open();

            // Add title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.DARK_GRAY);
            var title = new Paragraph("Customer List", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            // Create table
            var table = new PdfPTable(9);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 3f, 4f, 2f, 3f, 3f, 4f, 3f, 3f, 3f });

            // Define styles
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
            var headerBackground = new BaseColor(52, 152, 219); // A nice blue color

            // Add table headers
            string[] headers = { "User Name", "Email", "Status", "Created Date", "Phone Number", "Address", "Date of Birth", "Preferred Language", "User ID" };
            foreach (var header in headers)
            {
                var cell = new PdfPCell(new Phrase(header, headerFont));
                cell.BackgroundColor = headerBackground;
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                cell.Padding = 5;
                table.AddCell(cell);
            }

            // Add table rows
            foreach (var item in Customer.Items)
            {
                AddCell(table, item.UserName, cellFont);
                AddCell(table, item.UserEmail, cellFont);
                AddCell(table, item.Status, cellFont);
                AddCell(table, item.CreatedDate.ToString(), cellFont);
                AddCell(table, item.PhoneNumber, cellFont);
                AddCell(table, item.Address, cellFont);
                AddCell(table, item.DateOfBirth?.ToString() ?? "N/A", cellFont);
                AddCell(table, item.PreferredLanguage, cellFont);
               
            }

            document.Add(table);

            // Add footer
            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.GRAY);
            var footer = new Paragraph($"Generated on {DateTime.Now:dd/MM/yyyy HH:mm:ss}", footerFont);
            footer.Alignment = Element.ALIGN_RIGHT;
            footer.SpacingBefore = 10f;
            document.Add(footer);

            document.Close();

            return File(output.ToArray(), "application/pdf", "CustomerList.pdf");
        }

        private void AddCell(PdfPTable table, string text, Font font)
        {
            var cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
            cell.Padding = 5;
            table.AddCell(cell);
        }
    }
}
