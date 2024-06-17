using MilkShop.Data.DAO;
using MilkShop.Data.Models;
using MilkShopBusiness.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShopBusiness.OrderDetailBusiness
{
    public interface IOrderDetailBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> UpdateAsync(OrderDetail orderDetail);
        Task<IBusinessResult> Save(OrderDetail orderDetail);
        Task<IBusinessResult> DeleteAsync(int id);
    }
    public class OrderDetailBusiness : IOrderDetailBusiness
    {
        private readonly OrderDetailDAO _DAO;
        public OrderDetailBusiness()
        {
            _DAO = new OrderDetailDAO();
        }
        public Task<IBusinessResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var orderDetails = await _DAO.GetAllAsync();
                if (orderDetails != null)
                {
                    return new BusinessResult(1, "Get all order details successfully", orderDetails);
                }
                else
                {
                    return new BusinessResult(-1, "Get all order details fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }
        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var orderDetail = await _DAO.GetByIdAsync(id);
                if (orderDetail != null)
                {
                    return new BusinessResult(1, "Get order detail successfully", orderDetail);
                }
                else
                {
                    return new BusinessResult(-1, "Get order detail fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }
        public Task<IBusinessResult> Save(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
        public Task<IBusinessResult> UpdateAsync(OrderDetail orderDetail)
        {
            throw new NotImplementedException();
        }
    }
    
}
