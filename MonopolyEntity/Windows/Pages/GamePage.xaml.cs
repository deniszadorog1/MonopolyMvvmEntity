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
        }

        public void TestSound()
        {
            var player = new SoundPlayer();
            player.SoundLocation = MainWindowHelper.GetSoundLocation("bababooey.wav");
            player.Load();
            player.Play();
        }

        GameField _field;
        private readonly Size _baseFieldSize = new Size(950, 950);
        private void AddGameField()
        {
            const string fieldName = "GameField";
            _field = new GameField(_system, _userCards, _frame)
            {
                Height = _baseFieldSize.Width,
                Width = _baseFieldSize.Height, 
                Name = fieldName,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
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

        private Popup _dropdownMenuPopup;
        private void SetPopupsForUserCards()
        {
            GetContextMenu();
        }

        private void GetContextMenu()
        {
            const int popupWidth = 220;
            const int vertMove = -10;
            _dropdownMenuPopup = new Popup
            {
                Placement = PlacementMode.Bottom,
                //StaysOpen = false,
                AllowsTransparency = true,
                Width = popupWidth,
                VerticalOffset = vertMove,
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
                //Padding = new Thickness(10, 5, 10, 5),
                Margin = new Thickness(0),
                Background = Brushes.Transparent,
                Width = butWidth,
                Foreground = Brushes.Black,
                BorderThickness = new Thickness(0),
            };

            button.Click += (s, e) =>
            {
                MessageBox.Show($"You choosed: {content}");
                _dropdownMenuPopup.IsOpen = false;
            };
            return button;
        }

        private void UserCardPopup_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if(_system.MonGame.IfStepperLost()) return;
            if (_dropdownMenuPopup.PlacementTarget == null)
            {
                SetPopopMenu((UserCard)sender);
                _dropdownMenuPopup.PlacementTarget = ((UserCard)sender);
            }
            _dropdownMenuPopup.IsOpen = !_dropdownMenuPopup.IsOpen;
        }

        private void SetPopopMenu(UserCard card)
        {
            int cardIndex = _userCards.IndexOf(card);

            StackPanel popupPanel = (StackPanel)((Border)_dropdownMenuPopup.Child).Child;
            popupPanel.Children.Clear();

            if (_system.MonGame.IfIndexAndStepperIndexAreEqual(cardIndex))
            {
                SetGiveUpButton(popupPanel);
                return;
            }
            SetSendTradeButton(popupPanel, cardIndex);
        }

        private void SetSendTradeButton(StackPanel panel, int traderIndex)
        {
            Button but = GetButtonForUserCardMenu("Send trade");

            but.Click += (sender, e) =>
            {
                _field.CreateTrade(traderIndex);
            };
            panel.Children.Add(but);
        }

        private void SetGiveUpButton(StackPanel panel)
        {
            Button but = GetButtonForUserCardMenu("Give up");
            but.Click += (sender, e) =>
            {
                _field.PlayerGaveUp();
                _dropdownMenuPopup.IsOpen = !_dropdownMenuPopup.IsOpen;
            };
            panel.Children.Add(but);
        }

        private Button GetButtonForUserCardMenu(string content)
        {
            var button = new Button
            {
                Content = content,
                Margin = new Thickness(0),
                Background = Brushes.Transparent,
                Width = 220,
                Foreground = Brushes.Black,
                BorderThickness = new Thickness(0),
            };

            return button;
        }

        private Size _firstStep = new Size(1500, 900);
        private Size _zeroStep = new Size(1920, 1080);
        private void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(this.ActualWidth < _firstStep.Width &&
                this.ActualHeight < _firstStep.Height)
            {
                Console.WriteLine();
            }
        }
    }
}
