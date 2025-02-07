using MonopolyDLL.Monopoly.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.Bus
{
    public class UsualBus : ParentBus
    {
        public int BuySellHouse { get; set; }

        public UsualBus(string name, int price, int depositPrice, int rebuyPrice,List<int> paymentLevels, 
            int depositCounter, int level, int buySellHouse, int ownerIndex, BusinessType type,
            bool ifDeposited, int id)
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
            BusType = type;
            IfDeposited = ifDeposited;
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
            --Level;
            if (Level < 0) throw new Exception("Level can be lower than 0");
        }

        public int GetPriceForBuiltHouses()
        {
            return (Level * Level);
        }

    }
}
