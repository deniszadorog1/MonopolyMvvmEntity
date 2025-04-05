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
    /// Логика взаимодействия для BottomCell.xaml
    /// </summary>
    public partial class BottomCell : UserControl, IClearCellVis, IGetLastChipImage, 
        IChangeCellSize, IRegularCellsActions, IAllCellActions
    {
        public BottomCell()
        {
            InitializeComponent();

            SetChipsCanvasSize();
        }

        public void ChangeCellSize(Size size)
        {
            Width = size.Width;
            Height = size.Height;
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

        public void SetImagePlacerBg(SolidColorBrush brush)
        {
            ImagePlacer.Background = brush;
        }

        public Grid GetImagePlacer()
        {
            return ImagePlacer;
        }

        public Grid GetStarGrid()
        {
            return StarsGrid;
        }

        public Size GetCellSize()
        {
            return new Size(ChipsPlacer.Width, ChipsPlacer.Height);
        }

        public Image GetLastChipImageFromBusCell()
        {
            return ImagePlacer.Children.OfType<Image>().Last();
        }

        public void AddChip(Image chip)
        {
            ChipsPlacer.Children.Add(chip);
        }

        public TextBlock GetMoneyTextBlock()
        {
            return Money;
        }

        public Point GetCenterOfTheSquareForImage(Image img, int divider)
        {
            return new Point(ChipsPlacer.ActualWidth / divider - img.Width / divider,
                    ChipsPlacer.ActualHeight / divider - img.Height / divider);
        }

        public void SetValueForDepositCounter(int value)
        {
            DepositObj.Counter.Text = value.ToString();
        }

        public void ChangeDepositCounterVisibility(Visibility vis)
        {
            DepositObj.Visibility = vis;
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
