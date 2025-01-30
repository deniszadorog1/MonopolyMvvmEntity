using MahApps.Metro.Controls;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Serialization;

using MonopolyDLL;
using System.Windows.Media.Animation;
using System.Runtime.InteropServices.ComTypes;
using System.Diagnostics;
using System.Security.AccessControl;
using MonopolyEntity.Windows.UserControls.GameControls.BussinessInfo;
using System.Windows.Media.TextFormatting;
using System.Data.SqlTypes;

using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages;
using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.Cell.Bus;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf;
using System.Threading;
using MonopolyDLL.Monopoly.Enums;
using System.Globalization;
using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls;
using System.Windows.Media.Effects;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Controls.Primitives;

namespace MonopolyEntity.Windows.UserControls.GameControls
{
    /// <summary>
    /// Логика взаимодействия для GameField.xaml
    /// </summary>
    /// 

    public partial class GameField : UserControl
    {
        private MonopolySystem _system;
        private List<UserCard> _cards;
        public GameField(MonopolySystem system, List<UserCard> cards)
        {
            _system = system;
            _cards = cards;

            MakeBaseMethods();
        }

        private void MakeBaseMethods()
        {
            InitializeComponent();

            SetCellsInList();

            SetImmoratlImages();
            HideHeadersInCells();

            SetChipPositionsInStartSquare();

            SetClickEventForBusCells();
            SetStartPricesForBusses();

            SetHeaderColorsForBuses();

            AddThroughCubesControl();
            SetUsersColorsInList();

            SetCardForStartStepper();

            SetCellsStars();

            SetOwnerToAllCells();
        }

        public void SetOwnerToAllCells()
        {
            int stepperIndex = _system.MonGame.StepperIndex;
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.GameBoard.Cells[i] is ParentBus)
                {
                    ((ParentBus)_system.MonGame.GameBoard.Cells[i]).OwnerIndex = stepperIndex;
                    PaintCellInColor(i, _colors[stepperIndex]);
                }
            }
            _system.MonGame.SetMonopolies();
        }

        private void SetCardForStartStepper()
        {
            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
        }

        private void ChangeStepper()
        {
            //Get Back size for old Card
            _cards[_system.MonGame.StepperIndex].SetAnimation(null, false);

            //Change ing dll
            _system.MonGame.ChangeStepper();

            //Set Size for new Card
            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);

            SetActionAfterStepperChanged();
        }

        private void ChangeStepAfterAuction()
        {
            _system.MonGame.ChangeStepper();

            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);

            SetActionAfterStepperChanged();
        }

        private void SetActionAfterStepperChanged()
        {
            ActionAfterStepperChanged action = _system.MonGame.GetActionAfterStepperChanged();

            switch (action)
            {
                case ActionAfterStepperChanged.ThrowCubes:
                    AddThroughCubesControl();
                    break;
                case ActionAfterStepperChanged.PrisonQuerstion:
                    SetPrisonQuestion();
                    break;
            }
        }

        private void SetPrisonQuestion()
        {
            ChatMessages.Children.Clear();
            PrisonQuestion question = new PrisonQuestion();

            SetPrisonButtonsVisibility(question);
            SetEventsForPrisonQuestion(question);

            ChatMessages.Children.Add(question);
        }

        public void SetEventsForPrisonQuestion(PrisonQuestion question)
        {
            SetPayEventInPrison(question.PayBut);
            SetPayEventInPrison(question.LastPay);

            question.ContinueSitting.Click += (sender, e) =>
            {
                DropDiceInPrison();
            };
        }

        private void DropDiceInPrison()
        {
            (int, int) cubeVals = _system.MonGame.GetValsForPrisonDice();
            DicesDrop drop = new DicesDrop(cubeVals.Item1, cubeVals.Item2);

            Canvas.SetZIndex(drop, 0);
            drop.VerticalAlignment = VerticalAlignment.Center;

            drop._first3dCube._horizontalAnimation.Completed += (sender, e) =>
            {
                if (cubeVals.Item1 == cubeVals.Item2)
                {
                    //ChatMessages.Children.Clear();

                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperSitInPrisonCounter();

                    UpdatePlayersMoney();

                    SetActionAfterStepperChanged();
                    return;
                }
                _system.MonGame.MakeStepperPrisonCounterHigher();
                ChatMessages.Children.Clear();
                AddMessageTextBlock($"Values are not equal. You got {cubeVals.Item1} and {cubeVals.Item2}");
                ChangeStepper();
            };

            ChatMessages.Children.Add(drop);
        }

        public void SetPayEventInPrison(Button but)
        {
            but.Click += (sender, e) =>
            {
                if (_system.MonGame.IfStepperHasEnoughMoneyToPayPrisonPrice())
                {
                    _system.MonGame.PayPrisonBill();
                    ChatMessages.Children.Clear();

                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperSitInPrisonCounter();

                    UpdatePlayersMoney();

                    SetActionAfterStepperChanged();
                    return;
                }
                AddMessageTextBlock("Not enough money to pay prison price");
            };
        }

        private void SetPrisonButtonsVisibility(PrisonQuestion question)
        {
            if (!_system.MonGame.IfStepperSatInPrisonTooMuch()) //If Player sat NOT too much
            {
                question.SetEnoughMoneyButsVisibility();
                return;
            }
            //Player sat too many rounds in prison + set there if giveup later
            question.SetNotEnoughMoneyButsVisibility();
        }

        List<SolidColorBrush> _colors = new List<SolidColorBrush>();
        public void SetUsersColorsInList()
        {
            List<SolidColorBrush> temp = new List<SolidColorBrush>();
            temp.Add((SolidColorBrush)Application.Current.Resources["FirstUserColor"]);
            temp.Add((SolidColorBrush)Application.Current.Resources["SecondUserColor"]);
            temp.Add((SolidColorBrush)Application.Current.Resources["ThirdUserColor"]);
            temp.Add((SolidColorBrush)Application.Current.Resources["FourthUserColor"]);
            temp.Add((SolidColorBrush)Application.Current.Resources["FifthUserColor"]);

            for (int i = 0; i < _system.MonGame.Players.Count; i++)
            {
                _colors.Add(temp[i]);
            }
        }

        private void AddThroughCubesControl()
        {
            ThroughCubes cubes = new ThroughCubes();
            cubes.VerticalAlignment = VerticalAlignment.Top;
            cubes.ThroughCubesBut.Click += TrowCubes_Click;
            ChatMessages.Children.Add(cubes);
        }

        private void TrowCubes_Click(object sender, RoutedEventArgs e)
        {
            ChatMessages.Children.Clear();
            AddThrowingDice();
        }

        private void AddThrowingDice()
        {
            _system.MonGame.DropCubes();
            DicesDrop drop = new DicesDrop(_system.MonGame.GetFirstCubes(), _system.MonGame.GetSecondCube());
            drop.VerticalAlignment = VerticalAlignment.Center;

            drop._first3dCube._horizontalAnimation.Completed += CubesDropped_Completed;

            ChatMessages.Children.Add(drop);

            //MakeAMoveByChip();
        }

        private void CubesDropped_Completed(object sender, EventArgs e)
        {
            MakeChipsMovementAction(_system.MonGame.GetStepperPosition(),
                _system.MonGame.GetPointToMoveOn(), _imgs[_system.MonGame.StepperIndex]);

            //Change temp point 
            _system.MonGame.SetPlayerPosition();

            //Need to understand which cell is Cell On
            //Get enum action to show whats is happening

            _chipMoveAnimation.Completed += (compSender, eve) =>
            {
                //Check if stepper went through start(to get money)
                GoThroughStartCellCheck();

                //Remove dice 
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<DicesDrop>().First());

                CellAction action = _system.MonGame.GetAction();
                SetActionVisuals(action);
            };
        }

        private void GoThroughStartCellCheck()
        {
            if (_system.MonGame.IfStepperWentThroughStartCell())
            {
                int wentThroughStartMoney = _system.MonGame.GetGoThroughStartCellMoney();
                _system.MonGame.GetMoneyByStepper(wentThroughStartMoney);

                UpdatePlayersMoney();
                AddMessageTextBlock("Player went through start cell. Get money");
            }
        }

        private void SetActionVisuals(CellAction action)
        {
            switch (action)
            {
                case CellAction.GotOnBusinessToBuy:
                    SetBuyBusinessOffer();
                    break;
                case CellAction.GotOnOwnBusiness:
                    PlayerGotOnOwnBusiness();
                    break;
                case CellAction.GotOnEnemysBusiness:
                    SetGotOnEnemysBusiness();
                    break;
                case CellAction.GotOnTax:
                    SetGotOnTax();
                    break;
                case CellAction.GotOnCasino:
                    SetGetOnCasino();
                    break;
                case CellAction.GotOnGoToPrison:
                    break;
                case CellAction.GotOnChance:
                    SetGotOnChance(); //Correct movement things
                    break;
                case CellAction.GotOnStart:
                    GotOnStartAction();
                    break;
                case CellAction.SitInPrison:
                    break;
                case CellAction.VisitPrison:
                    break;
            }
            //trade
        }

        private void GotOnStartAction()
        {
            int money = _system.MonGame.GetGotOnStartCellMoney();
            _system.MonGame.GetMoneyByStepper(money);

            UpdatePlayersMoney();
            AddMessageTextBlock("Got on start cell get money");

            ChangeStepper();
        }

        private void SetGotOnChance()
        {
            ChanceAction action = ChanceAction.GoToPrison;// _system.MonGame.GetChanceAction();

            MakeChanceAction(action);

            UpdatePlayersMoney();

            string actionText = GetTextForChanceAction(action);
            AddMessageTextBlock(actionText);
        }



        private void MakeChanceAction(ChanceAction action)
        {
            switch (action)
            {
                case ChanceAction.ForwardInOne:
                    MakeLittleMoveFromChance(_system.MonGame.GetIndexToStepOnForChance(action));
                    break;
                case ChanceAction.BackwardsInOne:
                    MakeLittleMoveFromChance(_system.MonGame.GetIndexToStepOnForChance(action));
                    break;
                case ChanceAction.Pay500:
                    PayMoneyChanceAction(_system.MonGame.GameBoard.GetLitteleLoseMoneyChance());
                    break;
                case ChanceAction.Pay1500:
                    PayMoneyChanceAction(_system.MonGame.GameBoard.GetBigLoseMoneyChance());
                    break;
                case ChanceAction.Get500:
                    GetMoneyChanceAction(_system.MonGame.GameBoard.GetLitteleWinMoneyChance());
                    break;
                case ChanceAction.Get1500:
                    GetMoneyChanceAction(_system.MonGame.GameBoard.GetBigWinMoneyChance());
                    break;
                case ChanceAction.GoToPrison:
                    GoToPrisonFromChance(_system.MonGame.GameBoard.GetPrisonCellIndex());
                    break;
            }
        }

        private void MakeLittleMoveFromChance(int cellIndex)
        {
            int position = _system.MonGame.GetStepperPosition();


            MakeChipsMovementAction(position, cellIndex, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(cellIndex);
        }

        private void GoToPrisonFromChance(int cellIndexToMoveOn)
        {
            int stepperPosition = _system.MonGame.GetStepperPosition();
            IfLastMoveIsPrison = true;
            _system.MonGame.Players[_system.MonGame.StepperIndex].IfSitInPrison = true;
            MakeChipsMovementAction(stepperPosition, cellIndexToMoveOn, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(cellIndexToMoveOn);
            ChangeStepper();
        }

        private void GetMoneyChanceAction(int money)
        {
            _system.MonGame.GetMoneyFromChance(money);
        }

        private void PayMoneyChanceAction(int money)
        {
            SetPayMoney("You got on chance cell", $"neeed to Pay {money}", money);
        }

        public void SetPayMoney(string firstLine, string secondLine, int money, bool ifEnemysBus = false)
        {
            PayMoney bill = new PayMoney();

            bill.GotOnBusText.Text = $"{firstLine}";
            bill.PayBillText.Text = $"{secondLine}";


            bill.PayBillBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfStepperHasEnoughMoneyToPay(money))
                {
                    if (ifEnemysBus)
                    {
                        _system.MonGame.PayBusBillByStepper();
                    }
                    else { _system.MonGame.PayBillByStepper(money); }
                    AddMessageTextBlock($"Paid - {money}");
                    UpdatePlayersMoney();
                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<PayMoney>().First());

                    ChangeStepper(); //Next stepper after paid money
                }
                else
                {
                    AddMessageTextBlock("Not enough money to pay the bill");
                }
            };
            ChatMessages.Children.Add(bill);
        }

        public string GetTextForChanceAction(ChanceAction action)
        {
            switch (action)
            {
                case ChanceAction.ForwardInOne:
                    return "Moves forward";
                case ChanceAction.BackwardsInOne:
                    return "Moves backwards";
                case ChanceAction.Pay500:
                    return "Need to pay money";
                case ChanceAction.Pay1500:
                    return "Need to pay money";
                case ChanceAction.Get500:
                    return "Gets some money";
                case ChanceAction.Get1500:
                    return "Gets some money";
                case ChanceAction.GoToPrison:
                    return "Goes to prison";
            };

            throw new Exception("What can you get else");
        }

        private void SetGetOnCasino()
        {
            JackpotElem jackpot = new JackpotElem();

            jackpot.MakeBidBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfPlayerHasEnoughMoneyToPlayCasino())
                {
                    _system.MonGame.GetBillForCasino();

                    string casinoRes = _system.MonGame.PlayCasino(jackpot._chosenRibs);
                    AddMessageTextBlock(casinoRes);
                    ChatMessages.Children.Remove(ChatMessages.Children.
                    OfType<JackpotElem>().First());

                    UpdatePlayersMoney();

                    ChatMessages.Children.Clear();
                    ChangeStepper();
                }
                else
                {
                    AddMessageTextBlock("Not enough money to play casino");
                }
            };

            jackpot.DeclineBut.Click += (sender, e) =>
            {
                ChatMessages.Children.Remove(ChatMessages.Children.
                    OfType<JackpotElem>().First());

                ChatMessages.Children.Clear();
                ChangeStepper();
            };

            /*        JackPotCell.PreviewMouseDown += (sender, e) =>
                    {
                        ChatMessages.Children.Remove(jackpot);
                        ChatMessages.Children.Add(jackpot);
                    };*/

            ChatMessages.Children.Add(jackpot);
        }

        private void SetGotOnTax()
        {
            string firstLine = $"You got on Tax Cell - " +
                $"{_system.MonGame.GetTempPositionCellName()}";

            string secondLine = $"You neeed to pay the bill - " +
                $"{_system.MonGame.GetBillFromTaxStepperPosition()}";

            int money = _system.MonGame.GetBillFromTaxStepperPosition();

            SetPayMoney(firstLine, secondLine, money);

        }

        private void SetGotOnEnemysBusiness()
        {
            string firstLine = $"You got on the enemys business - " +
                $"{_system.MonGame.GetTempPositionCellName()}";

            string secondLine = $"You neeed to pay the bill - " +
                $"{_system.MonGame.GetBillForBusinessCell()}";

            int amountOfMoney = _system.MonGame.GetBillForBusinessCell();
            if (amountOfMoney == 0)
            {
                AddMessageTextBlock("Got on deposited Business");
                ChangeStepper();
                return;
            }


            SetPayMoney(firstLine, secondLine, amountOfMoney, true);
        }

        public void UpdatePlayersMoney()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].UserMoney.Text = _system.MonGame.GetPlayersMoney(i).ToString();
            }
        }

        private void PlayerGotOnOwnBusiness()
        {
            AddMessageTextBlock("Player got on his own Business");
            ChangeStepper();
        }

        private void SetBuyBusinessOffer()
        {
            BuyBusiness buy = new BuyBusiness();
            buy.YourTurnText.Text = $"You got on {_system.MonGame.GetTempPositionCellName()}";
            ChatMessages.Children.Add(buy);

            buy.BuyBusBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfStepperHasEnughMoneyToBuyBus())
                {
                    SetBuyingBusiness();
                    ChatMessages.Children.Clear();
                    ChangeStepper();
                    AddMessageTextBlock("Player is bought business");
                }
                else
                {
                    MessageBox.Show("Not enough money");
                    AddMessageTextBlock("Player doesnt want to buy this business");
                }
            };

            buy.SendToAuctionBut.Click += (sender, e) =>
            {
                //!!Make auction

                //auction action
                ChatMessages.Children.Clear();
                //ChangeStepper();
                //AddMessageTextBlock("Player is sending business to  auction");

                SetAuctionOffer();
            };
        }

        private void SetAuctionOffer()
        {
            _system.MonGame.SetStartAuctionPrice();
            _system.MonGame.SetStartPlayersForAuction();

            List<int> playresIndexesAuction = _system.MonGame._playerIndxesForAuction;

            if (playresIndexesAuction.Count == 0)
            {
                AddMessageTextBlock("Noone want to buy this business");
                return;
            }

            if (IfAuctionIsEnded()) return;

            SetBidderInAuction(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
            AddBidControl();
        }

        public bool IfAuctionIsEnded()
        {
            if (IfNooneTakesPartInAuction())
            {
                MakeEveryCardThinner();
                AddMessageTextBlock("Noone wants this business");

                ChangeStepAfterAuction();

                return true;
            }
            if (_system.MonGame.IfSomeOneWonAuction())
            {
                Console.WriteLine(_system.MonGame.Players);


                PaintCellInColor(_system.MonGame.GetStepperPosition(), _colors[_system.MonGame._bidderIndex]);
                AddMessageTextBlock($"Business is buying = - {_system.MonGame.GetBidderLogin()}");

                //Get money form winner 
                //_system.MonGame.GetMoneyFromAuctionWinner();


                UpdatePlayersMoney();
                //Clear visuals after auction 
                MakeEveryCardThinner();
                _system.MonGame.ClearAuctionValues();

                ChangeStepAfterAuction();

                return true;
            }
            return false;
        }

        private void MakeEveryCardThinner()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].MakeCardUsual();
            }
        }

        private bool AddBidControl()
        {
            if (IfAuctionIsEnded()) return true;

            AuctionBid bid = new AuctionBid();
            bid.BidForText.Text = $"Auction for {_system.MonGame.GetTempPositionCellName()}";
            bid.GoodLuckText.Text = $"Temp price in auction is {_system.MonGame.GetTempPriceInAuction()}"; ;

            bid.MakeBidBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfBidderHasEnoughMoneyToPlaceBid())
                {
                    _system.MonGame.MakeBidInAuction();
                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                    if (IfAuctionIsEnded()) return;
                    _system.MonGame.SetNextBidder();

                    if (AddBidControl()) return;
                    SetBidderInAuction(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
                }
                else
                {
                    AddMessageTextBlock("Not enough money!");
                }
            };

            bid.NotMakeABidBut.Click += (sender, e) =>
            {
                Console.WriteLine(_system.MonGame.Players);

                string message = $"{_system.MonGame.GetBidderLogin()} does not wanna buy business";
                AddMessageTextBlock(message);

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                if (IfAuctionIsEnded()) return;
                if (_system.MonGame.RemoveAuctionBidderIfItsWasLast())
                {
                    if (IfAuctionIsEnded()) return;
                }

                if (AddBidControl()) return;
                SetBidderInAuction(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
            };

            ChatMessages.Children.Add(bid);
            return false;
        }

        private void SetBidderInAuction(int makeUsualCardIndex, int toMarkCardIndex)
        {
            //Get Back size for old Card
            _cards[makeUsualCardIndex].SetAnimation(null, false);

            //Set Size for new Card
            _cards[toMarkCardIndex].SetAnimation(_colors[toMarkCardIndex], true);
        }

        private bool IfNooneTakesPartInAuction()
        {
            if (!_system.MonGame.IfSomeoneIsLeftInAuction())
            {
                _system.MonGame.ClearAuctionValues();
                return true;
            }
            return false;
        }


        private void SetBuyingBusiness()
        {
            _system.MonGame.BuyBusinessByStepper();

            int position = _system.MonGame.GetStepperPosition();

            PaintCellInColor(position, _colors[_system.MonGame.StepperIndex]);

            _cards[_system.MonGame.StepperIndex].UserMoney.Text = _system.MonGame.GetSteppersMoney().ToString();

            _system.MonGame.SetStartLevelOfBusinessForStepper();
            int startRent = _system.MonGame.GetStartPriceOfBoughtBusinessByStepper();
            ChangePriceInBusiness(position, startRent.ToString());
        }

        public void ChangePriceInBusiness(int cellIndex, string newPrice)
        {
            if (_cells[cellIndex] is UpperCell upper)
            {
                upper.Money.Text = newPrice;
            }
            else if (_cells[cellIndex] is RightCell right)
            {
                right.Money.Text = newPrice;
            }
            else if (_cells[cellIndex] is BottomCell bottom)
            {
                bottom.Money.Text = newPrice;
            }
            else if (_cells[cellIndex] is LeftCell left)
            {
                left.Money.Text = newPrice;
            }
        }

        private void PaintCellInColor(int cellIndex, SolidColorBrush brush)
        {
            if (_cells[cellIndex] is UpperCell upper)
            {
                upper.ImagePlacer.Background = brush;
            }
            else if (_cells[cellIndex] is RightCell right)
            {
                right.ImagePlacer.Background = brush;
            }
            else if (_cells[cellIndex] is BottomCell bottom)
            {
                bottom.ImagePlacer.Background = brush;
            }
            else if (_cells[cellIndex] is LeftCell left)
            {
                left.ImagePlacer.Background = brush;
            }
        }


        private void SetHeaderColorsForBuses()
        {
            PerfumeFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PerfumeColor"];
            PerfumeSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PerfumeColor"];

            UpCar.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
            RightCar.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
            DownCars.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];
            LeftCar.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];

            ClothesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["ClothesColor"];
            ClothesSeconBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["ClothesColor"];
            ClothesThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["ClothesColor"];

            MessagerFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["MessagerColor"];
            MessagerSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["MessagerColor"];
            MessagerThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["MessagerColor"];

            DrinkFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["DrinkColor"];
            DrinkSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["DrinkColor"];
            DrinkThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["DrinkColor"];

            PlanesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PlaneColor"];
            PlanesSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PlaneColor"];
            PlanesThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PlaneColor"];


            FoodFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["FoodColor"];
            FoodSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["FoodColor"];
            FoodThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["FoodColor"];

            HotelFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["HotelColor"];
            HotelSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["HotelColor"];
            HotelThirdBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["HotelColor"];

            PhonesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PhoneColor"];
            PhonesSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["PhoneColor"];

            GamesFirstBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];
            GamesSecondBus.MoneyPlacer.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];
        }



        private void SetStartPricesForBusses()
        {
            List<string> prices = new List<string>();
            for (int i = 0; i < _cells.Count; i++)
            {
                prices.Add(_system.MonGame.GameBoard.GetBusinessPrice(i));
            }

            for (int i = 0; i < _cells.Count; i++)
            {
                SetPriceInCell(prices[i], _cells[i]);
            }
        }

        private void SetPriceInCell(string price, UIElement cell)
        {
            if (cell is UpperCell up &&
                up.MoneyPlacer.Visibility == Visibility.Visible)
            {
                up.Money.Text = price;

            }
            else if (cell is RightCell right &&
                right.MoneyPlacer.Visibility == Visibility.Visible)
            {
                right.Money.Text = price;
                RotateTextBlock(right.Money, 90);
            }
            else if (cell is BottomCell bot &&
                bot.MoneyPlacer.Visibility == Visibility.Visible)
            {
                bot.Money.Text = price;
            }
            else if (cell is LeftCell left &&
                left.MoneyPlacer.Visibility == Visibility.Visible)
            {
                left.Money.Text = price;
                RotateTextBlock(left.Money, -90);
            }
        }

        private void RotateTextBlock(TextBlock block, int angle)
        {
            block.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform transform = new RotateTransform()
            {
                Angle = angle,
            };

            block.LayoutTransform = transform;
        }


        private void SetClickEventForBusCells()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SetClickEventForCell(_cells[i]);
            }
        }

        private int _clickedCellIndex;
        private void SetClickEventForCell(UIElement element)
        {
            if (element is UpperCell upper)
                upper.PreviewMouseDown += Bussiness_PreviewMouseDown;

            if (element is RightCell right)
                right.PreviewMouseDown += Bussiness_PreviewMouseDown;

            if (element is BottomCell bottom)
                bottom.PreviewMouseDown += Bussiness_PreviewMouseDown;

            if (element is LeftCell left)
                left.PreviewMouseDown += Bussiness_PreviewMouseDown;
        }

        private void SetClickedCellIndex(UIElement element)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (element == _cells[i])
                {
                    _clickedCellIndex = i;
                    return;
                }
            }
            _clickedCellIndex = -1;
        }

        private void Bussiness_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            SetClickedCellIndex((UIElement)sender);
            BussinessInfo.Children.Clear();

            //Add info about business
            (UIElement asd, Size? size) = AddBusinessInfo();

            if (asd is null) return;
            SetLocForInfoBox(asd, (UIElement)sender, (Size)size);
        }

        private (UIElement, Size?) AddBusinessInfo()
        {
            if (_system.MonGame.GameBoard.Cells[_clickedCellIndex].GetType() == typeof(CarBus))
            {
                CarBusInfo info = new CarBusInfo();

                BussinessInfo.Children.Add(info);
                SetCarBusInfoParams(info);
                info.UpdateLayout();
                return (info, new Size(info.ActualWidth, info.ActualHeight));
            }
            else if (_system.MonGame.GameBoard.Cells[_clickedCellIndex].GetType() == typeof(UsualBus))
            {
                UsualBusInfo info = new UsualBusInfo();
                BussinessInfo.Children.Add(info);
                SetUsualBusInfoParams(info);
                info.UpdateLayout();
                return (info, new Size(info.ActualWidth, info.ActualHeight));
            }
            else if (_system.MonGame.GameBoard.Cells[_clickedCellIndex].GetType() == typeof(GameBus))
            {
                GameBusInfo info = new GameBusInfo();
                BussinessInfo.Children.Add(info);
                SetGameBusInfoParams(info);
                info.UpdateLayout();
                return (info, new Size(info.ActualWidth, info.ActualHeight));
            }
            return (null, null);
        }

        private void SetCarBusInfoParams(CarBusInfo info)
        {
            CarBus bus = (CarBus)_system.MonGame.GameBoard.Cells[_clickedCellIndex];

            info.BusNameText.Text = bus.Name;
            info.BusType.Text = bus.BusType.ToString();

            info.BusDesc.Text = "This is car business";

            info.OneFieldMoney.Text = bus.PayLevels[0].ToString();
            info.TwoFieldMoney.Text = bus.PayLevels[1].ToString();
            info.ThreeFieldMoney.Text = bus.PayLevels[2].ToString();
            info.FourFieldMoney.Text = bus.PayLevels[3].ToString();

            info.FieldPrice.Text = bus.Price.ToString();
            info.DepositPriceText.Text = bus.DepositPrice.ToString();
            info.RebuyPrice.Text = bus.RebuyPrice.ToString();

            info.CarBusHeader.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];

            SetCarInfoVisualButs(info);
            SetEventsForCarsInfoButs(info);
        }

        public void SetCarInfoVisualButs(CarBusInfo info)
        {
            CarGameInfoVisual gameVis = _system.MonGame.GetGamesCarsButsVisual(_clickedCellIndex);

            info.BusDesc.Visibility = Visibility.Hidden;
            switch (gameVis)
            {
                case CarGameInfoVisual.Deposit:
                    info.DepositBut.Visibility = Visibility.Visible;
                    break;
                case CarGameInfoVisual.Rebuy:
                    info.RebuyBut.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void SetEventsForCarsInfoButs(CarBusInfo info)
        {
            SetCarDepositEvent(info.DepositBut);
            SetCarRebuyEvent(info.RebuyBut);
        }

        public void SetCarRebuyEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                if (_system.MonGame.IfPlayerCanRebuyBus(_clickedCellIndex))
                {
                    SetOpacityToBusiness(1);
                    _system.MonGame.RebuyBus(_clickedCellIndex);

                    //Set Players payments 
                    _system.MonGame.SetCarsPaymentLevels();

                    //Set Payment
                    SetCarsPayments();
                    UpdatePlayersMoney();
                    return;
                }
                AddMessageTextBlock("Not enough money to rebuy business");
            };
        }

        public void SetCarDepositEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                //Change Opacity
                SetOpacityToBusiness(0.5);
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Players payments 
                _system.MonGame.SetCarsPaymentLevels();

                //Set Payment
                SetCarsPayments();
                UpdatePlayersMoney();
            };
        }

        public void SetCarsPayments()
        {
            ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex).ToString());

            List<int> cellsIndexes =
                _system.MonGame.GetCarsIndexesWhichPlayerOwnNotDeposited();

            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                ChangePriceInBusiness(cellsIndexes[i], _system.MonGame.GetBusMoneyLevel(cellsIndexes[i]).ToString());
            }
        }

        private void SetGameBusInfoParams(GameBusInfo info)
        {
            GameBus bus = (GameBus)_system.MonGame.GameBoard.Cells[_clickedCellIndex];

            info.BusName.Text = bus.Name;
            info.BusType.Text = bus.BusType.ToString();

            info.BusDescription.Text = "This is game  business." +
                "Result bill si multiplication of bought game buses and got " +
                "cube ribs amount";

            info.OneFieldMoney.Text = bus.PayLevels[0].ToString();
            info.TwoFieldMoney.Text = bus.PayLevels[1].ToString();

            info.FieldPrice.Text = bus.Price.ToString();
            info.DepositPriceText.Text = bus.DepositPrice.ToString();
            info.RebuyPrice.Text = bus.RebuyPrice.ToString();

            info.GameBusHeader.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];

            SetGameInfoVisualButs(info);
            SetGameInfoEvents(info);
        }

        public void SetGameInfoVisualButs(GameBusInfo info)
        {
            CarGameInfoVisual gameVis = _system.MonGame.GetGamesCarsButsVisual(_clickedCellIndex);

            info.BusDescription.Visibility = Visibility.Hidden;
            switch (gameVis)
            {
                case CarGameInfoVisual.Deposit:
                    info.DepositBut.Visibility = Visibility.Visible;
                    break;
                case CarGameInfoVisual.Rebuy:
                    info.RebuyBut.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void SetGameInfoEvents(GameBusInfo info)
        {
            SetGameDepositButEvent(info.DepositBut);
            SetGameRebuyEvent(info.RebuyBut);
        }

        public void SetOpacityToBusiness(double opacity)
        {
            Grid toChangeOpacity = GetCanvasToChangeOpacityForDepositedCell();
            toChangeOpacity.Opacity = opacity;
        }

        public void SetGameRebuyEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                if (_system.MonGame.IfPlayerCanRebuyBus(_clickedCellIndex))
                {
                    SetOpacityToBusiness(1);
                    _system.MonGame.RebuyBus(_clickedCellIndex);

                    //Set Players payments 
                    _system.MonGame.SetGamePaymentLevels();

                    //Set Payment
                    SetGamePayments();
                    UpdatePlayersMoney();
                    return;
                }
                AddMessageTextBlock("Not enough money to rebuy business");
            };
        }

        public void SetGameDepositButEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                //Change Opacity
                SetOpacityToBusiness(0.5);
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Players payments 
                _system.MonGame.SetGamePaymentLevels();

                //Set Payment
                SetGamePayments();
                UpdatePlayersMoney();
            };
        }

        public void SetGamePayments()
        {
            ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex).ToString());

            List<int> cellsIndexes =
                _system.MonGame.GetGamesIndexesWhichPlayerOwnNotDeposited();

            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                ChangePriceInBusiness(cellsIndexes[i], _system.MonGame.GetBusMoneyLevel(cellsIndexes[i]).ToString());
            }
        }


        private void SetUsualBusInfoParams(UsualBusInfo info)
        {
            UsualBus bus = (UsualBus)_system.MonGame.GameBoard.Cells[_clickedCellIndex];

            info.BusName.Text = bus.Name;
            info.BusType.Text = bus.BusType.ToString();

            info.DescriptionText.Text = "It is a usual business";

            info.BaseRentMoney.Text = bus.PayLevels[0].ToString();
            info.OneStarRentMoney.Text = bus.PayLevels[1].ToString();
            info.TwoStarRentMoney.Text = bus.PayLevels[2].ToString();
            info.ThreeStarRentMoney.Text = bus.PayLevels[3].ToString();
            info.FourStarRentMoney.Text = bus.PayLevels[4].ToString();
            info.YellowStarRentMoney.Text = bus.PayLevels[4].ToString();

            info.BusPriceMoney.Text = bus.Price.ToString();
            info.DepositPriceMoney.Text = bus.DepositPrice.ToString();
            info.RebuyPriceMoney.Text = bus.RebuyPrice.ToString();
            info.HousePriceMoney.Text = bus.BuySellHouse.ToString();

            info.NameBusBorder.Background = GetColorForUsualBusHeader(bus);

            SetEventsForUsualBusInfo(info);
            SetHouseButtonsForBusInfo(info);
        }

        private void SetHouseButtonsForBusInfo(UsualBusInfo info)
        {
            //Its in monopoly
            if (!_system.MonGame.IfCellIsInMonopoly(_clickedCellIndex)) return;

            //Get type of busts visibility type
            UsualBusInfoVisual type = _system.MonGame.GetButsTypeVisibility(_clickedCellIndex);

            info.DescriptionText.Visibility = Visibility.Hidden;
            switch (type)
            {
                case UsualBusInfoVisual.BuyHouse:
                    info.BuildHouseBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusInfoVisual.SellHouse:
                    info.SellHouseBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusInfoVisual.Combine:
                    info.CombineHouseButs.Visibility = Visibility.Visible;
                    break;
                case UsualBusInfoVisual.Rebuy:
                    info.RebuyBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusInfoVisual.Deposit:
                    info.DepositBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusInfoVisual.DepositAndBuildHouse:
                    info.CombineDepositBuyHouse.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void SetEventsForUsualBusInfo(UsualBusInfo info)
        {
            SetBuyHouseEventBut(info.BuildHouseBut);
            SetBuyHouseEventBut(info.BuildHouseCombineBut);
            SetBuyHouseEventBut(info.BuildHouseSecondCombineBut);

            SetSellHouseEventBuy(info.SellHouseBut);
            SetSellHouseEventBuy(info.SellHouseCombineBut);

            SetDepositBusEvent(info.DepositCellSecondCombineBut);
            SetDepositBusEvent(info.DepositBut);

            SetRebuyBusEvent(info.RebuyBut);
        }

        public void SetRebuyBusEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                if (_system.MonGame.IfPlayerCanRebuyBus(_clickedCellIndex))
                {
                    //Change opacity
                    SetOpacityToBusiness(1);
                    //rebuy bus
                    _system.MonGame.RebuyBus(_clickedCellIndex);

                    //Set Payment
                    ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex).ToString());

                    UpdatePlayersMoney();
                    return;
                }
                AddMessageTextBlock("Not enough money to rebut busines");
            };
        }

        public void SetDepositBusEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                //Change Opacity
                SetOpacityToBusiness(0.5);
                //Deposit bus
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Payment
                ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex).ToString());
                UpdatePlayersMoney();
            };
        }

        public Grid GetCanvasToChangeOpacityForDepositedCell()
        {
            if (_cells[_clickedCellIndex] is UpperCell up) return up.ImagePlacer;
            if (_cells[_clickedCellIndex] is RightCell right) return right.ImagePlacer;
            if (_cells[_clickedCellIndex] is BottomCell bot) return bot.ImagePlacer;
            if (_cells[_clickedCellIndex] is LeftCell left) return left.ImagePlacer;

            throw new Exception("no such cellType!");
        }

        public void SetSellHouseEventBuy(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                _system.MonGame.SellHouse(_clickedCellIndex);
                SetHousesInCellParams();
                return;
            };
        }

        public void SetBuyHouseEventBut(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                if (_system.MonGame.IfPlayersHasEnoughMoneyToBuyHouse(_clickedCellIndex))
                {
                    _system.MonGame.BuyHouse(_clickedCellIndex);
                    SetHousesInCellParams();
                    return;
                }
                AddMessageTextBlock("Not enough money to buy house");
            };
        }

        public void SetHousesInCellParams()
        {
            SetCellStars(_clickedCellIndex);

            UpdatePlayersMoney();
            BussinessInfo.Children.Clear();
            ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex).ToString());
        }

        public void SetCellStars(int cellIndex)
        {
            if (_system.MonGame.IfCellIsUsualBusiness(cellIndex)) // cell is usual business
            {
                int level = _system.MonGame.GetBusLevel(cellIndex);
                SetBusinessStars(level, cellIndex);
            }
        }

        public void SetCellsStars()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IfCellIsUsualBusiness(i)) // cell is usual business
                {
                    int level = _system.MonGame.GetBusLevel(i);
                    SetBusinessStars(level, i);
                }
            }
        }

        public void SetBusinessStars(int level, int cellIndex)
        {
            //yellow or silver
            StarType type = _system.MonGame.GetStarType(cellIndex);

            switch (type)
            {
                case StarType.SilverStar:
                    SetSilverStart(level, GetGridForStars(cellIndex));
                    break;
                case StarType.YellowStar:
                    SetGoldenStar(GetGridForStars(cellIndex));
                    break;
            }
        }

        private Grid GetGridForStars(int cellIndex)
        {
            if (_cells[cellIndex] is UpperCell) return ((UpperCell)_cells[cellIndex]).StarsGrid;
            if (_cells[cellIndex] is RightCell) return ((RightCell)_cells[cellIndex]).StarsGrid;
            if (_cells[cellIndex] is BottomCell) return ((BottomCell)_cells[cellIndex]).StarsGrid;
            if (_cells[cellIndex] is LeftCell) return ((LeftCell)_cells[cellIndex]).StarsGrid;

            throw new Exception("No such cell type");
        }


        public void SetGoldenStar(Grid toAdd)
        {
            toAdd.Children.Clear();
            Image goldenStar = new Image()
            {
                Width = 20,
                Height = 20,
                Source = GetGoldenImage().Source
            };
            toAdd.Children.Add(goldenStar);
        }

        public void SetSilverStart(int level, Grid toAdd)
        {
            toAdd.Children.Clear();
            Image silver = GetImageSilverImage();

            WrapPanel starPanel = new WrapPanel()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            for (int i = 0; i < level; i++)
            {
                Image img = new Image()
                {
                    Width = 15,
                    Height = 15,
                    Source = silver.Source
                };
                starPanel.Children.Add(img);
            }
            toAdd.Children.Add(starPanel);
        }

        public Image GetImageSilverImage()
        {
            return BoardHelper.GetImageFromOtherFolder("whiteStar.png");
        }

        public Image GetGoldenImage()
        {
            return BoardHelper.GetImageFromOtherFolder("yellowStar.png");
        }

        public SolidColorBrush GetColorForUsualBusHeader(UsualBus bus)
        {
            if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Perfume)
            {
                return (SolidColorBrush)Application.Current.Resources["PerfumeColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Clothes)
            {
                return (SolidColorBrush)Application.Current.Resources["ClothesColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Messagers)
            {
                return (SolidColorBrush)Application.Current.Resources["MessagerColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Drinks)
            {
                return (SolidColorBrush)Application.Current.Resources["DrinkColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Planes)
            {
                return (SolidColorBrush)Application.Current.Resources["PlaneColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Food)
            {
                return (SolidColorBrush)Application.Current.Resources["FoodColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Hotels)
            {
                return (SolidColorBrush)Application.Current.Resources["HotelColor"];
            }
            else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Phones)
            {
                return (SolidColorBrush)Application.Current.Resources["PhoneColor"];
            }

            throw new Exception("No such business type...How is it possiable?");
        }


        private void SetLocForInfoBox(UIElement info, UIElement cell, Size size)
        {
            const int distToCell = 5;
            Point fieldPoint = this.PointToScreen(new Point(0, 0));
            Point cellPoint = cell.PointToScreen(new Point(0, 0));

            Point cellLocalPoint = new Point(cellPoint.X - fieldPoint.X, cellPoint.Y - fieldPoint.Y);

            Point infoLoc = new Point(0, 0);
            if (cell is UpperCell up)
            {
                infoLoc = new Point(cellLocalPoint.X + up.Width / 2 - size.Width / 2, up.Height + distToCell);
            }
            else if (cell is RightCell right)
            {
                double jacPotYLoc = JackPotCell.PointToScreen(new Point(0, 0)).Y - fieldPoint.Y;

                double yLoc = cellLocalPoint.Y + size.Height > jacPotYLoc ?
                    jacPotYLoc - size.Height : cellLocalPoint.Y;

                infoLoc = new Point(cellLocalPoint.X - size.Width - distToCell, yLoc);
            }
            else if (cell is BottomCell bot)
            {
                infoLoc = new Point(cellLocalPoint.X + bot.Width / 2 - size.Width / 2, cellLocalPoint.Y - size.Height - distToCell);
            }
            else if (cell is LeftCell left)
            {
                double jacPotYLoc = JackPotCell.PointToScreen(new Point(0, 0)).Y - fieldPoint.Y;

                double yLoc = cellLocalPoint.Y + size.Height > jacPotYLoc ?
                    jacPotYLoc - size.Height : cellLocalPoint.Y;

                infoLoc = new Point(cellLocalPoint.X + left.Width + distToCell, yLoc);
            }

            Canvas.SetLeft(info, infoLoc.X);
            Canvas.SetTop(info, infoLoc.Y);
        }


        //BREAKETS, mb need to move it into anouther file
        //set chips position and make a movement  

        //Set chips position in fields mathematicly


        private int counter = 0;
        private void UserControl_LayoutUpdated(object sender, EventArgs e)
        {
            return;
            if (counter >= 1) return;
            //MakeCheckThere
            //Point globalPosition = StartCell.PointToScreen(new Point(0, 0));

            const int startCellIndex = 0;
            const int cellIndexToMoveOn = 10;

            //make movement action
            MoveToPrisonCell(startCellIndex, cellIndexToMoveOn, _imgs[0]);

            //MakeChipsMovementAction(startCellIndex, cellIndexToMoveOn, _imgs[0]);
            counter++;
        }

        private void MoveToPrisonCell(int startCellIndex, int cellIndexToMoveOn, Image img)
        {
            MakeChipsMovementAction(startCellIndex, cellIndexToMoveOn, img);
        }

        private List<(Image, int)> chipAndCord = new List<(Image, int)>();

        private void MakeChipsMovementAction(int startCellIndex, int cellIndexToMoveOn, Image chipImage)
        {
            if (_ifChipMoves) return;

            _squareIndexesToGoThrough =
                BoardHelper.GetListOfSquareCellIndexesThatChipGoesThrough(startCellIndex, cellIndexToMoveOn);
            int checkLastCellIndex = cellIndexToMoveOn;

            if (!(_squareIndexesToGoThrough is null))
            {
                cellIndexToMoveOn = _squareIndexesToGoThrough.First();
            }

            Point chipToFieldPoint = GetChipImageLocToField(chipImage);

            Point insidePointStartCell = new Point(Canvas.GetLeft(chipImage), Canvas.GetTop(chipImage));
            Point newInsideChipPoint = GetInsidePointToStepOn(_cells[cellIndexToMoveOn], true);

            //Remove from cell
            ((Canvas)chipImage.Parent).Children.Remove(chipImage);

            //Add chip tom chipMove canvas
            AddChipToChipMovePanel(chipImage, chipToFieldPoint);

            //make move action
            MakeChipMove(startCellIndex, cellIndexToMoveOn, chipImage,
                insidePointStartCell, newInsideChipPoint);

            //Reassign chip image to new cell (ON ANIMATION COMPLETE EVENT)
            //ReassignChipImageInNewCell(moveDist, tempChipImg, newInsideChipPoint);


            //Change Chips In cell

            if (startCellIndex == _system.MonGame.GetPrisonIndex()) UpdatePrisonCanvases(_system.MonGame.GetPrisonIndex());
            else SetNewPositionsToChipsInCell(startCellIndex);

            SetNewPositionsToChipsInCell(checkLastCellIndex, true);
        }

        private void UpdatePrisonCanvases(int cellIndex)
        {
            UpdatePrisonCanvas(PrisonCell.ChipsPlacerVisit, cellIndex, false);
            UpdatePrisonCanvas(PrisonCell.ChipsPlacerSitters, cellIndex, true);
        }

        private void UpdatePrisonCanvas(Canvas can, int cellIndex, bool ifSit)
        {
            //new points for chips
            List<Point> newChipsPoints = ifSit ?
                BoardHelper.GetPointsForPrisonCellSitter(can.Children.OfType<Image>().ToList().Count,
                new Size(can.ActualWidth, can.ActualHeight)) :

                BoardHelper.GetPointsForPrisonCellExcurs(can.Children.OfType<Image>().ToList().Count,
                new Size(can.ActualWidth, can.ActualHeight));

            if (newChipsPoints is null) return;

            //Reassign chips in move panel
            SetChipsImagesInMovePanel(newChipsPoints, can, cellIndex);
        }

        private Point GetChipImageLocToField(Image img)
        {
            ChipMovePanel.UpdateLayout();
            //img.UpdateLayout();
            //Application.Current.MainWindow.UpdateLayout();

            Point startChipPoint = img.PointToScreen(new Point(0, 0));
            Point fieldPoint = this.PointToScreen(new Point(0, 0));

            return new Point(startChipPoint.X - fieldPoint.X, startChipPoint.Y - fieldPoint.Y);
        }

        private bool _ifGoesThroughSquareCell = false;
        private List<int> _squareIndexesToGoThrough = null;
        private int _tempSquareValToGoOn;
        private EventHandler _chipAnimEvent;

        private bool IfLastMoveIsPrison = false;

        public void MakeChipMove(int startCellIndex, int cellIndexToMoveOn, Image chipImg,
            Point insidePointStartCell, Point newInsideChipPoint)
        {
            //chip goes through square cell
            if (!(_squareIndexesToGoThrough is null))
            {
                _tempSquareValToGoOn = _squareIndexesToGoThrough.First();
                _ifGoesThroughSquareCell = true;

                if (_cells[_squareIndexesToGoThrough.Last()] is PrisonCell) IfLastMoveIsPrison = true;

                _chipAnimEvent = (s, e) =>
                {
                    Point qwe = GetInsidePointToStepOn(_cells[_tempSquareValToGoOn], false);

                    //BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn])

                    ReassignChipImageInNewCell(_tempSquareValToGoOn, chipImg, qwe);

                    int toRemoveSquareIndex = _tempSquareValToGoOn;
                    _squareIndexesToGoThrough.Remove(_tempSquareValToGoOn);
                    if (_squareIndexesToGoThrough.Count > 0)
                    {
                        _tempSquareValToGoOn = _squareIndexesToGoThrough.First();

                        Point startChipPoint = _cells[toRemoveSquareIndex].PointToScreen(new Point(0, 0));
                        Point fieldPoint = this.PointToScreen(new Point(0, 0));

                        Point check = new Point(Canvas.GetLeft(chipImg), Canvas.GetTop(chipImg));
                        Point asd = new Point(startChipPoint.X - fieldPoint.X + check.X,
                            startChipPoint.Y - fieldPoint.Y + check.Y);

                        ((Canvas)chipImg.Parent).Children.Remove(chipImg);
                        AddChipToChipMovePanel(chipImg, asd);


                        Point moveOn = GetInsidePointToStepOn(_cells[_tempSquareValToGoOn], false);
                        //BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn])

                        MakeChipMoveToAnoutherCell(toRemoveSquareIndex, _tempSquareValToGoOn, chipImg,
                        check, moveOn);

                        if (IfLastMoveIsPrison && _squareIndexesToGoThrough.Count == 1)
                        {
                            IfLastMoveIsPrison = false;
                            MakeAMoveToInPrisonCell(IfPlayerSitsInPrisonByChipImage(chipImg), chipImg);
                        }
                    }
                    else _ifChipMoves = false;
                };
                MakeChipMoveToAnoutherCell(startCellIndex, _tempSquareValToGoOn, chipImg,
                    insidePointStartCell, newInsideChipPoint);
            }
            else
            {
                //if (_cells[cellIndexToMoveOn] is PrisonCell) IfLastMoveIsPrison = true;

                //usualMove
                MakeChipMoveToAnoutherCell(startCellIndex, cellIndexToMoveOn, chipImg, insidePointStartCell, newInsideChipPoint);
            }
        }

        //To Make a move in prison cell(sitter or visiter)
        private void MakeAMoveToInPrisonCell(bool ifSitsInPrison, Image movedChip)
        {
            if (!ifSitsInPrison)
            {
                //PrisonCell.ChipsPlacer.Children.Remove(movedChip);
                //PrisonCell.ChipsPlacerVisit.Children.Add(movedChip);

                List<Point> points = BoardHelper.GetPointsForPrisonCellExcurs(PrisonCell.ChipsPlacerVisit.Children.Count,
                    new Size(PrisonCell.Width, PrisonCell.Height));

                SetChipsImagesInMovePanel(points, PrisonCell.ChipsPlacerVisit, _system.MonGame.GameBoard.GetPrisonCellIndex());
                return;
            }

            List<Point> sitPrisonPoints = BoardHelper.GetPointsForPrisonCellSitter(PrisonCell.ChipsPlacerSitters.Children.Count,
            new Size(PrisonCell.Width, PrisonCell.Height));
            SetChipsImagesInMovePanel(sitPrisonPoints, PrisonCell.ChipsPlacerSitters, _system.MonGame.GameBoard.GetPrisonCellIndex());

        }

        private void SetNewPositionsToChipsInCell(int cellIndex, bool ifInMove = false)
        {
            Canvas can = GetChipCanvas(_cells[cellIndex]);

            //new points for chips
            List<Point> newChipsPoints = BoardHelper.GetPointsForChips(
                new Size(can.ActualWidth, can.ActualHeight), can.Children.OfType<Image>().ToList().Count, ifInMove);

            if (newChipsPoints is null) return;

            //Reassign chips in move panel
            SetChipsImagesInMovePanel(newChipsPoints, can, cellIndex);
        }

        private void SetChipsImagesInMovePanel(List<Point> points, Canvas chipCanvas, int cellIndex)
        {
            List<Image> chips = chipCanvas.Children.OfType<Image>().ToList();
            List<Point> chipsStartPoints = new List<Point>();

            Point fieldGlobalPoint = this.PointToScreen(new Point(0, 0));
            Point cellChipCanPoint = chipCanvas.PointToScreen(new Point(0, 0));

            Point differ = new Point(cellChipCanPoint.X - fieldGlobalPoint.X, cellChipCanPoint.Y - fieldGlobalPoint.Y);

            foreach (Image img in chips)
            {
                chipsStartPoints.Add(new Point(Canvas.GetLeft(img), Canvas.GetTop(img)));

                chipCanvas.Children.Remove(img);
                Point chipPoint = new Point(Canvas.GetLeft(img) + differ.X, Canvas.GetTop(img) + differ.Y);

                Canvas.SetLeft(img, chipPoint.X);
                Canvas.SetTop(img, chipPoint.Y);

                ChipMovePanel.Children.Add(img);
            }
            for (int i = 0; i < chips.Count; i++)
            {
                if (chips[i] is Image img)
                {
                    Point chipPointDiffer = new Point(points[i].X - chipsStartPoints[i].X, points[i].Y - chipsStartPoints[i].Y);
                    SetChipToMoveAnimation(img, chipPointDiffer, cellIndex, points[i], true);
                }
            }
        }

        private void ReassignChipImageInNewCell(int newCellIndex, Image chipImage, Point newCellInsideLoc)
        {
            ChipMovePanel.Children.Remove(chipImage);

            UIElement cellToMoveOn = _cells[newCellIndex];

            Canvas chipCan = GetChipCanvas(cellToMoveOn, chipImage);

            chipCan.Children.Add(chipImage);
            //chipCan.UpdateLayout();
            //Application.Current.MainWindow.UpdateLayout();

            Canvas.SetLeft(chipImage, newCellInsideLoc.X);
            Canvas.SetTop(chipImage, newCellInsideLoc.Y);

            chipImage.RenderTransform = null;
        }

        public Canvas GetChipCanvas(UIElement el, Image img = null)
        {
            string canvasName = GetCanvasNameToStepOn(img);

            var chipsPlacerField = el.GetType().GetField(
                canvasName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (chipsPlacerField is null) throw new Exception("Cell doesnt have chips placer");
            return chipsPlacerField.GetValue(el) as Canvas;
        }

        public string GetCanvasNameToStepOn(Image img)
        {
            if (img is null) return PrisonCell.ChipsPlacer.Name.ToString();
            int index = _imgs.IndexOf(img);
            VisualPrisonCellActions action = _system.MonGame.IfPlayerIsInPrison(index);
            switch (action)
            {
                case VisualPrisonCellActions.SitInPrison:
                    return PrisonCell.ChipsPlacerSitters.Name.ToString();
                case VisualPrisonCellActions.VisitPrison:
                    return PrisonCell.ChipsPlacerVisit.Name.ToString();
                case VisualPrisonCellActions.OutOfPrison:
                    return PrisonCell.ChipsPlacer.Name.ToString();
            }

            throw new Exception("No such prison actions type...HOW?");
        }


        private void MakeChipMoveToAnoutherCell(int startCellIndex, int movePoint, Image chip,
            Point prevCellInsideChipPoint, Point newCellInsideChipPoint)
        {
            //Get cell to move on
            //Check how many items in chipsCanvas
            //GetPoints for this Cell To stepOn
            //Get point for new Chip

            //cell to step on
            UIElement cellToMoveOn = _cells[movePoint];

            //Last point - for chips which we are moving
            Point newCellPoint = GetChipCanvas(cellToMoveOn).PointToScreen(new Point(0, 0));
            Point startcCell = GetChipCanvas(_cells[startCellIndex]).PointToScreen(new Point(0, 0));

            Point pointToStepOn = new Point(newCellPoint.X - startcCell.X + newCellInsideChipPoint.X - prevCellInsideChipPoint.X,
                newCellPoint.Y - startcCell.Y + newCellInsideChipPoint.Y - prevCellInsideChipPoint.Y);

            //Set chip amimation 
            SetChipToMoveAnimation(chip, pointToStepOn, movePoint, newCellInsideChipPoint);
        }

        private Point GetInsidePointToStepOn(UIElement cellToMoveOn, bool ifInMove = false)
        {
            //amount of chips in cell to step on
            int amountOfChipsInCell = _system.MonGame.GetAmountOfPlayersInCell(_cells.IndexOf(cellToMoveOn));

            //Size of cell to step on
            Size cellSize = BoardHelper.GetSizeOfCell(cellToMoveOn);

            //Get Points for chips to place on cel to step on (need to replace all chips)
            List<Point> chipPoints = BoardHelper.GetPointsForChips(cellSize, amountOfChipsInCell, ifInMove);

            return chipPoints.Last();
        }

        private bool _ifChipMoves = false;
        private DoubleAnimation _chipMoveAnimation;

        private void SetChipToMoveAnimation(Image toMove, Point endPoint,
            int cellIndex, Point newInsideChipPoint, bool IfChipsInOldCell = false)
        {
            var transform = new TranslateTransform();
            toMove.RenderTransform = transform;

            _chipMoveAnimation = new DoubleAnimation
            {
                From = 0,
                To = endPoint.X,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            if (!(_squareIndexesToGoThrough is null) &&
                _ifGoesThroughSquareCell && !IfChipsInOldCell) // tempMove Chip Img
            {
                _chipMoveAnimation.Completed += _chipAnimEvent;
            }
            else
            {
                _chipMoveAnimation.Completed += (s, e) =>
                {
                    ReassignChipImageInNewCell(cellIndex, toMove, newInsideChipPoint);
                    if (IfLastMoveIsPrison)
                    {
                        IfLastMoveIsPrison = false;
                        MakeAMoveToInPrisonCell(IfPlayerSitsInPrisonByChipImage(toMove), toMove);
                    }

                    //if (_ifGoesThroughSquareCell) return;
                    _ifChipMoves = false;
                };
            }

            DoubleAnimation moveYAnimation = new DoubleAnimation
            {
                From = 0,
                To = endPoint.Y,
                Duration = TimeSpan.FromSeconds(1),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };

            transform.BeginAnimation(TranslateTransform.XProperty, _chipMoveAnimation);
            transform.BeginAnimation(TranslateTransform.YProperty, moveYAnimation);

            _ifChipMoves = true;
        }

        public bool IfPlayerSitsInPrisonByChipImage(Image chipImage)
        {
            int playerIndex = _imgs.IndexOf(chipImage);
            return _system.MonGame.IfPlayerSitsInPrison(playerIndex);
        }

        private void AddChipToChipMovePanel(Image img, Point startChipPoint)
        {
            Canvas.SetLeft(img, startChipPoint.X);
            Canvas.SetTop(img, startChipPoint.Y);

            ChipMovePanel.Children.Add(img);
        }

        private List<Image> _imgs = new List<Image>();
        private void SetChipPositionsInStartSquare()
        {
            _imgs = BoardHelper.GetAllChipsImages(_system.MonGame.Players.Count);

            for (int i = 0; i < _imgs.Count; i++)
            {
                chipAndCord.Add((_imgs[i], _system.MonGame.Players[i].Position));
            }

            for (int i = 0; i < _imgs.Count; i++)
            {
                SetStartChipInPlacerCanvas(_cells[_system.MonGame.Players[i].Position], _imgs[i]);
            }

            List<int> unique = _system.MonGame.GetUniquePositions();
            //Set Posiotions
            for (int i = 0; i < unique.Count; i++)
            {
                List<List<Point>> cellPoints = BoardHelper.GetChipsPoints(
                GetCellSize(_cells[unique[i]]));

                List<Point> res = cellPoints[_system.MonGame.GetAmountOfPlayersInCell(unique[i]) - 1];
                List<(Image, int)> chipsInCell = chipAndCord.Where(x => x.Item2 == unique[i]).ToList();

                for (int j = 0; j < chipsInCell.Count; j++)
                {
                    Canvas.SetLeft(chipsInCell[j].Item1, res[j].X);
                    Canvas.SetTop(chipsInCell[j].Item1, res[j].Y);
                }
            }
            return;
            List<List<Point>> points = BoardHelper.GetChipsPoints(
                new Size(StartCell.Width, StartCell.Height));

            List<Point> pointsForChips = points[_imgs.Count - 1];

            //pointsForChips = BoardHelper.GetPointsForPrisonCellSitter(5, new Size(PrisonCell.Width, PrisonCell.Height));

            for (int i = 0; i < _imgs.Count; i++)
            {
                Canvas.SetLeft(_imgs[i], pointsForChips[i].X);
                Canvas.SetTop(_imgs[i], pointsForChips[i].Y);

                StartCell.ChipsPlacer.Children.Add(_imgs[i]);
            }
        }

        private Size GetCellSize(UIElement cell)
        {
            if (cell is SquareCell square)
            {
                return new Size(square.ChipsPlacer.Width, square.ChipsPlacer.Height);
            }
            else if (cell is UpperCell up)
            {
                return new Size(up.ChipsPlacer.Width, up.ChipsPlacer.Height);
            }
            else if (cell is PrisonCell prison)
            {
                return new Size(prison.ChipsPlacer.Width, prison.ChipsPlacer.Height);
            }
            else if (cell is RightCell right)
            {
                return new Size(right.ChipsPlacer.Width, right.ChipsPlacer.Height);
            }
            else if (cell is BottomCell bot)
            {
                return new Size(bot.ChipsPlacer.Width, bot.ChipsPlacer.Height);
            }
            else if (cell is LeftCell left)
            {
                return new Size(left.ChipsPlacer.Width, left.ChipsPlacer.Height);
            }
            throw new Exception("Type of cell is not exist... Just How?");
        }

        private void SetStartChipInPlacerCanvas(UIElement cell, Image chip)
        {
            if (cell is SquareCell square)
            {
                square.ChipsPlacer.Children.Add(chip);
            }
            else if (cell is UpperCell up)
            {
                up.ChipsPlacer.Children.Add(chip);
            }
            else if (cell is PrisonCell prison)
            {
                prison.ChipsPlacer.Children.Add(chip);
            }
            else if (cell is RightCell right)
            {
                right.ChipsPlacer.Children.Add(chip);
            }
            else if (cell is BottomCell bot)
            {
                bot.ChipsPlacer.Children.Add(chip);
            }
            else if (cell is LeftCell left)
            {
                left.ChipsPlacer.Children.Add(chip);
            }
        }


        //

        public void HideHeadersInCells()
        {
            ChanceOne.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceTwo.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceThree.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceFour.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceFive.MoneyPlacer.Visibility = Visibility.Hidden;
            ChanceSix.MoneyPlacer.Visibility = Visibility.Hidden;

            LittleTax.MoneyPlacer.Visibility = Visibility.Hidden;
            BigTax.MoneyPlacer.Visibility = Visibility.Hidden;
        }

        private const int _upperMargin = 10;
        //That cant be changed with box items
        private void SetImmoratlImages()
        {
            SetSquareCells();

            SetTaxesImages();
            SetChances();

            SetBasicBusinessesImages();
        }

        public void SetBasicBusinessesImages()
        {
            //Get (to make in future) parameters for image from table

            SetBasicPerfumeImages();
            SetBasicClothesImages();

            SetBasicMessagerImages();
            SetBasicDrinkingsImages();

            SetBasicPlanesImages();
            SetBasicFoodImages();

            SetBasicHotelImages();
            SetBasicPhoneImages();

            SetBasicCarImages();
            SetBasicGameImages();
        }

        private void SetBasicGameImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("rockstar_games.png", "Games"), GamesFirstBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("rovio.png", "Games"), GamesSecondBus, new Size(100, 25));
        }

        private void SetBasicCarImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("mercedes.png", "Cars"), UpCar, new Size(50, 50));
            SetRightCellImage(BoardHelper.GetImageFromFolder("audi.png", "Cars"), RightCar, new Size(100, 30));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("ford.png", "Cars"), DownCars, new Size(95, 40));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("land_rover.png", "Cars"), LeftCar, new Size(100, 45));

        }

        private void SetBasicPhoneImages()
        {
            SetLeftCellImage(BoardHelper.GetImageFromFolder("apple.png", "Phones"), PhonesFirstBus, new Size(40, 45));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("nokia.png", "Phones"), PhonesSecondBus, new Size(100, 20));
        }

        private void SetBasicHotelImages()
        {
            SetLeftCellImage(BoardHelper.GetImageFromFolder("holiday_inn.png", "Hotels"), HotelFirstBus, new Size(65, 45));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("radisson_blu.png", "Hotels"), HotelSecondBus, new Size(100, 35));
            SetLeftCellImage(BoardHelper.GetImageFromFolder("novotel.png", "Hotels"), HotelThirdBus, new Size(100, 45));
        }

        private void SetLeftCellImage(Image img, LeftCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicFoodImages()
        {
            SetBottomCellImage(BoardHelper.GetImageFromFolder("max_burgers.png", "Food"), FoodFirstBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("burger_king.png", "Food"), FoodSecondBus, new Size(55, 45));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("kfc.png", "Food"), FoodThirdBus, new Size(55, 45));
        }

        private void SetBasicPlanesImages()
        {
            SetBottomCellImage(BoardHelper.GetImageFromFolder("lufthansa.png", "Planes"), PlanesFirstBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("british_airways.png", "Planes"), PlanesSecondBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("american_airlines.png", "Planes"), PlanesThirdBus, new Size(100, 20));
        }

        private void SetBottomCellImage(Image img, BottomCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicDrinkingsImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("coca_cola.png", "Drinkings"), DrinkFirstBus, new Size(100, 35));
            SetRightCellImage(BoardHelper.GetImageFromFolder("fanta.png", "Drinkings"), DrinkSecondBus, new Size(65, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("pepsi.png", "Drinkings"), DrinkThirdBus, new Size(100, 35));
        }

        private void SetBasicMessagerImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("vk.png", "Messagers"), MessagerFirstBus, new Size(75, 45));
            SetRightCellImage(BoardHelper.GetImageFromFolder("facebook.png", "Messagers"), MessagerSecondBus, new Size(100, 30));
            SetRightCellImage(BoardHelper.GetImageFromFolder("twitter.png", "Messagers"), MessagerThirdBus, new Size(55, 45));
        }

        private void SetRightCellImage(Image img, RightCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetRightLeftImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Fill;
            img.Width = imageSize.Width;
            img.Height = imageSize.Height;
        }



        private void SetBasicClothesImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("adidas.png", "Clothes"), ClothesFirstBus, new Size(75, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("puma.png", "Clothes"), ClothesSeconBus, new Size(100, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("lacoste.png", "Clothes"), ClothesThirdBus, new Size(100, 45));
        }

        public void SetBasicPerfumeImages()
        {
            SetUpperCellImage(BoardHelper.GetImageFromFolder("chanel.png", "Perfume"), PerfumeFirstBus, new Size(75, 45));
            SetUpperCellImage(BoardHelper.GetImageFromFolder("hugo_boss.png", "Perfume"), PerfumeSecondBus, new Size(100, 40));
        }

        private void SetUpperCellImage(Image img, UpperCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Fill;

            img.Width = imageSize.Width;
            img.Height = imageSize.Height;

            SetRenderToTurnImageForUpDownCell(img);
        }

        private void SetRenderToTurnImageForUpDownCell(Image img)
        {
            img.RenderTransformOrigin = new Point(0.5, 0.5);

            RotateTransform transform = new RotateTransform()
            {
                Angle = -90,
            };

            img.LayoutTransform = transform;
        }

        private Size _chanseSize = new Size(55, 80);
        private void SetChances()
        {
            SetUpDownChances();
            SetRightChances();
            SetLeftChances();
        }

        public void SetLeftChances()
        {
            ChanceFive.ImagePlacer.Children.Add(GetLeftChances());
            ChanceSix.ImagePlacer.Children.Add(GetLeftChances());
        }

        private Image GetLeftChances()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chance_rotated.png");

            img.Width = _chanseSize.Height;
            img.Height = ChanceFive.ImagePlacer.Height;

            return img;
        }

        private Image GetRightChanceIamge()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chanceUp.png");

            img.Width = this.Width;
            img.Height = ChanceFive.ImagePlacer.Height;

            return img;
        }

        private void SetRightChances()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chance.png");

            img.Width = _chanseSize.Height;
            img.Height = ChanceThree.ImagePlacer.Height;

            ChanceThree.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownChances()
        {
            ChanceOne.ImagePlacer.Children.Add(GetUpChanceImage());
            ChanceTwo.ImagePlacer.Children.Add(GetUpChanceImage());
            ChanceFour.ImagePlacer.Children.Add(GetUpChanceImage());

        }

        private Image GetUpChanceImage()
        {
            Image img = BoardHelper.GetImageFromOtherFolder("chanceUp.png");

            img.Width = _chanseSize.Width;
            img.Height = ChanceOne.ImagePlacer.Height;

            return img;
        }

        private void SetTaxesImages()
        {
            //Set little Tax
            Image little = BoardHelper.GetTaxImageByName("tax_income.png");
            little.Width = LittleTax.Width - _upperMargin;
            little.Height = LittleTax.Height / 2;

            little.HorizontalAlignment = HorizontalAlignment.Center;
            little.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(little, 10);

            LittleTax.ImagePlacer.Children.Add(little);

            //Set big tax

            Image big = BoardHelper.GetTaxImageByName("tax_luxury.png");
            big.Width = BigTax.Width / 2;
            big.Height = big.Height - 10;

            big.HorizontalAlignment = HorizontalAlignment.Center;
            big.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(big, 10);

            BigTax.ImagePlacer.Children.Add(big);
        }

        public void SetSquareCells()
        {
            SetStartImage();
            SetJackPotImage();
            SetGoToPrisonImage();
            SetPrisonImage();
        }

        private void SetPrisonImage()
        {
            const int width = 50;
            const int height = 50;
            Image jail = new Image()
            {
                Source = BoardHelper.GetSquareByName("jail.png").Source,
                Width = width + 10,
                Height = height + 10
            };

            Canvas.SetLeft(jail, 0);
            Canvas.SetTop(jail, 65);

            PrisonCell.ImagesPlacer.Children.Add(jail);


            Image donat = new Image()
            {
                Source = BoardHelper.GetSquareByName("jail-visiting.png").Source,
                Width = width,
                Height = height
            };

            Canvas.SetLeft(donat, 65);
            Canvas.SetTop(donat, 10);

            PrisonCell.ImagesPlacer.Children.Add(donat);
        }

        private void SetGoToPrisonImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("goToJail.png").Source,
                Width = 125,
                Height = 125
            };

            GoToPrisonCell.ImagesPlacer.Children.Add(img);
        }

        private void SetJackPotImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("jackpot.png").Source,
                Width = 125,
                Height = 125
            };

            JackPotCell.ImagesPlacer.Children.Add(img);
        }

        private void SetStartImage()
        {
            Image img = new Image()
            {
                Source = BoardHelper.GetSquareByName("start.png").Source,
                Width = 125,
                Height = 125
            };

            StartCell.ImagesPlacer.Children.Add(img);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.Enter) &&
                MessageWriter.Text != string.Empty)
            {
                AddMessageTextBlock(MessageWriter.Text);
                MessageWriter.Text = string.Empty;
            }
        }

        private void AddMessageTextBlock(string message)
        {
            TextBlock block = new TextBlock()
            {
                Text = message,
                FontSize = 16,
                Foreground = Brushes.White
            };

            GameChat.Items.Add(block);
        }

        private List<UIElement> _cells = new List<UIElement>();
        private void SetCellsInList()
        {
            _cells.Add(StartCell);

            for (int i = 0; i < FirstLinePanel.Children.Count; i++)
            {
                if (FirstLinePanel.Children[i] is UpperCell)
                {
                    _cells.Add(FirstLinePanel.Children[i]);
                }
            }
            _cells.Add(PrisonCell);

            for (int i = 0; i < SecondLinePanel.Children.Count; i++)
            {
                if (SecondLinePanel.Children[i] is RightCell)
                {
                    _cells.Add(SecondLinePanel.Children[i]);
                }
            }

            _cells.Add(JackPotCell);

            for (int i = ThirdLinePanel.Children.Count - 1; i >= 0; i--)
            {
                if (ThirdLinePanel.Children[i] is BottomCell)
                {
                    _cells.Add(ThirdLinePanel.Children[i]);
                }
            }

            _cells.Add(GoToPrisonCell);

            for (int i = FourthLinePanel.Children.Count - 1; i >= 0; i--)
            {
                if (FourthLinePanel.Children[i] is LeftCell)
                {
                    _cells.Add(FourthLinePanel.Children[i]);
                }
            }
        }

        public void PlayerGaveUp()
        {
            int allPlayersMoney = _system.MonGame.GetAllPlayersActivitiesPrice();
            RepaintAllPlayersCells();
            _system.MonGame.StepperGaveUp();

            IfSomeOneWon();
        }

        public void IfSomeOneWon()
        {
            if (_system.MonGame.IfSomeOneWon())
            {
                MessageBox.Show("Game is ended");
            }
        }

        public void RepaintAllPlayersCells()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IfCellIsSteppersBusiness(i))
                {
                    ClearCell(_cells[i], i);
                }
            }
        }

        //Repaint bg and clear stars
        public void ClearCell(UIElement el, int cellIndex)
        {
            if (el is UpperCell up)
            {
                up.ImagePlacer.Background = Brushes.White;
                up.ImagePlacer.Opacity = 1;
                up.StarsGrid.Children.Clear();
            }
            else if (el is RightCell right)
            {
                right.ImagePlacer.Background = Brushes.White;
                right.ImagePlacer.Opacity = 1;
                right.StarsGrid.Children.Clear();
            }
            else if(el is BottomCell bot)
            {
                bot.ImagePlacer.Background = Brushes.White;
                bot.ImagePlacer.Opacity = 1;
                bot.StarsGrid.Children.Clear();
            }
            else if(el is LeftCell left)
            {
                left.ImagePlacer.Background = Brushes.White;
                left.ImagePlacer.Opacity = 1;
                left.StarsGrid.Children.Clear();
            }
            ChangePriceInBusiness(cellIndex, _system.MonGame.GetBusPrice(cellIndex).ToString());
        }


        private int _tradeReciveIndex;
        private TradeOfferEl _tradeOffer;
        public void CreateTrade(int traderIndex)
        {
            if (ChatMessages.Children.OfType<TradeOfferEl>().Any()) return;
            _tradeReciveIndex = traderIndex;

            _system.MonGame.CreateTrade();
            _system.MonGame.SetTradeReciverIndex(traderIndex);

            ClearClickInfoEventForBusCells();
            SetTradeMouseDownForBusses(true);

            _tradeOffer = new TradeOfferEl();

            _tradeOffer.GiverItem.ItemName.Text = _system.MonGame.GetStepperLogin();
            _tradeOffer.GiverItem.ItemType.Text = "Sender Login";

            _tradeOffer.ReciverItem.ItemName.Text = _system.MonGame.GetPlayerLoginByIndex(traderIndex);
            _tradeOffer.ReciverItem.ItemType.Text = "Reciever login";

            _tradeOffer.CloseTrade.MouseDown += (sender, e) =>
            {
                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());
            };

            _tradeOffer.SendTradeBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfTwiceRuleinTradeComplite())
                {
                    _tradeOffer.SendTradeBut.Visibility = Visibility.Hidden;
                    _tradeOffer.AcceptanceButtons.Visibility = Visibility.Visible;

                    SetEventsForAcceptenceButsInTrade();
                }
                else
                {
                    AddMessageTextBlock("Twice rule does not complite");
                }
            };

            SetEventsForMoneyBoxesInTrade();

            ChatMessages.Children.Add(_tradeOffer);
        }

        private void SetEventsForAcceptenceButsInTrade()
        {
            _tradeOffer.AcceptTradeBut.Click += (sender, e) =>
            {
                _system.MonGame.AcceptTrade();

                UpdatePlayersMoney();
                RepaintAfterTradeBuses();

                AddMessageTextBlock("Trade accepted!");
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());
            };

            _tradeOffer.DeclineTradeBut.Click += (sender, e) =>
            {
                AddMessageTextBlock("Trade is declined");
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());
            };
        }

        private void RepaintAfterTradeBuses()
        {
            PaintCellsAfterTrade(_system.MonGame._trade.SenderIndex,
                _system.MonGame._trade.GetReciverIndexes());

            PaintCellsAfterTrade(_system.MonGame._trade.ReciverIndex,
                _system.MonGame._trade.GetSenderCellsIndexes());
        }

        private void PaintCellsAfterTrade(int newOwnerIndex, List<int> cellsIndexes)
        {
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                PaintCellInColor(cellsIndexes[i], _colors[newOwnerIndex]);
            }
        }

        private void SetEventsForMoneyBoxesInTrade()
        {
            _tradeOffer.SenderMoney.AmountOfMoneyBox.TextChanged += (sender, e) =>
            {
                int senderMoney = _tradeOffer.GetSenderTradeMoney();
                _system.MonGame.SetSenderMoneyTrade(senderMoney);

                _tradeOffer.UpdateSenderTotalMoney(_system.MonGame.GetSenderTotalMoneyForTrde());

            };

            _tradeOffer.ReciverMoney.AmountOfMoneyBox.TextChanged += (sender, e) =>
            {
                int reciverMoney = _tradeOffer.GetReciverTradeMoney();
                _system.MonGame.SetReciverMoneyTrade(reciverMoney);

                _tradeOffer.UpdateReciverTotalMoney(_system.MonGame.GetReciverTotalMoneyForTrade());
            };
        }

        private void UpdateTradeMoneyBoxes()
        {
            _tradeOffer.UpdateSenderTotalMoney(_system.MonGame.GetSenderTotalMoneyForTrde());
            _tradeOffer.UpdateReciverTotalMoney(_system.MonGame.GetReciverTotalMoneyForTrade());
        }


        private void SetTradeMouseDownForBusses(bool ifAddEvents)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SetTradeMouseDownForCell(_cells[i], ifAddEvents);
            }
        }

        public void SetTradeMouseDownForCell(UIElement element, bool ifAdd)
        {
            if (element is UpperCell upper)
            {
                if (ifAdd) upper.PreviewMouseDown += TradeBuss_PreviewMouseDown;
                else upper.PreviewMouseDown -= TradeBuss_PreviewMouseDown;
            }
            else if (element is RightCell right)
            {
                if (ifAdd) right.PreviewMouseDown += TradeBuss_PreviewMouseDown;
                else right.PreviewMouseDown -= TradeBuss_PreviewMouseDown;
            }
            else if (element is BottomCell bottom)
            {
                if (ifAdd) bottom.PreviewMouseDown += TradeBuss_PreviewMouseDown;
                else bottom.PreviewMouseDown -= TradeBuss_PreviewMouseDown;
            }
            else if (element is LeftCell left)
            {
                if (ifAdd) left.PreviewMouseDown += TradeBuss_PreviewMouseDown;
                else left.PreviewMouseDown -= TradeBuss_PreviewMouseDown;
            }
        }

        public void TradeBuss_PreviewMouseDown(object sender, EventArgs e)
        {
            int cellIndex = GetCellIndex((UIElement)sender);
            if (!_system.MonGame.IfCellIndexIsBusiness(cellIndex)) return; //cell is business
            if (!_system.MonGame.IfSteperOwnsBusiness(cellIndex) &&
                !_system.MonGame.IfPlayerOwnsBusiness(_tradeReciveIndex, cellIndex)) return; //Cell owner is sender or reciver 

            ParentBus bus = _system.MonGame.GetBusinessByIndex(cellIndex);
            if (_tradeOffer.IfTradeItemNameContainsInList(bus.Name)) return; // such bus is already exist
            TradeItem item = new TradeItem(bus, GetImageFromBusCell(_cells[cellIndex]));

            _system.MonGame.AddBusinesInTrade(cellIndex);
            UpdateTradeMoneyBoxes();

            item.PreviewMouseDown += (senderItem, action) =>
            {
                if (senderItem is TradeItem tradeItem)
                {
                    _tradeOffer.SenderListBox.Items.Remove(tradeItem);
                    _tradeOffer.ReciverListBox.Items.Remove(tradeItem);

                    _system.MonGame.RemoveBusinessFromTrade(cellIndex);
                    UpdateTradeMoneyBoxes();
                }
            };

            AddTradeItemToListBox(item, _system.MonGame.IfSteperOwnsBusiness(cellIndex));
        }

        public void AddTradeItemToListBox(TradeItem item, bool ifAddToSender)
        {
            if (ifAddToSender) _tradeOffer.SenderListBox.Items.Add(item);
            else _tradeOffer.ReciverListBox.Items.Add(item);
        }

        private Image GetImageFromBusCell(UIElement element)
        {
            if (element is UpperCell upper)
                return upper.ImagePlacer.Children.OfType<Image>().First();

            if (element is RightCell right)
                return right.ImagePlacer.Children.OfType<Image>().First();

            if (element is BottomCell bottom)
                return bottom.ImagePlacer.Children.OfType<Image>().First();

            if (element is LeftCell left)
                return left.ImagePlacer.Children.OfType<Image>().First();

            throw new Exception("No image in iamgePlacer!");
        }

        private int GetCellIndex(UIElement cell)
        {
            return _cells.IndexOf(cell);
        }

        private void ClearClickInfoEventForBusCells()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                ClearClickInfoEventForBus(_cells[i]);
            }
        }

        private void ClearClickInfoEventForBus(UIElement element)
        {
            if (element is UpperCell upper)
                upper.PreviewMouseDown -= Bussiness_PreviewMouseDown;

            if (element is RightCell right)
                right.PreviewMouseDown -= Bussiness_PreviewMouseDown;

            if (element is BottomCell bottom)
                bottom.PreviewMouseDown -= Bussiness_PreviewMouseDown;

            if (element is LeftCell left)
                left.PreviewMouseDown -= Bussiness_PreviewMouseDown;
        }
    }
}
