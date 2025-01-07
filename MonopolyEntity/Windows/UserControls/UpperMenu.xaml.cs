using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MonopolyEntity.VisualHelper;
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

        private void UserIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            //return;
            var animation = new DoubleAnimation
            {
                To = 150,
                Duration = TimeSpan.FromSeconds(0.2)
            };
            UserAnim.UserIcon.BeginAnimation(Canvas.LeftProperty, animation);

            UserAnim.UserMenu.Visibility = System.Windows.Visibility.Hidden;
            UserAnim.ElemBorder.Background = new SolidColorBrush(Colors.Transparent);
        }

        private void InventoryBut_Click(object sender, RoutedEventArgs e)
        {
            _page = Helper.FindParent<Page>(this);
            if (_page is null) return;

            if(_page is UserPage page)
            {
                page.OpenInventoryPage();
                return;
            }
           
            if(_page is MonopolyEntity.Windows.Pages.MainPage mainPage)
            {
                mainPage.OpenInventoryPage();
            }
        }

        private void MainLogoBut_Click(object sender, RoutedEventArgs e)
        {

        }

        private void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            _page = _page = Helper.FindParent<Page>(this);
            if (_page is null) return;

            if(_page is MonopolyEntity.Windows.Pages.MainPage mainPage)
            {
                mainPage.OpenGameField();
            }

        }
    }
}
