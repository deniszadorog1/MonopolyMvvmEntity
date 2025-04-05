using MonopolyEntity.VisualHelper;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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

        private DoubleAnimation _animation;
        private void UserIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            const double animationDuration = 0.2;
            _animation = new DoubleAnimation
            {
                To = 0,
                Duration = TimeSpan.FromSeconds(animationDuration)
            };

            _animation.Completed += Animation_Completed;
            AnimImage.BeginAnimation(Canvas.LeftProperty, _animation);
        }

        private void Animation_Completed(object sender, EventArgs e)
        {
            const int zIndex = 150;
            UserMenu.Visibility = Visibility.Visible;
            ElemBorder.Background = new SolidColorBrush(Colors.White);
            Canvas.SetZIndex(AnimImage, zIndex);
        }


        private  SolidColorBrush _mouseEnterColor = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Button)
            {
                SetColorsForButton((Button)sender, new SolidColorBrush(Colors.White), _mouseEnterColor);
                /*but.Foreground = new SolidColorBrush(Colors.White);
                but.Background = _mouseEnterColor; // new SolidColorBrush((Color)ColorConverter.ConvertFromString(_mouseEnterColor));*/
            }
        }

        //private bool _check = false;
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            if (/*_check ||*/ sender is Button)
            {
                SetColorsForButton((Button)sender, new SolidColorBrush(Colors.Gray), new SolidColorBrush(Colors.White));
/*                ((Button)sender).Foreground = new SolidColorBrush(Colors.Gray);
                ((Button)sender).Background = new SolidColorBrush(Colors.White);*/
            }
        }

        public void SetColorsForButton(Button but, SolidColorBrush foregroundColor, SolidColorBrush backgroundColor)
        {
            but.Foreground = foregroundColor;
            but.Background = backgroundColor;
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
