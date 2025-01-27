using MonopolyDLL.Monopoly.Cell;
using MonopolyDLL.Monopoly.Cell.AngleCells;
using MonopolyDLL.Monopoly.Cell.Bus;
using MonopolyDLL.Monopoly.Enums;
using MonopolyEntity.Windows.UserControls.GameControls;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations.Model;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Casino = MonopolyDLL.Monopoly.Cell.AngleCells.Casino;
using Chance = MonopolyDLL.Monopoly.Cell.Chance;
using Tax = MonopolyDLL.Monopoly.Cell.Tax;

using MonopolyDLL.Monopoly.TradeAction;
using System.Runtime.Remoting.Channels;

namespace MonopolyDLL.Monopoly
{
    public class Game
    {
        public List<User> Players { get; set; }
        public Board GameBoard { get; set; }
        public int StepperIndex { get; set; }
        public int DoublesCounter { get; set; }


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
            _firstCube = 6;// _rnd.Next(1, 7);
            _secondCube = 6;// _rnd.Next(1, 7);
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
            const int amountOfCell = 39;

            int tempPost = Players[StepperIndex].Position;
            int sumPoint = (tempPost + GetSumOfCubes());


            return sumPoint > lastCellIndex ?
                sumPoint - amountOfCell : sumPoint;
        }

        public void SetPlayerPosition()
        {
            const int lastCellIndex = 39;

            int sum = Players[StepperIndex].Position + GetSumOfCubes();
            int newPos = sum > lastCellIndex ? sum - lastCellIndex : sum;

            Players[StepperIndex].Position = newPos;
        }

        public void SetPlayerPositionAfterChanceMove(int newCellIndex)
        {
            Players[StepperIndex].Position = newCellIndex;
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
            else if (GameBoard.Cells[tempPos] is Start start)
            {
                //pay money for start action
                return CellAction.GotOnStart;
            }
            else if (GameBoard.Cells[tempPos] is Prison prison)
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

        public string GetTempPositionCellName()
        {
            return GameBoard.Cells[Players[StepperIndex].Position].Name;
        }

        public int GetBillForBusinessCell()
        {
            ParentBus bus = ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]);
            int payment = bus.PayLevels[bus.Level];

            if (bus is GameBus) payment *= (_firstCube + _secondCube);

            return payment;
        }

        public int GetSteppersMoney()
        {
            return Players[StepperIndex].AmountOfMoney;
        }

        public int GetPlayersMoney(int index)
        {
            return Players[index].AmountOfMoney;
        }

        public void BuyBusinessByStepper()
        {
            int pos = Players[StepperIndex].Position;
            int price = ((ParentBus)GameBoard.Cells[pos]).Price;
            Players[StepperIndex].AmountOfMoney -= price;

            ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]).OwnerIndex = StepperIndex;
        }

        public bool IfStepperHasEnoughMoneyToPayBill()
        {
            int bill = GetBillForBusinessCell();

            return Players[StepperIndex].AmountOfMoney >= bill;
        }

        public void PayBusBillByStepper()
        {
            int amount = GetBillForBusinessCell();
            Players[StepperIndex].AmountOfMoney -= amount;

            Players[((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]).OwnerIndex].AmountOfMoney += amount;
        }

        public int GetStartPriceOfBoughtBusinessByStepper()
        {
            ParentBus bus = ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]);
            return bus.PayLevels[bus.Level];
        }

        public void SetStartLevelOfBusinessForStepper()
        {
            ParentBus bus = ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]);
            bus.Level = 0;
        }

        public bool IfStepperHasEnughMoneyToBuyBus()
        {
            return Players[StepperIndex].AmountOfMoney >=
                ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]).Price;
        }

        public void ChangeStepper()
        {
            if (StepperIndex == Players.Count - 1)
            {
                StepperIndex = 0;
                return;
            }
            StepperIndex++;
        }

        public ActionAfterStepperChanged GetActionAfterStepperChanged()
        {
            if (Players[StepperIndex].IfSitInPrison)
            {
                return ActionAfterStepperChanged.PrisonQuerstion;
            }
            return ActionAfterStepperChanged.ThrowCubes;
        }

        public List<int> _playerIndxesForAuction = new List<int>();
        public int _prevBidderIndex = -1;
        public int _bidderIndex = -1;
        private int _tempBusPriceAuction = 0;
        private int _stepInAuction = 100;
        private bool _ifAnyBodyMadeAbidInAuction = false;

        public void SetStartPlayersForAuction()
        {
            ParentBus auctionBus = ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]);

            for (int i = 0; i < Players.Count; i++)
            {
                if (i != StepperIndex && Players[i].AmountOfMoney >= auctionBus.Price)
                {
                    _playerIndxesForAuction.Add(i);
                }
            }

            if (_playerIndxesForAuction.Count > 0)
            {
                _prevBidderIndex = StepperIndex;
                _bidderIndex = _playerIndxesForAuction.First();
            }
        }

        public string GetBidderLogin()
        {
            return Players[_bidderIndex].Login;
        }

        public bool RemoveAuctionBidderIfItsWasLast()
        {
            _prevBidderIndex = _bidderIndex;
            int tempIndex = _playerIndxesForAuction.FindIndex(x => x == _bidderIndex);

            _playerIndxesForAuction.Remove(_bidderIndex);
            if (_playerIndxesForAuction.Count == 0)
            {
                return true;
            }

            if (tempIndex >= _playerIndxesForAuction.Count)
            {
                _bidderIndex = _playerIndxesForAuction.First();
                return false;
            }
            _bidderIndex = _playerIndxesForAuction[tempIndex];
            return false;

        }

        public void SetNextBidder()
        {
            _prevBidderIndex = _bidderIndex;
            int tempIndex = _playerIndxesForAuction.FindIndex(x => x == _bidderIndex);

            if (_playerIndxesForAuction.Count == 0) return;
            if (_playerIndxesForAuction.Count == 1 ||
                _playerIndxesForAuction.Count - 1 <= tempIndex)
            {
                _bidderIndex = _playerIndxesForAuction.First();
            }
            else if (_playerIndxesForAuction.Count - 1 > tempIndex)
            {
                _bidderIndex = _playerIndxesForAuction[++tempIndex];
            }
        }

        public bool IfBidderHasEnoughMoneyToPlaceBid()
        {
            return Players[_bidderIndex].AmountOfMoney >= _tempBusPriceAuction;
        }

        public bool IfLeftedPlayerIsWonAuction()
        {
            return _ifAnyBodyMadeAbidInAuction;
        }

        public bool IfSomeoneIsLeftInAuction()
        {
            return _playerIndxesForAuction.Any();
        }

        public void SetStartAuctionPrice()
        {
            _tempBusPriceAuction = (((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]).Price + _stepInAuction);
        }

        public int GetTempPriceInAuction()
        {
            return _tempBusPriceAuction;
        }

        public void MakeBidInAuction()
        {
            _ifAnyBodyMadeAbidInAuction = true;
            _tempBusPriceAuction += _stepInAuction;
        }

        public bool IfSomeOneWonAuction()
        {
            if (_playerIndxesForAuction.Count == 1 &&
                _ifAnyBodyMadeAbidInAuction)
            {
                AssignBoughtBusinessInAuction();
                return true;
            }
            return false;
        }

        public void AssignBoughtBusinessInAuction()
        {
            int pos = Players[StepperIndex].Position;
            ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]).OwnerIndex = _bidderIndex;
            Players[_bidderIndex].AmountOfMoney -= _tempBusPriceAuction;
        }

        public void ClearAuctionValues()
        {
            _ifAnyBodyMadeAbidInAuction = false;
            _bidderIndex = -1;
            _prevBidderIndex = -1;
            _tempBusPriceAuction = 0;
            _playerIndxesForAuction = new List<int>();
        }

        public void GetMoneyFromAuctionWinner()
        {
            ParentBus wonBus = ((ParentBus)GameBoard.Cells[Players[StepperIndex].Position]);

            Players[_playerIndxesForAuction.First()].AmountOfMoney -= _tempBusPriceAuction;
            wonBus.OwnerIndex = _playerIndxesForAuction.First();
        }

        public int GetBillFromTaxStepperPosition()
        {
            Tax tax = ((Tax)GameBoard.Cells[Players[StepperIndex].Position]);
            return tax.TaxBill;
        }

        public bool IfPlayerHasEnoughMoneyToPayBill()
        {
            int taxBill = ((Tax)GameBoard.Cells[Players[StepperIndex].Position]).TaxBill;
            return Players[StepperIndex].AmountOfMoney >= taxBill;
        }

        public bool IfStepperHasEnoughMoneyToPay(int bill)
        {
            return Players[StepperIndex].AmountOfMoney >= bill;
        }

        public void PayBillByStepper(int bill)
        {
            Players[StepperIndex].AmountOfMoney -= bill;
        }

        public void PayTaxByStepper()
        {
            int taxBill = ((Tax)GameBoard.Cells[Players[StepperIndex].Position]).TaxBill;
            Players[StepperIndex].AmountOfMoney -= taxBill;
        }

        public string PlayCasino(List<int> chosenCasinoRibs)
        {
            (int, string) casinoResult =
                GameBoard.PlayCasino(chosenCasinoRibs);

            AddMoneyToStepper(casinoResult.Item1);

            return casinoResult.Item2;
        }

        public void AddMoneyToStepper(int money)
        {
            Players[StepperIndex].AmountOfMoney += money;
        }

        public bool IfPlayerHasEnoughMoneyToPlayCasino()
        {
            return Players[StepperIndex].AmountOfMoney >= GameBoard.GetCasinoGamePrice();
        }

        public void GetBillForCasino()
        {
            Players[StepperIndex].AmountOfMoney -= GameBoard.GetCasinoGamePrice();
        }

        public int GetCasinoPlayerPrice()
        {
            return GameBoard.GetCasinoGamePrice();
        }

        public ChanceAction GetChanceAction()
        {
            ChanceAction action = GameBoard.GetChanceAction(Players[StepperIndex].Position);

            return action;
        }

        public void GetMoneyFromChance(int money)
        {
            Players[StepperIndex].AmountOfMoney += money;
        }

        public int GetIndexToStepOnForChance(ChanceAction action)
        {
            int step = action == ChanceAction.ForwardInOne ?
                GameBoard.GetStepForwardChance() : GameBoard.GetStepBackwardChance();

            return Players[StepperIndex].Position + step;
        }

        public bool IfIndexAndStepperIndexAreEqual(int index)
        {
            return index == StepperIndex;
        }

        public string GetStepperLogin()
        {
            return Players[StepperIndex].Login;
        }

        public string GetPlayerLoginByIndex(int index)
        {
            return Players[index].Login;
        }

        public bool IfCellIndexIsBusiness(int cellIndex)
        {
            return GameBoard.Cells[cellIndex] is ParentBus;
        }

        public bool IfSteperOwnsBusiness(int busIndex)
        {
            return GameBoard.IfPlayerOwnsBusiness(StepperIndex, busIndex);
        }

        public bool IfPlayerOwnsBusiness(int playerIndex, int busIndex)
        {
            return GameBoard.IfPlayerOwnsBusiness(playerIndex, busIndex);
        }

        public ParentBus GetBusinessByIndex(int cellIndex)
        {
            return GameBoard.GetBusinessByIndex(cellIndex);
        }

        public TradeClass _trade;
        public void CreateTrade()
        {
            _trade = new TradeClass();

            _trade.SetSenderIndex(StepperIndex);
        }

        public void SetTradeReciverIndex(int reciverIndex)
        {
            _trade.SetReciverIndex(reciverIndex);  
        }

        public void AddBusinesInTrade(int busIndex)
        {
            bool ifSendersBus = GameBoard.IfPlayerOwnsBusiness(StepperIndex, busIndex);
            _trade.AddCellIndexInTrade(busIndex, ifSendersBus);
        }

        public void RemoveBusinessFromTrade(int busIndex)
        {
            _trade.RemoveCellIndexFromTrade(busIndex);
        }

        public int GetCellIndexByName(string name)
        {
            return GameBoard.GetCellIndexByName(name);
        }

        public int GetSenderTotalMoneyForTrde()
        {
            List<int> cellIndexes = _trade.GetSenderCellsIndexes();
            int totalBusesPrice = GameBoard.GetTotalPriceForBuses(cellIndexes);

            int money = _trade.GetSenderMoney();
            return (totalBusesPrice + money);
        }

        public int GetReciverTotalMoneyForTrade()
        {
            List<int> cellIndexes = _trade.GetReciverIndexes();
            int totalBusesPrice = GameBoard.GetTotalPriceForBuses(cellIndexes);

            int money = _trade.GetReciverMoney();
            return (totalBusesPrice + money);
        }

        public bool IfTwiceRuleinTradeComplite()
        {
            return _trade.IfTwiceRuleIsComplited(
                GetSenderTotalMoneyForTrde(), GetReciverTotalMoneyForTrade());
        }

        public void SetSenderMoneyTrade(int money)
        {
            _trade.SetSenderMoney(money);
        }

        public void SetReciverMoneyTrade(int money)
        {
            _trade.SetReciverMoney(money);
        }

        public void AcceptTrade()
        {
            //Chane cells owner
            GameBoard.SetNewOwnerAfterTrade(_trade.GetSenderCellsIndexes(), _trade.ReciverIndex);
            GameBoard.SetNewOwnerAfterTrade(_trade.GetReciverIndexes(), _trade.SenderIndex);

            //money
            GiveMoney(_trade.SenderIndex, _trade.ReciverIndex, _trade.GetSenderMoney());
            GiveMoney(_trade.ReciverIndex, _trade.SenderIndex, _trade.GetReciverMoney());

        }

        public void GiveMoney(int giverIndex, int reciverIndex, int money)
        {
            Players[giverIndex].PayMoney(money);
            Players[reciverIndex].GetMoney(money);
        }



    }
}
