using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.IO.Packaging;
using System.Linq;
using System.Security.Cryptography;
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
using MaterialDesignThemes.Wpf;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
using MonopolyEntity.Windows.UserControls;

namespace MonopolyEntity.Windows.UserControls.CaseOpening
{
    /// <summary>
    /// Логика взаимодействия для CaseRoulette.xaml
    /// </summary>
    public partial class CaseRoulette : UserControl
    {
        private List<Image> _imgs;
        private List<string> _names;

        public bool _animationIsDone = false;

        public CaseRoulette()
        {
            _imgs = new List<Image>();
            _names = new List<string>();
            _animationIsDone = false;
            SetTestValues();

            InitializeComponent();

            FillCaseCard();
            MakeRotationAction();

            //SetTestValues();
        }

        public CaseRoulette(List<Image> caseImgs, List<string> itemNames)
        {
            _imgs = caseImgs;
            _names = itemNames;
            _animationIsDone = false;
            SetTestValues();

            InitializeComponent();

            FillCaseCard();
            MakeRotationAction();

        }

        public void SetTestValues()
        {
            (_imgs, _names) = ThingForTest.GetParamsForCaseRoullete();
        }

        public void MakeRotationAction()
        {
            int cardId = 1;
            const int endPoint = -10000;
            int completeAnimsCount = 1;
            foreach (UIElement element in CaseRotator.Children)
            {
                var transform = new TranslateTransform();
                element.RenderTransform = transform;

                var animation = new DoubleAnimation
                {
                    From = 0,
                    To = endPoint,
                    Duration = TimeSpan.FromSeconds(4),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                animation.Completed += (s, e) =>
                {
                    if (completeAnimsCount  == amountOfItems)
                    {
                        // need to add won message here 
                        ShowPrizeWindow();
                        completeAnimsCount = 1;
                    }

                    //return;
           /*         CaseRotator.Children.Remove(element);
                    CaseRotator.Children.Add(element);*/

                    transform.X = 0;
                    completeAnimsCount++;

                };

                transform.BeginAnimation(TranslateTransform.XProperty, animation);
                SetCaseGotCard(Math.Abs(endPoint), cardId);
                cardId++;
            }
        }

        public void ShowPrizeWindow()
        {
            if (!(_resCard is null))
            {
                CustomMessageBoxWonPrize prize = new CustomMessageBoxWonPrize();
                prize.ShowDialog();
                CloseRoulette();
                return;
            }
            //Just how 
            throw new Exception("Prize card in case is null");
        }

        private void CloseRoulette()
        {
            OpenCase parent = Helper.FindParent<OpenCase>(this);

            this.Visibility = Visibility.Hidden;
            parent.CheckToOpen.Visibility = Visibility.Visible;
            parent.ExitBut.IsEnabled = true;
            parent.OpenCaseBut.Visibility = Visibility.Visible;
        }

        private CaseCard _resCard = null;
        public void SetCaseGotCard(int endPoint, int tempCardId)
        {
            CaseCard card = GetCardByXLocation(endPoint, tempCardId);
            if (!(card is null))
            {
                _resCard = card;
            }
        }

        public CaseCard GetCardByXLocation(int endPoint, int tempCardId)
        {
            CaseCard res = new CaseCard();
            int tempXLoc = tempCardId * _cardWidth + tempCardId * _distBetweenCards;

            if (tempXLoc > endPoint)
            {
                return _cardsToFind[tempCardId - 1];
            }

            return null;
        }


        private const int _cardWidth = 130;
        private const int _cardHeight = 150;
        private const int _distBetweenCards = 10;
        private List<CaseCard> _cardsToFind = new List<CaseCard>();
        private const int amountOfItems = 150;

        public void FillCaseCard()
        {
            for (int i = 0; i < amountOfItems; i++)
            {
                CaseCard item = new CaseCard()
                {
                    Width = _cardWidth,
                    Height = _cardHeight,
                    Margin = new Thickness(0, 0, _distBetweenCards, 0),
                    VerticalAlignment = VerticalAlignment.Top,

                };

                FillCaseCardInRandom(item);

                _cardsToFind.Add(item);
                CaseRotator.Children.Add(item);

                double xValue = _cardWidth * (i + 1) + _distBetweenCards * (i + 1);

                if (xValue > 10000)
                {
                    Console.WriteLine();
                }
            }
        }

        public void SetChosenLine()
        {
            CaseView.Width = Width;
            CaseView.Height = Height;

            ChoseLine.X1 = Width / 2;
            ChoseLine.Y1 = _distBetweenCards;
            ChoseLine.X2 = Width / 2;
            ChoseLine.Y2 = _distBetweenCards + _cardHeight;
        }


        Random rnd = new Random();
        public CaseCard FillCaseCardInRandom(CaseCard card)
        {
            int index = rnd.Next(0, _imgs.Count);

            card.CardImage.Source = _imgs[index].Source;
            card.CardName.Text = _names[index];

            return card;
        }

    }
}
