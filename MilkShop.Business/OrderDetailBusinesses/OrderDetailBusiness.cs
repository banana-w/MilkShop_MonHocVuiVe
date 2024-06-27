using MilkShop.Data;
using MilkShop.Data.Models;
using MilkShopBusiness.Base;
using System;
using System.Threading.Tasks;

namespace MilkShop.Business.OrderDetailBusinesses
{   
    public interface IOrderDetailBusiness
    {
        Task<IBusinessResult> Add(OrderDetail orderDetail);
        Task<IBusinessResult> Update(OrderDetail orderDetail);
        Task<IBusinessResult> Delete(int id);
        Task<IBusinessResult> DeleteByOrderId(int orderId);
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
    }

    public class OrderDetailBusiness : IOrderDetailBusiness
    {
        private readonly UnitOfWork _unitOfWork;

        public OrderDetailBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }

        public async Task<IBusinessResult> Add(OrderDetail orderDetail)
        {
            try
            {
                int result = await _unitOfWork.OrderDetailRepository.CreateAsync(orderDetail);
                if (result > 0)
                {
                    return new BusinessResult(1, "success");
                }
                else
                {
                    return new BusinessResult(2, "fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Delete(int id)
        {
            try
            {
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
                if (orderDetail != null)
                {
                    var result = await _unitOfWork.OrderDetailRepository.RemoveAsync(orderDetail);
                    if (result)
                        return new BusinessResult(1, "success");
                    else
                        return new BusinessResult(0, "error");
                }
                return new BusinessResult(0, "no content");
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> DeleteByOrderId(int orderId)
        {
            try
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetByOrderId(orderId);
                if (orderDetails != null)
                {
                    foreach (var orderDetail in orderDetails)
                    {
                        await _unitOfWork.OrderDetailRepository.RemoveAsync(orderDetail);
                    }
                  
                    return new BusinessResult(1, "success");
                }
                else
                {
                    // Trường hợp không tìm thấy OrderDetails
                    return new BusinessResult(0, "No order details found for the given order ID.");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }


        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var orderDetails = await _unitOfWork.OrderDetailRepository.GetAllAsync();
                if (orderDetails == null)
                {
                    return new BusinessResult(4, "No order detail data");
                }
                else
                {
                    return new BusinessResult(1, "Get order detail list success", orderDetails);
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
                var orderDetail = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
                if (orderDetail == null)
                {
                    return new BusinessResult(4, "No order detail found");
                }
                else
                {
                    return new BusinessResult(1, "Get order detail success", orderDetail);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Update(OrderDetail orderDetail)
        {
            try
            {
                int result = await _unitOfWork.OrderDetailRepository.UpdateAsync(orderDetail);
                if (result > 0)
                {
                    return new BusinessResult(1, "success");
                }
                else
                {
                    return new BusinessResult(2, "fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }
    }
}