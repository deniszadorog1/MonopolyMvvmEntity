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
    /// Логика взаимодействия для BuyBusiness.xaml
    /// </summary>
    public partial class BuyBusiness : UserControl
    {
        public bool _ifPlayerHasEnoughMoney = false;
        private SolidColorBrush _inActiveColor = (SolidColorBrush)Application.Current.Resources["InActiveColor"];

        public BuyBusiness(bool ifHasMoney)
        {
            _ifPlayerHasEnoughMoney = ifHasMoney;
            InitializeComponent();

            SetBuyButton(_ifPlayerHasEnoughMoney);
        }

        public void SetBuyButton(bool ifPlayerHasEnoughMoney)
        {
            if (ifPlayerHasEnoughMoney)
            {
                BuyBusBut.Background = (SolidColorBrush)Application.Current.Resources["MainGlobalColor"];
                //LockImage.Visibility = Visibility.Hidden;
                return;
            }
            BuyBusBut.Background = _inActiveColor;
            LockImage.Visibility = Visibility.Visible;
        }
    }
}
