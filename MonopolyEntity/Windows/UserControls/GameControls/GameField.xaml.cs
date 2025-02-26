using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Media.Animation;
using MonopolyEntity.Windows.UserControls.GameControls.BussinessInfo;


using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages;
using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.Cell.Bus;
using System.Xml.Linq;
using MaterialDesignThemes.Wpf;
using System.Threading;
using MonopolyDLL.Monopoly.Enums;
using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls;
using System.Reflection;
using MonopolyDLL.Monopoly.InventoryObjs;
using System.Security.Cryptography.X509Certificates;
using MonopolyDLL.Monopoly.TradeAction;
using MahApps.Metro.Controls;
using System.Text;


using MonopolyEntity.Windows.UserControls.GameControls.Other;
using System.Security.AccessControl;
using MonopolyEntity.Windows.Pages;
using System.Runtime.Remoting.Channels;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace MonopolyEntity.Windows.UserControls.GameControls
{
    /// <summary>
    /// Логика взаимодействия для GameField.xaml
    /// </summary>
    /// ★★

    public partial class GameField : UserControl
    {
        private MonopolySystem _system;
        private List<UserCard> _cards;
        private Frame _frame;
        public GameField(MonopolySystem system, List<UserCard> cards, Frame frame)
        {
            _system = system;
            _cards = cards;
            _frame = frame;

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

            AddHandCursorToBusses();

            SetEndTimerEvents();

            //SetOwnerToAllCells();
        }

        public void SetEndTimerEvents()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (!(_cards[i].UserTimer._timer is null) &&
                    _cards[i].UserTimer.Visibility == Visibility.Visible)
                {
                    int index = i;
                    _cards[index].UserTimer._timer.Elapsed += (sender, e) =>
                    {
                        if (_cards[index].UserTimer._timer.Enabled || _gameEnded) return;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PlayerGaveUp();
                            SetEndTimerEvents();
                        });
                    };
                }
            }
        }

        public void AddHandCursorToBusses()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IfCellIndexIsBusiness(i))
                {
                    _cells[i].MouseEnter += (sender, e) =>
                    {
                        Cursor = Cursors.Hand;
                    };
                    _cells[i].MouseLeave += (sender, e) =>
                    {
                        Cursor = null;
                    };
                }
            }
        }

        public void SetOwnerToAllCells()
        {
            int stepperIndex = _system.MonGame.StepperIndex;
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.GameBoard.Cells[i] is ParentBus /*&& (i == 39 || i == 37)*/)
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
            //_system.MonGame.ClearStepperDoublesCounter();

            if (_system.MonGame.IfCubeDropsAreEqual() && !_system.MonGame.IfStepperSitsInPrison())
            {
                SetActionAfterStepperChanged();
                return;
            }

            //MakeEveryCardThinner();

            // return;
            //Get Back size for old Card
            _cards[_system.MonGame.StepperIndex].SetAnimation(null, false);

            //Change ing dll
            if (_system.MonGame.ChangeStepper())
            {
                UpdateDepositCounters();
            }

            //Set Size for new Card
            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
            SetActionAfterStepperChanged();
        }

        private void ChangeStepAfterAuction()
        {
            SetChangeStepperAfterAuction();
            SetActionAfterStepperChanged();
        }

        public void SetChangeStepperAfterAuction()
        {
            _system.MonGame.ChangeStepper();
            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
        }

        private void SetActionAfterStepperChanged()
        {
            _system.MonGame.ClearStepperBoughtHouseTypes();
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
            if (!_system.MonGame.IfStepperSatInPrisonTooMuch())
            {
                AddWrapPanelToChatBox("In prison, what ot do?", _system.MonGame.StepperIndex);
            }
            else AddWrapPanelToChatBox("Must pay money to go out", _system.MonGame.StepperIndex);

            ChatMessages.Children.Clear();
            PrisonQuestion question = new PrisonQuestion(_system.MonGame.IfPlayerHasEnoughMoneyToPAyPrisonBill(),
                _system.MonGame.IfSteppersCanOnlyGiveUp(_system.MonGame.GetPrisonPrice()));

            SetGiveUpActionToButton(question.GiveUpBut);

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
                    _cards[_system.MonGame.StepperIndex].UpdateTimer();

                    AddWrapPanelToChatBox($"You are luck to go out of prison." +
                        $" Droped {cubeVals.Item1} and {cubeVals.Item2}", _system.MonGame.StepperIndex);

                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperSitInPrisonCounter();

                    UpdatePlayersMoney();

                    _system.MonGame.SetCubes(cubeVals.Item1, cubeVals.Item2);
                    ChatMessages.Children.Clear();

                    _system.MonGame.ClearStepperDoublesCounter();
                    //MakeAMoveAfterCubesDroped();
                    SetActionAfterStepperChanged();
                    return;
                }
                _system.MonGame.MakeStepperPrisonCounterHigher();
                ChatMessages.Children.Clear();
                AddWrapPanelToChatBox($"Values are not equal. You got {cubeVals.Item1} and {cubeVals.Item2}",
                    _system.MonGame.StepperIndex);

                if (_system.MonGame.IfStepperSatInPrisonTooMuch())
                {
                    SetPrisonQuestion();
                    return;
                }

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
                    AddWrapPanelToChatBox($"Paid {GetConvertedPrice(_system.MonGame.GetPrisonPrice())} to go out of prison", _system.MonGame.StepperIndex);

                    _cards[_system.MonGame.StepperIndex].UpdateTimer();

                    _system.MonGame.PayPrisonBill();
                    ChatMessages.Children.Clear();

                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperSitInPrisonCounter();

                    UpdatePlayersMoney();

                    SetActionAfterStepperChanged();
                    return;
                }
                AddWrapPanelToChatBox("Not enough money to pay prison price", _system.MonGame.StepperIndex);
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
            List<SolidColorBrush> temp = Helper.GetColorsQueue();

            for (int i = 0; i < _system.MonGame.Players.Count; i++)
            {
                _colors.Add(temp[i]);
            }
        }

        private void AddThroughCubesControl()
        {
            _system.MonGame.ClearStepperBoughtHouseTypes();

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
            DicesDrop drop = new DicesDrop(_system.MonGame.GetFirstCube(), _system.MonGame.GetSecondCube());
            drop.VerticalAlignment = VerticalAlignment.Center;

            drop._first3dCube._horizontalAnimation.Completed += CubesDropped_Completed;

            ChatMessages.Children.Add(drop);

            //MakeAMoveByChip();
        }

        private int _cellIndex;
        private EventHandler _checkEvent;
        private bool _toDropCubes = false;
        private bool _goToPrisonByDouble = false;

        private void CubesDropped_Completed(object sender, EventArgs e)
        {
            MakeAMoveAfterCubesDroped();
        }

        public void MakeAMoveAfterCubesDroped()
        {
            AddWrapPanelToChatBox($"Droped {_system.MonGame.GetFirstCube()} and {_system.MonGame.GetSecondCube()}", _system.MonGame.StepperIndex);

            int cellIndexToMoveOn = _system.MonGame.GetPointToMoveOn();
            _goToPrisonByDouble = false;

            if (_system.MonGame.IfCubeDropsAreEqual())
            {
                _cards[_system.MonGame.StepperIndex].UpdateTimer();
                _system.MonGame.AddToDoubleCounter();

                if (_system.MonGame.IfMaxDoublesIsAchieved())
                {
                    //Go To Prison
                    cellIndexToMoveOn = _system.MonGame.GetPrisonIndex();
                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperDoublesCounter();
                    _goToPrisonByDouble = true;

                    AddWrapPanelToChatBox("3 same drops, goes to prison", _system.MonGame.StepperIndex);
                    //return;
                }
            }

            if (cellIndexToMoveOn == _system.MonGame.GetPrisonIndex()) IfLastMoveIsPrison = true;
            _cellIndex = cellIndexToMoveOn;
            _toDropCubes = true;

            //Need to understand which cell is Cell On
            //Get enum action to show whats is happening
            _checkEvent += (compSender, eve) =>
            {
                ActionsAfterMoveOnBoard();
            };

            int tempPos = _system.MonGame.GetStepperPosition();

            MakeChipsMovementAction(tempPos,
                cellIndexToMoveOn, _imgs[_system.MonGame.StepperIndex]);

            //Change temp point 
            _system.MonGame.SetPlayerPosition(_goToPrisonByDouble);
        }

        public void ActionsAfterMoveOnBoard()
        {
            if (!_toDropCubes) return;
            _toDropCubes = false;

            //Check if stepper went through start(to get money)
            GoThroughStartCellCheck();

            //Remove dice 
            //ChatMessages.Children.Remove(ChatMessages.Children.OfType<DicesDrop>().First());
            ChatMessages.Children.Clear();
            _ifChipMoves = false;

            CellAction action = _system.MonGame.GetAction();

            if (_goToPrisonByDouble)
            {
                ChangeStepper();
                return;
            }
            SetActionVisuals(action);
        }


        private void GoThroughStartCellCheck()
        {
            if (_system.MonGame.IfStepperWentThroughStartCell())
            {
                int wentThroughStartMoney = _system.MonGame.GetGoThroughStartCellMoney();
                _system.MonGame.GetMoneyByStepper(wentThroughStartMoney);

                UpdatePlayersMoney();
                AddWrapPanelToChatBox($"Went through start cell. Get - {GetConvertedPrice(wentThroughStartMoney)}", _system.MonGame.StepperIndex);
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
                    SetGotOnGotToPrison();
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
                    SetVisitPrison();
                    break;
            }
            //trade - done
        }

        private bool _ifWithoutGoingThrugh = false;
        public void SetGotOnGotToPrison()
        {
            AddWrapPanelToChatBox("Goes to prison", _system.MonGame.StepperIndex);
            int stepperPosition = _system.MonGame.GetStepperPosition();
            int prisonCellIndex = _system.MonGame.GetPrisonIndex();
            _ifWithoutGoingThrugh = true;
            IfLastMoveIsPrison = true;
            _system.MonGame.Players[_system.MonGame.StepperIndex].IfSitInPrison = true;
            MakeChipsMovementAction(stepperPosition, prisonCellIndex, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(prisonCellIndex);
            _system.MonGame.ClearStepperDoublesCounter();
            ChangeStepper();
        }

        private void SetVisitPrison()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            AddWrapPanelToChatBox("Visits a prison", _system.MonGame.StepperIndex);
            ChangeStepper();
        }

        private void GotOnStartAction()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            int money = _system.MonGame.GetGotOnStartCellMoney();
            _system.MonGame.GetMoneyByStepper(money);

            UpdatePlayersMoney();
            AddWrapPanelToChatBox($"Got on start cell. Get - {GetConvertedPrice(money)}", _system.MonGame.StepperIndex);

            ChangeStepper();
        }

        private void SetGotOnChance()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            AddWrapPanelToChatBox("Got on chance", _system.MonGame.StepperIndex);

            ChanceAction action = /*ChanceAction.GoToPrison;//*/ _system.MonGame.GetChanceAction();

            string actionText = GetTextForChanceAction(action);
            AddWrapPanelToChatBox(actionText, _system.MonGame.StepperIndex);

            MakeChanceAction(action);

            UpdatePlayersMoney();
        }

        private void MakeChanceAction(ChanceAction action)
        {
            switch (action)
            {
                /*case ChanceAction.ForwardInOne:
                    MakeLittleMoveFromChance(_system.MonGame.GetIndexToStepOnForChance(action));
                    break;
                case ChanceAction.BackwardsInOne:
                    MakeLittleMoveFromChance(_system.MonGame.GetIndexToStepOnForChance(action));
                    break;*/
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
                    GoToPrisonFromChance();
                    break;
            }
        }

        private void MakeLittleMoveFromChance(int cellIndex)
        {
            int position = _system.MonGame.GetStepperPosition();

            MakeChipsMovementAction(position, cellIndex, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(cellIndex);
        }

        private void GoToPrisonFromChance()
        {
            int stepperPosition = _system.MonGame.GetStepperPosition();
            int prisonCellIndex = _system.MonGame.GetPrisonIndex();
            _ifWithoutGoingThrugh = true;
            IfLastMoveIsPrison = true;
            _system.MonGame.Players[_system.MonGame.StepperIndex].IfSitInPrison = true;
            MakeChipsMovementAction(stepperPosition, prisonCellIndex, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(prisonCellIndex);
            _system.MonGame.ClearStepperDoublesCounter();
            ChangeStepper();
            //

            /*            int stepperPosition = _system.MonGame.GetStepperPosition();
                        IfLastMoveIsPrison = true;
                        _system.MonGame.Players[_system.MonGame.StepperIndex].IfSitInPrison = true;
                        _ifChipMoves = false;
                        _system.MonGame.ClearStepperDoublesCounter();

                        MakeChipsMovementAction(stepperPosition, cellIndexToMoveOn, _imgs[_system.MonGame.StepperIndex]);
                        _system.MonGame.SetPlayerPositionAfterChanceMove(cellIndexToMoveOn);
                        ChangeStepper();*/
        }

        private void GetMoneyChanceAction(int money)
        {
            _system.MonGame.GetMoneyFromChance(money);
            ChangeStepper();
        }

        private void PayMoneyChanceAction(int money)
        {
            SetPayMoney("You got on chance cell", $"need to Pay {money}", money);
        }

        public void SetPayMoney(string firstLine, string secondLine, int money, bool ifEnemysBus = false)
        {
            PayMoney bill = new PayMoney(_system.MonGame.IfStepperHasEnoughMoneyToPayBill(money),
                _system.MonGame.IfSteppersCanOnlyGiveUp(money));
            SetGiveUpActionToButton(bill.GiveUpBut);

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
                    AddWrapPanelToChatBox($"Paid - {GetConvertedPrice(money)}", _system.MonGame.StepperIndex);
                    UpdatePlayersMoney();
                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<PayMoney>().First());

                    ChangeStepper(); //Next stepper after paid money
                }
                else
                {
                    //AddMessageTextBlock("Not enough money to pay the bill");
                }
            };
            ChatMessages.Children.Add(bill);
        }

        public string GetTextForChanceAction(ChanceAction action)
        {
            switch (action)
            {
                case ChanceAction.Pay500:
                    return $"lost {_system.MonGame.GameBoard.GetLitteleLoseMoneyChance()} in csgo cases";
                case ChanceAction.Pay1500:
                    return $"need to pay {_system.MonGame.GameBoard.GetBigLoseMoneyChance()} for health insurance";
                case ChanceAction.Get500:
                    return $"Found {_system.MonGame.GameBoard.GetLitteleWinMoneyChance()} bucks on the street";
                case ChanceAction.Get1500:
                    return $"Found {_system.MonGame.GameBoard.GetBigWinMoneyChance()} in winter jacket";
                case ChanceAction.GoToPrison:
                    return "Goes to prison";
            };

            throw new Exception("What can you get else");
        }

        private void SetGetOnCasino()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            AddWrapPanelToChatBox("Got on Casino game", _system.MonGame.StepperIndex);

            JackpotElem jackpot = new JackpotElem(_system.MonGame.IfPlayerHasEnoughMoneyToPlayCasino());

            jackpot.MakeBidBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfPlayerHasEnoughMoneyToPlayCasino() &&
                jackpot._chosenRibs.Count > 0)
                {
                    AddWrapPanelToChatBox($"Bidded on {GetChosenRibdsForCasinoInString(jackpot)}", _system.MonGame.StepperIndex);

                    //Set Cube drop
                    string winString = _system.MonGame.PlayCasino(jackpot._chosenRibs);
                    _system.MonGame.GetBillForCasino();
                    UpdatePlayersMoney();

                    AddCubeToCasinoAction(_system.MonGame.GetCasinoWinValue());

                    _casinoDice._horizontalAnimation.Completed += (send, ev) =>
                    {
                        Thread.Sleep(2000);
                        string casinoRes = winString;
                        AddWrapPanelToChatBox(casinoRes, _system.MonGame.StepperIndex);
                        ChatMessages.Children.Remove(ChatMessages.Children.
                        OfType<JackpotElem>().First());

                        UpdatePlayersMoney();

                        ChatMessages.Children.Clear();
                        ChangeStepper();
                    };
                }
            };

            jackpot.DeclineBut.Click += (sender, e) =>
            {
                AddWrapPanelToChatBox("Does not want to play casino", _system.MonGame.StepperIndex);

                ChatMessages.Children.Remove(ChatMessages.Children.
                    OfType<JackpotElem>().First());

                ChatMessages.Children.Clear();
                ChangeStepper();
            };

            ChatMessages.Children.Add(jackpot);
        }

        private string GetChosenRibdsForCasinoInString(JackpotElem elem)
        {
            string res = string.Empty;
            const string devider = " ";
            for (int i = 0; i < elem._chosenRibs.Count; i++)
            {
                res += (elem._chosenRibs[i].ToString() + devider);
            }
            return res;
        }

        private Dice _casinoDice;
        public void AddCubeToCasinoAction(int res)
        {
            const int cubeSize = 175;
            _casinoDice = new Dice(res)
            {
                Width = cubeSize,
                Height = cubeSize,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, 50)
            };

            ChatMessages.Children.Add(_casinoDice);
        }

        private void SetGotOnTax()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();

            string firstLine = $"You got on Tax Cell - " +
                $"{_system.MonGame.GetTempPositionCellName()}";

            string secondLine = $"You need to pay the bill - " +
                $"{_system.MonGame.GetBillFromTaxStepperPosition()}";

            int money = _system.MonGame.GetBillFromTaxStepperPosition();

            SetPayMoney(firstLine, secondLine, money);
            AddWrapPanelToChatBox($"Got on tax cell. To pay - {GetConvertedPrice(money)}", _system.MonGame.StepperIndex);
        }

        private void SetGotOnEnemysBusiness()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();

            string firstLine = $"You got on the enemys business - " +
                $"{_system.MonGame.GetTempPositionCellName()}";

            string secondLine = $"You need to pay the bill - " +
                $"{_system.MonGame.GetBillForBusinessCell()}";

            AddMessageWithTwoPlayers(_system.MonGame.StepperIndex, _system.MonGame.GetStepperPositionOwnerBus(), "gets on ",
                $"bus {secondLine}");

            int amountOfMoney = _system.MonGame.GetBillForBusinessCell();
            if (amountOfMoney == 0)
            {
                AddWrapPanelToChatBox("Got on deposited Business", _system.MonGame.StepperIndex);
                ChangeStepper();
                return;
            }

            SetPayMoney(firstLine, secondLine, amountOfMoney, true);
        }

        public void UpdatePlayersMoney()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (i > _system.MonGame.Players.Count - 1) return;
                _cards[i].UserMoney.Text = GetConvertedStringWithoutLastK(_system.MonGame.GetPlayersMoney(i));
            }
        }

        private void PlayerGotOnOwnBusiness()
        {
            AddWrapPanelToChatBox($"got on his own Business ({_system.MonGame.GetBusOnStepperName()})", _system.MonGame.StepperIndex);
            ChangeStepper();
        }

        private void SetBuyBusinessOffer()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();

            BuyBusiness buy = new BuyBusiness(_system.MonGame.IfStepperHasEnoughMoneyToBuyBus());
            buy.YourTurnText.Text = $"You got on {_system.MonGame.GetBusOnStepperName()}";
            AddWrapPanelToChatBox($"You got on {_system.MonGame.GetBusOnStepperName()}", _system.MonGame.StepperIndex);
            ChatMessages.Children.Add(buy);

            buy.BuyBusBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IfStepperHasEnughMoneyToBuyBus())
                {
                    SetSteppersBusFromInventory();

                    SetBuyingBusiness();
                    ChatMessages.Children.Clear();

                    SetLevelPayment(_system.MonGame.StepperIndex);

                    AddWrapPanelToChatBox($"bought {_system.MonGame.GetBusOnStepperName()} for " +
                        $"{GetConvertedPrice(_system.MonGame.GetSteppersBusPrice())}"
                        , _system.MonGame.StepperIndex);
                    ChangeStepper();
                }
                else
                {
                    //MessageBox.Show("Not enough money");
                }
            };

            buy.SendToAuctionBut.Click += (sender, e) =>
            {
                AddWrapPanelToChatBox($"does not want to buy {_system.MonGame.GetBusOnStepperName()} business", _system.MonGame.StepperIndex);

                //!!Make auction

                //auction action
                ChatMessages.Children.Clear();
                //ChangeStepper();
                //AddMessageTextBlock("Player is sending business to  auction");

                SetAuctionOffer();
            };
        }

        public void SetLevelPayment(int playerIndex)
        {
            _clickedCellIndex = _system.MonGame.GetStepperPosition();
            if (_system.MonGame.IfStepperPositionIsCar())
            {
                _system.MonGame.SetCarsPaymentLevels(playerIndex);
                SetCarsPayments(playerIndex);
            }
            else if (_system.MonGame.IfStepperPositionIsGame())
            {
                _system.MonGame.SetGamePaymentLevels(playerIndex);
                SetGamePayments(playerIndex);
            }
        }

        public void UpdateChatMessages()
        {
            if (ChatMessages.Children.Count == 0) return;

            if (ChatMessages.Children[0] is BuyBusiness bus)
            {
                bus.SetBuyButton(_system.MonGame.IfStepperHasEnoughMoneyToBuyBus());
            }
            else if (ChatMessages.Children[0] is JackpotElem jackPot)
            {
                jackPot.SetPlayButton(_system.MonGame.IfPlayerHasEnoughMoneyToPlayCasino());
            }
            else if (ChatMessages.Children[0] is PayMoney payMoney) // pay bus bill
            {
                payMoney.SetPayButton(_system.MonGame.IfPlayerHasEnoughMoneyToPayBusBill());
            }
            else if (ChatMessages.Children[0] is PrisonQuestion prisQuest)
            {
                prisQuest.SetPayButton(_system.MonGame.IfPlayerHasEnoughMoneyToPAyPrisonBill());
            }
        }

        private void SetSteppersBusFromInventory()
        {
            if (!_system.MonGame.IfStepperHasInventoryBusOnPosition()) return;
            _system.MonGame.SetPlayerSteppersBus();

            ParentBus bus = _system.MonGame.GetBusThatStepperIsOn();

            SetImageImg(bus);
        }

        private void SetImageImg(ParentBus bus)
        {
            BoxItem item = _system.MonGame.GetUserInventoryItem();
            UIElement cell = _cells[bus.GetId()];

            Image img = BoardHelper.GetAddedItemImage(item.ImagePath, item.Type);

            Grid grid = null;
            if (cell is UpperCell upper)
            {
                upper.ImagePlacer.Children.Add(img);
                grid = upper.ImagePlacer;
            }
            else if (cell is RightCell right)
            {
                right.ImagePlacer.Children.Add(img);
                grid = right.ImagePlacer;
            }
            else if (cell is BottomCell bottom)
            {
                bottom.ImagePlacer.Children.Add(img);
                grid = bottom.ImagePlacer;
            }
            else if (cell is LeftCell left)
            {
                left.ImagePlacer.Children.Add(img);
                grid = left.ImagePlacer;
            }


            MakeVisibleNewBusImage(grid);
        }

        private void MakeVisibleNewBusImage(Grid imagePlacer)
        {
            //2 - ususal bus pic and inventory bus pic
            if (imagePlacer is null || imagePlacer.Children.Count != 2) return;

            imagePlacer.Children[0].Visibility = Visibility.Hidden;
            imagePlacer.Children[1].Visibility = Visibility.Visible;
        }

        private void SetAuctionOffer()
        {
            _system.MonGame.SetStartAuctionPrice();
            _system.MonGame.SetStartPlayersForAuction();

            List<int> playresIndexesAuction = _system.MonGame._playerIndxesForAuction;

            if (playresIndexesAuction.Count == 0)
            {
                GameChat.Items.Add(GetTextBlockForMessage("No one has enough money to make bid"));
                ChangeStepper();
                return;
            }

            if (IfAuctionIsEnded()) return;

            SwipeUsersAnim(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
            AddBidControl();
        }

        public bool IfAuctionIsEnded()
        {
            if (IfNooneTakesPartInAuction())
            {
                AddWrapPanelToChatBox("Noone wants this business");

                MakeEveryCardThinner();

                _cards[_toAnumCardIndex]._horizAnim.Completed += (sender, e) =>
                {
                    ChangeStepAfterAuction();
                    _toAnumCardIndex = -1;
                };
                return true;
            }
            if (_system.MonGame.IfSomeOneWonAuction())
            {
                int startRent = _system.MonGame.GetStartPriceOfBoughtBusinessByStepper();
                ChangePriceInBusiness(_system.MonGame.GetStepperPosition(), startRent);

                PaintCellInColor(_system.MonGame.GetStepperPosition(), _colors[_system.MonGame._bidderIndex]);
                AddWrapPanelToChatBox($"is buying {_system.MonGame.GetBusOnStepperName()} for " +
                    $"{GetConvertedPrice(_system.MonGame.GetAuctionPrice())}", _system.MonGame.GetBidderIndex());

                SetLevelPayment(_system.MonGame._bidderIndex);

                //Get money form winner 
                //_system.MonGame.GetMoneyFromAuctionWinner();


                UpdatePlayersMoney();
                //Clear visuals after auction 
                _system.MonGame.ClearAuctionValues();


                MakeEveryCardThinner();

                _cards[_toAnumCardIndex]._horizAnim.Completed += (sender, e) =>
                {
                    ChangeStepAfterAuction();
                    _toAnumCardIndex = -1;
                };
                return true;
            }
            return false;
        }

        private int _toAnumCardIndex = -1;
        private void MakeEveryCardThinner()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (_cards[i].UserCardGrid.Background != _cards[i]._usualBrush)
                {
                    _cards[i].SetAnimation(null, false);
                    _toAnumCardIndex = i;
                }
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
                    AddWrapPanelToChatBox($"bidded {GetConvertedPrice(_system.MonGame.GetTempPriceInAuction())}", _system.MonGame.GetBidderIndex());

                    _system.MonGame.MakeBidInAuction();
                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                    if (IfAuctionIsEnded())
                    {
                        return;
                    }
                    _system.MonGame.SetNextBidder();

                    SwipeUsersAnim(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
                    _cards[_system.MonGame._bidderIndex]._horizAnim.Completed += (send, ev) =>
                    {
                        if (AddBidControl())
                        {
                            return;
                        }
                    };
                }
                else
                {
                    //AddMessageTextBlock("Not enough money!");
                }
            };

            bid.NotMakeABidBut.Click += (sender, e) =>
            {
                AddWrapPanelToChatBox("Does not wanna buy business", _system.MonGame.GetBidderIndex());

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                if (IfAuctionIsEnded())
                {
                    return;
                }
                if (_system.MonGame.RemoveAuctionBidderIfItsWasLast())
                {
                    if (IfAuctionIsEnded()) return;
                }

                SwipeUsersAnim(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
                _cards[_system.MonGame._bidderIndex]._horizAnim.Completed += (send, ev) =>
                {
                    if (AddBidControl()) return;
                };

            };

            ChatMessages.Children.Add(bid);
            return false;
        }

        private void SwipeUsersAnim(int makeUsualCardIndex, int toMarkCardIndex)
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

            _cards[_system.MonGame.StepperIndex].UserMoney.Text =
                GetConvertedStringWithoutLastK(_system.MonGame.GetSteppersMoney());

            _system.MonGame.SetStartLevelOfBusinessForStepper();
            int startRent = _system.MonGame.GetStartPriceOfBoughtBusinessByStepper();
            ChangePriceInBusiness(position, startRent);
        }

        public void ChangePriceInBusiness(int cellIndex, int price)
        {
            string newPrice = GetConvertedPrice(price);

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
                prices.Add(GetConvertedPrice(_system.MonGame.GameBoard.GetBusinessPrice(i)));
            }

            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IfCellIndexIsBusiness(i))
                {
                    SetPriceInCell(prices[i], _cells[i]);
                }
            }
        }

        public string GetConvertedPrice(int price)
        {
            StringBuilder build = new StringBuilder();

            for (int i = 0; i < price.ToString().Length; i++)
            {
                build.Append(price.ToString()[i]);
            }

            for (int i = price.ToString().Length; i >= 0; i--)
            {
                if (i % 3 == 0 && i != 0 && i != price.ToString().Length)
                {
                    build.Insert(price.ToString().Length - i, ",");
                }
            }

            build.Append("k");
            return build.ToString();
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
            (UIElement info, Size? size) = AddBusinessInfo();

            if (info is null) return;
            SetLocForInfoBox(info, (UIElement)sender, (Size)size);
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

            info.OneFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[0]);
            info.TwoFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[1]);
            info.ThreeFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[2]);
            info.FourFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[3]);

            info.FieldPrice.Text = GetConvertedStringWithoutLastK(bus.Price);
            info.DepositPriceText.Text = GetConvertedStringWithoutLastK(bus.DepositPrice);
            info.RebuyPrice.Text = GetConvertedStringWithoutLastK(bus.RebuyPrice);

            info.CarBusHeader.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];

            SetCarInfoVisualButs(info);
            SetEventsForCarsInfoButs(info);
        }

        public void SetCarInfoVisualButs(CarBusInfo info)
        {
            if (!_system.MonGame.IfStepperOwnsBusiness(_clickedCellIndex))
            {
                info.BusDesc.Visibility = Visibility.Visible;
                return;
            }

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
                    AddWrapPanelToChatBox($"Rebought bus on{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                    SetOpacityToBusiness(1);
                    _system.MonGame.RebuyBus(_clickedCellIndex);
                    //Set Players payments 
                    _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);

                    //Set Payment
                    SetCarsPayments(_system.MonGame.StepperIndex);
                    UpdatePlayersMoney();
                    BoardHelper.MakeDepositCounterHidden(_cells[_clickedCellIndex]);
                    UpdateChatMessages();
                    return;
                }
                //AddMessageTextBlock("Not enough money to rebuy business");
            };
        }

        public void SetCarDepositEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                AddWrapPanelToChatBox($"Deposited {_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                //Change Opacity
                SetOpacityToBusiness(0.5);
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Players payments 
                _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);

                //Set Payment
                SetCarsPayments(_system.MonGame.StepperIndex);
                UpdatePlayersMoney();

                BoardHelper.MakeDepositCounterVisible(_cells[_clickedCellIndex]);
                BoardHelper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                    _system.MonGame.GetDepositCounter(_clickedCellIndex));

                UpdateChatMessages();
            };
        }

        public void SetCarsPayments(int playerIndex)
        {
            ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));

            List<int> cellsIndexes =
                _system.MonGame.GetCarsIndexesWhichPlayerOwnNotDeposited(playerIndex);

            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                ChangePriceInBusiness(cellsIndexes[i], _system.MonGame.GetBusMoneyLevel(cellsIndexes[i]));
            }
        }

        private void SetGameBusInfoParams(GameBusInfo info)
        {
            GameBus bus = (GameBus)_system.MonGame.GameBoard.Cells[_clickedCellIndex];

            info.BusName.Text = bus.Name;
            info.BusType.Text = bus.BusType.ToString();

            info.BusDescription.Text = "This is game business. Payment eqauls " +
                "amount of game multed by cubes sum";

            info.OneFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[0]);
            info.TwoFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[1]);

            info.FieldPrice.Text = GetConvertedStringWithoutLastK(bus.Price);
            info.DepositPriceText.Text = GetConvertedStringWithoutLastK(bus.DepositPrice);
            info.RebuyPrice.Text = GetConvertedStringWithoutLastK(bus.RebuyPrice);

            info.GameBusHeader.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];

            SetGameInfoVisualButs(info);
            SetGameInfoEvents(info);
        }

        public void SetGameInfoVisualButs(GameBusInfo info)
        {
            if (!_system.MonGame.IfStepperOwnsBusiness(_clickedCellIndex))
            {
                info.BusDescription.Visibility = Visibility.Visible;
                return;
            }
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
                    AddWrapPanelToChatBox($"Rebought {_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                    SetOpacityToBusiness(1);
                    _system.MonGame.RebuyBus(_clickedCellIndex);

                    //Set Players payments 
                    _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);

                    //Set Payment
                    SetGamePayments(_system.MonGame.StepperIndex);
                    UpdatePlayersMoney();
                    BoardHelper.MakeDepositCounterHidden(_cells[_clickedCellIndex]);
                    UpdateChatMessages();
                    return;
                }
                // AddMessageTextBlock("Not enough money to rebuy business");
            };
        }

        public void SetGameDepositButEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                AddWrapPanelToChatBox($"Deposited bus {_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                //Change Opacity
                SetOpacityToBusiness(0.5);
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Players payments 
                _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);

                //Set Payment
                SetGamePayments(_system.MonGame.StepperIndex);
                UpdatePlayersMoney();

                BoardHelper.MakeDepositCounterVisible(_cells[_clickedCellIndex]);
                BoardHelper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                    _system.MonGame.GetDepositCounter(_clickedCellIndex));

                UpdateChatMessages();
            };
        }

        public void SetGamePayments(int playerIndex)
        {
            ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));

            List<int> cellsIndexes =
                _system.MonGame.GetGamesIndexesWhichPlayerOwnNotDeposited(playerIndex);

            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                ChangePriceInBusiness(cellsIndexes[i], _system.MonGame.GetBusMoneyLevel(cellsIndexes[i]));
            }
        }

        private void SetUsualBusInfoParams(UsualBusInfo info)
        {
            UsualBus bus = (UsualBus)_system.MonGame.GameBoard.Cells[_clickedCellIndex];

            info.BusName.Text = bus.Name;
            info.BusType.Text = bus.BusType.ToString();

            //info.DescriptionText.Text = "Build houses to get bigger payment";            

            info.BaseRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[0]);
            info.OneStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[1]);
            info.TwoStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[2]);
            info.ThreeStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[3]);
            info.FourStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[4]);
            info.YellowStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[5]);

            info.BusPriceMoney.Text = GetConvertedStringWithoutLastK(bus.Price);
            info.DepositPriceMoney.Text = GetConvertedStringWithoutLastK(bus.DepositPrice);
            info.RebuyPriceMoney.Text = GetConvertedStringWithoutLastK(bus.RebuyPrice);
            info.HousePriceMoney.Text = GetConvertedStringWithoutLastK(bus.BuySellHouse);

            info.NameBusBorder.Background = GetColorForUsualBusHeader(bus);

            SetEventsForUsualBusInfo(info);
            SetHouseButtonsForBusInfo(info);
        }

        public string GetConvertedStringWithoutLastK(int price)
        {
            string str = GetConvertedPrice(price);
            str = str.Remove(str.Length - 1);
            return str;
        }

        private void SetHouseButtonsForBusInfo(UsualBusInfo info)
        {
            if (!_system.MonGame.IfStepperOwnsBusiness(_clickedCellIndex) ||
                _system.MonGame.IfTypeContainsInBuiltCells(_system.MonGame.GameBoard.GetBusTypeByIndex(_clickedCellIndex)))
            {
                info.DescriptionText.Visibility = Visibility.Visible;
                return;
            }

            //Its in monopoly
            //if (!_system.MonGame.IfCellIsInMonopoly(_clickedCellIndex)) return;

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
                    AddWrapPanelToChatBox($"Rebought business on{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                    //Change opacity
                    SetOpacityToBusiness(1);
                    //rebuy bus
                    _system.MonGame.RebuyBus(_clickedCellIndex);

                    //Set Payment
                    ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));

                    UpdatePlayersMoney();
                    BoardHelper.MakeDepositCounterHidden(_cells[_clickedCellIndex]);
                    UpdateChatMessages();
                    return;
                }
                //AddMessageTextBlock("Not enough money to rebut busines");
            };
        }

        public void SetDepositBusEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                AddWrapPanelToChatBox($"Deposited {_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                //Change Opacity
                SetOpacityToBusiness(0.5);
                //Deposit bus
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Payment
                ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));
                UpdatePlayersMoney();

                BoardHelper.MakeDepositCounterVisible(_cells[_clickedCellIndex]);
                BoardHelper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                    _system.MonGame.GetDepositCounter(_clickedCellIndex));

                UpdateChatMessages();
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
                AddWrapPanelToChatBox($"Sold house on{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

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
                    AddWrapPanelToChatBox($"Buies house on{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                    _system.MonGame.BuyHouse(_clickedCellIndex);
                    SetHousesInCellParams();

                    _system.MonGame.AddStepperBoughtHouseType(_clickedCellIndex);
                    return;
                }
                //AddMessageTextBlock("Not enough money to buy house");
            };
        }

        public void SetHousesInCellParams()
        {
            SetCellStars(_clickedCellIndex);

            UpdatePlayersMoney();
            BussinessInfo.Children.Clear();

            ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));
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

            if (_ifWithoutGoingThrugh || _goToPrisonByDouble)
            {
                _ifWithoutGoingThrugh = false;
                _squareIndexesToGoThrough = null;
            }

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


            if (_squareIndexesToGoThrough is null) _chipMoveAnimation.Completed += _checkEvent;
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
                    Point InsidechipPoint = GetInsidePointToStepOn(_cells[_tempSquareValToGoOn], false);

                    //BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn])

                    ReassignChipImageInNewCell(_tempSquareValToGoOn, chipImg, InsidechipPoint);

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
                            MakeAMoveToInPrisonCell(IfPlayerSitsInPrisonByChipImage(chipImg));
                        }
                    }
                    else
                    {
                        _ifChipMoves = false;
                    }
                };
                MakeChipMoveToAnoutherCell(startCellIndex, _tempSquareValToGoOn, chipImg,
                    insidePointStartCell, newInsideChipPoint);
            }
            else
            {
                //usualMove
                MakeChipMoveToAnoutherCell(startCellIndex, cellIndexToMoveOn, chipImg, insidePointStartCell, newInsideChipPoint);
            }
        }

        //To Make a move in prison cell(sitter or visiter)
        private void MakeAMoveToInPrisonCell(bool ifSitsInPrison)
        {
            if (!ifSitsInPrison)
            {
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
            string canvasName = GetCanvasNameToStepOn(img, el);

            var chipsPlacerField = el.GetType().GetField(
                canvasName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (chipsPlacerField is null)
            {
                throw new Exception("Cell doesnt have chips placer");
            }
            return chipsPlacerField.GetValue(el) as Canvas;
        }

        public string GetCanvasNameToStepOn(Image img, UIElement el)
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
            Point startCell = GetChipCanvas(_cells[startCellIndex]).PointToScreen(new Point(0, 0));

            Point pointToStepOn = new Point(newCellPoint.X - startCell.X + newCellInsideChipPoint.X - prevCellInsideChipPoint.X,
                newCellPoint.Y - startCell.Y + newCellInsideChipPoint.Y - prevCellInsideChipPoint.Y);

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

                if (_squareIndexesToGoThrough.Count == 1) _chipMoveAnimation.Completed += _checkEvent;
            }
            else
            {
                _chipMoveAnimation.Completed += (s, e) =>
                {
                    ReassignChipImageInNewCell(cellIndex, toMove, newInsideChipPoint);

                    if (IfLastMoveIsPrison)
                    {
                        IfLastMoveIsPrison = false;
                        MakeAMoveToInPrisonCell(IfPlayerSitsInPrisonByChipImage(toMove));
                    }
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
            SetBottomCellImage(BoardHelper.GetImageFromFolder("american_airlines.png", "Planes"), PlanesFirstBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("lufthansa.png", "Planes"), PlanesSecondBus, new Size(100, 20));
            SetBottomCellImage(BoardHelper.GetImageFromFolder("british_airways.png", "Planes"), PlanesThirdBus, new Size(100, 20));
        }

        private void SetBottomCellImage(Image img, BottomCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicDrinkingsImages()
        {
            SetRightCellImage(BoardHelper.GetImageFromFolder("coca_cola.png", "Drinkings"), DrinkFirstBus, new Size(100, 35));
            SetRightCellImage(BoardHelper.GetImageFromFolder("pepsi.png", "Drinkings"), DrinkSecondBus, new Size(100, 35));
            SetRightCellImage(BoardHelper.GetImageFromFolder("fanta.png", "Drinkings"), DrinkThirdBus, new Size(65, 45));

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
            SetUpperCellImage(BoardHelper.GetImageFromFolder("hugo_boss.png", "Perfume"), PerfumeSecondBus, new Size(100, 45));
        }

        private void SetUpperCellImage(Image img, UpperCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetUpDownImage(Image img, Size imageSize)
        {
            img.Stretch = Stretch.Uniform;

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
                AddWrapPanelToChatBox(MessageWriter.Text, _system.MonGame.StepperIndex, true);
                MessageWriter.Text = string.Empty;
            }
        }


      
        private const int _textFontSize = 20;
        public void AddMessageWithTwoPlayers(int firstPlayerIndex, int secondPlayerIndex,
            string firstStr, string secondStr)
        {
            WrapPanel panel = GetWrapPanelForMessage(firstPlayerIndex);
            TextBlock offerBlock = GetTextBlockForMessage(firstStr);
            panel.Children.Add(offerBlock);

            TextBlock block = new TextBlock()
            {
                Text = _system.MonGame.GetPlayerLoginByIndex(secondPlayerIndex),
                Foreground = _colors[secondPlayerIndex],
                FontSize = _textFontSize,
                FontFamily = new FontFamily("Open sans")
            };
            panel.Children.Add(block);

            TextBlock secondTextBlock = GetTextBlockForMessage(secondStr);
            panel.Children.Add(secondTextBlock);

            GameChat.Items.Add(panel);
            ChatScroll.ScrollToEnd();
        }

        private void AddWrapPanelToChatBox(string message, int playerIndex = -1, bool ifPlayerWrote = false)
        {
            WrapPanel panel = new WrapPanel();
            if (playerIndex != -1) panel = GetWrapPanelForMessage(playerIndex);

            if (ifPlayerWrote && playerIndex != -1)
            {
                TextBlock block = panel.Children.OfType<TextBlock>().First();

                panel.Children.Remove(block);

                block.Foreground = new SolidColorBrush(Colors.White);
                Border border = new Border()
                {
                    CornerRadius = new CornerRadius(5),
                    Child = block,
                    Background = _colors[playerIndex],
                    Padding = new Thickness(5)
                };

                panel.Children.Add(border);
            }

            if (playerIndex != -1)
            {
                panel.Children.Add(GetTextBlockForMessage(message));
            }

            GameChat.Items.Add(panel);

            GameChat.PreviewMouseDown += Chat_MouseDown;

            ChatScroll.ScrollToEnd();
        }

        private void Chat_MouseDown(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        public TextBlock GetTextBlockForMessage(string message)
        {
            return new TextBlock()
            {
                Text = message,
                FontSize = _textFontSize,
                Foreground = Brushes.White,
                Margin = new Thickness(5, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                FontFamily = new FontFamily("Open sans"),
                TextWrapping = TextWrapping.Wrap,
            };
        }

        public WrapPanel GetWrapPanelForMessage(int playerIndex)
        {
            WrapPanel panel = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                Children =
                {
                    new TextBlock()
                    {
                        Text = _system.MonGame.GetPlayerLoginByIndex(playerIndex),
                        Foreground = _colors[playerIndex],
                        FontSize = _textFontSize,
                        FontFamily = new FontFamily("Open sans")
                    },
                }
            };
            return panel;
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

        public void SetGiveUpActionToButton(Button but)
        {
            but.Click += (sender, e) =>
            {
                PlayerGaveUp();
            };
        }


        public bool _gameEnded = false;
        public void PlayerGaveUp()
        {
            //If player owes money (bus bill)
            GiveAllMoneyToAnotherPlayer();

            RepaintAllPlayersCells();
            _system.MonGame.StepperGaveUp();
            SetPlayerCardIfHeGaveUp(_cards[_system.MonGame.StepperIndex]);
            _cards[_system.MonGame.StepperIndex].StopTimer();

            if (IfSomeOneWon()) return;

            RemoveLostPlayersChip(_system.MonGame.StepperIndex);

            ChatMessages.Children.Clear();
            ChangeStepper();
        }

        public void GiveAllMoneyToAnotherPlayer()
        {
            if (!_system.MonGame.IfStepperOnEnemiesBus()) return;
            _system.MonGame.GiveAllSteppersMoneyToBusOwner();
        }

        public void RemoveLostPlayersChip(int playerIndex)
        {
            Image toRemove = _imgs[playerIndex];

            //_imgs[playerIndex] = null;

            toRemove.Visibility = Visibility.Hidden;
            //((Canvas)toRemove.Parent).Children.Remove(toRemove);
        }

        public void SetPlayerCardIfHeGaveUp(UserCard card)
        {
            card.Opacity = 0.3;

            card.UserLoginCanvas.Visibility = Visibility.Hidden;
            card.UserMoneyAmountCanvas.Visibility = Visibility.Hidden;

            card.SkullImg.Visibility = Visibility.Visible;
        }

        public bool IfSomeOneWon()
        {
            if (!_system.MonGame.IfSomeOneWon() || _gameEnded) return false;

            if (!_gameEnded) MessageBox.Show("Game ended");
            _gameEnded = true;

            _frame.Content = new Pages.MainPage(_frame, _system);
            return true;
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
            else if (el is BottomCell bot)
            {
                bot.ImagePlacer.Background = Brushes.White;
                bot.ImagePlacer.Opacity = 1;
                bot.StarsGrid.Children.Clear();
            }
            else if (el is LeftCell left)
            {
                left.ImagePlacer.Background = Brushes.White;
                left.ImagePlacer.Opacity = 1;
                left.StarsGrid.Children.Clear();
            }
            ChangePriceInBusiness(cellIndex, _system.MonGame.GetBusPrice(cellIndex));
        }


        private int _tradeReciveIndex;
        private TradeOfferEl _tradeOffer;
        public void CreateTrade(int traderIndex)
        {
            if (ChatMessages.Children.OfType<TradeOfferEl>().Any()) return;
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            _tradeReciveIndex = traderIndex;

            _system.MonGame.CreateTrade();
            _system.MonGame.SetTradeReciverIndex(traderIndex);

            ClearClickInfoEventForBusCells();
            SetTradeMouseDownForBusses(true);

            _tradeOffer = new TradeOfferEl();

            _tradeOffer.SenderMoney.SetMaxMoney(_system.MonGame.GetTradeSenderMaxMoney());
            _tradeOffer.ReciverMoney.SetMaxMoney(_system.MonGame.GetTradeReciverMaxMoney());

            _tradeOffer.GiverItem.ItemName.Text = _system.MonGame.GetStepperLogin();
            _tradeOffer.GiverItem.ItemType.Text = "Sender Login";

            _tradeOffer.ReciverItem.ItemName.Text = _system.MonGame.GetPlayerLoginByIndex(traderIndex);
            _tradeOffer.ReciverItem.ItemType.Text = "Reciever login";

            _tradeOffer.CloseTrade.MouseDown += (sender, e) =>
            {
                _cards[_system.MonGame.StepperIndex].UpdateTimer();
                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());
            };

            _tradeOffer.SendTradeBut.Click += (sender, e) =>
            {
                if (!_system.MonGame.IfTradersHasEnoughMoney()) return;

                if (_system.MonGame.IfTwiceRuleinTradeComplite())
                {
                    AddMessageWithTwoPlayers(_system.MonGame.StepperIndex, _tradeReciveIndex, "Offers ", " to sign an offer");

                    _tradeOffer.SendTradeBut.IsEnabled = false;
                    SwipeUsersAnim(_system.MonGame._trade.GetSenderIndex(), _system.MonGame._trade.GetReciverIndex());

                    _cards[_system.MonGame._trade.GetSenderIndex()]._horizAnim.Completed += (sen, ev) =>
                    {
                        _tradeOffer.SendTradeBut.Visibility = Visibility.Hidden;
                        _tradeOffer.AcceptanceButtons.Visibility = Visibility.Visible;
                        SetEventsForAcceptanceButsInTrade();
                    };
                }
            };

            SetEventsForMoneyBoxesInTrade();

            ChatMessages.Children.Add(_tradeOffer);
        }

        private void SetEventsForAcceptanceButsInTrade()
        {
            _tradeOffer.AcceptTradeBut.Click += (sender, e) =>
            {
                AddWrapPanelToChatBox($"accepted trade", _system.MonGame._trade.ReciverIndex);

                MakeInActiveTradAnswerButs();
                _system.MonGame.AcceptTrade();

                UpdatePlayersMoney();
                RepaintAfterTradeBuses();

                CheckTradeBusesForInventoryBuses();

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());

                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);
                SwipeUsersAnim(_system.MonGame._trade.GetReciverIndex(), _system.MonGame._trade.GetSenderIndex());
            };

            _tradeOffer.DeclineTradeBut.Click += (sender, e) =>
            {
                AddWrapPanelToChatBox($"Declined trade", _system.MonGame._trade.ReciverIndex);

                MakeInActiveTradAnswerButs();
                SwipeUsersAnim(_system.MonGame._trade.GetReciverIndex(), _system.MonGame._trade.GetSenderIndex());

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());

                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);
            };
        }

        private void MakeInActiveTradAnswerButs()
        {
            _tradeOffer.AcceptTradeBut.IsEnabled = false;
            _tradeOffer.DeclineTradeBut.IsEnabled = false;
        }

        private void CheckTradeBusesForInventoryBuses()
        {
            TradeClass trade = _system.MonGame.GetTrade();

            //Clear senders and receivers busses
            ClearInventoriedBuses(trade.SenderBusesIndexes, trade.ReciverIndex);
            ClearInventoriedBuses(trade.ReciverBusesIndexes, trade.SenderIndex);
            //Set new Inventory cells for Sender and receiver

            SetPlayersInventoryItems(trade.ReciverIndex, trade.SenderBusesIndexes);
            SetPlayersInventoryItems(trade.SenderIndex, trade.ReciverBusesIndexes);

            SetStartRentForBuses(trade.SenderBusesIndexes);
            SetStartRentForBuses(trade.ReciverBusesIndexes);
        }

        private void SetStartRentForBuses(List<int> cellIndexes)
        {
            for (int i = 0; i < cellIndexes.Count; i++)
            {
                int startRent = _system.MonGame.GetStartPaymentForBusByIndex(cellIndexes[i]);
                ChangePriceInBusiness(cellIndexes[i], startRent);
            }
        }

        private void SetPlayersInventoryItems(int playerIndex, List<int> cellsIndexes)
        {
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                if (_system.MonGame.IfPlayerHasInventoryItemOnIndex(playerIndex, cellsIndexes[i]))
                {
                    _system.MonGame.SetInventoryItemForPlayer(playerIndex, cellsIndexes[i]);

                    ParentBus bus = _system.MonGame.GetBusinessByIndex(cellsIndexes[i]);

                    SetImageImg(bus);
                }
            }
        }

        private void ClearInventoriedBuses(List<int> cellsIndexes, int newOwnerIndex)
        {
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                ClearInventoryBusImage(cellsIndexes[i]);
                _system.MonGame.SetBasicBusBack(cellsIndexes[i]);
                _system.MonGame.GetBusinessByIndex(cellsIndexes[i]).ChangeOwner(newOwnerIndex);
            }
        }

        private void ClearInventoryBusImage(int cellIndex)
        {
            UIElement cell = _cells[cellIndex];

            if (cell is UpperCell up && up.ImagePlacer.Children.Count > 1)
            {
                up.ImagePlacer.Children.RemoveAt(up.ImagePlacer.Children.Count - 1);
                up.ImagePlacer.Children[0].Visibility = Visibility.Visible;
            }
            else if (cell is RightCell right && right.ImagePlacer.Children.Count > 1)
            {
                right.ImagePlacer.Children.RemoveAt(right.ImagePlacer.Children.Count - 1);
                right.ImagePlacer.Children[0].Visibility = Visibility.Visible;
            }
            else if (cell is BottomCell bot && bot.ImagePlacer.Children.Count > 1)
            {
                bot.ImagePlacer.Children.RemoveAt(bot.ImagePlacer.Children.Count - 1);
                bot.ImagePlacer.Children[0].Visibility = Visibility.Visible;
            }
            else if (cell is LeftCell left && left.ImagePlacer.Children.Count > 1)
            {
                left.ImagePlacer.Children.RemoveAt(left.ImagePlacer.Children.Count - 1);
                left.ImagePlacer.Children[0].Visibility = Visibility.Visible;
            }
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

                _tradeOffer.UpdateSenderTotalMoney(GetConvertedPrice(_system.MonGame.GetSenderTotalMoneyForTrde()));

            };

            _tradeOffer.ReciverMoney.AmountOfMoneyBox.TextChanged += (sender, e) =>
            {
                int reciverMoney = _tradeOffer.GetReciverTradeMoney();
                _system.MonGame.SetReciverMoneyTrade(reciverMoney);

                _tradeOffer.UpdateReciverTotalMoney(GetConvertedPrice(_system.MonGame.GetReciverTotalMoneyForTrade()));
            };
        }

        private void UpdateTradeMoneyBoxes()
        {
            _tradeOffer.UpdateSenderTotalMoney(GetConvertedPrice(_system.MonGame.GetSenderTotalMoneyForTrde()));
            _tradeOffer.UpdateReciverTotalMoney(GetConvertedPrice(_system.MonGame.GetReciverTotalMoneyForTrade()));
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
            if (!_system.MonGame.IfStepperOwnsBusiness(cellIndex) &&
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

            AddTradeItemToListBox(item, _system.MonGame.IfStepperOwnsBusiness(cellIndex));
        }

        public void AddTradeItemToListBox(TradeItem item, bool ifAddToSender)
        {
            if (ifAddToSender) _tradeOffer.SenderListBox.Items.Add(item);
            else _tradeOffer.ReciverListBox.Items.Add(item);
        }

        private Image GetImageFromBusCell(UIElement element)
        {
            if (element is UpperCell upper)
                return upper.ImagePlacer.Children.OfType<Image>().Last();

            if (element is RightCell right)
                return right.ImagePlacer.Children.OfType<Image>().Last();

            if (element is BottomCell bottom)
                return bottom.ImagePlacer.Children.OfType<Image>().Last();

            if (element is LeftCell left)
                return left.ImagePlacer.Children.OfType<Image>().Last();

            throw new Exception("No image in imagePlacer!");
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

        public void UpdateDepositCounters()
        {
            _system.MonGame.SetNewDepositCircle();
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IfDeposited(i))
                {
                    BoardHelper.SetValueForDepositCounter(_cells[i], _system.MonGame.GetDepositCounter(i));
                    if (_system.MonGame.IfBusDepositCounterIsZero(i))
                    {
                        AddWrapPanelToChatBox($"{_system.MonGame.GetBusNameOnGivenIndex(i)} goes to bank");

                        ClearCell(_cells[i], i);
                        _system.MonGame.ClearBusiness(i);
                        BoardHelper.MakeDepositCounterHidden(_cells[i]);

                        ClearInventoryBusImage(i);
                        _system.MonGame.SetBasicBusBack(i);
                    }
                }
            }
        }


        

        private Size GetSquareCellSize(GamePageSize sizeType)
        {
            return new Size(1, 1);
        }

        private Size GetTopBoyCelSize(GamePageSize sizeType)
        {
            return new Size(1, 1);
        }

        private Size GetLeftRightSize(GamePageSize sizeType)
        {
            return new Size(1, 1);
        }

        public void UpdateFieldSize(GamePageSize sizeType)
        {
            List<UIElement> squares = GetSquares(); //Prison cell is other type
            List<UIElement> topBotElems = GetTopBotCells();
            List<UIElement> leftRightElems = GetLeftRightCells();

            //Change Cell size
            ChangeSizeForSquareElement(squares, GetSquareCellSize(sizeType));
            ChangeTopBotCellSize(topBotElems, GetTopBoyCelSize(sizeType));
            ChangeLeftRightSize(leftRightElems, GetLeftRightSize(sizeType));

            //Other params 
            
        }

        private void ChangeLeftRightSize(List<UIElement> elems, Size size)
        {
            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is LeftCell left)
                {
                    left.Width = size.Width;
                    left.Height = size.Height;
                }
                else if (elems[i] is RightCell right)
                {
                    right.Width = size.Width;
                    right.Height = size.Height;
                }
            }
        }

        private void ChangeTopBotCellSize(List<UIElement> elems, Size size)
        {
            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is UpperCell upper)
                {
                    upper.Width = size.Width;
                    upper.Height = size.Height;
                }
                else if (elems[i] is BottomCell bottom)
                {
                    bottom.Width = size.Width;
                    bottom.Height = size.Height;
                }
            }
        }

        private void ChangeSizeForSquareElement(List<UIElement> elems, Size size)
        {
            for(int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is SquareCell square)
                {
                    square.Width = size.Width;
                    square.Height = size.Height;
                }
                else if (elems[i] is PrisonCell prison)
                {
                    prison.Width = size.Width;
                    prison.Height = size.Height;
                }
            }
        }


        public List<UIElement> GetLeftRightCells()
        {
            List<UIElement> res = new List<UIElement>();
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i] is LeftCell ||
                    _cells[i] is RightCell)
                {
                    res.Add(_cells[i]);
                }
            }
            return res;
        }

        public List<UIElement> GetTopBotCells()
        {
            List<UIElement> res = new List<UIElement>();
            for(int i = 0; i < _cells.Count; i++)
            {
                if (_cells[i] is UpperCell ||
                    _cells[i] is BottomCell)
                {
                    res.Add(_cells[i]);
                }
            }
            return res;
        }

        public List<UIElement> GetSquares()
        {
            return new List<UIElement>()
            {
                StartCell, 
                PrisonCell,
                JackPotCell,
                GoToPrisonCell
            };
        }
    }
}
