using MonopolyDLL.Monopoly.Cell.Businesses;
using MonopolyDLL.Monopoly.Enums;
using MonopolyDLL.Monopoly.InventoryObjs;
using System.Collections.Generic;
using System.Linq;
using BoxItem = MonopolyDLL.Monopoly.InventoryObjs.BoxItem;

namespace MonopolyDLL.Monopoly
{
    public class User
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public int AmountOfMoney { get; set; }
        public int Position { get; set; }
        public bool IsSitInPrison { get; set; }
        public int SitInPrisonCounter { get; set; }

        public UserInventory Inventory { get; set; }
        public List<BoxItem> GameBusses { get; set; }

        public bool IsLost { get; set; }
        public int DoubleCounter { get; set; }

        public List<BusinessType> BuiltHousesInRowType { get; set; }
        public bool IsMoveBackwards = false;
        private List<BusinessType> _collectedMonopolies = new List<BusinessType>();

        private int _id;
        private int _maxDoubles = SystemParamsService.GetNumByName("MaxDoubles"); //3;
        private int? _pictureId;
        private bool _skipMove = false;


        public User(string login, int id, int? picId, string password)
        {
            Login = login;
            Password = password;
            _id = id;
            GameBusses = new List<InventoryObjs.BoxItem>();
            AmountOfMoney = SystemParamsService.GetNumByName("StartMoney"); //15000;
            Position = 0;
            IsSitInPrison = false;
            BuiltHousesInRowType = new List<BusinessType>();
            IsLost = false;
            DoubleCounter = 0;
            GameBusses = new List<InventoryObjs.BoxItem>();
            SetPictureId(picId);
        }

        public User()
        {
            Login = string.Empty;
            AmountOfMoney = SystemParamsService.GetNumByName("StartMoney");// 15000;
            Position = 0;
            IsSitInPrison = false;
            BuiltHousesInRowType = new List<BusinessType>();
            IsLost = false;
            DoubleCounter = 0;
            GameBusses = new List<InventoryObjs.BoxItem>();
        }

        public User(string login, int amountOfMoney, int position, bool isLost)
        {
            Login = login;
            AmountOfMoney = amountOfMoney;
            Position = position;
            IsSitInPrison = false;
            BuiltHousesInRowType = new List<BusinessType>();
            IsLost = isLost;
            DoubleCounter = 0;
            GameBusses = new List<InventoryObjs.BoxItem>();
        }

        public int GetId()
        {
            return _id;
        }

        public void SetSkipMoveOpposite()
        {
            _skipMove = !_skipMove;
        }

        public bool IsSleeping()
        {
            return _skipMove;
        }

        public void SetOppositeMoveBackwardsVal()
        {
            IsMoveBackwards = !IsMoveBackwards;
        }

        public bool IsNeedToMoveBackwards()
        {
            return IsMoveBackwards;
        }

        public void ClearBuiltHousesInRowType()
        {
            BuiltHousesInRowType.Clear();
        }

        public void AddTypeInBuiltHousesInRowType(BusinessType type)
        {
            BuiltHousesInRowType.Add(type);
        }

        public bool IsTypeContainsInBuiltHouses(BusinessType type)
        {
            return BuiltHousesInRowType.Contains(type);
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

        public bool IsPlayersInCellIndex(int cellIndex)
        {
            return Position == cellIndex;
        }

        public bool IsPlayerSitsInPrison()
        {
            return IsSitInPrison;
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
            IsSitInPrison = !IsSitInPrison;
        }

        public int GetPrisonCounter()
        {
            return SitInPrisonCounter;
        }

        public List<BusinessType> GetCollectedMonopolies()
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

        public bool IsPlayerHasEnoughMoney(int money)
        {
            return AmountOfMoney >= money;
        }

        public bool IsMaxDoublesIsAchieved()
        {
            return _maxDoubles <= DoubleCounter;
        }

        public bool IsDoublesMoreThenZero()
        {
            return DoubleCounter != 0;
        }

        public void ClearDoubleCounter()
        {
            DoubleCounter = 0;
        }

        public bool IsBusWithSuchIdIsUsedInGame(int stationId)
        {
            return GameBusses.Any(x => x.StationId == stationId);
        }

        public BoxItem GetItemWhichUsesInGameById(int id)
        {
            return GameBusses.Any(x => x.StationId == id) ?
                GameBusses.Find(x => x.StationId == id) : null;
        }

        public bool IsBusWithSuchNameIsUsedInGame(string name)
        {
            return GameBusses.Any(x => x.Name == name);
        }

        public BoxItem GetItemByName(string name)
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

        public BoxItem GetUsingItemByIndex(int index)
        {
            return GameBusses[index];
        }

        public bool IsHasInventoryOnPosition()
        {
            return GameBusses.Where(x => x.StationId == Position).Any();
        }

        public bool IsHasInventoryItemOnCellIndex(int cellIndex)
        {
            return GameBusses.Where(x => x.StationId == cellIndex).Any();
        }

        public Business GetInventoryItemById(int position, Business usualPosBus, int newOwnerIndex)
        {
            BoxItem item = GameBusses.Where(x => x.StationId == position).FirstOrDefault();

            int usCounter = SystemParamsService.GetNumByName("MaxDepositCounter");// 15; //!!!

            Business bus;

            if (item.Type == BusinessType.Cars)
            {
                bus = new CarBusiness(item.Name, usualPosBus.Price, usualPosBus.DepositPrice,
                     usualPosBus.RebuyPrice, item.GetNewPaymentList(usualPosBus.PayLevels),
                     usCounter, 0, newOwnerIndex, item.Type, usualPosBus.IsDeposited, usualPosBus.GetId());
            }
            else if (item.Type == BusinessType.Games)
            {
                bus = new GameBusiness(item.Name, usualPosBus.Price, usualPosBus.DepositPrice,
                    usualPosBus.RebuyPrice, item.GetNewPaymentList(usualPosBus.PayLevels),
                    usCounter, 0, newOwnerIndex, item.Type, usualPosBus.IsDeposited, usualPosBus.GetId());
            }
            else
            {
                bus = new RegularBusiness(item.Name, usualPosBus.Price, usualPosBus.DepositPrice,
                usualPosBus.RebuyPrice, item.GetNewPaymentList(usualPosBus.PayLevels),
                usCounter, 0, ((RegularBusiness)usualPosBus).BuySellHouse, newOwnerIndex,
                item.Type, usualPosBus.IsDeposited, usualPosBus.GetId());
            }

            bus.SetTempDepositCounter(usualPosBus.TempDepositCounter);

            return bus;
        }

        public BoxItem GetBoxItemByPosition(int position)
        {
            return GameBusses.Where(x => x.StationId == position).FirstOrDefault();
        }

        public string GetLogin()
        {
            return Login;
        }

        public int? GetPictureId()
        {
            return _pictureId;
        }

        public void SetPictureId(int? picId)
        {
            _pictureId = picId;
        }

        public void RemoveAddedBusWithGivenId(int id)
        {
            GameBusses.Remove(GameBusses.Where(x => x.StationId == id).FirstOrDefault());
        }
    }
}
