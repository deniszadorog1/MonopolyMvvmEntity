using MonopolyEntity.VisualHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.UserControls.GameControls
{
    /// <summary>
    /// Логика взаимодействия для UserCard.xaml
    /// </summary>
    public partial class UserCard : UserControl
    {
        public SolidColorBrush _usualBrush = (SolidColorBrush)Application.Current.Resources["BaseUserCardBackground"];
        public UserCard()
        {
            InitializeComponent();

            SetTestCardImage();

            //SetAnimation(new SolidColorBrush(Colors.Green), true);
        }

        public void SetTestCardImage()
        {
            Image img = ThingForTest.GetCalivanBigCircleImage(70, 70);

            img.VerticalAlignment = VerticalAlignment.Center;
            img.HorizontalAlignment = HorizontalAlignment.Center;

            UserImageGrid.Children.Add(img);
        }

        private int _toMakeBigger = 20;
        private int _toMakeThinner = -20;

        const int translateMove = 10;

        public DoubleAnimation _horizAnim;
        public void SetAnimation(SolidColorBrush brush, bool ifStepper)
        {
            var transform = new TranslateTransform();
            UserCardGrid.RenderTransform = transform;

            int value = ifStepper ? _toMakeBigger : _toMakeThinner;
            int horValue = ifStepper ? translateMove : 0;
            UserCardGrid.Background = brush is null ? _usualBrush : brush;

            var widthAnimation = new DoubleAnimation
            {
                From = UserCardGrid.Width,
                To = UserCardGrid.Width + value,
                Duration = TimeSpan.FromSeconds(0.5),
                // AutoReverse = true 
            };

            var heightAnimation = new DoubleAnimation
            {
                From = UserCardGrid.Height,
                To = UserCardGrid.Height + value,
                Duration = TimeSpan.FromSeconds(0.5),
                //AutoReverse = true 
            };

            _horizAnim = new DoubleAnimation
            {
                From = 0,
                To = horValue,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
            _horizAnim.Completed += HorizAnim_Complited;

            transform.BeginAnimation(TranslateTransform.XProperty, _horizAnim);

            UserCardGrid.BeginAnimation(WidthProperty, widthAnimation);
            UserCardGrid.BeginAnimation(HeightProperty, heightAnimation);
        }        

        public void HorizAnim_Complited(object sender, EventArgs e)
        {
            
        }

        public void MakeCardUsual()
        {
            return;
            var transform = new TranslateTransform();
            UserCardGrid.RenderTransform = transform;

            if (UserCardGrid.Background == _usualBrush) return;
            UserCardGrid.Background = _usualBrush;

            var widthAnimation = new DoubleAnimation
            {
                From = UserCardGrid.Width,
                To = UserCardGrid.Width + _toMakeThinner,
                Duration = TimeSpan.FromSeconds(0.5),
                // AutoReverse = true 
            };

            var heightAnimation = new DoubleAnimation
            {
                From = UserCardGrid.Height,
                To = UserCardGrid.Height + _toMakeThinner,
                Duration = TimeSpan.FromSeconds(0.5),
                //AutoReverse = true 
            };

            var translateAnim = new DoubleAnimation
            {
                From = 0,
                To = 600,
                Duration = TimeSpan.FromSeconds(0.5),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            transform.BeginAnimation(TranslateTransform.XProperty, translateAnim);

            UserCardGrid.BeginAnimation(WidthProperty, widthAnimation);
            UserCardGrid.BeginAnimation(HeightProperty, heightAnimation);
        }

    }
}
