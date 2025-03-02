using MonopolyDLL.Monopoly.InventoryObjs;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

using MonopolyDLL.DBService;
using System.Dynamic;
using System.CodeDom.Compiler;

using Item = MonopolyDLL.Monopoly.InventoryObjs.Item;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;

namespace MonopolyDLL.Monopoly
{
    public class MonopolySystem
    {
        public User LoggedUser { get; set; }
        public Game MonGame { get; set; }

        private const int _checkId = 1; //before making right login form
        public MonopolySystem()
        {
            LoggedUser = new User();
            LoggedUser = DBQueries.GetPlayerById(_checkId);
            SetUserInventory();
            
            MonGame = new Game(LoggedUser);
        }   

        public void SetUserInventory()
        {
            List<Item> items = DBQueries.GetUserItemsFromInventory(_checkId);
            LoggedUser.Inventory = new UserInventory(items);;

            List<BoxItem> boxItems = DBQueries.GetItemsToUseInGame(_checkId);
            LoggedUser.GameBusses = boxItems;
        }

        public void AddUsingBusInList(BoxItem item)
        {
            LoggedUser.AddBoxItemInUsingList(item);
        }

        public void RemoveBoxItemFromUsingList(BoxItem item)
        {
            LoggedUser.RemoveBoxItemFromList(item);
        }

        public void RemoveBoxItemByStationId(int index)
        {
            LoggedUser.RemoveAddedBusWithGivenId(index);
        }
        
        public BoxItem GetUserInventoryItemByIndex(int index)
        {
            return LoggedUser.GetUsingItemByIndex(index);
        }

        public BoxItem GetUserInventoryItemByName(string name)
        {
            return LoggedUser.GetItemByName(name);
        }

        public bool IfBussWithSuchNameIsUsing(string name)
        {
            return LoggedUser.IfBusWithSuchNameIsUsedInGame(name);
        }
    }
}
