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
using MonopolyDLL.Monopoly.Enums;
using MonopolyDLL.Monopoly.InventoryObjs;

namespace MonopolyDLL.Monopoly
{
    public class User
    {
        public string Login { get; set; }
        public int AmountOfMoney { get; set; }
        public int Position { get; set; }
        public bool IfSitInPrison { get; set; }
        public int SitInPrisonCounter { get; set; }

        public UserInventory Inventory { get; set; }
        public List<InventoryObjs.BoxItem> GameBusses { get; set; }

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

        public bool IfBusWithSuchNameIsUSedInGame(string name)
        {
            return GameBusses.Any(x => x.Name == name);
        }
    }
}
