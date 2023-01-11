using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
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
using BO;
using PL.Cart;

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        public static readonly DependencyProperty CartProperty = DependencyProperty.Register(
        "Cart", typeof(BO.Cart), typeof(CartWindow), new PropertyMetadata(default(BO.Cart)));

        public  BO.Cart Cart
        {
            get => (BO.Cart)GetValue(CartProperty);
            set => SetValue(CartProperty, value);
        }
        public static readonly DependencyProperty OrderItemsProperty = DependencyProperty.Register(
      "OrderItems", typeof(IEnumerable<BO.OrderItem?>), typeof(CartWindow), new PropertyMetadata(default(IEnumerable<BO.OrderItem?>)));

        public IEnumerable<BO.OrderItem?> OrderItems
        {
            get => (List<BO.OrderItem?>)GetValue(CartProperty);
            set => SetValue(CartProperty, value);
        }
        //public  ObservableCollection<BO.OrderItem?> OrderItems;
        public static bool empthyCart;

        public CartWindow(BO.Cart cart_)
        {
            InitializeComponent();
            Cart = cart_;
            if (Cart.Items != null)
            {
                empthyCart = false;
                OrderItems = new ObservableCollection<BO.OrderItem?>();
            }
            else
                empthyCart = true;
        }
    }
}
