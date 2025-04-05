using MonopolyDLL.Monopoly.Enums;
using System;
using System.Collections.Generic;

namespace MonopolyDLL.Monopoly.Cell.Businesses
{
    public class RegularBusiness : Business
    {
        public int BuySellHouse { get; set; }

        public RegularBusiness(string name, int price, int depositPrice, int rebuyPrice, List<int> paymentLevels,
            int depositCounter, int level, int buySellHouse, int ownerIndex, BusinessType type,
            bool isDeposited, int id)
        {
            Name = name;
            Price = price;
            DepositPrice = depositPrice;
            RebuyPrice = rebuyPrice;
            PayLevels = paymentLevels;
            DepositCounter = depositCounter;
            Level = level;
            BuySellHouse = buySellHouse;
            OwnerIndex = ownerIndex;
            BusinessType = type;
            IsDeposited = isDeposited;
            TempDepositCounter = _depositCounterMax;
            Id = id;
        }

        public int GetHousePrice()
        {
            return BuySellHouse;
        }

        public void BuyHouse()
        {
            ++Level;
        }

        public void SellHouse()
        {
            if (Level == 0) throw new Exception("Level can be lower than 0");
            --Level;
        }

        public int GetPriceForBuiltHouses()
        {
            return (BuySellHouse * Level);
        }

        public bool IsBusinessLevelIsMax()
        {
            return Level == PayLevels.Count - 1;
        }
    }
}
