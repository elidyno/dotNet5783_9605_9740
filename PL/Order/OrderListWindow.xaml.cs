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
    /// Interaction logic for OrderListWindow.xaml
    /// </summary>
    public partial class OrderListWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        private ObservableCollection<BO.OrderForList?> orderList;
        public OrderListWindow()
        {
            InitializeComponent();
            orderList = new ObservableCollection<BO.OrderForList?>(bl!.Order.GetList());
            DataContext = orderList;
            // orderList(bl?.Order.GetList);
        }

        private void orderSelected(object sender, MouseButtonEventArgs e)
        {
            BO.OrderForList orderForList = (BO.OrderForList)orderListView.SelectedItem;
            OrderWindow orderWindow = new ();
            //orderWindow.IsEditMode = true;
            orderWindow.OrderId = orderForList.Id;
            orderWindow.Show();
        }
    }
}
