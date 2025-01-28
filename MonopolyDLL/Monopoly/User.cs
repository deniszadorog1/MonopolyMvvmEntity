using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MonopolyDLL.Monopoly
{
    public class User
    {
        public string Login { get; set; }
        public int AmountOfMoney { get; set; }
        public int Position { get; set; }
        public bool IfSitInPrison { get; set; }
        public int SitInPrisonCounter { get; set; }

        public User()
        {
            Login = string.Empty;
            AmountOfMoney = 15000;
            Position = 0;
            IfSitInPrison = false;

        }

        public User(string login, int amountOfMoney, int position)
        {
            Login = login;
            AmountOfMoney = amountOfMoney;
            Position = position;
            IfSitInPrison = false;
        }

        public void PayMoney(int money)
        {
            AmountOfMoney -= money;
        }

        public void GetMoney(int money)
        {
            AmountOfMoney += money;
        }

        public bool IfPlayersInCellIndex(int cellIndex)
        {
            return Position == cellIndex;
        }

        public bool IfPlayerSitsInPrison()
        {
            return IfSitInPrison;
        }

        public void ClearSitInPrisonCounter()
        {
            SitInPrisonCounter = 0;
        }

        public void MakePrisonCounterHigher()
        {
            SitInPrisonCounter++;
        }

        public void ReverseSitInPrison()
        {
            IfSitInPrison = !IfSitInPrison;
        }

        public int GetPrisonCounter()
        {
            return SitInPrisonCounter;
        }
    }
}
