using Microsoft.EntityFrameworkCore;
using MilkShop.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Data.Base
{
    public class LoginDAO
    {
        private readonly MilkShopContext _context;

        private static LoginDAO instance = null!;

        private static readonly object instanceLock = new object();

        public LoginDAO()
        {
            _context = new MilkShopContext();
        }

        public static LoginDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new LoginDAO();
                    }
                    return instance;
                }
            }
        }
        public Customer checkLogin(string userName, string password)
        {
            try
            {
                var check = _context.Customers.Where(u => u.UserName!.Equals(userName) && u.Password!.Equals(password)).FirstOrDefault();

                if (check != null)
                {
                    return check;
                }
                throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
