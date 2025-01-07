using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MonopolyEntity.VisualHelper
{
    public class BoardHelper
    {
        public static Image GetSquareByName(string imageName)
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");
            string boardImages = Path.Combine(imagePath, "BoardImages");


            string squarePath = Path.Combine(Path.Combine(boardImages, "CardImages"), "Squares");

            string imgSquarePath = Path.Combine(squarePath, imageName.ToString());


            Image res = new Image()
            {
                Source = new BitmapImage(new Uri(imgSquarePath, UriKind.Absolute))
            };

            return res;
        }

    }
}
