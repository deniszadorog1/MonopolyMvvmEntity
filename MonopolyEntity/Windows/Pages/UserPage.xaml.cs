using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using static System.Net.Mime.MediaTypeNames;

using MonopolyDLL.Monopoly;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private Frame _frame;
        private MonopolySystem _system;
        public UserPage(Frame workFrame, MonopolySystem system)
        {
            _frame = workFrame;
            _system = system;

            InitializeComponent();

            SetImages();
            SetUpperLineSettings();
        }

        public void SetUserImageEvent()
        {
            UpperMenuu.UserAnim.UpperRowUserName.MouseDown += (sender, e) =>
            {
                _frame.Content = new UserPage(_frame, _system);
            };
        }

        public void OpenMainPage()
        {
            MainPage page = new MainPage(_frame, _system);
            _frame.Content = page;
        }

        public void OpenInventoryPage()
        {
            InventoryPage page = new InventoryPage(_frame, _system);
            _frame.Content = page;
        }

        public void OpenGameField()
        {
            GamePage page = new GamePage(_frame, _system);
            _frame.Content = page;
        }


        public void SetUpperLineSettings()
        {
            //Set bg
            UpperMenuu.CanvasBg.Background = new SolidColorBrush(Colors.Transparent);
            UpperMenuu.Background = new SolidColorBrush(Colors.Transparent);

            //Set buttons colors
            UpperMenuu.MainLogoBut.Foreground = new SolidColorBrush(Colors.Gray);

            UpperMenuu.StartGameBut.Foreground = new SolidColorBrush(Colors.White);
            UpperMenuu.StartGameBut.Background = (Brush)System.Windows.Application.Current.Resources["MainGlobalColor"];

            UpperMenuu.InventoryBut.Foreground = new SolidColorBrush(Colors.Gray);
            
            UpperMenuu.UserAnim.UserIcon.Foreground = new SolidColorBrush(Colors.Gray);
        }

        public void SetImages()
        {
            SetUserImage();
            SetRankImage();
        }

        public void SetUserImage()
        {
            System.Windows.Controls.Image img = ThingForTest.GetCalivanBigCircleImage(200, 200);
            UserImageGrid.Children.Add(img);

            Canvas.SetLeft(img, 20); 
            Canvas.SetTop(img, 50);
        }

        public void SetRankImage()
        {
            System.Windows.Controls.Image img = ThingForTest.GetCalivanBigCircleImage(80, 80);

            RankImageGrid.Children.Add(img);

            Canvas.SetLeft(img, -10);
            Canvas.SetTop(img, -10);
        }

        private void ShowInventoryBut_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(sender is Button but)
            {
                but.Background = (Brush)System.Windows.Application.Current.Resources["MainGlobalColor"];
                but.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void ShowInventoryBut_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (sender is Button but)
            {
                but.Background = Brushes.LightGray;
                but.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }
    }
}
