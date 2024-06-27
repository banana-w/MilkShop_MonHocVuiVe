using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Base;
using MilkShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Data.Repository
{
    public interface ILoginRepository
    {
        Customer checkLogin(string userName, string password);
    }

    public class LoginRepository : ILoginRepository
    {
        public Customer checkLogin(string userName, string password) => LoginDAO.Instance.checkLogin(userName, password);
    }
}
