using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
using MonopolyDLL;
using MonopolyDLL.Services;

namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages
{
    /// <summary>
    /// Логика взаимодействия для JackpotElem.xaml
    /// </summary>
    public partial class JackpotElem : UserControl
    {
        public bool _ifPlayerHasEnoughMoney = false;
        private SolidColorBrush _inActiveColor = (SolidColorBrush)Application.Current.Resources["InActiveColor"];

        public JackpotElem(bool isPlayerHasEnoughMoney)
        {
            _ifPlayerHasEnoughMoney = isPlayerHasEnoughMoney; 
            InitializeComponent();

            SetBordersRibsInList();

            SetPlayButton(_ifPlayerHasEnoughMoney);
        }

        public void SetPlayButton(bool isPlayerHasEnoughMoney)
        {
            if (isPlayerHasEnoughMoney)
            {
                MakeBidBut.Background = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];
                //LockImage.Visibility = Visibility.Hidden;
                return;
            }
            MakeBidBut.Background = _inActiveColor;
            LockImage.Visibility = Visibility.Visible;
        }

        List<Border> _ribBorders = new List<Border>();
        private void SetBordersRibsInList()
        {
            _ribBorders.Add(OneRibBorder);
            _ribBorders.Add(TwoRibBorder);
            _ribBorders.Add(ThreeRibBorder);
            _ribBorders.Add(FourRibBorder);
            _ribBorders.Add(FiveRibBorder);
            _ribBorders.Add(SixRibBorder);

            SetEventsForBorders();
        }

        const double _basicOpacity = 0.5;
        private void SetEventsForBorders()
        {
            for (int i = 0; i < _ribBorders.Count; i++)
            {
                if (_ribBorders[i] is Border border)
                {
                    border.Opacity = _basicOpacity;
                    border.PreviewMouseDown += RibBorder_PreviewMouseDown;
                }
            }
        }

        public List<int> _chosenRibs = new List<int>();
        private void RibBorder_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            const int baseOpacity = 1;
            const int indexAdder = 1;
            int index = -1;
            for (int i = 0; i < _ribBorders.Count; i++)
            {
                if (_ribBorders[i] == sender)
                {
                    index = i;
                }
            }

            const int maxAllowedChosenRib = 3;
            SolidColorBrush brush = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];

            if (sender is Border border)
            {
                if (border.BorderBrush == brush)
                {
                    border.Opacity = _basicOpacity;
                    border.BorderBrush = Brushes.Black;
                    _chosenRibs.Remove((index + indexAdder));
                }
                else if (border.BorderBrush != brush &&
                    GetAmountOfChosenBorders(brush) < maxAllowedChosenRib)
                {
                    border.Opacity = baseOpacity;
                    border.BorderBrush = brush;
                    if (index != -1) _chosenRibs.Add((index + indexAdder));
                }
            }

            SetPotMoneyToWin(brush);
        }

        public void SetPotMoneyToWin(SolidColorBrush globalBrush)
        {
            int amount = GetAmountOfChosenBorders(globalBrush);
            const string notChosenCasinoRibs = "-";
            switch (amount)
            {
                case 0:
                    PotWon.Text = notChosenCasinoRibs;
                    return;
                case 1:
                    PotWon.Text = GetConvertedPrice(SystemParamsService.GetNumByName("CasinoWinThird"));
                    return;
                case 2:
                    PotWon.Text = GetConvertedPrice(SystemParamsService.GetNumByName("CasinoWinSecond"));
                    return;
                case 3:
                    PotWon.Text = GetConvertedPrice(SystemParamsService.GetNumByName("CasinoWinFirst"));
                    return;
                    
            }
            throw new Exception("Something wrong with casino ribs choosing");
        }


        public string GetConvertedPrice(int price)
        {
            const char endAdd = 'k';
            const char thousandDivider = ',';
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < price.ToString().Length; i++)
            {
                build.Append(price.ToString()[i]);
            }

            for (int i = price.ToString().Length; i >= 0; i--)
            {
                if (i % 3 == 0 && i != 0 && i != price.ToString().Length)
                {
                    build.Insert(price.ToString().Length - i, thousandDivider);
                }
            }

            build.Append(endAdd);
            return build.ToString();
        }

        private int GetAmountOfChosenBorders(SolidColorBrush brush)
        {
            return _ribBorders.Where(x => x.BorderBrush == brush).Count();
        }

        const int _cubeSize = 100;
        const int _circleSize = 8;
        private const int _centerDivider = 2;
        private void SetCubeSquares()
        {
            const int distanceFromBorder = 10;

            Point center = new Point(OneRibGrid.ActualWidth / _centerDivider - _circleSize / _centerDivider,
                OneRibGrid.ActualHeight / _centerDivider - _circleSize / _centerDivider);

            Point upLeft = new Point(distanceFromBorder, distanceFromBorder);
            Point upRight = new Point(OneRibGrid.ActualWidth - _circleSize - distanceFromBorder, distanceFromBorder);

            Point downLeft = new Point(distanceFromBorder, OneRibGrid.ActualHeight - _circleSize - distanceFromBorder);
            Point downRight = new Point(OneRibGrid.ActualWidth - _circleSize - distanceFromBorder,
                OneRibGrid.ActualHeight - _circleSize - distanceFromBorder);

            Point centerLeft = new Point(distanceFromBorder, 
                OneRibGrid.ActualHeight / _centerDivider - _circleSize / _centerDivider);
            Point centerRight = new Point(OneRibGrid.ActualWidth - _circleSize - distanceFromBorder,
                OneRibGrid.ActualHeight / _centerDivider - _circleSize / _centerDivider);


            OneRibGrid.Children.Add(GetEllipseInRib(center));

            TwoRibGrid.Children.Add(GetEllipseInRib(upRight));
            TwoRibGrid.Children.Add(GetEllipseInRib(downLeft));

            ThreeRibGrid.Children.Add(GetEllipseInRib(upRight));
            ThreeRibGrid.Children.Add(GetEllipseInRib(center));
            ThreeRibGrid.Children.Add(GetEllipseInRib(downLeft));

            FourRibGrid.Children.Add(GetEllipseInRib(upLeft));
            FourRibGrid.Children.Add(GetEllipseInRib(upRight));
            FourRibGrid.Children.Add(GetEllipseInRib(downRight));
            FourRibGrid.Children.Add(GetEllipseInRib(downLeft));

            FiveRibGrid.Children.Add(GetEllipseInRib(upLeft));
            FiveRibGrid.Children.Add(GetEllipseInRib(upRight));
            FiveRibGrid.Children.Add(GetEllipseInRib(downRight));
            FiveRibGrid.Children.Add(GetEllipseInRib(downLeft));
            FiveRibGrid.Children.Add(GetEllipseInRib(center));

            SixRibGrid.Children.Add(GetEllipseInRib(upLeft));
            SixRibGrid.Children.Add(GetEllipseInRib(centerLeft));
            SixRibGrid.Children.Add(GetEllipseInRib(downLeft));
            SixRibGrid.Children.Add(GetEllipseInRib(upRight));
            SixRibGrid.Children.Add(GetEllipseInRib(centerRight));
            SixRibGrid.Children.Add(GetEllipseInRib(downRight));
        }

        private Ellipse GetEllipseInRib(Point point)
        {
            Ellipse el = new Ellipse()
            {
                Width = _circleSize,
                Height = _circleSize,
                Fill = Brushes.Black
            };

            Canvas.SetLeft(el, point.X);
            Canvas.SetTop(el, point.Y);

            return el;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetCubeSquares();
        }

        private void Border_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void Border_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = null;
        }
    }
}
