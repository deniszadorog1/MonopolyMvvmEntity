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

            Cells[0] = new Start("Start");
            _startIndex = 0;

            Cells[1] = new UsualBus("Chanel", 600, 300, 360, new List<int>() { 20, 100, 300, 900, 1600, 2500 }, 0, 0, 500, -1, BusinessType.Perfume);
            Cells[2] = new Chance("Chance");
            Cells[3] = new UsualBus("HugoBoss", 600, 300, 360, new List<int>() { 40, 200, 600, 1800, 3200, 4500 }, 0, 0, 500, -1, BusinessType.Perfume);
            Cells[4] = new Tax("LittleTax", 1000);
            Cells[5] = new CarBus("Mercedes", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars);
            Cells[6] = new UsualBus("Adidas", 1000, 500, 600, new List<int>() { 60, 300, 900, 2700, 4000, 5500 }, 0, 0, 500, -1, BusinessType.Clothes);
            Cells[7] = new Chance("Chance");
            Cells[8] = new UsualBus("Puma", 1000, 500, 600, new List<int>() { 60, 300, 900, 2700, 4000, 5500 }, 0, 0, 500, -1, BusinessType.Clothes);
            Cells[9] = new UsualBus("Lacoste", 1200, 600, 720, new List<int>() { 80, 400, 1000, 3000, 4500, 6000 }, 0, 0, 500, -1, BusinessType.Clothes);

            Cells[10] = new Prison("Prison");
            _prisonIndex = 10;

            Cells[11] = new UsualBus("VK", 1400, 700, 840, new List<int>() { 100, 500, 1500, 4500, 6250, 7500 }, 0, 0, 750, -1, BusinessType.Messagers);
            Cells[12] = new GameBus("RockStarGames", 1500, 750, 900, new List<int>() { 100, 250 }, 0, 0, -1, BusinessType.Games);
            Cells[13] = new UsualBus("Facebook", 1400, 700, 840, new List<int>() { 100, 500, 1500, 4500, 6250, 7500 }, 0, 0, 750, -1, BusinessType.Messagers);
            Cells[14] = new UsualBus("Twitter", 1600, 800, 960, new List<int>() { 120, 600, 1800, 5000, 7000, 9000 }, 0, 0, 750, -1, BusinessType.Messagers);
            Cells[15] = new CarBus("Audi", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars);
            Cells[16] = new UsualBus("CocaCola", 1800, 900, 1080, new List<int>() { 140, 700, 2000, 5500, 7500, 9500 }, 0, 0, 1000, -1, BusinessType.Drinks);
            Cells[17] = new Chance("Chance");
            Cells[18] = new UsualBus("Pepsi", 1800, 900, 1080, new List<int>() { 140, 700, 2000, 5500, 7500, 9500 }, 0, 0, 1000, -1, BusinessType.Drinks);
            Cells[19] = new UsualBus("Fanta", 2000, 1000, 1200, new List<int>() { 160, 800, 2200, 6000, 8000, 10000 }, 0, 0, 1000, -1, BusinessType.Drinks);

            Cells[20] = new Casino("Casino");
            _casinoIndex = 20;

            Cells[21] = new UsualBus("AmericanAirlines", 2200, 1100, 1320, new List<int>() { 180, 900, 2500, 7000, 8750, 10500 }, 0, 0, 1250, -1, BusinessType.Planes);
            Cells[22] = new Chance("Chance");
            Cells[23] = new UsualBus("Lufthansa", 2200, 1100, 1320, new List<int>() { 180, 900, 2500, 7000, 8750, 10500 }, 0, 0, 1250, -1, BusinessType.Planes);
            Cells[24] = new UsualBus("BritishAirways", 2400, 1200, 1440, new List<int>() { 200, 1000, 3000, 7500, 9250, 11000 }, 0, 0, 1250, -1, BusinessType.Planes);
            Cells[25] = new CarBus("Ford", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars);
            Cells[26] = new UsualBus("MaxBurgers", 2600, 1300, 1500, new List<int>() { 220, 1100, 3300, 8000, 9750, 11500 }, 0, 0, 1500, -1, BusinessType.Food);
            Cells[27] = new UsualBus("Burger King", 2600, 1300, 1500, new List<int>() { 220, 1100, 3300, 8000, 9750, 11500 }, 0, 0, 1500, -1, BusinessType.Food);
            Cells[28] = new GameBus("Rovio", 1500, 750, 900, new List<int>() { 100, 250 }, 0, 0, 0, BusinessType.Games);
            Cells[29] = new UsualBus("KFC", 2800, 1400, 1680, new List<int>() { 240, 1200, 3600, 8500, 10250, 12000 }, 0, 0, 1500, -1, BusinessType.Food);

            Cells[30] = new GoToPrison("GoToPrison");
            _goToPrisonIndex = 30;

            Cells[31] = new UsualBus("HolidayInn", 3000, 1500, 1800, new List<int>() { 260, 1300, 3900, 9000, 11000, 12750 }, 0, 0, 1750, -1, BusinessType.Hotels);
            Cells[32] = new UsualBus("RadissonBlu", 3000, 1500, 1800, new List<int>() { 260, 1300, 3900, 9000, 11000, 12750 }, 0, 0, 1750, -1, BusinessType.Hotels);
            Cells[33] = new Chance("Chance");
            Cells[34] = new UsualBus("Novotel", 3200, 1600, 1920, new List<int>() { 280, 1500, 4500, 10000, 12000, 14000 }, 0, 0, 1750, -1, BusinessType.Hotels);
            Cells[35] = new CarBus("LandRover", 2000, 1000, 1200, new List<int>() { 250, 500, 1000, 2000 }, 0, 0, -1, BusinessType.Cars);
            Cells[36] = new Tax("BigTax", 2000);
            Cells[37] = new UsualBus("Apple", 3500, 1750, 2100, new List<int>() { 360, 1750, 5000, 11000, 13000, 15000 }, 0, 0, 2000, -1, BusinessType.Phones);
            Cells[38] = new Chance("Chance");
            Cells[39] = new UsualBus("Nokia", 4000, 2000, 2400, new List<int>() { 500, 2000, 6000, 14000, 17000, 20000 }, 0, 0, 2000, -1, BusinessType.Phones);
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
            for(int i = 0; i < Cells.Length; i++)
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
            for(int i = 0; i < Cells.Length; i++)
            {
                if (Cells[i].Name == cellName) return i;
            }
            throw new Exception("There is no cell with such name!");
        }

        public int GetTotalPriceForBuses(List<int> buses)
        {
            int res = 0;
            for(int i = 0; i < buses.Count; i++)
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
            for(int i = 0; i < indexes.Count; i++)
            {
                ((ParentBus)Cells[indexes[i]]).ChangeOwner(newOwnerIndex);
            }
        }


    }
}
