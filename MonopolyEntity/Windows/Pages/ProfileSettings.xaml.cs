using MonopolyDLL.Monopoly;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для ProfileSettings.xaml
    /// </summary>
    public partial class ProfileSettings : Page
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
        
        public void OpenInventoryPage()
        {
            InventoryPage page = new InventoryPage(_frame, _system);
            _frame.Content = page;
        }

        public void OpenMainPage()
        {
            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;
        }

        public void OpenGameField()
        {
            SetPlayersInGame page = new SetPlayersInGame(_system, _frame);
            _frame.Content = page;
        }

        private void SetUpperMenuStyle()
        {
            //Set bg
            UpperMenuu.CanvasBg.Background = new SolidColorBrush(Colors.White);
            UpperMenuu.Background = new SolidColorBrush(Colors.White);

            //Set buttons colors
            UpperMenuu.MainLogoBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.StartGameBut.Foreground = new SolidColorBrush(Colors.White);
            UpperMenuu.StartGameBut.Background = (Brush)Application.Current.Resources["MainGlobalColor"];

            UpperMenuu.InventoryBut.Foreground = new SolidColorBrush(Colors.Gray);

            //UpperMenuu.UserAnim.UserIcon.Foreground = new SolidColorBrush(Colors.Gray);
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
