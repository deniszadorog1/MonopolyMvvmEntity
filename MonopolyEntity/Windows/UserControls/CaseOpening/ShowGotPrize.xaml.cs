﻿using MonopolyDLL;
using MonopolyDLL.Monopoly.InventoryObjs;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.UserControls.CaseOpening
{
    /// <summary>
    /// Логика взаимодействия для ShowGotPrize.xaml
    /// </summary>
    public partial class ShowGotPrize : UserControl
    {
        private BoxItem _prize;
        private Image _prizeImg;
        
        public ShowGotPrize(BoxItem item, Image img)
        {
            _prize = item;
            _prizeImg = img;
            InitializeComponent();

            FillParams();
        }

        public ShowGotPrize(Image img)
        {
            _prizeImg = img;
            InitializeComponent();

            PrizeImg.Source = img.Source;
            HeadText.Text = SystemParamsService.GetStringByName("GotWinner");//  "We have a winner!";
            AcceptBut.Content = SystemParamsService.GetStringByName("CloseWindow");// "Close this window";
        }

        public void FillParams()
        {
            PrizeImg.Source = _prizeImg.Source;
            NameText.Text = $"{SystemParamsService.GetStringByName("CaseOpenPrizeMessage")}" +
                $" {_prize.Name}";
        }
    }
}
