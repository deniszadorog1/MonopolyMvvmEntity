using MahApps.Metro.Controls;
using MonopolyEntity.Windows.UserControls.GameControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MonopolyDLL.Monopoly;
using MonopolyEntity.VisualHelper;
using System.Data.Entity.Infrastructure;
using MonopolyDLL;
using MonopolyDLL.DBService;
using System.Media;
using System.CodeDom;
using System.Data.Entity.ModelConfiguration.Conventions;
using MonopolyDLL.Monopoly.Enums;
using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        private Frame _frame;
        private MonopolySystem _system;
        public GamePage(Frame workFrame, MonopolySystem system)
        {
            _frame = workFrame;
            _system = system;

            InitializeComponent();

            SetPopupsForUserCards();
            SetUserCardsInList();
            
            AddGameField();

            SetStartValuesInUserCards();

            TestSound();
            SetFrameToTimers();
        }

        public void SetFrameToTimers()
        {
            for (int i = 0; i < _userCards.Count; i++)
            {
                _userCards[i].UserTimer.SetFrame(_frame);
            }
        }

        public void TestSound()
        {
            var player = new SoundPlayer();
            player.SoundLocation = MainWindowHelper.GetSoundLocation("bababooey.wav");
            player.Load();
            player.Play();
        }

        public GameField _field = null;
        private readonly Size _baseFieldSize = new Size(895, 895);
        private void AddGameField()
        {
            const int fromUpperBorderMargin = 25;
            const string fieldName = "GameField";
            _field = new GameField(_system, _userCards, _frame)
            {
                Height = _baseFieldSize.Width,
                Width = _baseFieldSize.Height,
                Name = fieldName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, fromUpperBorderMargin, 0, 0)
            };
            FieldGrid.Children.Add(_field);
        }

        private void SetStartValuesInUserCards()
        {
            for (int i = 0; i < _system.MonGame.Players.Count; i++)
            {
                _userCards[i].UserLogin.Text = _system.MonGame.Players[i].Login;
                _userCards[i].UserMoney.Text = _field.GetConvertedStringWithoutLastK(_system.MonGame.Players[i].AmountOfMoney);
                _userCards[i].SetNewCardImage(MainWindowHelper.GetCircleImage(
                    _userCards[i]._imgSize, _userCards[i]._imgSize,
                    DBQueries.GetPictureNameById(_system.MonGame.Players[i].GetPictureId())));
            }
        }

        private List<UserCard> _userCards = new List<UserCard>();
        public void SetUserCardsInList()
        {
            _userCards.Add(FirstPlayerRedControl);
            _userCards.Add(SecondPlayerBlueControl);
            _userCards.Add(ThirdPlayerGreenControl);
            _userCards.Add(FourthPlayerPurpleControl);
            _userCards.Add(FifthPlayerOrangeControl);

            List<UserCard> res = new List<UserCard>();
            List<SolidColorBrush> colors = Helper.GetColorsQueue();
            for (int i = 0; i < _system.MonGame.Players.Count; i++)
            {
                res.Add(_userCards[i]);
                _userCards[i].SetCircleColors(colors[i]);

                _userCards[i].MouseEnter += UserCard_MouseEnter;
                _userCards[i].MouseLeave += UserCard_MouseLeave;
            }

            for (int i = 0; i < _userCards.Count; i++)
            {
                if (!res.Contains(_userCards[i]))
                {
                    UserCards.Children.Remove(_userCards[i]);
                }

            }
            _userCards = res;    
        }

        private void UserCard_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void UserCard_MouseLeave(object sender, EventArgs e)
        {
            Cursor = null;
        }

        private void Page_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            FieldGrid.Children.OfType<GameField>().First().BussinessInfo.Children.Clear();
            _dropdownMenuPopup.IsOpen = false;
            _dropdownMenuPopup.PlacementTarget = null;
        }

        public Popup _dropdownMenuPopup;
        private void SetPopupsForUserCards()
        {
            GetContextMenu();
        }

        //private const int _horOffset = 0;
        private const int _vertOffset = 0;
        private void GetContextMenu()
        {
            const int popupWidth = 220;
            _dropdownMenuPopup = new Popup
            {
                Placement = PlacementMode.Bottom,
                //StaysOpen = false,
                HorizontalAlignment = HorizontalAlignment.Center,
                AllowsTransparency = true,
                Width = popupWidth,
                VerticalOffset = _vertOffset,
            };

            const int lastXPoint = 500;
            const int thickness = 1;
            const string bgColor = "#E6E9ED";
            var popupContent = new Border
            {
                Background = (SolidColorBrush)new BrushConverter().ConvertFromString(bgColor),
                //BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(0),
                //CornerRadius = new CornerRadius(5),
                Child = new StackPanel
                {
                    Children =
                    {
                        CreateMenuItem("Test first"),
                        new Line(){X1 = 0, Y1 = 0, X2 = lastXPoint, Y2 = 0, 
                            Stroke = Brushes.Black, StrokeThickness = thickness},
                        CreateMenuItem("Test second"),
                    }
                }
            };
            _dropdownMenuPopup.Child = popupContent;
        }

        private Button CreateMenuItem(string content)
        {
            const int butWidth = 220;
            var button = new Button
            {
                Content = content,
                Margin = new Thickness(0),
                Background = Brushes.Transparent,
                Width = butWidth,
                Foreground = Brushes.Black,
                BorderThickness = new Thickness(0),
            };

            //Its test Misha!!!
            button.Click += (s, e) =>
            {
                MessageBox.Show($"You choose: {content}");
                _dropdownMenuPopup.IsOpen = false;
            };
            return button;
        }

        private void UserCardPopup_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            int playerIndex = _field._ifBirthdayChance ? _field._toPayMoneyBirthdayIndex :
                _system.MonGame.StepperIndex;

            if (_system.MonGame.Players[playerIndex].IsLost || _field._ifBlockMenus ||
                (_field.ChatMessages.Children.OfType<DicesDrop>().Any())) return;
            if (_dropdownMenuPopup.PlacementTarget == null)
            {
                SetPopupMenu((UserCard)sender, playerIndex);
                _dropdownMenuPopup.PlacementTarget = ((UserCard)sender);
            }
            _dropdownMenuPopup.IsOpen = !_dropdownMenuPopup.IsOpen;
        }

        private void SetPopupMenu(UserCard card, int playerIndex)
        {
            int cardIndex = _userCards.IndexOf(card);

            StackPanel popupPanel = (StackPanel)((Border)_dropdownMenuPopup.Child).Child;
            popupPanel.Children.Clear();

            if (playerIndex == cardIndex)
            {
                SetGiveUpButton(popupPanel, playerIndex);
                return;
            }
            SetSendTradeButton(popupPanel, cardIndex);
        }

        private void SetSendTradeButton(StackPanel panel, int traderIndex)
        {
            _dropdownMenuPopup.Width = _userCards[traderIndex].UserCardGrid.Width;
            _dropdownMenuPopup.HorizontalOffset = (_userCards[traderIndex].Width - _userCards[traderIndex].UserCardGrid.Width) / 2;
            _dropdownMenuPopup.VerticalOffset = -(_userCards[traderIndex].Height - _userCards[traderIndex].UserCardGrid.Height) / 2;

            Button but = GetButtonForUserCardMenu(
                SystemParamsService.GetStringByName("SendTradePopup"), traderIndex);

            but.Click += (sender, e) =>
            {
                _field.CreateTrade(traderIndex);
            };
            panel.Children.Add(but);
        }

        private const int _centerDivider = 2;
        private void SetGiveUpButton(StackPanel panel, int playerIndex)
        {
            _dropdownMenuPopup.Width = _userCards[playerIndex].UserCardGrid.Width ;
            _dropdownMenuPopup.HorizontalOffset = 
                (_userCards[playerIndex].Width - 
                _userCards[playerIndex].UserCardGrid.Width) / _centerDivider;

            _dropdownMenuPopup.VerticalOffset =
                -Math.Abs((_userCards[playerIndex].Height -
                _userCards[playerIndex].UserCardGrid.Height) / _centerDivider); 

            Button but = GetButtonForUserCardMenu(SystemParamsService.GetStringByName("GiveUpPopup"), playerIndex);
            but.Click += (sender, e) =>
            {
                _field.PlayerGaveUp(playerIndex);
                //_dropdownMenuPopup.IsOpen = !_dropdownMenuPopup.IsOpen;
                _dropdownMenuPopup.IsOpen = false;
            };
            panel.Children.Add(but);
        }

        private Button GetButtonForUserCardMenu(string content, int playerIndex)
        {
            var button = new Button
            {
                Content = content,
                //Margin = new Thickness(10, 0, 0, 0),
                Background = Brushes.Transparent,
                Width = _userCards[playerIndex].UserCardGrid.Width,
                Foreground = Brushes.Black,
                BorderThickness = new Thickness(0),
                HorizontalContentAlignment = HorizontalAlignment.Center,
            };
            return button;
        }

        private Size _firstStep = new Size(1500, 950);
        private GamePageSize _windowSizeType = GamePageSize.BigWindow;
        private readonly Size _middleWindowSize = new Size(835, 835);

        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            IfPageIsRendered();
            if (!_ifPageIsRendered) return;

            if ((this.ActualWidth < _firstStep.Width ||
                this.ActualHeight < _firstStep.Height) &&
                _windowSizeType != GamePageSize.MediumWindow)
            {
                _windowSizeType = GamePageSize.MediumWindow;
                _field.UpdateFieldSize(_windowSizeType, _middleWindowSize);
            }
            else if(this.ActualWidth > _firstStep.Width &&
                this.ActualHeight > _firstStep.Height &&
                _windowSizeType != GamePageSize.BigWindow)
            {
                _windowSizeType = GamePageSize.BigWindow;
                _field.UpdateFieldSize(_windowSizeType, _baseFieldSize);
            }
        }

        private bool _ifPageIsRendered = false;
        private void IfPageIsRendered()
        {
            if (_ifPageIsRendered) return;
            Window parentWindow = Window.GetWindow(_frame);

            if(parentWindow is MainWindow window)
            {
                _ifPageIsRendered = window._ifGamePageIsRendered;
            }
        }

        public void StopGmeTimers()
        {
            _field.StopTimers();
        }
    }
}
