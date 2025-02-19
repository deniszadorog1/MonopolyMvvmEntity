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

namespace MonopolyEntity.Windows.UserControls.UserSettingsFolder
{
    /// <summary>
    /// Логика взаимодействия для ParamToCorrect.xaml
    /// </summary>
    public partial class ParamToCorrect : UserControl
    {
        public ParamToCorrect()
        {
            InitializeComponent();
        }

        private readonly SolidColorBrush _borderActiveColor = new SolidColorBrush(Color.FromRgb(59, 175, 218));

        private void ParamNameBox_GotFocus(object sender, RoutedEventArgs e)
        {
            ParamNameBox.BorderBrush = _borderActiveColor;
        }

        private void ParamNameBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ParamNameBox.BorderBrush = Brushes.Black;
        }

        private void SaveBox_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void SaveBox_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = null;
        }
    }
}
