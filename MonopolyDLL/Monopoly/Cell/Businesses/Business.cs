using MonopolyDLL.Monopoly.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography.Pkcs;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.Bus
{
    public class Business : Cell
    {
        public int Price { get; set; }
        public int DepositPrice { get; set; }
        public int RebuyPrice { get; set; }
        public List<int> PayLevels { get; set; }
        public int DepositCounter { get; set; }
        public int Level { get; set; }
        public int OwnerIndex { get; set; }
        public BusinessType BusType { get; set; }
        public bool IsDeposited { get; set; }
        public int TempDepositCounter { get; set; }

        public int _depositCounterMax = SystemParamsService.GetNumByName("MaxDepositCounter");

        public int GetPriceForBus()
        {
            return Price;
        }

        public void ChangeOwner(int newOwnerIndex)
        {
            OwnerIndex = newOwnerIndex;
        }

        public BusinessType GetBusType()
        {
            return BusType;
        }

        public bool IsLevelIsMax()
        {
            return Level == PayLevels.Count - 1;
        }

        public bool IsThereAreNoHouses()
        {
            return Level == 0;
        }

        public int GetAddedHousesAmount(int addAmount)
        {
            return Level + addAmount;
        }

        public int GetTakenHousesAmount(int getAmount)
        {
            return Level - getAmount;
        }

        public int GetLevel()
        {
            return Level;
        }

        public int GetPayMoney()
        {
            return IsDeposited ? 0 : PayLevels[Level];
        }

        public bool IsBusinessIsDeposited()
        {
            return IsDeposited;
        }

        public void DepositBus()
        {
            IsDeposited = true;
        }

        public void RebuyBus()
        {
            IsDeposited = false;
        }

        public int GetRubyPrice()
        {
            return RebuyPrice;
        }

        public int GetDepositMoney()
        {
            return DepositPrice;
        }

        public int GetPaymentByIndex(int index)
        {
            return index < 0 || index > PayLevels.Count - 1 ? 0 : PayLevels[index];
        }

        public void SetBusLevel(int level)
        {
            Level = level;
        }

        public void SetNewDepositCounter()
        {
            TempDepositCounter = _depositCounterMax;
        }

        public int GetDepositCounter()
        {
            return TempDepositCounter;
        }

        public void NewCircleOfDeposit()
        {
            TempDepositCounter--;
        }

        public bool IsDepositCounterIsZero()
        {
            return TempDepositCounter == 0;
        }

        public void ClearBusValues()
        {
            OwnerIndex = SystemParamsService.GetNumByName("NoOwnerIndex");
            TempDepositCounter = _depositCounterMax;
            Level = 0;
            IsDeposited = false;
        }

        public int GetOwnerIndex()
        {
            return OwnerIndex;
        }

        public void SetTempDepositCounter(int counter)
        {
            TempDepositCounter = counter;
        }

    }
}
