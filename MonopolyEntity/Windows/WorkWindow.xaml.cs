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
using System.Windows.Shapes;

using MonopolyEntity.Windows.Pages;
using MonopolyDLL.Monopoly;

namespace MonopolyEntity.Windows
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindow : Window
    {
        private MonopolySystem _monSystem;
        public WorkWindow(MonopolySystem monSystem)
        {
            _monSystem = monSystem;
            InitializeComponent();

            SetStartPage();
        }

        public void SetStartPage()
        {
            SetWindowSize();
            MainPage mainPage = new MainPage(WorkFrame, _monSystem);
            WorkFrame.Content = mainPage;   
        }

        public void SetWindowSize()
        {
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
