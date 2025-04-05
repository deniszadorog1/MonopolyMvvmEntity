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
using System.Windows;
using System.CodeDom;
using MonopolyDLL;

namespace MonopolyEntity.VisualHelper
{
    public static class MainWindowHelper
    {
        //Get Images
        public static Image GetImageByName(string imageName)
        {
            string mainWindowsImgsPath = GetMainWindowImagePath();
            string imgPath = Path.Combine(mainWindowsImgsPath, imageName);

            return new Image()
            {
                Source = new BitmapImage(new Uri(imgPath, UriKind.Absolute))
            };
        }

        private static string GetMainWindowImagePath()
        {
            return Path.Combine(Helper.GetImagesPath(), "MainWindowImages");
        }

        public static void SetUpperMenuParams(UpperMenu menu, User user)
        {            
            menu.UserAnim.LoginText.Text = user.Login;
            menu.UserAnim.MoneyText.Text = SystemParamsService.GetStringByName("UpperMoneyMoney");
        }

        public static string GetUserImagePath()
        {
            return Path.Combine(Helper.GetImagesPath(), "UserImages");
        }

        public static Image GetUserImage(string name)
        {
            if (name is null) return null;
            string userFolderPath = GetUserImagePath();

            //string imgPath = Path.Combine(userFolderPath, name);
            return new Image()
            {
                Source = new BitmapImage(new Uri(Path.Combine(userFolderPath, name), UriKind.Absolute))
            };
        }

        public static Image GetCircleImage(int imgWidth, int imgHeight, string name)
        {
            return GetCircleImage(new Image()
            {
                Source = name is null ? ThingForTest.GetCalivanImage().Source : GetUserImage(name).Source,
                Width = imgWidth,
                Height = imgHeight
            });

/*            Image img = name is null ? ThingForTest.GetCalivanImage() : GetUserImage(name);

            img.Width = imgWidth;
            img.Height = imgHeight;

            return GetCircleImage(img);*/
        }

        private static readonly int _centerDivider = 2;
        private static readonly int _dpiInBitmap = 96;
        public static Image GetCircleImage(Image image)
        {           
            int width = (int)image.Width;
            int height = (int)image.Height;

            DrawingVisual drawingVisual = new DrawingVisual();
            using (DrawingContext context = drawingVisual.RenderOpen())
            {
                EllipseGeometry clip = new EllipseGeometry(new System.Windows.Point(width / _centerDivider, height / _centerDivider), 
                    width / _centerDivider, height / _centerDivider);

                context.PushClip(clip);
                context.DrawImage(image.Source, new Rect(0, 0, width, height));
                context.Pop();
            }

            RenderTargetBitmap targetBitmap = new RenderTargetBitmap(width, height, _dpiInBitmap, _dpiInBitmap, PixelFormats.Pbgra32);
            targetBitmap.Render(drawingVisual);

            return new Image { Source = targetBitmap, Width = width, Height = height };
        }

        public static string GetSoundLocation(string soundName)
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string soundPath = Path.Combine(visPath, "Sounds");
            return Path.Combine(soundPath, soundName);
        }

        public static Image GetCircleImageFace(Size size)
        {
            Image img = Helper.GetFaceImage();

            img.Width = size.Width;
            img.Height = size.Height;

            return GetCircleImage(img);
        }

        
    }
}
