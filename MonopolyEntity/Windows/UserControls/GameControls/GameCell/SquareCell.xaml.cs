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
    /// Логика взаимодействия для SquareCell.xaml
    /// </summary>
    public partial class SquareCell : UserControl, IAllCellActions
    {
        public SquareCell()
        {
            InitializeComponent();

            SetChipsCanvasSize();
        }
        
        public void SetChipsCanvasSize()
        {
            ChipsPlacer.Width = Width;
            ChipsPlacer.Height = Height;
        }

        public Size GetCellSize()
        {
            return new Size(ChipsPlacer.Width, ChipsPlacer.Height);
        }

        public void AddChip(Image chip)
        {
            ChipsPlacer.Children.Add(chip);
        }

        public Point GetCenterOfTheSquareForImage(Image img, int divider)
        {
            return new Point(ChipsPlacer.ActualWidth / divider - img.Width / divider,
                    ChipsPlacer.ActualHeight / divider - img.Height / divider);
        }

        public int GetAmountOfItemsInCell()
        {
            return ChipsPlacer.Children.Count;
        }

        public Size GetActualCellSize()
        {
            return new Size(ChipsPlacer.ActualWidth, ChipsPlacer.ActualHeight);
        }
    }
}
