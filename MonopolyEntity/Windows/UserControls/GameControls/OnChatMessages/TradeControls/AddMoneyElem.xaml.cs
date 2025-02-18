using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls
{
    /// <summary>
    /// Логика взаимодействия для AddMoneyElem.xaml
    /// </summary>
    public partial class AddMoneyElem : UserControl
    {
        public AddMoneyElem()
        {
            InitializeComponent();
        }
        private void AmountOfMoneyBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$");
        }

        private int _maxMoney;
        private string _prevAcceptedMoney = string.Empty;
        private void AmountOfMoneyBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(AmountOfMoneyBox.Text, out int money);

            if (_maxMoney < money)
            {
                AmountOfMoneyBox.Text = _prevAcceptedMoney;
            }
            else _prevAcceptedMoney = AmountOfMoneyBox.Text;
        }

        public void SetMaxMoney(int money)
        {
            _maxMoney = money;
        }
    }
}
