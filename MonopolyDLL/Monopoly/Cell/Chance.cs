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
    public class Chance : Cell
    {
        private readonly int _littleWinMoney = SystemParamsService.GetNumByName("LittleMoneyChance");
        private readonly int _bigWinMoney = SystemParamsService.GetNumByName("MuchMoneyChance");

        private readonly int _littleLoseMoney = SystemParamsService.GetNumByName("LittleMoneyChance");
        private readonly int _bigLoseMoney = SystemParamsService.GetNumByName("MuchMoneyChance");

        private readonly int _stepForward = SystemParamsService.GetNumByName("StepChance");
        private readonly int _stepBackward = -SystemParamsService.GetNumByName("StepChance");

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
            _resChance = (ChanceAction)GetRandomService.GetRandom(startVal, (int)ChanceAction.SkipMove + moveBordValue);

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
            return _littleLoseMoney;
        }

        public int GetBigLoseMoney()
        {
            return _bigLoseMoney;
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
