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
    /// Логика взаимодействия для PrisonCell.xaml
    /// </summary>
    public partial class PrisonCell : UserControl
    {
        public PrisonCell()
        {
            InitializeComponent();
            SetChipsCanvasSize();

        }
        public void SetChipsCanvasSize()
        {
            ChipsPlacer.Width = Width;
            ChipsPlacer.Height = Height;
        }
    }
}
