using Azure;
using Microsoft.EntityFrameworkCore;
using MilkShop.Common;
using MilkShop.Data;
using MilkShop.Data.Models;
using MilkShopBusiness.Base;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace MilkShopBusiness.ProductBusiness
{
    public interface IProductBusiness
    {
        Task<IBusinessResult> GetAll(int page, int size);
        Task<IBusinessResult> GetAllProduct();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> FindId(int id);
        Task<IBusinessResult> UpdateAsync(Product product);
        Task<IBusinessResult> Save(Product product);
        Task<IBusinessResult> DeleteAsync(int id);
        Task<IBusinessResult> Search(string searchTerm, int page, int size);
    }

    public class ProductBusiness : IProductBusiness
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductBusiness()
        {
            _unitOfWork ??= new UnitOfWork();
        }
        public async Task<IBusinessResult> DeleteAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product != null)
                {
                    var result = await _unitOfWork.ProductRepository.RemoveAsync(product);
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

        public async Task<IBusinessResult> GetAll(int page, int size)
        {
            try
            {
                #region Business rule
                #endregion

                //var currencies = _DAO.GetAll();
                var products = await _unitOfWork.ProductRepository.GetPagingListAsync(
                    selector: x => x,
                    page: page,
                    size: size,
                    include: x => x.Include(p => p.ProductBrand).Include(p => p.ProductCategory)
                    );
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

        public async Task<IBusinessResult> GetAllProduct()
        {
            try
            {
                #region Business rule
                #endregion

                //var currencies = _DAO.GetAll();
                var products = await _unitOfWork.ProductRepository.GetAllAsync();
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

        public async Task<IBusinessResult> GetById(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.SingleOrDefaultAsync(
                    selector: x => x,
                    predicate: x => x.ProductId == id,
                    include: x => x.Include(p => p.ProductBrand).Include(p => p.ProductCategory)
                    );
                if (product != null)
                {
                    return new BusinessResult(1, "Get product successfully", product);
                }
                else
                {
                    return new BusinessResult(-1, "Get product fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }
        public async Task<IBusinessResult> FindId(int id)
        {
            try
            {
                var product = await _unitOfWork.ProductRepository.GetByIdAsync(id);
                if (product != null)
                {
                    return new BusinessResult(1, "Get product successfully", product);
                }
                else
                {
                    return new BusinessResult(-1, "Get product fail");
                }
            }
            catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(Product product)
        {
            try
            {
                int result = await _unitOfWork.ProductRepository.CreateAsync(product);
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

        public async Task<IBusinessResult> Search(string searchTerm, int page, int size)
        {
            try
            {
                var productBrands = await _unitOfWork.ProductRepository.GetPagingListAsync(
                    selector: x => x,
                    predicate: x => x.ProductName.Contains(searchTerm),
                    page: page,
                    size: size,
                    include: x => x.Include(p => p.ProductBrand).Include(p => p.ProductCategory)
                    );

                if (productBrands != null)
                {
                    return new BusinessResult(1, "Create successfully", productBrands);
                }
                else
                {
                    return new BusinessResult(1, "Create fail");
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
                int result = await _unitOfWork.ProductRepository.UpdateAsync(product);
                if (result > 0)
                {
                    return new BusinessResult(1, Const.SUCCESS_UPDATE_MSG);
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
