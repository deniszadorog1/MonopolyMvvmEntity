using MonopolyDLL.Monopoly.Enums;
using System.Collections.Generic;

namespace MonopolyDLL.Monopoly.InventoryObjs
{
    public class BoxItem : Item
    {
        public BusinessRarity Rarity { get; set; }
        public BusinessType Type { get; set; }
        public int StationId { get; set; }
        public double Multiplier { get; set; }

        private int rColor = -1;
        private int gColor = -1;
        private int bColor = -1;

        private bool _ifTicked;
        private int _caseCardId;
        private int _inventoryIdInDB;

        public BoxItem(string name, string imagePath,
            BusinessRarity rarity, BusinessType type, int stationId,
            double multiplier, int r, int g, int b)
        {
            Name = name;
            IsBox = false;
            ImagePath = imagePath;
            Rarity = rarity;
            Type = type;
            StationId = stationId;
            Multiplier = multiplier;
            rColor = r;
            gColor = g;
            bColor = b;
        }

        public BoxItem()
        {
            Name = string.Empty;
            IsBox = false;
            ImagePath = string.Empty;
            Rarity = BusinessRarity.Usual;
            Type = BusinessType.Perfume;
            Multiplier = 0;
            rColor = 0;
            gColor = 0;
            bColor = 0;
        }

        public void SetTick(bool? tick)
        {
            if (tick is null) return;
            _ifTicked = (bool)tick;
        }

        public bool IsTicked()
        {
            return _ifTicked;
        }

        public byte GetRParam()
        {
            return (byte)rColor;
        }

        public byte GetGParam()
        {
            return (byte)gColor;
        }

        public byte GetBParam()
        {
            return (byte)bColor;
        }

        public bool IsBusIsUsual()
        {
            return Type != BusinessType.Games &&
                Type != BusinessType.Cars;
        }

        public void SetCaseCardId(int id)
        {
            _caseCardId = id;
        }

        public int GetCaseCardId()
        {
            return _caseCardId;
        }

        public void SetInventoryId(int id)
        {
            _inventoryIdInDB = id;
        }

        public int GetInventoryIdInDB()
        {
            return _inventoryIdInDB;
        }

        public List<int> GetNewPaymentList(List<int> payments)
        {
            const int paymentMultiplier = 100;
            const int paymentAdder = 1;
            List<int> res = new List<int>();

            double multiplier = Multiplier / paymentMultiplier + paymentAdder;

            for (int i = 0; i < payments.Count; i++)
            {
                res.Add((int)(payments[i] * multiplier));
            }
            return res;
        }
    }
}
