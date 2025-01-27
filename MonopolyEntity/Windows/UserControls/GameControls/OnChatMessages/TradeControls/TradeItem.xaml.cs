using MonopolyDLL.Monopoly.Cell.Bus;
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
    /// Логика взаимодействия для TradeItem.xaml
    /// </summary>
    public partial class TradeItem : UserControl
    {
        public TradeItem()
        {
            InitializeComponent();
        }

        public TradeItem(ParentBus bus, Image busImg)
        {
            InitializeComponent();

            ItemName.Text = bus.Name;
            ItemType.Text = bus.Price.ToString();

            Itemimg.Source = busImg.Source;
        }

    }
}
