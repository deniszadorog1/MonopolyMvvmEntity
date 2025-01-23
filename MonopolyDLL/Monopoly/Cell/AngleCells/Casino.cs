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

        public Casino(string name)
        {
            Name = name;
        }
    }
}
