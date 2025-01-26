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

        public Casino(string name)
        {
            Name = name;
        }

        public int GetCasinoGamePrice()
        {
            return _gamePrice;
        }

        private Random _rnd = new Random();
        public void SetCasinoWinValue()
        {
            _winValue = _rnd.Next(1, 7);
        }

        public bool IfPlayerWonInCasino(List<int> chosenVals)
        {
            return (chosenVals.Contains(_winValue));
        }

        private int _firstLevel = 6000;
        private int _secondLevel = 3000;
        private int _thirdLevel = 1000;

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

        public string GetResultMessage(int wonMoney)
        {
            string noWon = $"You lost( Won number - {_winValue}";
            string bigWin = $"You won - {_wonLevels.First()}, Won number - {_winValue}";
            string middleWin = $"You won - {_wonLevels[1]}, Won number - {_winValue}";
            string littleWin = $"You won - {_wonLevels.Last()}, Won number - {_winValue}";

            return _wonLevels.First() == wonMoney ? bigWin :
                   _wonLevels[1] == wonMoney ? middleWin :
                   _wonLevels.Last() == wonMoney ? littleWin : noWon;
        }

        public int GetGamePrice()
        {
            return _gamePrice;
        }
    }
}
