using MonopolyEntity.Windows.UserControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

using MonopolyDLL.Monopoly;
using System.Windows.Media;

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

        public static void SetUpperMenuParams(UpperMenu menu, User user)
        {
            menu.UserAnim.LoginText.Text = user.Login;
            menu.UserAnim.MoneyText.Text = "No money";
        }

        public static string GetUserImagePath()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");
            string userImages = Path.Combine(imagePath, "UserImages");

            return userImages;
        }

        public static Image GetUserImage(string name)
        {
            if (name is null) return null;
            string userFolderPath = GetUserImagePath();

            string imgPath = Path.Combine(userFolderPath, name);

            Image img = new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };

            return img;
        }

        public static Image GetCircleImage(int imgWidth, int imgHeight, string name)
        {
            Image img = name is null ? ThingForTest.GetCalivanImage() : GetUserImage(name);

            img.Width = imgWidth;
            img.Height = imgHeight;

            return GetCircleImage(img);
        }

        public static Image GetCircleImage(Image image)
        {
            EllipseGeometry clip = new EllipseGeometry
            {
                Center = new System.Windows.Point(image.Width / 2, image.Height / 2),
                RadiusX = image.Width / 2,
                RadiusY = image.Height / 2
            };

            image.Clip = clip;

            return image;
        }
    }
}
