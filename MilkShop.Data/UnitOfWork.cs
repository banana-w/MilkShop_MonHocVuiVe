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
        private CustomerRepository _customerRepository;
        private CompanyRepository _companyRepository;




        private OrderRepository _orderRepository;
        private OrderDetailRepository _orderDetailRepository;
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
        public CustomerRepository CustomerRepository
        {
            get
            {
                return _customerRepository ??= new CustomerRepository();
            }
        }
        public OrderRepository OrderRepository
        {
            get
            {
                return _orderRepository ??= new OrderRepository();
            }
        }

        public OrderDetailRepository OrderDetailRepository
        {
            get
            {
                return _orderDetailRepository ??= new OrderDetailRepository();
            }
        }

        public CompanyRepository CompanyRepository
        {
            get
            {
                return _companyRepository ??= new CompanyRepository();
            }
        }
    }
}
