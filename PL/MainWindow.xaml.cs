﻿//using PL.Product;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();
        public static bool AdminAccess { get; set; } = false;

        public MainWindow()
        {
            
            InitializeComponent();

        }

        /// <summary>
        /// Opens a product list window and closes the main window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Admin_Click(object sender, RoutedEventArgs e)
        {
            new AdminPassWord().ShowDialog();
            if(!AdminAccess)
                return;
            new Product.ProductForList().Show();
            this.Close();
        }

        private void NewOrder_Click(object sender, RoutedEventArgs e)
        {
            new Product.Catalogue().ShowDialog();
        }

        private void OrderNumber_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Track_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
