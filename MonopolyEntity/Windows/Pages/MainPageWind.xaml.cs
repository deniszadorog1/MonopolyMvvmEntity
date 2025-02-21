using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using MahApps.Metro.Actions;
using MonopolyEntity.Windows.UserControls.MainPage;

using MonopolyDLL.Monopoly;
using MonopolyEntity.VisualHelper;
using System.Windows;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame _frame;
        private MonopolySystem _system;
        public MainPage(Frame workFrame, MonopolySystem system)
        {
            _frame = workFrame;
            _system = system;
            InitializeComponent();

            SetGameWindowImg();
            SetDescRibeCards();

            SetUserImageEvent();

            SetUserMenu();
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

        public void SetUserImageEvent()
        {
            return;
            UpperMenuu.UserAnim.UpperRowUserName.MouseDown += (sender, e) =>
            {
                _frame.Content = new UserPage(_frame, _system);
            };
        }

        public void SetDescRibeCards()
        {
            //return;
            SetParamsToCard(OneDesc,"One", "This is first", MainWindowHelper.GetImagePyName("okay.png"));
            SetParamsToCard(TwoDesc, "Two", "This is second", MainWindowHelper.GetImagePyName("dices.png"));
            SetParamsToCard(ThreeDesc, "Three", "This is third", MainWindowHelper.GetImagePyName("cup.png"));
            SetParamsToCard(FourDesc, "Four", "This is fourth", MainWindowHelper.GetImagePyName("rating-positive.png"));
            SetParamsToCard(FiveDesc, "Five", "This is fifth", MainWindowHelper.GetImagePyName("delivery.png"));
            SetParamsToCard(SixDesc, "Six", "This is sixth", MainWindowHelper.GetImagePyName("planet.png"));
        }

        public void SetParamsToCard(DescribeBox box, string nameText, string descText, Image img)
        {
            box.Height = 200;
            box.Width = 250;
            box.CardImg.Source = img.Source;
            box.NameText.Text = nameText;
            box.DescribeText.Text = descText;
        }

        public void SetGameWindowImg()
        {
            GameWindowImg.Source = MainWindowHelper.GetImagePyName("BoardImg.png").Source; //ThingForTest.GetCalivanImage().Source;
        }

        public void OpenInventoryPage()
        {
            InventoryPage page = new InventoryPage(_frame, _system);
            _frame.Content = page;
        }

        public void OpenGameField()
        {
            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
            _frame.Content = page;

            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize(page);
        }

        public void OpenMainPage()
        {
            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;
        }

        private void StartGameButSecRow_Click(object sender, RoutedEventArgs e)
        {
            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
            _frame.Content = page;

            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize(page);
        }
    }
}
