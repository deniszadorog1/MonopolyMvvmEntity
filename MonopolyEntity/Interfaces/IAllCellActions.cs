using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MonopolyEntity.Interfaces
{
    internal interface IAllCellActions
    {
        Size GetCellSize();
        Size GetActualCellSize();
        void AddChip(Image chip);
        Point GetCenterOfTheSquareForImage(Image img, int divider);
        int GetAmountOfItemsInCell();
    }
}
