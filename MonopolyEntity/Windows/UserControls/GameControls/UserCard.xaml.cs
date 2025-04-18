﻿using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls.GameControls.Other;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
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
        }

        public int _imgSize = 70;
        private const int _zIndex = 10;
        public void SetTestCardImage()
        {
            Image img = MainWindowHelper.GetCircleImage(_imgSize, _imgSize, null);

            //img.VerticalAlignment = VerticalAlignment.Center;
            //img.HorizontalAlignment = HorizontalAlignment.Center;

            Canvas.SetZIndex(img, _zIndex);
            UserImageGrid.Children.Add(img);
        }

        public void SetNewCardImage(Image img)
        {
            if (img is null) return;
            Image tempImg = UserImageGrid.Children.OfType<Image>().FirstOrDefault();
            if (tempImg is null) return;
            tempImg.Source = img.Source;
        }

        private SolidColorBrush _color;
        public void SetCircleColors(SolidColorBrush color)
        {
            _color = color;
            AddColorCircle();
            AddEmptyColorCircle();
        }

        public void AddColorCircle()
        {
            const int circleSize = 77;
            Ellipse colorCircle = new Ellipse()
            {
                Width = circleSize,
                Height = circleSize,
                Fill = _color,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            UserImageGrid.Children.Add(colorCircle);
        }

        public void AddEmptyColorCircle()
        {
            const int zIndex = 9;
            const int circleSize = 73;
            Ellipse colorCircle = new Ellipse()
            {
                Width = circleSize,
                Height = circleSize,
                Fill = _usualBrush,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            Canvas.SetZIndex(colorCircle, zIndex);

            UserImageGrid.Children.Add(colorCircle);
        }

        public int _toMakeBigger = 20;
        public int _toMakeThinner = -20;

        public DoubleAnimation _horizontalAnimation;
        private const double _animationDuration = 0.2;
        public void SetAnimation(SolidColorBrush brush, bool isStepper)
        {
            var transform = new TranslateTransform();
            UserCardGrid.RenderTransform = transform;

            int value = isStepper ? _toMakeBigger : _toMakeThinner;
            int horValue = /*ifStepper ? translateMove :*/ 0;
            PaintBg(brush);

            SetTimer(brush);

            var widthAnimation = GetAnimation(UserCardGrid.Width, UserCardGrid.Width + value, _animationDuration); /*new DoubleAnimation
            {
                From = UserCardGrid.Width,
                To = UserCardGrid.Width + value,
                Duration = TimeSpan.FromSeconds(_animationDuration),
                // AutoReverse = true 
            };*/

            var heightAnimation = GetAnimation(UserCardGrid.Height, UserCardGrid.Height + value, _animationDuration);  /*new DoubleAnimation
            {
                From = UserCardGrid.Height,
                To = UserCardGrid.Height + value,
                Duration = TimeSpan.FromSeconds(_animationDuration),
                //AutoReverse = true 
            };*/

            _horizontalAnimation = GetAnimation(0, horValue, _animationDuration); /*new DoubleAnimation
            {
                From = 0,
                To = horValue,
                Duration = TimeSpan.FromSeconds(_animationDuration),
                // EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };*/
            _horizontalAnimation.Completed += (sender, e) => { };

            transform.BeginAnimation(TranslateTransform.XProperty, _horizontalAnimation);

            UserCardGrid.BeginAnimation(WidthProperty, widthAnimation);
            UserCardGrid.BeginAnimation(HeightProperty, heightAnimation);
        }

        public DoubleAnimation GetAnimation(double from, double to, double duration)
        {
            return  new DoubleAnimation
            {
                From = from,
                To = to,
                Duration = TimeSpan.FromSeconds(duration),
            };
        }

        public void PaintBg(SolidColorBrush brush)
        {
            UserCardGrid.Background = brush is null ? _usualBrush : brush;
        }

        public bool IsUserCardBgIsUsual()
        {
            return ((SolidColorBrush)UserCardGrid.Background).Color == _usualBrush.Color;
        }

        public void SetTimer(SolidColorBrush brush)
        {
            if (brush is null)
            {
                StopTimer();
                return;
            }
            ShowTimer(brush);
        }

        public void ShowTimer(SolidColorBrush brush)
        {
            UserTimer.Visibility = Visibility.Visible;
            UserTimer.SetTimer();

            SetTimerBgColor(brush);
        }

        public void SetVisibleToTimer()
        {
            UserTimer.Visibility = Visibility.Visible;
            UserTimer.SetTimer();
        }

        public void UpdateTimer()
        {
            if (UserTimer.Visibility == Visibility.Hidden) return;
            UserTimer.UpdateTimeOnTimer();
        }

        private readonly SolidColorBrush _firstUserTimerColor =
            new SolidColorBrush(Color.FromRgb(188, 66, 70));

        private readonly SolidColorBrush _secondUserTimerColor =
            new SolidColorBrush(Color.FromRgb(54, 152, 199));

        private readonly SolidColorBrush _thirdUserTimerColor =
            new SolidColorBrush(Color.FromRgb(111, 168, 73));

        private readonly SolidColorBrush _fourthUserTimerColor =
            new SolidColorBrush(Color.FromRgb(115, 111, 198));

        private readonly SolidColorBrush _fifthUserTimerColor =
            new SolidColorBrush(Color.FromRgb(185, 131, 57));

        public void SetTimerBgColor(SolidColorBrush brush)
        {
            UserTimer.BgBorder.Background =
                brush == (SolidColorBrush)Application.Current.Resources["FirstUserColor"] ? _firstUserTimerColor :
                brush == (SolidColorBrush)Application.Current.Resources["SecondUserColor"] ? _secondUserTimerColor :
                brush == (SolidColorBrush)Application.Current.Resources["ThirdUserColor"] ? _thirdUserTimerColor :
                brush == (SolidColorBrush)Application.Current.Resources["FourthUserColor"] ? _fourthUserTimerColor : _fifthUserTimerColor;
        }

        public void StopTimer()
        {
            UserTimer.Visibility = Visibility.Hidden;
            UserTimer.StopTimer();
        }


        /*public void MakeCardUsual()
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
*/
    }
}
