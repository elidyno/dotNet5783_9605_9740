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
using PL.Product;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for Catalogue.xaml
    /// </summary>
    public partial class Catalogue : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public ObservableCollection<BO.ProductItem?> ProductItems { get; set; }
        static BO.Cart cart  = new BO.Cart();

        public Catalogue()
        {

            try
            {
                ProductItems = new ObservableCollection<BO.ProductItem?>(bl!.Product.GetItemList(cart));
            }
            catch (Exception e)
            {

                MessageBox.Show("Error whas created in our Application:\n" + e.Message + "\n please try again");
            }
            InitializeComponent();
            //catalogueListView.ItemsSource = ProductItems;

        }

        

        private void catalogueListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
