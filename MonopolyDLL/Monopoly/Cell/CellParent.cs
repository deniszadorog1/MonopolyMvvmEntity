using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell
{
    public class CellParent
    {
        public string Name { get; set; }
        protected int Id { get; set; }


        public int GetId()
        {
            return Id;
        }

    }
}
