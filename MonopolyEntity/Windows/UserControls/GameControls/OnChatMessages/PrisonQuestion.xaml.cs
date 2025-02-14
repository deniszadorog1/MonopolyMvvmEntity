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

namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages
{
    /// <summary>
    /// Логика взаимодействия для PrisonQuestion.xaml
    /// </summary>
    public partial class PrisonQuestion : UserControl
    {
        public bool _ifPlayerHasEnoughMoney = false;
        private SolidColorBrush _inActiveColor = (SolidColorBrush)Application.Current.Resources["InActiveColor"];
        bool _ifGiveUp;

        public PrisonQuestion(bool ifPlayerHasEnoughMoney, bool ifOnlyGiveUp)
        {
            _ifPlayerHasEnoughMoney = ifPlayerHasEnoughMoney;
            _ifGiveUp = ifOnlyGiveUp;
            InitializeComponent();

            SetPayButton(_ifPlayerHasEnoughMoney);

            SetGiveUpButton();
        }

        public void SetGiveUpButton()
        {
            if (!_ifGiveUp) return;
            GiveUpBut.Visibility = Visibility.Visible;
        }

        public void SetPayButton(bool ifPlayerHasEnoughMoney)
        {
            if (ifPlayerHasEnoughMoney)
            {
                LastPay.Background = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];
                PayBut.Background = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];
                //LockImage.Visibility = Visibility.Hidden;
                return;
            }
            LastPay.Background = _inActiveColor;
            PayBut.Background = _inActiveColor;
            // LockImage.Visibility = Visibility.Visible;
        }

        public void SetEnoughMoneyButsVisibility()
        {
            ActiveButs.Visibility = Visibility.Visible;
            LastPay.Visibility = Visibility.Hidden;
        }

        public void SetNotEnoughMoneyButsVisibility()
        {
            ActiveButs.Visibility = Visibility.Hidden;
            LastPay.Visibility = Visibility.Visible;
        }

    }
}
