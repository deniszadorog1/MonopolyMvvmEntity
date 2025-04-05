using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.Cell.Businesses;
using MonopolyDLL.Services;
using MonopolyEntity.Interfaces;
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

namespace MonopolyEntity.Windows.UserControls.GameControls.BusinessInfo 
{
    /// <summary>
    /// Логика взаимодействия для UsualBusInfo.xaml
    /// </summary>
    public partial class RegularBusInfo : UserControl, IBusinessInfoActions
    {
        public RegularBusInfo()
        {
            InitializeComponent();
        }

        private const int _firstElementIndex = 0;
        private const int _secondElementIndex = 1;
        private const int _thirdElementIndex = 2;
        private const int _fourthElementIndex = 3;
        private const int _fifthElementIndex = 4;
        private const int _sixthElementIndex = 5;

        public void SetInfoParams(RegularBusiness business)
        {
            BusName.Text = business.Name;
            BusType.Text = business.BusinessType.ToString();

            BaseRentMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_firstElementIndex]);
            OneStarRentMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_secondElementIndex]);
            TwoStarRentMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_thirdElementIndex]);
            ThreeStarRentMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_fourthElementIndex]);
            FourStarRentMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_fifthElementIndex]);
            YellowStarRentMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.PayLevels[_sixthElementIndex]);

            BusPriceMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.Price);
            DepositPriceMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.DepositPrice);
            RebuyPriceMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.RebuyPrice);
            HousePriceMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(business.BuySellHouse);
            NameBusBorder.Background = GetColorForUsualBusHeader(business);
        }

        private SolidColorBrush GetColorForUsualBusHeader(RegularBusiness bus)
        {
            return bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Perfume ? (SolidColorBrush)Application.Current.Resources["PerfumeColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Clothes ? (SolidColorBrush)Application.Current.Resources["ClothesColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Messengers ? (SolidColorBrush)Application.Current.Resources["MessagerColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Drinks ? (SolidColorBrush)Application.Current.Resources["DrinkColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Planes ? (SolidColorBrush)Application.Current.Resources["PlaneColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Food ? (SolidColorBrush)Application.Current.Resources["FoodColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Hotels ? (SolidColorBrush)Application.Current.Resources["HotelColor"] :
                bus.BusinessType == MonopolyDLL.Monopoly.Enums.BusinessType.Phones ? (SolidColorBrush)Application.Current.Resources["PhoneColor"] :
                throw new Exception("No such business type...How is it possible?");

/*
            if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Perfume)
            {
                return (SolidColorBrush)Application.Current.Resources["PerfumeColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Clothes)
            {
                return (SolidColorBrush)Application.Current.Resources["ClothesColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Messengers)
            {
                return (SolidColorBrush)Application.Current.Resources["MessagerColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Drinks)
            {
                return (SolidColorBrush)Application.Current.Resources["DrinkColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Planes)
            {
                return (SolidColorBrush)Application.Current.Resources["PlaneColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Food)
            {
                return (SolidColorBrush)Application.Current.Resources["FoodColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Hotels)
            {
                return (SolidColorBrush)Application.Current.Resources["HotelColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Phones)
            {
                return (SolidColorBrush)Application.Current.Resources["PhoneColor"];
            }

            throw new Exception("No such business type...How is it possible?");*/
        }

        public Size GetSize()
        {
            return new Size(ActualWidth, ActualHeight);
        }

    }
}
