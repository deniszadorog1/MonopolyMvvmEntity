using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.InventoryObjs
{
    public class UserInventory
    {
        public List<Item> InventoryItems { get; set; }

        public UserInventory()
        {
            InventoryItems = new List<Item>();
        }

        public UserInventory(List<Item> inventoryItems)
        {
            InventoryItems = inventoryItems;
        }
    }
}
