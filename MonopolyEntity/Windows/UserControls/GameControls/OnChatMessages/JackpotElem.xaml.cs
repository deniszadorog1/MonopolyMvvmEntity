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

namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages
{
    /// <summary>
    /// Логика взаимодействия для JackpotElem.xaml
    /// </summary>
    public partial class JackpotElem : UserControl
    {
        public JackpotElem()
        {
            InitializeComponent();

            SetBordersRibsInList();
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
            for(int i = 0; i < _ribBorders.Count; i++)
            {
                if (_ribBorders[i] is Border border)
                {
                    border.Opacity = _basicOpacity;
                    border.PreviewMouseDown += RibBorder_PreviewMouseDown;
                }
            }
        }

        private void RibBorder_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            const int maxAllowedChosenRib = 3;
            SolidColorBrush brush = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];
            //Color color = brush.Color;

            if(sender is Border border)
            {
                if (border.BorderBrush == brush)
                {
                    border.Opacity = _basicOpacity;
                    border.BorderBrush = Brushes.Black;
                }
                else if(border.BorderBrush != brush &&
                    GetAmountOfChosenBorders(brush) < maxAllowedChosenRib)
                {
                    border.Opacity = 1;
                    border.BorderBrush = brush;
                }
            }
        }
        
        private int GetAmountOfChosenBorders(SolidColorBrush brush)
        {
            return _ribBorders.Where(x => x.BorderBrush == brush).Count();
        }
        



        const int _cubeSize = 100;
        const int _circleSize = 8;
        private void SetCubeSquares()
        {
            const int distFromBorder = 10;

            Point center = new Point(OneRibGrid.ActualWidth / 2 - _circleSize / 2, 
                OneRibGrid.ActualHeight / 2 - _circleSize / 2);

            Point upLeft = new Point(distFromBorder, distFromBorder);
            Point upRight = new Point(OneRibGrid.ActualWidth - _circleSize - distFromBorder, distFromBorder);

            Point downLeft = new Point(distFromBorder, OneRibGrid.ActualHeight - _circleSize - distFromBorder);
            Point downRight = new Point(OneRibGrid.ActualWidth - _circleSize - distFromBorder,
                OneRibGrid.ActualHeight - _circleSize - distFromBorder);

            Point centerLeft = new Point(distFromBorder, OneRibGrid.ActualHeight / 2 - _circleSize / 2);
            Point centerRight = new Point(OneRibGrid.ActualWidth - _circleSize - distFromBorder, OneRibGrid.ActualHeight / 2 - _circleSize / 2);


            OneRibGrid.Children.Add(GetElipseInRib(center));

            TwoRibGrid.Children.Add(GetElipseInRib(upRight));
            TwoRibGrid.Children.Add(GetElipseInRib(downLeft));

            ThreeRibGrid.Children.Add(GetElipseInRib(upRight));
            ThreeRibGrid.Children.Add(GetElipseInRib(center));
            ThreeRibGrid.Children.Add(GetElipseInRib(downLeft));

            FourRibGrid.Children.Add(GetElipseInRib(upLeft));
            FourRibGrid.Children.Add(GetElipseInRib(upRight));
            FourRibGrid.Children.Add(GetElipseInRib(downRight));
            FourRibGrid.Children.Add(GetElipseInRib(downLeft));

            FiveRibGrid.Children.Add(GetElipseInRib(upLeft));
            FiveRibGrid.Children.Add(GetElipseInRib(upRight));
            FiveRibGrid.Children.Add(GetElipseInRib(downRight));
            FiveRibGrid.Children.Add(GetElipseInRib(downLeft));
            FiveRibGrid.Children.Add(GetElipseInRib(center));

            SixRibGrid.Children.Add(GetElipseInRib(upLeft));
            SixRibGrid.Children.Add(GetElipseInRib(centerLeft));
            SixRibGrid.Children.Add(GetElipseInRib(downLeft));
            SixRibGrid.Children.Add(GetElipseInRib(upRight));
            SixRibGrid.Children.Add(GetElipseInRib(centerRight));
            SixRibGrid.Children.Add(GetElipseInRib(downRight));        
        }

        private Ellipse GetElipseInRib(Point point)
        {
            Ellipse el =  new Ellipse()
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
    }
}
