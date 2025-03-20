using Microsoft.Win32;
using MonopolyDLL;
using MonopolyDLL.Monopoly;
using MonopolyEntity.VisualHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace MonopolyEntity.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UserSettings.xaml
    /// </summary>
    public partial class UserSettings : UserControl
    {
        private MonopolySystem _system;
        private Frame _frame;

        public UserSettings()
        {
            InitializeComponent();
        }

        public UserSettings(MonopolySystem system, Frame frame)
        {
            _system = system;
            _frame = frame;

            InitializeComponent();

            SetStartParams();
        }

        public void SetStartParams()
        {
            SetUserImage();

            SetLoginParam();
            SetOldPassword();
            SetNewPassword();
        }

        public void SetNewPassword()
        {
            UserNewPasswordParam.ParamNameBlock.Text = SystemParamsService.GetStringByName("SettingsNewPas");

            UserNewPasswordParam.ParamNameBox.TextChanged += (sender, e) =>
            {
                if(!string.IsNullOrWhiteSpace(UserNewPasswordParam.ParamNameBox.Text) &&
                UserOldPasswordParam.SaveBox.Visibility == Visibility.Visible)
                {
                    UserNewPasswordParam.SaveBox.Visibility = Visibility.Visible;
                    UserNewPasswordParam.CrossBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    UserNewPasswordParam.CrossBox.Visibility = Visibility.Visible;
                    UserNewPasswordParam.SaveBox.Visibility = Visibility.Hidden;
                }
            };

            UserNewPasswordParam.SaveBox.PreviewMouseDown += (sender, e) =>
            {
                if (UserOldPasswordParam.SaveBox.Visibility != Visibility.Visible) return;

                DBQueries.UpdateUserPassword(_system.LoggedUser.Login, UserNewPasswordParam.ParamNameBox.Text);
                _system.LoggedUser.Password = UserLoignParam.ParamNameBox.Text;

                UserOldPasswordParam.SaveBox.Visibility = Visibility.Hidden;
                UserOldPasswordParam.CrossBox.Visibility = Visibility.Visible;     
            };
        }

        public void SetOldPassword()
        {
            UserOldPasswordParam.ParamNameBlock.Text = SystemParamsService.GetStringByName("SettingsOldPas");
            UserOldPasswordParam.ParamPasswordBox.Visibility = Visibility.Visible;
            UserOldPasswordParam.ParamNameBox.Visibility = Visibility.Hidden;

            UserOldPasswordParam.ParamPasswordBox.PasswordChanged += (sender, e) =>
            {
                if(UserOldPasswordParam.ParamPasswordBox.Password != _system.LoggedUser.Password)
                {
                    UserOldPasswordParam.CrossBox.Visibility = Visibility.Visible;
                    UserOldPasswordParam.SaveBox.Visibility = Visibility.Hidden;

                    UserNewPasswordParam.CrossBox.Visibility = Visibility.Visible;
                    UserNewPasswordParam.SaveBox.Visibility = Visibility.Hidden;
                }
                else
                {
                    UserOldPasswordParam.CrossBox.Visibility = Visibility.Hidden;
                    UserOldPasswordParam.SaveBox.Visibility = Visibility.Visible;
                }
            };
        }

        public void SetLoginParam()
        {
            UserLoignParam.ParamNameBlock.Text = SystemParamsService.GetStringByName("SettingsUserLog");
            UserLoignParam.ParamNameBox.Text = _system.LoggedUser.Login;

            UserLoignParam.ParamNameBox.TextChanged += (sender, e) =>
            {
                if (_system.LoggedUser.Login != UserLoignParam.ParamNameBox.Text &&
                !string.IsNullOrWhiteSpace(UserLoignParam.ParamNameBox.Text) &&
                !DBQueries.IsUserExistByLogin(UserLoignParam.ParamNameBox.Text))
                {
                    UserLoignParam.SaveBox.Visibility = Visibility.Visible;
                }
                else UserLoignParam.SaveBox.Visibility = Visibility.Hidden;
            };

            UserLoignParam.SaveBox.PreviewMouseDown += (sender, e) =>
            {
                DBQueries.UpdateUserLogin(_system.LoggedUser.Login, UserLoignParam.ParamNameBox.Text);
                _system.LoggedUser.Login = UserLoignParam.ParamNameBox.Text;
                UserLoignParam.SaveBox.Visibility = Visibility.Hidden;
            };
        }    

        public void SetUserImage()
        {
            const int imageParam = 100;

            string imageName = DBQueries.GetPictureNameById(_system.LoggedUser.GetPictureId());

            UserImage.Source = MainWindowHelper.GetCircleImage(imageParam, imageParam, imageName).Source;
        }

        private void ChangeImageBut_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*",
                Title = "Choose file"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string fullImgPath = openFileDialog.FileName;
                UserImage.Source = new BitmapImage(new Uri(fullImgPath));

                string newImagePath = System.IO.Path.Combine(MainWindowHelper.GetUserImagePath(), openFileDialog.SafeFileName);
                DBQueries.AddPicture(openFileDialog.SafeFileName);
                _system.LoggedUser.SetPictureId(DBQueries.GetLastPicId());

                System.IO.File.Copy(fullImgPath, newImagePath, true);
                UserImage.Source = new BitmapImage(new Uri(newImagePath));

                DBQueries.SetToPlayerLastPicId(_system.LoggedUser.Login);
            }
        }
    }
}
