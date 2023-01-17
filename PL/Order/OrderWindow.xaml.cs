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
        //Dependency Property "Order" for holding order data
        public BO.Order Order
        {
            get => (BO.Order)GetValue(OrderProperty);
            set => SetValue(OrderProperty, value);
        }
        BlApi.IBl? bl = BlApi.Factory.Get();       
        public bool IsDisplayMode { get; set; }

        //List for the order products
        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register(
        "Items", typeof(IEnumerable<BO.OrderItem?>), typeof(OrderWindow), new PropertyMetadata(default(IEnumerable<BO.OrderItem?>)));
        //Dependency Property "Order" for holding order data
        public IEnumerable<BO.OrderItem?> Items
        {
            get => (IEnumerable<BO.OrderItem?>)GetValue(ItemsProperty);
            set => SetValue(ItemsProperty, value);
        }
        public OrderWindow(bool isDisplayMode, int orderId)
        {           
            Order = new BO.Order();
            Order = bl!.Order.Get(orderId);
            IsDisplayMode = isDisplayMode;
            Items = Order.Items!.ToList();
            InitializeComponent();  
           
        }
        //Handles order shipping update request
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
        //Handles order delivery update request
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
