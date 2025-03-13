using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MonopolyDLL.Monopoly.Enums;
using MonopolyDLL.Services;

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

        public Chance(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public ChanceAction _resChance;
        public ChanceAction GetRandomChanceAction()
        {
            const int startVal = 1;
            const int moveBordValue = 1;
            _resChance = (ChanceAction)GetRandomServese.GetRandom(startVal, (int)ChanceAction.Tax + moveBordValue);

            //_resChance = ChanceAction.Pay1500;
            return _resChance;
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
