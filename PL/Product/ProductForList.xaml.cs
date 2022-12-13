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
            AttributeSelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
           
        }

        private void AttributeSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //AttributeSelector.SelectedItem
            ProductListview.ItemsSource = bl.Product.GetList(
                (BO.ProductForList product) => product.Category == (BO.Category)(AttributeSelector.SelectedItem));
        }

        private void CategoryLableMouseKlick(object sender, MouseButtonEventArgs e)
        {
            ProductListview.ItemsSource = bl.Product.GetList();
        }
    }
}
