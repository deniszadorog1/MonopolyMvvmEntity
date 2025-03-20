using MonopolyDLL.Monopoly.Cell;
using MonopolyDLL.Monopoly.Cell.AngleCells;
using MonopolyDLL.Monopoly.Cell.Bus;
using MonopolyDLL.Monopoly.Enums;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using Casino = MonopolyDLL.Monopoly.Cell.AngleCells.Casino;
using Chance = MonopolyDLL.Monopoly.Cell.Chance;
using Tax = MonopolyDLL.Monopoly.Cell.Tax;

using MonopolyDLL.Monopoly.TradeAction;
using System.Dynamic;
using System.Security.Cryptography;
using MonopolyDLL.Services;

namespace MonopolyDLL.Monopoly
{
    public class Game
    {
        public List<User> Players { get; set; }
        public Board GameBoard { get; set; }
        public int StepperIndex { get; set; }
        public int DoublesCounter { get; set; }


        private int _firstCube = -1;
        private int _secondCube = -1;

        public Game(User loggedUser)
        {
            Players = new List<User>()
            {
               new User("One", SystemParamsService.GetNumByName("StartMoney"), 0, false),
               //loggedUser,
               new User("Two", SystemParamsService.GetNumByName("StartMoney"), 0, false),
               new User("Three", SystemParamsService.GetNumByName("StartMoney"), 0, false),
               new User("Four", SystemParamsService.GetNumByName("StartMoney"), 0, false),
               //new User("Five", SystemParamsService.GetNumByName("StartMoney"), 0, false)
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
            if (cellIndex == GameBoard.GetPrisonCellIndex()) return 0;
            return Players.Where(x => x.Position == cellIndex).Count();
        }

        Random _rnd;// = new Random(Guid.NewGuid().GetHashCode());

        public bool check = false;
        public void DropCubes()
        {
            _rnd = new Random();
            /*            if (StepperIndex == 0 && check == true)
                        {
                            _firstCube = 4;
                            _secondCube = 4;
                            return;
                        }
                        check = true;*/
            _firstCube = GetRandomCubeValue();
            _secondCube = GetRandomCubeValue();
        }

        public (int, int) GetValuesForPrisonDice()
        {
            //return (2, 2);
            //_rnd = new Random();

            return (GetRandomService.GetRandom(), GetRandomService.GetRandom());

            //return (GetRandomCubeValue(), GetRandomCubeValue());
        }

        public int GetRandomCubeValue()
        {
            //_rnd = new Random();
            return _rnd.Next(GetRandomService.GetRandom(), GetRandomService.GetRandom());
        }

        public void SetCubes(int firstCube, int secondCube)
        {
            _firstCube = firstCube;
            _secondCube = secondCube;
        }

/*        public void SetOppositeMoveBackwards()
        {
            Players[StepperIndex].SetOppositeMoveBackwardsVal();
        }*/

        public int GetFirstCube()
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
            if (Players[StepperIndex].IsNeedToMoveBackwards())
            {
                return MoveBackwards();
            }

            int lastCellIndex = GameBoard.Cells.Count() - 1; //39; //!
            int amountOfCell = GameBoard.Cells.Count();// 40; //!

            int tempPost = Players[StepperIndex].Position;
            int sumPoint = (tempPost + GetSumOfCubes());

            return sumPoint > lastCellIndex ?
                sumPoint - amountOfCell : sumPoint;
        }

        public void SetOppositeMoveBackwards()
        {
            if (!Players[StepperIndex].IsNeedToMoveBackwards()) return;
            Players[StepperIndex].SetOppositeMoveBackwardsVal();
        }


        public bool IsNeedToMoveBackwards()
        {
            return Players[StepperIndex].IsNeedToMoveBackwards();
        }

        private int MoveBackwards()
        {
             int amountOfCell = GameBoard.Cells.Count(); //40 

            int tempPost = Players[StepperIndex].Position;
            int sumPoint = (tempPost - GetSumOfCubes());


            return sumPoint >= 0 ? sumPoint : amountOfCell + sumPoint;
        }

        public void SetPlayerPosition(bool isGoesToPrison)
        {
            if (Players[StepperIndex].IsNeedToMoveBackwards())
            {
                Players[StepperIndex].Position = MoveBackwards();
                return;
            }
            int lastCellIndex = GameBoard.Cells.Count(); //40//!

            int sum = Players[StepperIndex].Position + GetSumOfCubes();
            int newPos = sum >= lastCellIndex ? sum - lastCellIndex : sum;

            Players[StepperIndex].Position = isGoesToPrison ? GetPrisonIndex() : newPos;
        }
        /// /////////////////////////////////////////////////////////////////////////////////

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
            if (GameBoard.Cells[tempPos] is Business parentBus)
            {
                if (parentBus.OwnerIndex == SystemParamsService.GetNumByName("NoOwnerIndex"))//free bus, can be bought
                {
                    // return to buyBusiness Action
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
                    return CellAction.GotOnEnemiesBusiness;
                }
            }
            else if (GameBoard.Cells[tempPos] is Tax)
            {
                //return tax pay action
                return CellAction.GotOnTax;
            }
            else if (GameBoard.Cells[tempPos] is Casino)
            {
                //casino action
                return CellAction.GotOnCasino;
            }
            else if (GameBoard.Cells[tempPos] is GoToPrison)
            {
                //tp to prison action
                return CellAction.GotOnGoToPrison;
            }
            else if (GameBoard.Cells[tempPos] is Chance)
            {
                //return chance action
                return CellAction.GotOnChance;
            }
            else if (GameBoard.Cells[tempPos] is Start)
            {
                //pay money for start action
                return CellAction.GotOnStart;
            }
            else if (GameBoard.Cells[tempPos] is Prison)
            {
                if (Players[StepperIndex].IsSitInPrison)
                {
                    //return sit in prison action
                    return CellAction.GotOnGoToPrison;
                }
                else
                {
                    //visit prison message
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
            Business bus = ((Business)GameBoard.Cells[Players[StepperIndex].Position]);
            int payment = bus.GetPayMoney();

            if (bus is GameBusiness) payment *= (_firstCube + _secondCube);

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
            int price = ((Business)GameBoard.Cells[pos]).Price;
            Players[StepperIndex].AmountOfMoney -= price;

            ((Business)GameBoard.Cells[Players[StepperIndex].Position]).OwnerIndex = StepperIndex;
            ((Business)GameBoard.Cells[Players[StepperIndex].Position]).IsDeposited = false;
        }

        public bool IsStepperHasEnoughMoneyToPayBill()
        {
            int bill = GetBillForBusinessCell();

            return Players[StepperIndex].AmountOfMoney >= bill;
        }

        public void PayBusBillByStepper()
        {
            int amount = GetBillForBusinessCell();
            Players[StepperIndex].AmountOfMoney -= amount;

            Players[((Business)GameBoard.Cells[Players[StepperIndex].Position]).OwnerIndex].AmountOfMoney += amount;
        }

        public void PayBirthdayMoneyByPlayer(int playerIndex)
        {
            int moneyToPay = SystemParamsService.GetNumByName("MoneyToPayOnBirthday");
            Players[playerIndex].PayMoney(moneyToPay);
        }

        public bool IsPlayerHasEnoughMoneyToPayBusBill()
        {
            int amount = GetBillForBusinessCell();
            return Players[StepperIndex].IsPlayerHasEnoughMoney(amount);
        }

        public bool IsPlayerHasEnoughMoneyToPayMoney()
        {
            if (GameBoard.Cells[Players[StepperIndex].Position] is Business)
            {
                return IsPlayerHasEnoughMoneyToPayBusBill();
            }
            else if (GameBoard.Cells[Players[StepperIndex].Position] is Chance chance)
            {
                int bill = chance._resChance == ChanceAction.Pay500 ?
                    chance.GetLittleLoseMoney() : chance.GetBigLoseMoney();
                return Players[StepperIndex].IsPlayerHasEnoughMoney(bill);
            }
            return Players[StepperIndex].IsPlayerHasEnoughMoney(GetBillFromTaxStepperPosition());
        }

        public int GetMoneyToPayForStepper()
        {
            //int position = Players[StepperIndex].Position;

            if (GameBoard.Cells[Players[StepperIndex].Position] is Business)
            {
                return GetBillForBusinessCell();
            }
            else if (GameBoard.Cells[Players[StepperIndex].Position] is Chance chance)
            {
                return chance._resChance == ChanceAction.Pay500 ?
                    chance.GetLittleLoseMoney() : chance.GetBigLoseMoney();
            }
            return GetBillFromTaxStepperPosition();
        }

        public bool IsStepperHasEnoughMoneyToPayBill(int money)
        {
            return Players[StepperIndex].IsPlayerHasEnoughMoney(money);
        }

        public int GetStartPriceOfBoughtBusinessByStepper()
        {
            Business bus = ((Business)GameBoard.Cells[Players[StepperIndex].Position]);
            return bus.PayLevels[bus.Level];
        }

        public int GetStartPaymentForBusByIndex(int index)
        {
            Business bus = ((Business)GameBoard.Cells[index]);
            return bus.IsDeposited ? 0 : bus.PayLevels[bus.Level];
        }

        public void SetStartLevelOfBusinessForStepper()
        {
            Business bus = ((Business)GameBoard.Cells[Players[StepperIndex].Position]);
            bus.Level = 0;
        }

        public bool IsStepperHasEnoughMoneyToBuyBus()
        {
            return Players[StepperIndex].AmountOfMoney >=
                ((Business)GameBoard.Cells[Players[StepperIndex].Position]).Price;
        }

        public bool _ifSkipped = false;
        public bool ChangeStepper()
        {
            bool isCircle = false;
            bool isTempStepper;// = true;

            do
            {
                isTempStepper = true;
                if (StepperIndex == Players.Count - 1)
                {
                    StepperIndex = 0;
                    isCircle = true;
                }
                else StepperIndex++;

                if (Players[StepperIndex].IsSleeping())
                {
                    isTempStepper = false;
                    SetSkipMoveOppositeForStepper();
                    _ifSkipped = true;
                }

            } while (Players[StepperIndex].IsLost || !isTempStepper);

            return isCircle;
        }

        public bool IsStepperLost()
        {
            return Players[StepperIndex].IsLost;
        }

        public ActionAfterStepperChanged GetActionAfterStepperChanged()
        {
            if (Players[StepperIndex].IsSitInPrison)
            {
                return ActionAfterStepperChanged.PrisonQuestion;
            }
            return ActionAfterStepperChanged.ThrowCubes;
        }

        public List<int> _playerIndexesForAuction = new List<int>();
        public int _prevBidderIndex = -1;
        public int _bidderIndex = -1;
        private int _tempBusPriceAuction = 0;
        private int _stepInAuction = 100;
        private bool _ifAnyBodyMadeAbidInAuction = false;

        public void SetStartPlayersForAuction()
        {
            Business auctionBus = ((Business)GameBoard.Cells[Players[StepperIndex].Position]);

            for (int i = 0; i < Players.Count; i++)
            {
                if (i != StepperIndex && Players[i].AmountOfMoney >= (auctionBus.Price + _stepInAuction) &&
                    !Players[i].IsLost)
                {
                    _playerIndexesForAuction.Add(i);
                }
            }

            if (_playerIndexesForAuction.Count > 0)
            {
                _prevBidderIndex = StepperIndex;
                _bidderIndex = _playerIndexesForAuction.First();
            }
        }

        public string GetBidderLogin()
        {
            return Players[_bidderIndex].Login;
        }

        public int GetBidderIndex()
        {
            return _bidderIndex;
        }

        public bool RemoveAuctionBidderIfItsWasLast()
        {
            _prevBidderIndex = _bidderIndex;
            int tempIndex = _playerIndexesForAuction.FindIndex(x => x == _bidderIndex);

            _playerIndexesForAuction.Remove(_bidderIndex);
            if (_playerIndexesForAuction.Count <= 1)
            {
                if (_playerIndexesForAuction.Count == 0) return true;
                _bidderIndex = _playerIndexesForAuction.First();
                return true;
            }

            if (tempIndex >= _playerIndexesForAuction.Count)
            {
                _bidderIndex = _playerIndexesForAuction.First();
                return false;
            }
            _bidderIndex = _playerIndexesForAuction[tempIndex];
            return false;
        }

        public void SetNextBidder()
        {
            _prevBidderIndex = _bidderIndex;
            int tempIndex = _playerIndexesForAuction.FindIndex(x => x == _bidderIndex);

            if (_playerIndexesForAuction.Count == 0) return;
            if (_playerIndexesForAuction.Count == 1 ||
                _playerIndexesForAuction.Count - 1 <= tempIndex)
            {
                _bidderIndex = _playerIndexesForAuction.First();
            }
            else if (_playerIndexesForAuction.Count - 1 > tempIndex)
            {
                _bidderIndex = _playerIndexesForAuction[++tempIndex];
            }
        }

        public bool IsBidderHasEnoughMoneyToPlaceBid()
        {
            return Players[_bidderIndex].AmountOfMoney >= _tempBusPriceAuction;
        }

        public bool IsLeftPlayerIsWonAuction()
        {
            return _ifAnyBodyMadeAbidInAuction;
        }

        public bool IsSomeoneIsLeftInAuction()
        {
            return _playerIndexesForAuction.Any();
        }

        public void SetStartAuctionPrice()
        {
            _tempBusPriceAuction = (((Business)GameBoard.Cells[Players[StepperIndex].Position]).Price + _stepInAuction);
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

        public bool IsSomeOneWonAuction()
        {
            const int toEndAuctionAmount = 1;
            if (_playerIndexesForAuction.Count == toEndAuctionAmount &&
                _ifAnyBodyMadeAbidInAuction)
            {
                AssignBoughtBusinessInAuction();
                return true;
            }
            return false;
        }

        public void AssignBoughtBusinessInAuction()
        {
            ((Business)GameBoard.Cells[Players[StepperIndex].Position]).OwnerIndex = _bidderIndex;
            Players[_bidderIndex].AmountOfMoney -= (_tempBusPriceAuction - _stepInAuction);
        }

        public void ClearAuctionValues()
        {
            _ifAnyBodyMadeAbidInAuction = false;
            _bidderIndex = SystemParamsService.GetNumByName("NoOwnerIndex");
            _prevBidderIndex = SystemParamsService.GetNumByName("NoOwnerIndex");
            _tempBusPriceAuction = 0;
            _playerIndexesForAuction = new List<int>();
        }

        public void GetMoneyFromAuctionWinner()
        {
            Business wonBus = ((Business)GameBoard.Cells[Players[StepperIndex].Position]);

            Players[_playerIndexesForAuction.First()].AmountOfMoney -= _tempBusPriceAuction;
            wonBus.OwnerIndex = _playerIndexesForAuction.First();
        }

        public int GetBillFromTaxStepperPosition()
        {
            Tax tax = ((Tax)GameBoard.Cells[Players[StepperIndex].Position]);
            return tax.TaxBill;
        }

        public bool IsPlayerHasEnoughMoneyToPayBill()
        {
            int taxBill = ((Tax)GameBoard.Cells[Players[StepperIndex].Position]).TaxBill;
            return Players[StepperIndex].AmountOfMoney >= taxBill;
        }

        public bool IsStepperHasEnoughMoneyToPay(int bill)
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

        public bool IsPlayerHasEnoughMoneyToPlayCasino()
        {
            return Players[StepperIndex].AmountOfMoney >= GameBoard.GetCasinoGamePrice();
        }

        public void GetBillForCasino()
        {
            Players[StepperIndex].AmountOfMoney -= GameBoard.GetCasinoGamePrice();
        }

        public int GetCasinoPrice()
        {
            return GameBoard.GetCasinoGamePrice();
        }

        public int GetCasinoPlayerPrice()
        {
            return GameBoard.GetCasinoGamePrice();
        }

        public ChanceAction GetChanceAction()
        {
            return GameBoard.GetChanceAction(Players[StepperIndex].Position);
        }

        public void GetMoneyFromChance(int money)
        {
            Players[StepperIndex].AmountOfMoney += money;
        }

        public bool IsIndexAndStepperIndexAreEqual(int index)
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

        public bool IsCellIndexIsBusiness(int cellIndex)
        {
            return GameBoard.Cells[cellIndex] is Business;
        }

        public bool IsCellIsGameOrCar(int cellIndex)
        {
            return GameBoard.Cells[cellIndex] is CarBusiness ||
                GameBoard.Cells[cellIndex] is GameBusiness;
        }

        public bool IsCellIsPlayersBusiness(int cellIndex, int playerIndex)
        {
            return GameBoard.Cells[cellIndex] is Business &&
                ((Business)GameBoard.Cells[cellIndex]).OwnerIndex == playerIndex;
        }

        public bool IsCellIsUsualBusiness(int cellIndex)
        {
            return GameBoard.Cells[cellIndex] is RegularBusiness;
        }

        public bool IsStepperOwnsBusiness(int busIndex)
        {
            return GameBoard.IsPlayerOwnsBusiness(StepperIndex, busIndex);
        }

        public bool IsPlayerOwnsBusiness(int playerIndex, int busIndex)
        {
            return GameBoard.IsPlayerOwnsBusiness(playerIndex, busIndex);
        }

        public Business GetBusinessByIndex(int cellIndex)
        {
            return GameBoard.GetBusinessByIndex(cellIndex);
        }

        public TradePerformance _trade;
        public void CreateTrade()
        {
            _trade = new TradePerformance();
            _trade.SetSenderIndex(StepperIndex);
        }

        public int GetTradeSenderMaxMoney()
        {
            return Players[_trade.SenderIndex].AmountOfMoney;
        }

        public int GetTradeReceiverMaxMoney()
        {
            return Players[_trade.ReceiverIndex].AmountOfMoney;
        }

        public void SetTradeReceiverIndex(int receiverIndex)
        {
            _trade.SetReceiverIndex(receiverIndex);
        }

        public void AddBusinessInTrade(int busIndex)
        {
            bool isSendersBus = GameBoard.IsPlayerOwnsBusiness(StepperIndex, busIndex);
            _trade.AddCellIndexInTrade(busIndex, isSendersBus);
        }

        public void RemoveBusinessFromTrade(int busIndex)
        {
            _trade.RemoveCellIndexFromTrade(busIndex);
        }

        public int GetCellIndexByName(string name)
        {
            return GameBoard.GetCellIndexByName(name);
        }

        public int GetSenderTotalMoneyForTrade()
        {
            List<int> cellIndexes = _trade.GetSenderCellsIndexes();
            int totalBusesPrice = GameBoard.GetTotalPriceForBuses(cellIndexes);

            int money = _trade.GetSenderMoney();
            return (totalBusesPrice + money);
        }

        public int GetReceiverTotalMoneyForTrade()
        {
            List<int> cellIndexes = _trade.GetReceiverIndexes();
            int totalBusesPrice = GameBoard.GetTotalPriceForBuses(cellIndexes);

            int money = _trade.GetReceiverMoney();
            return (totalBusesPrice + money);
        }

        public bool IsTwiceRuleInTradeComplete()
        {
            return _trade.IsTwiceRuleIsCompleted(
                GetSenderTotalMoneyForTrade(), GetReceiverTotalMoneyForTrade());
        }

        public void SetSenderMoneyTrade(int money)
        {
            _trade.SetSenderMoney(money);
        }

        public TradePerformance GetTrade()
        {
            return _trade;
        }

        public void SetReceiverMoneyTrade(int money)
        {
            _trade.SetReceiverMoney(money);
        }

        public void AcceptTrade()
        {
            //Chane cells owner
            GameBoard.SetNewOwnerAfterTrade(_trade.GetSenderCellsIndexes(), _trade.ReceiverIndex);
            GameBoard.SetNewOwnerAfterTrade(_trade.GetReceiverIndexes(), _trade.SenderIndex);

            //money
            GiveMoney(_trade.SenderIndex, _trade.ReceiverIndex, _trade.GetSenderMoney());
            GiveMoney(_trade.ReceiverIndex, _trade.SenderIndex, _trade.GetReceiverMoney());
        }

        public void GiveMoney(int giverIndex, int receiverIndex, int money)
        {
            Players[giverIndex].PayMoney(money);
            Players[receiverIndex].GetMoney(money);
        }

        public VisualPrisonCellActions IfPlayerIsInPrison(int playerIndex)
        {
            return Players[playerIndex].IsPlayerSitsInPrison() ? VisualPrisonCellActions.SitInPrison :
            Players[playerIndex].Position == GameBoard.GetPrisonCellIndex() ? VisualPrisonCellActions.VisitPrison :
            VisualPrisonCellActions.OutOfPrison;
        }

        public bool IsPlayerSitsInPrison(int playerIndex)
        {
            return Players[playerIndex].IsSitInPrison;
        }

        public bool IsStepperHasEnoughMoneyToPayPrisonPrice()
        {
            return Players[StepperIndex].AmountOfMoney >= GameBoard.GetPrisonPrice();
        }

        public bool IsStepperSatInPrisonTooMuch()
        {
            return Players[StepperIndex].SitInPrisonCounter >= GameBoard.GetMaxSittingRoundsInPrison();
        }

        public bool IsStepperSitsInPrison()
        {
            return Players[StepperIndex].IsSitInPrison;
        }

        public void PayPrisonBill()
        {
            Players[StepperIndex].PayMoney(GameBoard.GetPrisonPrice());
        }

        public void ClearStepperSitInPrisonCounter()
        {
            Players[StepperIndex].ClearSitInPrisonCounter();
        }

        public void ClearStepperDoublesCounter()
        {
            Players[StepperIndex].ClearDoubleCounter();
        }

        public void MakeStepperPrisonCounterHigher()
        {
            Players[StepperIndex].MakePrisonCounterHigher();
        }

        public void ReverseStepperSitInPrison()
        {
            Players[StepperIndex].ReverseSitInPrison();
        }

        public int GetStepperPrisonCounter()
        {
            return Players[StepperIndex].GetPrisonCounter();
        }

        public int GetPrisonIndex()
        {
            return GameBoard.GetPrisonCellIndex();
        }

        public int GetGotOnStartCellMoney()
        {
            return GameBoard.GetGotOnStartCellMoney();
        }

        public int GetGoThroughStartCellMoney()
        {
            return GameBoard.GetGotThroughStartCellMoney();
        }

        public void GetMoneyByStepper(int money)
        {
            Players[StepperIndex].GetMoney(money);
        }

        public bool IsStepperWentThroughStartCell()
        {
            if (Players[StepperIndex].Position == GameBoard.GetStartCellIndex()) return false;
            return Players[StepperIndex].Position - _firstCube - _secondCube < 0;
        }

        public bool IsCellIsInMonopoly(int cellIndex)
        {
            //if its usual bus (only on usual bus houses can be built)
            if (!(GameBoard.Cells[cellIndex] is Business)) return false;

            //If player does not have such monopoly
            if (!Players[StepperIndex].GetCollectedMonopolies().Contains(
                GameBoard.GetBusTypeByIndex(cellIndex))) return false;

            return true;
        }

        public UsualBusInfoVisual? GetButsTypeVisibility(int cellIndex)
        {
            BusinessType type = GameBoard.GetBusTypeByIndex(cellIndex);

            //If cell IS deposited
            if (GameBoard.IsBusinessIsDeposited(cellIndex)) return UsualBusInfoVisual.Rebuy;
            //Check int cell can be ONLY Deposited
            if (IsCellOnlyCanBeDeposited(type)) return UsualBusInfoVisual.Deposit;

            //Monopoly
            if (IsBusCanBeDepositedAndBuildHouse(type) &&
                !IsTypeContainsInBuiltCells(type)) return UsualBusInfoVisual.DepositAndBuildHouse;

            if (GameBoard.IsBusLevelIsMax(cellIndex)) return UsualBusInfoVisual.SellHouse;
            if (GameBoard.IsBusDoesNotHaveHouses(cellIndex) &&
                !IsTypeContainsInBuiltCells(type)) return UsualBusInfoVisual.BuyHouse;

            bool isBuilt = IsTypeContainsInBuiltCells(GameBoard.GetBusTypeByIndex(cellIndex));

            UsualBusInfoVisual? vis = GameBoard.GetVisualBySettingNewAmountOfHouses(cellIndex, isBuilt);
            return vis;
        }

        public bool IsBusCanBeDepositedAndBuildHouse(BusinessType type)
        {
            List<Business> ownedBusesByType = GameBoard.GetBusesWhichPlayersOwnByType(type, StepperIndex);
            List<Business> allBusesByType = GameBoard.GetBusesByType(type);

            //Not all cells form monopoly
            if (ownedBusesByType.Count() != allBusesByType.Count()) return false;

            //If all cell from monopoly, and at least one is deposited
            if (GameBoard.IsAnyOfBussesIsDeposited(ownedBusesByType)) return false;

            //No one of them has house
            if (GameBoard.IsBusesHaveHouses(ownedBusesByType)) return false;
            return true;
        }

        public bool IsCellOnlyCanBeDeposited(BusinessType type)
        {
            List<Business> ownedBusesByType = GameBoard.GetBusesWhichPlayersOwnByType(type, StepperIndex);
            List<Business> allBusesByType = GameBoard.GetBusesByType(type);

            bool isHouseWasBuilt = Players[StepperIndex].BuiltHousesInRowType.Contains(type);

            //Not all cells form monopoly
            if (ownedBusesByType.Count() != allBusesByType.Count()) return true;

            //If all cell from monopoly, and at least one is deposited
            if (GameBoard.IsAnyOfBussesIsDeposited(ownedBusesByType)) return true;

            //House was built(than sold) + no houses in all buses by type
            if (isHouseWasBuilt && !allBusesByType.Where(x => x.Level != 0).Any()) return true;

            return false;
        }

        public int GetBusLevel(int cellIndex)
        {
            return GameBoard.GetBusLevel(cellIndex);
        }

        public StarType GetStarType(int cellIndex)
        {
            return GameBoard.IsBusLevelIsMax(cellIndex) ?
                StarType.YellowStar : StarType.SilverStar;
        }

        public bool IsPlayersHasEnoughMoneyToBuyHouse(int busIndex)
        {
            int housePrice = GameBoard.GetUsualBusHousePrice(busIndex);
            return Players[StepperIndex].IsPlayerHasEnoughMoney(housePrice);
        }

        public void BuyHouse(int busIndex)
        {
            int housePrice = GameBoard.GetUsualBusHousePrice(busIndex);
            GameBoard.BuyHouseUsualBus(busIndex);
            Players[StepperIndex].PayMoney(housePrice);
        }

        public void SellHouse(int busIndex)
        {
            int housePrice = GameBoard.GetUsualBusHousePrice(busIndex);
            GameBoard.SellHouseUsualBus(busIndex);
            Players[StepperIndex].GetMoney(housePrice);
        }

        public void SetMonopolies()
        {
            for (int i = 0; i < Players.Count; i++)
            {
                SetMonopolyForPlayer(i);
            }
        }

        public void SetMonopolyForPlayer(int playerIndex)
        {
            Players[playerIndex].ClearCollectedMonopolies();
            for (int i = (int)BusinessType.Perfume; i <= (int)BusinessType.Phones; i++)
            {
                if (GameBoard.IsPlayerHasMonopolyByType(playerIndex, (BusinessType)i))
                {
                    Players[playerIndex].AddMonopoly((BusinessType)i);
                }
            }
        }

        public int GetBusMoneyLevel(int cellIndex)
        {
            return GameBoard.GetBusMoneyLevel(cellIndex);
        }

        public void SetBusAsDeposited(int busIndex)
        {
            GameBoard.DepositBus(busIndex);
            Players[StepperIndex].GetMoney(GameBoard.GetDepositPrice(busIndex));
        }

        public void RebuyBus(int busIndex)
        {
            GameBoard.RebuyBus(busIndex);
            GameBoard.SetMaxDepositCounter(busIndex);
            Players[StepperIndex].PayMoney(GameBoard.GetRubyPrice(busIndex));
        }

        public bool IsPlayerCanRebuyBus(int cellIndex)
        {
            return Players[StepperIndex].IsPlayerHasEnoughMoney(GameBoard.GetRubyPrice(cellIndex));
        }

        public void SetCarsPaymentLevels(int playerIndex)
        {
            GameBoard.SetCarsPaymentLevelByIndex(playerIndex);
        }

        public void SetGamePaymentLevels(int playerIndex)
        {
            GameBoard.SetGamePaymentsLevelByIndex(playerIndex);
        }

        public List<int> GetGamesIndexesWhichPlayerOwnNotDeposited(int playerIndex)
        {
            return GameBoard.GetGamesWhichPlayerOwnNotDeposited(playerIndex);
        }

        public List<int> GetCarsIndexesWhichPlayerOwnNotDeposited(int playerIndex)
        {
            return GameBoard.GetCarsWhichPlayerOwnNotDeposited(playerIndex);
        }

        public CarGameInfoVisual GetGamesCarsButsVisual(int gameIndex)
        {
            return GameBoard.IsBusinessIsDeposited(gameIndex) ?
                CarGameInfoVisual.Rebuy : CarGameInfoVisual.Deposit;
        }

        public void PlayerGaveUp(int playerIndex)
        {
            GameBoard.ClearAllPlayersBuses(playerIndex);
            Players[playerIndex].IsLost = true;
        }

        public int GetAllPlayersActivitiesPrice(int playerIndex)
        {
            int priceForAllHouses = GameBoard.GetPriceOfAllBuiltHouses(playerIndex);
            int priceForDepositBuses = GameBoard.GetPriceForNotDepositedBuses(playerIndex);
            int playersMoney = Players[playerIndex].AmountOfMoney;

            int totalPrice = priceForAllHouses + priceForDepositBuses + playersMoney;

            return totalPrice;
        }

        public int GetBusPrice(int busIndex)
        {
            return GameBoard.GetBusPrice(busIndex);
        }

        public bool IsSomeOneWon()
        {
            const int amountOfPlayersToWin = 1;
            return Players.Where(x => x.IsLost == false).Count() == amountOfPlayersToWin;
        }

        public int GetWinnerIndex()
        {
            return Players.IndexOf(Players.Where(x => x.IsLost == false).First());
        }

        public int GetDepositCounter(int cellIndex)
        {
            return GameBoard.GetDepositCounter(cellIndex);
        }

        public bool IsDeposited(int cellIndex)
        {
            if (!(GameBoard.Cells[cellIndex] is Business)) return false;
            return GameBoard.IsBusinessIsDeposited(cellIndex);
        }

        public bool IsBusDepositCounterIsZero(int busIndex)
        {
            return GameBoard.IsDepositCounterIsZero(busIndex);
        }

        public void SetNewDepositCircle()
        {
            GameBoard.SetNewCircleOfDepositedBuses();
        }

        public void ClearBusiness(int busIndex)
        {
            Players[GameBoard.GetOwnerIndex(busIndex)].
                RemoveFromCollectedMonopolies(GameBoard.GetBusTypeByIndex(busIndex));
            GameBoard.ClearBusiness(busIndex);
            GameBoard.SetMaxDepositCounter(busIndex);

        }

        public bool IsCubeDropsAreEqual()
        {
            return _firstCube == _secondCube && _firstCube != -1 && _secondCube != -1;
        }

        public void AddToDoubleCounter()
        {
            Players[StepperIndex].AddToDoubleCounter();
        }

        public int GetDoubleCounter()
        {
            return Players[StepperIndex].GetDoubleCounter();
        }

        public bool IsMaxDoublesIsAchieved()
        {
            return Players[StepperIndex].IsMaxDoublesIsAchieved();
        }

        public List<Business> GetBusWithGivenBoxItem(InventoryObjs.BoxItem item)
        {
            return item.Type == BusinessType.Games ? GameBoard.GetAllGameBuses() :
                item.Type == BusinessType.Cars ? GameBoard.GetAllCarBuses() :
                GameBoard.GetBusesByType(item.Type);
        }

        public bool IsStepperHasInventoryBusOnPosition()
        {
            return Players[StepperIndex].IsHasInventoryOnPosition();
        }

        public void SetPlayerSteppersBus()
        {
            int position = Players[StepperIndex].Position;
            Business newBus = Players[StepperIndex].GetInventoryItemById(position, GameBoard.GetBusByIndex(position), StepperIndex);
            GameBoard.ChangeBoardItemOnInventory(newBus, position);
        }

        public void SetInventoryItemForPlayer(int playerIndex, int cellIndex)
        {
            Business newBus = Players[playerIndex].GetInventoryItemById(cellIndex, GameBoard.GetBusByIndex(cellIndex), playerIndex);
            GameBoard.ChangeBoardItemOnInventory(newBus, cellIndex);
        }

        public Business GetBusThatStepperIsOn()
        {
            int position = Players[StepperIndex].Position;
            return GameBoard.GetBusByIndex(position);
        }

        public InventoryObjs.BoxItem GetUserInventoryItem(int playerIndex, int id)
        {
            return Players[playerIndex].GetBoxItemByPosition(id);
        }

        public void SetBasicBusBack(int cellIndex)
        {
            GameBoard.GetBasicBusBack(cellIndex);
        }

        public bool IsPlayerHasInventoryItemOnIndex(int playerIndex, int cellIndex)
        {
            return Players[playerIndex].IsHasInventoryItemOnCellIndex(cellIndex);
        }

       /* public bool IsStepperHasEnoughMoneyToBuyBus()
        {
            int index = Players[StepperIndex].Position;

            return Players[StepperIndex].IfPlayerHasEnoughMoney(GameBoard.GetParentBusPrice(index));
        }
*/
        public bool IsPlayerHasEnoughMoneyToPAyPrisonBill()
        {
            int price = GameBoard.GetPrisonPrice();
            return Players[StepperIndex].IsPlayerHasEnoughMoney(price);
        }

        public bool IsStepperOnEnemiesBus()
        {
            int position = Players[StepperIndex].Position;
            return GameBoard.IsPlayerIsOnEnemiesBus(StepperIndex, position);
        }

        public void GiveAllSteppersMoneyToBusOwner()
        {
            int allPlayersMoney = GetAllPlayersActivitiesPrice(StepperIndex);

            int ownerIndex = GameBoard.GetBusOwnerIndex(Players[StepperIndex].Position);
            if (ownerIndex == SystemParamsService.GetNumByName("NoOwnerIndex")) return;

            int billMoney = GetPlayersBillMoney();

            if (allPlayersMoney >= billMoney)
            {
                allPlayersMoney = billMoney;
            }
            Players[ownerIndex].GetMoney(allPlayersMoney);
        }

        public void GiveAllMoneyToStepper(int playerIndex)
        {
            int allPlayersMoney = GetAllPlayersActivitiesPrice(playerIndex);

            int birthdayMoney = SystemParamsService.GetNumByName("MoneyToPayOnBirthday");

            allPlayersMoney = allPlayersMoney > birthdayMoney ? birthdayMoney : allPlayersMoney;

            Players[StepperIndex].GetMoney(allPlayersMoney);
        }

        public int GetPlayersBillMoney()
        {
            int position = Players[StepperIndex].Position;

            if (GameBoard.Cells[position] is Business bus)
            {
                return bus.PayLevels[bus.Level];
            }
            return 0;
        }

        public bool IsSteppersCanOnlyGiveUp(int bill)
        {
            int allPlayersMoney = GetAllPlayersActivitiesPrice(StepperIndex);
            return allPlayersMoney < bill;
        }

        public int GetPrisonPrice()
        {
            return GameBoard.GetPrisonPrice();
        }

        public bool IsStepperHasEnoughMoney(int money)
        {
            return Players[StepperIndex].IsPlayerHasEnoughMoney(money);
        }

        public bool IsTradersHasEnoughMoney()
        {
            return Players[_trade.ReceiverIndex].IsPlayerHasEnoughMoney(_trade.ReceiverMoney) &&
                Players[_trade.SenderIndex].IsPlayerHasEnoughMoney(_trade.SenderMoney);
        }

        public int GetCasinoWinValue()
        {
            return GameBoard.GetCasinoWinValue();
        }

        public int GetStepperBusPositionBusIndex()
        {
            int position = Players[StepperIndex].Position;
            return ((Business)GameBoard.Cells[position]).OwnerIndex;
        }

        public string GetBusOnStepperName()
        {
            return ((Business)GameBoard.Cells[Players[StepperIndex].Position]).Name;
        }

        public int GetSteppersBusPrice()
        {
            return ((Business)GameBoard.Cells[Players[StepperIndex].Position]).Price;
        }

        public int GetAuctionPrice()
        {
            return (_tempBusPriceAuction - _stepInAuction);
        }

        public string GetBusNameOnGivenIndex(int index)
        {
            return ((Business)GameBoard.Cells[index]).Name;
        }

        public void ClearStepperBoughtHouseTypes()
        {
            Players[StepperIndex].ClearBuiltHousesInRowType();
        }

        public void AddStepperBoughtHouseType(int busIndex)
        {
            if (!(GameBoard.Cells[busIndex] is Business)) return;
            Players[StepperIndex].AddTypeInBuiltHousesInRowType(((Business)GameBoard.Cells[busIndex]).BusType);
        }

        public bool IsTypeContainsInBuiltCells(BusinessType type)
        {
            return Players[StepperIndex].IsTypeContainsInBuiltHouses(type);
        }

        public int GetStepperPositionOwnerBus()
        {
            int pos = Players[StepperIndex].Position;
            return ((Business)GameBoard.Cells[pos]).OwnerIndex;
        }

        public bool IsStepperIsBusOwner(int cellIndex)
        {
            return ((Business)GameBoard.Cells[cellIndex]).OwnerIndex == StepperIndex;
        }

        public bool IsPlayerIsBusOwner(int playerIndex, int cellIndex)
        {
            return ((Business)GameBoard.Cells[cellIndex]).OwnerIndex == playerIndex;
        }

        public bool IsStepperPositionIsGame()
        {
            return GameBoard.Cells[Players[StepperIndex].Position] is GameBusiness;
        }

        public bool IsStepperPositionIsCar()
        {
            return GameBoard.Cells[Players[StepperIndex].Position] is CarBusiness;
        }

        public bool IsCellIsCarBus(int cellIndex)
        {
            return GameBoard.Cells[cellIndex] is CarBusiness;
        }

        public bool IsCellIsGame(int cellIndex)
        {
            return GameBoard.Cells[cellIndex] is GameBusiness;
        }

        public bool IsOneOfTheBusesContainsHouse(int cellIndex)
        {
            if (!(GameBoard.Cells[cellIndex] is RegularBusiness)) return false;

            BusinessType type = ((RegularBusiness)GameBoard.Cells[cellIndex]).BusType;
            List<RegularBusiness> toCheck = GameBoard.Cells.OfType<RegularBusiness>().Where(x => x.BusType == type).ToList();

            return toCheck.Any(x => x.Level != 0);
        }

        public int GetRandomCellIndex()
        {
            const int toGetLast = 1;
            _rnd = new Random();
            return _rnd.Next(0, GameBoard.Cells.Length + toGetLast);
        }

        public bool IsPlayerLost()
        {
            return Players[StepperIndex].IsLost;
        }

        public bool IsPlayerLost(int playerIndex)
        {
            return Players[playerIndex].IsLost;
        }

        public List<int> GetBirthdayToPayPlayersIndexes(List<int> playerIndexesThatPaidGift)
        {
            List<int> res = new List<int>();

            for (int i = 0; i < Players.Count; i++)
            {
                if (i != StepperIndex && !Players[i].IsLost &&
                    !playerIndexesThatPaidGift.Contains(i))
                {
                    res.Add(i);
                }
            }
            return res;
        }

        public int GetTaxMoneyForBuildings(int playerIndex)
        {
            int houseAmount = GetPlayersHouseAmount(playerIndex);
            int hotelsAmount = GetHotelsAmount(playerIndex);

            int housePrice = SystemParamsService.GetNumByName("HousePriceChance");
            int hotelPrice = SystemParamsService.GetNumByName("HotelPriceChance");

            int res = houseAmount * housePrice + hotelsAmount * hotelPrice;

            return res;
        }

        public int GetPlayersHouseAmount(int playerIndex)
        {
            int res = 0;
            const int maxLevel = 5;

            for (int i = 0; i < GameBoard.Cells.Length; i++)
            {
                if (GameBoard.Cells[i] is RegularBusiness usual &&
                    usual.OwnerIndex == playerIndex &&
                    usual.Level != maxLevel)
                {
                    res += usual.Level;
                }
            }
            return res;
        }

        public int GetHotelsAmount(int playerIndex)
        {
            int res = 0;
            const int maxLevel = 5;

            for (int i = 0; i < GameBoard.Cells.Length; i++)
            {
                if (GameBoard.Cells[i] is RegularBusiness usual &&
                    usual.OwnerIndex == playerIndex &&
                    usual.Level == maxLevel)
                {
                    res++;
                }
            }
            return res;
        }

        public bool _ifTP = false;
        public void SetValuesToGoOnFromChanceTp()
        {
            int randCellIndex = GameBoard.GetRandomCellIndexToGoOnInChance();

            int stepperPosition = Players[StepperIndex].Position;

            int res = stepperPosition >= randCellIndex ? stepperPosition - randCellIndex :
                GameBoard.Cells.Length - stepperPosition + randCellIndex;

            _firstCube = res;
            _secondCube = 0;

            _ifTP = true;
        }

        public void SetSkipMoveOppositeForStepper()
        {
            Players[StepperIndex].SetSkipMoveOpposite();
        }

        public void SetPositSkipForPlayer(int playerIndex)
        {
            Players[playerIndex].SetSkipMoveOpposite();
        }

        public bool IsStepperIsSleeping()
        {
            return Players[StepperIndex].IsSleeping();
        }

        public int GetStepperAmountOfSilverStars()
        {
            int silvers = GameBoard.GetAmountOfStars(StepperIndex, false);

            return silvers;
        }

        public int GetSteppersAmountOfGoldenStars()
        {
            const int silverStarsDivider = 5;
            int golden = GameBoard.GetAmountOfStars(StepperIndex, true) / silverStarsDivider;
            return golden;
        }
    }
}
