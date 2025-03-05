using System;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

using Path = System.IO.Path;

namespace MonopolyEntity.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для CaseCard.xaml
    /// </summary>
    public partial class CaseCard : UserControl
    {      
        public CaseCard(string name)
        {
            InitializeComponent();

            SetCardImage(name);
            SetBorderClip();
        }

        public CaseCard()
        {
            InitializeComponent();

            SetBorderClip();
            //SetCardImage(string.Empty);
        }

        public void SetBorderClip()
        {
            if (ActualHeight == 0 || ActualWidth == 0) return; 

            Console.WriteLine(ActualWidth + ActualHeight);

            return;
            BorderBase.Clip = new RectangleGeometry()
            {
                RadiusX = 10,
                RadiusY = 10,
                Rect = new Rect(0, 0, ActualWidth, ActualHeight)
            };
        }

        public void SetCardImage(string picName)
        {
            string imgPath = GetImageFolderDirectory();
            SetPicImageByName(picName, imgPath);
        }

        private void SetPicImageByName(string picName, string folderPath)
        {
            string testPath = Path.Combine(folderPath, "Calivan.jpg");

            string imgPath = GetGivenImgNamePath(folderPath, picName);

            CardImage.Source = !(imgPath is null) ?
                new BitmapImage(new Uri(imgPath, UriKind.Absolute)) :
                new BitmapImage(new Uri(testPath, UriKind.Absolute));
        }

        private string GetGivenImgNamePath(string folderPath, string imgName)
        {
            string[] supExts = { ".jpg", ".png", ".jpeg" };
            string imgPath = null;

            foreach (string exts in supExts)
            {
                string potentialPath = Path.Combine(folderPath, imgName + exts);
                if (File.Exists(potentialPath))
                {
                    imgPath = potentialPath;
                    break;
                }
            }
            return imgPath;
        }

        public string GetImageFolderDirectory()
        {
            DirectoryInfo baseDirectoryInfo = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            string parentPath = baseDirectoryInfo.Parent.Parent.FullName;
            string visPath = Path.Combine(parentPath, "Visuals");
            string imagePath = Path.Combine(visPath, "Images");

            return imagePath;
        }

        private const int _centerDevider = 2;
        private void CardName_Loaded(object sender, RoutedEventArgs e)
        {
            double upperPadding = DownRow.ActualHeight / _centerDevider - CardName.FontSize / _centerDevider;
            if (upperPadding < 0) return;

            CardName.Padding = new Thickness(0, DownRow.ActualHeight / _centerDevider - CardName.FontSize / _centerDevider, 0, 0);
        }

    }
}
