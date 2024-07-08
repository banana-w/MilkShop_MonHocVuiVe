using MilkShop.Business.CustomerBusiness;
using MilkShop.Business.OrderBusinesses;
using MilkShop.Data.Models;
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

namespace MilkShop.WPF.UI
{
    /// <summary>
    /// Interaction logic for Customer.xaml
    /// </summary>
    public partial class wCustomer : Window
    {
        private readonly CustomerBusiness _customerBusiness;
        public wCustomer()
        {
            InitializeComponent();
            _customerBusiness ??= new CustomerBusiness();
            Load();
        }

        private async void Load()
        {
            var result = await _customerBusiness.GetAll(1, 100);
            if (result.Status > 0 && result.Data != null)
            {
                grdCustomer.ItemsSource = null;
                grdCustomer.ItemsSource = result.Data as List<Customer>;
            }
            else
            {
                grdCustomer.ItemsSource = new List<Customer>();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newCustomer = new Customer
                {
                    UserName = txtCustomerName.Text,
                    UserEmail = txtCustomerEmail.Text,
                    PhoneNumber = txtCustomerPhone.Text,
                    Address = txtCustomerAddress.Text,
                    DateOfBirth = DateOnly.Parse(txtDateOfBirth.Text),
                    Password = txtPassword.Password,
                    PreferredLanguage = txtPreferredLanguage.Text,
                    ImageUrl = txtImageUrl.Text,
                };

                var result = await _customerBusiness.Save(newCustomer);
                MessageBox.Show(result.Message, "Create");
                Load();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
