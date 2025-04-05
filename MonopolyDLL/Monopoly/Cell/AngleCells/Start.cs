namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Start : Cell
    {
        private readonly int _paymentToGetOn = SystemParamsService.GetNumByName("GetOnStartMoney");// 3000;
        private readonly int _paymentToGetThrough = SystemParamsService.GetNumByName("GoThroughStartMoney");//2000;

        public Start(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int GetGeOnCellMoney()
        {
            return _paymentToGetOn;
        }

        public int GetGoThroughMoney()
        {
            return _paymentToGetThrough;
        }
    }
}
