using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
            string cardIamgesPath = GetCardImagesFolder();
            string pefumePath = Path.Combine(cardIamgesPath, folderName);

            string imgPath = Path.Combine(pefumePath, name);

            return GetImageByPath(imgPath);
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

        public static List<Image> GetAllChipsImages()
        {
            List<Image> res = new List<Image>();

            res.Add(GetChipImageByName("chipRed.png"));
            res.Add(GetChipImageByName("chipBlue.png"));
            res.Add(GetChipImageByName("chipGreen.png"));
            res.Add(GetChipImageByName("chipPurple.png"));
            res.Add(GetChipImageByName("chipOrange.png"));

            return res;
        }

        public static List<List<Point>> GetChipsPoints(Size squareSize)
        {
            List<List<Point>> res = new List<List<Point>>();

            const int distFromBorder = 5;
            int maxPlayers = 5;

            double chipRadius = _chipSize / 2;

            Point center = new Point(squareSize.Width / 2 - chipRadius, squareSize.Height / 2 - chipRadius);

            Point leftUp = new Point(distFromBorder, distFromBorder);
            Point rightUp = new Point(squareSize.Width - _chipSize - distFromBorder, leftUp.Y);

            Point leftDown = new Point(leftUp.X, squareSize.Height - leftUp.Y - _chipSize);
            Point rightDown = new Point(rightUp.X, squareSize.Height - rightUp.Y - _chipSize);

            Point upCenter = new Point(center.X, leftUp.Y);
            Point downCenter = new Point(center.X, leftDown.Y);

            for (int i = 1; i <= maxPlayers; i++)
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

        public static List<Point> GetPointsForChips(Size cellSize, int amountOfChipsInCell)
        {
            List<List<Point>> allPoints = GetChipsPoints(cellSize);

            return allPoints[amountOfChipsInCell];
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
            return new Size();
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

        public static Canvas GetChipCanvas(UIElement el)
        {
            var chipsPlacerField = el.GetType().GetField(
                "ChipsPlacer",
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (chipsPlacerField is null) throw new Exception("Cell doesnt have chips placer");
            return chipsPlacerField.GetValue(el) as Canvas;
        }






    }
}
