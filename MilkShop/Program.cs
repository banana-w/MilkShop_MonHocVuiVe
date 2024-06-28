using Microsoft.EntityFrameworkCore;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Data.Models;
using MilkShopBusiness.ProductCategoryBusiness;
using MilkShopBusiness.ProductBrandBusiness;
using MilkShopBusiness.ProductBusiness;
using MilkShop.Business.LoginBusiness;
using MilkShop.Data.Repository;
using MilkShop.Business.CompanyBusiness;

namespace MilkShop
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddScoped<ICustomerBusiness, CustomerBusiness>();
            builder.Services.AddScoped<IProductCategoryBusiness, ProductCategoryBusiness>();

            builder.Services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromMinutes(20);
                options.Cookie.HttpOnly = true; // Ensure the cookie is accessible only to the server.
                options.Cookie.IsEssential = true; // Make the session cookie essential.
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<MilkShop.Business.OrderBusinesses.IOrderBusiness, MilkShop.Business.OrderBusinesses.OrderBusiness>();
            builder.Services.AddScoped<MilkShop.Business.OrderDetailBusinesses.IOrderDetailBusiness, MilkShop.Business.OrderDetailBusinesses.OrderDetailBusiness>();
            builder.Services.AddDbContext<MilkShopContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IProductBrandBusiness, ProductBrandBusiness>();
            builder.Services.AddScoped<IProductBusiness, ProductBusiness>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.AddScoped<ILoginBusiness, LoginBusiness>();
            builder.Services.AddScoped<ICompanyBusiness, CompanyBusiness>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();
            app.MapRazorPages();

            app.Run();
        }
    }
}
