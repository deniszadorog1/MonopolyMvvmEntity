using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Casino : CellParent
    {
        private const int _gamePrice = 1000;
        private int _winValue;

        private List<int> _wonLevels = new List<int>();

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
            _winValue = _rnd.Next(SystemParamsServeses.GetNumByName("minCubeRib"), 
                SystemParamsServeses.GetNumByName("maxCubeRub"));
        }

        public bool IfPlayerWonInCasino(List<int> chosenVals)
        {
            return (chosenVals.Contains(_winValue));
        }

        private int _firstLevel = 6000;
        private int _secondLevel = 4000;
        private int _thirdLevel = 2000;

        public void SetWonLevels()
        {
            _wonLevels.Clear();
            _wonLevels.Add(_firstLevel);
            _wonLevels.Add(_secondLevel);
            _wonLevels.Add(_thirdLevel);
        }

        public int Play(List<int> chosenRibs)
        {
            SetWonLevels();
            SetCasinoWinValue();
            bool ifWon = IfPlayerWonInCasino(chosenRibs);

            return (chosenRibs.Count == 1 && ifWon) ? _wonLevels[0] :
                (chosenRibs.Count == 2 && ifWon) ? _wonLevels[1] :
                (chosenRibs.Count == 3 && ifWon) ? _wonLevels[2] : 0;
        }

        public int GetWinValue()
        {
            return _winValue;
        }

        public string GetResultMessage(int wonMoney)
        {
            string noWon = $"{SystemParamsServeses.GetStringByName("CasinoLost")}{GetConvertedPrice(_winValue)}";
            string bigWin = $"{SystemParamsServeses.GetStringByName("CasinoWon")}{GetConvertedPrice(_wonLevels.First())}," +
                $" {SystemParamsServeses.GetStringByName("CasinoWonNumberMessage")}{_winValue}";
            string middleWin = $"{SystemParamsServeses.GetStringByName("CasinoWon")}{GetConvertedPrice(_wonLevels[1])}, " +
                $"{SystemParamsServeses.GetStringByName("CasinoWonNumberMessage")}{_winValue}";
            string littleWin = $"{SystemParamsServeses.GetStringByName("CasinoWon")}{GetConvertedPrice(_wonLevels.Last())}, " +
                $"{SystemParamsServeses.GetStringByName("CasinoWonNumberMessage")}{_winValue}";

            return _wonLevels.First() == wonMoney ? bigWin :
                   _wonLevels[1] == wonMoney ? middleWin :
                   _wonLevels.Last() == wonMoney ? littleWin : noWon;
        }

        public string GetConvertedPrice(int price)
        {
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < price.ToString().Length; i++)
            {
                build.Append(price.ToString()[i]);
            }

            for (int i = price.ToString().Length; i >= 0; i--)
            {
                if (i % 3 == 0 && i != 0 && i != price.ToString().Length)
                {
                    build.Insert(price.ToString().Length - i, SystemParamsServeses.GetStringByName("MoneyDevider"));
                }
            }

            build.Append(SystemParamsServeses.GetStringByName("MoneyLastCur"));
            return build.ToString();
        }

        public int GetGamePrice()
        {
            return _gamePrice;
        }
    }
}
