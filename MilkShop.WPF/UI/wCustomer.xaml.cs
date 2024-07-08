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
    /// Interaction logic for wCustomer.xaml
    /// </summary>
    public partial class wCustomer : Window
    {
        private readonly ICustomerBusiness _customerBusiness;

        public wCustomer()
        {
            InitializeComponent();
            _customerBusiness ??= new CustomerBusiness();
            txtCustomerId.IsReadOnly = true;
            cmbStatus.Items.Add(new ComboBoxItem { Content = "Active" });
            cmbStatus.Items.Add(new ComboBoxItem { Content = "Inactive" });
            cmbStatus.SelectedIndex = 0;
            Load();
        }

        private async void Load()
        {
            try
            {
                var result = await _customerBusiness.GetAllCustomer();

                if (result.Status > 0 && result.Data != null)
                {
                    grdCustomer.ItemsSource = result.Data as List<Customer>;
                }
                else
                {
                    grdCustomer.ItemsSource = new List<Customer>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private async void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newCustomer = new Customer
                {
                    UserName = txtCustomerName.Text,
                    UserEmail = txtCustomerEmail.Text,
                    Status = cmbStatus.Text,
                    PhoneNumber = txtCustomerPhone.Text,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now), 
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

        private async void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdCustomer.SelectedItem is Customer selectedCustomer)
                {
                    selectedCustomer.UserName = txtCustomerName.Text;
                    selectedCustomer.UserEmail = txtCustomerEmail.Text;
                    selectedCustomer.Status = cmbStatus.Text;
                    selectedCustomer.PhoneNumber = txtCustomerPhone.Text;
                    selectedCustomer.Address = txtCustomerAddress.Text;
                    selectedCustomer.DateOfBirth = DateOnly.Parse(txtDateOfBirth.Text);
                    selectedCustomer.Password = txtPassword.Password;
                    selectedCustomer.PreferredLanguage = txtPreferredLanguage.Text;
                    selectedCustomer.ImageUrl = txtImageUrl.Text;

                    var result = await _customerBusiness.UpdateAsync(selectedCustomer);
                    MessageBox.Show(result.Message, "Update");
                    Load();
                }
                else
                {
                    MessageBox.Show("Please select a customer to update.", "Update");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private async void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grdCustomer.SelectedItem is Customer selectedCustomer)
                {
                    var result = await _customerBusiness.DeleteCustomer(selectedCustomer.UserId);
                    MessageBox.Show(result.Message, "Delete");
                    Load();
                }
                else
                {
                    MessageBox.Show("Please select a customer to delete.", "Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void grdCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grdCustomer.SelectedItem is Customer selectedCustomer)
            {
                txtCustomerId.Text = selectedCustomer.UserId.ToString();
                txtCustomerName.Text = selectedCustomer.UserName;
                txtCustomerEmail.Text = selectedCustomer.UserEmail;
                txtCustomerPhone.Text = selectedCustomer.PhoneNumber;
                txtCustomerAddress.Text = selectedCustomer.Address;
                txtDateOfBirth.Text = selectedCustomer.DateOfBirth.ToString();
                txtPassword.Password = selectedCustomer.Password;
                txtPreferredLanguage.Text = selectedCustomer.PreferredLanguage;
                txtImageUrl.Text = selectedCustomer.ImageUrl;
            }
        }
    }
}
