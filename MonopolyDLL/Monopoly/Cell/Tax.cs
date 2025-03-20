using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell
{
    public class Tax : Cell
    {
        public int TaxBill { get; set; }

        public Tax(string name, int bill, int id)
        {
            Name = name;
            TaxBill = bill;
            Id = id;
        }
    }
}
