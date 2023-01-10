using BO;
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

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderTrackingWindow.xaml
    /// </summary>
    public partial class OrderTrackingWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public int OrderId;
        public BO.OrderTracking orderTracking { get; set; }
        public OrderTrackingWindow(int orderId)
        {
            orderTracking = new BO.OrderTracking();
            orderTracking = bl!.Order.GetTracking(orderId);
            OrderId = orderId;
            InitializeComponent();
            //TrackingData.Text = bl?.Order.GetTracking(orderId).ToString();
        }

        private void order_details_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new(true, OrderId);
            orderWindow.Show();
        }
    }
}
