using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Prison : Cell
    {
        private const int _outPrisonPrice = 500;
        private readonly int _maxSittingRounds = SystemParamsService.GetNumByName("MaxSitInPrison");

        public Prison(string name, int id)
        {
            Name = name;
            Id = id;
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
