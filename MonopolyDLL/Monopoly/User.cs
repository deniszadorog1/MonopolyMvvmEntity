using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using MonopolyDLL.DBService;
using MonopolyDLL.Monopoly.Cell.Bus;
using MonopolyDLL.Monopoly.Enums;
using MonopolyDLL.Monopoly.InventoryObjs;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;

namespace MonopolyDLL.Monopoly
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int AmountOfMoney { get; set; }
        public int Position { get; set; }
        public bool IfSitInPrison { get; set; }
        public int SitInPrisonCounter { get; set; }

        public UserInventory Inventory { get; set; }
        public List<BoxItem> GameBusses { get; set; }

        public bool IfLost { get; set; }
        public int DoubleCounter { get; set; }

        public List<BusinessType> BuiltHousesInRowType { get; set; }         
        private List<BusinessType> _collectedMonopolies = new List<BusinessType>();

        private int Id;
        private int _maxDoubles = 3;

        public User(string login, int id)
        {
            Login = login;
            Id = id;
            GameBusses = new List<InventoryObjs.BoxItem>();
            AmountOfMoney = 15000;
            Position = 0;
            IfSitInPrison = false;
            BuiltHousesInRowType = new List<BusinessType>();
            IfLost = false;
            DoubleCounter = 0;
            GameBusses = new List<InventoryObjs.BoxItem>();
        }

        public User()
        {
            Login = string.Empty;
            AmountOfMoney = 15000;
            Position = 0;
            IfSitInPrison = false;
            BuiltHousesInRowType = new List<BusinessType>();
            IfLost = false;
            DoubleCounter = 0;
            GameBusses = new List<InventoryObjs.BoxItem>();
        }

        public User(string login, int amountOfMoney, int position, bool ifLost)
        {
            Login = login;
            AmountOfMoney = amountOfMoney;
            Position = position;
            IfSitInPrison = false;
            BuiltHousesInRowType = new List<BusinessType>();
            IfLost = ifLost;
            DoubleCounter = 0;
            GameBusses = new List<InventoryObjs.BoxItem>();
        }

        public void AddToDoubleCounter()
        {
            DoubleCounter++;
        }

        public int GetDoubleCounter()
        {
            return DoubleCounter;
        }

        public void PayMoney(int money)
        {
            AmountOfMoney -= money;
        }

        public void GetMoney(int money)
        {
            AmountOfMoney += money;
        }

        public bool IfPlayersInCellIndex(int cellIndex)
        {
            return Position == cellIndex;
        }

        public bool IfPlayerSitsInPrison()
        {
            return IfSitInPrison;
        }

        public void ClearSitInPrisonCounter()
        {
            SitInPrisonCounter = 0;
        }

        public void MakePrisonCounterHigher()
        {
            SitInPrisonCounter++;
        }

        public void ReverseSitInPrison()
        {
            IfSitInPrison = !IfSitInPrison;
        }

        public int GetPrisonCounter()
        {
            return SitInPrisonCounter;
        }

        public List<BusinessType> GetCollectedMonopolys()
        {
            return _collectedMonopolies;
        }

        public void AddMonopoly(BusinessType type)
        {
            if (_collectedMonopolies.Contains(type)) return;
            _collectedMonopolies.Add(type);
        }

        public void RemoveFromCollectedMonopolies(BusinessType type)
        {
            _collectedMonopolies.Remove(type);
        }

        public void ClearCollectedMonopolies()
        {
            _collectedMonopolies.Clear();
        }

        public bool IfPlayerHasEnoughMoney(int money)
        {
            return AmountOfMoney >= money;
        }

        public bool IfMaxDoublesIsAchieved()
        {
            return _maxDoubles <= DoubleCounter;
        }

        public void ClearDoubleCounter()
        {
            DoubleCounter = 0;
        }

        public bool IfBusWithSuchIdIsUsedInGame(int stationId)
        {
            return GameBusses.Any(x => x.StationId == stationId);
        }

        public InventoryObjs.BoxItem GetItemWhichUsesInGameById(int id)
        {
            return GameBusses.Any(x => x.StationId == id) ?
                GameBusses.Find(x => x.StationId == id) : null;
        }

        public bool IfBusWithSuchNameIsUsedInGame(string name)
        {
            return GameBusses.Any(x => x.Name == name);
        }

        public InventoryObjs.BoxItem GetItemByName(string name)
        {
            return GameBusses.Find(x => x.Name == name);
        }

        public void AddBoxItemInUsingList(InventoryObjs.BoxItem item)
        {
            GameBusses.Add(item);
        }

        public void RemoveBoxItemFromList(InventoryObjs.BoxItem item)
        {
            GameBusses.Remove(GameBusses.Find(x => x.Name == item.Name));
        }

        public InventoryObjs.BoxItem GetUsingItemByIndex(int index)
        {
            return GameBusses[index];
        }

        public bool IfHasInventoryOnPosition()
        {
            return GameBusses.Where(x => x.StationId == Position).Any();
        }

        public bool IfHasInventoryItemOnCellIndex(int cellIndex)
        {
            return GameBusses.Where(x => x.StationId == cellIndex).Any();
        }

        public ParentBus GetInventoryItemById(int position, ParentBus ususalPosBus, int newOwnerIndex)
        {
            BoxItem item = GameBusses.Where(x => x.StationId == position).First();

            const int usCounter = 15;
            
            if (item.Type == BusinessType.Cars)
            {
                return new CarBus(item.Name, ususalPosBus.Price, ususalPosBus.DepositPrice, 
                    ususalPosBus.RebuyPrice, item.GetNewPaymentList(ususalPosBus.PayLevels), 
                    usCounter, 0, newOwnerIndex, item.Type, false, ususalPosBus.GetId());
            }
            if (item.Type == BusinessType.Games)
            {
                return new GameBus(item.Name, ususalPosBus.Price, ususalPosBus.DepositPrice, 
                    ususalPosBus.RebuyPrice, item.GetNewPaymentList(ususalPosBus.PayLevels), 
                    usCounter, 0, newOwnerIndex, item.Type, false, ususalPosBus.GetId());
            }
            return new UsualBus(item.Name, ususalPosBus.Price, ususalPosBus.DepositPrice, 
                ususalPosBus.RebuyPrice, item.GetNewPaymentList(ususalPosBus.PayLevels), 
                usCounter, 0, ((UsualBus)ususalPosBus).BuySellHouse, newOwnerIndex, 
                item.Type, false, ususalPosBus.GetId());
        }

        public BoxItem GetBoxItemByPosition(int position)
        {
            BoxItem item = GameBusses.Where(x => x.StationId == position).First();
            return item;
        }

    }
}
