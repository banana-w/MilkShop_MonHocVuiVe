using MilkShop.Data.Models;
using MilkShopBusiness.Base;
using MilkShopData.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShopBusiness.ProductBusiness
{
    public interface IProductBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> UpdateAsync(Product product);
        Task<IBusinessResult> Save(Product product);
        Task<IBusinessResult> DeleteAsync(int id);

    }

    public class ProductBusiness : IProductBusiness
    {
        private readonly ProductDAO _DAO;

        public ProductBusiness()
        {
            _DAO = new ProductDAO();
        }
        public async Task<IBusinessResult> DeleteAsync(int id)
        {
            try
            {
                var product = await _DAO.GetByIdAsync(id);
                if (product != null)
                {
                    var result = await _DAO.RemoveAsync(product);
                    if (result)
                        return new BusinessResult(1, "success");
                    else
                        return new BusinessResult(0, "error");
                }
                return new BusinessResult(0, "no content");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                #region Business rule
                #endregion

                //var currencies = _DAO.GetAll();
                var products = await _DAO.GetAllAsync();

                if (products == null)
                {
                    return new BusinessResult(4, "No currency data");
                }
                else
                {
                    return new BusinessResult(1, "Get currency list success", products);
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public Task<IBusinessResult> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IBusinessResult> Save(Product product)
        {
            try
            {
                int result = await _DAO.CreateAsync(product);
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

        public async Task<IBusinessResult> UpdateAsync(Product product)
        {
            try
            {
                int result = await _DAO.UpdateAsync(product);
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
