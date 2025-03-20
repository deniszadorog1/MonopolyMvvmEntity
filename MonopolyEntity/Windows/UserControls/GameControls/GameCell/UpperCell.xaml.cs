using MonopolyDLL.Monopoly;
using MonopolyEntity.Interfaces;
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
    /// Логика взаимодействия для UpperCell.xaml
    /// </summary>
    public partial class UpperCell : UserControl, IClearCellVis, IGetLastChipImage, IChangeCellSize
    {
        public UpperCell()
        {
            InitializeComponent();
            SetChipsCanvasSize();
        }

        public void SetChipsCanvasSize()
        {
            const int divider = 5;
            const int multiplier = 4;
            ChipsPlacer.Width = Width;
            ChipsPlacer.Height = Height / divider * multiplier;
        }

        public void ClearCell()
        {
            const int clearOpacity = 1;

            ImagePlacer.Background = Brushes.White;
            ImagePlacer.Opacity = clearOpacity;
            StarsGrid.Children.Clear();
        }

        public Image GetLastChipImageFromBusCell()
        {
            return ImagePlacer.Children.OfType<Image>().Last();
        }

        public void ChangeCellSize(Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }
    }
}
