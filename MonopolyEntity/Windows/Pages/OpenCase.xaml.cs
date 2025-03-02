using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MonopolyEntity;
using MonopolyEntity.Windows.UserControls;
using MonopolyEntity.Windows.UserControls.CaseOpening;

using MonopolyDLL.Monopoly.InventoryObjs;
using MonopolyEntity.VisualHelper;
using MonopolyDLL;
using System.Windows.Input;
using System.Windows.Media.Effects;

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
            CheckToOpen.KeyToBox.CardName.Text = SystemParamsServeses.GetStringByName("CaseOpenKeyName");

            Image img = BoardHelper.GetLotBoxImage(_caseBox.ImagePath);
            CheckToOpen.Box.CardName.Text = $"{_caseBox.Name} " +
                $"{SystemParamsServeses.GetStringByName("CaseOpenToOpenBox")}";
            CheckToOpen.Box.CardImage.Source = img.Source;

            CheckToOpen.Box.CardImage.Stretch = Stretch.Uniform;
            CheckToOpen.Box.CardImage.Width = 125;
            CheckToOpen.Box.CardImage.Height = 125;

            Image key = BoardHelper.GetKeyImage();
            CheckToOpen.KeyToBox.CardImage.Source = key.Source;
            CheckToOpen.KeyToBox.CardImage.Width = 100;
            CheckToOpen.KeyToBox.CardImage.Height = 100;
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

        public static CaseCard GetCaseCards(string name, Image img, Item item)
        {
            Size cardSize = new Size(175, 175);
            Size cardImageSize = new Size(125, 125);
            const int cardMargin = 20;
            const int baseRadius = 10;

            CaseCard newCard = new CaseCard()
            {
                Width = cardSize.Width,
                Height = cardSize.Height
            };

            newCard.CardImage.Source = img.Source;
            newCard.CardImage.Width = cardImageSize.Width;
            newCard.CardImage.Height = cardImageSize.Height;


            newCard.CardName.Text = name;
            newCard.Margin = new Thickness(cardMargin);

            newCard.BorderBgColor.Background = BoardHelper.GetRearityColorForCard(item);

            newCard.BorderBase.Clip = new RectangleGeometry()
            {
                RadiusX = baseRadius,
                RadiusY = baseRadius,
                Rect = new Rect(0, 0, newCard.Width, newCard.Height)
            };
            return newCard;
        }

        private (List<Image>, List<string>, List<SolidColorBrush>) GetParamsForCaseRoulette()
        {
            (List<Image> images, List<string> names, List<SolidColorBrush> colors) res =
                (new List<Image>(), new List<string>(), new List<SolidColorBrush>());

            for (int i = 0; i < _caseBox.ItemsThatCanDrop.Count; i++)
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
            Size rouletteSize = new Size(600, 175);

            ExitBut.IsEnabled = false;
            OpenCaseBut.Visibility = Visibility.Hidden;
            CheckToOpen.Visibility = Visibility.Hidden;

            (List<Image> images, List<string> names, List<SolidColorBrush> colors)
                rouletteParams = GetParamsForCaseRoulette();
            _rol = new CaseRoulette(rouletteParams.images, rouletteParams.names, rouletteParams.colors)
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Width = rouletteSize.Width,
                Height = rouletteSize.Height,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            _rol._animation.Completed += (obj, ev) =>
            {
                if (!_rol._animationIsDone) return;

                BoxItem prize =
                DBQueries.GetBoxItemByName(_rol._resCard.CardName.Text);

                DBQueries.AddBoxItemInUserInventory(_loggedUserLogin, prize.Name);

                _rol._animationIsDone = false;

                AddPrizeControl(prize);
            };

            _rol.SetChosenLine();

            OpenGrid.Children.Remove(_rol);
            OpenGrid.Children.Add(_rol);
        }

        private void AddPrizeControl(BoxItem prize)
        {
            ShowGotPrize prizeControl = new ShowGotPrize(prize, _rol._resCard.CardImage);

            prizeControl.VerticalAlignment = VerticalAlignment.Center;
            prizeControl.HorizontalAlignment = HorizontalAlignment.Center;

            prizeControl.AcceptBut.Click += (sender, e) =>
            {
                OpenCaseGrid.Effect = null;
                PrizeGrid.Children.Clear();
            };

            PrizeGrid.Children.Add(prizeControl);
            SetBlurEffect();
        }

        private void SetBlurEffect()
        {
            const int fullBlur = 100;
            OpenCaseGrid.Effect = new BlurEffect
            {
                Radius = fullBlur
            };
        }

        private void ExitBut_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
            ExitBut.Foreground = Brushes.LightGray;
        }

        private void ExitBut_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Cursor = null;
            ExitBut.Foreground = Brushes.Gray;
        }

        private void ExitBut_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MainWindow window = (MainWindow)Window.GetWindow(this);
            if (window != null)
            {
                window.CaseFrame.Content = null;
                window.CaseFrame.Visibility = Visibility.Hidden;
                window.VisiableItems.Effect = null;
            }
        }
    }
}
