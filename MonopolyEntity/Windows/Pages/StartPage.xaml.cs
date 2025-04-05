using MonopolyDLL;
using MonopolyDLL.Monopoly;
using MonopolyEntity.Windows.UserControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Page
    {
        private MonopolySystem _monopolySys;
        private Frame _frame;
        public StartPage(Frame frame, MonopolySystem system)
        {
            _frame = frame;
            _monopolySys = system;
            InitializeComponent();
        }

        private void LoginBut_Click(object sender, RoutedEventArgs e)
        {
            User user = DBQueries.GetUserByLoginAndPassword(LoginTextBox.Text, PasswordTextBox.Password);


            if (user is null) return;
           // _monopolySys.LoggedUser = user;
            int id = user.GetId();

            _monopolySys = new MonopolySystem(id);

            //WorkPage page = new WorkPage(_monopolySys, _frame);
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new WorkPage(_monopolySys, _frame));
        }

        private void RegistrationBut_Click(object sender, RoutedEventArgs e)
        {
            _frame.Content = new RegistrationPage(_frame, _monopolySys);
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginTextBox.Text == SystemParamsService.GetStringByName("BaseLoginFieldName"))
            {
                //LoginTextBox.Text = string.Empty;
                LoginTextBox.Foreground = Brushes.Black;
            }
            LoginTextBox.BorderBrush = Brushes.Green;
        }
        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
            {
                //LoginTextBox.Text = "Login";
                LoginTextBox.Foreground = Brushes.Gray;
            }
            LoginTextBox.BorderBrush = Brushes.Transparent;
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordTextBox.Password == SystemParamsService.GetStringByName("BasePasswordFieldName"))
            {
                //PasswordTextBox.Password = string.Empty;
                PasswordTextBox.Foreground = Brushes.Black;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordTextBox.Password))
            {
                //PasswordTextBox.Password = "Password";
                PasswordTextBox.Foreground = Brushes.Gray;
            }
        }
    }
}
