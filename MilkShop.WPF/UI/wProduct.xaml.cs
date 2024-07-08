using MilkShop.Data.Models;
using MilkShopBusiness.ProductBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace MilkShop.WPFApp.UI
{
    /// <summary>
    /// Interaction logic for wProduct.xaml
    /// </summary>
    public partial class wProduct : Window
    {
        private readonly IProductBusiness _productBusiness;
        public wProduct()
        {
            _productBusiness ??= new ProductBusiness();
            InitializeComponent();
            LoadGrdMentor();

        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int productId = -1;
                int.TryParse(txtProductCode.Text, out productId);
                var item = await _productBusiness.FindId(productId);
                if (item.Data == null)
                {
                    var NewProduct = new Product()
                    {
                        ProductName = txtProductName.Text,
                        ProductPrice = decimal.Parse(txtProductPrice.Text),
                        ProductDescription = txtDesc.Text,
                        ProductImage = txtImage.Text,
                        Status = txtStatus.Text,
                        CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                        StockQuantity = int.Parse(txtStock.Text)
                    };

                    var result = await _productBusiness.Save(NewProduct);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var product = item.Data as Product;
                    product.ProductName = txtProductName.Text;
                    product.ProductPrice = decimal.Parse(txtProductPrice.Text);
                    product.ProductDescription = txtDesc.Text;
                    product.ProductImage = txtImage.Text;
                    product.Status = txtStatus.Text;
                    product.StockQuantity = int.Parse(txtStock.Text);
                    
                    var result = await _productBusiness.UpdateAsync(product);
                    MessageBox.Show(result.Message, "Update");
                }

                txtProductCode.Text = string.Empty;
                txtProductName.Text = string.Empty;
                txtProductPrice.Text = string.Empty;
                txtDesc.Text = string.Empty;
                txtImage.Text = string.Empty;
                txtStatus.Text = string.Empty;
                txtDateJoined.Text = string.Empty;
                txtStock.Text = string.Empty;
                LoadGrdMentor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            txtProductCode.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtProductPrice.Text = string.Empty;
            txtDesc.Text = string.Empty;
            txtImage.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtDateJoined.Text = string.Empty;
            txtStock.Text = string.Empty;
        }
        private async void grdProduct_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Double Click on Grid");
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Product;
                    if (item != null)
                    {
                        var Result = await _productBusiness.GetById(item.ProductId);

                        if (Result.Status > 0 && Result.Data != null)
                        {
                            var product = Result.Data as Product;
                            txtProductCode.Text = product.ProductId.ToString();
                            txtProductName.Text = product.ProductName;
                            txtProductPrice.Text = product.ProductPrice.ToString();
                            txtDesc.Text = product.ProductDescription;
                            txtImage.Text = product.ProductImage;
                            txtStatus.Text = product.Status;
                            txtDateJoined.Text = product.CreatedDate.ToString();
                            txtStock.Text = product.StockQuantity.ToString();
                        }
                    }
                }
            }
        }
        private async void grdMentor_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            // Attempt to convert CommandParameter to int
            int? productId;
            if (btn.CommandParameter != null && int.TryParse(btn.CommandParameter.ToString(), out int value))
            {
                productId = value;
            }
            else
            {
                // Handle the case where CommandParameter is not an integer or null
                MessageBox.Show("Invalid Product ID format. Please try again.", "Error", MessageBoxButton.OK);
                return;
            }

            // Confirmation and deletion logic (assuming InternId is valid)
            if (MessageBox.Show("Do you want to delete this item?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var result = await _productBusiness.DeleteAsync(productId.Value);
                MessageBox.Show($"{result.Message}", "Delete");
                LoadGrdMentor();
            }

        }

        private async void LoadGrdMentor()
        {
            var result = await _productBusiness.GetAllProduct();
            if (result.Status > 0 && result.Data != null)
            {
                grdProduct.ItemsSource = null;
                grdProduct.ItemsSource = result.Data as List<Product>;
            }
            else
            {
                grdProduct.ItemsSource = new List<Product>();
            }
        }
    }
}
