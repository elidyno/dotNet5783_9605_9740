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
    /// Creates a window for adding a product or updating an existing product
    /// </summary>
    public partial class Product : Window
    {
        private IBl bl = new BlImplementation.Bl();
        /// <summary>
        /// Constructor for creating a window to add a product
        /// </summary>
        public Product()
        {
            InitializeComponent();
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
            AddOrUpdate.Name = "Add";
            AddOrUpdate.Content = "Add";
        }
        /// <summary>
        /// Constructor for creating a window to update a product
        /// </summary>
        /// <param name="productId"></param>
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
        /// <summary>
        /// Handles the button click event according to the button type created in the constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {
            int id, price, inStock;
            //Trying to get a valid value from the user
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
            //Creates a product entity of the user's choice
            BO.Product product = new()
            {
                Category = (BO.Category)CategorySelector.SelectedItem,
                Name = GetName.Text,
                Id = id,
                Price = price,
                InStock = inStock,
            };

            Button? button = sender as Button;
            //button == Add
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
            //Button == Update
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
