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
    public static class MainWindowHelper
    {
        //Get Images
        public static Image GetImagePyName(string imageName)
        {
            string mainWindowsImgsPath = GetMainWindowImagePath();
            string imgPath = Path.Combine(mainWindowsImgsPath, imageName);

            Image img = new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };

            return img;
        }

        private static string GetMainWindowImagePath()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");
            string addedImagesPath = Path.Combine(imagePath, "MainWindowImages");

            return addedImagesPath;
        }
    }
}
