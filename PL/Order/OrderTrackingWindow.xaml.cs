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
        //IBl instance variable used for accessing business logic methods
        BlApi.IBl? bl = BlApi.Factory.Get();
        //OrderId for the tracking order
        public int OrderId;
        //Property for the OrderTracking object
        public BO.OrderTracking orderTracking { get; set; }
        //Constructor that initializes OrderTracking object and OrderId
        public OrderTrackingWindow(int orderId)
        {
            orderTracking = new BO.OrderTracking();
            orderTracking = bl!.Order.GetTracking(orderId);
            OrderId = orderId;
            InitializeComponent();
            
        }
        /// <summary>
        /// Event handler for the "order_details" button click event
        /// </summary>
        /// <param name="sender">Object that raised the event</param>
        /// <param name="e">Event arguments</param>
        private void order_details_Click(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new(true, OrderId);
            orderWindow.Show();
        }
    }
}
