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

namespace MonopolyDLL.Monopoly
{
    public class Board
    {
        public CellParent[] Cells { get; set; }

        public Board()
        {
            SetBasicBoard();
        }

        private int _startIndex;
        private int _prisonIndex;
        private int _casinoIndex;
        private int _goToPrisonIndex;

        private void SetBasicBoard()
        {
            Cells = new CellParent[40];

            Cells[0] = new Start("Start", 0);
            _startIndex = 0;

            Cells[1] = new UsualBus("Chanel", 600, 300, 360, new List<int>() { 20, 100, 300, 900, 1600, 2500 }, 0, 0, 500, -1, BusinessType.Perfume, false, 1);
            Cells[2] = new Chance("Chance", 2);
            Cells[3] = new UsualBus("HugoBoss", 600, 300, 360, new List<int>() { 40, 200, 600, 1800, 3200, 4500 }, 0, 0, 500, -1, BusinessType.Perfume, false,3);
            Cells[4] = new Tax("LittleTax", 1000, 4);
            Cells[5] = new CarBus("Mercedes", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 5);
            Cells[6] = new UsualBus("Adidas", 1000, 500, 600, new List<int>() { 60, 300, 900, 2700, 4000, 5500 }, 0, 0, 500, -1, BusinessType.Clothes, false, 6);
            Cells[7] = new Chance("Chance", 7);
            Cells[8] = new UsualBus("Puma", 1000, 500, 600, new List<int>() { 60, 300, 900, 2700, 4000, 5500 }, 0, 0, 500, -1, BusinessType.Clothes, false, 8);
            Cells[9] = new UsualBus("Lacoste", 1200, 600, 720, new List<int>() { 80, 400, 1000, 3000, 4500, 6000 }, 0, 0, 500, -1, BusinessType.Clothes, false, 9);

            Cells[10] = new Prison("Prison", 10);
            _prisonIndex = 10;

            Cells[11] = new UsualBus("VK", 1400, 700, 840, new List<int>() { 100, 500, 1500, 4500, 6250, 7500 }, 0, 0, 750, -1, BusinessType.Messagers, false, 11);
            Cells[12] = new GameBus("RockStarGames", 1500, 750, 900, new List<int>() { 100, 250 }, 0, 0, -1, BusinessType.Games, false, 12);
            Cells[13] = new UsualBus("Facebook", 1400, 700, 840, new List<int>() { 100, 500, 1500, 4500, 6250, 7500 }, 0, 0, 750, -1, BusinessType.Messagers, false, 13);
            Cells[14] = new UsualBus("Twitter", 1600, 800, 960, new List<int>() { 120, 600, 1800, 5000, 7000, 9000 }, 0, 0, 750, -1, BusinessType.Messagers, false, 14);
            Cells[15] = new CarBus("Audi", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 15);
            Cells[16] = new UsualBus("CocaCola", 1800, 900, 1080, new List<int>() { 140, 700, 2000, 5500, 7500, 9500 }, 0, 0, 1000, -1, BusinessType.Drinks, false, 16);
            Cells[17] = new Chance("Chance", 17);
            Cells[18] = new UsualBus("Pepsi", 1800, 900, 1080, new List<int>() { 140, 700, 2000, 5500, 7500, 9500 }, 0, 0, 1000, -1, BusinessType.Drinks, false, 18);
            Cells[19] = new UsualBus("Fanta", 2000, 1000, 1200, new List<int>() { 160, 800, 2200, 6000, 8000, 10000 }, 0, 0, 1000, -1, BusinessType.Drinks, false, 19);

            Cells[20] = new Casino("Casino", 20);
            _casinoIndex = 20;

            Cells[21] = new UsualBus("AmericanAirlines", 2200, 1100, 1320, new List<int>() { 180, 900, 2500, 7000, 8750, 10500 }, 0, 0, 1250, -1, BusinessType.Planes, false, 21);
            Cells[22] = new Chance("Chance", 22);
            Cells[23] = new UsualBus("Lufthansa", 2200, 1100, 1320, new List<int>() { 180, 900, 2500, 7000, 8750, 10500 }, 0, 0, 1250, -1, BusinessType.Planes, false, 23);
            Cells[24] = new UsualBus("BritishAirways", 2400, 1200, 1440, new List<int>() { 200, 1000, 3000, 7500, 9250, 11000 }, 0, 0, 1250, -1, BusinessType.Planes, false, 24);
            Cells[25] = new CarBus("Ford", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 25);
            Cells[26] = new UsualBus("MaxBurgers", 2600, 1300, 1500, new List<int>() { 220, 1100, 3300, 8000, 9750, 11500 }, 0, 0, 1500, -1, BusinessType.Food, false, 26);
            Cells[27] = new UsualBus("Burger King", 2600, 1300, 1500, new List<int>() { 220, 1100, 3300, 8000, 9750, 11500 }, 0, 0, 1500, -1, BusinessType.Food, false, 27);
            Cells[28] = new GameBus("Rovio", 1500, 750, 900, new List<int>() { 100, 250 }, 0, 0, 0, BusinessType.Games, false, 28);
            Cells[29] = new UsualBus("KFC", 2800, 1400, 1680, new List<int>() { 240, 1200, 3600, 8500, 10250, 12000 }, 0, 0, 1500, -1, BusinessType.Food, false, 29);

            Cells[30] = new GoToPrison("GoToPrison", 30);
            _goToPrisonIndex = 30;

            Cells[31] = new UsualBus("HolidayInn", 3000, 1500, 1800, new List<int>() { 260, 1300, 3900, 9000, 11000, 12750 }, 0, 0, 1750, -1, BusinessType.Hotels, false, 31);
            Cells[32] = new UsualBus("RadissonBlu", 3000, 1500, 1800, new List<int>() { 260, 1300, 3900, 9000, 11000, 12750 }, 0, 0, 1750, -1, BusinessType.Hotels, false, 32);
            Cells[33] = new Chance("Chance", 33);
            Cells[34] = new UsualBus("Novotel", 3200, 1600, 1920, new List<int>() { 280, 1500, 4500, 10000, 12000, 14000 }, 0, 0, 1750, -1, BusinessType.Hotels, false, 34);
            Cells[35] = new CarBus("LandRover", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars, false, 35);
            Cells[36] = new Tax("BigTax", 2000, 36);
            Cells[37] = new UsualBus("Apple", 3500, 1750, 2100, new List<int>() { 360, 1750, 5000, 11000, 13000, 15000 }, 0, 0, 2000, -1, BusinessType.Phones, false, 37);
            Cells[38] = new Chance("Chance", 38);
            Cells[39] = new UsualBus("Nokia", 4000, 2000, 2400, new List<int>() { 500, 2000, 6000, 14000, 17000, 20000 }, 0, 0, 2000, -1, BusinessType.Phones, false, 39);
        }

        public string GetBusinessPrice(int index)
        {
            return !(Cells[index] is ParentBus) ? string.Empty :
                ((ParentBus)Cells[index]).Price.ToString();
        }

        public (int, string) PlayCasino(List<int> chosenCasinoRibs)
        {
            int winMoney = ((Casino)Cells[_casinoIndex]).Play(chosenCasinoRibs);
            string resultMessage = ((Casino)Cells[_casinoIndex]).GetResultMessage(winMoney);

            return (winMoney, resultMessage);
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

        public int GetLitteleWinMoneyChance()
        {
            return GetChance().GetLittleWinMoney();
        }

        public int GetBigWinMoneyChance()
        {
            return GetChance().GetBigWinMoney();
        }

        public int GetLitteleLoseMoneyChance()
        {
            return GetChance().GetLittleLoseMoney();
        }

        public int GetBigLoseMoneyChance()
        {
            return GetChance().GetBigloseMoney();
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

        public bool IfPlayerOwnsBusiness(int playerIndex, int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).OwnerIndex == playerIndex;
        }

        public ParentBus GetBusinessByIndex(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]);
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
                if (Cells[buses[i]] is ParentBus bus)
                {
                    res += bus.GetPriceForBus();
                }
            }
            return res;
        }

        public void SetNewOwnerAfterTrade(List<int> indexes, int newOwnerIndex)
        {
            for (int i = 0; i < indexes.Count; i++)
            {
                ((ParentBus)Cells[indexes[i]]).ChangeOwner(newOwnerIndex);
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
            return ((ParentBus)Cells[cellIndex]).GetBusType();
        }

        public List<ParentBus> GetBusesByType(BusinessType type)
        {
            return Cells.OfType<ParentBus>().Where(x => x.BusType == type).ToList();
        }

        public List<ParentBus> GetBusesWhichPlayersOwnByType(BusinessType type, int playerIndex)
        {
            return Cells.OfType<ParentBus>().Where(x => x.BusType == type && x.OwnerIndex == playerIndex).ToList();
        }

        public UsualBusInfoVisual GetVisualBySettingNewAmountOfHouses(int cellIndex)
        {
            const int checkAmount = 1;
            //set +1 house and -1
            bool ifCanBeAdded = IfAmountOfHousesIsOk(cellIndex,
                ((ParentBus)Cells[cellIndex]).GetAddedHousesAmount(checkAmount));

            bool ifCanBeTaken = IfAmountOfHousesIsOk(cellIndex,
                ((ParentBus)Cells[cellIndex]).GetTakenHousesAmount(checkAmount));

            return ifCanBeAdded && ifCanBeTaken ? UsualBusInfoVisual.Combine :
                   ifCanBeAdded && !ifCanBeTaken ? UsualBusInfoVisual.BuyHouse : UsualBusInfoVisual.SellHouse;
            //!ifCanBeAdded && ifCanBeTaken ? UsualBusVisual.SellHouse 
        }

        public bool IfAmountOfHousesIsOk(int cellIndex, int amountOfHouses)
        {
            BusinessType type = GetBusTypeByIndex(cellIndex);
            List<ParentBus> buses = GetBusesByType(type);

            for (int i = 0; i < buses.Count; i++)
            {
                int differ = buses[i].Level - amountOfHouses;
                if (differ < -1 || differ > 1)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IfBusLevelIsMax(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).IfLevelIsMax();
        }

        public bool IfBusDoesNotHaveHoueses(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).IfThereAreNoHouses();
        }

        public int GetAmountOfBusesWithBusType(BusinessType type)
        {
            return Cells.OfType<UsualBus>().Where(x => x.BusType == type).Count();
        }

        public int GetAmountOfBusesTypeByOwningPlayer(int playersIndex, BusinessType type)
        {
            return Cells.OfType<UsualBus>().Where(x => x.BusType == type &&
            x.OwnerIndex == playersIndex).Count();
        }

        public bool IfPlayerHasMonopolyByType(int playerIndex, BusinessType type)
        {
            return GetAmountOfBusesWithBusType(type) ==
                GetAmountOfBusesTypeByOwningPlayer(playerIndex, type);
        }

        public int GetBusLevel(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).GetLevel();
        }

        public int GetUsualBusHousePrice(int cellIndex)
        {
            return ((UsualBus)Cells[cellIndex]).GetHousePrice();
        }

        public void BuyHouseUsualBus(int cellIndex)
        {
            ((UsualBus)Cells[cellIndex]).BuyHouse();
        }

        public void SellHouseUsualBus(int cellIndex)
        {
            ((UsualBus)Cells[cellIndex]).SellHouse();
        }

        public int GetBusMoneyLevel(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).GetPayMoney();
        }

        public bool IfBusinessIsDeposited(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).IfBusinessIsDeposited();
        }

        public bool IfBusesHaveHouses(List<ParentBus> buses)
        {
            return buses.Where(x => x.Level != 0).Any();
        }

        public bool IfAnyOfBussesIsDeposited(List<ParentBus> buses)
        {
            return buses.Where(x => x.IfBusinessIsDeposited()).Any();
        }

        public void DepositBus(int cellIndex)
        {
            ((ParentBus)Cells[cellIndex]).DepositBus();
        }

        public void RebuyBus(int cellIndex)
        {
            ((ParentBus)Cells[cellIndex]).RebuyBus();
        }

        public int GetRubyPrice(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).GetRubyPrice();
        }

        public int GetDepositPrice(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).GetDepositMoney();
        }

        public void SetCarsPaymentLevelByIndex(int playerIndex)
        {
            List<CarBus> buses =
                Cells.OfType<CarBus>().Where(x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();

            if (buses.Count() == 0) return;
            for (int i = 0; i < buses.Count; i++)
            {
                buses[i].SetBusLevel(buses.Count() - 1);
            }
        }

        public void SetGamePaymentsLevelByIndex(int playerIndex)
        {
            List<GameBus> buses =
                Cells.OfType<GameBus>().Where(x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();
            if (buses.Count == 0) return;
            for (int i = 0; i < buses.Count; i++)
            {
                buses[i].SetBusLevel(buses.Count() - 1);
            }
        }

        public List<int> GetGamesWhichPlayerOwnNotDeposited(int playerIndex)
        {
            List<int> res = new List<int>();

            List<GameBus> cars = Cells.OfType<GameBus>().Where(x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();

            for (int i = 0; i < cars.Count(); i++)
            {
                res.Add(Cells.ToList().IndexOf(cars[i]));
            }
            return res;
        }

        public List<int> GetCarsWhichPlayerOwnNotDeposited(int playerIndex)
        {
            List<int> res = new List<int>();

            List<CarBus> cars = Cells.OfType<CarBus>().Where(x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();

            for (int i = 0; i < cars.Count(); i++)
            {
                res.Add(Cells.ToList().IndexOf(cars[i]));
            }
            return res;
        }

        public List<UsualBus> GetAllPlayersNotDepositedUsualBuses(int playerIndex)
        {
            return Cells.OfType<UsualBus>().Where(
                x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();
        }

        public int GetPriceOfAllBuiltHouses(int playerIndex)
        {
            int res = 0;
            List<UsualBus> buses = GetAllPlayersNotDepositedUsualBuses(playerIndex);

            for(int i = 0; i < buses.Count; i++)
            {
                res += buses[i].GetPriceForBuiltHouses();
            }

            return res;
        }

        public int GetPriceForNotDepositedBuses(int playerIndex)
        {
            int res = 0;
            List<ParentBus> buses = Cells.OfType<ParentBus>().Where(
                x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();

            for(int i = 0; i < buses.Count; i++)
            {
                res += buses[i].GetDepositMoney();
            }
            return res;
        }

        public void UnDepositPlayersBuses(int playerIndex)
        {
            List<ParentBus> buses = Cells.OfType<ParentBus>().Where(
                    x => x.OwnerIndex == playerIndex && x.IfDeposited).ToList();

            for(int i = 0; i < buses.Count; i++)
            {
                buses[i].RebuyBus();
            }
        }

        public void ClearPlayersBussHouses(int playerIndex)
        {
            List<ParentBus> buses = Cells.OfType<ParentBus>().Where(
                    x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();
        
            for(int i = 0; i < buses.Count; i++)
            {
                buses[i].Level = 0;
            }
        }

        public void ClearPlayersBussesOwner(int playerIndex)
        {
            List<ParentBus> buses = Cells.OfType<ParentBus>().Where(
                    x => x.OwnerIndex == playerIndex && !x.IfDeposited).ToList();

            for (int i = 0; i < buses.Count; i++)
            {
                buses[i].OwnerIndex = -1;
            }
        }

        public void ClearAllPlayersBuses(int playerIndex)
        {
            //UnDeposit all players busess
            UnDepositPlayersBuses(playerIndex);

            //Clear Houses
            ClearPlayersBussHouses(playerIndex);

            //Clear Buses Owner
            ClearPlayersBussesOwner(playerIndex);
        }

        public int GetBusPrice(int busIndex)
        {
            return ((ParentBus)Cells[busIndex]).Price;
        }

        public int GetDepositCounter(int cellIndex)
        {
            return ((ParentBus)Cells[cellIndex]).GetDepositCounter();
        }

        public bool IfDepositCounterIsZero(int busIndex)
        {
            return ((ParentBus)Cells[busIndex]).IfDepositCounterIsZero();
        }

        public void SetNewCircleOfDepositedBuses()
        {
            for(int i = 0; i < Cells.Length; i++)
            {
                if(Cells[i] is ParentBus bus)
                {
                    bus.NewCircleOfDeposit();
                }
            }
        }
        
        public void ClearBusiness(int busIndex)
        {
            ((ParentBus)Cells[busIndex]).DepositCounterIsZero();
        }

        public int GetOwnerIndex(int busIndex)
        {
            return ((ParentBus)Cells[busIndex]).GetOwnerIndex();
        }

        private const int _littleStrickBusType = 2;
        private const int _bigStrickBusType = 2;
        public List<ParentBus> GetUsualBussesToChangeOn(InventoryObjs.BoxItem item)
        {
            List<ParentBus> res = new List<ParentBus>();

            List<UsualBus> buses = Cells.OfType<UsualBus>().Where(x => x.BusType == item.Type).ToList();
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
            res.Add(buses[1]);

            return res;
        }

        public List<ParentBus> GetAllCarBuses()
        {
            List<ParentBus> buses = new List<ParentBus>(); 
            buses.AddRange(Cells.OfType<CarBus>().ToList());
            return buses;
        }

        public List<ParentBus> GetAllGameBuses()
        {
            List<ParentBus> buses = new List<ParentBus>();
            buses.AddRange(Cells.OfType<GameBus>().ToList());
            return buses;
        }

      
        

    }
}
