using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.InventoryObjs
{
    public class UserInventory
    {
        public List<Item> InventoryItems { get; set; }
        public List<BoxItem> TickedItems { get; set; }

        public UserInventory()
        {
            InventoryItems = new List<Item>();
            SetTickedItems();
        }

        public UserInventory(List<Item> inventoryItems)
        {
            InventoryItems = inventoryItems;
            SetTickedItems();
        }

        public void SetTickedItems()
        {
            TickedItems = InventoryItems.OfType<BoxItem>().Where(x => x.IsTicked()).ToList();
        }

    }
}
