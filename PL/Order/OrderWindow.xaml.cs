using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


namespace PL.Order
{
    
    /// <summary>
    /// Interaction logic for OrderWindow.xaml
    /// </summary>
    public partial class OrderWindow : Window
    {
        public static readonly DependencyProperty OrderProperty = DependencyProperty.Register(
        "Order", typeof(BO.Order), typeof(OrderWindow), new PropertyMetadata(default(BO.Order)));

        public BO.Order Order
        {
            get => (BO.Order)GetValue(OrderProperty);
            set => SetValue(OrderProperty, value);
        }
        BlApi.IBl? bl = BlApi.Factory.Get();       
        public bool IsDisplayMode { get; set; }
       
        public ObservableCollection<BO.OrderItem?>? items { get; set; }
        public OrderWindow(bool isDisplayMode, int orderId)
        {           
            Order = new BO.Order();
            Order = bl!.Order.Get(orderId);
            IsDisplayMode = isDisplayMode;
            items = new ObservableCollection<BO.OrderItem?>(Order.Items);
            InitializeComponent();  
           
        }

        private void updateShip_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order = bl!.Order.UpdateOrderSheep(Order.Id);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }

        private void updateDelivery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Order = bl!.Order.UpdateOrderDelivery(Order.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
