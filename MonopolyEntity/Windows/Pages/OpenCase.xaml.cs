using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Actions;
using MahApps.Metro.Controls;
using MonopolyEntity;
using MonopolyEntity.Windows.UserControls;
using MonopolyEntity.Windows.UserControls.CaseOpening;

using MonopolyDLL.Monopoly.InventoryObjs;
using MonopolyEntity.VisualHelper;
using MonopolyDLL.DBService;
using System.CodeDom;
using MonopolyDLL;


namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для OpenCase.xaml
    /// </summary>
    public partial class OpenCase : Page
    {
        private CaseRoulette _rol;
        private CaseBox _caseBox;
        private string _loggedUserLogin;

        public OpenCase(CaseBox box, string loggedUserLogin)
        {
            _caseBox = box;
            _loggedUserLogin = loggedUserLogin;

            InitializeComponent();

            SetCaseDrops();
            FillCheckToOpen();
        }

        public void FillCheckToOpen()
        {
            CheckToOpen.KeyToBox.CardName.Text = "Key";

            Image img = BoardHelper.GetLotBoxImage(_caseBox.ImagePath);
            CheckToOpen.Box.CardName.Text = $"{_caseBox.Name} box";
            CheckToOpen.Box.CardImage.Source = img.Source;
        }

        public void SetCaseDrops()
        {
            List<CaseCard> cards = new List<CaseCard>();
            for (int i = 0; i < _caseBox.ItemsThatCanDrop.Count; i++)
            {
                cards.Add(GetCaseCards(_caseBox.ItemsThatCanDrop[i].Name,
                    BoardHelper.GetAddedItemImage(_caseBox.ItemsThatCanDrop[i].ImagePath, _caseBox.ItemsThatCanDrop[i].Type),
                    _caseBox.ItemsThatCanDrop[i]));
            }
            
            cards = BoardHelper.SetCardsInRightPosition(cards);

            for (int i = 0; i < cards.Count; i++)
            {
                CanBeDropedPanel.Children.Add(cards[i]);
            }
        }
      
        public static CaseCard GetCaseCards(string name, Image img, MonopolyDLL.Monopoly.InventoryObjs.Item item)
        {
            CaseCard newCard = new CaseCard()
            {
                Width = 175,
                Height = 175
            };

            newCard.CardImage.Source = img.Source;
            newCard.CardName.Text = name;
            newCard.Margin = new System.Windows.Thickness(20);

            newCard.BorderBgColor.Background = BoardHelper.GetRearityColorForCard(item);


            newCard.BorderBase.Clip = new RectangleGeometry()
            {
                RadiusX = 10,
                RadiusY = 10,
                Rect = new System.Windows.Rect(0, 0, newCard.Width, newCard.Height)
            };
            return newCard;
        }

        private (List<Image>, List<string>, List<SolidColorBrush>) GetParamsForCaseRoulette()
        {
            (List<Image> images, List<string> names, List<SolidColorBrush> colors) res = 
                (new List<Image>(), new List<string>(), new List<SolidColorBrush>());

            for(int i = 0; i < _caseBox.ItemsThatCanDrop.Count; i++)
            {
                res.names.Add(_caseBox.ItemsThatCanDrop[i].Name);
                res.images.Add(BoardHelper.GetAddedItemImage(
                    _caseBox.ItemsThatCanDrop[i].ImagePath, _caseBox.ItemsThatCanDrop[i].Type));
                res.colors.Add(BoardHelper.GetRearityColorForCard(_caseBox.ItemsThatCanDrop[i]));
            }
            return res;
        }

        private void OpenCaseBut_Click(object sender, RoutedEventArgs e)
        {
            ExitBut.IsEnabled = false;
            OpenCaseBut.Visibility = Visibility.Hidden;
            CheckToOpen.Visibility = Visibility.Hidden;

            (List<Image> images, List<string> names, List<SolidColorBrush> colors) 
                rouletteParams = GetParamsForCaseRoulette();
            _rol = new CaseRoulette(rouletteParams.images, rouletteParams.names, rouletteParams.colors)
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Width = 600,
                Height = 175,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };


            _rol._animation.Completed += (obj, ev) =>
            {
                if (!_rol._animationIsDone) return;

                 MonopolyDLL.Monopoly.InventoryObjs.BoxItem prize = 
                DBQueries.GetBoxItemByName(_rol._resCard.CardName.Text);

                DBQueries.AddBoxItemInUserInventory(_loggedUserLogin, prize.Name);

                _rol._animationIsDone = false;
            };


            _rol.SetChosenLine();

            OpenGrid.Children.Remove(_rol);
            OpenGrid.Children.Add(_rol);
        }

        private void ExitBut_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ExitBut.Foreground = Brushes.LightGray;
        }

        private void ExitBut_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            ExitBut.Foreground = Brushes.Gray;
        }

        private void ExitBut_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            WorkWindow window = (WorkWindow)Window.GetWindow(this);
            if (window != null)
            {
                window.CaseFrame.Content = null;
                window.CaseFrame.Visibility = Visibility.Hidden;
                //window.WorkFrame.Effect = null;
                window.VisiableItems.Effect = null;
            }
        }
    }
}
