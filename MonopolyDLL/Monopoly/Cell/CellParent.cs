using MonopolyDLL.Monopoly.Cell.Bus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonopolyDLL.Monopoly.Cell
{
    public class CellParent
    {
        public string Name { get; set; }
        protected int Id { get; set; }

        public int GetId()
        {
            return Id;
        }

        public CellParent GetCopy(CellParent cell)
        {
            if (cell is UsualBus usual)
            {
                return new UsualBus(usual.Name, usual.Price, usual.DepositPrice, usual.RebuyPrice,
                    usual.PayLevels, 15, 0, usual.BuySellHouse, -1, usual.BusType, false, usual.Id);
            }
            else if (cell is CarBus carBus)
            {
                return new CarBus(carBus.Name, carBus.Price, carBus.DepositPrice, carBus.RebuyPrice,
                    carBus.PayLevels, 15, 0, -1, carBus.BusType, false, carBus.Id);
            }
            else if (cell is GameBus gameBus)
            {
                return new GameBus(gameBus.Name, gameBus.Price, gameBus.DepositPrice, gameBus.RebuyPrice, 
                    gameBus.PayLevels, 15, 0, -1, gameBus.BusType, false, gameBus.Id);
            }

            return cell;
        } 

    }
}
