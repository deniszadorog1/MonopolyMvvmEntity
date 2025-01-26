using MonopolyDLL.Monopoly.Enums;
using MonopolyEntity.Windows.UserControls.GameControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell.Bus
{
    public class ParentBus : CellParent
    {
        public int Price { get; set; }
        public int DepositPrice { get; set; }
        public int RebuyPrice { get; set; }
        public List<int> PayLevels { get; set; }
        public int DepositCounter { get; set; }
        public int Level { get; set; }
        public int OwnerIndex { get; set; }
        public BusinessType BusType { get; set; }

        public int _depositCounterMax = 15;

    }
}
