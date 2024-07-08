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
    public class CustomerRepository : GenericRepository<Customer>
    {
        public CustomerRepository() { }

        public async Task<bool> RemoveCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _dbSet.FirstOrDefaultAsync(x => x.UserId == customerId);
                if (customer != null)
                {
                    customer.Status = "Inactive";

                    _dbSet.Update(customer);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    Console.WriteLine($"Customer with ID {customerId} not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error removing customer: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Customer>> GetAllActiveCustomers()
        {
            try
            {
                var customers = await _dbSet.Where(x => x.Status == "Active").ToListAsync();
                return customers;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving active customers: {ex.Message}");
                return new List<Customer>();
            }

        }

    }

}
