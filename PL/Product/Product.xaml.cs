using PL.Order;
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
using System.Globalization;


namespace PL.Product
{
    /// <summary>
    /// Creates a window for adding a product or updating an existing product
    /// </summary>
    public partial class Product : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public bool IsDisplayMode { get; set; }
        public bool IsAddWindow { get; set; }
        public bool IsEditMode { get; set; }
        public string ButtonContent { get; set; }
        public Array categories { get; set; }

        public static readonly DependencyProperty productProperty = DependencyProperty.Register(
        "product", typeof(BO.Product), typeof(Product), new PropertyMetadata(default(BO.Product)));

        public BO.Product product
        {
            get => (BO.Product)GetValue(productProperty);
            set => SetValue(productProperty, value);
        }
        /// <summary>
        /// Constructor for creating a window to add a product
        /// </summary>
        public Product(bool isDisplayMode, bool isAddWindow, int productId = 0)
        {
            IsDisplayMode = isDisplayMode;
            IsEditMode = !isDisplayMode;
            IsAddWindow = isAddWindow;
            categories = Enum.GetValues(typeof(BO.Category));
            ButtonContent = IsAddWindow ? "Add" : "Update";
            product = new();
            InitializeComponent();
            if(!isAddWindow)
            {
                try
                {
                    product = bl!.Product.Get(productId);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
           
        }

        /// <summary>
        /// Handles the button click event according to the button type created in the constructor
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOrUpdate_Click(object sender, RoutedEventArgs e)
        {

            if (IsAddWindow)
            {
               
                try
                {
                    bl?.Product.Add(product);     
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                    return;
                }
                this.Close();
            }
            //Button == Update
            else 
            {
                
                try
                {
                    bl?.Product.Update(product);                
                }
                catch (Exception E)
                {
                    MessageBox.Show(E.Message);
                    return;
                }
                this.Close();
            }

        }

        private void OnlyNumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Check if the entered character is a digit or a negative sign (for negative numbers)
            if (!char.IsDigit(e.Text, e.Text.Length - 1) && !(e.Text == "-" && GetId.SelectionStart == 0))
            {
                // If it's not a digit or negative sign, cancel the input
                e.Handled = true;
            }
        }
    }
}
