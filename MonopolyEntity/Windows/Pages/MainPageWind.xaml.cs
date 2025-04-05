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

        public void SetDescribeCards()
        {
            //return;
            SetParamsToCard(OneDesc, SystemParamsService.GetStringByName("FirstMainWindowName"),
                SystemParamsService.GetStringByName("FirstMainWindowDesc"), MainWindowHelper.GetImageByName("okay.png"));
            
            SetParamsToCard(TwoDesc, SystemParamsService.GetStringByName("SecondMainWindowName"),
                SystemParamsService.GetStringByName("SecondMainWindowDesc"), MainWindowHelper.GetImageByName("dices.png"));
            
            SetParamsToCard(ThreeDesc, SystemParamsService.GetStringByName("ThirdMainWindowName"),
                SystemParamsService.GetStringByName("ThirdMainWindowDesc"), MainWindowHelper.GetImageByName("cup.png"));
            
            SetParamsToCard(FourDesc, SystemParamsService.GetStringByName("FourthMainWindowName"),
                SystemParamsService.GetStringByName("FourthMainWindowDesc"), MainWindowHelper.GetImageByName("ratingPositive.png"));
            
            SetParamsToCard(FiveDesc, SystemParamsService.GetStringByName("FifthMainWindowName"),
                SystemParamsService.GetStringByName("FifthMainWindowDesc"), MainWindowHelper.GetImageByName("delivery.png"));
            
            SetParamsToCard(SixDesc, SystemParamsService.GetStringByName("SixthMainWindowName"),
                SystemParamsService.GetStringByName("SixthMainWindowDesc"), MainWindowHelper.GetImageByName("planet.png"));
        }

        public void SetParamsToCard(DescribeBox box, string nameText, string descText, Image img)
        {
            const int invWidth = 250;
            const int invHeight = 200;
            Size baseInvCardSize = new Size(invWidth, invHeight);

            box.Height = baseInvCardSize.Height;
            box.Width = baseInvCardSize.Width;
            box.CardImg.Source = img.Source;
            box.NameText.Text = nameText;
            box.DescribeText.Text = descText;
        }

        public void SetGameWindowImg()
        {
            GameWindowImg.Source = MainWindowHelper.GetImageByName("boardImg.png").Source;
        }

 /*       public void OpenInventoryPage()
        {
*//*            InventoryPage page = new InventoryPage(_frame, _system);
            _frame.Content = page;

            return;*//*
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new InventoryPage(_frame, _system));
        }*/

        public void OpenGameField()
        {
/*            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
            _frame.Content = page;*/

            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new SetPlayersInGame(_system, _frame));
        }

        public void OpenMainPage()
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new MainPage(_frame, _system));

/*            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;*/
        }

        private void StartGameButSecRow_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new SetPlayersInGame(_system, _frame));


/*            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
            _frame.Content = page;

            ((MainWindow)Window.GetWindow(_frame)).SetWindowSize(page);*/
        }
    }
}
