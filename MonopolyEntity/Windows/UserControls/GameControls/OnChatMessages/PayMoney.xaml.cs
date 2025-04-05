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
    /// Логика взаимодействия для PayMoney.xaml
    /// </summary>
    public partial class PayMoney : UserControl
    {
        public bool _ifPlayerHasEnoughMoney = false;
        public bool _ifGiveUp = false;
        private SolidColorBrush _inActiveColor = (SolidColorBrush)Application.Current.Resources["InActiveColor"];

        public PayMoney(bool isPlayerHasEnoughMoney, bool isGiveUp)
        {
            _ifPlayerHasEnoughMoney = isPlayerHasEnoughMoney;
            _ifGiveUp = isGiveUp;

            InitializeComponent();
            SetPayButton(_ifPlayerHasEnoughMoney, _ifGiveUp);
            
            SetGiveUpButton();
        }

        public void SetGiveUpButton()
        {
            if (!_ifGiveUp) return;
            SetButtonsVisibility(true);
            //SetVisToGiveUp();
        }

        public void SetButtonsVisibility(bool ifGiveUp)
        {
            PayBillBut.Visibility = ifGiveUp ? Visibility.Hidden : Visibility.Visible;
            GiveUpBut.Visibility = ifGiveUp ? Visibility.Visible : Visibility.Hidden;
        }

/*        public void SetPayButtons()
        {
            PayBillBut.Visibility = Visibility.Visible;
            GiveUpBut.Visibility = Visibility.Hidden;
        }*/

        public void SetPayButton(bool isPlayerHasEnoughMoney, bool isPLayerCanOnlyGiveUp)
        {
            if (isPLayerCanOnlyGiveUp)
            {
                SetButtonsVisibility(true);
                //SetVisToGiveUp();
                return;
            }

            SetButtonsVisibility(false);
            //SetPayButtons();

            if (isPlayerHasEnoughMoney)
            {
                PayBillBut.Background = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];
                //LockImage.Visibility = Visibility.Hidden;
                return;
            }
            PayBillBut.Background = _inActiveColor;
            LockImage.Visibility = Visibility.Visible;
        }

/*        private void SetVisToGiveUp()
        {
            PayBillBut.Visibility = Visibility.Hidden;
            GiveUpBut.Visibility = Visibility.Visible;
        }*/

    }
}
