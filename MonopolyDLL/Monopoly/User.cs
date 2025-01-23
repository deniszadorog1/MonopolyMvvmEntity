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
        public string Login;
        public int AmountOfMoney;
        public int Position;
        public bool IfSitInPrison;


        private int _sitInPrisonCounter = 0;
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
    }
}
