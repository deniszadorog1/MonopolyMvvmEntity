using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using MonopolyEntity.Windows.UserControls;

namespace MonopolyEntity
{
    public static class ThingForTest
    {
        public static Image GetCalivanImage()
        {
            string imagePath = GetImagesPath();

            string testPath = Path.Combine(imagePath, "Calivan.jpg");


            Image calivan = new Image()
            {
                Source = new BitmapImage(new Uri(testPath, UriKind.Absolute))
            };

            return calivan;
        }

        private static string GetImagesPath()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");

            return imagePath;
        }

        public static List<CaseCard> GetTestCaseCards()
        {
            const int amountOfTestCards = 8;

            List<CaseCard> res = new List<CaseCard>();
            for (int i = 0; i < amountOfTestCards; i++)
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


        public static (List<Image> imgs, List<string> names) GetParamsForCaseRoullete()
        {
            const int amountOfItems = 10;
            List<Image> imgs = new List<Image>();
            List<string> names = new List<string>();

            for (int i = 0; i < amountOfItems; i++)
            {
                imgs.Add(GetCalivanImage());
                names.Add("Item - " + i);
            }

            return (imgs, names);
        }


        public static CaseCard GetDragonBoxCard()
        {
            string imgPath = GetImagesPath();
            string itemsPath = Path.Combine(imgPath, "AddItemsImages");
            string boxPath = Path.Combine(itemsPath, "Box");

            string testPath = Path.Combine(boxPath, "dragon.png");
            Image dragonTest = new Image()
            {
                Source = new BitmapImage(new Uri(testPath, UriKind.Absolute))
            };


            CaseCard res = new CaseCard()
            {
                Width = 150,
                Height = 175,
                Margin = new Thickness(10)
            };

            res.CardImage.Source = dragonTest.Source;
            res.CardImage.Width = 100;
            res.CardImage.Height = 100;
            res.CardImage.Stretch = Stretch.Fill;
            res.CardImage.VerticalAlignment = VerticalAlignment.Center;
            res.CardImage.HorizontalAlignment = HorizontalAlignment.Center;


            res.BorderBgColor.Background = Brushes.Blue;
            res.CardName.Foreground = Brushes.White;


            res.CardName.Text = "Dragon case";
            return res;
        }

    }
}
