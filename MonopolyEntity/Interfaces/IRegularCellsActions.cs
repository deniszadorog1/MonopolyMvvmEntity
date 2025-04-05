using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace MonopolyEntity.Interfaces
{
    public interface IRegularCellsActions
    {
        void SetImagePlacerBg(SolidColorBrush brush);
        Grid GetImagePlacer();
        Grid GetStarGrid();
        TextBlock GetMoneyTextBlock();
        void SetValueForDepositCounter(int value);
        void ChangeDepositCounterVisibility(Visibility vis);
    }
}
