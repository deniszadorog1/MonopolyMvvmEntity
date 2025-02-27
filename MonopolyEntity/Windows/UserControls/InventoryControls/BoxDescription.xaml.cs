﻿using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MonopolyDLL.Monopoly.InventoryObjs;

namespace MonopolyEntity.Windows.UserControls.InventoryControls
{
    /// <summary>
    /// Логика взаимодействия для ItemDescription.xaml
    /// </summary>
    public partial class BoxDescription : UserControl
    {
        private Frame _frame;
        private CaseBox _caseBox;
        private string _loggedUserLogin;

        public BoxDescription(Frame workingFrame, CaseBox item, string loggedUserLogin)
        {
            _frame = workingFrame;
            _caseBox = item;
            _loggedUserLogin = loggedUserLogin;

            InitializeComponent();

            SetTextParams();
        }

        public void SetTextParams()
        {
            ItemName.Text = _caseBox.Name;
            ItemType.Text = "LotBox";
            ItemDesctiption.Text = "This is box which you can open";
            ColType.Text = "The same as box name btw";
            CanBeDroppedDescription.Text = "Several items from the worst type to the best";
        }

        private void OpenCaseBut_Click(object sender, RoutedEventArgs e)
        {
            //return;
            MainWindow obj = 
                Helper.FindParent<MainWindow>(_frame);

            OpenCase inventory = new OpenCase(_caseBox, _loggedUserLogin);
            //_frame.Content = inventory;

            obj.CaseFrame.Content = inventory;

            BlurEffect blurEffect = new BlurEffect
            {
                Radius = 100 
            };
            obj.VisiableItems.Effect = blurEffect;

            _frame.Effect = blurEffect;
            obj.CaseFrame.Visibility = Visibility.Visible;
        }
    }
}
