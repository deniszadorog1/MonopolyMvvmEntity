using MonopolyDLL.Monopoly.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.InventoryObjs
{
    public class BoxItem : Item
    {
        public BusRearity Rearity { get; set; }
        public BusinessType Type { get; set; }
        public int StationId { get; set; }
        public double Multiplier { get; set; }

        private int rColor = -1;
        private int gColor = -1;
        private int bColor = -1;

        private bool _ifTicked;

        public BoxItem(string name, string imagePath,
            BusRearity rearity, BusinessType type, int stationId,
            double multiplier, int r, int g, int b)
        {
            Name = name;
            IsBox = false;
            ImagePath = imagePath;
            Rearity = rearity;
            Type = type;
            StationId = stationId;
            Multiplier = multiplier;
            rColor = r;
            gColor = g;
            bColor = b;
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

        public bool IfBusIsUsual()
        {
            return Type != BusinessType.Games && 
                Type != BusinessType.Cars;

        }
    }
}
