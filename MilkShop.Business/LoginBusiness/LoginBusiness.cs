using MilkShop.Data.Models;
using MilkShop.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Business.LoginBusiness
{
    public interface ILoginBusiness
    {
        Customer checkLogin(string userName, string password);
    }
    public class LoginBusiness : ILoginBusiness
    {
        private readonly ILoginRepository _loginRepository;
        public LoginBusiness(ILoginRepository userRepository)
        {
            _loginRepository = userRepository;
        }
        public Customer checkLogin(string userName, string password) => _loginRepository.checkLogin(userName, password);
    }
}
