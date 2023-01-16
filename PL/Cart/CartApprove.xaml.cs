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

namespace PL.Cart
{
    /// <summary>
    /// Interaction logic for CartApprove.xaml
    /// </summary>
    public partial class CartApprove : Window
    {
        BlApi.IBl? bl = BlApi.Factory.Get();

        public static readonly DependencyProperty CartProperty = DependencyProperty.Register(
        "Cart", typeof(BO.Cart), typeof(CartWindow), new PropertyMetadata(default(BO.Cart)));

        public BO.Cart Cart
        {
            get => (BO.Cart)GetValue(CartProperty);
            set => SetValue(CartProperty, value);
        }
        public CartApprove(BO.Cart Cart_)
        {
            Cart = Cart_;
            InitializeComponent();
        }

        private void ApproveCart_Click(object sender, RoutedEventArgs e)
        {
            int? orderId;
            if (Name == null || Email == null || Adress == null)
                MessageBox.Show("please fill all faildes befor send order to approve");
            string name = Name.Text;
            string email = Email.Text;
            string adress = Adress.Text;
            try
            {
                orderId = bl?.Cart.Approve(Cart, name, email, adress);
            }
            catch (Exception ex)
            {

               MessageBox.Show("Unable to Approve your Ordder:\n" + ex.Message);
                return;
            }

            MessageBox.Show("The order hs been approval succesfuly\n your Order Id for Tracking: " + orderId);

        }
    }
}
