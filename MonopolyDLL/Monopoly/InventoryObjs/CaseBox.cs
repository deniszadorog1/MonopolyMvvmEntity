using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.InventoryObjs
{
    public class CaseBox : Item
    {
        private List<BoxItem> ItemsThatCanDrop { get; set; }


        public CaseBox()
        {
            Name = string.Empty;
            ImagePath = string.Empty;
            IsBox = true;
        }

        public CaseBox(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
            IsBox = true;
        }
    }
}
