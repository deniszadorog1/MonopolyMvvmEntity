using MonopolyEntity.Windows.UserControls;
using System;
using System.Collections.Generic;
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

using MonopolyEntity.Windows;
using MonopolyDLL.Monopoly;

using MonopolyEntity.Windows.Pages;
using System.ComponentModel;
namespace MonopolyEntity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetStartPage();
        }

        private MonopolySystem _system = new MonopolySystem();
        public void SetStartPage()
        {
            MainFrame.Content = new StartPage(MainFrame, _system);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (MainFrame.Content is StartPage) return;
            
            if (MainFrame.Content is RegistrationPage || 
                MainFrame.Content is WorkPage || MainFrame.Content is MainPage) 
            {
                this.Hide();
                MainFrame.Content = new StartPage(MainFrame, _system);
                e.Cancel = true;
                return;
                InitStartPage(e);
            }
            else if (MainFrame.Content is SetPlayersInGame playersInGame || 
                MainFrame.Content is GamePage game || 
                MainFrame.Content is InventoryPage inventory ||
                MainFrame.Content is ProfileSettings settings) 
            {
                ClosePopupIfGame();

                ClearCaseOpenEffect();
                ClearGame();

                InitStartPage(e);
            }
        }

        private void InitStartPage(CancelEventArgs e)
        {
            this.Hide();
            MainFrame.Content = new MainPage(MainFrame, _system);
            e.Cancel = true;
        }

        private void ClosePopupIfGame()
        {
            if(MainFrame.Content is GamePage game)
            {
                game._dropdownMenuPopup.IsOpen = false;
                _ifGamePageIsRendered = false;
            }
        }

        public void ClearGame()
        {
            _system.MonGame = new Game(_system.LoggedUser);
            if(MainFrame.Content is GamePage game)
            {
                game.StopGmeTimers();
            }
        }

        public void ClearCaseOpenEffect()
        {
            VisiableItems.Children.Clear();
            CaseFrame.Content = null;
            
            MainFrame.Effect = null;
            VisiableItems.Effect = null;

            CaseFrame.Visibility = Visibility.Hidden;
        }

        public void SetWindowSize(Page page)
        {
            if (page is WorkPage || page is MainPage || page is GamePage || 
                page is InventoryPage || page is ProfileSettings)
            {
                SetMaxSizeOfPage(page);
                return;
            }
            this.MaxWidth = page.MaxWidth;
            this.MaxHeight = page.MaxHeight;

            this.Width = this.MaxWidth;
            this.Height = this.MaxHeight;

            SetInLittleWindow();
        }

        private void SetMaxSizeOfPage(Page page)
        {
            this.MaxWidth = SystemParameters.PrimaryScreenWidth;
            this.MaxHeight = SystemParameters.PrimaryScreenHeight;

            this.Width = this.MaxWidth;
            this.Height = this.MaxHeight;

            this.Top = 0;
            this.Left = 0;
        }

        private const int _centerDivider = 2;
        public void SetInLittleWindow()
        {
           this.WindowState = WindowState.Normal;

            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double screenHeight = SystemParameters.PrimaryScreenHeight;

            this.Left = (screenWidth - this.Width) / _centerDivider;
            this.Top = (screenHeight - this.Height) / _centerDivider;
        }

        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            this.Hide();
        }

        public bool _ifGamePageIsRendered = false;
        private void MainFrame_ContentRendered(object sender, EventArgs e)
        {
            SetWindowSize((Page)MainFrame.Content);

            if(MainFrame.Content is GamePage)
            {
                _ifGamePageIsRendered = true;
            }

            this.Show();
        }

        public void ClearVisibleItems()
        {
            VisiableItems.Children.Clear();
        }

        private readonly Size _baseSize = new Size(450, 450);
        private readonly Size _minSize = new Size(1500, 900);
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if(MainFrame.Content is GamePage game)
            {
                MinHeight = _minSize.Height;
                MinWidth = _minSize.Width;
                e.Handled = true;
                game._dropdownMenuPopup.IsOpen = false;
            }
            else
            {
                MinHeight = _baseSize.Height;
                MinWidth = _baseSize.Width;
            }
           
        }
    }
}
