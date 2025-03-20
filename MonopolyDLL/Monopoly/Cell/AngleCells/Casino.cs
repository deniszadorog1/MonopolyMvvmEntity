using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Casino : Cell
    {
        private readonly int _gamePrice = SystemParamsService.GetNumByName("CasinoGamePrice");
        private int _winValue;

        //private List<int> _wonLevels = new List<int>();

        public Casino(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int GetCasinoGamePrice()
        {
            return _gamePrice;
        }

        private Random _rnd = new Random();
        public void SetCasinoWinValue()
        {
            _winValue = _rnd.Next(SystemParamsService.GetNumByName("MinCubeRib"), 
                SystemParamsService.GetNumByName("MaxCubeRub"));
        }

        public bool IsPlayerWonInCasino(List<int> chosenValues)
        {
            return (chosenValues.Contains(_winValue));
        }

        private readonly int _firstLevel = SystemParamsService.GetNumByName("CasinoWinThird");
        private readonly int _secondLevel = SystemParamsService.GetNumByName("CasinoWinSecond");
        private readonly int _thirdLevel = SystemParamsService.GetNumByName("CasinoWinFirst");

/*        public void SetWonLevels()
        {
            _wonLevels.Clear();
            _wonLevels.Add(_firstLevel);
            _wonLevels.Add(_secondLevel);
            _wonLevels.Add(_thirdLevel);
        }*/

        private const int secondWinIndex = 1;
        public int Play(List<int> chosenRibs)
        {
            const int oneChosenRib = 1;
            const int twoChosenRibs = 2;
            const int threeChosenRibs = 3;

            //SetWonLevels();
            SetCasinoWinValue();
            bool isWon = IsPlayerWonInCasino(chosenRibs);

            return (chosenRibs.Count == oneChosenRib && isWon) ? _firstLevel :
                (chosenRibs.Count == twoChosenRibs && isWon) ? _secondLevel :
                (chosenRibs.Count == threeChosenRibs && isWon) ? _thirdLevel : 0;
        }

        public int GetWinValue()
        {
            return _winValue;
        }

        public string GetResultMessage(int wonMoney)
        {
            string noWon = $"{SystemParamsService.GetStringByName("CasinoLost")}{GetConvertedPrice(_winValue)}";
            string bigWin = $"{SystemParamsService.GetStringByName("CasinoWon")}{GetConvertedPrice(_firstLevel)}," +
                $" {SystemParamsService.GetStringByName("CasinoWonNumberMessage")}{_winValue}";
            string middleWin = $"{SystemParamsService.GetStringByName("CasinoWon")}{GetConvertedPrice(_secondLevel)}, " +
                $"{SystemParamsService.GetStringByName("CasinoWonNumberMessage")}{_winValue}";
            string littleWin = $"{SystemParamsService.GetStringByName("CasinoWon")}{GetConvertedPrice(_thirdLevel)}, " +
                $"{SystemParamsService.GetStringByName("CasinoWonNumberMessage")}{_winValue}";

            return _firstLevel == wonMoney ? bigWin :
                   _secondLevel == wonMoney ? middleWin :
                   _thirdLevel == wonMoney ? littleWin : noWon;
        }

        public string GetConvertedPrice(int price)
        {
            const int commaDivider = 3;
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < price.ToString().Length; i++)
            {
                build.Append(price.ToString()[i]);
            }


            for (int i = price.ToString().Length - 1; i >= 1; i--)
            {
                if (i % commaDivider == 0)
                {
                    build.Insert(price.ToString().Length - i, SystemParamsService.GetStringByName("MoneyDivider"));
                }
            }

            build.Append(SystemParamsService.GetStringByName("MoneyLastCur"));
            return build.ToString();
        }

        public int GetGamePrice()
        {
            return _gamePrice;
        }
    }
}
