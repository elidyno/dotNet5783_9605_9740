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
using System.Collections.ObjectModel;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for Catalogue.xaml
    /// </summary>
    public partial class Catalogue : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        IEnumerable<BO.ProductForList?> products = new List<BO.ProductForList?>();
        ObservableCollection<BO.ProductItem?> ProductItems = new ObservableCollection<BO.ProductItem?>();
        static BO.Cart cart  = new BO.Cart();

        public Catalogue()
        {
            InitializeComponent();
            DataContext = products;
            try
            {
                products = bl?.Product.GetList();
                foreach (var item in products)
                {
                    ProductItems.Add(bl.Product.Get(item.Id, cart));
                }
            }
            catch (Exception e)
            {

                MessageBox.Show("Error whas created in our Application:\n" + e.Message +"\n please try again");
            }



        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
