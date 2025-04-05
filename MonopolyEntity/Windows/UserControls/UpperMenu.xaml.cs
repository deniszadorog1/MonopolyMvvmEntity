using MonopolyDLL.Monopoly;
using MonopolyEntity.Interfaces;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

using MonopolyEntity.Windows.Pages;

namespace MonopolyEntity.Windows.UserControls
{
    /// <summary>
    /// Логика взаимодействия для UpperMenu.xaml
    /// </summary>
    public partial class UpperMenu : UserControl
    {
        private Page _page;

        public UpperMenu()
        {
            InitializeComponent();
        }

        MonopolySystem _system;
        Frame _frame;
        public void SetMonSystemAndFrame(Frame frame, MonopolySystem system)
        {
            _frame = frame;
            _system = system;
        }
       
        private void UserIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            const int animationDistance = 150;
            const double duration = 0.2;

            var animation = new DoubleAnimation
            {
                To = animationDistance,
                Duration = TimeSpan.FromSeconds(duration)
            };

            animation.Completed += Animation_Complete;
            UserAnim.UserMenu.Visibility = Visibility.Hidden;
            UserAnim.ElemBorder.Background = new SolidColorBrush(Colors.Transparent);

            UserAnim.AnimImage.BeginAnimation(Canvas.LeftProperty, animation);
        }

        private void Animation_Complete(object sender, EventArgs e)
        {
            Canvas.SetZIndex(UserAnim.AnimImage, 0);
        }

        private void InventoryBut_Click(object sender, RoutedEventArgs e)
        {
/*            _page = Helper.FindParent<Page>(this);
            if (_page is null) return;*/

            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new InventoryPage(_frame, _system));

           /* if (_page is Pages.MainPage mainPage)
            {
                mainPage.OpenInventoryPage();
            }
            else if (_page is Pages.ProfileSettings settings)
            {
                settings.OpenInventoryPage();
            }*/

        }

        private void MainLogoBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new Pages.MainPage(_frame, _system));
/*            _page = _page = Helper.FindParent<Page>(this);
            if (_page is null) return;

            if (_page is IPagesOpener inter)
            {
                inter.OpenMainPage();
            }*/

            /*
                        if (_page is Pages.MainPage mainPage)
                        {
                            mainPage.OpenMainPage();
                        }
                        else if (_page is InventoryPage invPage)
                        {
                            invPage.OpenMainPage();
                        }
                        else if (_page is ProfileSettings settings)
                        {
                            settings.OpenMainPage();
                        }*/
        }

/*        public void PageOpener(bool ifGamePage)
        {
            _page = _page = Helper.FindParent<Page>(this);
            if (_page is null) return;



            if (_page is IPagesOpener inter)
            {
                if (ifGamePage) inter.OpenGameField();
                else inter.OpenMainPage();
            }
        }*/

        private void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Window.GetWindow(_frame)).SetFrameContent(new Pages.SetPlayersInGame(_system, _frame));

            /*          _page = _page = Helper.FindParent<Page>(this);
                        if (_page is null) return;

                        if (_page is IPagesOpener inter)
                        {
                            inter.OpenGameField();
                        }*/

            /*            if (_page is Pages.MainPage mainPage)
                        {
                            mainPage.OpenGameField();
                        }
                        else if (_page is InventoryPage invPage)
                        {
                            invPage.OpenGameField();
                        }
                        else if (_page is ProfileSettings settings)
                        {
                            settings.OpenGameField();
                        }*/
        }
    }
}
