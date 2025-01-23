using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly
{
    public class MonopolySystem
    {
        public User LoggedUser;
        public Game MonGame;

        public MonopolySystem()
        {
            LoggedUser = new User();
            MonGame = new Game();
        }
    }
}
