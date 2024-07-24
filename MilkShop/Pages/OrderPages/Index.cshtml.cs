using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;

namespace MilkShop.Pages.OrderPages
{
    public class IndexModel : PageModel
    {
        private readonly MilkShop.Data.Models.MilkShopContext _context;
        private readonly MilkShop.Business.OrderBusinesses.IOrderBusiness _orderBusiness;

        public IndexModel(MilkShop.Data.Models.MilkShopContext context, Business.OrderBusinesses.IOrderBusiness orderBusiness)
        {
            _context = context;
            _orderBusiness = orderBusiness;
        }

        public Paginate<Order> Order { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal Price { get; set; }
        [BindProperty(SupportsGet = true)]

        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 5;


        private async Task<Paginate<Order>> GetOrder()
        {
            var result = await _orderBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var order = result.Data;
                return (Paginate<Order>)order;
            }
            return null;
        }

        private async Task<Paginate<Order>> Search()
        {
            var result = await _orderBusiness.Search(SearchTerm, Price, PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var order = result.Data;
                return (Paginate<Order>)order;
            }
            return null;
        }

        public async Task OnGetAsync()
        {
            ViewData["Message"] = TempData["Message"];
           if(!string.IsNullOrEmpty(SearchTerm))
            {
                Order = await Search();
            }
            else
            {
                Order = await GetOrder();
            }
        }

        public async Task<IActionResult> OnGetGeneratePdf()
        {
            Order = await GetOrder();
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            document.Open();

            // Add title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.DARK_GRAY);
            var title = new Paragraph("Order List", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            // Create table
            var table = new PdfPTable(11);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 3f, 3f, 3f, 3f, 3f, 3f, 3f, 4f, 4f, 3f, 3f });

            // Define styles
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
            var headerBackground = new BaseColor(52, 152, 219); // A nice blue color

            // Add table headers
            string[] headers = { "Order Date", "Order Status", "Total Amount", "Payment Method", "Payment Status", "Status", "Created Date", "Shipping Address", "Billing Address", "Shipping Method", "User" };
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
            foreach (var item in Order.Items)
            {
                AddCell(table, item.OrderDate.ToString(), cellFont);
                AddCell(table, item.OrderStatus, cellFont);
                AddCell(table, $"${item.OrderTotalAmount:N2}", cellFont);
                AddCell(table, item.PaymentMethodId, cellFont);
                AddCell(table, item.PaymentStatus, cellFont);
                AddCell(table, item.Status, cellFont);
                AddCell(table, item.CreatedDate.ToString(), cellFont);
                AddCell(table, item.ShippingAddress, cellFont);
                AddCell(table, item.BillingAddress, cellFont);
                AddCell(table, item.ShippingMethod, cellFont);
                AddCell(table, item.User.UserName, cellFont);
            }

            document.Add(table);

            // Add footer
            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.GRAY);
            var footer = new Paragraph($"Generated on {DateTime.Now:dd/MM/yyyy HH:mm:ss}", footerFont);
            footer.Alignment = Element.ALIGN_RIGHT;
            footer.SpacingBefore = 10f;
            document.Add(footer);

            document.Close();

            return File(output.ToArray(), "application/pdf", "OrderList.pdf");
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
