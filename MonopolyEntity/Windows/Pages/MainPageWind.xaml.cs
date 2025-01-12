using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media;
using MahApps.Metro.Actions;
using MonopolyEntity.Windows.UserControls.MainPage;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private Frame _frame;
        public MainPage(Frame workFrame)
        {
            _frame = workFrame;
            InitializeComponent();

            SetGameWindowImg();
            SetDescRibeCards();

            SetUserImageEvent();
        }

        public void SetUserImageEvent()
        {
            UpperMenuu.UserAnim.UpperRowUserName.MouseDown += (sender, e) =>
            {
                _frame.Content = new UserPage(_frame);
            };
        }

        public void SetDescRibeCards()
        {
            //return;
            SetParamsToCard(OneDesc,"One", "This is first", ThingForTest.GetCalivanImage());
            SetParamsToCard(TwoDesc, "Two", "This is second", ThingForTest.GetCalivanImage());
            SetParamsToCard(ThreeDesc, "Three", "This is third", ThingForTest.GetCalivanImage());
            SetParamsToCard(FourDesc, "Four", "This is fourth", ThingForTest.GetCalivanImage());
            SetParamsToCard(FiveDesc, "Five", "This is fifth", ThingForTest.GetCalivanImage());
            SetParamsToCard(SixDesc, "Six", "This is sixth", ThingForTest.GetCalivanImage());
        }

        public void SetParamsToCard(DescribeBox box, string nameText, string descText, Image img)
        {
            box.Height = 200;
            box.Width = 250;
            box.CardImg.Source = img.Source;
            box.NameText.Text = nameText;
            box.DescribeText.Text = descText;
        }

        public void SetGameWindowImg()
        {
            GameWindowImg.Source = ThingForTest.GetCalivanImage().Source;
        }

        public void OpenInventoryPage()
        {
/*            OpenCase open = new OpenCase();
            _frame.Content = open;

            return;*/
            InventoryPage page = new InventoryPage(_frame);
            _frame.Content = page;
        }

        public void OpenGameField()
        {
            GamePage page = new GamePage(_frame);
            _frame.Content = page;
        }

        public void OpenMainPage()
        {
            MainPage page = new MainPage(_frame);
            _frame.Content = page;
        }
    }
}
