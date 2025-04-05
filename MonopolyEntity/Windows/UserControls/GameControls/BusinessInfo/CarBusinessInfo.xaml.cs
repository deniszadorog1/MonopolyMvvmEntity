using MonopolyDLL;
using MonopolyDLL.Monopoly.Cell.Businesses;
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
    /// Логика взаимодействия для CarBusInfo.xaml
    /// </summary>
    public partial class CarBusInfo : UserControl, IBusinessInfoActions
    {
        public CarBusInfo()
        {
            InitializeComponent();
        }

        private const int _secondElementIndex = 1;
        private const int _thirdElementIndex = 2;
        private const int _fourthElementIndex = 3;

        public void SetInfoParams(CarBusiness business)
        {
            BusNameText.Text = business.Name;
            BusType.Text = business.BusinessType.ToString();

            OneFieldMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels.First());
            TwoFieldMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_secondElementIndex]);
            ThreeFieldMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_thirdElementIndex]);
            FourFieldMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_fourthElementIndex]);

            FieldPrice.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.Price);
            DepositPriceText.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.DepositPrice);
            RebuyPrice.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.RebuyPrice);

            CarBusHeader.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
        }

        public Size GetSize()
        {
            return new Size(ActualWidth, ActualHeight);
        }
    }
}
