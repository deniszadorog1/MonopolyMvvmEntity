using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MonopolyDLL;
using MonopolyDLL.Monopoly.InventoryObjs;


namespace MonopolyEntity.Windows.UserControls.InventoryControls
{
    /// <summary>
    /// Логика взаимодействия для BussinessDescription.xaml
    /// </summary>
    public partial class BussinessDescription : UserControl
    {
        private BoxItem _boxItem;
        public BussinessDescription(BoxItem item)
        {
            _boxItem = item;
            InitializeComponent();

            SetParams();
        }

        public void SetParams()
        {
            ItemName.Text = _boxItem.Name;
            ItemType.Text = _boxItem.Type.ToString();
            
            CardPersErnings.Text = $"You will get more from this card in {_boxItem.Multiplier}%";
            ItemDesctiption.Text = "This is business card which you can use in game";

            ColType.Text = $"{DBQueries.GetBoxNameByItsDropItemName(_boxItem.Name)} collection";

        }
    }
}
