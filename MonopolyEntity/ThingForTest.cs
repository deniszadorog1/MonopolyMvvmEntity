using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using MonopolyEntity.Windows.UserControls;

namespace MonopolyEntity
{
    public static class ThingForTest
    {
        public static Image GetCalivanImage()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");

            string testPath = Path.Combine(imagePath, "Calivan.jpg");


            Image calivan = new Image()
            {
                Source = new BitmapImage(new Uri(testPath, UriKind.Absolute))
            };

            return calivan;
        }

        public static List<CaseCard> GetTestCaseCards()
        {
            const int amountOfTestCards = 8;

            List<CaseCard> res = new List<CaseCard>();
            for(int i = 0; i < amountOfTestCards; i++)
            {
                CaseCard newCard = new CaseCard()
                {
                    Width = 175,
                    Height = 175
                };

                newCard.CardImage.Source = GetCalivanImage().Source;
                newCard.CardName.Text = ("Test Index - " + i).ToString();
                newCard.Margin = new System.Windows.Thickness(20);

                newCard.BorderBase.Clip = new RectangleGeometry()
                {
                    RadiusX = 10,
                    RadiusY = 10,
                    Rect = new System.Windows.Rect(0, 0, newCard.Width, newCard.Height)
                };

                res.Add(newCard);
            }

            return res;
        }

        public static Image GetCalivanBigCircleImage(int imgWidth, int imgHeight)
        {
            Image img = GetCalivanImage();

            img.Width = imgWidth;
            img.Height = imgHeight;

            return GetCircleImage(img);
        }
      
        public static Image GetCircleImage(Image image)
        {

            EllipseGeometry clip = new EllipseGeometry
            {
                Center = new Point(image.Width / 2, image.Height / 2),
                RadiusX = image.Width / 2,
                RadiusY = image.Height / 2
            };

            image.Clip = clip;

            return image;
        }

    }
}
