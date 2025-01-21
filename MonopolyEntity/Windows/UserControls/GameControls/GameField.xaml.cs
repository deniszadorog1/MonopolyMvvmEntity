using MahApps.Metro.Controls;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

using MonopolyDLL;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using System.Security.AccessControl;
using MonopolyEntity.Windows.UserControls.GameControls.BussinessInfo;
using System.Windows.Media.TextFormatting;
using System.Data.SqlTypes;

using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages;

namespace MonopolyEntity.Windows.UserControls.GameControls
{
    /// <summary>
    /// Логика взаимодействия для GameField.xaml
    /// </summary>
    /// 

    public partial class GameField : UserControl
    {
        public GameField()
        {
            InitializeComponent();

            SetCellsInList();

            SetImmoratlImages();
            HideHeadersInCells();

            SetChipPositionsInStartSquare();

            SetClickEventForBusCells();
            SetStartPricesForBusses();
            SetTestJacPotClick();

            SetHeaderColorsForBuses();
        }

        private void SetHeaderColorsForBuses()
        {
            PerfumeFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PerfumeColor"];
            PerfumeSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PerfumeColor"];

            UpCar.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
            RightCar.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
            DownCars.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
            LeftCar.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];

            ClothesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["ClothesColor"];
            ClothesSeconBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["ClothesColor"];
            ClothesThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["ClothesColor"];

            MessagerFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["MessagerColor"];
            MessagerSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["MessagerColor"];
            MessagerThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["MessagerColor"];

            DrinkFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["DrinkColor"];
            DrinkSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["DrinkColor"];
            DrinkThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["DrinkColor"];

            PlanesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PlaneColor"];
            PlanesSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PlaneColor"];
            PlanesThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PlaneColor"];


            FoodFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["FoodColor"];
            FoodSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["FoodColor"];
            FoodThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["FoodColor"];

            HotelFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["HotelColor"];
            HotelSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["HotelColor"];
            HotelThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["HotelColor"];

            PhonesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PhoneColor"];
            PhonesSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PhoneColor"];

            GamesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];
            GamesSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];
        }

        private void SetTestJacPotClick()
        {
            JackpotElem jackpot = new JackpotElem();

            JackPotCell.PreviewMouseDown += (sender, e) =>
            {
                ChatMessages.Children.Remove(jackpot);
                ChatMessages.Children.Add(jackpot);
            };
        }

        private void SetStartPricesForBusses()
        {
            const string testString = "55555";
            List<string> prices = new List<string>();
            for (int i = 0; i < _cells.Count; i++)
            {
                prices.Add(testString);
            }

            for (int i = 0; i < _cells.Count; i++)
            {
                SetPriceInCell(prices[i], _cells[i]);
            }
        }

        private void SetPriceInCell(string price, UIElement cell)
        {
            if (cell is UpperCell up &&
                up.MoneyPlacer.Visibility == Visibility.Visible)
            {
                up.Money.Text = price;

            }
            else if (cell is RightCell right &&
                right.MoneyPlacer.Visibility == Visibility.Visible)
            {
                right.Money.Text = price;
                RotateTextBlock(right.Money, 90);
            }
            else if (cell is BottomCell bot &&
                bot.MoneyPlacer.Visibility == Visibility.Visible)
            {
                bot.Money.Text = price;
            }
            else if (cell is LeftCell left &&
                left.MoneyPlacer.Visibility == Visibility.Visible)
            {
                left.Money.Text = price;
                RotateTextBlock(left.Money, -90);
            }
        }

        private void RotateTextBlock(TextBlock block, int angle)
        {
            block.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform transform = new RotateTransform()
            {
                Angle = angle,
            };

            block.LayoutTransform = transform;
        }


        private void SetClickEventForBusCells()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SetClickEventForCell(_cells[i]);
            }
        }

        private void SetClickEventForCell(UIElement element)
        {
            if (element is UpperCell upper)
                upper.PreviewMouseDown += Bussiness_PreviewMouseDown;

            if (element is RightCell right)
                right.PreviewMouseDown += Bussiness_PreviewMouseDown;

            if (element is BottomCell bottom)
                bottom.PreviewMouseDown += Bussiness_PreviewMouseDown;

            if (element is LeftCell left)
                left.PreviewMouseDown += Bussiness_PreviewMouseDown;
        }

        private void Bussiness_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            BussinessInfo.Children.Clear();

            //Add info about business
            GameBusInfo info = new GameBusInfo();
            BussinessInfo.Children.Add(info);

            info.UpdateLayout();
            SetLocForInfoBox(info, (UIElement)sender);
        }

        private void SetLocForInfoBox(GameBusInfo info, UIElement cell)
        {
            const int distToCell = 5;
            Point fieldPoint = this.PointToScreen(new Point(0, 0));
            Point cellPoint = cell.PointToScreen(new Point(0, 0));

            Point cellLocalPoint = new Point(cellPoint.X - fieldPoint.X, cellPoint.Y - fieldPoint.Y);

            Point infoLoc = new Point(0, 0);
            if (cell is UpperCell up)
            {
                infoLoc = new Point(cellLocalPoint.X + up.Width / 2 - info.ActualWidth / 2, up.Height + distToCell);
            }
            else if (cell is RightCell right)
            {
                double jacPotYLoc = JackPotCell.PointToScreen(new Point(0, 0)).Y - fieldPoint.Y;

                double yLoc = cellLocalPoint.Y + info.ActualHeight > jacPotYLoc ?
                    jacPotYLoc - info.ActualHeight : cellLocalPoint.Y;

                infoLoc = new Point(cellLocalPoint.X - info.ActualWidth - distToCell, yLoc);
            }
            else if (cell is BottomCell bot)
            {
                infoLoc = new Point(cellLocalPoint.X + bot.Width / 2 - info.ActualWidth / 2, cellLocalPoint.Y - info.ActualHeight - distToCell);
            }
            else if (cell is LeftCell left)
            {
                double jacPotYLoc = JackPotCell.PointToScreen(new Point(0, 0)).Y - fieldPoint.Y;

                double yLoc = cellLocalPoint.Y + info.ActualHeight > jacPotYLoc ?
                    jacPotYLoc - info.ActualHeight : cellLocalPoint.Y;

                infoLoc = new Point(cellLocalPoint.X + left.Width + distToCell, yLoc);
            }

            Canvas.SetLeft(info, infoLoc.X);
            Canvas.SetTop(info, infoLoc.Y);
        }


        //BREAKETS, mb need to move it into anouther file
        //set chips position and make a movement  

        //Set chips position in fields mathematicly


        private int counter = 0;
        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            return;
            if (counter >= 1) return;
            //MakeCheckThere
            //Point globalPosition = StartCell.PointToScreen(new Point(0, 0));

            const int startCellIndex = 0;
            const int cellIndexToMoveOn = 10;

            //make movement action
            MoveToPrisonCell(startCellIndex, cellIndexToMoveOn, _imgs[0]);

            //MakeChipsMovementAction(startCellIndex, cellIndexToMoveOn, _imgs[0]);
            counter++;
        }

        private void MoveToPrisonCell(int startCellIndex, int cellIndexToMoveOn, Image img)
        {
            MakeChipsMovementAction(startCellIndex, cellIndexToMoveOn, img);
        }

        private List<(Image, int)> chipAndCord = new List<(Image, int)>();

        private void MakeChipsMovementAction(int startCellIndex, int cellIndexToMoveOn, Image chipImage)
        {
            if (_ifChipMoves) return;

            _squareIndexesToGoThrough =
                BoardHelper.GetListOfSquareCellIndexesThatChipGoesThrough(startCellIndex, cellIndexToMoveOn);

            if (!(_squareIndexesToGoThrough is null))
            {
                cellIndexToMoveOn = _squareIndexesToGoThrough.First();
            }

            //const int moveDist = 10;
            //Image tempChipImg = _imgs[0];

            Point chipToFieldPoint = GetChipImageLocToField(chipImage);

            Point insidePointStartCell = new Point(Canvas.GetLeft(chipImage), Canvas.GetTop(chipImage));
            Point newInsideChipPoint = GetInsidePointToStepOn(_cells[cellIndexToMoveOn]);

            //Remove from cell
            ((Canvas)chipImage.Parent).Children.Remove(chipImage);

            //Add chip tom chipMove canvas
            AddChipToChipMovePanel(chipImage, chipToFieldPoint);

            //make move action
            MakeChipMove(startCellIndex, cellIndexToMoveOn, chipImage,
                insidePointStartCell, newInsideChipPoint);
            //MakeChipMoveToAnoutherCell(startCellIndex, cellIndexToMoveOn, chipImage, insidePointStartCell, newInsideChipPoint);



            //Reassign chip image to new cell (ON ANIMATION COMPLETE EVENT)
            //ReassignChipImageInNewCell(moveDist, tempChipImg, newInsideChipPoint);

            //Change Chips In cell
            SetNewPositionsToChipsInCell(startCellIndex);
        }

        private Point GetChipImageLocToField(Image img)
        {
            Point startChipPoint = img.PointToScreen(new Point(0, 0));
            Point fieldPoint = this.PointToScreen(new Point(0, 0));

            return new Point(startChipPoint.X - fieldPoint.X, startChipPoint.Y - fieldPoint.Y);
        }

        private bool _ifGoesThroughSquareCell = false;
        private List<int> _squareIndexesToGoThrough = null;
        private int _tempSquareValToGoOn;
        private EventHandler _chipAnimEvent;

        private bool IfLastMoveIsPrison = false; 

        public void MakeChipMove(int startCellIndex, int cellIndexToMoveOn, Image chipImg,
            Point insidePointStartCell, Point newInsideChipPoint)
        {
            //chip goes through square cell
            if (!(_squareIndexesToGoThrough is null))
            {
                _tempSquareValToGoOn = _squareIndexesToGoThrough.First();
                _ifGoesThroughSquareCell = true;

                if (_cells[_squareIndexesToGoThrough.Last()] is PrisonCell) IfLastMoveIsPrison = true;

                _chipAnimEvent = (s, e) =>
                {
                    ReassignChipImageInNewCell(_tempSquareValToGoOn, _imgs[0],
                        BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn]));

                    int toRemoveSquareIndex = _tempSquareValToGoOn;
                    _squareIndexesToGoThrough.Remove(_tempSquareValToGoOn);
                    if (_squareIndexesToGoThrough.Count > 0)
                    {
                        _tempSquareValToGoOn = _squareIndexesToGoThrough.First();

                        Point startChipPoint = _cells[toRemoveSquareIndex].PointToScreen(new Point(0, 0));
                        Point fieldPoint = this.PointToScreen(new Point(0, 0));

                        Point check = new Point(Canvas.GetLeft(chipImg), Canvas.GetTop(chipImg));
                        Point asd = new Point(startChipPoint.X - fieldPoint.X + check.X,
                            startChipPoint.Y - fieldPoint.Y + check.Y);

                        ((Canvas)chipImg.Parent).Children.Remove(chipImg);
                        AddChipToChipMovePanel(chipImg, asd);

                        MakeChipMoveToAnoutherCell(toRemoveSquareIndex, _tempSquareValToGoOn, chipImg,
                        check, BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn]));

                        if (IfLastMoveIsPrison && _squareIndexesToGoThrough.Count == 1)
                        {
                            IfLastMoveIsPrison = false;
                            MakeAMoveToInPrisonCell(true, chipImg);
                        }

                    }
                };
                //_toAddSecondaryAnimation.Completed += _chipAnim;
                MakeChipMoveToAnoutherCell(startCellIndex, _tempSquareValToGoOn, chipImg,
                    insidePointStartCell, newInsideChipPoint);

            }
            else
            {
                if (_cells[cellIndexToMoveOn] is PrisonCell) IfLastMoveIsPrison = true;

                //usualMove
                MakeChipMoveToAnoutherCell(startCellIndex, cellIndexToMoveOn, chipImg, insidePointStartCell, newInsideChipPoint);
            }
        }

        //To Make a move in prison cell(sitter or visiter)
        private void MakeAMoveToInPrisonCell(bool ifVisiter, Image movedChip)
        {
            if (ifVisiter)
            {
                PrisonCell.ChipsPlacer.Children.Remove(movedChip);
                PrisonCell.ChipsPlacerVisit.Children.Add(movedChip);

                List<Point> points = BoardHelper.GetPointsForPrisonCellExcurs(PrisonCell.ChipsPlacerVisit.Children.Count,
                    new Size(PrisonCell.Width, PrisonCell.Height));

                SetChipsImagesInMovePanel(points, PrisonCell.ChipsPlacerVisit, 10);
            }
        }


        private void SetNewPositionsToChipsInCell(int cellIndex)
        {
            Canvas can = BoardHelper.GetChipCanvas(_cells[cellIndex]);

            //new points for chips
            List<Point> newChipsPoints = BoardHelper.GetPointsForChips(
                new Size(can.ActualWidth, can.ActualHeight), can.Children.OfType<Image>().ToList().Count);

            if (newChipsPoints is null) return;

            //Reassign chips in move panel
            SetChipsImagesInMovePanel(newChipsPoints, can, cellIndex);
        }

        private void SetChipsImagesInMovePanel(List<Point> points, Canvas chipCanvas, int cellIndex)
        {
            List<Image> chips = chipCanvas.Children.OfType<Image>().ToList();
            List<Point> chipsStartPoints = new List<Point>();

            Point fieldGlobalPoint = this.PointToScreen(new Point(0, 0));
            Point cellChipCanPoint = chipCanvas.PointToScreen(new Point(0, 0));

            Point differ = new Point(cellChipCanPoint.X - fieldGlobalPoint.X, cellChipCanPoint.Y - fieldGlobalPoint.Y);

            foreach (Image img in chips)
            {
                chipsStartPoints.Add(new Point(Canvas.GetLeft(img), Canvas.GetTop(img)));

                chipCanvas.Children.Remove(img);
                Point chipPoint = new Point(Canvas.GetLeft(img) + differ.X, Canvas.GetTop(img) + differ.Y);

                Canvas.SetLeft(img, chipPoint.X);
                Canvas.SetTop(img, chipPoint.Y);

                ChipMovePanel.Children.Add(img);
            }
            for (int i = 0; i < chips.Count; i++)
            {
                if (chips[i] is Image img)
                {
                    Point chipPointDiffer = new Point(points[i].X - chipsStartPoints[i].X, points[i].Y - chipsStartPoints[i].Y);
                    SetChipToMoveAnimation(img, chipPointDiffer, cellIndex, points[i]);
                }
            }
        }

        private void ReassignChipImageInNewCell(int newCellIndex, Image chipImage, Point newCellInsideLoc)
        {
            ChipMovePanel.Children.Remove(chipImage);

            UIElement cellToMoveOn = _cells[newCellIndex];
            Canvas chipCan = BoardHelper.GetChipCanvas(cellToMoveOn);
            chipCan.Children.Add(chipImage);

            Canvas.SetLeft(chipImage, newCellInsideLoc.X);
            Canvas.SetTop(chipImage, newCellInsideLoc.Y);

            chipImage.RenderTransform = null;
        }


        private void MakeChipMoveToAnoutherCell(int startCellIndex, int movePoint, Image chip,
            Point prevCellInsideChipPoint, Point newCellInsideChipPoint)
        {
            //Get cell to move on
            //Check how many items in chipsCanvas
            //GetPoints for this Cell To stepOn
            //Get point for new Chip


            //cell to step on
            UIElement cellToMoveOn = _cells[movePoint];


            //Last point - for chips which we are moving
            Point newCellPoint = BoardHelper.GetChipCanvas(cellToMoveOn).PointToScreen(new Point(0, 0));
            Point startcCell = BoardHelper.GetChipCanvas(_cells[startCellIndex]).PointToScreen(new Point(0, 0));

            Point pointToStepOn = new Point(newCellPoint.X - startcCell.X + newCellInsideChipPoint.X - prevCellInsideChipPoint.X,
                newCellPoint.Y - startcCell.Y + newCellInsideChipPoint.Y - prevCellInsideChipPoint.Y);


            //Set chip amimation 
            SetChipToMoveAnimation(chip, pointToStepOn, movePoint, newCellInsideChipPoint);
        }

        private Point GetInsidePointToStepOn(UIElement cellToMoveOn)
        {
            //amount of chips in cell to step on
            int amountOfChipsInCell = BoardHelper.GetAmountOfItemsInCell(cellToMoveOn);

            //Size of cell to step on
            Size cellSize = BoardHelper.GetSizeOfCell(cellToMoveOn);

            //Get Points for chips to place on cel to step on (need to replace all chips)
            List<Point> chipPoints = BoardHelper.GetPointsForChips(cellSize, amountOfChipsInCell);

            return chipPoints.Last();
        }

        private bool _ifChipMoves = false;
        private DoubleAnimation _toAddSecondaryAnimation;

        private void SetChipToMoveAnimation(Image toMove, Point endPoint,
            int cellIndex, Point newInsideChipPoint)
        {
            var transform = new TranslateTransform();
            toMove.RenderTransform = transform;

            DoubleAnimation moveXAnimation = new DoubleAnimation
            {
                From = 0,
                To = endPoint.X,
                Duration = TimeSpan.FromSeconds(3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            if (_ifGoesThroughSquareCell && toMove.Name == "One") // tempMove Chip Img
            {
                moveXAnimation.Completed += _chipAnimEvent;
            }
            else
            {
                moveXAnimation.Completed += (s, e) =>
                {
                    ReassignChipImageInNewCell(cellIndex, toMove, newInsideChipPoint);

                    if (IfLastMoveIsPrison)
                    {
                        IfLastMoveIsPrison = false;
                        MakeAMoveToInPrisonCell(true, toMove);
                    }
                    
                    //if (_ifGoesThroughSquareCell) return;
                    _ifChipMoves = false;
                };
            }

            DoubleAnimation moveYAnimation = new DoubleAnimation
            {
                From = 0,
                To = endPoint.Y,
                Duration = TimeSpan.FromSeconds(3),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            transform.BeginAnimation(TranslateTransform.XProperty, moveXAnimation);
            transform.BeginAnimation(TranslateTransform.YProperty, moveYAnimation);

            _ifChipMoves = true;
        }


        private void AddChipToChipMovePanel(Image img, Point startChipPoint)
        {
            Canvas.SetLeft(img, startChipPoint.X);
            Canvas.SetTop(img, startChipPoint.Y);

            ChipMovePanel.Children.Add(img);
        }

        private List<Image> _imgs = new List<Image>();
        private void SetChipPositionsInStartSquare()
        {
            _imgs = BoardHelper.GetAllChipsImages();

            for (int i = 0; i < _imgs.Count; i++)
            {
                chipAndCord.Add((_imgs[i], 0));
            }

            List<List<Point>> points = BoardHelper.GetChipsPoints(
                new Size(StartCell.Width, StartCell.Height));

            List<Point> pointsForChips = points[_imgs.Count - 1];

            //pointsForChips = BoardHelper.GetPointsForPrisonCellSitter(5, new Size(PrisonCell.Width, PrisonCell.Height));

            for (int i = 0; i < _imgs.Count; i++)
            {
                Canvas.SetLeft(_imgs[i], pointsForChips[i].X);
                Canvas.SetTop(_imgs[i], pointsForChips[i].Y);

                StartCell.ChipsPlacer.Children.Add(_imgs[i]);
            }
        }


        //

        public void HideHeadersInCells()
        {
            ChanceOne.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceTwo.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceThree.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceFour.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceFive.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceSix.MoneyPlacer.Visibility = Visibility.Hidden;

            LittleTax.MoneyPlacer.Visibility = Visibility.Hidden;
            BigTax.MoneyPlacer.Visibility = Visibility.Hidden;
        }

        private const int _upperMargin = 10;
        //That cant be changed with box items
        private void SetImmoratlImages()
        {
            SetSquareCells();

            SetTaxesImages();
            SetChances();

            SetBasicBusinessesImages();
        }

        public void SetBasicBusinessesImages()
        {
            //Get (to make in future) parameters for image from table

            SetBasicPerfumeImages();
            SetBasicClothesImages();

            SetBasicMessagerImages();
            SetBasicDrinkingsImages();

            SetBasicPlanesImages();
            SetBasicFoodImages();

            SetBasicHotelImages();
            SetBasicPhoneImages();

            SetBasicCarImages();
            SetBasicGameImages();
        }

        private void SetBasicGameImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("rockstar_games.png", "Games"), GamesFirstBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("rovio.png", "Games"), GamesSecondBus, new Size(100, 25));
        }

        private void SetBasicCarImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("mercedes.png", "Cars"), UpCar, new Size(50, 50));
            SetRightCellImage(BoardHelper.GetImageFromFolder("audi.png", "Cars"), RightCar, new Size(100, 30));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("ford.png", "Cars"), DownCars, new Size(95, 40));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("land_rover.png", "Cars"), LeftCar, new Size(100, 45));

        }

        private void SetBasicPhoneImages()
        {
            SetLeftCellImage(BoardHelper.GetImageFromFolder("apple.png", "Phones"), PhonesFirstBus, new Size(40, 45));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("nokia.png", "Phones"), PhonesSecondBus, new Size(100, 20));
        }

        private void SetBasicHotelImages()
        {
            SetLeftCellImage(BoardHelper.GetImageFromFolder("holiday_inn.png", "Hotels"), HotelFirstBus, new Size(65, 45));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("radisson_blu.png", "Hotels"), HotelSecondBus, new Size(100, 35));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("novotel.png", "Hotels"), HotelThirdBus, new Size(100, 45));
        }

        private void SetLeftCellImage(Image img, LeftCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicFoodImages()
        {
            SetBottomCellImage(BoardHelper.GetImageFromFolder("max_burgers.png", "Food"), FoodFirstBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("burger_king.png", "Food"), FoodSecondBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("kfc.png", "Food"), FoodThirdBus, new Size(55, 45));
        }

        private void SetBasicPlanesImages()
        {
            SetBottomCellImage(BoardHelper.GetImageFromFolder("lufthansa.png", "Planes"), PlanesFirstBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("british_airways.png", "Planes"), PlanesSecondBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("american_airlines.png", "Planes"), PlanesThirdBus, new Size(100, 20));
        }

        private void SetBottomCellImage(Image img, BottomCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicDrinkingsImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("coca_cola.png", "Drinkings"), DrinkFirstBus, new Size(100, 35));
            SetRightCellImage(BoardHelper.GetImageFromFolder("fanta.png", "Drinkings"), DrinkSecondBus, new Size(65, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("pepsi.png", "Drinkings"), DrinkThirdBus, new Size(100, 35));
        }

        private void SetBasicMessagerImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("vk.png", "Messagers"), MessagerFirstBus, new Size(75, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("facebook.png", "Messagers"), MessagerSecondBus, new Size(100, 30));
            SetRightCellImage(BoardHelper.GetImageFromFolder("twitter.png", "Messagers"), MessagerThirdBus, new Size(55, 45));
        }

        private void SetRightCellImage(Image img, RightCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetRightLeftImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Fill;
            img.Width = imageSize.Width;
            img.Height = imageSize.Height;
        }



        private void SetBasicClothesImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("adidas.png", "Clothes"), ClothesFirstBus, new Size(75, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("puma.png", "Clothes"), ClothesSeconBus, new Size(100, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("lacoste.png", "Clothes"), ClothesThirdBus, new Size(100, 45));
        }

        public void SetBasicPerfumeImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("chanel.png", "Perfume"), PerfumeFirstBus, new Size(75, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("hugo_boss.png", "Perfume"), PerfumeSecondBus, new Size(100, 40));
        }

        private void SetUpperCellImage(Image img, UpperCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Fill;

            img.Width = imageSize.Width;
            img.Height = imageSize.Height;

            SetRenderToTurnImageForUpDownCell(img);
        }

        private void SetRenderToTurnImageForUpDownCell(Image img)
        {
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform transform = new RotateTransform()
            {
                Angle = -90,
            };

            img.LayoutTransform = transform;
        }

        private Size _chanseSize = new Size(55, 80);
        private void SetChances()
        {
            SetUpDownChances();
            SetRightChances();
            SetLeftChances();
        }

        public void SetLeftChances()
        {
            ChanceFive.ImagePlacer.Children.Add(GetLeftChances());
            ChanceSix.ImagePlacer.Children.Add(GetLeftChances());
        }

        private Image GetLeftChances()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chance_rotated.png");

            img.Width = _chanseSize.Height;
            img.Height = ChanceFive.ImagePlacer.Height;

            return img;
        }

        private Image GetRightChanceIamge()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chanceUp.png");

            img.Width = this.Width;
            img.Height = ChanceFive.ImagePlacer.Height;

            return img;
        }

        private void SetRightChances()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chance.png");

            img.Width = _chanseSize.Height;
            img.Height = ChanceThree.ImagePlacer.Height;

            ChanceThree.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownChances()
        {
            ChanceOne.ImagePlacer.Children.Add(GetUpChanceImage());
            ChanceTwo.ImagePlacer.Children.Add(GetUpChanceImage());
            ChanceFour.ImagePlacer.Children.Add(GetUpChanceImage());

        }

        private Image GetUpChanceImage()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chanceUp.png");

            img.Width = _chanseSize.Width;
            img.Height = ChanceOne.ImagePlacer.Height;

            return img;
        }

        private void SetTaxesImages()
        {
            //Set little Tax
            Image little = BoardHelper.GetTaxImageByName("tax_income.png");
            little.Width = LittleTax.Width - _upperMargin;
            little.Height = LittleTax.Height / 2;

            little.HorizontalAlignment = HorizontalAlignment.Center;
            little.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(little, 10);

            LittleTax.ImagePlacer.Children.Add(little);

            //Set big tax

            Image big = BoardHelper.GetTaxImageByName("tax_luxury.png");
            big.Width = BigTax.Width / 2;
            big.Height = big.Height - 10;

            big.HorizontalAlignment = HorizontalAlignment.Center;
            big.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(big, 10);

            BigTax.ImagePlacer.Children.Add(big);
        }

        public void SetSquareCells()
        {
            SetStartImage();
            SetJackPotImage();
            SetGoToPrisonImage();
            SetPrisonImage();
        }

        private void SetPrisonImage()
        {
            const int width = 50;
            const int height = 50;
            Image jail = new Image()
            {
                Source = BoardHelper.GetSquareByName("jail.png").Source,
                Width = width + 10,
                Height = height + 10
            };

            Canvas.SetLeft(jail, 0);
            Canvas.SetTop(jail, 65);

            PrisonCell.ImagesPlacer.Children.Add(jail);


            Image donat = new Image()
            {
                Source = BoardHelper.GetSquareByName("jail-visiting.png").Source,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(donat, 65);
            Canvas.SetTop(donat, 10);

            PrisonCell.ImagesPlacer.Children.Add(donat);
        }

        private void SetGoToPrisonImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("goToJail.png").Source,
                Width = 125,
                Height = 125
            };

            GoToPrisonCell.ImagesPlacer.Children.Add(img);
        }

        private void SetJackPotImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("jackpot.png").Source,
                Width = 125,
                Height = 125
            };

            JackPotCell.ImagesPlacer.Children.Add(img);
        }

        private void SetStartImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("start.png").Source,
                Width = 125,
                Height = 125
            };

            StartCell.ImagesPlacer.Children.Add(img);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter) &&
                MessageWriter.Text != string.Empty)
            {
                AddMessageTextBlock(MessageWriter.Text);
                MessageWriter.Text = string.Empty;
            }
        }

        private void AddMessageTextBlock(string message)
        {
            TextBlock block = new TextBlock()
            {
                Text = message,
                FontSize = 16,
                Foreground = Brushes.White
            };

            GameChat.Items.Add(block);
        }

        private List<UIElement> _cells = new List<UIElement>();
        private void SetCellsInList()
        {
            _cells.Add(StartCell);

            for (int i = 0; i < FirstLinePanel.Children.Count; i++)
            {
                if (FirstLinePanel.Children[i] is UpperCell)
                {
                    _cells.Add(FirstLinePanel.Children[i]);
                }
            }
            _cells.Add(PrisonCell);

            for (int i = 0; i < SecondLinePanel.Children.Count; i++)
            {
                if (SecondLinePanel.Children[i] is RightCell)
                {
                    _cells.Add(SecondLinePanel.Children[i]);
                }
            }

            _cells.Add(JackPotCell);

            for (int i = ThirdLinePanel.Children.Count - 1; i >= 0; i--)
            {
                if (ThirdLinePanel.Children[i] is BottomCell)
                {
                    _cells.Add(ThirdLinePanel.Children[i]);
                }
            }

            _cells.Add(GoToPrisonCell);

            for (int i = FourthLinePanel.Children.Count - 1; i >= 0; i--)
            {
                if (FourthLinePanel.Children[i] is LeftCell)
                {
                    _cells.Add(FourthLinePanel.Children[i]);
                }
            }
        }

    }
}
