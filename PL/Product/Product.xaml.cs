using BlApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            GetId.IsReadOnly = true;
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
            int id = 0, price = 0, inStock = 0;
            try
            {
                id = int.Parse(GetId.Text);
                price = int.Parse(GetPrice.Text);
                inStock = int.Parse(GetInStock.Text);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
                return;
            }
            
            BO.Product product = new()
            {
                Category = (BO.Category)CategorySelector.SelectedItem,
                Name = GetName.Text,
                Id = id,
                Price = price,
                InStock = inStock,
            };

            Button? button = sender as Button;
            
            if (button?.Name == "Add")
            {     
                try
                {
                    bl.Product.Add(product);
                    this.Close();
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
                    bl.Product.Update(product);
                    this.Close();
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                }
            }

        }
    }
}
