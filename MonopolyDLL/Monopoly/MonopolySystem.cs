using MonopolyDLL.Monopoly.InventoryObjs;
using System.Collections.Generic;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;
using Item = MonopolyDLL.Monopoly.InventoryObjs.Item;

namespace MonopolyDLL.Monopoly
{
    public class MonopolySystem
    {
        public User LoggedUser { get; set; }
        public Game MonGame { get; set; }

        private  int _checkId = 1;
        public MonopolySystem()
        {
            LoggedUser = new User();
            LoggedUser = DBQueries.GetPlayerById(_checkId);
            SetUserInventory();

            MonGame = new Game(LoggedUser);
        }

        public MonopolySystem(int id)
        {
            _checkId = id;

            LoggedUser = new User();
            LoggedUser = DBQueries.GetPlayerById(_checkId);
            SetUserInventory();

            MonGame = new Game(LoggedUser);
        }

        public void SetUserInventory()
        {
            List<Item> items = DBQueries.GetUserItemsFromInventory(_checkId);
            LoggedUser.Inventory = new UserInventory(items);

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

        public bool IsBussWithSuchNameIsUsing(string name)
        {
            return LoggedUser.IsBusWithSuchNameIsUsedInGame(name);
        }
    }
}
