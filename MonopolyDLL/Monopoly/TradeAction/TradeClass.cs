using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.TradeAction
{
    public class TradeClass
    {
        public int SenderIndex { get; set; }
        public int ReciverIndex { get; set; }

        public int SenderMoney { get; set; }
        public int ReciverMoney { get; set; }

        public List<int> SenderBusesIndexes { get; set; }
        public List<int> ReciverBusesIndexes { get; set; }
    
        public TradeClass()
        {
            SenderIndex = -1;
            ReciverIndex = -1;

            SenderMoney = 0;
            ReciverMoney = 0;

            SenderBusesIndexes = new List<int>();
            ReciverBusesIndexes = new List<int>();
        }

        public void SetSenderIndex(int senderIndex)
        {
            SenderIndex = senderIndex;
        }

        public void SetReciverIndex(int reciverIndex)
        {
            ReciverIndex = reciverIndex;
        }

        public void AddCellIndexInTrade(int cellIndex, bool ifSenderIsOwner)
        {
            if (ifSenderIsOwner) SenderBusesIndexes.Add(cellIndex);
            else ReciverBusesIndexes.Add(cellIndex);
        }

        public void RemoveCellIndexFromTrade(int cellIndex)
        {
            SenderBusesIndexes.Remove(cellIndex);
            ReciverBusesIndexes.Remove(cellIndex);
        }

        public int GetSenderMoney()
        {
            return SenderMoney;
        }

        public List<int> GetSenderCellsIndexes()
        {
            return SenderBusesIndexes;
        }

        public int GetReciverMoney()
        {
            return ReciverMoney;
        }

        public List<int> GetReciverIndexes()
        {
            return ReciverBusesIndexes;
        }

        public bool IfTwiceRuleIsComplited(int totalSender, int totalReciver)
        {
            const int twiceRuleValue = 2;
            return !(totalSender / twiceRuleValue > totalReciver) && 
                !(totalReciver / twiceRuleValue > totalSender);
        }

        public void SetSenderMoney(int money)
        {
            SenderMoney = money;
        }

        public void SetReciverMoney(int money)
        {
            ReciverMoney = money;
        }
    }
}
