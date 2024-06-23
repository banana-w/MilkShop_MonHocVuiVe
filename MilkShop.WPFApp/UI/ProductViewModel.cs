using MilkShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkShop.WPFApp.UI
{
    public class ProductViewModel : INotifyPropertyChanged
    {
        private Product _product;

        public Product Product
        {
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged(nameof(Product));
            }
        }

        public ProductViewModel()
        {
            Product = new Product
            {
                ProductId = 1,
                ProductName = "Sample Product",
                ProductPrice = 9.99m,
                ProductDescription = "Sample Description",
                ProductImage = "sample.jpg",
                ProductBrandId = 1,
                ProductCategoryId = 1,
                Status = "Active",
                CreatedDate = new DateOnly(),
                StockQuantity = 100
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
