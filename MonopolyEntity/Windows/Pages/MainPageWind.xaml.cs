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
using MonopolyDLL;
using System.CodeDom;
using System.Data.Entity.ModelConfiguration.Conventions;
using MonopolyEntity.Interfaces;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page, IPagesOpener
    {
        private Frame _frame;
        private MonopolySystem _system;
        public MainPage(Frame workFrame, MonopolySystem system)
        {
            _frame = workFrame;
            _system = system;
            InitializeComponent();

            SetGameWindowImg();
            SetDescribeCards();

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

        public void SetDescribeCards()
        {
            //return;
            SetParamsToCard(OneDesc, SystemParamsService.GetStringByName("FirstMainWindowName"),
                SystemParamsService.GetStringByName("FirstMainWindowDesc"), MainWindowHelper.GetImagePyName("okay.png"));
            
            SetParamsToCard(TwoDesc, SystemParamsService.GetStringByName("SecondMainWindowName"),
                SystemParamsService.GetStringByName("SecondMainWindowDesc"), MainWindowHelper.GetImagePyName("dices.png"));
            
            SetParamsToCard(ThreeDesc, SystemParamsService.GetStringByName("ThirdMainWindowName"),
                SystemParamsService.GetStringByName("ThirdMainWindowDesc"), MainWindowHelper.GetImagePyName("cup.png"));
            
            SetParamsToCard(FourDesc, SystemParamsService.GetStringByName("FourthMainWindowName"),
                SystemParamsService.GetStringByName("FourthMainWindowDesc"), MainWindowHelper.GetImagePyName("rating-positive.png"));
            
            SetParamsToCard(FiveDesc, SystemParamsService.GetStringByName("FifthMainWindowName"),
                SystemParamsService.GetStringByName("FifthMainWindowDesc"), MainWindowHelper.GetImagePyName("delivery.png"));
            
            SetParamsToCard(SixDesc, SystemParamsService.GetStringByName("SixthMainWindowName"),
                SystemParamsService.GetStringByName("SixthMainWindowDesc"), MainWindowHelper.GetImagePyName("planet.png"));
        }

        public void SetParamsToCard(DescribeBox box, string nameText, string descText, Image img)
        {
            Size baseInvCardSize = new Size(250, 200);

            box.Height = baseInvCardSize.Height;
            box.Width = baseInvCardSize.Width;
            box.CardImg.Source = img.Source;
            box.NameText.Text = nameText;
            box.DescribeText.Text = descText;
        }

        public void SetGameWindowImg()
        {
            GameWindowImg.Source = MainWindowHelper.GetImagePyName("BoardImg.png").Source;
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
