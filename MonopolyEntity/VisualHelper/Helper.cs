using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;
using MonopolyDLL.Monopoly.Enums;
using System.Windows.Media.Imaging;
using System.IO;
using MonopolyDLL.Monopoly.InventoryObjs;
using MonopolyDLL;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using MonopolyEntity.Windows.UserControls;
using MonopolyEntity.Interfaces;

namespace MonopolyEntity.VisualHelper
{
    public static class Helper
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);

            while (parent != null)
            {
                if (parent is T parentAsT)
                {
                    return parentAsT;
                }

                parent = VisualTreeHelper.GetParent(parent);
            }

            return null;
        }

        public static Point GetElementLocation(UIElement element, WrapPanel wrapPanel)
        {
            if (!wrapPanel.Children.Contains(element))
                throw new ArgumentException("cant find the elems location in WrapPanel.");

            return GetTransformedPoint(element, wrapPanel);
/*

            GeneralTransform transform = element.TransformToAncestor(wrapPanel);
            Point location = transform.Transform(new Point(0, 0));

            return location; */
        }

        public static Point GetElementLocationRelativeToPage(UIElement element, Page page)
        {
            if (element == null || page == null)
                throw new ArgumentNullException("Smth wrong with given page");
            return GetTransformedPoint(element, page);
/*
            GeneralTransform transform = element.TransformToAncestor(page);
            Point location = transform.Transform(new Point(0, 0));

            return location; */
        }

        private static  Point GetTransformedPoint(UIElement element, Visual page)
        {
            GeneralTransform transform = element.TransformToAncestor(page);
            return transform.Transform(new Point(0, 0));

        }

        public static List<SolidColorBrush> GetColorsQueue()
        {
            List<SolidColorBrush> res = new List<SolidColorBrush>();
            res.Add((SolidColorBrush)Application.Current.Resources["FirstUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["SecondUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["ThirdUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["FourthUserColor"]);
            res.Add((SolidColorBrush)Application.Current.Resources["FifthUserColor"]);

            return res;
        }

        private static string GetAddedImagePath()
        {
            return Path.Combine(GetImagesPath(), "AddItemsImages");
        }

        public static Image GetLotBoxImage(string imageName)
        {
            string boxPath = Path.Combine(GetAddedImagePath(), "Box");
            string imgPath = Path.Combine(boxPath, imageName);

            return new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute)),
                Stretch = Stretch.Uniform
            };
        }

        public static Image GetSquareByName(string imageName)
        {
            string squarePath = Path.Combine(Path.Combine(GetBoardImagesPath(), "CardImages"), "Squares");
            string imgSquarePath = Path.Combine(squarePath, imageName);

            return new Image()
            {
                Source = new BitmapImage(new Uri(imgSquarePath, UriKind.Absolute))
            };
        }

        public static Image GetTaxImageByName(string imageName)
        {
            string cardImages = Path.Combine(GetBoardImagesPath(), "CardImages");
            string taxPath = Path.Combine(cardImages, "Tax");

            string imgPath = Path.Combine(taxPath, imageName);

            return new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };
        }

        public static Image GetAddedItemImage(string imageName, BusinessType type)
        {
            string busPath = Path.Combine(GetAddedImagePath(), "Businesses");
            string busTypePath = GetFieldPathCombineByType(busPath, type);

            return new Image()
            {
                Source = new BitmapImage(new Uri(Path.Combine(busTypePath, imageName), UriKind.Absolute)),
                Stretch = Stretch.Uniform
            };
        }

        private static string GetBoardImagesPath()
        {
            return Path.Combine(Helper.GetImagesPath(), "BoardImages");
        }

        public static Image GetImageFromOtherFolder(string name)
        {
            string otherImages = Path.Combine(GetBoardImagesPath(), "Other");
            string imgPath = Path.Combine(otherImages, name);

            return new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };
        }

        private static Image GetImageByPath(string imgPath)
        {
            return new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };
        }

        private static string GetCardImagesFolder()
        {
            string boardImagesPath = GetBoardImagesPath();
            return Path.Combine(boardImagesPath, "CardImages");
        }


        public static Image GetImageFromFolder(string name, string folderName)
        {
            string perfumePath = Path.Combine(GetCardImagesFolder(), folderName);
            string imgPath = Path.Combine(perfumePath, name);

            return GetImageByPath(imgPath);
        }

        public static string GetDiceFolderPath()
        {
            return Path.Combine(GetBoardImagesPath(), "DiceRibs");
        }

        private const int _chipSize = 25;
        public static Image GetChipImageByName(string name)
        {
            string boardPath = GetBoardImagesPath();
            string chipsPath = Path.Combine(boardPath, "Chips");

            string imagePath = Path.Combine(chipsPath, name);

            Image res = GetImageByPath(imagePath);
            res.Width = _chipSize;
            res.Height = _chipSize;

            return res;
        }

        private static readonly string _firstChipName = "One";
        private static readonly string _secondChipName = "Two";
        private static readonly string _thirdChipName = "Three";
        private static readonly string _fourthChipName = "Four";
        private static readonly string _fifthChipName = "Five";

        public static List<Image> GetAllChipsImages(int amountOfPlayers)
        {
            const int secondObjIndex = 1;
            const int thirdObjIndex = 2;
            const int fourthObjIndex = 3;

            List<Image> chips = GetListOfChipImages();

            chips.First().Name = _firstChipName;
            chips[secondObjIndex].Name = _secondChipName;
            chips[thirdObjIndex].Name = _thirdChipName;
            chips[fourthObjIndex].Name = _fourthChipName;
            chips.Last().Name = _fifthChipName;

            List<Image> res = new List<Image>();
            for (int i = 0; i < amountOfPlayers; i++)
            {
                res.Add(chips[i]);
            }
            return res;
        }

        private static List<Image> GetListOfChipImages()
        {
            return new List<Image>()
            {
                GetChipImageByName("chipRed.png"),
                GetChipImageByName("chipBlue.png"),
                GetChipImageByName("chipGreen.png"),
                GetChipImageByName("chipPurple.png"),
                GetChipImageByName("chipOrange.png")
            };
        }

        public static List<List<Point>> GetChipsPoints(Size squareSize)
        {
            List<List<Point>> res = new List<List<Point>>();

            const int distanceFromBorder = 5;
            const int centerDivider = 2;

            double chipRadius = _chipSize / 2;

            Point center = new Point(squareSize.Width / centerDivider - chipRadius,
                squareSize.Height / centerDivider - chipRadius);

            Point leftUp = new Point(distanceFromBorder, distanceFromBorder);
            Point rightUp = new Point(squareSize.Width - _chipSize - distanceFromBorder, leftUp.Y);

            Point leftDown = new Point(leftUp.X, squareSize.Height - leftUp.Y - _chipSize);
            Point rightDown = new Point(rightUp.X, squareSize.Height - rightUp.Y - _chipSize);

            Point upCenter = new Point(center.X, leftUp.Y);
            Point downCenter = new Point(center.X, leftDown.Y);

            return GetListOfPointToStand(center, leftUp, rightUp, leftDown, rightDown, upCenter, downCenter, false);
            /*            for (int i = 1; i <= _maxPlayers; i++)
                        {
                            List<Point> temp = new List<Point>();
                            switch (i)
                            {
                                case 1:
                                    {
                                        temp.Add(center);
                                        break;
                                    }
                                case 2:
                                    {
                                        temp.Add(upCenter);
                                        temp.Add(downCenter);
                                        break;
                                    }
                                case 3:
                                    {
                                        temp.Add(upCenter);
                                        temp.Add(leftDown);
                                        temp.Add(rightDown);
                                        break;
                                    }
                                case 4:
                                    {
                                        temp.Add(leftUp);
                                        temp.Add(rightUp);
                                        temp.Add(leftDown);
                                        temp.Add(rightDown);
                                        break;
                                    }
                                case 5:
                                    {
                                        temp.Add(leftUp);
                                        temp.Add(rightUp);
                                        temp.Add(leftDown);
                                        temp.Add(rightDown);
                                        temp.Add(center);

                                        break;
                                    }
                            }

                            res.Add(temp);
                        }

                        return res;*/
        }

        private static readonly int _maxPlayers = SystemParamsService.GetNumByName("MaxAmountOfPlayers");



        public static List<Point> GetPointsForChips(Size cellSize, int amountOfChipsInCell, bool isInMove)
        {
            const int toGetPrevPoint = -1;
            List<List<Point>> allPoints = GetChipsPoints(cellSize);

            if (amountOfChipsInCell == 0) return allPoints.First();
            int ifChipIsInMove = isInMove ? 0 : toGetPrevPoint;

            return allPoints[amountOfChipsInCell + ifChipIsInMove];
        }

        public static Size GetSizeOfCell(UIElement cell)
        {
            return cell is IAllCellActions inter ? inter.GetActualCellSize() : throw new Exception("No such type of cell");

/*            return cell is UpperCell up ? new Size(up.ChipsPlacer.ActualWidth, up.ChipsPlacer.ActualHeight) :
                cell is RightCell right ? new Size(right.ChipsPlacer.ActualWidth, right.ChipsPlacer.ActualHeight) :
                cell is BottomCell bot ? new Size(bot.ChipsPlacer.ActualWidth, bot.ChipsPlacer.ActualHeight) :
                cell is LeftCell left ? new Size(left.ChipsPlacer.ActualWidth, left.ChipsPlacer.ActualHeight) :
                cell is SquareCell square ? new Size(square.ChipsPlacer.ActualWidth, square.ChipsPlacer.ActualHeight) :
                cell is PrisonCell prison ? new Size(prison.ChipsPlacerVisit.ActualWidth, prison.ChipsPlacerVisit.ActualHeight) :
                throw new Exception("No such type of cell");*/

            /*            if (cell is UpperCell up)
                        {
                            return new Size(up.ChipsPlacer.ActualWidth, up.ChipsPlacer.ActualHeight);
                        }
                        else if (cell is RightCell right)
                        {
                            return new Size(right.ChipsPlacer.ActualWidth, right.ChipsPlacer.ActualHeight);
                        }
                        else if (cell is BottomCell bot)
                        {
                            return new Size(bot.ChipsPlacer.ActualWidth, bot.ChipsPlacer.ActualHeight);
                        }
                        else if (cell is LeftCell left)
                        {
                            return new Size(left.ChipsPlacer.ActualWidth, left.ChipsPlacer.ActualHeight);
                        }
                        else if (cell is SquareCell square)
                        {
                            return new Size(square.ChipsPlacer.ActualWidth, square.ChipsPlacer.ActualHeight);
                        }
                        else if (cell is PrisonCell prison)
                        {
                            return new Size(prison.ChipsPlacerVisit.ActualWidth, prison.ChipsPlacerVisit.ActualHeight);
                        }
                        throw new Exception("No such type of cell");*/
        }

        public static int GetAmountOfItemsInCell(UIElement cell)
        {
            return cell is IAllCellActions inter ? inter.GetAmountOfItemsInCell() : 0;

/*
            return cell is UpperCell up ? up.ChipsPlacer.Children.Count :
                cell is RightCell right ? right.ChipsPlacer.Children.Count :
                cell is BottomCell bot ? bot.ChipsPlacer.Children.Count :
                cell is LeftCell left ? left.ChipsPlacer.Children.Count :
                cell is SquareCell square ? square.ChipsPlacer.Children.Count : 0;*/

            /*            if (cell is UpperCell up)
                        {
                            return up.ChipsPlacer.Children.Count;
                        }
                        else if (cell is RightCell right)
                        {
                            return right.ChipsPlacer.Children.Count;
                        }
                        else if (cell is BottomCell bot)
                        {
                            return bot.ChipsPlacer.Children.Count;
                        }
                        else if (cell is LeftCell left)
                        {
                            return left.ChipsPlacer.Children.Count;
                        }
                        else if (cell is SquareCell square)
                        {
                            return square.ChipsPlacer.Children.Count;
                        }
                        return 0;*/
        }

        public static List<List<Point>> GetPointsToStandForPrisonCell(Size squareSize)
        {
            //Points for up Right (Donat), if for prison, need to change x and y in cords

            List<List<Point>> res = new List<List<Point>>();
            const int distanceFromBorder = 5;
            const int distanceBetweenChips = 5;

            Point rightCorner = new Point(squareSize.Width - distanceFromBorder - _chipSize, distanceFromBorder);

            Point cornerRightOne = new Point(rightCorner.X - distanceBetweenChips - _chipSize, distanceFromBorder);
            Point cornerRightTwo = new Point(cornerRightOne.X - distanceBetweenChips - _chipSize, distanceFromBorder);

            Point cornerDownOne = new Point(rightCorner.X, rightCorner.Y + distanceBetweenChips + _chipSize);
            Point cornerDownTwo = new Point(rightCorner.X, cornerDownOne.Y + distanceBetweenChips + _chipSize);

            return GetListOfPointToStand(rightCorner, cornerRightOne, cornerRightTwo, cornerDownOne, cornerDownTwo, null, null, true);
            /*
                        for (int i = 1; i <= _maxPlayers; i++)
                        {
                            List<Point> temp = new List<Point>();
                            switch (i)
                            {
                                case 1:
                                    {
                                        temp.Add(rightCorner);
                                        break;
                                    }
                                case 2:
                                    {
                                        temp.Add(cornerRightOne);
                                        temp.Add(cornerDownOne);
                                        break;
                                    }
                                case 3:
                                    {
                                        temp.Add(rightCorner);
                                        temp.Add(cornerRightOne);
                                        temp.Add(cornerDownOne);
                                        break;
                                    }
                                case 4:
                                    {
                                        temp.Add(cornerRightOne);
                                        temp.Add(cornerDownOne);
                                        temp.Add(cornerRightTwo);
                                        temp.Add(cornerDownTwo);
                                        break;
                                    }
                                case 5:
                                    {
                                        temp.Add(rightCorner);
                                        temp.Add(cornerRightOne);
                                        temp.Add(cornerDownOne);
                                        temp.Add(cornerRightTwo);
                                        temp.Add(cornerDownTwo);
                                        break;
                                    }
                            }
                        }

                        return res;*/
        }



        private static List<Point> GetPointForOneChipInCell(params Point[] points)
        {
            return points.ToList();
        }

        private static List<List<Point>> GetListOfPointToStand(Point first, Point second, Point third,
            Point fourth, Point fifth, Point? sixth, Point? seventh, bool ifPrison)
        {
            const int onePlayer = 1;
            const int twoPlayers = 2;
            const int threePlayers = 3;
            const int fourPlayers = 4;
            const int fivePlayers = 5;

            List<List<Point>> res = new List<List<Point>>();

            for (int i = 1; i <= _maxPlayers; i++)
            {
                List<Point> temp = new List<Point>();
                switch (i)
                {
                    case onePlayer:
                        {
                            if (ifPrison) temp.AddRange(GetPointForOneChipInCell(first));
                            else temp.AddRange(GetPointForOneChipInCell(first));
                            break;
                        }
                    case twoPlayers:
                        {
                            if (ifPrison) temp.AddRange(GetPointForOneChipInCell(second, fourth));
                            else temp.AddRange(GetPointForOneChipInCell((Point)sixth, (Point)seventh));
                            break;
                        }
                    case threePlayers:
                        {
                            if (ifPrison) temp.AddRange(GetPointForOneChipInCell(first, second, fourth));
                            else temp.AddRange(GetPointForOneChipInCell(fourth, fifth, (Point)sixth));
                            break;
                        }
                    case fourPlayers:
                        {
                            temp.AddRange(GetPointForOneChipInCell(second, third, fourth, fifth));

                            /*                            if (ifPrison) temp.AddRange(GetPointForOneChipInCell(second, third, fourth, fifth));
                                                        else temp.AddRange(GetPointForOneChipInCell(second, third, fourth, fifth));*/
                            break;
                        }
                    case fivePlayers:
                        {
                            temp.AddRange(GetPointForOneChipInCell(first, second, third, fourth, fifth));

                            /*                            if (ifPrison) temp.AddRange(GetPointForOneChipInCell(first, second, third, fourth, fifth));
                                                        else temp.AddRange(GetPointForOneChipInCell(first, second, third, fourth, fifth));*/
                            break;
                        }
                }

                res.Add(temp);
            }
            return res;
        }


        public static List<Point> GetPointsForPrisonCellExcursion(int amountOfChips, Size cellSize)
        {
            List<List<Point>> points = GetPointsToStandForPrisonCell(cellSize);
            return amountOfChips == 0 ? points.First() : points[amountOfChips - 1];

            /*            if (amountOfChips == 0) return points.First();
                        return points[amountOfChips - 1];*/
        }

        public static List<Point> GetPointsForPrisonCellSitter(int amountOfChips, Size cellSize)
        {
            List<List<Point>> points = GetPointsToStandForPrisonCell(cellSize);
            return amountOfChips == 0 ? SwapPointsForPrisonCell(points.First()) :
                SwapPointsForPrisonCell(points[amountOfChips - 1]);

            /*            if (amountOfChips == 0) return SwapPointsForPrisonCell(points.First());
                        return SwapPointsForPrisonCell(points[amountOfChips - 1]);*/
        }

        private static List<Point> SwapPointsForPrisonCell(List<Point> points)
        {
            List<Point> res = new List<Point>();
            for (int i = 0; i < points.Count; i++)
            {
                res.Add(new Point(points[i].Y, points[i].X));
            }
            return res;
        }

        public static List<int> GetListOfSquareCellIndexesThatChipGoesThrough(int startIndex, int endIndex)
        {
            bool isGoesTrough = IsChipGoesThroughCorner(startIndex, endIndex);
            return !isGoesTrough ? null : GetCellIndexToGoThrough(startIndex, endIndex);
        }

        public static List<int> GetListOfSquaresIndexesToGoThroughBackwards(int startIndex, int endIndex)
        {
            bool isGoesTrough = IsChipGoesThroughCorner(startIndex, endIndex);
            return !isGoesTrough ? null : GetCellIndexToGoThroughBackwards(startIndex, endIndex);
        }

        private static int GetAmountOfStepsFromEndToStartIndexesBackwards(int startIndex, int endIndex)
        {
            int res = 0;
            int counter = startIndex;
            int lastCellIndex = SystemParamsService.GetNumByName("AmountOfCells") - 1;// 39;
            do
            {
                counter--;
                if (counter < 0)
                {
                    counter = lastCellIndex;
                }
                res++;

                if (counter == endIndex)
                {
                    return res;
                }
            } while (true);
        }

        public static List<int> GetCellIndexToGoThroughBackwards(int startIndex, int endIndex)
        {
            int cubesVal = GetAmountOfStepsFromEndToStartIndexesBackwards(startIndex, endIndex);
            List<int> res = new List<int>();
            const int squareDivider = 10;
            const int firstCell = 0;
            do
            {
                startIndex--;
                cubesVal--;

                if (startIndex < firstCell) startIndex = SystemParamsService.GetNumByName("AmountOfCells") - 1; //39;
                if (startIndex % squareDivider == 0) res.Add(startIndex);
                if (cubesVal == 0)
                {
                    if (endIndex % squareDivider != 0) res.Add(endIndex);
                    return res;
                }
            } while (true);
        }

        public static bool IsChipGoesThroughCorner(int startIndex, int endPointIndex)
        {
            int firstLineLasCellIndex = SystemParamsService.GetNumByName("FirstLineLastPoint");
            int secondLineLasCellIndex = SystemParamsService.GetNumByName("SecondLineLastPoint");
            int thirdLineLasCellIndex = SystemParamsService.GetNumByName("ThirdLineLastPoint");
            int fourthLineLasCellIndex = SystemParamsService.GetNumByName("FourthLineLastPoint");

            return ((startIndex < firstLineLasCellIndex && endPointIndex > firstLineLasCellIndex) ||
                    (startIndex < secondLineLasCellIndex && endPointIndex > secondLineLasCellIndex) ||
                    (startIndex < thirdLineLasCellIndex && endPointIndex > thirdLineLasCellIndex) ||
                    (startIndex > thirdLineLasCellIndex && endPointIndex > fourthLineLasCellIndex) ||
                    startIndex > endPointIndex);
        }

        public static List<int> GetCellIndexToGoThrough(int startIndex, int endIndex)
        {
            int cubesVal = GetAmountOfStepsFromEndToStartIndexes(startIndex, endIndex);
            List<int> res = new List<int>();
            const int squareDivider = 10;
            int lastCell = SystemParamsService.GetNumByName("AmountOfCells");
            do
            {
                startIndex++;
                cubesVal--;

                if (startIndex == lastCell) startIndex = 0;
                if (startIndex % squareDivider == 0) res.Add(startIndex);
                if (cubesVal == 0)
                {
                    if (endIndex % squareDivider != 0) res.Add(endIndex);
                    return res;
                }
            } while (true);
        }

        private static int GetAmountOfStepsFromEndToStartIndexes(int startIndex, int endIndex)
        {
            int res = 0;
            int counter = endIndex;
            int lastCellIndex = SystemParamsService.GetNumByName("AmountOfCells") - 1; //39
            do
            {
                counter--;
                if (counter < 0)
                {
                    counter = lastCellIndex;
                }
                res++;

                if (counter == startIndex)
                {
                    return res;
                }
            } while (true);
        }

        private const int _centerDivider = 2;
        public static Point GetCenterOfTheSquareCellForImage(SquareCell cell, Image img)
        {
            return new Point(cell.ActualWidth / _centerDivider - img.Width / _centerDivider,
                cell.ActualHeight / _centerDivider - img.Height / _centerDivider);
        }

        public static Point GetCenterOfTheSquareForImage(Image img, UIElement cell)
        {
            return cell is IAllCellActions inter ? inter.GetCenterOfTheSquareForImage(img, _centerDivider) : new Point(0, 0);

/*            return cell is SquareCell square ? new Point(square.ActualWidth / _centerDivider - img.Width / _centerDivider,
                    square.ActualHeight / _centerDivider - img.Height / _centerDivider) :
                    cell is PrisonCell prison ? new Point(prison.ActualWidth / _centerDivider - img.Width / _centerDivider,
                    prison.ActualHeight / _centerDivider - img.Height / _centerDivider) :
                    cell is UpperCell upper ? new Point(upper.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                    upper.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider) :
                    cell is RightCell right ? new Point(right.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                    right.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider) :
                    cell is BottomCell bottom ? new Point(bottom.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                    bottom.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider) :
                    cell is LeftCell left ? new Point(left.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                    left.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider) : new Point(0, 0);*/

            /*            if (cell is SquareCell square)
                        {
                            return new Point(square.ActualWidth / _centerDivider - img.Width / _centerDivider,
                                square.ActualHeight / _centerDivider - img.Height / _centerDivider);
                        }
                        if (cell is PrisonCell prison)
                        {
                            return new Point(prison.ActualWidth / _centerDivider - img.Width / _centerDivider,
                                prison.ActualHeight / _centerDivider - img.Height / _centerDivider);
                        }
                        if (cell is UpperCell upper)
                        {
                            return new Point(upper.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                                upper.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider);
                        }
                        if (cell is RightCell right)
                        {
                            return new Point(right.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                                right.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider);
                        }
                        if (cell is BottomCell bottom)
                        {
                            return new Point(bottom.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                                bottom.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider);
                        }
                        if (cell is LeftCell left)
                        {
                            return new Point(left.ChipsPlacer.ActualWidth / _centerDivider - img.Width / _centerDivider,
                                left.ChipsPlacer.ActualHeight / _centerDivider - img.Height / _centerDivider);
                        }
                        return new Point(0, 0);*/
        }

        private static List<Point> GetChipsPointsOnPrisonCell(PrisonCell cell)
        {
            List<Point> res = new List<Point>();
            List<Image> img = cell.ChipsPlacerVisit.Children.OfType<Image>().ToList();

            for (int i = 0; i < img.Count; i++)
            {
                res.Add(new Point(Canvas.GetLeft(img[i]), Canvas.GetTop(img[i])));
            }
            return res;
        }

        private static string GetFieldPathCombineByType(string boxFolderPath, BusinessType type)
        {
            return type == BusinessType.Perfume ? Path.Combine(boxFolderPath, "Perfume") :
                type == BusinessType.Cars ? Path.Combine(boxFolderPath, "Car") :
                type == BusinessType.Clothes ? Path.Combine(boxFolderPath, "Clothes") :
                type == BusinessType.Games ? Path.Combine(boxFolderPath, "Games") :
                type == BusinessType.Messengers ? Path.Combine(boxFolderPath, "Messenger") :
                type == BusinessType.Drinks ? Path.Combine(boxFolderPath, "Drinks") :
                type == BusinessType.Planes ? Path.Combine(boxFolderPath, "Planes") :
                type == BusinessType.Food ? Path.Combine(boxFolderPath, "Food") :
                type == BusinessType.Hotels ? Path.Combine(boxFolderPath, "Hotels") :
                type == BusinessType.Phones ? Path.Combine(boxFolderPath, "Phones") :
                throw new Exception("No such type");

            /*            switch (type)
                        {
                            case BusinessType.Perfume:
                                return Path.Combine(boxFolderPath, "Perfume");
                            case BusinessType.Cars:
                                return Path.Combine(boxFolderPath, "Car");
                            case BusinessType.Clothes:
                                return Path.Combine(boxFolderPath, "Clothes");
                            case BusinessType.Games:
                                return Path.Combine(boxFolderPath, "Games");
                            case BusinessType.Messengers:
                                return Path.Combine(boxFolderPath, "Messager");
                            case BusinessType.Drinks:
                                return Path.Combine(boxFolderPath, "Drinks");
                            case BusinessType.Planes:
                                return Path.Combine(boxFolderPath, "Planes");
                            case BusinessType.Food:
                                return Path.Combine(boxFolderPath, "Food");
                            case BusinessType.Hotels:
                                return Path.Combine(boxFolderPath, "Hotels");
                            case BusinessType.Phones:
                                return Path.Combine(boxFolderPath, "Phones");
                        }
                        throw new Exception("No such type");*/
        }

        private static SolidColorBrush _basicRarityColor = (SolidColorBrush)Application.Current.Resources["BasicRarity"];
        //private static readonly Color _basicRarityColor = Color.FromRgb(76, 180, 219);
        public static SolidColorBrush GetRarityColorForCard(Item item)
        {
            if (item is BoxItem boxItem)
            {
                return new SolidColorBrush(Color.FromRgb(
                    boxItem.GetRParam(), boxItem.GetGParam(), boxItem.GetBParam()));
            }
            return _basicRarityColor;
        }

        public static List<CaseCard> SetCardsInRightPosition(List<CaseCard> cards)
        {
            List<CaseCard> res = new List<CaseCard>();

            List<SolidColorBrush> colors = GetAllColorsFromDB();
            for (int i = 0; i < colors.Count; i++)
            {
                for (int j = 0; j < cards.Count; j++)
                {
                    if (SolidColorBrushesComparation(colors[i],
                        (SolidColorBrush)cards[j].BorderBgColor.Background))
                    {
                        res.Add(cards[j]);
                    }
                }
            }
            return res;
        }

        private static bool SolidColorBrushesComparation(SolidColorBrush first, SolidColorBrush second)
        {
            return first.Color.R == second.Color.R &&
                first.Color.G == second.Color.G &&
                first.Color.B == second.Color.B;
        }

        private static List<SolidColorBrush> GetAllColorsFromDB()
        {
            List<SolidColorBrush> colors = new List<SolidColorBrush>();
            List<(byte r, byte g, byte b)> colorParams = DBQueries.GetAllRarityColors();

            for (int i = 0; i < colorParams.Count; i++)
            {
                colors.Add(new SolidColorBrush(Color.FromRgb(colorParams[i].r, colorParams[i].g, colorParams[i].b)));
            }
            return colors;
        }

        public static SolidColorBrush GetColorFromSystemColorName(string name)
        {
            (byte r, byte g, byte b) colorParams = DBQueries.GetColorByRarityName(name);
            return new SolidColorBrush(Color.FromRgb(colorParams.r, colorParams.g, colorParams.b));
        }

        public static void ChangeDepositCounterVisibility(UIElement cell, Visibility visibility)
        {
            if (cell is IRegularCellsActions inter) inter.ChangeDepositCounterVisibility(visibility);

/*            if (cell is UpperCell up) up.DepositObj.Visibility = visibility;
            if (cell is RightCell right) right.DepositObj.Visibility = visibility;
            if (cell is BottomCell bottom) bottom.DepositObj.Visibility = visibility;
            if (cell is LeftCell left) left.DepositObj.Visibility = visibility;*/
        }

        /*        public static void MakeDepositCounterVisible(UIElement cell)
                {
                    if (cell is UpperCell up) up.DepositObj.Visibility = Visibility.Visible; 
                    if (cell is RightCell right) right.DepositObj.Visibility = Visibility.Visible; 
                    if (cell is BottomCell bottom) bottom.DepositObj.Visibility = Visibility.Visible; 
                    if (cell is LeftCell left) left.DepositObj.Visibility = Visibility.Visible; 
                }

                public static void MakeDepositCounterHidden(UIElement cell)
                {
                    if (cell is UpperCell up) up.DepositObj.Visibility = Visibility.Hidden;
                    if (cell is RightCell right) right.DepositObj.Visibility = Visibility.Hidden;
                    if (cell is BottomCell bottom) bottom.DepositObj.Visibility = Visibility.Hidden;
                    if (cell is LeftCell left) left.DepositObj.Visibility = Visibility.Hidden;
                }*/

        public static void SetValueForDepositCounter(UIElement cell, int value)
        {
            if (cell is IRegularCellsActions inter) inter.SetValueForDepositCounter(value);

/*            if (cell is UpperCell up) up.DepositObj.Counter.Text = value.ToString();
            if (cell is RightCell right) right.DepositObj.Counter.Text = value.ToString();
            if (cell is BottomCell bottom) bottom.DepositObj.Counter.Text = value.ToString();
            if (cell is LeftCell left) left.DepositObj.Counter.Text = value.ToString();*/
        }

        public static Image GetKeyImage()
        {
            string addedImagesPath = GetAddedImagePath();
            string keyPath = Path.Combine(addedImagesPath, "Keys");
            string imagePath = Path.Combine(keyPath, "dragon.png");

            return new Image()
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
                Stretch = Stretch.Uniform
            };
        }

        public static Image GetFaceImage()
        {
            string resPath = Path.Combine(GetImagesPath(), "face.png");

            return new Image()
            {
                Source = new BitmapImage(new Uri(resPath, UriKind.Absolute))
            };
        }

        public static string GetImagesPath()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            return Path.Combine(visPath, "Images");
        }
    }
}
