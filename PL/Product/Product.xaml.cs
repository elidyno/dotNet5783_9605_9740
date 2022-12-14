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
    /// Interaction logic for Product.xaml
    /// </summary>
    public partial class Product : Window
    {
        private IBl bl = new BlImplementation.Bl();
        public Product()
        {
            InitializeComponent();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
            AddOrUpdate.Name = "Add";
            AddOrUpdate.Content = "Add";
        }

        public Product(int productId)
        {
            InitializeComponent();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
            AddOrUpdate.Name = "Update";
            AddOrUpdate.Content = "Update";
            BO.Product product = new();
            try
            {
               product = bl.Product.Get(productId);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
            GetId.Text = productId.ToString();
            CategorySelector.Text = product.Category.ToString();
            GetName.Text = product.Name;
            GetPrice.Text = product.Price.ToString();
            GetInStock.Text = product.InStock.ToString();

        }

        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            
            if (button?.Name == "Add")
            {
                BO.Product product = new()
                {
                    Category = (BO.Category)CategorySelector.SelectedItem,
                    Name = GetName.Text,
                    Id = int.Parse(GetId.Text),
                    Price = int.Parse(GetPrice.Text),
                    InStock = int.Parse(GetInStock.Text),
                };
                try
                {
                    bl.Product.Add(product);
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }
            //Button = Update
            else
            {
                try
                {
                    bl.Product.Update(bl.Product.Get(int.Parse(GetId.Text)));
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }

        }
    }
}
