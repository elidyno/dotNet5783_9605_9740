using BlApi;
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
    /// Interaction logic for ProductForList.xaml
    /// Creates a product display window with the option to switch to the add or update window
    /// </summary>
    public partial class ProductForList : Window
    {
        private IBl bl = new BlImplementation.Bl();
        public ProductForList()
        {
            InitializeComponent();
            //The list view control gets the list of products
            ProductListview.ItemsSource = bl.Product.GetList();
            //The comboBox control accepts the category values
            CategorySelector.ItemsSource = Enum.GetValues(typeof(PL.Product.Category));
        }
        /// <summary>
        /// Category selection event handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((int)CategorySelector.SelectedValue == -1)// the "None" option was selected
                ProductListview.ItemsSource = bl.Product.GetList();
            //else
            //{
            //    ProductListview.ItemsSource = bl.Product.GetList(
            //    (BO.ProductForList product) => product.Category == (BO.Category)(CategorySelector.SelectedItem));
            //}
            else
            {
                ProductListview.ItemsSource = bl.Product.GetList(
                (BO.ProductForList product) => product.Category == (BO.Category)(CategorySelector.SelectedIndex));
            }
 ;
        }

        /// <summary>
        /// Handling the event of clicking on the add product button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            new Product().ShowDialog();
            ProductListview.ItemsSource = bl.Product.GetList();
        }
        /// <summary>
        /// Handling the event of clicking on a product from the list  
        /// calls the constructor of "product" with the product ID to open an update window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void product_selected(object sender, MouseButtonEventArgs e)
        {
            BO.ProductForList p = (BO.ProductForList)ProductListview.SelectedItem;
            new Product(p.Id).ShowDialog();
            ProductListview.ItemsSource = bl.Product.GetList();
        }

       
    }
}
