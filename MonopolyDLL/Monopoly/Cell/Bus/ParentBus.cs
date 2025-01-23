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
        public int Price;
        public int DepositPrice;
        public int RebuyPrice;
        public List<int> PayLevels;
        public int DepositCounter;
        public int Level;
        public int OwnerIndex;
        public BusinessType BusType;

        public int _depositCounterMax = 15;

    }
}
