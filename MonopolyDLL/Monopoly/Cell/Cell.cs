using MonopolyDLL.Monopoly.Cell.Businesses;

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
            if (cell is RegularBusiness regularBusiness)
            {
                return new RegularBusiness(regularBusiness.Name, regularBusiness.Price, regularBusiness.DepositPrice, regularBusiness.RebuyPrice,
                    regularBusiness.PayLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                    regularBusiness.BuySellHouse, SystemParamsService.GetNumByName("NoOwnerIndex"), regularBusiness.BusinessType, false, regularBusiness.Id);
            }
            else if (cell is CarBusiness carBusiness)
            {
                return new CarBusiness(carBusiness.Name, carBusiness.Price, carBusiness.DepositPrice, carBusiness.RebuyPrice,
                    carBusiness.PayLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                    SystemParamsService.GetNumByName("NoOwnerIndex"), carBusiness.BusinessType, false, carBusiness.Id);
            }
            else if (cell is GameBusiness gameBusiness)
            {
                return new GameBusiness(gameBusiness.Name, gameBusiness.Price, gameBusiness.DepositPrice, gameBusiness.RebuyPrice,
                    gameBusiness.PayLevels, SystemParamsService.GetNumByName("MaxDepositCounter"), 0,
                    SystemParamsService.GetNumByName("NoOwnerIndex"), gameBusiness.BusinessType, false, gameBusiness.Id);
            }
            return cell;
        }

    }
}
