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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using MonopolyEntity.Windows.Pages;

namespace MonopolyEntity.Windows
{
    /// <summary>
    /// Логика взаимодействия для WorkWindow.xaml
    /// </summary>
    public partial class WorkWindow : Window
    {
        public WorkWindow()
        {
            InitializeComponent();

            SetStartPage();
        }

        public void SetStartPage()
        {
            //WorkFrame.Content = new OpenCase();

            MainPage mainPage = new MainPage(WorkFrame);
            SetEventsForMainPage(mainPage);

            WorkFrame.Content = mainPage;   
        }

        public void SetEventsForMainPage(MainPage page)
        {
            page.UpperMenuu.UserAnim.UpperRowUserName.MouseDown += (sender, e) =>
            {
                SetUserPage();
            };
        }

        public void SetUserPage()
        {
            UserPage page = new UserPage(WorkFrame);

            WorkFrame.Content = page;
        }

    }
}
