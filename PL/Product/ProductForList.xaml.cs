using PL.Order;
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

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for ProductForList.xaml
    /// Creates a product display window with the option to switch to the add or update window
    /// </summary>
    public partial class ProductForList : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty ProductListProperty = DependencyProperty.Register(
        "ProductList", typeof(IEnumerable<BO.ProductForList?>), typeof(ProductForList),
        new PropertyMetadata(default(IEnumerable<BO.ProductForList?>)));

        public  IEnumerable<BO.ProductForList?> ProductList
        {
            get => (List<BO.ProductForList?>)GetValue(ProductListProperty);
            set => SetValue(ProductListProperty, value);
        }
       
        public ProductForList()
        {
            InitializeComponent();
            ProductList = bl!.Product.GetList();
            
            //The comboBox control accepts the category values
            BO.Category category = 0;
            for (int i = 0; i < Enum.GetValues(typeof(BO.Category)).Length ; i++)
            {
                CategorySelector.Items.Add(category++);
            }
            CategorySelector.Items.Add("All");

        }
        /// <summary>
        /// Category selection event handling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategorySelector.SelectedIndex == Enum.GetValues(typeof(BO.Category)).Length) // the "All" option was selected
                ProductList = bl!.Product.GetList();
            else
            {
                ProductList = bl!.Product.GetList(
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
            new Product(true,true).ShowDialog();
            ProductList = bl!.Product.GetList();
            //if (CategorySelector.SelectedIndex == Enum.GetValues(typeof(BO.Category)).Length) // the "All" option was selected
            //    ProductList = bl!.Product.GetList();
            //else
            //{
            //    ProductList = bl!.Product.GetList(
            //       (BO.ProductForList product) => product.Category == (BO.Category)(CategorySelector.SelectedIndex));
            //}
            ///ProductListview.ItemsSource = bl?.Product.GetList();
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
            new Product(true, false, p.Id).ShowDialog();
            ProductList = bl!.Product.GetList();
            //if (CategorySelector.SelectedIndex == Enum.GetValues(typeof(BO.Category)).Length) // the "All" option was selected
            //    ProductList = bl!.Product.GetList();
            //else
            //{
            //    ProductList = bl!.Product.GetList(
            //       (BO.ProductForList product) => product.Category == (BO.Category)(CategorySelector.SelectedIndex));
            //}
        }

    }
}
