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
using DO;
using PL.Cart;
using PL.Product;

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CartWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        
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
            set => SetValue(OrderItemsProperty, value);
        }

        public static readonly DependencyProperty TotalPriceProperty = DependencyProperty.Register(
        "TotalPrice", typeof(double), typeof(CartWindow), new PropertyMetadata(default(double)));

        public double TotalPrice
        {
            get => (double)GetValue(TotalPriceProperty);
            set => SetValue(TotalPriceProperty, value);
        }

        public static readonly DependencyProperty ItemsCountProperty = DependencyProperty.Register(
        "ItemsCount", typeof(int), typeof(CartWindow), new PropertyMetadata(default(int)));

        public int ItemsCount
        {
            get => (int)GetValue(ItemsCountProperty);
            set => SetValue(ItemsCountProperty, value);
        }


        public static bool empthyCart;

        public CartWindow(BO.Cart cart_)
        {
            InitializeComponent();
            Cart = cart_;
            if (Cart.Items != null)
            {
                empthyCart = false;
                OrderItems = Cart.Items.ToList();
                ItemsCount = Cart.Items.Count;
                TotalPrice = (double)Cart.TotalPrice;
            }
            else
                empthyCart = true;
        }

        private void AddItemAmount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button!.Tag as BO.OrderItem;
            BO.Cart tempCart = new BO.Cart();
            try
            {
                tempCart = bl!.Cart.Add(Cart, item.ProductId);
            }
            catch (Exception ex)
            {

                MessageBox.Show("Error: unable to add item amount:\n" + ex.Message);
                return;
            }
           
            Cart = tempCart;
            OrderItems = Cart.Items.ToList();
            ItemsCount = Cart.Items.Count;
            TotalPrice = (double)Cart.TotalPrice;
        }


        private void SubAmount_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderItem = button!.Tag as BO.OrderItem;
            try
            {
                Cart = bl!.Cart.Sub(Cart, orderItem.ProductId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to sub item amount:\n" + ex.Message);
                return;
            }

            OrderItems = Cart.Items.ToList();
            ItemsCount = Cart.Items.Count;
            TotalPrice = (double)Cart.TotalPrice;
        }

        private void RemoveItem_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderItem = button!.Tag as BO.OrderItem;
            try
            {
                Cart = bl!.Cart.Remove(Cart, orderItem.ProductId);
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to Remove item From Cart:\n" + ex.Message);
                return;
            }

            OrderItems = Cart.Items.ToList();
            ItemsCount = Cart.Items.Count;
            TotalPrice = (double)Cart.TotalPrice;
        }

        private void ClearCart_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cart = bl!.Cart.RemoveAll(Cart);
            }
            catch (Exception ex)
            {
                MessageBox.Show("unable to Remove all Items From Cart:\n" + ex.Message);
                return;
            }

            OrderItems = null;
            ItemsCount = 0;
            TotalPrice = 0;
        }

        private void Approve_Click(object sender, RoutedEventArgs e)
        {
            if (Cart == null || Cart.Items == null)
                MessageBox.Show("Unable to create an order: Cart is empthy");
            new CartApprove(Cart).ShowDialog();
            this.Close();
            
        }

        private void BackWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new Catalogue(Cart).Show();
        }
    }
}
