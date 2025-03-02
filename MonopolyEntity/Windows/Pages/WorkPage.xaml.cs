using MonopolyDLL.Monopoly;
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

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для WorkPage.xaml
    /// </summary>
    public partial class WorkPage : Page
    {
        private MonopolySystem _monSystem;
        private Frame _frame;
        public WorkPage(MonopolySystem monSystem, Frame frmae)
        {
            _monSystem = monSystem;
            _frame = frmae;

            InitializeComponent();
            SetStartPage();
        }

        public void SetStartPage()
        {
            SetWindowSize();
            MainPage mainPage = new MainPage(_frame, _monSystem);
            _frame.Content = mainPage;
        }

        public void SetWindowSize()
        {
            return;
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Maximized;
            }
        }

        public void ClearVisiableItems()
        {
            VisiableItems.Children.Clear();
        }
    }
}
