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

        public BoxItem(string name, string imagePath, 
            BusRearity rearity, BusinessType type, int stationId, 
            double multiplier)
        {
            Name = name;
            IsBox = false;
            ImagePath = imagePath;
            Rearity = rearity;
            Type = type;
            StationId = stationId;
            Multiplier = multiplier;
        }
    }
}
