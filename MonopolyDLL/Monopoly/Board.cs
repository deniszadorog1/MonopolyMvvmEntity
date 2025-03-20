using MonopolyDLL.Monopoly.Cell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonopolyDLL.Monopoly.Cell.AngleCells;
using System.Data.Entity.Infrastructure;
using MonopolyDLL.Monopoly.Cell.Bus;
using MonopolyDLL.Monopoly.Enums;
using System.Dynamic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Chance = MonopolyDLL.Monopoly.Cell.Chance;
using Tax = MonopolyDLL.Monopoly.Cell.Tax;
using Casino = MonopolyDLL.Monopoly.Cell.AngleCells.Casino;
using MonopolyDLL.DBService;
using System.Security.Policy;

namespace MonopolyDLL.Monopoly
{
    public class Board
    {
        public Cell.Cell[] Cells { get; set; }

        public Board()
        {
            SetBasicBoard();
            SetSquareCellsIndexes();
        }

        private int _startIndex;
        private int _prisonIndex;
        private int _casinoIndex;
        //private int _goToPrisonIndex;

        private void SetBasicBoard()
        {
            int amountOfCells = SystemParamsService.GetNumByName("AmountOfCells");
            Cells = new Cell.Cell[amountOfCells];

            List<Cell.Cell> basicGamesCells = SetBasicBoardCells();

            for (int i = 0; i < Cells.Length; i++)
            {
                Cells[i] = basicGamesCells[i];
            }
        }

        private readonly List<Cell.Cell> _basicBoardCells = SetBasicBoardCells();
        public static List<Cell.Cell> SetBasicBoardCells()
        {
            List<Cell.Cell> res = new List<Cell.Cell>();

            res = DBQueries.GetBasicBoardCells();

            return res;
/*
            res.Add(new Start("Start", 0));

            res.Add(new UsualBus("Chanel", 600, 300, 360, new List<int>() { 20, 100, 300, 900, 1600, 2500 }, 0, 0, 500, -1, BusinessType.Perfume, false, 1));
            res.Add(new Chance("Chance", 2));
            res.Add(new UsualBus("HugoBoss", 600, 300, 360, new List<int>() { 40, 200, 600, 1800, 3200, 4500 }, 0, 0, 500, -1, BusinessType.Perfume, false, 3));
            res.Add(new Tax("LittleTax", 2000, 4));
            res.Add(new CarBus("Mercedes", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 5));
            res.Add(new UsualBus("Adidas", 1000, 500, 600, new List<int>() { 60, 300, 900, 2700, 4000, 5500 }, 0, 0, 500, -1, BusinessType.Clothes, false, 6));
            res.Add(new Chance("Chance", 7));
            res.Add(new UsualBus("Puma", 1000, 500, 600, new List<int>() { 60, 300, 900, 2700, 4000, 5500 }, 0, 0, 500, -1, BusinessType.Clothes, false, 8));
            res.Add(new UsualBus("Lacoste", 1200, 600, 720, new List<int>() { 80, 400, 1000, 3000, 4500, 6000 }, 0, 0, 500, -1, BusinessType.Clothes, false, 9));

            res.Add(new Prison("Prison", 10));

            res.Add(new UsualBus("VK", 1400, 700, 840, new List<int>() { 100, 500, 1500, 4500, 6250, 7500 }, 0, 0, 750, -1, BusinessType.Messagers, false, 11));
            res.Add(new GameBus("RockStarGames", 1500, 750, 900, new List<int>() { 100, 250 }, 0, 0, -1, BusinessType.Games, false, 12));
            res.Add(new UsualBus("Facebook", 1400, 700, 840, new List<int>() { 100, 500, 1500, 4500, 6250, 7500 }, 0, 0, 750, -1, BusinessType.Messagers, false, 13));
            res.Add(new UsualBus("Twitter", 1600, 800, 960, new List<int>() { 120, 600, 1800, 5000, 7000, 9000 }, 0, 0, 750, -1, BusinessType.Messagers, false, 14));
            res.Add(new CarBus("Audi", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 15));
            res.Add(new UsualBus("CocaCola", 1800, 900, 1080, new List<int>() { 140, 700, 2000, 5500, 7500, 9500 }, 0, 0, 1000, -1, BusinessType.Drinks, false, 16));
            res.Add(new Chance("Chance", 17));
            res.Add(new UsualBus("Pepsi", 1800, 900, 1080, new List<int>() { 140, 700, 2000, 5500, 7500, 9500 }, 0, 0, 1000, -1, BusinessType.Drinks, false, 18));
            res.Add(new UsualBus("Fanta", 2000, 1000, 1200, new List<int>() { 160, 800, 2200, 6000, 8000, 10000 }, 0, 0, 1000, -1, BusinessType.Drinks, false, 19));

            res.Add(new Casino("Casino", 20));

            res.Add(new UsualBus("AmericanAirlines", 2200, 1100, 1320, new List<int>() { 180, 900, 2500, 7000, 8750, 10500 }, 0, 0, 1250, -1, BusinessType.Planes, false, 21));
            res.Add(new Chance("Chance", 22));
            res.Add(new UsualBus("Lufthansa", 2200, 1100, 1320, new List<int>() { 180, 900, 2500, 7000, 8750, 10500 }, 0, 0, 1250, -1, BusinessType.Planes, false, 23));
            res.Add(new UsualBus("BritishAirways", 2400, 1200, 1440, new List<int>() { 200, 1000, 3000, 7500, 9250, 11000 }, 0, 0, 1250, -1, BusinessType.Planes, false, 24));
            res.Add(new CarBus("Ford", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 25));
            res.Add(new UsualBus("MaxBurgers", 2600, 1300, 1500, new List<int>() { 220, 1100, 3300, 8000, 9750, 11500 }, 0, 0, 1500, -1, BusinessType.Food, false, 26));
            res.Add(new UsualBus("Burger King", 2600, 1300, 1500, new List<int>() { 220, 1100, 3300, 8000, 9750, 11500 }, 0, 0, 1500, -1, BusinessType.Food, false, 27));
            res.Add(new GameBus("Rovio", 1500, 750, 900, new List<int>() { 100, 250 }, 0, 0, -1, BusinessType.Games, false, 28));
            res.Add(new UsualBus("KFC", 2800, 1400, 1680, new List<int>() { 240, 1200, 3600, 8500, 10250, 12000 }, 0, 0, 1500, -1, BusinessType.Food, false, 29));

            res.Add(new GoToPrison("GoToPrison", 30));

            res.Add(new UsualBus("HolidayInn", 3000, 1500, 1800, new List<int>() { 260, 1300, 3900, 9000, 11000, 12750 }, 0, 0, 1750, -1, BusinessType.Hotels, false, 31));
            res.Add(new UsualBus("RadissonBlu", 3000, 1500, 1800, new List<int>() { 260, 1300, 3900, 9000, 11000, 12750 }, 0, 0, 1750, -1, BusinessType.Hotels, false, 32));
            res.Add(new Chance("Chance", 33));
            res.Add(new UsualBus("Novotel", 3200, 1600, 1920, new List<int>() { 280, 1500, 4500, 10000, 12000, 14000 }, 0, 0, 1750, -1, BusinessType.Hotels, false, 34));
            res.Add(new CarBus("LandRover", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 35));
            res.Add(new Tax("BigTax", 1000, 36));
            res.Add(new UsualBus("Apple", 3500, 1750, 2100, new List<int>() { 360, 1750, 5000, 11000, 13000, 15000 }, 0, 0, 2000, -1, BusinessType.Phones, false, 37));
            res.Add(new Chance("Chance", 38));
            res.Add(new UsualBus("Nokia", 4000, 2000, 2400, new List<int>() { 500, 2000, 6000, 14000, 17000, 20000 }, 0, 0, 2000, -1, BusinessType.Phones, false, 39));

            return res;*/
        }

        private void SetSquareCellsIndexes()
        {
            _startIndex = _basicBoardCells.OfType<Start>().First().GetId();
            _prisonIndex = _basicBoardCells.OfType<Prison>().First().GetId();
            _casinoIndex = _basicBoardCells.OfType<Casino>().First().GetId();
            //_goToPrisonIndex = _basicBoardCells.OfType<GoToPrison>().First().GetId();
        }

        public int GetBusinessPrice(int index)
        {
            const int emptyPrice = -1;
            return Cells[index] is Business parentBus ? parentBus.Price : emptyPrice;
        }

        public (int, string) PlayCasino(List<int> chosenCasinoRibs)
        {
            int winMoney = ((Casino)Cells[_casinoIndex]).Play(chosenCasinoRibs);
            string resultMessage = ((Casino)Cells[_casinoIndex]).GetResultMessage(winMoney);

            return (winMoney, resultMessage);
        }

        public int GetCasinoWinValue()
        {
            return ((Casino)Cells[_casinoIndex]).GetWinValue();
        }

        public int GetCasinoGamePrice()
        {
            return ((Casino)Cells[_casinoIndex]).GetGamePrice();
        }

        public ChanceAction GetChanceAction(int position)
        {
            ChanceAction action = ((Chance)Cells[position]).GetRandomChanceAction();
            return action;
        }

        public int GetLittleWinMoneyChance()
        {
            return GetChance().GetLittleWinMoney();
        }

        public int GetBigWinMoneyChance()
        {
            return GetChance().GetBigWinMoney();
        }

        public int GetLittleLoseMoneyChance()
        {
            return GetChance().GetLittleLoseMoney();
        }

        public int GetBigLoseMoneyChance()
        {
            return GetChance().GetBigLoseMoney();
        }

        public int GetStepForwardChance()
        {
            return GetChance().GetStepForward();
        }

        public int GetStepBackwardChance()
        {
            return GetChance().GetStepBackwards();
        }

        public Chance GetChance()
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i] is Chance chance)
                {
                    return chance;
                }
            }
            throw new Exception("Cant find any chance cell!");
        }

        public int GetPrisonCellIndex()
        {
            return _prisonIndex;
        }

        public bool IsPlayerOwnsBusiness(int playerIndex, int cellIndex)
        {
            return ((Business)Cells[cellIndex]).OwnerIndex == playerIndex;
        }

        public Business GetBusinessByIndex(int cellIndex)
        {
            return (Business)Cells[cellIndex];
        }

        public int GetCellIndexByName(string cellName)
        {
            for (int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].Name == cellName) return i;
            }
            throw new Exception("There is no cell with such name!");
        }

        public int GetTotalPriceForBuses(List<int> buses)
        {
            int res = 0;
            for (int i = 0; i < buses.Count; i++)
            {
                if (Cells[buses[i]] is Business bus)
                {
                    res += bus.GetPriceForBus();
                }
            }
            return res;
        }

        public int GetParentBusPrice(int index)
        {
            return ((Business)Cells[index]).Price;
        }

        public void SetNewOwnerAfterTrade(List<int> indexes, int newOwnerIndex)
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                ((Business)Cells[indexes[i]]).ChangeOwner(newOwnerIndex);
            }
        }

        public int GetPrisonPrice()
        {
            return ((Prison)Cells[GetPrisonCellIndex()]).GetOutPrisonPrice();
        }

        public int GetMaxSittingRoundsInPrison()
        {
            return ((Prison)Cells[GetPrisonCellIndex()]).GetMaxSittingRounds();
        }

        public int GetGotOnStartCellMoney()
        {
            return ((Start)Cells[_startIndex]).GetGeOnCellMoney();
        }

        public int GetGotThroughStartCellMoney()
        {
            return ((Start)Cells[_startIndex]).GetGoThroughMoney();
        }

        public int GetStartCellIndex()
        {
            return _startIndex;
        }

        public BusinessType GetBusTypeByIndex(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).GetBusType();
        }

        public List<Business> GetBusesByType(BusinessType type)
        {
            return Cells.OfType<Business>().Where(x => x.BusType == type).ToList();
        }

        public List<Business> GetBusesWhichPlayersOwnByType(BusinessType type, int playerIndex)
        {
            return Cells.OfType<Business>().Where(x => x.BusType == type && x.OwnerIndex == playerIndex).ToList();
        }

        public UsualBusInfoVisual? GetVisualBySettingNewAmountOfHouses(int cellIndex, bool isBuilt)
        {
            const int checkAmount = 1;
            //set +1 house and -1
            bool isCanBeAdded = IsAmountOfHousesIsOk(cellIndex,
                ((Business)Cells[cellIndex]).GetAddedHousesAmount(checkAmount)) && !isBuilt;

            bool isCanBeTaken = IsAmountOfHousesIsOk(cellIndex,
                ((Business)Cells[cellIndex]).GetTakenHousesAmount(checkAmount)) &&
                            !(((Business)Cells[cellIndex]).Level == 0);


            if (!isCanBeAdded && !isCanBeTaken) return null;

            return isCanBeAdded && isCanBeTaken ? UsualBusInfoVisual.Combine :
                   isCanBeAdded && !isCanBeTaken ? UsualBusInfoVisual.BuyHouse : UsualBusInfoVisual.SellHouse;
                   //!ifCanBeAdded && ifCanBeTaken ? UsualBusInfoVisual.SellHouse : null;
        }

        public bool IsAmountOfHousesIsOk(int cellIndex, int amountOfHouses)
        {
            BusinessType type = GetBusTypeByIndex(cellIndex);
            List<Business> buses = GetBusesByType(type);

            const int maxHouseDiffer = 1;

            for (int i = 0; i < buses.Count; i++)
            {
                int differ = buses[i].Level - amountOfHouses;
                if (differ < -maxHouseDiffer || differ > maxHouseDiffer)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsBusLevelIsMax(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).IsLevelIsMax();
        }

        public bool IsBusDoesNotHaveHouses(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).IsThereAreNoHouses();
        }

        public int GetAmountOfBusesWithBusType(BusinessType type)
        {
            return Cells.OfType<RegularBusiness>().Where(x => x.BusType == type).Count();
        }

        public int GetAmountOfBusesTypeByOwningPlayer(int playersIndex, BusinessType type)
        {
            return Cells.OfType<RegularBusiness>().Where(x => x.BusType == type &&
            x.OwnerIndex == playersIndex).Count();
        }

        public bool IsPlayerHasMonopolyByType(int playerIndex, BusinessType type)
        {
            return GetAmountOfBusesWithBusType(type) ==
                GetAmountOfBusesTypeByOwningPlayer(playerIndex, type);
        }

        public int GetBusLevel(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).GetLevel();
        }

        public int GetUsualBusHousePrice(int cellIndex)
        {
            return ((RegularBusiness)Cells[cellIndex]).GetHousePrice();
        }

        public void BuyHouseUsualBus(int cellIndex)
        {
            ((RegularBusiness)Cells[cellIndex]).BuyHouse();
        }

        public void SellHouseUsualBus(int cellIndex)
        {
            ((RegularBusiness)Cells[cellIndex]).SellHouse();
        }

        public int GetBusMoneyLevel(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).GetPayMoney();
        }

        public bool IsBusinessIsDeposited(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).IsBusinessIsDeposited();
        }

        public bool IsBusesHaveHouses(List<Business> buses)
        {
            return buses.Any(x => x.Level != 0);
        }

        public bool IsAnyOfBussesIsDeposited(List<Business> buses)
        {
            return buses.Any(x => x.IsBusinessIsDeposited());
        }


        public void DepositBus(int cellIndex)
        {
            ((Business)Cells[cellIndex]).DepositBus();
        }

        public void RebuyBus(int cellIndex)
        {
            ((Business)Cells[cellIndex]).RebuyBus();
        }

        public void SetMaxDepositCounter(int cellIndex)
        {
            ((Business)Cells[cellIndex]).SetNewDepositCounter();
        }

        public int GetRubyPrice(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).GetRubyPrice();
        }

        public int GetDepositPrice(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).GetDepositMoney();
        }

        public void SetCarsPaymentLevelByIndex(int playerIndex)
        {
            List<CarBusiness> buses =
                Cells.OfType<CarBusiness>().Where(x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();

            if (buses.Count() == 0) return;
            for (int i = 0; i < buses.Count; i++)
            {
                buses[i].SetBusLevel(buses.Count() - 1);
            }
        }

        public void SetGamePaymentsLevelByIndex(int playerIndex)
        {
            List<GameBusiness> buses =
                Cells.OfType<GameBusiness>().Where(x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();
            if (buses.Count == 0) return;

            for (int i = 0; i < buses.Count; i++)
            {
                buses[i].SetBusLevel(buses.Count() - 1);
            }
        }

        public List<int> GetGamesWhichPlayerOwnNotDeposited(int playerIndex)
        {
            List<int> res = new List<int>();

            List<GameBusiness> games = Cells.OfType<GameBusiness>().Where(x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();

            for (int i = 0; i < games.Count(); i++)
            {
                res.Add(Cells.ToList().IndexOf(games[i]));
            }
            return res;
        }

        public List<int> GetCarsWhichPlayerOwnNotDeposited(int playerIndex)
        {
            List<int> res = new List<int>();

            List<CarBusiness> cars = Cells.OfType<CarBusiness>().Where(x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();

            for (int i = 0; i < cars.Count(); i++)
            {
                res.Add(Cells.ToList().IndexOf(cars[i]));
            }
            return res;
        }

        public List<RegularBusiness> GetAllPlayersNotDepositedUsualBuses(int playerIndex)
        {
            return Cells.OfType<RegularBusiness>().Where(
                x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();
        }

        public int GetPriceOfAllBuiltHouses(int playerIndex)
        {
            int res = 0;
            List<RegularBusiness> buses = GetAllPlayersNotDepositedUsualBuses(playerIndex);

            for(int i = 0; i < buses.Count; i++)
            {
                res += buses[i].GetPriceForBuiltHouses();
            }

            return res;
        }

        public int GetPriceForNotDepositedBuses(int playerIndex)
        {
            int res = 0;
            List<Business> buses = Cells.OfType<Business>().Where(
                x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();

            for(int i = 0; i < buses.Count; i++)
            {
                res += buses[i].GetDepositMoney();
            }
            return res;
        }

        public void UnDepositPlayersBuses(int playerIndex)
        {
            List<Business> buses = Cells.OfType<Business>().Where(
                    x => x.OwnerIndex == playerIndex && x.IsDeposited).ToList();

            for(int i = 0; i < buses.Count; i++)
            {
                buses[i].RebuyBus();
            }
        }

        public void ClearPlayersBussHouses(int playerIndex)
        {
            List<Business> buses = Cells.OfType<Business>().Where(
                    x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();
        
            for(int i = 0; i < buses.Count; i++)
            {
                buses[i].Level = 0;
            }
        }


        public void ClearAllPlayersBuses(int playerIndex)
        {
            //Clear Players bus deposit counter 
            ClearDepositCounter(playerIndex);

            //UnDeposit all players Businesses
            UnDepositPlayersBuses(playerIndex);

            //Clear Houses
            ClearPlayersBussHouses(playerIndex);

            //Clear Buses Owner
            ClearPlayersBussesOwner(playerIndex);
        }

        public void ClearDepositCounter(int ownerIndex)
        {
            for(int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i] is Business bus && 
                    bus.OwnerIndex == ownerIndex)
                {
                    bus.SetNewDepositCounter();
                }
            }
        }

        public int GetBusPrice(int busIndex)
        {
            return ((Business)Cells[busIndex]).Price;
        }

        public int GetDepositCounter(int cellIndex)
        {
            return ((Business)Cells[cellIndex]).GetDepositCounter();
        }

        public bool IsDepositCounterIsZero(int busIndex)
        {
            return ((Business)Cells[busIndex]).IsDepositCounterIsZero();
        }

        public void SetNewCircleOfDepositedBuses()
        {
            for(int i = 0; i < Cells.Length; i++)
            {
                if(Cells[i] is Business bus && bus.IsDeposited)
                {
                    bus.NewCircleOfDeposit();
                }
            }
        }
        
        public void ClearBusiness(int busIndex)
        {
            ((Business)Cells[busIndex]).ClearBusValues();
        }

        public int GetOwnerIndex(int busIndex)
        {
            return ((Business)Cells[busIndex]).GetOwnerIndex();
        }

        private const int _littleStrickBusType = 2;
        private const int _secondObjIndex = 1;
        public List<Business> GetUsualBussesToChangeOn(InventoryObjs.BoxItem item)
        {          
            List<Business> res = new List<Business>();

            List<RegularBusiness> buses = Cells.OfType<RegularBusiness>().Where(x => x.BusType == item.Type).ToList();
            if(buses.Count == _littleStrickBusType)
            {
                res.AddRange(buses.Where(x => x.GetId() == item.StationId));
                return res; 
            }

            //if three busses in row
            if(buses.Last().GetId() == item.StationId) // if change for last one
            {
                res.Add(buses.Last());
                return res;
            }

            //to change for first two
            res.Add(buses.First());
            res.Add(buses[_secondObjIndex]);

            return res;
        }

        public List<Business> GetAllCarBuses() //to check
        {
            List<Business> buses = new List<Business>(); 
            buses.AddRange(Cells.OfType<Business>().ToList());
            return buses;
        }

        public void ClearPlayersBussesOwner(int playerIndex)
        {
            List<Business> buses = Cells.OfType<Business>().Where(
                    x => x.OwnerIndex == playerIndex && !x.IsDeposited).ToList();

            for (int i = 0; i < buses.Count; i++)
            {
                buses[i].OwnerIndex = -1;
            }
        }

        public List<Business> GetAllGameBuses()
        {
            List<Business> buses = new List<Business>();
            buses.AddRange(Cells.OfType<GameBusiness>().ToList());
            return buses;
        }

        public void ChangeBoardItemOnInventory(Business bus, int id)
        {
            bool isDeposited = false;
            int desCounter = SystemParamsService.GetNumByName("MaxDepositCounter");
            if (Cells[id] is Business)
            {
                isDeposited = bus.IsDeposited;
                desCounter = bus.TempDepositCounter;
            }

            Cells[id] = bus;
            if (Cells[id] is Business newBus)
            {
                newBus.IsDeposited = isDeposited;
                newBus.TempDepositCounter = desCounter;
            }
        }

        public void GetBasicBusBack(int position)
        {
            bool isDeposited = false;
            int desCounter = SystemParamsService.GetNumByName("MaxDepositCounter");
            if (Cells[position] is Business bus)
            {
                isDeposited = bus.IsDeposited;
                desCounter = bus.TempDepositCounter;
                bus.OwnerIndex = SystemParamsService.GetNumByName("NoOwnerIndex");
            }

            Cells[position] = _basicBoardCells[position];
            //Cells[position] = _basicBoardCells[position].GetCopy(_basicBoardCells[position]);

            if (Cells[position] is Business newBus)
            {
                newBus.IsDeposited = isDeposited;
                newBus.TempDepositCounter = desCounter;
                newBus.OwnerIndex = SystemParamsService.GetNumByName("NoOwnerIndex");
            }
        }

        public Business GetBusByIndex(int index)
        {   
            return ((Business)Cells[index]);
        }

        public bool IsPlayerIsOnEnemiesBus(int playerIndex, int cellIndex)
        {
            return Cells[cellIndex] is Business && ((Business)Cells[cellIndex]).OwnerIndex != 
                SystemParamsService.GetNumByName("NoOwnerIndex") &&
                ((Business)Cells[cellIndex]).OwnerIndex != playerIndex;
        }

        public int GetBusOwnerIndex(int cellIndex)
        {
            return Cells[cellIndex] is Business ? ((Business)Cells[cellIndex]).OwnerIndex : 
                SystemParamsService.GetNumByName("NoOwnerIndex");
        }

        public void ClearBusOwner(int busIndex)
        {
            if (!(Cells[busIndex] is Business)) return;
            ((Business)Cells[busIndex]).OwnerIndex = SystemParamsService.GetNumByName("NoOwnerIndex");
        }

        public int GetRandomCellIndexToGoOnInChance()
        {
            Random rnd = new Random();
            return rnd.Next(0, Cells.Length);
        }

        public int GetAmountOfStars(int ownerIndex, bool isGoldStar)
        {
            int res = 0;
            for(int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i] is RegularBusiness bus && bus.OwnerIndex == ownerIndex &&
                    !bus.IsDeposited &&  bus.IsBusLevelIsMax() == isGoldStar)
                {
                    res += bus.Level;       
                }
            }
            return res;
        }

        
    }
}
