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

namespace PL
{
    /// <summary>
    /// Interaction logic for AdminPassWord.xaml
    /// </summary>
    public partial class AdminPassWord : Window
    {
        public AdminPassWord()
        {
            InitializeComponent();
        }

        private void Chack_Click(object sender, RoutedEventArgs e)
        {
            if (AdminPassword.Password == "123456")
            {
                MainWindow.AdminAccess = true;
                this.Close();
            }
                
            else
                MessageBox.Show("ERROR: Password is invalid");

        }
    }
}
