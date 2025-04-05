using MonopolyDLL.Monopoly.Enums;
using System.Collections.Generic;

namespace MonopolyDLL.Monopoly.Cell.Businesses
{
    public class GameBusiness : Business
    {
        public GameBusiness(string name, int price, int depositPrice, int rebuyPrice,
            List<int> paymentLevels, int depositCounter, int level, int ownerIndex, BusinessType type,
            bool isDeposited, int id)
        {
            Name = name;
            Price = price;
            DepositPrice = depositPrice;
            RebuyPrice = rebuyPrice;
            PayLevels = paymentLevels;
            DepositCounter = depositCounter;
            Level = level;
            OwnerIndex = ownerIndex;
            BusinessType = type;
            IsDeposited = isDeposited;
            TempDepositCounter = _depositCounterMax;
            Id = id;
        }
    }
}
