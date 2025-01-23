using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MonopolyEntity.Windows.UserControls.InventoryControls;
using System.Net;
using System.Xml.Linq;
using System.Windows.Media.Effects;

using MonopolyDLL.Monopoly;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page
    {
        private Frame _frame;
        private MonopolySystem _system;
        public InventoryPage(Frame workFrame, MonopolySystem system)
        {
            _frame = workFrame;
            _system = system;
            InitializeComponent();

            SetUpperLineSettings();

            SetUserImage();
            SetUserImageEvent();

            SetInventoryItems();
        }

        public void SetInventoryItems()
        {
            SetTestInventoryItems();
        }

        public void SetTestInventoryItems()
        {
            CaseCard testCard = ThingForTest.GetDragonBoxCard();

            testCard.PreviewMouseDown += InventoryItem_MouseDown;

            ItemsPanel.Children.Add(testCard);
        }

        private void InventoryItem_MouseDown(object sender, EventArgs e)
        {
            //Make Description animation here
             
            if(sender is CaseCard card)
            {
                //Point wrapLoc = Helper.GetElementLocation(card, ItemsPanel);

                Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);

                SetAnimationforCaseBox(pagePoint, card.CardImage);

                //MakeImageDescriptionAnimation(card.CardImage);
            }            
        }

        private BoxDescription _boxDescript = null;
        private BussinessDescription _busDesc = null;

        private const double _inActiveOpacity = 0.1;

        public void SetAnimationforCaseBox(Point cardLocation, Image caseImg)
        {
            if (_frame.Opacity == _inActiveOpacity) return;

            WorkWindow window = Helper.FindParent<WorkWindow>(_frame);
            Canvas items =  window.VisiableItems;

            _boxDescript = new BoxDescription(_frame);
            _boxDescript.DescImage.Source = caseImg.Source;

            Canvas.SetLeft(_boxDescript, cardLocation.X);
            Canvas.SetTop(_boxDescript, cardLocation.Y) ;

            MakeImageDescriptionAnimation(_boxDescript.DescImage);
            MoveElementLeftAnimation(_boxDescript.DescriptionGrid);

            items.Children.Add(_boxDescript);

            _frame.Effect =  new BlurEffect
            {
                Radius = 100
            }; 

            //this.IsEnabled = false;
        }
        
        private void MoveElementLeftAnimation(UIElement element)
        {
            const int movePoint = -50;
            var transform = new TranslateTransform();
            element.RenderTransform = transform;

            var animation = new DoubleAnimation
            {
                From = 0,
                To = movePoint,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            transform.BeginAnimation(TranslateTransform.XProperty, animation);
        }

        private void MakeImageDescriptionAnimation(Image img)
        {
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            ScaleTransform ImageScaleTransform = new ScaleTransform();
            img.RenderTransform = ImageScaleTransform;

            var scaleXAnimation = new DoubleAnimation
            {
                From = 1,
                To = 1.5,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            var scaleYAnimation = new DoubleAnimation
            {
                From = 1,
                To = 1.5,
                Duration = TimeSpan.FromSeconds(0.2),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
            ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
        }

        public void SetUserImageEvent()
        {
            UpperMenuu.UserAnim.UpperRowUserName.MouseDown += (sender, e) =>
            {
                _frame.Content = new UserPage(_frame, _system);
            };
        }

        public void SetUserImage()
        {
            Image img = ThingForTest.GetCalivanBigCircleImage(65, 65);
            UserImage.Children.Add(img);

            Canvas.SetLeft(img, 10);
            Canvas.SetTop(img, 5);
        }

        public void OpenGameField()
        {
            GamePage page = new GamePage(_frame, _system);
            _frame.Content = page;
        }

        public void OpenMainPage()
        {
            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;
        }

        public void SetUpperLineSettings()
        {
            //Set bg
            UpperMenuu.CanvasBg.Background = new SolidColorBrush(Colors.Transparent);
            UpperMenuu.Background = new SolidColorBrush(Colors.Transparent);

            //Set buttons colors
            UpperMenuu.MainLogoBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.StartGameBut.Foreground = new SolidColorBrush(Colors.White);
            UpperMenuu.StartGameBut.Background = (Brush)System.Windows.Application.Current.Resources["MainGlobalColor"];

            UpperMenuu.InventoryBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.UserAnim.UserIcon.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void Page_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(!(_boxDescript is null))
            {
                _frame.Effect = null;
                ClearDescription();
                this.IsEnabled = true;
                _boxDescript = null;
            }
        }

        private void ClearDescription()
        {
            WorkWindow window = Helper.FindParent<WorkWindow>(_frame);
            window.ClearVisiableItems();

        }


    }
}
