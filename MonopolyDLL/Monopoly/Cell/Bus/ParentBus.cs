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
    public class ParentBus : CellParent
    {
        public int Price { get; set; }
        public int DepositPrice { get; set; }
        public int RebuyPrice { get; set; }
        public List<int> PayLevels { get; set; }
        public int DepositCounter { get; set; }
        public int Level { get; set; }
        public int OwnerIndex { get; set; }
        public BusinessType BusType { get; set; }
        public bool IfDeposited { get; set; }
        public int TempDepositCounter { get; set; }

        public int _depositCounterMax = SystemParamsServeses.GetNumByName("MaxDepositCounter");

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

        public bool IfLevelIsMax()
        {
            return Level == PayLevels.Count - 1;
        }

        public bool IfThereAreNoHouses()
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
            if (IfDeposited) return 0;
            return PayLevels[Level];
        }

        public bool IfBusinessIsDeposited()
        {
            return IfDeposited;
        }

        public void DepositBus()
        {
            IfDeposited = true;
        }

        public void RebuyBus()
        {
            IfDeposited = false;
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
            return PayLevels[index];
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

        public bool IfDepositCounterIsZero()
        {
            return TempDepositCounter == 0;
        }

        public void ClearBusVals()
        {
            OwnerIndex = -1;
            TempDepositCounter = _depositCounterMax;
            Level = 0;
            IfDeposited = false;
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
