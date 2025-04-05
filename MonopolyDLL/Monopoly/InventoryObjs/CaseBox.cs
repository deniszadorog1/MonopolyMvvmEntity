using System.Collections.Generic;

namespace MonopolyDLL.Monopoly.InventoryObjs
{
    public class CaseBox : Item
    {
        public List<BoxItem> ItemsThatCanDrop { get; set; }

        public CaseBox()
        {
            Name = string.Empty;
            ImagePath = string.Empty;
            IsBox = true;
        }

        public CaseBox(string name, string imagePath, List<BoxItem> items)
        {
            Name = name;
            ImagePath = imagePath;
            IsBox = true;
            ItemsThatCanDrop = items;
        }

        public CaseBox(string name, string imagePath)
        {
            Name = name;
            ImagePath = imagePath;
            IsBox = true;
        }
    }
}
