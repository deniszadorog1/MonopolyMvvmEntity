using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.UserControls.MainPage
{
    /// <summary>
    /// Логика взаимодействия для UserAnimated.xaml
    /// </summary>
    public partial class UserAnimated : UserControl
    {
        public UserAnimated()
        {
            InitializeComponent();
        }

        private void UserIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            var animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            animation.Completed -= Animation_Complited;
            animation.Completed += Animation_Complited;

            UserIcon.BeginAnimation(Canvas.LeftProperty, animation);
        }

        private void Animation_Complited(object sender, EventArgs e)
        {
            UserMenu.Visibility = Visibility.Visible;
            ElemBorder.Background = new SolidColorBrush(Colors.White);
        }

        private void UserMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            return;
            if (UserMenu.Visibility == Visibility.Visible)
            {
                UserMenu.Visibility = Visibility.Hidden;
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button but)
            {
                but.Foreground = new SolidColorBrush(Colors.White);
                but.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37BC9D"));
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (sender is Button but)
            {
                but.Foreground = new SolidColorBrush(Colors.Gray);
                but.Background = new SolidColorBrush(Colors.White);
            }
        }

    }
}
