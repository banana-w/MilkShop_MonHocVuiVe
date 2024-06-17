using MilkShop.Data.Models;

using MilkShop.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.Data
{
    public class UnitOfWork
    {

        private MilkShopContext _unitOfWorkContext;
        private ProductBrandRepository _productBrand;
        private ProductCategoryRepository _productCategoryRepository;
        private ProductRepository _productRepository;
        public UnitOfWork() { }

        public ProductCategoryRepository ProductCategoryRepository
        {
            get
            {
                return _productCategoryRepository ??= new ProductCategoryRepository();
            }
        }
        public ProductBrandRepository ProductBrandRepository
        {
            get
            {
                return _productBrand ??= new ProductBrandRepository();
            }
        }
        public ProductRepository ProductRepository
        {
            get
            {
                return _productRepository ??= new ProductRepository();
            }
        }
    }
}
