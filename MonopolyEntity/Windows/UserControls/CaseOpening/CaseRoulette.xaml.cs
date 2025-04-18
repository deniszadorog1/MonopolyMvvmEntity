﻿using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using MaterialDesignThemes.Wpf;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;

namespace MonopolyEntity.Windows.UserControls.CaseOpening
{
    /// <summary>
    /// Логика взаимодействия для CaseRoulette.xaml
    /// </summary>
    public partial class CaseRoulette : UserControl
    {
        private List<Image> _images;
        private List<string> _names;
        private List<SolidColorBrush> _colors;

        public bool _animationIsDone = false;

        public CaseRoulette()
        {
            _images = new List<Image>();
            _names = new List<string>();
            _colors = new List<SolidColorBrush>();

            _animationIsDone = false;
            SetTestValues();

            InitializeComponent();

            FillCaseCard();
            MakeRotationAction();

            //SetTestValues();
        }

        public CaseRoulette(List<Image> caseImgs,
            List<string> itemNames, List<SolidColorBrush> colors)
        {
            _images = caseImgs;
            _names = itemNames;
            _colors = colors;

            _animationIsDone = false;
            //SetTestValues();

            InitializeComponent();

            FillCaseCard();
            MakeRotationAction();
        }

        public void SetTestValues()
        {
            (_images, _names) = ThingForTest.GetParamsForCaseRoullete();
        }

        public DoubleAnimation _animation;
        public void MakeRotationAction()
        {
            int cardId = 1;
            const int endPoint = -10000;
            int completeAnimationCount = 1;
            const int animationDuration = 4;
            const int baseAnimCount = 1;

            foreach (UIElement element in CaseRotator.Children)
            {
                var transform = new TranslateTransform();
                element.RenderTransform = transform;

                _animation = new DoubleAnimation
                {
                    From = 0,
                    To = endPoint,
                    Duration = TimeSpan.FromSeconds(animationDuration),
                    EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
                };

                _animation.Completed += (s, e) =>
                {
                    if (completeAnimationCount == amountOfItems)
                    {
                        Console.WriteLine(_resCard.CardName.Text);
                        ShowPrizeWindow();
                        completeAnimationCount = baseAnimCount;
                        _animationIsDone = true;
                    }
                    transform.X = 0;
                    completeAnimationCount++;
                };

                transform.BeginAnimation(TranslateTransform.XProperty, _animation);
                SetCaseGotCard(Math.Abs(endPoint), cardId);
                cardId++;
            }
        }

        public void ShowPrizeWindow()
        {
            if (!(_resCard is null))
            {
                CloseRoulette();
                return;
            }
            throw new Exception("Prize card in case is null");
        }

        private void CloseRoulette()
        {
            OpenCase parent = Helper.FindParent<OpenCase>(this);
            if (parent is null) return;

            this.Visibility = Visibility.Hidden;
            parent.CheckToOpen.Visibility = Visibility.Visible;
            parent.ExitBut.IsEnabled = true;
            parent.OpenCaseBut.Visibility = Visibility.Visible;
        }

        public CaseCard _resCard = null;
        public void SetCaseGotCard(int endPoint, int tempCardId)
        {
            if (!(_resCard is null)) return;

            CaseCard card = GetCardByXLocation(endPoint, tempCardId);
            if (_resCard is null && !(card is null))
            {
                _resCard = card;
            }
            
        }

        private const int _centerWidth = _cardWidth * _centerDivider + _distanceBetweenCards * _centerDivider;
        public CaseCard GetCardByXLocation(int endPoint, int tempCardId)
        {
            int tempXLoc = tempCardId * _cardWidth + tempCardId * _distanceBetweenCards;

            if (tempXLoc > endPoint + _centerWidth)
            {
                Console.WriteLine(_cardsToFind[tempCardId ].CardName.Text);
                return _cardsToFind[tempCardId - 1];
            }
            return null;
        }

        private const int _cardWidth = 130;
        private const int _cardHeight = 150;
        private const int _distanceBetweenCards = 10;
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
                    Margin = new Thickness(0, 0, _distanceBetweenCards, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                };

                FillCaseCardInRandom(item);

                _cardsToFind.Add(item);
                CaseRotator.Children.Add(item);
            }
        }

        private const int _centerDivider = 2; 
        public void SetChosenLine()
        {
            CaseView.Width = Width;
            CaseView.Height = Height;

            ChoseLine.X1 = Width / _centerDivider;
            ChoseLine.Y1 = _distanceBetweenCards;
            ChoseLine.X2 = Width / _centerDivider;
            ChoseLine.Y2 = _distanceBetweenCards + _cardHeight;
        }

        Random rnd = new Random();
        public CaseCard FillCaseCardInRandom(CaseCard card)
        {
            int index = rnd.Next(0, _images.Count);

            card.CardImage.Source = _images[index].Source;
            card.CardName.Text = _names[index];
            card.BorderBgColor.Background = _colors[index];

            return card;
        }

    }
}
