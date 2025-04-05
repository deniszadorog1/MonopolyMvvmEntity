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
        public void SetRandomChanceAction()
        {
            const int startVal = 1;
            const int moveBordValue = 1;

            //return (ChanceAction)RandomService.GetRandom(startVal, (int)ChanceAction.SkipMove + moveBordValue);
            _resChance = (ChanceAction)RandomService.GetRandom(startVal, (int)ChanceAction.SkipMove + moveBordValue);
            //return _resChance;
        }

        public ChanceAction GetChanceType()
        {
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
