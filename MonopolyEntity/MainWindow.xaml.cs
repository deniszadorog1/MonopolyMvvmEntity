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

namespace MonopolyEntity
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MonopolyDLL.Monopoly.MonopolySystem _monopolySys;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBut_Click(object sender, RoutedEventArgs e)
        {
            //Set logged user here

            WorkWindow window = new WorkWindow(_monopolySys);
            window.ShowDialog();

        }
    }
}
