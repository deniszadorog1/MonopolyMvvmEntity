using MonopolyDLL.Monopoly;
using MonopolyEntity.Interfaces;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileSettings.xaml
    /// </summary>
    public partial class ProfileSettings : Page/*, IPagesOpener*/
    {
        private MonopolySystem _system;
        private Frame _frame;

        public ProfileSettings(MonopolySystem system, Frame frame)
        {
            _system = system;
            _frame = frame;
            InitializeComponent();

            AddControlParam();
            SetUpperMenuStyle();

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

/*        public void OpenInventoryPage()
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new InventoryPage(_frame, _system));

            *//*            InventoryPage page = new InventoryPage(_frame, _system);
                        _frame.Content = page;*//*
        }
*/

        public void OpenPage(bool ifMainPage)
        {
            Page page = ifMainPage ? new MainPage(_frame, _system) : (Page)new SetPlayersInGame(_system, _frame);
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(page);
        }

/*        public void OpenMainPage()
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new MainPage(_frame, _system));

            *//*            MainPage page = new MainPage(_frame, _system);
                        _frame.Content = page;*//*
        }

        public void OpenGameField()
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new SetPlayersInGame(_system, _frame));

            *//*            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
                        _frame.Content = page;*//*
        }
*/
        private void SetUpperMenuStyle()
        {
            //Set bg
            UpperMenuu.CanvasBg.Background = new SolidColorBrush(Colors.White);
            UpperMenuu.Background = new SolidColorBrush(Colors.White);

            //Set buttons colors
            UpperMenuu.MainLogoBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.StartGameBut.Foreground = new SolidColorBrush(Colors.White);
            UpperMenuu.StartGameBut.Background = (Brush)System.Windows.Application.Current.Resources["MainGlobalColor"];

            UpperMenuu.InventoryBut.Foreground = new SolidColorBrush(Colors.Gray);

            //UpperMenu.UserAnimation.UserIcon.Foreground = new SolidColorBrush(Colors.Gray);
            UpperMenuu.AllPanelGrid.Width = CenterColDef.Width.Value;
        }

        private UserSettings _settings;
        public void AddControlParam()
        {
            const int _size = 800;
            _settings = new UserSettings(_system, _frame)
            {
                Width = _size,
                Name = "ToCorrectControl"
            };
            SettignsParamGrid.Children.Add(_settings);
        }
    }
}
