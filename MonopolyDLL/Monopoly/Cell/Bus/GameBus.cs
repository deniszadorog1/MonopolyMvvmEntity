using MonopolyDLL.Monopoly.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.Bus
{
    public class GameBus : ParentBus
    {
        public GameBus(string name, int price, int depositPrice, int rebuyPrice,
            List<int> paymentLevels, int depositCounter, int level, int ownerIndex, BusinessType type,
            bool ifDeposited, int id)
        {
            Name = name;
            Price = price;
            DepositPrice = depositPrice;
            RebuyPrice = rebuyPrice;
            PayLevels = paymentLevels;
            DepositCounter = depositCounter;
            Level = level;
            OwnerIndex = ownerIndex;
            BusType = type;
            IfDeposited = ifDeposited;
            TempDepositCounter = _depositCounterMax;
            Id = id;
        }



    }
}
