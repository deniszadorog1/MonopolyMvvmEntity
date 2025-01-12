using MahApps.Metro.Controls;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

using MonopolyDLL;

namespace MonopolyEntity.Windows.UserControls.GameControls
{
    /// <summary>
    /// Логика взаимодействия для GameField.xaml
    /// </summary>
    /// 

    public partial class GameField : UserControl
    {
        public GameField()
        {
            InitializeComponent();

            SetImmoratlImages();
            HideHeadersInCells();

            MovementTest();
        }


     
        //BREAKETS, mb need to move it into anouther file
        //set chips position and make a movement  
        
        //Set chips position in fields mathematicly
        
        private void MovementTest()
        {
            //Set chips in start cell
            SetChipPositionsInStartSquare();

            //make 
        }

        private void SetChipPositionsInStartSquare()
        {
            List<Image> imgs = BoardHelper.GetAllChipsImages();

            List<List<Point>> points = BoardHelper.GetChipsPoints(
                new Size(StartCell.Width, StartCell.Height));

            List<Point> pointsForChips = points[imgs.Count - 1];

            for (int i = 0; i < imgs.Count; i++)
            {
                Canvas.SetLeft(imgs[i], pointsForChips[i].X);
                Canvas.SetTop(imgs[i], pointsForChips[i].Y);

                StartCell.ChipsPlacer.Children.Add(imgs[i]);
            }
        }
        

        //



        public void HideHeadersInCells()
        {
            ChanceOne.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceTwo.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceThree.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceFour.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceFive.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceSix.MoneyPlacer.Visibility = Visibility.Hidden;

            LittleTax.MoneyPlacer.Visibility = Visibility.Hidden;
            BigTax.MoneyPlacer.Visibility = Visibility.Hidden;
        }

        private const int _upperMargin = 10;
        //That cant be changed with box items
        private void SetImmoratlImages()
        {
            SetSquareCells();

            SetTaxesImages();
            SetChancees();

            SetBasicBusinessesImages();
        }

        public void SetBasicBusinessesImages()
        {
            //Get (to make in future) parameters for image from table

            SetBasicPerfumeImages();
            SetBasicClothesImages();

            SetBasicMessagerImages();
            SetBasicDrinkingsImages();

            SetBasicPlanesImages();
            SetBasicFoodImages();

            SetBasicHotelImages();
            SetBasicPhoneImages();

            SetBasicCarImages();
            SetBasicGameImages();
        }

        private void SetBasicGameImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("rockstar_games.png", "Games"), GamesFirstBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("rovio.png", "Games"), GamesSecondBus, new Size(100, 25));
        }

        private void SetBasicCarImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("mercedes.png", "Cars"), UpCar, new Size(50, 50));
            SetRightCellImage(BoardHelper.GetImageFromFolder("audi.png", "Cars"), RightCar, new Size(100, 30));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("ford.png", "Cars"), DownCars, new Size(95, 40));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("land_rover.png", "Cars"), LeftCar, new Size(100, 45));

        }

        private void SetBasicPhoneImages()
        {
            SetLeftCellImage(BoardHelper.GetImageFromFolder("apple.png", "Phones"), PhonesFirstBus, new Size(45, 45));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("nokia.png", "Phones"), PhonesSecondBus, new Size(100, 25));
        }

        private void SetBasicHotelImages()
        {
            SetLeftCellImage(BoardHelper.GetImageFromFolder("holiday_inn.png", "Hotels"), HotelFirstBus, new Size(65, 45));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("radisson_blu.png", "Hotels"), HotelSecondBus, new Size(100, 35));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("novotel.png", "Hotels"), HotelThirdBus, new Size(100, 45));
        }

        private void SetLeftCellImage(Image img, LeftCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicFoodImages()
        {
            SetBottomCellImage(BoardHelper.GetImageFromFolder("max_burgers.png", "Food"), FoodFirstBus, new Size(65, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("burger_king.png", "Food"), FoodSecondBus, new Size(65, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("kfc.png", "Food"), FoodThirdBus, new Size(55, 45));
        }

        private void SetBasicPlanesImages()
        {
            SetBottomCellImage(BoardHelper.GetImageFromFolder("lufthansa.png", "Planes"), PlanesFirstBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("british_airways.png", "Planes"), PlanesSecondBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("american_airlines.png", "Planes"), PlanesThirdBus, new Size(100, 20));
        }

        private void SetBottomCellImage(Image img, BottomCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicDrinkingsImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("coca_cola.png", "Drinkings"), DrinkFirstBus, new Size(100, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("fanta.png", "Drinkings"), DrinkSecondBus, new Size(75, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("pepsi.png", "Drinkings"), DrinkThirdBus, new Size(75, 45));
        }

        private void SetBasicMessagerImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("vk.png", "Messagers"), MessagerFirstBus, new Size(75, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("facebook.png", "Messagers"), MessagerSecondBus, new Size(100, 30));
            SetRightCellImage(BoardHelper.GetImageFromFolder("twitter.png", "Messagers"), MessagerThirdBus, new Size(55, 45));
        }

        private void SetRightCellImage(Image img, RightCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetRightLeftImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Fill;
            img.Width = imageSize.Width;
            img.Height = imageSize.Height;
        }



        private void SetBasicClothesImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("adidas.png", "Clothes"), ClothesFirstBus, new Size(75, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("puma.png", "Clothes"), ClothesSeconBus, new Size(100, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("lacoste.png", "Clothes"), ClothesThirdBus, new Size(100, 45));
        }

        public void SetBasicPerfumeImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("chanel.png", "Perfume"), PerfumeFirstBus, new Size(75, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("hugo_boss.png", "Perfume"), PerfumeSecondBus, new Size(100, 45));
        }

        private void SetUpperCellImage(Image img, UpperCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Fill;

            img.Width = imageSize.Width;
            img.Height = imageSize.Height;

            SetRenderToTurnImageForUpDownCell(img);
        }

        private void SetRenderToTurnImageForUpDownCell(Image img)
        {
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform transform = new RotateTransform()
            {
                Angle = -90,
            };

            img.LayoutTransform = transform;
        }


        private void SetChancees()
        {
            SetUpDownChances();
            SetRightChances();
            SetLeftChances();
        }

        public void SetLeftChances()
        {
            ChanceFive.ImagePlacer.Children.Add(GetLeftChances());
            ChanceSix.ImagePlacer.Children.Add(GetLeftChances());
        }

        private Image GetLeftChances()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chance_rotated.png");

            img.Width = this.Width;
            img.Height = ChanceFive.ImagePlacer.Height;

            return img;
        }

        private Image GetRightChanceIamge()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chanceUp.png");

            img.Width = this.Width;
            img.Height = ChanceFive.ImagePlacer.Height;

            return img;
        }

        private void SetRightChances()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chance.png");

            img.Width = this.Width;
            img.Height = ChanceThree.ImagePlacer.Height;

            ChanceThree.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownChances()
        {
            ChanceOne.ImagePlacer.Children.Add(GetUpChanceImage());
            ChanceTwo.ImagePlacer.Children.Add(GetUpChanceImage());
            ChanceFour.ImagePlacer.Children.Add(GetUpChanceImage());

        }

        private Image GetUpChanceImage()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chanceUp.png");

            img.Width = this.Width;
            img.Height = ChanceOne.ImagePlacer.Height;

            return img;
        }

        private void SetTaxesImages()
        {
            //Set little Tax
            Image little = BoardHelper.GetTaxImageByName("tax_income.png");
            little.Width = LittleTax.Width - _upperMargin;
            little.Height = LittleTax.Height / 2;

            little.HorizontalAlignment = HorizontalAlignment.Center;
            little.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(little, 10);

            LittleTax.ImagePlacer.Children.Add(little);

            //Set big tax

            Image big = BoardHelper.GetTaxImageByName("tax_luxury.png");
            big.Width = BigTax.Width / 2;
            big.Height = big.Height - 10;

            big.HorizontalAlignment = HorizontalAlignment.Center;
            big.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(big, 10);

            BigTax.ImagePlacer.Children.Add(big);
        }

        public void SetSquareCells()
        {
            SetStartImage();
            SetJackPotImage();
            SetGoToPrisonImage();
            SetPrisonImage();
        }

        private void SetPrisonImage()
        {
            const int width = 50;
            const int height = 50;
            Image jail = new Image()
            {
                Source = BoardHelper.GetSquareByName("jail.png").Source,
                Width = width + 10,
                Height = height + 10
            };

            Canvas.SetLeft(jail, 0);
            Canvas.SetTop(jail, 65);

            PrisonCell.ImagesPlacer.Children.Add(jail);


            Image donat = new Image()
            {
                Source = BoardHelper.GetSquareByName("jail-visiting.png").Source,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(donat, 65);
            Canvas.SetTop(donat, 10);

            PrisonCell.ImagesPlacer.Children.Add(donat);
        }

        private void SetGoToPrisonImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("goToJail.png").Source,
                Width = 125,
                Height = 125
            };

            GoToPrisonCell.ImagesPlacer.Children.Add(img);
        }

        private void SetJackPotImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("jackpot.png").Source,
                Width = 125,
                Height = 125
            };

            JackPotCell.ImagesPlacer.Children.Add(img);
        }

        private void SetStartImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("start.png").Source,
                Width = 125,
                Height = 125
            };

            StartCell.ImagesPlacer.Children.Add(img);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter) &&
                MessageWriter.Text != string.Empty)
            {
                AddMessageTextBlock(MessageWriter.Text);
                MessageWriter.Text = string.Empty;
            }
        }

        private void AddMessageTextBlock(string message)
        {
            TextBlock block = new TextBlock()
            {
                Text = message,
                FontSize = 16,
                Foreground = Brushes.White
            };

            GameChat.Items.Add(block);
        }

    }
}
