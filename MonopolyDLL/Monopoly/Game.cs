using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly
{
    public class Game
    {
        public List<User> Players;
        public Board GameBoard;
 
        public Game()
        {
            Players = new List<User>();
            GameBoard = new Board();
        }
        public Game(List<User> users)
        {
            Players = users;
            GameBoard = new Board();
        }

    }
}
