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
using PL.Product;

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderListWindow.xaml
    /// </summary>
    public partial class OrderListWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public ObservableCollection<BO.OrderForList?> orderList { get; set; }
        public OrderListWindow()
        {
            orderList = new ObservableCollection<BO.OrderForList?>(bl!.Order.GetList());
            InitializeComponent();
           
        }
        
        //Opens a window to display order details, in case of selecting an order from the list.
        private void orderSelected(object sender, MouseButtonEventArgs e)
        {
            BO.OrderForList orderForList = (BO.OrderForList)orderListView.SelectedItem;
            OrderWindow orderWindow = new (false, orderForList.Id);
            orderWindow.Show();
        }
        //A window opens to display a list of products
        private void Products_Click(object sender, RoutedEventArgs e)
        {
            ProductForList ProductsWindow = new ProductForList();
            ProductsWindow.Show();
            
        }
        //Displays the order list
        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            
            orderListView.Visibility = Visibility.Visible;
        }
        //Return to main window
        private void back_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
