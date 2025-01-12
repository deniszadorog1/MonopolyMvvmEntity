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

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для OpenCase.xaml
    /// </summary>
    public partial class OpenCase : Page
    {
        private CaseRoulette _rol = new CaseRoulette();
        public OpenCase()
        {
            InitializeComponent();

            SetTestCaseDrops();
        }

        public void SetTestCaseDrops()
        {
            List<CaseCard> testCards = ThingForTest.GetTestCaseCards();
        
            for(int i = 0; i < testCards.Count; i++)
            {
                CanBeDropedPanel.Children.Add(testCards[i]);
            }
        }

        private void OpenCaseBut_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ExitBut.IsEnabled = false;
            OpenCaseBut.Visibility = Visibility.Hidden;
            CheckToOpen.Visibility = System.Windows.Visibility.Hidden;
            _rol = new CaseRoulette(new List<Image>(), new List<string>())
            {
                Background = new SolidColorBrush(Colors.Transparent),
                Width = 600,
                Height = 175,
                VerticalAlignment = System.Windows.VerticalAlignment.Top,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Center
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
            WorkWindow window =  (WorkWindow)Window.GetWindow(this); 
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
