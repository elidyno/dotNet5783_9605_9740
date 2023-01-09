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
using BO;
using PL.Product;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for Catalogue.xaml
    /// </summary>
    public partial class Catalogue : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty ProductItemProperty = DependencyProperty.Register(
  "ProductItems", typeof(IEnumerable<BO.ProductItem?>), typeof(Catalogue),
  new PropertyMetadata(default(IEnumerable<BO.ProductItem?>)));

        public IEnumerable<BO.ProductItem?> ProductItems
        {
            get => (List<BO.ProductItem?>)GetValue(ProductItemProperty);
            set => SetValue(ProductItemProperty, value);
        }
        //public ObservableCollection<BO.ProductItem?> ProductItems { get; set; }
        static BO.Cart cart  = new BO.Cart();

        public Catalogue()
        {
            InitializeComponent();
            try
            {
                ProductItems = bl!.Product.GetItemList(cart);
            }
            catch (Exception e)
            {

                MessageBox.Show("Error whas created in our Application:\n" + e.Message + "\n please try again");
            }
            
            //The comboBox control accepts the category values
            BO.Category category = 0;
            for (int i = 0; i < Enum.GetValues(typeof(BO.Category)).Length; i++)
            {
                CategoryComboBox.Items.Add(category++);
            }
            CategoryComboBox.Items.Add("All");

        }

        

        private void catalogueListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void catalogueListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CategorySelected(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedIndex == Enum.GetValues(typeof(BO.Category)).Length) // the "All" option was selected
                ProductItems = bl!.Product.GetItemList(cart);
            else
            {
                ProductItems = bl!.Product.GetItemList(cart,
                   (BO.ProductItem productItem) => productItem.Category == (BO.Category)(CategoryComboBox.SelectedIndex));
            }
        }
    }
}
