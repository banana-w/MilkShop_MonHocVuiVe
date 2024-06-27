using MilkShop.Business.CompanyBusiness;
using MilkShop.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for wCompany.xaml
    /// </summary>
    public partial class wCompany : Window
    {
        private readonly CompanyBusiness _companyBusiness;
        public wCompany()
        {
            InitializeComponent();
            _companyBusiness ??=  new CompanyBusiness();
            LoadGrdMentor();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int companyId = -1;
                int.TryParse(txtCompanyCode.Text, out companyId);
                var item = await _companyBusiness.FindId(companyId);
                if (item.Data == null)
                {
                    var NewCompany = new Company()
                    {
                        CompanyName = txtCompanyName.Text,
                        CompanyAddress = txtAddress.Text,
                        CompanyPhone = txtPhone.Text,
                        CompanyEmail = txtEmail.Text,
                        CompanyFirstName = txtFirstName.Text,
                        Status = txtStatus.Text,
                        CreateDated = DateTime.Now
                    };

                    var result = await _companyBusiness.Save(NewCompany);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var company = item.Data as Company;
                    company.CompanyName = txtCompanyName.Text;
                    company.CompanyAddress = txtAddress.Text;
                    company.CompanyPhone = txtPhone.Text;
                    company.CompanyFirstName = txtFirstName.Text;
                    company.Status = txtStatus.Text;
                    company.CreateDated = DateTime.Now;

                    var result = await _companyBusiness.UpdateAsync(company);
                    MessageBox.Show(result.Message, "Update");
                }

                txtCompanyCode.Text = string.Empty;
                txtCompanyName.Text = string.Empty;
                txtAddress.Text = string.Empty;
                txtPhone.Text = string.Empty;
                txtFirstName.Text = string.Empty;
                txtStatus.Text = string.Empty;
                txtDateJoined.Text = string.Empty;
                LoadGrdMentor();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }

        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            txtCompanyCode.Text = string.Empty;
            txtCompanyName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtFirstName.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtDateJoined.Text = string.Empty;
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
                    var item = row.Item as Company;
                    if (item != null)
                    {
                        var Result = await _companyBusiness.GetById(item.CompanyId);

                        if (Result.Status > 0 && Result.Data != null)
                        {
                            var company = Result.Data as Company;
                            txtCompanyCode.Text = company.CompanyId.ToString();
                            txtCompanyName.Text = company.CompanyName;
                            txtAddress.Text = company.CompanyAddress;
                            txtPhone.Text = company.CompanyPhone;
                            txtFirstName.Text = company.CompanyFirstName;
                            txtStatus.Text = company.Status;
                            txtDateJoined.Text = company.CreateDated.ToString();
                        }
                    }
                }
            }
        }
        private async void grdMentor_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            // Attempt to convert CommandParameter to int
            int? companyId;
            if (btn.CommandParameter != null && int.TryParse(btn.CommandParameter.ToString(), out int value))
            {
                companyId = value;
            }
            else
            {
                // Handle the case where CommandParameter is not an integer or null
                MessageBox.Show("Invalid Company ID format. Please try again.", "Error", MessageBoxButton.OK);
                return;
            }

            // Confirmation and deletion logic (assuming InternId is valid)
            if (MessageBox.Show("Do you want to delete this company?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var result = await _companyBusiness.DeleteAsync(companyId.Value);
                MessageBox.Show($"{result.Message}", "Delete");
                LoadGrdMentor();
            }

        }
        private async void LoadGrdMentor()
        {
            var result = await _companyBusiness.GetAllCompany();
            if (result.Status > 0 && result.Data != null)
            {
                grdCompany.ItemsSource = null;
                grdCompany.ItemsSource = result.Data as List<Company>;
            }
            else
            {
                grdCompany.ItemsSource = new List<Company>();
            }
        }
    }
}
