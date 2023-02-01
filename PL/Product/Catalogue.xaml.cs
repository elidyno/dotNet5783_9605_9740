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
using PL.Product;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for Catalogue.xaml
    /// </summary>
    public partial class Catalogue : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        #region Dependency Property ProductItems
        /// <summary>
        /// The Dependency Property for the `ProductItems` property
        /// </summary>
        public static readonly DependencyProperty ProductItemProperty = DependencyProperty.Register(
          "ProductItems", typeof(IEnumerable<BO.ProductItem?>), typeof(Catalogue),
          new PropertyMetadata(default(IEnumerable<BO.ProductItem?>)));

        /// <summary>
        /// Gets or sets the list of `ProductItem` objects
        /// </summary>
        public IEnumerable<BO.ProductItem?> ProductItems
        {
            get => (List<BO.ProductItem?>)GetValue(ProductItemProperty);
            set => SetValue(ProductItemProperty, value);
        }
        #endregion

        /// <summary>
        /// A `Cart` object used to store items
        /// </summary>
        public BO.Cart Cart = new BO.Cart();

        /// <summary>
        /// Initializes a new instance of the `Catalogue` class
        /// </summary>
        /// <param name="cart_">An optional `Cart` object</param>
        public Catalogue(BO.Cart? cart_ = null)
        {
            if (cart_ != null)
                Cart = cart_;
            InitializeComponent();
            try
            {
                ProductItems = bl!.Product.GetItemList(Cart);
            }
            catch (Exception e)
            {
                MessageBox.Show("Error was created in our Application:\n" + e.Message + "\n please try again");
            }

            //The comboBox control accepts the category values
            BO.Category category = 0;
            for (int i = 0; i < Enum.GetValues(typeof(BO.Category)).Length; i++)
            {
                CategoryComboBox.Items.Add(category++);
            }
            CategoryComboBox.Items.Add("All");
            CategoryComboBox.SelectedIndex = Enum.GetValues(typeof(BO.Category)).Length; // the "All" option was selected
        }


        // This method handles the selection change event for the category combobox.
        // Depending on the selected category, it populates the product items list.
        private void CategorySelected(object sender, SelectionChangedEventArgs e)
        {
            // If the "All" option was selected
            if (CategoryComboBox.SelectedIndex == Enum.GetValues(typeof(BO.Category)).Length)
                ProductItems = bl!.Product.GetItemList(Cart);
            else
            {
                // Filter the product items based on the selected category
                ProductItems = bl!.Product.GetItemList(Cart,
                   (productItem) => productItem.Category == (BO.Category)(CategoryComboBox.SelectedIndex));
            }
        }

        // This method handles the click event of the "Add" menu item.
        // It adds a selected product item to the cart.
        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected product item
            BO.ProductItem? productItem = new BO.ProductItem();
            productItem = catalogueListView.SelectedItem as BO.ProductItem;

            try
            {
                // Add the product item to the cart
                Cart = bl!.Cart.Add(Cart, productItem!.Id);

                // Populate the product items list depending on the selected category
                if (CategoryComboBox.SelectedIndex == Enum.GetValues(typeof(BO.Category)).Length) // the "All" option was selected
                    ProductItems = bl!.Product.GetItemList(Cart);
                else
                {
                    ProductItems = bl!.Product.GetItemList(Cart,
                       (productItem) => productItem.Category == (BO.Category)(CategoryComboBox.SelectedIndex));
                }
            }
            catch (Exception ex)
            {
                // Show an error message if the product item could not be added to the cart
                MessageBox.Show("Can't add product to cart:\n" + ex.Message);
            }
        }

        // This method handles the click event of the "View" menu item.
        // It opens a window to view the details of a selected product item.
        private void ViewMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected product item
            BO.ProductItem? productItem = new BO.ProductItem();
            productItem = ProductItems.ToList().Find(x => x == catalogueListView.SelectedItem as BO.ProductItem);

            // Open a window to view the details of the product item
            new ProductItemWindow(productItem, Cart).ShowDialog();
            this.Close();
        }

        // This method handles the click event of the "My Cart" menu item.
        // It opens a window to view the contents of the cart.
        private void MyCart_Click(object sender, RoutedEventArgs e)
        {
            // Open a window to view the contents of the cart
            new CartWindow(Cart).ShowDialog();
            // Close the current window
            this.Close();
        }
    }
}
