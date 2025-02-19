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
using MonopolyDLL;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;
using Item = MonopolyDLL.Monopoly.InventoryObjs.Item;
using MonopolyDLL.DBService;

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

            SetUserMenu();
            Height = 10000;
        }
        public void SetUserMenu()
        {
            MainWindowHelper.SetUpperMenuParams(UpperMenuu, _system.LoggedUser);
            UpperMenuu.UserAnim.ExitBut.Click += (sender, e) =>
            {
                _frame.Content = null;
                ((MainWindow)((Grid)_frame.Parent).Parent).Close();
            };
        }

        public void ResetItemsAndUserInventory()
        {
            _system.SetUserInventory();
            SetInventoryItems();
        }

        public void SetUserParamsText()
        {
            UserLogin.Content = _system.LoggedUser.Login;
            AmountOfItems.Text = _system.LoggedUser.Inventory.InventoryItems.Count().ToString();
        }

        private List<CaseCard> _cards = new List<CaseCard>();
        public void SetInventoryItems()
        {
            ItemsPanel.Children.Clear();
            _cards.Clear();

            for (int i = 0; i < _system.LoggedUser.Inventory.InventoryItems.Count; i++)
            {
                if (_system.LoggedUser.Inventory.InventoryItems[i] is CaseBox box)
                {
                    CaseCard card = SetLootBoxCard(box, BoardHelper.GetLotBoxImage(box.ImagePath));
                    card.MouseDown += (sender, e) =>
                    {
                        if (_ifJustBlured) { _ifJustBlured = !_ifJustBlured; return; }
                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                        SetAnimationForCaseBox(pagePoint, card.CardImage, box);
                    };
                    _cards.Add(card);
                }
                else if (_system.LoggedUser.Inventory.InventoryItems[i] is BoxItem boxItem)
                {
                    CaseCard card = SetLootBoxCard(boxItem, BoardHelper.GetAddedItemImage(boxItem.ImagePath, boxItem.Type));

                    _cards.Add(card);
                    boxItem.SetCaseCardId(_cards.Count() - 1);


                    card.MouseDown += (sender, e) =>
                    {
                        if (_ifJustBlured) { _ifJustBlured = !_ifJustBlured; return; }
                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                        SetAnimationForInventoryBusiness(pagePoint, card.CardImage, boxItem);
                    };

                    if (IfBoxCardIsTicked(boxItem))
                    {
                        card.IfTicked.Visibility = Visibility.Visible;
                        boxItem.SetTick(true);
                    }
                }
            }
            SetCardsInItemsPanel();
        }

        private bool IfBoxCardIsTicked(BoxItem item)
        {
            return DBQueries.IfInventoryStaffIsEnabled(item.GetInventoryIdInDB());
        }

        private void SetCardsInItemsPanel()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                ItemsPanel.Children.Add(_cards[i]);
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
            res.CardImage.Stretch = Stretch.Uniform;
            res.CardImage.VerticalAlignment = VerticalAlignment.Center;
            res.CardImage.HorizontalAlignment = HorizontalAlignment.Center;

            res.BorderBgColor.Background = BoardHelper.GetRearityColorForCard(item);
            res.CardName.Foreground = Brushes.White;

            res.CardName.Text = $"{item.Name}";
            return res;
        }

        private BussinessDescription _busDesc;
        public void SetAnimationForInventoryBusiness(Point cardLocation, Image busImg, BoxItem bus)
        {
            if (_frame.Opacity == _inActiveOpacity) return;

            MainWindow window = Helper.FindParent<MainWindow>(_frame);
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

        private readonly SolidColorBrush _ususalCellBrush = new SolidColorBrush(Color.FromRgb(167, 175, 187));
        public List<BusDescButton> GetButtonForUsualBus(BoxItem item)
        {
           
            List<BusDescButton> res = new List<BusDescButton>();
            List<ParentBus> busesToGetButsFor = _system.MonGame.GetBusWithGivenBoxItem(item);

            const string changeText = "Change";
            const string getBackText = "Get back";


            for (int i = 0; i < busesToGetButsFor.Count; i++)
            {
                BoxItem usingItem = _system.LoggedUser.GetItemWhichUsesInGameById(busesToGetButsFor[i].GetId());

                BusDescButton but = new BusDescButton();

                if (usingItem is null)
                {
                    but = GetBusForBusinessDescription(busesToGetButsFor[i].Name, _ususalCellBrush, changeText);
                    SetButMouseDownEventChangedWithParentBus(but, busesToGetButsFor[i], item);
                }
                else if (usingItem.GetInventoryIdInDB() != item.GetInventoryIdInDB() && !usingItem.IsTicked() && 
                     usingItem.Name == item.Name)
                {
                    TextBlock block = new TextBlock()
                    {
                        Text = "Already chosen",
                        FontSize = 16
                    };
                    _busDesc.ButtonsPanel.Children.Add(block);
                    res.Clear();
                    return res;
                }
                else if (usingItem.GetInventoryIdInDB() == item.GetInventoryIdInDB())
                {
                    but = GetBusForBusinessDescription(busesToGetButsFor[i].Name, _ususalCellBrush, getBackText);

                    GetUsualBuyBack(but, item);

                    res.Clear();
                    res.Add(but);
                    return res;
                }
                else
                {              
                    but = GetBusForBusinessDescription(usingItem.Name, GetBrushByBoxItem(usingItem), getBackText);
                    SetButMouseDownEventChangeWithBoxItem(but, usingItem, item);

                    //GetUsualBuyBack(but, item);
                }
                res.Add(but);
            }
            return res;
        }

        public SolidColorBrush GetBrushByBoxItem(BoxItem item)
        {
            return new SolidColorBrush(Color.FromRgb(item.GetRParam(), item.GetGParam(), item.GetBParam()));
        }

        public void GetUsualBuyBack(BusDescButton but, BoxItem item)
        {
            but.MouseDown += (sender, e) =>
            {
                BoxItem toRemove = _system.GetUserInventoryItemByName(item.Name);
                _system.RemoveBoxItemFromUsingList(toRemove);

                DBQueries.ClearInventoryItemById(toRemove.GetInventoryIdInDB());

                _busDesc.ButtonsPanel.Children.Clear();
                SetButtonsInBusDescription(item);

                RemoveTickForCaseCard(item);
            };
        }

        private void SetButMouseDownEventChangedWithParentBus(BusDescButton but, ParentBus oldItem, BoxItem newItem)
        {
            but.MouseDown += (sender, e) =>
            {
                // if (_system.IfBussWithSuchNameIsUsing(newItem.Name)) return;

                _system.RemoveBoxItemByStationId(oldItem.GetId());


                newItem.StationId = oldItem.GetId();
                _system.AddUsingBusInList(newItem);
                _busDesc.ButtonsPanel.Children.Clear();
                SetButtonsInBusDescription(newItem);

                AddTickForCaseCard(newItem);

                DBQueries.SetBoxItemWhichUserStartsToUse(newItem);
            };
        }

        private void SetButMouseDownEventChangeWithBoxItem(BusDescButton but, BoxItem oldItem, BoxItem newItem)
        {
            but.MouseDown += (sender, e) =>
            {
                if (_system.IfBussWithSuchNameIsUsing(newItem.Name)) return;

                newItem.StationId = oldItem.StationId;
                _system.RemoveBoxItemFromUsingList(oldItem);
                _system.AddUsingBusInList(newItem);
                _busDesc.ButtonsPanel.Children.Clear();
                SetButtonsInBusDescription(newItem);

                RemoveTickForCaseCard(oldItem);
                AddTickForCaseCard(newItem);

                DBQueries.SetBoxItemWhichUserNotUse(oldItem);
                DBQueries.SetBoxItemWhichUserStartsToUse(newItem);
            };
        }

        private void AddTickForCaseCard(BoxItem toAdd)
        {
            CaseCard card = _cards[toAdd.GetCaseCardId()];
            card.IfTicked.Visibility = Visibility.Visible;
        }

        private void RemoveTickForCaseCard(BoxItem toHide)
        {
            CaseCard card = _cards[toHide.GetCaseCardId()];
            card.IfTicked.Visibility = Visibility.Hidden;
        }

        public BusDescButton GetBusForBusinessDescription(string busName, SolidColorBrush color, string actionText)
        {
            BusDescButton res = new BusDescButton();

            res.ActionNameText.Text = actionText;
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

            MainWindow window = Helper.FindParent<MainWindow>(_frame);
            Canvas items = window.VisiableItems;

            Console.WriteLine(window.CaseFrame.Visibility);
            Console.WriteLine(items.Visibility);

            _boxDescript = new BoxDescription(_frame, box, _system.LoggedUser.Login);
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
            return;
            UpperMenuu.UserAnim.UpperRowUserName.MouseDown += (sender, e) =>
            {
                _frame.Content = new UserPage(_frame, _system);
            };
        }

        public void SetUserImage()
        {
            Image img = ThingForTest.GetCalivanBigCircleImage(65, 65);

            Image userImg = MainWindowHelper.GetUserImage(DBQueries.GetPictureNameById(_system.LoggedUser.GetPictureId()));
            if (!(userImg is null)) img.Source = userImg.Source;

            UserImage.Children.Add(img);

            Canvas.SetLeft(img, 10);
            Canvas.SetTop(img, 5);
        }

        public void OpenGameField()
        {
            return;
/*            _system.MonGame = new MonopolyDLL.Monopoly.Game(_system.LoggedUser);
            SetPlayersForGame setPlayers = new SetPlayersForGame(_system, _frame);
            setPlayers.ShowDialog();*/
        }

        public void OpenMainPage()
        {
            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;
        }

        public void SetUpperLineSettings()
        {
            //Set bg
            UpperMenuu.CanvasBg.Background = new SolidColorBrush(Colors.White);
            UpperMenuu.Background = new SolidColorBrush(Colors.White);

            //Set buttons colors
            UpperMenuu.MainLogoBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.StartGameBut.Foreground = new SolidColorBrush(Colors.White);
            UpperMenuu.StartGameBut.Background = (Brush)Application.Current.Resources["MainGlobalColor"];

            UpperMenuu.InventoryBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.UserAnim.UserIcon.Foreground = new SolidColorBrush(Colors.Gray);
            UpperMenuu.AllPanelGrid.Width = CenterColDef.Width.Value;
        }

        private bool _ifJustBlured = false;
        private void Page_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(_boxDescript is null) || !(_busDesc is null))
            {
                _frame.Effect = null;
                ClearDescription();
                this.IsEnabled = true;
                _boxDescript = null;
                _busDesc = null;
                _ifJustBlured = true;


                //Update item is box is opened
                ResetItemsAndUserInventory();
                return;
            }
            _ifJustBlured = false;
        }

        private void ClearDescription()
        {
            MainWindow window = Helper.FindParent<MainWindow>(_frame);
            window.ClearVisiableItems();
            window.CaseFrame.Visibility = Visibility.Hidden;
        }
    }
}
