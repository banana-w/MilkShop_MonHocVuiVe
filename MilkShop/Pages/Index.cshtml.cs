using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkShop.Business.CustomerBusiness;
using MilkShop.Business.LoginBusiness;
using MilkShop.Data.Models;
using MilkShop.Data.Repository;
using System.Security.Claims;

namespace MilkShop.Pages
{
    public class Index : PageModel
    {
        private readonly ILoginRepository _userRepository;
        private readonly ILoginBusiness _userService;
        private readonly ICustomerBusiness _customerBusiness;

        public Index(ILoginRepository userRepository, ILoginBusiness userService, ICustomerBusiness customerBusiness)
        {
            _userRepository = userRepository;
            _userService = userService;
            _customerBusiness = customerBusiness;
        }

        [BindProperty]
        public Customer user { get; set; } = default!;
        public string ErrorMessage { get; private set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        public IActionResult OnPost()
        {

            if (!string.IsNullOrWhiteSpace(user.UserName) && !string.IsNullOrWhiteSpace(user.Password))
            {
                try
                {
                    var check = _userService.checkLogin(user.UserName, user.Password);
                    if (check != null)
                    {
                        HttpContext.Session.SetInt32("UserID", check.UserId);
                        return RedirectToPage("HomePage");
                    }
                }
                catch
                {
                    ErrorMessage = "Incorect User Name or Password Please Try Again";
                    return Page();
                }
            }
            return Page();
        }

        public IActionResult OnGetLogin()
        {
            var redirectUrl = Url.Page("Index", "GoogleResponse", new { ReturnUrl = ReturnUrl });
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, "Google");
        }


        public async Task<IActionResult> OnGetGoogleResponseAsync(string returnUrl = "/")
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result?.Principal == null)
            {
                return RedirectToPage("./Index");
            }

            // Handle user info and save to session or database
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);
            var name = result.Principal.FindFirstValue(ClaimTypes.Name);
            try
            {
                var check = _userRepository.checkLoginByEmail(email, "1");
                if (check != null)
                {
                    HttpContext.Session.SetInt32("UserID", check.UserId);
                    return RedirectToPage("HomePage");
                }
                else
                {
                    var newCustomer = new Customer
                    {
                        UserName = name,
                        UserEmail = email,
                        Password = "1",
                        CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                        Status = "Active"
                    };
                    _customerBusiness.Save(newCustomer);
                    return RedirectToPage("HomePage");
                }
            }
            catch
            {
                ErrorMessage = "Incorect User Name or Password Please Try Again";
                return Page();
            }
        }
    }
}
