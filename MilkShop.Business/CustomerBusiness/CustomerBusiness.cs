using Microsoft.EntityFrameworkCore;
using MilkShop.Data;
using MilkShop.Data.Models;
using MilkShopBusiness.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Business.CustomerBusiness
{
    public interface ICustomerBusiness
    {
        Task<IBusinessResult> GetAll(int page, int size);
        Task<IBusinessResult> GetByIdAsync(int id);
        Task<IBusinessResult> UpdateAsync(Customer customer);
        Task<IBusinessResult> Save(Customer customer);
        Task<IBusinessResult> DeleteAsync(int id);
        Task<IBusinessResult> DeleteCustomer(int id);
        Task<IBusinessResult> Search(string searchTerm, int page, int size);
        Task<IBusinessResult> GetAllCustomer();

    }

    public class CustomerBusiness : ICustomerBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public CustomerBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> DeleteAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (order != null)
                {
                    var deleted = await _unitOfWork.CustomerRepository.RemoveAsync(order);
                    if (deleted)
                    {
                        return new BusinessResult(1, "Delete customer successfully");
                    }
                    else
                    {
                        return new BusinessResult(0, "Delete customer failed");
                    }
                }
                return new BusinessResult(0, "No content");
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAll(int page, int size)
        {
            try
            {
                //var customer = await _unitOfWork.CustomerRepository.GetAllAsync();
                var customer = await _unitOfWork.CustomerRepository.GetPagingListAsync(
                   selector: x => x,
                   page: page,
                   size: size
                   );
                if (customer != null)
                {
                    return new BusinessResult(1, "Get all customer successfully", customer);
                }
                else
                {
                    return new BusinessResult(-1, "Get all customer fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetAllCustomer()
        {
            try
            {
                //var currencies = _DAO.GetAll();
                var customers = await _unitOfWork.CustomerRepository.GetAllActiveCustomers();
                if (customers == null)
                {
                    return new BusinessResult(4, "No currency data");
                }
                else
                {
                    return new BusinessResult(1, "Get currency list success", customers);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Search(string searchTerm, int page, int size)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetPagingListAsync(
                    selector: x => x,
                    predicate: x => x.UserName.Contains(searchTerm),
                    page: page,
                    size: size
                    );

                if (customer != null)
                {
                    return new BusinessResult(1, "Search successfully", customer);
                }
                else
                {
                    return new BusinessResult(1, "Search fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetByIdAsync(int id)
        {
            try
            {
                var customer = await _unitOfWork.CustomerRepository.GetByIdAsync(id);
                if (customer != null)
                {
                    return new BusinessResult(1, "Get customer successfully", customer);
                }
                else
                {
                    return new BusinessResult(-1, "Get customer fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(Customer customer)
        {
            try
            {
                var newCustomer = await _unitOfWork.CustomerRepository.CreateAsync(customer);
                if (newCustomer >= 1)
                {
                    return new BusinessResult(1, "Create successfully");
                }
                else
                {
                    return new BusinessResult(-1, "Create fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateAsync(Customer customer)
        {
            try
            {
                var updateCustomer = await _unitOfWork.CustomerRepository.UpdateAsync(customer);
                if (updateCustomer != null)
                {
                    return new BusinessResult(1, "Update successfully");
                }
                else
                {
                    return new BusinessResult(-1, "Update fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteCustomer(int id)
        {
            try
            {
                var removeCustomer = await _unitOfWork.CustomerRepository.RemoveCustomerAsync(id);
                if (removeCustomer != null)
                {
                    return new BusinessResult(1, "Remove successfully");
                }
                else
                {
                    return new BusinessResult(-1, "Remove fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }
    }
}
