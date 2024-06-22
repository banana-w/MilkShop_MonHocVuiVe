using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MilkShop.Business.LoginBusiness;
using MilkShop.Data.Models;
using MilkShop.Data.Repository;

namespace MilkShop.Pages
{
    public class Index : PageModel
    {
        private readonly ILoginRepository _userRepository;
        private readonly ILoginBusiness _userService;
        public Index(ILoginRepository userRepository, ILoginBusiness userService)
        {
            _userRepository = userRepository;
            _userService = userService;
        }

        [BindProperty]
        public Customer user { get; set; } = default!;
        public string ErrorMessage { get; private set; }

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
    }
}
