using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Start : Cell
    {
        private const int _paymentToGetOn = 3000;
        private const int _paymentToGetThrough = 2000;

        public Start(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int GetGeOnCellMoney()
        {
            return _paymentToGetOn;
        }

        public int GetGoThroughMoney()
        {
            return _paymentToGetThrough;
        }
    }
}
