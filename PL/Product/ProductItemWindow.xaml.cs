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
    /// Interaction logic for Productitem.xaml
    /// </summary>
    public partial class ProductItemWindow : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty ProductItemProperty = DependencyProperty.Register(
  "ProductItem", typeof(BO.ProductItem), typeof(ProductItemWindow),
  new PropertyMetadata(default(BO.ProductItem)));

        public BO.ProductItem ProductItem
        {
            get => (BO.ProductItem)GetValue(ProductItemProperty);
            set => SetValue(ProductItemProperty, value);
        }
        //public ObservableCollection<BO.ProductItem?> ProductItems { get; set; }
        static BO.Cart cart = new BO.Cart();
        public ProductItemWindow(BO.ProductItem item)
        {
            ProductItem = item;
            InitializeComponent();
        }
    }
}
