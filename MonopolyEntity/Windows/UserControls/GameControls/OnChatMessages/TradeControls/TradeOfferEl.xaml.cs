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

namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls
{
    /// <summary>
    /// Логика взаимодействия для TradeOfferEl.xaml
    /// </summary>
    public partial class TradeOfferEl : UserControl
    {
        public TradeOfferEl()
        {
            InitializeComponent();
        }

        public bool IfTradeItemNameContainsInList(string busName)
        {
            return IfItemNameContainsInListBox(busName, SenderListBox) ||
                IfItemNameContainsInListBox(busName, ReciverListBox);
        }

        public bool IfItemNameContainsInListBox(string name, ListBox box)
        {
            for (int i = 0; i < box.Items.Count; i++)
            {
                if (box.Items[i] is TradeItem item &&
                    item.ItemName.Text == name)
                {
                    return true;
                }
            }
            return false;
        }

        public void UpdateSenderTotalMoney(string money)
        {
            SenderTotalMoney.Text = money;
        }

        public void UpdateReciverTotalMoney(string money)
        {
            ReciverTotalMoney.Text = money;
        }

        public int GetSenderTradeMoney()
        {
            RemoveSpace(SenderMoney.AmountOfMoneyBox.Text, SenderMoney.AmountOfMoneyBox);
            return ConvertMoneyStringInInterger(SenderMoney.AmountOfMoneyBox.Text);
        }

        public void RemoveSpace(string money, TextBox block)
        {
            string res = string.Empty;
            for (int i = 0; i < money.Length; i++)
            {
                if (money[i] != ' ')
                {
                    res += money[i];
                }
            }
            block.Text = res;
        }

        public int GetReciverTradeMoney()
        {
            RemoveSpace(ReciverMoney.AmountOfMoneyBox.Text, ReciverMoney.AmountOfMoneyBox);

            return ConvertMoneyStringInInterger(ReciverMoney.AmountOfMoneyBox.Text);
        }

        private int ConvertMoneyStringInInterger(string money)
        {
            string zeroRemoved = RemoveZerosFromStart(money);
            return int.Parse(zeroRemoved);
        }

        public string RemoveZerosFromStart(string moneyString)
        {
            StringBuilder res = new StringBuilder();
            bool startWrite = false;

            for (int i = 0; i < moneyString.Length; i++)
            {
                if (moneyString[i] != '0') startWrite = true;
                if (startWrite) res.Append(moneyString[i]);
            }

            if (res.Length == 0) res.Append("0");
            return res.ToString();
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SenderScroll.Height = TradeRow.ActualHeight -
                GiverItem.ActualHeight - SenderMoney.ActualHeight;

            RecieverScroll.Height = SenderScroll.Height;
        }
    }
}
