using System.Collections.Generic;

namespace MonopolyDLL.Monopoly.TradeAction
{
    public class TradePerformance
    {
        public int SenderIndex { get; set; }
        public int ReceiverIndex { get; set; }

        public int SenderMoney { get; set; }
        public int ReceiverMoney { get; set; }

        public List<int> SenderBusinessesIndexes { get; set; }
        public List<int> ReceiverBusinessesIndexes { get; set; }

        public TradePerformance()
        {
            SenderIndex = -1;
            ReceiverIndex = -1;

            SenderMoney = 0;
            ReceiverMoney = 0;

            SenderBusinessesIndexes = new List<int>();
            ReceiverBusinessesIndexes = new List<int>();
        }

        public void SetSenderIndex(int senderIndex)
        {
            SenderIndex = senderIndex;
        }

        public void SetReceiverIndex(int receiverIndex)
        {
            ReceiverIndex = receiverIndex;
        }

        public void AddCellIndexInTrade(int cellIndex, bool isSenderIsOwner)
        {
            if (isSenderIsOwner) SenderBusinessesIndexes.Add(cellIndex);
            else ReceiverBusinessesIndexes.Add(cellIndex);
        }

        public void RemoveCellIndexFromTrade(int cellIndex)
        {
            SenderBusinessesIndexes.Remove(cellIndex);
            ReceiverBusinessesIndexes.Remove(cellIndex);
        }

        public int GetSenderMoney()
        {
            return SenderMoney;
        }

        public List<int> GetSenderCellsIndexes()
        {
            return SenderBusinessesIndexes;
        }

        public int GetReceiverMoney()
        {
            return ReceiverMoney;
        }

        public List<int> GetReceiverIndexes()
        {
            return ReceiverBusinessesIndexes;
        }

        public bool IsTwiceRuleIsCompleted(int totalSender, int totalReceiver)
        {
            const int twiceRuleValue = 2;

            return totalSender / twiceRuleValue <= totalReceiver &&
                totalReceiver / twiceRuleValue <= totalSender;

            /*            return !(totalSender / twiceRuleValue > totalReceiver) && 
                            !(totalReceiver / twiceRuleValue > totalSender);*/
        }

        public void SetSenderMoney(int money)
        {
            SenderMoney = money;
        }

        public void SetReceiverMoney(int money)
        {
            ReceiverMoney = money;
        }

        public int GetSenderIndex()
        {
            return SenderIndex;
        }

        public int GetReceiverIndex()
        {
            return ReceiverIndex;
        }

    }
}
