using MonopolyDLL.Monopoly.Cell.Bus;

namespace MonopolyDLL.Monopoly.Cell
{
    public class Cell
    {
        public string Name { get; set; }
        protected int Id { get; set; }

        public int GetId()
        {
            return Id;
        }

        public Cell GetCopy(Cell cell)
        {
            if (cell is RegularBusiness usual)
            {
                return new RegularBusiness(usual.Name, usual.Price, usual.DepositPrice, usual.RebuyPrice,
                    usual.PayLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                    usual.BuySellHouse, SystemParamsService.GetNumByName("NoOwnerIndex"), usual.BusType, false, usual.Id);
            }
            else if (cell is CarBusiness carBus)
            {
                return new CarBusiness(carBus.Name, carBus.Price, carBus.DepositPrice, carBus.RebuyPrice,
                    carBus.PayLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                    SystemParamsService.GetNumByName("NoOwnerIndex"), carBus.BusType, false, carBus.Id);
            }
            else if (cell is GameBusiness gameBus)
            {
                return new GameBusiness(gameBus.Name, gameBus.Price, gameBus.DepositPrice, gameBus.RebuyPrice,
                    gameBus.PayLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                    SystemParamsService.GetNumByName("NoOwnerIndex"), gameBus.BusType, false, gameBus.Id);
            }
            return cell;
        }

    }
}
