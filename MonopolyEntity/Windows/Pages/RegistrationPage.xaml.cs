using Microsoft.Win32;
using MonopolyDLL.Monopoly;
using MonopolyDLL;
using MonopolyEntity.VisualHelper;
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

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegistrationPage.xaml
    /// </summary>
    public partial class RegistrationPage : Page
    {
        private Frame _frame;
        private MonopolySystem _system;
        public RegistrationPage(Frame frame, MonopolySystem system)
        {
            _frame = frame;
            _system = system;
            InitializeComponent();
        }

        private User _newUser = new User();
        private void RegBut_Click(object sender, RoutedEventArgs e)
        {
            //check if one of the fields is empty
            _newUser.Login = LoginBox.Text;
            _newUser.Password = PasswordBox.Text;

            if (DBQueries.IfUserExistByLogin(_newUser.Login) || 
                LoginBox.Text == string.Empty ||
                PasswordBox.Text == string.Empty) return;
            DBQueries.AddNewUserInDB(_newUser);

            _frame.Content = new StartPage(_frame, _system);
        }

        private void LoginBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (LoginBox.Text == "Login")
            {
                LoginBox.Text = string.Empty;
                LoginBox.Foreground = Brushes.Black;
            }
        }

        private void LoginBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(LoginBox.Text))
            {
                LoginBox.Text = "Login";
                LoginBox.Foreground = Brushes.Gray;
            }
        }

        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (PasswordBox.Text == "Password")
            {
                PasswordBox.Text = string.Empty;
                PasswordBox.Foreground = Brushes.Black;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(PasswordBox.Text))
            {
                PasswordBox.Text = "Password";
                PasswordBox.Foreground = Brushes.Gray;
            }
        }

        private void ChoseImgBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
                Title = SystemParamsServeses.GetStringByName("RegistrationChooseImage")
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fullImgPath = openFileDialog.FileName;
                UserImg.Source = new BitmapImage(new Uri(fullImgPath));

                string newImagePath = System.IO.Path.Combine(MainWindowHelper.GetUserImagePath(), openFileDialog.SafeFileName);
                DBQueries.AddPicture(openFileDialog.SafeFileName);
                _newUser.SetPictureId(DBQueries.GetLastPicId());

                System.IO.File.Copy(fullImgPath, newImagePath, true);
                UserImg.Source = new BitmapImage(new Uri(newImagePath));
            }
        }

    }
}
