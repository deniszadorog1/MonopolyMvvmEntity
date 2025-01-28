using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Prison : CellParent
    {
        private const int _outPrisonPrice = 500;
        private const int _maxSittingRounds = 3;

        public Prison(string name)
        {
            Name = name;
        }

        public int GetOutPrisonPrice()
        {
            return _outPrisonPrice;
        }

        public int GetMaxSittingRounds()
        {
            return _maxSittingRounds;
        }
    }
}
