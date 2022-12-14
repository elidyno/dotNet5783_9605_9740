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
    /// </summary>
    public partial class ProductForList : Window
    {
        private IBl bl = new BlImplementation.Bl();
        public ProductForList()
        {
            InitializeComponent();
            ProductListview.ItemsSource = bl.Product.GetList();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
           
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
    
            ProductListview.ItemsSource = bl.Product.GetList(
                (BO.ProductForList product) => product.Category == (BO.Category)(CategorySelector.SelectedItem));
        }

        private void CategoryLableMouseKlick(object sender, MouseButtonEventArgs e)
        {
            ProductListview.ItemsSource = bl.Product.GetList();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Product().Show();
        }
    }
}
