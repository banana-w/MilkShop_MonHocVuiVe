using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using MilkShop.Data.Paging;
using MilkShopBusiness.ProductBrandBusiness;
using MilkShopBusiness.ProductBusiness;
using MilkShopBusiness.ProductCategoryBusiness;

namespace MilkShop.Pages.ProductPages
{
    public class IndexModel : PageModel
    {
        private readonly IProductBusiness _productBusiness;
        private readonly IProductBrandBusiness _productBrandBusiness;
        private readonly IProductCategoryBusiness _productCategoryBusiness;

        public IndexModel(IProductBusiness productBusiness, IProductCategoryBusiness productCategoryBusiness, IProductBrandBusiness productBrandBusiness)
        {
            _productBusiness = productBusiness;
            _productCategoryBusiness = productCategoryBusiness;
            _productBrandBusiness = productBrandBusiness;

        }
        public Paginate<Product> Product { get; set; } = default!;
        [BindProperty(SupportsGet = true)]
        public int SortPrice { get; set; }
        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }
        [BindProperty(SupportsGet = true)]
        public int BrandId { get; set; }
        [BindProperty(SupportsGet = true)]
        public int CateId { get; set; }
        [BindProperty(SupportsGet = true)]
        public decimal Price { get; set; }
        [BindProperty(SupportsGet = true)]
        public int PageIndex { get; set; } = 1;
        [BindProperty(SupportsGet = true)]
        public int Size { get; set; } = 6;

        private async Task<Paginate<Product>> GetProduct()
        {
            var result = await _productBusiness.GetAll(PageIndex, Size);
            if (result.Status > 0 && result.Data != null)
            {
                var product = result.Data;
                return (Paginate<Product>)product;
            }
            return null;
        }
        private async Task<Paginate<Product>> Search()
        {
            var result = await _productBusiness.Search(SearchTerm, BrandId, CateId, Price, PageIndex, Size, SortPrice);
            if (result.Status > 0 && result.Data != null)
            {
                var product = result.Data;
                return (Paginate<Product>)product;
            }
            return null;
        }
        public async Task OnGetAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm) || BrandId != 0 || CateId != 0 || Price != default || SortPrice != 0)
            {
                Product = await Search();
            }
            else
            {
                Product = await GetProduct();
            }
            ViewData["SuccessMessage"] = TempData["SuccessMessage"];
            var productBrand = await _productBrandBusiness.GetAll();
            var productCate = await _productCategoryBusiness.GetCategories();
            ViewData["ProductBrandId"] = new SelectList((System.Collections.IEnumerable)productBrand.Data, "ProductBrandId", "ProductBrandName");
            ViewData["ProductCategoryId"] = new SelectList((System.Collections.IEnumerable)productCate.Data, "ProductCategoryId", "ProductCategoryName");
            ViewData["SortEnum"] = new SelectList(new[] {
                                        new { Value = "1", Text = "Ascending Price" },
                                        new { Value = "2", Text = "Descending Price" }}, "Value", "Text");
        }

        public async Task<IActionResult> OnGetGeneratePdf()
        {
            Product = await GetProduct();
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            document.Open();

            // Add title
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, BaseColor.DARK_GRAY);
            var title = new Paragraph("Product List", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            title.SpacingAfter = 20f;
            document.Add(title);

            // Create table
            var table = new PdfPTable(8);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 3f, 4f, 2f, 2f, 2f, 2f, 3f, 3f });

            // Define styles
            var headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
            var cellFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, BaseColor.BLACK);
            var headerBackground = new BaseColor(52, 152, 219); // A nice blue color

            // Add table headers
            string[] headers = { "Product Name", "Description", "Price", "Status", "Created Date", "Stock", "Brand", "Category" };
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
            foreach (var item in Product.Items)
            {
                AddCell(table, item.ProductName, cellFont);
                AddCell(table, item.ProductDescription, cellFont);
                AddCell(table, $"{item.ProductPrice:C}", cellFont);
                AddCell(table, item.Status, cellFont);
                AddCell(table, item.CreatedDate.ToString(), cellFont);
                AddCell(table, item.StockQuantity.ToString(), cellFont);
                AddCell(table, item.ProductBrand?.ProductBrandName ?? "N/A", cellFont);
                AddCell(table, item.ProductCategory?.ProductCategoryName ?? "N/A", cellFont);
            }

            document.Add(table);

            // Add footer
            var footerFont = FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.GRAY);
            var footer = new Paragraph($"Generated on {DateTime.Now:dd/MM/yyyy HH:mm:ss}", footerFont);
            footer.Alignment = Element.ALIGN_RIGHT;
            footer.SpacingBefore = 10f;
            document.Add(footer);

            document.Close();

            return File(output.ToArray(), "application/pdf", "ProductList.pdf");
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
