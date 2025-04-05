namespace MonopolyDLL.Monopoly.Cell.AngleCells
{
    public class Prison : Cell
    {
        private readonly int _outPrisonPrice = SystemParamsService.GetNumByName("OutOfPrisonPrice");// 500;
        private readonly int _maxSittingRounds = SystemParamsService.GetNumByName("MaxSitInPrison");

        public Prison(string name, int id)
        {
            Name = name;
            Id = id;
        }

        public int GetOutPrisonPrice()
        {
            return _outPrisonPrice;
        }

        public int GetMaxSittingRounds()
        {
            return _maxSittingRounds;
        }
    }
}
