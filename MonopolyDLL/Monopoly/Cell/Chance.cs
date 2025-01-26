using MonopolyEntity.Windows.UserControls.GameControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonopolyDLL.Monopoly.Enums;

namespace MonopolyDLL.Monopoly.Cell
{
    public class Chance : CellParent
    {
        private const int _littleWinMoney = 500;
        private const int _bigWinMoney = 1500;

        private const int _littleLoseMony = 500;
        private const int _bigLoseMony = 1500;

        private const int _stepForward = 1;
        private const int _stepBackward = -1;

        public Chance(string name)
        {
            Name = name;
        }

        private Random _rnd = new Random();
        public ChanceAction GetRandomChanceAction()
        {
            ChanceAction res = (ChanceAction)_rnd.Next(1, ((int)ChanceAction.GoToPrison + 1));
            return res;
        }

        public int GetLittleWinMoney()
        {
            return _littleWinMoney;
        }

        public int GetBigWinMoney()
        {
            return _bigWinMoney;
        }

        public int GetLittleLoseMoney()
        {
            return _littleLoseMony;
        }

        public int GetBigloseMoney()
        {
            return _bigLoseMony;
        }

        public int GetStepForward()
        {
            return _stepForward;
        }

        public int GetStepBackwards()
        {
            return _stepBackward;
        }




    }
}
