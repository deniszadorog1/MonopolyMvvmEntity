using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MonopolyEntity.VisualHelper;

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
            AnimImage.Source =
                MainWindowHelper.GetCircleImageFace(
                    new Size(AnimImage.Width, AnimImage.Height)).Source;
        }

        private DoubleAnimation _anim;
        private void UserIcon_MouseEnter(object sender, MouseEventArgs e)
        {

            _anim = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(0.2)
            };

            _anim.Completed += Animation_Complited;
            AnimImage.BeginAnimation(Canvas.LeftProperty, _anim);
        }

        private void Animation_Complited(object sender, EventArgs e)
        {
            UserMenu.Visibility = Visibility.Visible;
            ElemBorder.Background = new SolidColorBrush(Colors.White);
            Canvas.SetZIndex(AnimImage, 150);
        }

        private void UserMenu_MouseLeave(object sender, MouseEventArgs e)
        {
            return;
            UserMenu.Visibility = Visibility.Hidden;

        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button but)
            {
                but.Foreground = new SolidColorBrush(Colors.White);
                but.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37BC9D"));
            }
        }

        private bool _check = false;
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (_check || sender is Button)
            {
                ((Button)sender).Foreground = new SolidColorBrush(Colors.Gray);
                ((Button)sender).Background = new SolidColorBrush(Colors.White);
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = null;
        }
    }
}
