using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using MonopolyEntity.Windows.UserControls.InventoryControls;
using System.Windows.Media.Effects;

using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.InventoryObjs;
using System.Collections.Generic;
using MonopolyDLL.Monopoly.Cell.Bus;
using System.Data.Entity.Infrastructure;
using MonopolyDLL;

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

            SetUserParamsText();
        }

        public void SetUserParamsText()
        {
            UserLogin.Content = _system.LoggedUser.Login;
            AmountOfItems.Text = _system.LoggedUser.Inventory.InventoryItems.Count().ToString();
        }

        public void SetInventoryItems()
        {
            for (int i = 0; i < _system.LoggedUser.Inventory.InventoryItems.Count; i++)
            {
                if (_system.LoggedUser.Inventory.InventoryItems[i] is CaseBox box)
                {
                    CaseCard card = SetLootBoxCard(box, BoardHelper.GetLotBoxImage(box.ImagePath));
                    card.PreviewMouseDown += (sender, e) =>
                    {
                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                        SetAnimationForCaseBox(pagePoint, card.CardImage, box);
                    };
                    ItemsPanel.Children.Add(card);
                }
                else if (_system.LoggedUser.Inventory.InventoryItems[i] is BoxItem boxItem)
                {
                    CaseCard card = SetLootBoxCard(boxItem, BoardHelper.GetAddedItemImage(boxItem.ImagePath, boxItem.Type));
                    card.PreviewMouseDown += (sender, e) =>
                    {
                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                        SetAnimationForInventoryBusiness(pagePoint, card.CardImage, boxItem);
                    };
                    ItemsPanel.Children.Add(card);
                }
            }
        }

        public CaseCard SetLootBoxCard(Item item, Image img)
        {
            string name = item.Name;

            CaseCard res = new CaseCard()
            {
                Width = 150,
                Height = 175,
                Margin = new Thickness(10)
            };

            res.CardImage.Source = img.Source;
            res.CardImage.Width = 100;
            res.CardImage.Height = 100;
            res.CardImage.Stretch = Stretch.Fill;
            res.CardImage.VerticalAlignment = VerticalAlignment.Center;
            res.CardImage.HorizontalAlignment = HorizontalAlignment.Center;

            res.BorderBgColor.Background = BoardHelper.GetRearityColorForCard(item);
            res.CardName.Foreground = Brushes.White;

            res.CardName.Text = $"{item.Name}";
            return res;
        }



        /*        public void SetTestInventoryItems()
                {
                    CaseCard testCard = ThingForTest.GetDragonBoxCard();
                    testCard.PreviewMouseDown += InventoryItem_MouseDown;
                    ItemsPanel.Children.Add(testCard);
                }

                private void InventoryItem_MouseDown(object sender, EventArgs e)
                {
                    //Make Description animation here

                    if (sender is CaseCard card)
                    {
                        //Point wrapLoc = Helper.GetElementLocation(card, ItemsPanel);

                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);

                        SetAnimationForCaseBox(pagePoint, card.CardImage);

                        //MakeImageDescriptionAnimation(card.CardImage);
                    }
                }*/

        private BussinessDescription _busDesc;
        public void SetAnimationForInventoryBusiness(Point cardLocation, Image busImg, BoxItem bus)
        {
            if (_frame.Opacity == _inActiveOpacity) return;

            WorkWindow window = Helper.FindParent<WorkWindow>(_frame);
            Canvas items = window.VisiableItems;

            _busDesc = new BussinessDescription(bus);
            _busDesc.DescImage.Source = busImg.Source;

            SetButtonsInBusDescription(bus);

            Canvas.SetLeft(_busDesc, cardLocation.X);
            Canvas.SetTop(_busDesc, cardLocation.Y);

            MakeImageDescriptionAnimation(_busDesc.DescImage);
            MoveElementLeftAnimation(_busDesc.DescriptionGrid);

            items.Children.Add(_busDesc);

            SetBlurEffect();
        }

        private void SetButtonsInBusDescription(BoxItem item)
        {
            List<BusDescButton> buts = GetButtonForUsualBus(item);

            for (int i = 0; i < buts.Count; i++)
            {
                _busDesc.ButtonsPanel.Children.Add(buts[i]);
            }
        }


        /// <summary>
        /// Set buttons for bus description
        /// to change busses in game
        /// </summary>
        public List<BusDescButton> GetButtonForUsualBus(BoxItem item)
        {
            List<BusDescButton> res = new List<BusDescButton>();
            List<ParentBus> busesToGetButsFor = _system.MonGame.GetBusWithGivenBoxItem(item);

            for (int i = 0; i < busesToGetButsFor.Count; i++)
            {
                BoxItem usingItem = _system.LoggedUser.GetItemWhichUsesInGameById(busesToGetButsFor[i].GetId());

                if (usingItem is null)
                {
                    res.Add(GetBusForBusinessDescription(
                        busesToGetButsFor[i].Name, new SolidColorBrush(Color.FromRgb(76, 180, 219))));
                }
                else
                {
                    res.Add(GetBusForBusinessDescription(usingItem.Name,
                        BoardHelper.GetColorFromSystemColorName(usingItem.Rearity.ToString())));
                }
            }
            return res;
        }

        /// <summary>
        /// Check for not using same cards several times
        /// </summary>
        /// <param name="item"></param>
        private void IfSuchItemIsAlreadyUsingInGame(BoxItem item)
        {   //If user is using boxItem with such name
            if (_system.LoggedUser.IfBusWithSuchNameIsUSedInGame(item.Name))
            {

            }
        }


        public BusDescButton GetBusForBusinessDescription(string busName, SolidColorBrush color)
        {
            BusDescButton res = new BusDescButton();

            res.ActionNameText.Text = "Change";
            res.BusName.Text = busName;
            res.RearityColor.Background = color;

            res.PreviewMouseDown += (sender, e) =>
            {

            };
            return res;
        }

        private BoxDescription _boxDescript = null;
        private const double _inActiveOpacity = 0.1;

        public void SetAnimationForCaseBox(Point cardLocation, Image caseImg, CaseBox box)
        {
            if (_frame.Opacity == _inActiveOpacity) return;

            WorkWindow window = Helper.FindParent<WorkWindow>(_frame);
            Canvas items = window.VisiableItems;

            _boxDescript = new BoxDescription(_frame, box);
            SetBoxDescriptionParams(cardLocation, caseImg);

            MakeImageDescriptionAnimation(_boxDescript.DescImage);
            MoveElementLeftAnimation(_boxDescript.DescriptionGrid);

            items.Children.Add(_boxDescript);

            SetBlurEffect();
        }

        public void SetBoxDescriptionParams(Point cardLocation, Image caseImg)
        {
            _boxDescript.DescImage.Source = caseImg.Source;

            Canvas.SetLeft(_boxDescript, cardLocation.X);
            Canvas.SetTop(_boxDescript, cardLocation.Y);
        }

        private void SetBlurEffect()
        {
            const int fullBlur = 100;
            _frame.Effect = new BlurEffect
            {
                Radius = fullBlur
            };
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
            if (!(_boxDescript is null) || !(_busDesc is null))
            {
                _frame.Effect = null;
                ClearDescription();
                this.IsEnabled = true;
                _boxDescript = null;
                _busDesc = null;
            }
        }

        private void ClearDescription()
        {
            WorkWindow window = Helper.FindParent<WorkWindow>(_frame);
            window.ClearVisiableItems();
        }
    }
}
