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
    public class CompanyRepository: GenericRepository<Company>
    {
        public CompanyRepository() { }
    }
}
