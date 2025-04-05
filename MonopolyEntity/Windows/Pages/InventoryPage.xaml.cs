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
using MonopolyDLL.Monopoly.Cell.Businesses;
using MonopolyDLL;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;
using Item = MonopolyDLL.Monopoly.InventoryObjs.Item;
using MonopolyDLL.Monopoly.Enums;
using MonopolyEntity.Interfaces;
using MonopolyDLL.DBService;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для InventoryPage.xaml
    /// </summary>
    public partial class InventoryPage : Page/*, IPagesOpener*/
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
            SetInventoryItems();

            SetUserParamsText();

            SetUserMenu();

            UpperMenuu.SetMonSystemAndFrame(_frame, _system);
        }

        public void SetUserMenu()
        {
            MainWindowHelper.SetUpperMenuParams(UpperMenuu, _system.LoggedUser);
            UpperMenuu.UserAnim.ExitBut.Click += (sender, e) =>
            {
                _frame.Content = null;
                ((MainWindow)((Grid)_frame.Parent).Parent).Close();
            };

            UpperMenuu.UserAnim.SettingsBut.Click += (sender, e) =>
            {
                _frame.Content = new ProfileSettings(_system, _frame);
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
                    CaseCard card = SetLootBoxCard(box, Helper.GetLotBoxImage(box.ImagePath));

                    SetMouseDownEvent(card, box);

/*                  card.MouseDown += (sender, e) =>
                    {
                        if (_ifJustBlurred) { _ifJustBlurred = !_ifJustBlurred; return; }
                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                        SetAnimationForCaseBox(pagePoint, card.CardImage, box);
                    };*/
                    _cards.Add(card);
                }
                else if (_system.LoggedUser.Inventory.InventoryItems[i] is BoxItem boxItem)
                {
                    CaseCard card = SetLootBoxCard(boxItem, Helper.GetAddedItemImage(boxItem.ImagePath, boxItem.Type));

                    _cards.Add(card);
                    boxItem.SetCaseCardId(_cards.Count() - 1);

                    SetMouseDownEvent(card, boxItem);

/*                  card.MouseDown += (sender, e) =>
                    {
                        if (_ifJustBlurred) { _ifJustBlurred = !_ifJustBlurred; return; }
                        Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                        SetAnimationForInventoryBusiness(pagePoint, card.CardImage, boxItem);
                    };*/

                    if (IsBoxCardIsTicked(boxItem))
                    {
                        card.IfTicked.Visibility = Visibility.Visible;
                        boxItem.SetTick(true);
                    }
                }
            }
            SetCardsInItemsPanel();
        }

        public void SetMouseDownEvent(CaseCard card, object element)
        {
            card.MouseDown += (sender, e) =>
            {
                if (_ifJustBlurred) { _ifJustBlurred = !_ifJustBlurred; return; }
                Point pagePoint = Helper.GetElementLocationRelativeToPage(card, this);
                
                if(element is BoxItem boxItem) SetAnimationForInventoryBusiness(pagePoint, card.CardImage, boxItem);
                else if(element is CaseBox caseBox) SetAnimationForCaseBox(pagePoint, card.CardImage, caseBox);
            };
        }

        private bool IsBoxCardIsTicked(BoxItem item)
        {
            return DBQueries.IsInventoryStaffIsEnabled(item.GetInventoryIdInDB());
        }

        private void SetCardsInItemsPanel()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                ItemsPanel.Children.Add(_cards[i]);
            }
        }

        private readonly Size _baseCardSize = new Size(150, 175);
        public CaseCard SetLootBoxCard(Item item, Image img)
        {
            const int baseMargin = 10;
            const int baseImageSizeParam = 100;

            CaseCard res = new CaseCard()
            {
                Width = _baseCardSize.Width,
                Height = _baseCardSize.Height,
                Margin = new Thickness(baseMargin)
            };

            res.CardImage.Source = img.Source;
            res.CardImage.Width = baseImageSizeParam;
            res.CardImage.Height = baseImageSizeParam;
            res.CardImage.Stretch = Stretch.Uniform;
            res.CardImage.VerticalAlignment = VerticalAlignment.Center;
            res.CardImage.HorizontalAlignment = HorizontalAlignment.Center;

            res.BorderBgColor.Background = Helper.GetRarityColorForCard(item);
            res.CardName.Foreground = Brushes.White;

            res.CardName.Text = $"{item.Name}";
            return res;
        }

        private BusinessDescription _busDesc;
        public void SetAnimationForInventoryBusiness(Point cardLocation, Image busImg, BoxItem bus)
        {
            if (_frame.Opacity == _inActiveOpacity) return;

            MainWindow window = Helper.FindParent<MainWindow>(_frame);
            Canvas items = window.VisiableItems;

            _busDesc = new BusinessDescription(bus);
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

        private readonly SolidColorBrush _usualCellBrush = new SolidColorBrush(Color.FromRgb(167, 175, 187));
        public List<BusDescButton> GetButtonForUsualBus(BoxItem item)
        {
            List<BusDescButton> res = new List<BusDescButton>();
            List<Business> busesToGetButsFor = _system.MonGame.GetBusWithGivenBoxItem(item);

             string changeText = SystemParamsService.GetStringByName("ChangeItemInventory");
             string getBackText = SystemParamsService.GetStringByName("GetBackItemInventory");
             string sameItemIsChosen = SystemParamsService.GetStringByName("ItemChosenInventory");
             int textBlockFz = 16;

            for (int i = 0; i < busesToGetButsFor.Count; i++)
            {
                BoxItem usingItem = _system.LoggedUser.GetItemWhichUsesInGameById(busesToGetButsFor[i].GetId());

                BusDescButton but;

                if (usingItem is null)
                {
                    but = GetBusForBusinessDescription(busesToGetButsFor[i].Name, _usualCellBrush, changeText);
                    SetButMouseDownEventChangedWithParentBus(but, busesToGetButsFor[i], item);
                }
                else if (usingItem.GetInventoryIdInDB() != item.GetInventoryIdInDB() && !usingItem.IsTicked() &&
                     usingItem.Name == item.Name)
                {
                    TextBlock block = new TextBlock()
                    {
                        Text = sameItemIsChosen,
                        FontSize = textBlockFz
                    };
                    _busDesc.ButtonsPanel.Children.Add(block);
                    res.Clear();
                    return res;
                }
                else if (usingItem.GetInventoryIdInDB() == item.GetInventoryIdInDB())
                {
                    but = GetBusForBusinessDescription(busesToGetButsFor[i].Name, _usualCellBrush, getBackText);

                    GetUsualBuyBack(but, item);

                    res.Clear();
                    res.Add(but);
                    return res;
                }
                else
                {
                    but = GetBusForBusinessDescription(usingItem.Name, GetBrushByBoxItem(usingItem), changeText);
                    SetButMouseDownEventChangeWithBoxItem(but, usingItem, item);
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

                SetVisibilityForCaseCard(item, Visibility.Hidden);

                //RemoveTickForCaseCard(item);
            };
        }

        private void SetButMouseDownEventChangedWithParentBus(BusDescButton but, Business oldItem, BoxItem newItem)
        {
            but.MouseDown += (sender, e) =>
            {
                _system.RemoveBoxItemByStationId(oldItem.GetId());

                newItem.StationId = oldItem.GetId();
                _system.AddUsingBusInList(newItem);
                _busDesc.ButtonsPanel.Children.Clear();
                SetButtonsInBusDescription(newItem);

                //AddTickForCaseCard(newItem);
                SetVisibilityForCaseCard(newItem, Visibility.Visible);

                DBQueries.SetBoxItemWhichUserStartsToUse(newItem);
            };
        }

        private void SetButMouseDownEventChangeWithBoxItem(BusDescButton but, BoxItem oldItem, BoxItem newItem)
        {
            but.MouseDown += (sender, e) =>
            {
                if (_system.IsBussWithSuchNameIsUsing(newItem.Name)) return;

                newItem.StationId = oldItem.StationId;
                _system.RemoveBoxItemFromUsingList(oldItem);
                _system.AddUsingBusInList(newItem);
                _busDesc.ButtonsPanel.Children.Clear();
                SetButtonsInBusDescription(newItem);


                SetVisibilityForCaseCard(oldItem, Visibility.Hidden);

                //RemoveTickForCaseCard(oldItem);

                SetVisibilityForCaseCard(newItem, Visibility.Visible);

                //AddTickForCaseCard(newItem);

                DBQueries.SetBoxItemWhichUserNotUse(oldItem);
                DBQueries.SetBoxItemWhichUserStartsToUse(newItem);
            };
        }

        public void SetVisibilityForCaseCard(BoxItem item, Visibility visibility)
        {
            CaseCard card = _cards[item.GetCaseCardId()];
            card.IfTicked.Visibility = visibility;
        }

/*        private void AddTickForCaseCard(BoxItem toAdd, Visibility visible)
        {
            CaseCard card = _cards[toAdd.GetCaseCardId()];
            card.IfTicked.Visibility = Visibility.Visible;
        }

        private void RemoveTickForCaseCard(BoxItem toHide)
        {
            CaseCard card = _cards[toHide.GetCaseCardId()];
            card.IfTicked.Visibility = Visibility.Hidden;
        }*/

        public BusDescButton GetBusForBusinessDescription(string busName, SolidColorBrush color, string actionText)
        {
            BusDescButton res = new BusDescButton();

            res.ActionNameText.Text = actionText;
            res.BusName.Text = busName;
            res.RearityColor.Background = color;

            res.PreviewMouseDown += (sender, e) => { };
            return res;
        }

        private BoxDescription _boxDescript = null;
        private const double _inActiveOpacity = 0.1;

        public void SetAnimationForCaseBox(Point cardLocation, Image caseImg, CaseBox box)
        {
            if (_frame.Opacity == _inActiveOpacity) return;

            MainWindow window = Helper.FindParent<MainWindow>(_frame);
            Canvas items = window.VisiableItems;

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
            const double _centerOrigin = 0.5;
            const double _movementSpeed = 0.2;
            const double scaleFrom = 1;
            const double scaleTo = 1.5;

            img.RenderTransformOrigin = new Point(_centerOrigin, _centerOrigin);

            ScaleTransform ImageScaleTransform = new ScaleTransform();
            img.RenderTransform = ImageScaleTransform;

            var scaleXAnimation = GetAnimation(scaleFrom, scaleTo, _movementSpeed); /*new DoubleAnimation
            {
                From = scaleFrom,
                To =scaleTo,
                Duration = TimeSpan.FromSeconds(_movementSpeed),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };*/

            var scaleYAnimation = GetAnimation(scaleFrom, scaleTo, _movementSpeed);/*new DoubleAnimation
            {
                From = scaleFrom,
                To = scaleTo,
                Duration = TimeSpan.FromSeconds(_movementSpeed),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };*/
            ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, scaleXAnimation);
            ImageScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, scaleYAnimation);
        }

        public DoubleAnimation GetAnimation(double scaleFrom, double scaleTo, double speed)
        {
            return new DoubleAnimation
            {
                From = scaleFrom,
                To = scaleTo,
                Duration = TimeSpan.FromSeconds(speed),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        public void SetUserImage()
        {
            const int xLoc = 10;
            const int yLoc = 5;

            Image img = ThingForTest.GetCalivanBigCircleImage(65, 65);

            Image userImg = MainWindowHelper.GetUserImage(DBQueries.GetPictureNameById(_system.LoggedUser.GetPictureId()));
            if (!(userImg is null)) img.Source = userImg.Source;

            UserImage.Children.Add(img);

            Canvas.SetLeft(img, xLoc);
            Canvas.SetTop(img, yLoc);
        }

        public void OpenPage(bool ifMainPage)
        {
            Page page = ifMainPage ? new MainPage(_frame, _system) : (Page)new SetPlayersInGame(_system, _frame);
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(page);
        }

/*        public void OpenGameField()
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new SetPlayersInGame(_system, _frame));
*//*
            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
            _frame.Content = page;*//*
        }

        public void OpenMainPage()
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new MainPage(_frame, _system));

*//*            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;*//*
        }*/

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

            UpperMenuu.AllPanelGrid.Width = CenterColDef.Width.Value;
        }

        private bool _ifJustBlurred = false;
        private void Page_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!(_boxDescript is null) || !(_busDesc is null))
            {
                _frame.Effect = null;
                ClearDescription();
                this.IsEnabled = true;
                _boxDescript = null;
                _busDesc = null;
                _ifJustBlurred = true;


                //Update item is box is opened
                ResetItemsAndUserInventory();

                FilterByBameBox.Text = string.Empty;
                TypeChooseBox.SelectedItem = BaseChooseType;
                RareFilter.SelectedItem = BaseChooseRare;
                return;
            }
            _ifJustBlurred = false;
        }

        private void ClearDescription()
        {
            MainWindow window = Helper.FindParent<MainWindow>(_frame);
            window.ClearVisibleItems();
            window.CaseFrame.Visibility = Visibility.Hidden;
        }

        private string _textFilter = string.Empty;
        private void FilterByBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ItemsPanel.Children.Clear();
            _textFilter = ((TextBox)sender).Text.ToLower();
            SetCardsByName();
        }

        private void SetCardsByName()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i].CardName.Text.ToLower().Contains(_textFilter) &&
                    ((!(_rarity is null) && _system.LoggedUser.Inventory.InventoryItems[i] is BoxItem boxItem && 
                    boxItem.Rarity == _rarity) || _rarity is null) &&
                    ((_itemType is BoxItem && _system.LoggedUser.Inventory.InventoryItems[i] is BoxItem) ||
                    (_itemType is CaseBox && _system.LoggedUser.Inventory.InventoryItems[i] is CaseBox) || _itemType is null))
                {
                    ItemsPanel.Children.Add(_cards[i]);
                }
            }
        }

        private BusinessRarity? _rarity;
        private void RareFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
             string clearFilterStr = SystemParamsService.GetStringByName("SetAllInFilterInventory");
            ItemsPanel.Children.Clear();
            string res = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();

            if (res == clearFilterStr) _rarity = null;
            else SetRarity(res);

            SetCardsByName();
        }

        private void SetRarity(string rarity)
        {
            for (int i = (int)BusinessRarity.Usual; i <= (int)BusinessRarity.Legend; i++)
            {
                if (rarity == ((BusinessRarity)i).ToString())
                {
                    _rarity = (BusinessRarity)i;
                    return;
                }
            }
        }

        private Item _itemType = null;
        private const string _baseItemsFilterName = "Items";
        private const string _baseBoxFilterName = "Box";

        private void TypeChooseBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemsPanel.Children.Clear();
            string res = ((ComboBoxItem)((ComboBox)sender).SelectedItem).Content.ToString();

            if (res == _baseItemsFilterName) _itemType = new BoxItem();
            else if (res == _baseBoxFilterName) _itemType = new CaseBox();
            else _itemType = null;

            //_itemType = res == "Items" ? new BoxItem() : res == "Box" ? new CaseBox() : null;


            SetCardsByName();
        }
    }
}
