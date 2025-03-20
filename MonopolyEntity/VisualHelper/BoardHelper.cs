using MonopolyDLL.Monopoly.Enums;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using MonopolyDLL.Monopoly.InventoryObjs;
using MonopolyEntity.Windows.UserControls;
using MonopolyDLL;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.CodeDom;

namespace MonopolyEntity.VisualHelper
{
    public class BoardHelper
    {
        public static Image GetSquareByName(string imageName)
        {
            string boardImages = GetBoardImagesPath();
            string squarePath = Path.Combine(Path.Combine(boardImages, "CardImages"), "Squares");
            string imgSquarePath = Path.Combine(squarePath, imageName);

            Image res = new Image()
            {
                Source = new BitmapImage(new Uri(imgSquarePath, UriKind.Absolute))
            };

            return res;
        }

        public static Image GetTaxImageByName(string imageName)
        {
            string boardImagesPath = GetBoardImagesPath();
            string cardImages = Path.Combine(boardImagesPath, "CardImages");
            string taxPath = Path.Combine(cardImages, "Tax");

            string imgPath = Path.Combine(taxPath, imageName);

            Image res = new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };

            return res;
        }

        private static string GetBoardImagesPath()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");
            string boardImages = Path.Combine(imagePath, "BoardImages");

            return boardImages;
        }

        public static Image GetImageFromOtherFolder(string name)
        {
            string boardImagesPath = GetBoardImagesPath();
            string otherImages = Path.Combine(boardImagesPath, "Other");

            string imgPath = Path.Combine(otherImages, name);

            Image res = new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };

            return res;
        }

        private static Image GetImageByPath(string imgPath)
        {
            Image res = new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };
            return res;
        }

        private static string GetCardImagesFolder()
        {
            string boardImagesPath = GetBoardImagesPath();
            string cardImagesPath = Path.Combine(boardImagesPath, "CardImages");

            return cardImagesPath;
        }


        public static Image GetImageFromFolder(string name, string folderName)
        {
            string cardImagesPath = GetCardImagesFolder();
            string perfumePath = Path.Combine(cardImagesPath, folderName);

            string imgPath = Path.Combine(perfumePath, name);

            return GetImageByPath(imgPath);
        }

        public static string GetDiceFolderPath()
        {
            string res;

            string borderImagesPath = GetBoardImagesPath();
            res = Path.Combine(borderImagesPath, "DiceRibs");

            return res;
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

            List<Image> chips = new List<Image>();

            chips.Add(GetChipImageByName("chipRed.png"));
            chips.Add(GetChipImageByName("chipBlue.png"));
            chips.Add(GetChipImageByName("chipGreen.png"));
            chips.Add(GetChipImageByName("chipPurple.png"));
            chips.Add(GetChipImageByName("chipOrange.png"));

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

        private const int _maxPlayers = 5;

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

            for (int i = 1; i <= _maxPlayers; i++)
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

            return res;
        }

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
            if (cell is UpperCell up)
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
            throw new Exception("No such type of cell");
        }

        public static int GetAmountOfItemsInCell(UIElement cell)
        {
            if (cell is UpperCell up)
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
            return 0;
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

                res.Add(temp);
            }

            return res;
        }

        public static List<Point> GetPointsForPrisonCellExcurs(int amountOfChips, Size cellSize)
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
            const int firstLineLasCellIndex = 10;
            const int secondLineLasCellIndex = 20;
            const int thirdLineLasCellIndex = 30;
            const int fourthLineLasCellIndex = 40;
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
            if (cell is SquareCell square)
            {
                return new Point(square.ActualWidth / _centerDivider  - img.Width / _centerDivider, 
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
                    upper.ChipsPlacer.ActualHeight / _centerDivider  - img.Height / _centerDivider);
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
            return new Point(0, 0);
        }

        private List<Point> GetChipsPointsOnPrisonCell(PrisonCell cell)
        {
            List<Point> res = new List<Point>();
            List<Image> img = cell.ChipsPlacerVisit.Children.OfType<Image>().ToList();

            for (int i = 0; i < img.Count; i++)
            {
                res.Add(new Point(Canvas.GetLeft(img[i]), Canvas.GetTop(img[i])));
            }
            return res;
        }

        private static string GetAddedImagePath()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");
            string addedImagesPath = Path.Combine(imagePath, "AddItemsImages");

            return addedImagesPath;
        }

        public static Image GetLotBoxImage(string imageName)
        {
            string addedImagesPath = GetAddedImagePath();
            string boxPath = Path.Combine(addedImagesPath, "Box");
            string imgPath = Path.Combine(boxPath, imageName);

            Image img = new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute)),
                Stretch = Stretch.Uniform                
            };
            return img;
        }

        public static Image GetAddedItemImage(string imageName, BusinessType type)
        {
            string addedImagesPath = GetAddedImagePath();
            string busPath = Path.Combine(addedImagesPath, "Businesses");
            string busTypePath = GetFieldPathCombineByType(busPath, type);
            string imagePath = Path.Combine(busTypePath, imageName);
            return new Image()
            {
                Source = new BitmapImage(new Uri(imagePath, UriKind.Absolute)),
                Stretch = Stretch.Uniform
            };
        }

        private static string GetFieldPathCombineByType(string boxFolderPath, BusinessType type)
        {
            switch (type)
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
                    return Path.Combine(boxFolderPath, "Phones") ;
            }
            throw new Exception("No such type");
        }

        private static readonly Color _basicRartyColor = Color.FromRgb(76, 180, 219);
        public static SolidColorBrush GetRarityColorForCard(Item item)
        {
            if (item is BoxItem boxItem)
            {
                return new SolidColorBrush(Color.FromRgb(
                    boxItem.GetRParam(), boxItem.GetGParam(), boxItem.GetBParam()));
            }
            return new SolidColorBrush(_basicRartyColor);
        }

        public static List<CaseCard> SetCardsInRightPosition(List<CaseCard> cards)
        {
            List<CaseCard> res = new List<CaseCard>();
            
            List<SolidColorBrush> colors = GetAllColorsFromDB();
            for(int i = 0; i < colors.Count; i++)
            {
                for(int j = 0; j < cards.Count; j++)
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
            (byte r,byte g, byte b) colorParams = DBQueries.GetColorByRarityName(name);       
            return new SolidColorBrush(Color.FromRgb(colorParams.r, colorParams.g, colorParams.b));
        }

        public static void ChangeDepositCounterVisibility(UIElement cell, Visibility visibility)
        {
            if (cell is UpperCell up) up.DepositObj.Visibility = visibility;
            if (cell is RightCell right) right.DepositObj.Visibility = visibility;
            if (cell is BottomCell bottom) bottom.DepositObj.Visibility = visibility;
            if (cell is LeftCell left) left.DepositObj.Visibility = visibility;
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
            if (cell is UpperCell up)  up.DepositObj.Counter.Text = value.ToString();
            if (cell is RightCell right)  right.DepositObj.Counter.Text = value.ToString();
            if (cell is BottomCell bottom) bottom.DepositObj.Counter.Text = value.ToString();
            if (cell is LeftCell left)  left.DepositObj.Counter.Text = value.ToString();
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
    }
}
