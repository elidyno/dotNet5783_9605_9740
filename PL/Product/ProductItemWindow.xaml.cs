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

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for Productitem.xaml
    /// </summary>
    public partial class ProductItemWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty ProductItemProperty = DependencyProperty.Register(
  "ProductItem", typeof(BO.ProductItem), typeof(ProductItemWindow),
  new PropertyMetadata(default(BO.ProductItem)));

        public BO.ProductItem ProductItem
        {
            get => (BO.ProductItem)GetValue(ProductItemProperty);
            set => SetValue(ProductItemProperty, value);
        }
        //public ObservableCollection<BO.ProductItem?> ProductItems { get; set; }
        static BO.Cart cart = new BO.Cart();

        /// <summary>
        /// A `Cart` object used to store items
        /// </summary>
        public BO.Cart Cart = new BO.Cart();

        public ProductItemWindow(BO.ProductItem item, BO.Cart cart)
        {
            ProductItem = item;
            Cart = cart;
            InitializeComponent();
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Add the product item to the cart
                Cart = bl!.Cart.Add(Cart, ProductItem!.Id);
            }
            catch (Exception ex)
            {
                // Show an error message if the product item could not be added to the cart
                MessageBox.Show("Can't add product to cart:\n" + ex.Message);
            }
            this.Close();
            new Catalogue(Cart).Show();
        }

    }
    
}
