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
        public int BuySellHouse;

        public UsualBus(string name, int price, int depositPrice, int rebuyPrice,List<int> paymentLevels, 
            int depositCounter, int level, int buySellHouse, int ownerIndex, BusinessType type)
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
        }

    }
}
