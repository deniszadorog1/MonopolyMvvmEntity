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

namespace MonopolyEntity.Windows.UserControls.GameControls.GameCell
{
    /// <summary>
    /// Логика взаимодействия для LeftPart.xaml
    /// </summary>
    public partial class LeftCell : UserControl
    {
        public LeftCell()
        {
            InitializeComponent();

            SetChipsCanvasSize();
        }

        public void SetChipsCanvasSize()
        {
            const int devider = 5;
            const int multiplier = 4;
            ChipsPlacer.Width = Width / devider * multiplier;
            ChipsPlacer.Height = Height;
        }
    }
}
