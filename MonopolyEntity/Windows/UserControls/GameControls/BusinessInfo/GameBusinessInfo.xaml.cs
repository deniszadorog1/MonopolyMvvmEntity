using MonopolyDLL.Monopoly.Cell.Businesses;
using MonopolyDLL;
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
using MonopolyDLL.Services;
using MonopolyEntity.Interfaces;

namespace MonopolyEntity.Windows.UserControls.GameControls.BusinessInfo 
{
    /// <summary>
    /// Логика взаимодействия для GameBusInfo.xaml
    /// </summary>
    public partial class GameBusInfo : UserControl, IBusinessInfoActions
    {
        public GameBusInfo()
        {
            InitializeComponent();
        }

        public void SetInfoParams(GameBusiness business)
        {
            const int secondIndex = 1;

            BusName.Text = business.Name;
            BusType.Text = business.BusinessType.ToString();

            BusDescription.Text = SystemParamsService.GetStringByName("GameBusInfoDesc");

            OneFieldMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels.First());
            TwoFieldMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[secondIndex]);

            FieldPrice.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.Price);
            DepositPriceText.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.DepositPrice);
            RebuyPrice.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.RebuyPrice);

            GameBusHeader.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];
        }

        public Size GetSize()
        {
            return new Size(ActualWidth, ActualHeight);
        }
    }
}
