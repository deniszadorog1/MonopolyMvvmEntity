using MonopolyDLL.Monopoly.Cell;
using MonopolyDLL.Monopoly.Cell.AngleCells;
using MonopolyDLL.Monopoly.Cell.Bus;
using MonopolyDLL.Monopoly.Enums;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly
{
    public class Game
    {
        public List<User> Players;
        public Board GameBoard;
        public int StepperIndex;
        public int DoublesCounter;

        private int _firstCube = 1;
        private int _secondCube = 1;

        public Game()
        {
            Players = new List<User>()
            {
               new User("One", 15000, 0),
               new User("Two", 15000, 0),
               new User("Three", 15000, 0),
               new User("Four", 15000, 0),
               new User("Five", 15000, 0)
            };
            GameBoard = new Board();
            StepperIndex = 0;
            DoublesCounter = 0;
        }

        public Game(List<User> users)
        {
            Players = users;
            GameBoard = new Board();
            StepperIndex = 0;
            DoublesCounter = 0;
        }

        public List<int> GetUniquePositions()
        {
            List<int> res = new List<int>();

            for (int i = 0; i < Players.Count; i++)
            {
                if (!res.Contains(Players[i].Position))
                {
                    res.Add(Players[i].Position);
                }
            }
            return res;
        }

        public int GetAmountOfPlayersInCell(int cellIndex)
        {
            return Players.Where(x => x.Position == cellIndex).Count();
        }

        Random _rnd = new Random(Guid.NewGuid().GetHashCode());
        public void DropCubes()
        {
            _firstCube = _rnd.Next(0, 6);
            _secondCube = _rnd.Next(0, 6);
        }

        public int GetFirstCubes()
        {
            return _firstCube;
        }

        public int GetSecondCube()
        {
            return _secondCube;
        }

        public int GetSumOfCubes()
        {
            return _firstCube + _secondCube;
        }

        public int GetPointToMoveOn()
        {
            const int lastCellIndex = 39;
            const int amountOfCell = 40;

            int tempPost = Players[StepperIndex].Position;
            int sumPoint = (tempPost + GetSumOfCubes());


            return sumPoint > lastCellIndex ?
                sumPoint - amountOfCell : sumPoint;
        }

        public int GetStepperPosition()
        {
            return Players[StepperIndex].Position;
        }

        public CellAction GetAction()
        {
            CellAction res = new CellAction();

            int tempPos = Players[StepperIndex].Position;

            //Get on business
            if (GameBoard.Cells[tempPos] is ParentBus parentBus)
            {
                if (parentBus.OwnerIndex == -1)//free bus, can be bought
                {
                    // return to buyBusinessAvtion
                    return CellAction.GotOnBusinessToBuy;
                }
                else if (parentBus.OwnerIndex == StepperIndex)//players own bus 
                {
                    //return to send message in chat action
                    return CellAction.GotOnOwnBusiness;
                }
                else if (parentBus.OwnerIndex != StepperIndex)//enemy owns bus
                {
                    //return pay bill action
                    return CellAction.GotOnEnemysBusiness;
                }
            }
            else if (GameBoard.Cells[tempPos] is Tax tax)
            {
                //return tax pay action
                return CellAction.GotOnTax;
            }
            else if (GameBoard.Cells[tempPos] is Casino)
            {
                //casino action
                return CellAction.GotOnCasino;
            }
            else if (GameBoard.Cells[tempPos] is GoToPrison goToPrison)
            {
                //tp to prison action
                return CellAction.GotOnGoToPrison;
            }
            else if (GameBoard.Cells[tempPos] is Chance chance)
            {
                //return chance action
                return CellAction.GotOnChance;
            }
            else if(GameBoard.Cells[tempPos] is Start start)
            {
                //pay money for start action
                return CellAction.GotOnStart;
            }
            else if(GameBoard.Cells[tempPos] is Prison prison)
            {
                if (Players[StepperIndex].IfSitInPrison)
                {
                    //return sit in priosn action
                    return CellAction.GotOnGoToPrison;
                }
                else
                {
                    //vist priosn messaage
                    return CellAction.VisitPrison;
                }
            }
            return res;
        }

      


        
    }
}
