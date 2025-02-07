using MonopolyDLL.Monopoly.InventoryObjs;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

using MonopolyDLL.DBService;
using System.Dynamic;

namespace MonopolyDLL.Monopoly
{
    public class MonopolySystem
    {
        public User LoggedUser { get; set; }
        public Game MonGame { get; set; }

        public MonopolySystem()
        {
            LoggedUser = new User();
            MonGame = new Game();

            List<InventoryObjs.Item> items = DBQueries.GetUserItemsFromInventory(1);

            UserInventory inventory = new UserInventory(items);
            LoggedUser = DBQueries.GetPlayerById(1);
            LoggedUser.Inventory = inventory;
            return;
            //UserInventory = DBQueries.GetInventoryItems(LoggedUser.Login);
        }   
    }
}
