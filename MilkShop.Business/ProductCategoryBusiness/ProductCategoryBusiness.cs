using MilkShop.Data;
using MilkShop.Data.DAO;
using MilkShop.Data.Models;
using MilkShopBusiness.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShopBusiness.ProductCategoryBusiness
{
    public interface IProductCategoryBusiness
    {
        Task<IBusinessResult> GetAll();
        Task<IBusinessResult> GetById(int id);
        Task<IBusinessResult> UpdateAsync(ProductCategory productCategory);
        Task<IBusinessResult> Save(ProductCategory productCategory);
        Task<IBusinessResult> DeleteAsync(int id);

    }
    public class ProductCategoryBusiness : IProductCategoryBusiness
    {
        //private readonly ProductCategoryDAO _DAO;
        private readonly UnitOfWork _unitOfWork;

        public ProductCategoryBusiness()
        {
            //_DAO = new ProductCategoryDAO();
            _unitOfWork ??= new UnitOfWork();
        }
       
        public Task<IBusinessResult> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IBusinessResult> GetAll()
        {
            try
            {
                var productCategories = await _unitOfWork.ProductCategoryRepository.GetAllAsync();
                if (productCategories != null)
                {
                    return new BusinessResult(1, "Get all product categories successfully", productCategories);
                }
                else
                {
                    return new BusinessResult(-1, "Get all product categories fail");
                }
            }catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> GetById(int id)
        {
            

            try
            {
                var productCategory = await _unitOfWork.ProductCategoryRepository.GetByIdAsync(id);
                if (productCategory != null)
                {
                    return new BusinessResult(1, "Get product category successfully", productCategory);
                }
                else
                {
                    return new BusinessResult(-1, "Get product category fail");
                }
            }catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> Save(ProductCategory productCategory)
        {
            try
            {
                var newProductCategory = await _unitOfWork.ProductCategoryRepository.CreateAsync(productCategory);
                if (newProductCategory != null)
                {
                    return new BusinessResult(1, "Create product category successfully", newProductCategory);
                }
                else
                {
                    return new BusinessResult(-1, "Create product category fail");
                }
              
            }catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }

        public async Task<IBusinessResult> UpdateAsync(ProductCategory productCategory)
        {
            try
            {
                var productCategoryForUpdate = await _unitOfWork.ProductCategoryRepository.UpdateAsync(productCategory);
                if(productCategoryForUpdate != null)
                {
                    return new BusinessResult(1, "Update product category successfully", productCategoryForUpdate);
                }
                else
                {
                    return new BusinessResult(-1, "Update product category fail");
                }

            }catch (Exception ex)
            {
                return new BusinessResult(-4, ex.Message);
            }
        }   
    }
    
}
