using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
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

namespace TravelAppWpf.Views
{
    /// <summary>
    /// Interaction logic for SignInView.xaml
    /// </summary>
    public partial class SignInView : UserControl
    {
        public SignInView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SecureString secureString = new SecureString();
            foreach(var item in PasswordBox.Password)
            {
                secureString.AppendChar(item);
            }
            Messenger.Default.Send<SecureString>(secureString);
        }
    }
}
