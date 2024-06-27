using MilkShop.Business.OrderBusinesses;
using MilkShop.Business.OrderDetailBusinesses;
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
    /// Interaction logic for wOrder.xaml
    /// </summary>
    public partial class wOrder : Window
    {
        private readonly OrderBusiness _orderBusiness;
        private readonly OrderDetailBusiness _orderDetailBusiness;
        public wOrder()
        {
            InitializeComponent();
            _orderBusiness ??= new OrderBusiness();
            _orderDetailBusiness ??= new OrderDetailBusiness();
            LoadGrdOrder();
        }
        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int orderId = -1;
                int.TryParse(txtOrderId.Text, out orderId);
                var item = await _orderBusiness.FindById(orderId);
                if (item.Data == null)
                {
                    var NewOrder = new Order()
                    {
                        OrderDate = DateTime.Parse(txtOrderDate.Text),
                        OrderStatus = txtOrderStatus.Text,
                        OrderTotalAmount = decimal.Parse(txtOrderTotalAmount.Text),
                        UserId = int.Parse(txtUserId.Text),
                        PaymentMethodId = (txtPaymentMethodId.Text),
                        PaymentStatus = txtPaymentStatus.Text,
                        Status = txtStatus.Text,
                        CreatedDate = DateTime.Now.ToString(),
                        ShippingAddress = txtShippingAddress.Text,
                        BillingAddress = txtBillingAddress.Text,
                        ShippingMethod = txtShippingMethod.Text
                    };

                    var result = await _orderBusiness.Save(NewOrder);
                    MessageBox.Show(result.Message, "Save");
                }
                else
                {
                    var order = item.Data as Order;
                    order.OrderDate = DateTime.Parse(txtOrderDate.Text);
                    order.OrderStatus = txtOrderStatus.Text;
                    order.OrderTotalAmount = decimal.Parse(txtOrderTotalAmount.Text);
                    order.UserId = int.Parse(txtUserId.Text);
                    order.PaymentMethodId = (txtPaymentMethodId.Text);
                    order.PaymentStatus = txtPaymentStatus.Text;
                    order.Status = txtStatus.Text;
                    order.CreatedDate = DateTime.Now.ToString();
                    order.ShippingAddress = txtShippingAddress.Text;
                    order.BillingAddress = txtBillingAddress.Text;
                    order.ShippingMethod = txtShippingMethod.Text;

                    var result = await _orderBusiness.UpdateAsync(order);
                    MessageBox.Show(result.Message, "Update");
                }

                ClearFields();
                LoadGrdOrder();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error");
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtOrderId.Text = string.Empty;
            txtOrderDate.Text = string.Empty;
            txtOrderStatus.Text = string.Empty;
            txtOrderTotalAmount.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtPaymentMethodId.Text = string.Empty;
            txtPaymentStatus.Text = string.Empty;
            txtStatus.Text = string.Empty;
            txtCreatedDate.Text = string.Empty;
            txtShippingAddress.Text = string.Empty;
            txtBillingAddress.Text = string.Empty;
            txtShippingMethod.Text = string.Empty;
        }

        private async void grdOrder_MouseDouble_Click(object sender, RoutedEventArgs e)
        {
            DataGrid grd = sender as DataGrid;
            if (grd != null && grd.SelectedItems != null && grd.SelectedItems.Count == 1)
            {
                var row = grd.ItemContainerGenerator.ContainerFromItem(grd.SelectedItem) as DataGridRow;
                if (row != null)
                {
                    var item = row.Item as Order;
                    if (item != null)
                    {
                        var Result = await _orderBusiness.GetById(item.OrderId);

                        if (Result.Status > 0 && Result.Data != null)
                        {
                            var order = Result.Data as Order;
                            txtOrderId.Text = order.OrderId.ToString();
                            txtOrderDate.Text = order.OrderDate.ToString();
                            txtOrderStatus.Text = order.OrderStatus;
                            txtOrderTotalAmount.Text = order.OrderTotalAmount.ToString();
                            txtUserId.Text = order.UserId.ToString();
                            txtPaymentMethodId.Text = order.PaymentMethodId.ToString();
                            txtPaymentStatus.Text = order.PaymentStatus;
                            txtStatus.Text = order.Status;
                            txtCreatedDate.Text = order.CreatedDate.ToString();
                            txtShippingAddress.Text = order.ShippingAddress;
                            txtBillingAddress.Text = order.BillingAddress;
                            txtShippingMethod.Text = order.ShippingMethod;
                        }
                    }
                }
            }
        }

        private async void grdOrder_ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;

            int? orderId;
            if (btn.CommandParameter != null && int.TryParse(btn.CommandParameter.ToString(), out int value))
            {
                orderId = value;
            }
            else
            {
                MessageBox.Show("Invalid Order ID format. Please try again.", "Error", MessageBoxButton.OK);
                return;
            }

            if (MessageBox.Show("Do you want to delete this order?", "Delete", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                var resultX = await _orderDetailBusiness.DeleteByOrderId(orderId.Value);
                var result = await _orderBusiness.DeleteAsync(orderId.Value);
               
                MessageBox.Show($"{result.Message}, {resultX.Message}", "Delete");
                LoadGrdOrder();
            }
        }

        private async void LoadGrdOrder()
        {
            var result = await _orderBusiness.GetAll();
            if (result.Status > 0 && result.Data != null)
            {
                grdOrder.ItemsSource = null;
                grdOrder.ItemsSource = result.Data as List<Order>;
            }
            else
            {
                grdOrder.ItemsSource = new List<Order>();
            }
        }

        private void grdOrder_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
