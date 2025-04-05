using MonopolyDLL;
using MonopolyDLL.Monopoly;
using MonopolyDLL.Monopoly.Cell.Businesses;
using MonopolyDLL.Monopoly.Enums;
using MonopolyDLL.Monopoly.TradeAction;
using MonopolyDLL.Services;
using MonopolyEntity.Interfaces;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
using MonopolyEntity.Windows.UserControls.CaseOpening;
using MonopolyEntity.Windows.UserControls.GameControls.BusinessInfo;
using MonopolyEntity.Windows.UserControls.GameControls.GameCell;
using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages;
using MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages.TradeControls;
using MonopolyEntity.Windows.UserControls.GameControls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

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
            //system.MonGame.Players[4].AmountOfMoney = 345;

            _system = system;
            _cards = cards;
            _frame = frame;

            MakeBaseMethods();
        }

        private void MakeBaseMethods()
        {
            //_system.MonGame.Players[2].AmountOfMoney = 400;

            InitializeComponent();

            SetCellsInList();

            SetImmortalImages();
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
                        if (_cards[index].UserTimer._timer is null ||
                        _cards[index].UserTimer._timer.Enabled || _gameEnded) return;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            PlayerGaveUp(_system.MonGame.StepperIndex);
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
                if (_system.MonGame.IsCellIndexIsBusiness(i))
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
                if (_system.MonGame.GameBoard.Cells[i] is Business /*&& (i == 39 || i == 37)*/)
                {
                    ((Business)_system.MonGame.GameBoard.Cells[i]).OwnerIndex = stepperIndex;
                    ((Business)_system.MonGame.GameBoard.Cells[i]).Level = 4;
                    PaintCellInColor(i, _colors[stepperIndex]);
                }
            }
            _system.MonGame.SetMonopolies();
        }

        private void SetCardForStartStepper()
        {
            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
        }

        private void ChangeStepper(bool isAfterAuction = false)
        {
            //_cards[_system.MonGame.StepperIndex].UpdateTimer();
            if (_system.MonGame.IsCubeDropsAreEqual() && !_system.MonGame.IsStepperSitsInPrison() &&
                !_system.MonGame.IsPlayerLost() && !_system.MonGame.IsStepperIsSleeping())
            {
                if (_ifBirthdayChance) _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
                _cards[_system.MonGame.StepperIndex].UserTimer.SetTimer();
                SetActionAfterStepperChanged();
                return;
            }

            _system.MonGame.ClearStepperDoublesCounter();

            //MakeEveryCardThinner();

            // return;
            //Get Back size for old Card
            if (!isAfterAuction && !_ifBirthdayChance) _cards[_system.MonGame.StepperIndex].SetAnimation(null, false);


            int prevStepperIndex = _system.MonGame.StepperIndex;

            //Change ing dll
            if (_system.MonGame.ChangeStepper())
            {
                UpdateDepositCounters();
            }

            if (prevStepperIndex == _system.MonGame.StepperIndex && _system.MonGame._ifSkipped)
            {
                _cards[prevStepperIndex]._horizontalAnimation.Completed += (sender, e) =>
                {
                    _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
                    _cards[_system.MonGame.StepperIndex].PaintBg(_colors[_system.MonGame.StepperIndex]);
                    _system.MonGame._ifSkipped = false;
                };
            }
            else _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);

            SetActionAfterStepperChanged();
        }

        public void SetChangeStepperAfterAuction()
        {
            _system.MonGame.ChangeStepper();
            _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
        }

        private void SetActionAfterStepperChanged(bool isSatInPrison = false)
        {
            BussinessInfo.Children.Clear();
            ClearDropDown();

            if (!isSatInPrison) _system.MonGame.ClearStepperBoughtHouseTypes();

            ActionAfterStepperChanged action = _system.MonGame.GetActionAfterStepperChanged();

            switch (action)
            {
                case ActionAfterStepperChanged.ThrowCubes:
                    AddThroughCubesControl(isSatInPrison);
                    break;
                case ActionAfterStepperChanged.PrisonQuestion:
                    SetPrisonQuestion();
                    break;
            }
        }

        private void SetPrisonQuestion()
        {
            if (!_system.MonGame.IsStepperSatInPrisonTooMuch())
            {
                AddWrapPanelToChatBox(SystemParamsService.GetStringByName("InPrisonQuestion"), _system.MonGame.StepperIndex);
            }
            else AddWrapPanelToChatBox(SystemParamsService.GetStringByName("InPrisonPayMoney"), _system.MonGame.StepperIndex);

            ChatMessages.Children.Clear();
            PrisonQuestion question = new PrisonQuestion(_system.MonGame.IsPlayerHasEnoughMoneyToPAyPrisonBill(),
                _system.MonGame.IsPlayerCanOnlyGiveUp(_system.MonGame.GetPrisonPrice(), _system.MonGame.StepperIndex));

            SetGiveUpActionToButton(question.GiveUpBut, null);

            SetPrisonButtonsVisibility(question);
            SetEventsForPrisonQuestion(question);

            ChatMessages.Children.Add(question);
        }

        private bool _ifPrisQuestClicked = false;
        public void SetEventsForPrisonQuestion(PrisonQuestion question)
        {
            SetPayEventInPrison(question.PayBut);
            SetPayEventInPrison(question.LastPay);

            question.ContinueSitting.Click += (sender, e) =>
            {
                if (_ifPrisQuestClicked) return;
                _ifPrisQuestClicked = true;
                DropDiceInPrison();
            };
        }

        //const int _sleepTime = 1000;
        private void DropDiceInPrison()
        {
            const int sleepTime = 500;
            (int, int) cubeValues = _system.MonGame.GetValuesForPrisonDice();
            DicesDrop drop = new DicesDrop(cubeValues.Item1, cubeValues.Item2);

            Canvas.SetZIndex(drop, 0);
            drop.VerticalAlignment = VerticalAlignment.Center;

            drop._first3dCube._horizontalAnimation.Completed += (sender, e) =>
            {
                _ifPrisQuestClicked = false;
                //Thread.Sleep(_sleepTime);

                BussinessInfo.Children.Clear();
                ClearDropDown();

                if (cubeValues.Item1 == cubeValues.Item2)
                {
                    _cards[_system.MonGame.StepperIndex].UpdateTimer();

                    AddWrapPanelToChatBox(SystemParamsService.GetStringByName("InPrisonGettingOut") +
                        SystemParamsService.GetStringByName("InPrisonDropped") +
                        $" {cubeValues.Item1} and {cubeValues.Item2}", _system.MonGame.StepperIndex);

                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperSitInPrisonCounter();

                    UpdatePlayersMoney();

                    _system.MonGame.SetCubes(cubeValues.Item1, cubeValues.Item2);

                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<PrisonQuestion>().First());

                    _system.MonGame.ClearStepperDoublesCounter();
                    //MakeAMoveAfterCubesDropped();

                    MoveChipFromSitTiExcursionInPrison();
                    _chipMoveAnimation.Completed += (send, ev) =>
                    {
                        ChatMessages.Children.Clear();
                        SetActionAfterStepperChanged(true);
                    };
                    return;
                }

                Thread.Sleep(sleepTime);

                _system.MonGame.MakeStepperPrisonCounterHigher();
                ChatMessages.Children.Clear();
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("ValuesNotEqual")} " +
                    $"{SystemParamsService.GetStringByName("YouGot")} {cubeValues.Item1} " +
                    $"{SystemParamsService.GetStringByName("And")} {cubeValues.Item2}",
                    _system.MonGame.StepperIndex);

                if (_system.MonGame.IsStepperSatInPrisonTooMuch())
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
                if (_ifPrisQuestClicked) return;

                if (_system.MonGame.IsStepperHasEnoughMoneyToPayPrisonPrice())
                {
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("PaidStr")} " +
                        $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetPrisonPrice())} " +
                        SystemParamsService.GetStringByName("ToOutOfPrison"), _system.MonGame.StepperIndex);

                    _cards[_system.MonGame.StepperIndex].UpdateTimer();

                    _system.MonGame.PayPrisonBill();
                    ChatMessages.Children.Clear();

                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperSitInPrisonCounter();

                    UpdatePlayersMoney();

                    MoveChipFromSitTiExcursionInPrison();
                    _chipMoveAnimation.Completed += (send, ev) =>
                    {
                        SetActionAfterStepperChanged(true);
                    };
                    return;
                }
            };
        }

        private void MoveChipFromSitTiExcursionInPrison()
        {
            Image img = _images[_system.MonGame.StepperIndex];

            PrisonCell pris = (PrisonCell)_cells[_system.MonGame.GetPrisonIndex()];
            pris.ChipsPlacerSitters.Children.Remove(img);
            pris.ChipsPlacerVisit.Children.Add(img);

            UpdatePrisonCanvases(_system.MonGame.GetPrisonIndex());
        }

        private void SetPrisonButtonsVisibility(PrisonQuestion question)
        {
            if (!_system.MonGame.IsStepperSatInPrisonTooMuch()) //If Player sat NOT too much
            {
                question.SetButtonsVisibility(false);
                //question.SetEnoughMoneyButsVisibility();
                return;
            }
            question.SetButtonsVisibility(true);

            //Player sat too many rounds in prison + set there if giveUp later
            //question.SetNotEnoughMoneyButsVisibility();
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

        private void AddThroughCubesControl(bool isSatInPrison = false)
        {
            if (!isSatInPrison) _system.MonGame.ClearStepperBoughtHouseTypes();

            /*            if (_system.MonGame.IfStepperIsSleeping())
                        {
                            ChangeStepper();
                            //_system.MonGame.SetSkipMoveOppositeForStepper();
                            return;
                        }*/

            ThroughCubes cubes = new ThroughCubes();
            cubes.VerticalAlignment = VerticalAlignment.Top;
            cubes.ThroughCubesBut.Click += ThrowCubes_Click;
            ChatMessages.Children.Add(cubes);
        }

        private void ThrowCubes_Click(object sender, RoutedEventArgs e)
        {
            ChatMessages.Children.Clear();
            _cards[_system.MonGame.StepperIndex].StopTimer();
            AddThrowingDice();
        }

        private void AddThrowingDice()
        {
            _system.MonGame.DropCubes();
            DicesDrop drop = new DicesDrop(_system.MonGame.GetFirstCube(), _system.MonGame.GetSecondCube());
            drop.VerticalAlignment = VerticalAlignment.Center;

            drop._first3dCube._horizontalAnimation.Completed += CubesDropped_Completed;

            ChatMessages.Children.Add(drop);
        }

        private EventHandler _checkEvent;
        private bool _toDropCubes = false;
        private bool _goToPrisonByDouble = false;

        private void CubesDropped_Completed(object sender, EventArgs e)
        {
            MakeAMoveAfterCubesDropped();
        }

        public void MakeAMoveAfterCubesDropped()
        {
            if (!_system.MonGame.GetIsTp())
            {
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("InPrisonDropped")}" +
                    $" {_system.MonGame.GetFirstCube()} {SystemParamsService.GetStringByName("And")}" +
                    $" {_system.MonGame.GetSecondCube()}", _system.MonGame.StepperIndex);
            }
            _system.MonGame.SetIsTp(false);

            int cellIndexToMoveOn = _system.MonGame.GetPointToMoveOn();
            _goToPrisonByDouble = false;

            if (_system.MonGame.IsCubeDropsAreEqual())
            {
                _cards[_system.MonGame.StepperIndex].UpdateTimer();
                _system.MonGame.AddToDoubleCounter();

                if (_system.MonGame.IsMaxDoublesIsAchieved())
                {
                    //Go To Prison
                    cellIndexToMoveOn = _system.MonGame.GetPrisonIndex();
                    _system.MonGame.ReverseStepperSitInPrison();
                    _system.MonGame.ClearStepperDoublesCounter();
                    _goToPrisonByDouble = true;

                    AddWrapPanelToChatBox(SystemParamsService.GetStringByName("ThreeSameDrops"), _system.MonGame.StepperIndex);
                    //return;
                }
            }

            if (cellIndexToMoveOn == _system.MonGame.GetPrisonIndex()) IsLastMoveIsPrison = true;
            _toDropCubes = true;

            //Need to understand which cell is Cell On
            //Get enum action to show what is happening
            _checkEvent += (compSender, eve) =>
            {
                int tempPost = _system.MonGame.Players[_system.MonGame.StepperIndex].Position;

                _cards[_system.MonGame.StepperIndex].SetVisibleToTimer();
                BussinessInfo.Children.Clear();
                ClearDropDown();

                ActionsAfterMoveOnBoard();
                // _system.MonGame.SetIsTp(false);
            };

            int tempPos = _system.MonGame.GetStepperPosition();

            MakeChipsMovementAction(tempPos,
                cellIndexToMoveOn, _images[_system.MonGame.StepperIndex]);

            //Change temp point 
            _system.MonGame.SetPlayerPosition(_goToPrisonByDouble);
        }

        public void ActionsAfterMoveOnBoard()
        {
            if (!_toDropCubes) return;
            _toDropCubes = false;

            //Check if stepper went through start(to get money)
            if (!_system.MonGame.IsNeedToMoveBackwards()) GoThroughStartCellCheck();

            _system.MonGame.SetOppositeMoveBackwards();

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
            if (_system.MonGame.IsStepperWentThroughStartCell())
            {
                int wentThroughStartMoney = _system.MonGame.GetGoThroughStartCellMoney();
                _system.MonGame.GetMoneyByStepper(wentThroughStartMoney);

                UpdatePlayersMoney();
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("ThroughStart")} " +
                    $"{SystemParamsService.GetStringByName("GetSmth")} " +
                    $"{MoneyConvertingService.GetConvertedPrice(wentThroughStartMoney)}", _system.MonGame.StepperIndex);
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
                case CellAction.GotOnEnemiesBusiness:
                    SetGotOnEnemiesBusiness();
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
                    SetGotOnChance();
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
        }

        private bool _ifWithoutGoingThrough = false;

        public void SetGotOnGotToPrison()
        {
            AddWrapPanelToChatBox(SystemParamsService.GetStringByName("GoesToPrison"), _system.MonGame.StepperIndex);
            int stepperPosition = _system.MonGame.GetStepperPosition();
            int prisonCellIndex = _system.MonGame.GetPrisonIndex();
            _ifWithoutGoingThrough = true;
            IsLastMoveIsPrison = true;
            _system.MonGame.Players[_system.MonGame.StepperIndex].IsSitInPrison = true;
            MakeChipsMovementAction(stepperPosition, prisonCellIndex, _images[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(prisonCellIndex);
            _system.MonGame.ClearStepperDoublesCounter();

            _chipMoveAnimation.Completed += (sender, e) =>
            {
                ChangeStepper();
            };
        }

        private void SetVisitPrison()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            AddWrapPanelToChatBox(SystemParamsService.GetStringByName("PrisonVisit"), _system.MonGame.StepperIndex);
            ChangeStepper();
        }

        private void GotOnStartAction()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            int money = _system.MonGame.GetGotOnStartCellMoney();
            _system.MonGame.GetMoneyByStepper(money);

            UpdatePlayersMoney();

            AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("GotOnStart")}" +
                $" {SystemParamsService.GetStringByName("GetSmth")}{MoneyConvertingService.GetConvertedPrice(money)}", _system.MonGame.StepperIndex);
            ChangeStepper();
        }

        private void SetGotOnChance()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            AddWrapPanelToChatBox(SystemParamsService.GetStringByName("GotOnChance"), _system.MonGame.StepperIndex);

            ChanceAction action = /*_system.MonGame.StepperIndex == 0 ? ChanceAction.SkipMove : ChanceAction.Get500;// */_system.MonGame.GetChanceAction();

           // action = ChanceAction.Tax;

            string actionText = GetTextForChanceAction(action);
            AddWrapPanelToChatBox(actionText, _system.MonGame.StepperIndex);

            MakeChanceAction(action);

            UpdatePlayersMoney();
        }

        private void MakeChanceAction(ChanceAction action)
        {
            switch (action)
            {
                case ChanceAction.Pay500:
                    PayMoneyChanceAction(_system.MonGame.GameBoard.GetLittleLoseMoneyChance());
                    break;
                case ChanceAction.Pay1500:
                    PayMoneyChanceAction(_system.MonGame.GameBoard.GetBigLoseMoneyChance());
                    break;
                case ChanceAction.Get500:
                    GetMoneyChanceAction(_system.MonGame.GameBoard.GetLittleWinMoneyChance());
                    break;
                case ChanceAction.Get1500:
                    GetMoneyChanceAction(_system.MonGame.GameBoard.GetBigWinMoneyChance());
                    break;
                case ChanceAction.GoToPrison:
                    GoToPrisonFromChance();
                    break;
                case ChanceAction.MoveBackwards:
                    MoveBackWards();
                    break;
                case ChanceAction.BirthDay:
                    BirthdayChance();
                    break;
                case ChanceAction.Tax:
                    MakeTaxPay();
                    break;
                case ChanceAction.TP:
                    MakeTPChanceAction();
                    break;
                case ChanceAction.SkipMove:
                    SkipMoveChance();
                    break;
            }
        }

        public void SkipMoveChance()
        {
            _system.MonGame.SetSkipMoveOppositeForStepper();
            ChangeStepper();
        }

        public void MakeTPChanceAction()
        {
            //Thread.Sleep(350);

            _system.MonGame.SetValuesToGoOnFromChanceTp();
            MakeAMoveAfterCubesDropped();
        }

        private void MakeTaxPay()
        {
            int price = _system.MonGame.GetTaxMoneyForBuildings(_system.MonGame.StepperIndex);
            if (price == 0)
            {
                AddWrapPanelToChatBox("have no houses or hotels", _system.MonGame.StepperIndex);
                ChangeStepper();
                return;
            };
            SetPayMoney($"{SystemParamsService.GetStringByName("BuildingsTax")}",
                $"{SystemParamsService.GetStringByName("BuildingsTaxToPay")} {price}", price, _system.MonGame.StepperIndex);
        }

        public int _toPayMoneyBirthdayIndex = -1;
        private bool _ifPaidBirthdayMoney = false;
        private List<int> _playersIndexesThatPaid = new List<int>();
        public bool _ifBirthdayChance = false;
        private void BirthdayChance()
        {

            List<int> indexesToPayOnBirthday = _system.MonGame.GetBirthdayToPayPlayersIndexes(_playersIndexesThatPaid);

            for (int i = 0; i < indexesToPayOnBirthday.Count; i++)
            {
                int moneyToPay = _system.MonGame.GetBirthdayPayment(indexesToPayOnBirthday[i]);
                _system.MonGame.PayBirthdayMoneyByPlayer(indexesToPayOnBirthday[i], moneyToPay);
                _system.MonGame.GetMoneyByStepper(moneyToPay);

                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("PaidStr") + moneyToPay}", indexesToPayOnBirthday[i]);
            }

            _playersIndexesThatPaid.Clear();
            ClearBirthdayValues();
            _ifBirthdayChance = false;

            UpdatePlayersMoney();
            ChangeStepper();


            return;


            _ifBirthdayChance = true;



            MakeEveryCardThinner();

            _cards[_toAnimateCardIndex]._horizontalAnimation.Completed += (send, ev) =>
            {
                _cards[_system.MonGame.StepperIndex].StopTimer();
                SetPlayerBirthdayPayment();
            };
        }

        public void SetPlayerBirthdayPayment()
        {


            List<int> indexesToPayOnBirthday = _system.MonGame.GetBirthdayToPayPlayersIndexes(_playersIndexesThatPaid);
            if (indexesToPayOnBirthday.Count == 0)
            {
                ClearBirthdayValues();
                _playersIndexesThatPaid.Clear();
                ChangeStepper();
                _ifBirthdayChance = false;
                return;
            }

            _toPayMoneyBirthdayIndex = indexesToPayOnBirthday.First();


            MarkPlayerCardIfItsNotMarked(_toPayMoneyBirthdayIndex);
            //_cards[_toPayMoneyBirthdayIndex].SetAnimation(_colors[_toPayMoneyBirthdayIndex], true);

            _cards[_toPayMoneyBirthdayIndex]._horizontalAnimation.Completed += (send, ev) =>
            {
                int moneyToPay = _system.MonGame.GetBirthdayPayment(_toPayMoneyBirthdayIndex);// SystemParamsService.GetNumByName("MoneyToPayOnBirthday");

                SetPayMoney(SystemParamsService.GetStringByName("BirthdayGift"),
                    $"{SystemParamsService.GetStringByName("GiftIs")} {moneyToPay}", moneyToPay, _toPayMoneyBirthdayIndex);

                PayMoney money = ChatMessages.Children.OfType<PayMoney>().First();

                money.PayBillBut.Click += (sender, e) =>
                {
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("PaidStr") + moneyToPay}", _toPayMoneyBirthdayIndex);
                    PaidBirthdayGift(_system.MonGame.GetBirthdayToPayPlayersIndexes(_playersIndexesThatPaid));
                };

                money.GiveUpBut.Click += (sender, e) =>
                {
                    PlayerGaveUp(_toPayMoneyBirthdayIndex);
                    SetNextGifterOnBirthday(indexesToPayOnBirthday);
                };
            };
        }

        private void PaidBirthdayGift(List<int> indexesToPayOnBirthday)
        {
            if (!_ifPaidBirthdayMoney) return;
            SetNextGifterOnBirthday(indexesToPayOnBirthday);
        }

        public void SetNextGifterOnBirthday(List<int> indexesToPayOnBirthday)
        {
            _playersIndexesThatPaid.Add(_toPayMoneyBirthdayIndex);
            indexesToPayOnBirthday.Remove(_toPayMoneyBirthdayIndex);

            ClearBirthdayValues();
            if (indexesToPayOnBirthday.Count == 0)
            {
                MakeEveryCardThinner();

                _playersIndexesThatPaid.Clear();
                _cards[_toAnimateCardIndex]._horizontalAnimation.Completed += (sender, e) =>
                {
                    ChangeStepper();
                    _ifBirthdayChance = false;
                };
                return;
            }
            BirthdayChance();
        }

        public void MarkPlayerCardIfItsNotMarked(int index)
        {
            if (IsColorsAreEqual(index))
            {
                _cards[_toPayMoneyBirthdayIndex].SetAnimation(_colors[_toPayMoneyBirthdayIndex], true);
            }
        }

        public void ClearBirthdayValues()
        {
            ChatMessages.Children.Clear();
            _toPayMoneyBirthdayIndex = -1;
            _ifPaidBirthdayMoney = false;
        }

        private void MoveBackWards()
        {
            _system.MonGame.SetOppositeMoveBackwardsVal();
            //_system.MonGame.SetOppositeMoveBackwards();
            ChangeStepper();
        }

        private void GoToPrisonFromChance()
        {
            int stepperPosition = _system.MonGame.GetStepperPosition();
            int prisonCellIndex = _system.MonGame.GetPrisonIndex();
            _ifWithoutGoingThrough = true;
            IsLastMoveIsPrison = true;
            _system.MonGame.Players[_system.MonGame.StepperIndex].IsSitInPrison = true;
            MakeChipsMovementAction(stepperPosition, prisonCellIndex, _images[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(prisonCellIndex);
            _system.MonGame.ClearStepperDoublesCounter();
            ChangeStepper();
        }

        private void GetMoneyChanceAction(int money)
        {
            _system.MonGame.GetMoneyFromChance(money);
            ChangeStepper();
        }

        private void PayMoneyChanceAction(int money)
        {
            SetPayMoney(SystemParamsService.GetStringByName("GotOnChance"),
                $"{SystemParamsService.GetStringByName("NeedToPat")} {money}", money, _system.MonGame.StepperIndex);
        }

        public void SetPayMoney(string firstLine, string secondLine, int money, int payerIndex, bool isEnemiesBus = false)
        {
            PayMoney bill = new PayMoney(_system.MonGame.IsPlayerHasEnoughMoneyToPayBill(money, payerIndex),
                _system.MonGame.IsPlayerCanOnlyGiveUp(money, payerIndex));

            SetGiveUpActionToButton(bill.GiveUpBut, null);

            bill.GotOnBusText.Text = $"{firstLine}";
            bill.PayBillText.Text = $"{secondLine}";

            bill.PayBillBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IsPlayerHasEnoughMoneyToPay(money, payerIndex))
                {
                    if (_toPayMoneyBirthdayIndex != -1)
                    {
                        _ifPaidBirthdayMoney = true;
                        _system.MonGame.PayBirthdayMoneyByPlayer(_toPayMoneyBirthdayIndex, money);
                        _system.MonGame.GetMoneyByStepper(money);
                        UpdatePlayersMoney();
                        return;
                    }


                    if (isEnemiesBus)
                    {
                        _system.MonGame.PayBusBillByStepper();
                    }
                    else { _system.MonGame.PayBillByStepper(money); }
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("PaidStr")}" +
                        $"{MoneyConvertingService.GetConvertedPrice(money)}", _system.MonGame.StepperIndex);
                    UpdatePlayersMoney();
                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<PayMoney>().First());

                    ChangeStepper(); //Next stepper after paid money
                }
            };
            ChatMessages.Children.Add(bill);
        }

        public string GetTextForChanceAction(ChanceAction action)
        {
            switch (action)
            {
                case ChanceAction.Pay500:
                    return $"{SystemParamsService.GetStringByName("ChangeLost")} " +
                        $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GameBoard.GetLittleLoseMoneyChance())} " +
                        $"{SystemParamsService.GetStringByName("ChanceLittleLost")}";
                case ChanceAction.Pay1500:
                    return $"{SystemParamsService.GetStringByName("NeedToPat")}" +
                        $" {MoneyConvertingService.GetConvertedPrice(_system.MonGame.GameBoard.GetBigLoseMoneyChance())} " +
                        $"{SystemParamsService.GetStringByName("ChanceBigLost")}";
                case ChanceAction.Get500:
                    return $"{SystemParamsService.GetStringByName("ChanceMoneyGot")}" +
                        $" {MoneyConvertingService.GetConvertedPrice(_system.MonGame.GameBoard.GetLittleWinMoneyChance())} " +
                        $"{SystemParamsService.GetStringByName("ChanceLittleMoneyGain")}";
                case ChanceAction.Get1500:
                    return $"{SystemParamsService.GetStringByName("ChanceMoneyGot")}" +
                        $" {MoneyConvertingService.GetConvertedPrice(_system.MonGame.GameBoard.GetBigWinMoneyChance())} " +
                        $"{SystemParamsService.GetStringByName("ChanceBigMoneyGain")}";
                case ChanceAction.GoToPrison:
                    return SystemParamsService.GetStringByName("GoesToPrison");
                case ChanceAction.MoveBackwards:
                    return SystemParamsService.GetStringByName("ChanceMoveBackwards");
                case ChanceAction.BirthDay:
                    return SystemParamsService.GetStringByName("BirthdayChance");
                case ChanceAction.Tax:
                    {
                        int silverAmount = _system.MonGame.GetStepperAmountOfSilverStars();
                        int goldenAmount = _system.MonGame.GetSteppersAmountOfGoldenStars();
                        int total = _system.MonGame.GetTaxMoneyForBuildings(_system.MonGame.StepperIndex);

                        return $"{SystemParamsService.GetStringByName("BuildingsTaxChance")} silver stars - {silverAmount}\n golden amount - {goldenAmount} Total - {total}";
                    }
                case ChanceAction.TP:
                    return SystemParamsService.GetStringByName("TeleportChance");
                case ChanceAction.SkipMove:
                    return SystemParamsService.GetStringByName("SkipMoveChance");
            };

            throw new Exception("What can you get else");
        }

        private void SetGetOnCasino()
        {
            const int sleepTime = 1000;
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            AddWrapPanelToChatBox(SystemParamsService.GetStringByName("GotOnCasino"), _system.MonGame.StepperIndex);

            JackpotElem jackpot = new JackpotElem(_system.MonGame.IsPlayerHasEnoughMoneyToPlayCasino());

            jackpot.MakeBidBut.Click += (sender, e) =>
            {
                if (!jackpot.DeclineBut.IsEnabled) return;
                if (_system.MonGame.IsPlayerHasEnoughMoneyToPlayCasino() &&
                jackpot._chosenRibs.Count > 0)
                {
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("CasinoPlaceMoney")} " +
                        $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetCasinoPrice())} " +
                        $"{SystemParamsService.GetStringByName("CasinoAndMakeBid")} " +
                        $"{GetChosenRibsForCasinoInString(jackpot)}", _system.MonGame.StepperIndex);

                    //Set Cube drop
                    _system.MonGame.GetBillForCasino();
                    UpdatePlayersMoney();

                    string winString = _system.MonGame.PlayCasino(jackpot._chosenRibs);

                    AddCubeToCasinoAction(_system.MonGame.GetCasinoWinValue());
                    jackpot.DeclineBut.IsEnabled = false;

                    _casinoDice._horizontalAnimation.Completed += (send, ev) =>
                    {
                        Thread.Sleep(sleepTime);
                        jackpot.MakeBidBut.IsEnabled = true;

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
                AddWrapPanelToChatBox(SystemParamsService.GetStringByName("CasinoNotToPlay"), _system.MonGame.StepperIndex);

                ChatMessages.Children.Remove(ChatMessages.Children.
                    OfType<JackpotElem>().First());

                ChatMessages.Children.Clear();
                ChangeStepper();
            };

            ChatMessages.Children.Add(jackpot);
        }

        private string GetChosenRibsForCasinoInString(JackpotElem elem)
        {
            string res = string.Empty;
            const string divider = " ";
            for (int i = 0; i < elem._chosenRibs.Count; i++)
            {
                res += (elem._chosenRibs[i].ToString() + divider);
            }
            return res;
        }

        private Dice _casinoDice;
        public void AddCubeToCasinoAction(int res)
        {
            const int cubeSize = 175;
            const int botMargin = 50;
            _casinoDice = new Dice(res, true)
            {
                Width = cubeSize,
                Height = cubeSize,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 0, botMargin)
            };

            ChatMessages.Children.Add(_casinoDice);
        }

        private void SetGotOnTax()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();

            string firstLine = SystemParamsService.GetStringByName("GotOnTax") +
                $"{_system.MonGame.GetTempPositionCellName()}";

            string secondLine = SystemParamsService.GetStringByName("TaxNeedToPay") +
                $" {MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetBillFromTaxStepperPosition())}";

            int money = _system.MonGame.GetBillFromTaxStepperPosition();

            SetPayMoney(firstLine, secondLine, money, _system.MonGame.StepperIndex);
            AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("TaxMessagePayMoney")}" +
                $"{MoneyConvertingService.GetConvertedPrice(money)}", _system.MonGame.StepperIndex);
        }

        private void SetGotOnEnemiesBusiness()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();

            string firstLine = SystemParamsService.GetStringByName("GotOnEnemiesBus") +
                $"{_system.MonGame.GetTempPositionCellName()}";

            string secondLine = SystemParamsService.GetStringByName("PayOnEnemiesBus") +
                $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetBillForBusinessCell())}";

            AddMessageWithTwoPlayers(_system.MonGame.StepperIndex, _system.MonGame.GetStepperPositionOwnerBus(),
                $"{SystemParamsService.GetStringByName("GetsOn")} ",
                $"{SystemParamsService.GetStringByName("Bus")} {secondLine}");

            int amountOfMoney = _system.MonGame.GetBillForBusinessCell();
            if (amountOfMoney == 0)
            {
                AddWrapPanelToChatBox(SystemParamsService.GetStringByName("GotOnDepositedBus"), _system.MonGame.StepperIndex);
                ChangeStepper();
                return;
            }

            SetPayMoney(firstLine, secondLine, amountOfMoney, _system.MonGame.StepperIndex, true);
        }

        public void UpdatePlayersMoney()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (i > _system.MonGame.Players.Count - 1) return;
                _cards[i].UserMoney.Text = MoneyConvertingService.GetConvertedStringWithoutLastK(_system.MonGame.GetPlayersMoney(i));
            }
        }

        private void PlayerGotOnOwnBusiness()
        {
            AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("GotOnOwnBus")} " +
                $"({_system.MonGame.GetBusOnStepperName()})", _system.MonGame.StepperIndex);
            ChangeStepper();
        }

        private void SetBuyBusinessOffer()
        {
            _cards[_system.MonGame.StepperIndex].UpdateTimer();

            BuyBusiness buy = new BuyBusiness(_system.MonGame.IsStepperHasEnoughMoneyToBuyBus());
            buy.YourTurnText.Text = $"{SystemParamsService.GetStringByName("GotOn")}" +
                $"{_system.MonGame.GetBusOnStepperName()}";
            AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("GotOn")}" +
                $"{_system.MonGame.GetBusOnStepperName()}", _system.MonGame.StepperIndex);
            ChatMessages.Children.Add(buy);

            string busName = _system.MonGame.GetBusOnStepperName();
            buy.BuyBusBut.Click += (sender, e) =>
            {
                //return;
                if (_system.MonGame.IsStepperHasEnoughMoneyToBuyBus())
                {
                    SetSteppersBusFromInventory(_system.MonGame.StepperIndex);

                    SetBuyingBusiness();
                    ChatMessages.Children.Clear();

                    SetLevelPayment(_system.MonGame.StepperIndex, _system.MonGame.GetStepperPosition());

                    string boughtBusName = _system.MonGame.GetBusOnStepperName();

                    if (boughtBusName != busName)
                    {
                        GameChat.Items.Add(GetTextBlockForMessage($"{busName} {SystemParamsService.GetStringByName("BusTurns")} {boughtBusName}"));
                    }

                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("Bought")} {_system.MonGame.GetBusOnStepperName()} for " +
                        $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetSteppersBusPrice())}"
                        , _system.MonGame.StepperIndex);
                    ChangeStepper();
                }
            };

            buy.SendToAuctionBut.Click += (sender, e) =>
            {
                ClearDropDown();
                _ifBlockMenus = true;
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("NotBuyBus")}{_system.MonGame.GetBusOnStepperName()} business", _system.MonGame.StepperIndex);

                //!!Make auction
                //auction action
                ChatMessages.Children.Clear();
                //ChangeStepper();
                SetAuctionOffer();
            };
        }

        public bool _ifBlockMenus = false;

        public void SetLevelPayment(int playerIndex, int position)
        {
            _clickedCellIndex = position;
            if (_system.MonGame.IsCellIsCarBus(position))
            {
                _system.MonGame.SetCarsPaymentLevels(playerIndex);
                SetCarsPayments(playerIndex);
            }
            else if (_system.MonGame.IsCellIsGame(position))
            {
                _system.MonGame.SetGamePaymentLevels(playerIndex);
                SetGamePayments(playerIndex);
            }
        }

        public void UpdateChatMessages()
        {
            const int firstElemIndex = 0;
            if (ChatMessages.Children.Count == 0) return;

            if (ChatMessages.Children[firstElemIndex] is BuyBusiness bus)
            {
                bus.SetBuyButton(_system.MonGame.IsStepperHasEnoughMoneyToBuyBus());
            }
            else if (ChatMessages.Children[firstElemIndex] is JackpotElem jackpot)
            {
                jackpot.SetPlayButton(_system.MonGame.IsPlayerHasEnoughMoneyToPlayCasino());
            }
            else if (ChatMessages.Children[firstElemIndex] is PayMoney payMoney) // pay bus bill
            {
                bool isPlayerHasEnoughMoney = _system.MonGame.IsPlayerHasEnoughMoneyToPayMoney();
                bool isPlayerNeedToGiveUp = _system.MonGame.IsPlayerCanOnlyGiveUp(_system.MonGame.GetMoneyToPayForStepper(), _system.MonGame.StepperIndex);

                payMoney.SetPayButton(isPlayerHasEnoughMoney, isPlayerNeedToGiveUp);
            }
            else if (ChatMessages.Children[firstElemIndex] is PrisonQuestion prisQuest)
            {
                bool isEnoughForPrisonPay = _system.MonGame.IsPlayerHasEnoughMoneyToPAyPrisonBill();
                bool isPlayerNeedToGiveUp = _system.MonGame.IsPlayerCanOnlyGiveUp(_system.MonGame.GetPrisonPrice(), _system.MonGame.StepperIndex);
                prisQuest.SetPayButton(isEnoughForPrisonPay, isPlayerNeedToGiveUp);
            }
        }

        private void SetSteppersBusFromInventory(int playerIndex)
        {
            if (!_system.MonGame.IsStepperHasInventoryBusOnPosition()) return;
            _system.MonGame.SetPlayerSteppersBus();

            Business bus = _system.MonGame.GetBusThatStepperIsOn();

            SetImageImg(bus, playerIndex);
        }

        private void SetImageImg(Business bus, int playerIndex)
        {
            MonopolyDLL.Monopoly.InventoryObjs.BoxItem item = _system.MonGame.GetUserInventoryItem(playerIndex, bus.GetId());
            if (item is null) return;
            UIElement cell = _cells[bus.GetId()];

            Image img = Helper.GetAddedItemImage(item.ImagePath, item.Type);

            Grid grid = null;/* cell is UpperCell upper ? upper.ImagePlacer :
                cell is RightCell right ? right.ImagePlacer :
                cell is BottomCell bottom ? bottom.ImagePlacer :
                cell is LeftCell left ? left.ImagePlacer : null;
*/

            if (cell is IRegularCellsActions inter) grid = inter.GetImagePlacer();

            /*            if (cell is UpperCell upper)
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
                        }*/

            if (grid is null) return;
            grid.Children.Add(img);
            MakeVisibleNewBusImage(grid);
        }

        private void MakeVisibleNewBusImage(Grid imagePlacer)
        {
            const int firstElementIndex = 0;
            const int secondElementIndex = 1;
            const int maxChips = 2;

            //2 - usual bus pic and inventory bus pic
            if (imagePlacer is null || imagePlacer.Children.Count != maxChips) return;

            imagePlacer.Children[firstElementIndex].Visibility = Visibility.Hidden;
            imagePlacer.Children[secondElementIndex].Visibility = Visibility.Visible;
        }

        private void SetAuctionOffer()
        {
            _system.MonGame.SetStartAuctionPrice();
            _system.MonGame.SetStartPlayersForAuction();

            List<int> playersIndexesAuction = _system.MonGame._playerIndexesForAuction;

            if (playersIndexesAuction.Count == 0)
            {
                _ifBlockMenus = false;

                GameChat.Items.Add(GetTextBlockForMessage(SystemParamsService.GetStringByName("NoOneHasMoneyBids")));
                ChangeStepper();
                return;
            }

            if (IsAuctionIsEnded()) return;

            SwipeUsersAnimation(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
            AddBidControl();
        }

        public bool IsAuctionIsEnded()
        {
            const int clearValue = -1;
            if (IsNoOneTakesPartInAuction())
            {
                GameChat.Items.Add(GetTextBlockForMessage(SystemParamsService.GetStringByName("NoOneWantsBus")));
                MakeEveryCardThinner();

                _cards[_toAnimateCardIndex]._horizontalAnimation.Completed += (sender, e) =>
                {
                    ChangeStepper(true);
                    _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);
                    //ChangeStepAfterAuction();
                    _toAnimateCardIndex = clearValue;
                };
                _ifBlockMenus = false;
                return true;
            }
            if (_system.MonGame.IsSomeOneWonAuction())
            {
                int startRent = _system.MonGame.GetStartPriceOfBoughtBusinessByStepper();
                ChangePriceInBusiness(_system.MonGame.GetStepperPosition(), startRent);

                PaintCellInColor(_system.MonGame.GetStepperPosition(), _colors[_system.MonGame._bidderIndex]);
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("PlayerBuying")} " +
                    $"{_system.MonGame.GetBusOnStepperName()} " +
                    $"{SystemParamsService.GetStringByName("BoughtFor")} " +
                    $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetAuctionPrice())}", _system.MonGame.GetBidderIndex());

                SetLevelPayment(_system.MonGame._bidderIndex, _system.MonGame.GetStepperPosition());

                //Get money form winner 
                //_system.MonGame.GetMoneyFromAuctionWinner();
                UpdatePlayersMoney();
                //Clear visuals after auction 
                _system.MonGame.ClearAuctionValues();

                MakeEveryCardThinner();

                _cards[_toAnimateCardIndex]._horizontalAnimation.Completed += (sender, e) =>
                {
                    ChangeStepper(true);
                    _cards[_system.MonGame.StepperIndex].SetAnimation(_colors[_system.MonGame.StepperIndex], true);

                    //ChangeStepAfterAuction();
                    _toAnimateCardIndex = clearValue;
                };
                _ifBlockMenus = false;
                return true;
            }
            return false;
        }

        private int _toAnimateCardIndex = -1;
        private void MakeEveryCardThinner()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (!IsColorsAreEqual(i) /*&& !_system.MonGame.IfPlayerLost(i)*/)
                {
                    _cards[i].SetAnimation(null, false);
                    _toAnimateCardIndex = i;
                }
            }
        }

        private bool IsColorsAreEqual(int index)
        {
            SolidColorBrush backgroundBrush = _cards[index].UserCardGrid.Background as SolidColorBrush;
            SolidColorBrush usualBrush = _cards[index]._usualBrush as SolidColorBrush;

            return (backgroundBrush != null && usualBrush != null && backgroundBrush.Color == usualBrush.Color);
        }

        private bool AddBidControl()
        {
            if (IsAuctionIsEnded()) return true;

            AuctionBid bid = new AuctionBid();
            bid.BidForText.Text = $"{SystemParamsService.GetStringByName("AuctionFor")}" +
                $" {_system.MonGame.GetTempPositionCellName()}";
            bid.GoodLuckText.Text = $"{SystemParamsService.GetStringByName("TempPriceAuction")} " +
                $"{_system.MonGame.GetTempPriceInAuction()}"; ;

            bid.MakeBidBut.Click += (sender, e) =>
            {
                if (_system.MonGame.IsBidderHasEnoughMoneyToPlaceBid())
                {
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("MadeBid")} " +
                        $"{MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetTempPriceInAuction())}", _system.MonGame.GetBidderIndex());

                    _system.MonGame.MakeBidInAuction();
                    ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                    if (IsAuctionIsEnded())
                    {
                        return;
                    }
                    _system.MonGame.SetNextBidder();

                    SwipeUsersAnimation(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
                    _cards[_system.MonGame._bidderIndex]._horizontalAnimation.Completed += (send, ev) =>
                    {
                        if (AddBidControl())
                        {
                            return;
                        }
                    };
                }
            };

            bid.NotMakeABidBut.Click += (sender, e) =>
            {
                AddWrapPanelToChatBox(SystemParamsService.GetStringByName("NotBuyBus"), _system.MonGame.GetBidderIndex());

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                if (IsAuctionIsEnded()) return;
                if (_system.MonGame.RemoveAuctionBidderIfItsWasLast())
                {
                    if (IsAuctionIsEnded()) return;
                }

                SwipeUsersAnimation(_system.MonGame._prevBidderIndex, _system.MonGame._bidderIndex);
                _cards[_system.MonGame._bidderIndex]._horizontalAnimation.Completed += (send, ev) =>
                {
                    if (AddBidControl()) return;
                };
            };

            ChatMessages.Children.Add(bid);
            return false;
        }

        private void SwipeUsersAnimation(int makeUsualCardIndex, int toMarkCardIndex)
        {
            //Get Back size for old Card
            _cards[makeUsualCardIndex].SetAnimation(null, false);

            //Set Size for new Card
            _cards[toMarkCardIndex].SetAnimation(_colors[toMarkCardIndex], true);
        }

        private bool IsNoOneTakesPartInAuction()
        {
            if (!_system.MonGame.IsSomeoneIsLeftInAuction())
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
                MoneyConvertingService.GetConvertedStringWithoutLastK(_system.MonGame.GetSteppersMoney());

            _system.MonGame.SetStartLevelOfBusinessForStepper();
            int startRent = _system.MonGame.GetStartPriceOfBoughtBusinessByStepper();
            ChangePriceInBusiness(position, startRent);
        }

        public void ChangePriceInBusiness(int cellIndex, int price, bool isTaken = false)
        {
            string newPrice = MoneyConvertingService.GetConvertedPrice(price);

            if (!isTaken) newPrice = CorrectGameLastSign(newPrice, cellIndex);

            TextBlock block = null; /* _cells[cellIndex] is UpperCell upper ? upper.Money :
                _cells[cellIndex] is RightCell right ? right.Money :
                _cells[cellIndex] is BottomCell bottom ? bottom.Money :
                _cells[cellIndex] is LeftCell left ? left.Money : null;*/

            if (_cells[cellIndex] is IRegularCellsActions inter) block = inter.GetMoneyTextBlock();

            if (block is null) return;
            block.Text = newPrice;
            /*
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
                        }*/
        }

        private string CorrectGameLastSign(string price, int cellIndex)
        {
            if (!_system.MonGame.IsCellIsGame(cellIndex)) return price;

            const string gameMultSign = "x";
            price = price.Remove(price.Length - 1);
            price += gameMultSign;

            return price;
        }

        private void PaintCellInColor(int cellIndex, SolidColorBrush brush)
        {
            if (_cells[cellIndex] is IRegularCellsActions inter)
            {
                inter.SetImagePlacerBg(brush);
            }

            /*            if (_cells[cellIndex] is UpperCell upper)
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
                        }*/
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
                prices.Add(MoneyConvertingService.GetConvertedPrice(_system.MonGame.GameBoard.GetBusinessPrice(i)));
            }

            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IsCellIndexIsBusiness(i))
                {
                    SetPriceInCell(prices[i], _cells[i]);
                }
            }
        }

        /*        public string GetConvertedPrice(int price)
                {
                    const int priceDivider = 3;
                    StringBuilder build = new StringBuilder();

                    for (int i = 0; i < price.ToString().Length; i++)
                    {
                        build.Append(price.ToString()[i]);
                    }

                    for (int i = price.ToString().Length; i >= 0; i--)
                    {
                        if (i % priceDivider == 0 && i != 0 && i != price.ToString().Length)
                        {
                            build.Insert(price.ToString().Length - i, ",");
                        }
                    }

                    build.Append("k");
                    return build.ToString();
                }*/

        private void SetPriceInCell(string price, UIElement cell)
        {
            if (cell.Visibility != Visibility.Visible) return;

            const int rotateAngle = 90;
            if (cell is UpperCell up)
            {
                up.Money.Text = price;
            }
            else if (cell is RightCell right)
            {
                right.Money.Text = price;
                RotateTextBlock(right.Money, rotateAngle);
            }
            else if (cell is BottomCell bot)
            {
                bot.Money.Text = price;
            }
            else if (cell is LeftCell left)
            {
                left.Money.Text = price;
                RotateTextBlock(left.Money, -rotateAngle);
            }
        }

        private void RotateTextBlock(TextBlock block, int angle)
        {
            const double centerOrigin = 0.5;
            block.RenderTransformOrigin = new Point(centerOrigin, centerOrigin);

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
            if (!(element is IRegularCellsActions)) return;

            /*            if (!(element is UpperCell) && !(element is RightCell) &&
                            !(element is BottomCell) && !(element is LeftCell)) return;*/

            element.PreviewMouseDown += Business_PreviewMouseDown;

            /*            if (element is UpperCell upper)
                            upper.PreviewMouseDown += Business_PreviewMouseDown;

                        if (element is RightCell right)
                            right.PreviewMouseDown += Business_PreviewMouseDown;

                        if (element is BottomCell bottom)
                            bottom.PreviewMouseDown += Business_PreviewMouseDown;

                        if (element is LeftCell left)
                            left.PreviewMouseDown += Business_PreviewMouseDown;*/
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

        private void Business_PreviewMouseDown(object sender, MouseEventArgs e)
        {
            if (_ifBirthdayChance) return;

            SetClickedCellIndex((UIElement)sender);
            BussinessInfo.Children.Clear();

            //Add info about business
            (UIElement info, Size? size) = AddBusinessInfo();

            if (info is null) return;
            SetLocForInfoBox(info, (UIElement)sender, (Size)size);
        }

        private (UIElement, Size?) AddBusinessInfo()
        {
            MonopolyDLL.Monopoly.Cell.Cell cell = _system.MonGame.GameBoard.Cells[_clickedCellIndex];
            if (!(cell is Business)) return (null, null);

            return GetBusinessInfo(cell);
            /*
                        if (_system.MonGame.GameBoard.Cells[_clickedCellIndex].GetType() == typeof(CarBusiness))
                        {
                            CarBusInfo info = new CarBusInfo();
                            BussinessInfo.Children.Add(info);
                            SetCarBusInfoParams(info);
                            info.UpdateLayout();
                            return (info, new Size(info.ActualWidth, info.ActualHeight));
                        }
                        else if (_system.MonGame.GameBoard.Cells[_clickedCellIndex].GetType() == typeof(RegularBusiness))
                        {
                            RegularBusInfo info = new RegularBusInfo();
                            BussinessInfo.Children.Add(info);
                            SetRegularBusinessInfoParams(info);
                            info.UpdateLayout();
                            return (info, new Size(info.ActualWidth, info.ActualHeight));
                        }
                        else if (_system.MonGame.GameBoard.Cells[_clickedCellIndex].GetType() == typeof(GameBusiness))
                        {
                            GameBusInfo info = new GameBusInfo();
                            BussinessInfo.Children.Add(info);
                            SetGameBusInfoParams(info);
                            info.UpdateLayout();
                            return (info, new Size(info.ActualWidth, info.ActualHeight));
                        }


                        return (null, null);*/
        }

        public (UIElement, Size?) GetBusinessInfo(MonopolyDLL.Monopoly.Cell.Cell cell)
        {
            UIElement resInfo;
            Size? size = null;

            resInfo = GetClearedBusinessInfo(cell);
            if (resInfo is null) return (resInfo, null);

            if (resInfo is IBusinessInfoActions inter) size = inter.GetSize();

            BussinessInfo.Children.Add(resInfo);

            resInfo.UpdateLayout();
            SetInfoParams(resInfo);

            return (resInfo, size);
        }

        public void SetInfoParams(UIElement info)
        {
            if (info is CarBusInfo car) SetCarBusInfoParams(car);
            else if (info is RegularBusInfo regular) SetRegularBusinessInfoParams(regular);
            else if (info is GameBusInfo game) SetGameBusInfoParams(game);
        }

        public UIElement GetClearedBusinessInfo(MonopolyDLL.Monopoly.Cell.Cell cell)
        {
            if (cell is CarBusiness) return new CarBusInfo();
            else if (cell is RegularBusiness) return new RegularBusInfo();
            else if (cell is GameBusiness) return new GameBusInfo();
            return null;
        }

        private const int _firstElementIndex = 0;
        private const int _secondElementIndex = 1;
        private const int _thirdElementIndex = 2;
        private const int _fourthElementIndex = 3;
        private const int _fifthElementIndex = 4;
        private const int _sixthElementIndex = 5;

        private void SetCarBusInfoParams(CarBusInfo info)
        {
            //CarBusiness bus = (CarBusiness)_system.MonGame.GameBoard.Cells[_clickedCellIndex];
            info.SetInfoParams((CarBusiness)_system.MonGame.GameBoard.Cells[_clickedCellIndex]);


            /*            info.BusNameText.Text = bus.Name;
                        info.BusType.Text = bus.BusType.ToString();

                        info.OneFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_firstElementIndex]);
                        info.TwoFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_secondElementIndex]);
                        info.ThreeFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_thirdElementIndex]);
                        info.FourFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_fourthElementIndex]);

                        info.FieldPrice.Text = GetConvertedStringWithoutLastK(bus.Price);
                        info.DepositPriceText.Text = GetConvertedStringWithoutLastK(bus.DepositPrice);
                        info.RebuyPrice.Text = GetConvertedStringWithoutLastK(bus.RebuyPrice);

                        info.CarBusHeader.Background = (SolidColorBrush)Application.Current.Resources["CarColor"];*/

            SetCarInfoVisualButs(info);
            SetEventsForCarsInfoButs(info);
        }

        public void SetCarInfoVisualButs(CarBusInfo info)
        {
            if (!_system.MonGame.IsStepperOwnsBusiness(_clickedCellIndex) || _ifBlockMenus)
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
                SetRebuyBusiness(BusinessType.Cars);
                /* const int opacity = 1;
                 if (_system.MonGame.IsPlayerCanRebuyBus(_clickedCellIndex))
                 {
                     AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("RebuyBus")}" +
                         $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                     SetOpacityToBusiness(opacity);
                     _system.MonGame.RebuyBus(_clickedCellIndex);

                     //Set Players payments 
                     _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);

                     //Set Payment
                     SetCarsPayments(_system.MonGame.StepperIndex);
                     UpdatePlayersMoney();

                     Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Hidden);
                     UpdateChatMessages();
                     return;
                 }
                 //AddMessageTextBlock("Not enough money to rebuy business");*/
            };
        }


        public void SetRebuyBusiness(BusinessType type)
        {
            const int opacity = 1;
            if (_system.MonGame.IsPlayerCanRebuyBus(_clickedCellIndex))
            {
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("RebuyBus")} " +
                    $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                SetOpacityToBusiness(opacity);
                _system.MonGame.RebuyBus(_clickedCellIndex);

                //Set Players payments 
                //Set Payment
                if (type == BusinessType.Games)
                {
                    _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);
                    SetGamePayments(_system.MonGame.StepperIndex);

                }
                else if (type == BusinessType.Cars)
                {
                    _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);
                    SetCarsPayments(_system.MonGame.StepperIndex);
                }
                else ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));

                UpdatePlayersMoney();

                Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Hidden);
                UpdateChatMessages();
                return;
            }
        }


        /*        public void SetDepositBusiness(BusinessType type)
                {
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("DepositedBus")} " +
                        $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                    //Change Opacity
                    SetOpacityToBusiness(_depositOpacity);
                    _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                    //Set Players payments 
                    //Set Payment

                    if (type == BusinessType.Games)
                    {
                        _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);
                        SetGamePayments(_system.MonGame.StepperIndex);
                    }
                    else if(type == Business.Cars){
                       
                        _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);

                //Set Payment
                SetCarsPayments(_system.MonGame.StepperIndex);
        }

                    UpdatePlayersMoney();


                    Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Visible);
                    Helper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                        _system.MonGame.GetDepositCounter(_clickedCellIndex));

                    UpdateChatMessages();
                }*/

        public void SetCarDepositEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                SetDepositBusiness(BusinessType.Cars);
                /*const double opacity = 0.5;
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("DepositedBus")}" +
                    $" {_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                //Change Opacity
                SetOpacityToBusiness(opacity);
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Players payments 
                _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);

                //Set Payment
                SetCarsPayments(_system.MonGame.StepperIndex);
                UpdatePlayersMoney();

                Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Visible);

                Helper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                    _system.MonGame.GetDepositCounter(_clickedCellIndex));

                UpdateChatMessages();*/
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
            info.SetInfoParams((GameBusiness)_system.MonGame.GameBoard.Cells[_clickedCellIndex]);

            /*            info.BusName.Text = bus.Name;
                        info.BusType.Text = bus.BusType.ToString();

                        info.BusDescription.Text = SystemParamsService.GetStringByName("GameBusInfoDesc");

                        info.OneFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[0]);
                        info.TwoFieldMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[1]);

                        info.FieldPrice.Text = GetConvertedStringWithoutLastK(bus.Price);
                        info.DepositPriceText.Text = GetConvertedStringWithoutLastK(bus.DepositPrice);
                        info.RebuyPrice.Text = GetConvertedStringWithoutLastK(bus.RebuyPrice);

                        info.GameBusHeader.Background = (SolidColorBrush)Application.Current.Resources["GameColor"];*/

            SetGameInfoVisualButs(info);
            SetGameInfoEvents(info);
        }

        public void SetGameInfoVisualButs(GameBusInfo info)
        {
            if (!_system.MonGame.IsStepperOwnsBusiness(_clickedCellIndex) || _ifBlockMenus)
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
                SetRebuyBusiness(BusinessType.Games);

                /*                if (_system.MonGame.IsPlayerCanRebuyBus(_clickedCellIndex))
                                {
                                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("RebuyBus")} " +
                                        $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                                    SetOpacityToBusiness(1);
                                    _system.MonGame.RebuyBus(_clickedCellIndex);

                                    //Set Players payments 
                                    _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);

                                    //Set Payment
                                    SetGamePayments(_system.MonGame.StepperIndex);
                                    UpdatePlayersMoney();
                                    Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Hidden);
                                    UpdateChatMessages();
                                    return;
                                }*/
                // AddMessageTextBlock("Not enough money to rebuy business");
            };
        }

        private const double _depositOpacity = 0.5;
        public void SetGameDepositButEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                SetDepositBusiness(BusinessType.Games);

                /* AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("DepositedBus")} " +
                     $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                 //Change Opacity
                 SetOpacityToBusiness(_depositOpacity);
                 _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                 //Set Players payments 
                 _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);

                 //Set Payment
                 SetGamePayments(_system.MonGame.StepperIndex);
                 UpdatePlayersMoney();

                 Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Visible);
                 Helper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                     _system.MonGame.GetDepositCounter(_clickedCellIndex));

                 UpdateChatMessages();*/
            };
        }

        public void SetDepositBusiness(BusinessType type)
        {
            AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("DepositedBus")} " +
                $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

            //Change Opacity
            SetOpacityToBusiness(_depositOpacity);
            _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

            //Set Players payments 
            //Set Payment

            if (type == BusinessType.Games)
            {
                _system.MonGame.SetGamePaymentLevels(_system.MonGame.StepperIndex);
                SetGamePayments(_system.MonGame.StepperIndex);
            }
            else if (type == BusinessType.Cars)
            {
                _system.MonGame.SetCarsPaymentLevels(_system.MonGame.StepperIndex);
                SetCarsPayments(_system.MonGame.StepperIndex);
            }
            else ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));

            UpdatePlayersMoney();

            Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Visible);
            Helper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                _system.MonGame.GetDepositCounter(_clickedCellIndex));

            UpdateChatMessages();
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

        private void SetRegularBusinessInfoParams(RegularBusInfo info)
        {
            info.SetInfoParams((RegularBusiness)_system.MonGame.GameBoard.Cells[_clickedCellIndex]);

            // RegularBusiness bus = (RegularBusiness)_system.MonGame.GameBoard.Cells[_clickedCellIndex];

            /*            info.BusName.Text = bus.Name;
                        info.BusType.Text = bus.BusType.ToString();

                        //info.DescriptionText.Text = "Build houses to get bigger payment";            

                        info.BaseRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_firstElementIndex]);
                        info.OneStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_secondElementIndex]);
                        info.TwoStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_thirdElementIndex]);
                        info.ThreeStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_fourthElementIndex]);
                        info.FourStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_fifthElementIndex]);
                        info.YellowStarRentMoney.Text = GetConvertedStringWithoutLastK(bus.PayLevels[_sixthElementIndex]);

                        info.BusPriceMoney.Text = GetConvertedStringWithoutLastK(bus.Price);
                        info.DepositPriceMoney.Text = GetConvertedStringWithoutLastK(bus.DepositPrice);
                        info.RebuyPriceMoney.Text = GetConvertedStringWithoutLastK(bus.RebuyPrice);
                        info.HousePriceMoney.Text = GetConvertedStringWithoutLastK(bus.BuySellHouse);

                        info.NameBusBorder.Background = GetColorForUsualBusHeader(bus);*/

            SetEventsForUsualBusInfo(info);
            SetHouseButtonsForBusInfo(info);
        }

        /*    public string GetConvertedStringWithoutLastK(int price)
            {
                string str = GetConvertedPrice(price);
                str = str.Remove(str.Length - 1);
                return str;
            }*/

        private void SetHouseButtonsForBusInfo(RegularBusInfo info)
        {
            if (!_system.MonGame.IsStepperOwnsBusiness(_clickedCellIndex) || _ifBlockMenus)
            {
                info.DescriptionText.Visibility = Visibility.Visible;
                return;
            }

            bool isHouseWafBuilt =
                _system.MonGame.IsTypeContainsInBuiltCells(
                    _system.MonGame.GameBoard.GetBusinessTypeByIndex(_clickedCellIndex));

            //Its in monopoly
            //if (!_system.MonGame.IfCellIsInMonopoly(_clickedCellIndex)) return;

            //Get type of busts visibility type
            UsualBusinessInfoVisual? type = _system.MonGame.GetButsTypeVisibility(_clickedCellIndex);

            if (!(type is null) && isHouseWafBuilt &&
                (type == UsualBusinessInfoVisual.Combine || type == UsualBusinessInfoVisual.SellHouse))
            {
                type = UsualBusinessInfoVisual.SellHouse;
            }
            else if (isHouseWafBuilt && type is null)
            {
                info.DescriptionText.Visibility = Visibility.Visible;
                return;
            }
            else if (!ChatMessages.Children.OfType<ThroughCubes>().Any() &&
                !ChatMessages.Children.OfType<PrisonQuestion>().Any())
            {
                if (type == UsualBusinessInfoVisual.BuyHouse)
                {
                    info.DescriptionText.Visibility = Visibility.Visible;
                    return;
                }
                else if (type == UsualBusinessInfoVisual.DepositAndBuildHouse)
                {
                    type = UsualBusinessInfoVisual.Deposit;
                }
                else if (type == UsualBusinessInfoVisual.Combine)//
                {
                    type = UsualBusinessInfoVisual.SellHouse;
                    //return;
                }
            }

            info.DescriptionText.Visibility = Visibility.Hidden;
            switch (type)
            {
                case UsualBusinessInfoVisual.BuyHouse:
                    info.BuildHouseBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusinessInfoVisual.SellHouse:
                    info.SellHouseBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusinessInfoVisual.Combine:
                    info.CombineHouseButs.Visibility = Visibility.Visible;
                    break;
                case UsualBusinessInfoVisual.Rebuy:
                    info.RebuyBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusinessInfoVisual.Deposit:
                    info.DepositBut.Visibility = Visibility.Visible;
                    break;
                case UsualBusinessInfoVisual.DepositAndBuildHouse:
                    info.CombineDepositBuyHouse.Visibility = Visibility.Visible;
                    break;
            }
        }

        public void SetEventsForUsualBusInfo(RegularBusInfo info)
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
                SetRebuyBusiness(BusinessType.Perfume);
                /*                const int clearOpacity = 1;
                                if (_system.MonGame.IsPlayerCanRebuyBus(_clickedCellIndex))
                                {
                                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("RebuyBus")} " +
                                        $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                                    //Change opacity
                                    SetOpacityToBusiness(clearOpacity);
                                    //rebuy bus
                                    _system.MonGame.RebuyBus(_clickedCellIndex);

                                    //Set Payment
                                    ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));

                                    UpdatePlayersMoney();
                                    Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Hidden);
                                    UpdateChatMessages();
                                    return;
                                }*/
                //AddMessageTextBlock("Not enough money to rebut business");
            };
        }


        public void SetDepositBusEvent(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                SetDepositBusiness(BusinessType.Perfume);

                /*const double opacity = 0.5;
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("DepositedBus")} " +
                    $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                //Change Opacity
                SetOpacityToBusiness(opacity);
                //Deposit bus
                _system.MonGame.SetBusAsDeposited(_clickedCellIndex);

                //Set Payment
                ChangePriceInBusiness(_clickedCellIndex, _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));
                UpdatePlayersMoney();

                Helper.ChangeDepositCounterVisibility(_cells[_clickedCellIndex], Visibility.Visible);
                Helper.SetValueForDepositCounter(_cells[_clickedCellIndex],
                    _system.MonGame.GetDepositCounter(_clickedCellIndex));

                UpdateChatMessages();*/
            };
        }

        public Grid GetCanvasToChangeOpacityForDepositedCell()
        {
            return _cells[_clickedCellIndex] is IRegularCellsActions inter ?
                inter.GetImagePlacer() : throw new Exception("no such cellType!");

            /*    return _cells[_clickedCellIndex] is UpperCell up ? up.ImagePlacer :
                    _cells[_clickedCellIndex] is RightCell right ? right.ImagePlacer :
                    _cells[_clickedCellIndex] is BottomCell bot ? bot.ImagePlacer :
                    _cells[_clickedCellIndex] is LeftCell left ? left.ImagePlacer :
                    throw new Exception("no such cellType!");*/
        }

        public void SetSellHouseEventBuy(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("SoldHouse")} " +
                    $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                _system.MonGame.SellHouse(_clickedCellIndex);
                SetHousesInCellParams();

                UpdateChatMessages();
                return;
            };
        }

        public void SetBuyHouseEventBut(Button but)
        {
            but.PreviewMouseDown += (sender, e) =>
            {
                if (_system.MonGame.IsPlayersHasEnoughMoneyToBuyHouse(_clickedCellIndex))
                {
                    AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("BuyHouse")} " +
                        $"{_system.MonGame.GetBusNameOnGivenIndex(_clickedCellIndex)}", _system.MonGame.StepperIndex);

                    _system.MonGame.BuyHouse(_clickedCellIndex);
                    SetHousesInCellParams();

                    _system.MonGame.AddStepperBoughtHouseType(_clickedCellIndex);
                    UpdateChatMessages();
                    return;
                }
            };
        }

        public void SetHousesInCellParams()
        {
            SetCellStars(_clickedCellIndex);

            UpdatePlayersMoney();
            BussinessInfo.Children.Clear();

            ChangePriceInBusiness(_clickedCellIndex,
                _system.MonGame.GetBusMoneyLevel(_clickedCellIndex));
        }

        public void SetCellStars(int cellIndex)
        {
            if (_system.MonGame.IsCellIsUsualBusiness(cellIndex)) // cell is usual business
            {
                int level = _system.MonGame.GetBusLevel(cellIndex);
                SetBusinessStars(level, cellIndex);
            }
        }

        public void SetCellsStars()
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SetCellStars(i);
                /*
                                if (_system.MonGame.IsCellIsUsualBusiness(i)) // cell is usual business
                                {
                                    int level = _system.MonGame.GetBusLevel(i);
                                    SetBusinessStars(level, i);
                                }*/
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
            return _cells[cellIndex] is IRegularCellsActions intro ? intro.GetStarGrid() : throw new Exception("No such cell type");
            /*
                        return _cells[cellIndex] is UpperCell upper ? upper.StarsGrid :
                            _cells[cellIndex] is RightCell right ? right.StarsGrid :
                            _cells[cellIndex] is BottomCell bottom ? bottom.StarsGrid :
                            _cells[cellIndex] is LeftCell left ? left.StarsGrid : throw new Exception("No such cell type");*/
        }

        public void SetGoldenStar(Grid toAdd)
        {
            const int hotelStarParam = 20;
            toAdd.Children.Clear();
            Image goldenStar = new Image()
            {
                Width = hotelStarParam,
                Height = hotelStarParam,
                Source = GetGoldenImage().Source
            };
            toAdd.Children.Add(goldenStar);
        }

        public void SetSilverStart(int level, Grid toAdd)
        {
            const int silverStarParam = 15;
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
                    Width = silverStarParam,
                    Height = silverStarParam,
                    Source = silver.Source
                };
                starPanel.Children.Add(img);
            }
            toAdd.Children.Add(starPanel);
        }

        public Image GetImageSilverImage()
        {
            return Helper.GetImageFromOtherFolder("whiteStar.png");
        }

        public Image GetGoldenImage()
        {
            return Helper.GetImageFromOtherFolder("yellowStar.png");
        }

        public SolidColorBrush GetColorForUsualBusHeader(RegularBusiness bus)
        {
            return bus.BusinessType == BusinessType.Perfume ? (SolidColorBrush)Application.Current.Resources["PerfumeColor"] :
                bus.BusinessType == BusinessType.Clothes ? (SolidColorBrush)Application.Current.Resources["ClothesColor"] :
                bus.BusinessType == BusinessType.Messengers ? (SolidColorBrush)Application.Current.Resources["MessagerColor"] :
                bus.BusinessType == BusinessType.Drinks ? (SolidColorBrush)Application.Current.Resources["DrinkColor"] :
                bus.BusinessType == BusinessType.Planes ? (SolidColorBrush)Application.Current.Resources["PlaneColor"] :
                bus.BusinessType == BusinessType.Food ? (SolidColorBrush)Application.Current.Resources["FoodColor"] :
                bus.BusinessType == BusinessType.Hotels ? (SolidColorBrush)Application.Current.Resources["HotelColor"] :
                bus.BusinessType == BusinessType.Phones ? (SolidColorBrush)Application.Current.Resources["PhoneColor"] :
                throw new Exception("No such business type...How is it possible?");


            /*            if (bus.BusType == BusinessType.Perfume)
                        {
                            return (SolidColorBrush)Application.Current.Resources["PerfumeColor"];
                        }
                        else if (bus.BusType == BusinessType.Clothes)
                        {
                            return (SolidColorBrush)Application.Current.Resources["ClothesColor"];
                        }
                        else if (bus.BusType == MonopolyDLL.Monopoly.Enums.BusinessType.Messengers)
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

                        throw new Exception("No such business type...How is it possible?");*/
        }

        private const int _centerDivider = 2;
        private void SetLocForInfoBox(UIElement info, UIElement cell, Size size)
        {
            const int distanceToCell = 5;
            Point fieldPoint = this.PointToScreen(new Point(0, 0));
            Point cellPoint = cell.PointToScreen(new Point(0, 0));

            size = new Size(((FrameworkElement)info).ActualWidth, ((FrameworkElement)info).ActualHeight);

            if (info is GameBusInfo) size.Width += distanceToCell * _centerDivider + distanceToCell;

            Point cellLocalPoint = new Point(cellPoint.X - fieldPoint.X, cellPoint.Y - fieldPoint.Y);

            Point infoLoc = new Point(0, 0);
            if (cell is UpperCell up)
            {
                infoLoc = new Point(cellLocalPoint.X + up.Width / _centerDivider - size.Width / _centerDivider, up.Height + distanceToCell);
            }
            else if (cell is RightCell)
            {
                double jacPotYLoc = JackPotCell.PointToScreen(new Point(0, 0)).Y - fieldPoint.Y;

                double yLoc = cellLocalPoint.Y + size.Height > jacPotYLoc ?
                    jacPotYLoc - size.Height : cellLocalPoint.Y;

                infoLoc = new Point(cellLocalPoint.X - size.Width - distanceToCell, yLoc);
            }
            else if (cell is BottomCell bot)
            {
                infoLoc = new Point(cellLocalPoint.X + bot.Width / _centerDivider - size.Width / _centerDivider,
                    cellLocalPoint.Y - size.Height - distanceToCell);
            }
            else if (cell is LeftCell left)
            {
                double jacPotYLoc = JackPotCell.PointToScreen(new Point(0, 0)).Y - fieldPoint.Y;

                double yLoc = cellLocalPoint.Y + size.Height > jacPotYLoc ?
                    jacPotYLoc - size.Height : cellLocalPoint.Y;

                infoLoc = new Point(cellLocalPoint.X + left.Width + distanceToCell, yLoc);
            }

            Canvas.SetLeft(info, infoLoc.X);
            Canvas.SetTop(info, infoLoc.Y);
        }

        //BREAKETS, mb need to move it into another file
        //set chips position and make a movement  
        //Set chips position in fields by math

        private List<(Image, int)> chipAndCord = new List<(Image, int)>();
        private void MakeChipsMovementAction(int startCellIndex, int cellIndexToMoveOn, Image chipImage)
        {
            if (_ifChipMoves) return;

            _squareIndexesToGoThrough = !_system.MonGame.IsNeedToMoveBackwards() ?
                Helper.GetListOfSquareCellIndexesThatChipGoesThrough(startCellIndex, cellIndexToMoveOn) :
                Helper.GetListOfSquaresIndexesToGoThroughBackwards(startCellIndex, cellIndexToMoveOn);


            if (_ifWithoutGoingThrough || _goToPrisonByDouble)
            {
                _ifWithoutGoingThrough = false;
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
            //ReassignChipImageInNewCell(move distance, tempChipImg, newInsideChipPoint);


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

        private void UpdatePrisonCanvas(Canvas can, int cellIndex, bool isSit)
        {
            //new points for chips
            List<Point> newChipsPoints = isSit ?
                Helper.GetPointsForPrisonCellSitter(can.Children.OfType<Image>().ToList().Count,
                new Size(can.ActualWidth, can.ActualHeight)) :

                Helper.GetPointsForPrisonCellExcursion(can.Children.OfType<Image>().ToList().Count,
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
        private EventHandler _chipAnimationEvent;

        private bool IsLastMoveIsPrison = false;

        public void MakeChipMove(int startCellIndex, int cellIndexToMoveOn, Image chipImg,
            Point insidePointStartCell, Point newInsideChipPoint)
        {
            const int goThroughPrisonAmount = 1;
            //chip goes through square cell
            if (!(_squareIndexesToGoThrough is null))
            {
                _tempSquareValToGoOn = _squareIndexesToGoThrough.First();
                _ifGoesThroughSquareCell = true;

                if (_cells[_squareIndexesToGoThrough.Last()] is PrisonCell) IsLastMoveIsPrison = true;

                _chipAnimationEvent = (s, e) =>
                {
                    Point InsideChipPoint = GetInsidePointToStepOn(_cells[_tempSquareValToGoOn], false);

                    //Helper.GetCenterOfTheSquareForImage(chipImg, _cells[_tempSquareValToGoOn])

                    ReassignChipImageInNewCell(_tempSquareValToGoOn, chipImg, InsideChipPoint);

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
                        //Helper.GetCenterOfTheSquareForImage(chipImg, _cells[_tempSquareValToGoOn])

                        MakeChipMoveToAnotherCell(toRemoveSquareIndex, _tempSquareValToGoOn, chipImg,
                        check, moveOn);

                        if (IsLastMoveIsPrison && _squareIndexesToGoThrough.Count == goThroughPrisonAmount)
                        {
                            IsLastMoveIsPrison = false;
                            MakeAMoveToInPrisonCell(IsPlayerSitsInPrisonByChipImage(chipImg));
                        }
                    }
                    else
                    {
                        _ifChipMoves = false;
                    }
                };
                MakeChipMoveToAnotherCell(startCellIndex, _tempSquareValToGoOn, chipImg,
                    insidePointStartCell, newInsideChipPoint);
            }
            else
            {
                //usualMove
                MakeChipMoveToAnotherCell(startCellIndex, cellIndexToMoveOn, chipImg, insidePointStartCell, newInsideChipPoint);
            }
        }

        //To Make a move in prison cell(sitter or visit)
        private void MakeAMoveToInPrisonCell(bool isSitsInPrison)
        {
            if (!isSitsInPrison)
            {
                List<Point> points = Helper.GetPointsForPrisonCellExcursion(PrisonCell.ChipsPlacerVisit.Children.Count,
                    new Size(PrisonCell.Width, PrisonCell.Height));

                SetChipsImagesInMovePanel(points, PrisonCell.ChipsPlacerVisit, _system.MonGame.GameBoard.GetPrisonCellIndex());
                return;
            }
            List<Point> sitPrisonPoints = Helper.GetPointsForPrisonCellSitter(PrisonCell.ChipsPlacerSitters.Children.Count,
            new Size(PrisonCell.Width, PrisonCell.Height));
            SetChipsImagesInMovePanel(sitPrisonPoints, PrisonCell.ChipsPlacerSitters, _system.MonGame.GameBoard.GetPrisonCellIndex());
        }

        private void SetNewPositionsToChipsInCell(int cellIndex, bool isInMove = false)
        {
            Canvas can = GetChipCanvas(_cells[cellIndex]);

            //new points for chips
            List<Point> newChipsPoints = Helper.GetPointsForChips(
                new Size(can.ActualWidth, can.ActualHeight), can.Children.OfType<Image>().ToList().Count, isInMove);

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

            if (el is SquareCell square)
            {
                return square.ChipsPlacer;
            }

            var chipsPlacerField = el.GetType().GetField(
                canvasName,
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            if (chipsPlacerField is null)
            {
                throw new Exception("Cell does not have chips placer");
            }
            return chipsPlacerField.GetValue(el) as Canvas;
        }

        public string GetCanvasNameToStepOn(Image img)
        {
            if (img is null) return PrisonCell.ChipsPlacer.Name.ToString();
            int index = _images.IndexOf(img);
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


        private void MakeChipMoveToAnotherCell(int startCellIndex, int movePoint, Image chip,
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

            //Set chip animation 
            SetChipToMoveAnimation(chip, pointToStepOn, movePoint, newCellInsideChipPoint);
        }

        private Point GetInsidePointToStepOn(UIElement cellToMoveOn, bool isInMove = false)
        {
            //amount of chips in cell to step on
            int amountOfChipsInCell = _system.MonGame.GetAmountOfPlayersInCell(_cells.IndexOf(cellToMoveOn));

            //Size of cell to step on
            Size cellSize = Helper.GetSizeOfCell(cellToMoveOn);

            //Get Points for chips to place on cel to step on (need to replace all chips)
            List<Point> chipPoints = Helper.GetPointsForChips(cellSize, amountOfChipsInCell, isInMove);

            return chipPoints.Last();
        }

        private bool _ifChipMoves = false;
        private DoubleAnimation _chipMoveAnimation;
        private const double _chipAnimationDur = 0.85;
        private void SetChipToMoveAnimation(Image toMove, Point endPoint,
            int cellIndex, Point newInsideChipPoint, bool IsChipsInOldCell = false)
        {
            var transform = new TranslateTransform();
            toMove.RenderTransform = transform;

            _chipMoveAnimation = GetAnimation(0, endPoint.X, _chipAnimationDur);/*new DoubleAnimation
            {
                From = 0,
                To = endPoint.X,
                Duration = TimeSpan.FromSeconds(_chipAnimationDur),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };*/

            if (!(_squareIndexesToGoThrough is null) &&
                _ifGoesThroughSquareCell && !IsChipsInOldCell) // tempMove Chip Img
            {
                _chipMoveAnimation.Completed += _chipAnimationEvent;

                if (_squareIndexesToGoThrough.Count == 1) _chipMoveAnimation.Completed += _checkEvent;
            }
            else
            {
                _chipMoveAnimation.Completed += (s, e) =>
                {
                    ReassignChipImageInNewCell(cellIndex, toMove, newInsideChipPoint);

                    if (IsLastMoveIsPrison)
                    {
                        IsLastMoveIsPrison = false;
                        MakeAMoveToInPrisonCell(IsPlayerSitsInPrisonByChipImage(toMove));
                    }
                    _ifChipMoves = false;
                };
            }


            DoubleAnimation moveYAnimation = GetAnimation(0, endPoint.Y, _chipAnimationDur); /*new DoubleAnimation
            {
                From = 0,
                To = endPoint.Y,
                Duration = TimeSpan.FromSeconds(_chipAnimationDur),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };*/

            transform.BeginAnimation(TranslateTransform.XProperty, _chipMoveAnimation);
            transform.BeginAnimation(TranslateTransform.YProperty, moveYAnimation);

            _ifChipMoves = true;
        }

        public DoubleAnimation GetAnimation(double startPoint, double endPoint, double duration)
        {
            return new DoubleAnimation
            {
                From = startPoint,
                To = endPoint,
                Duration = TimeSpan.FromSeconds(duration),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut }
            };
        }

        public bool IsPlayerSitsInPrisonByChipImage(Image chipImage)
        {
            return _system.MonGame.IsPlayerSitsInPrison(_images.IndexOf(chipImage));
        }

        private void AddChipToChipMovePanel(Image img, Point startChipPoint)
        {
            Canvas.SetLeft(img, startChipPoint.X);
            Canvas.SetTop(img, startChipPoint.Y);

            ChipMovePanel.Children.Add(img);
        }

        private List<Image> _images = new List<Image>();
        private void SetChipPositionsInStartSquare()
        {
            _images = Helper.GetAllChipsImages(_system.MonGame.Players.Count);

            for (int i = 0; i < _images.Count; i++)
            {
                chipAndCord.Add((_images[i], _system.MonGame.Players[i].Position));
            }

            for (int i = 0; i < _images.Count; i++)
            {
                SetStartChipInPlacerCanvas(_cells[_system.MonGame.Players[i].Position], _images[i]);
            }

            List<int> unique = _system.MonGame.GetUniquePositions();
            //Set Positions
            for (int i = 0; i < unique.Count; i++)
            {
                List<List<Point>> cellPoints = Helper.GetChipsPoints(
                    GetCellSize(_cells[unique[i]]));

                List<Point> res = cellPoints[_system.MonGame.GetAmountOfPlayersInCell(unique[i]) - 1];
                List<(Image, int)> chipsInCell = chipAndCord.Where(x => x.Item2 == unique[i]).ToList();

                for (int j = 0; j < chipsInCell.Count; j++)
                {
                    Canvas.SetLeft(chipsInCell[j].Item1, res[j].X);
                    Canvas.SetTop(chipsInCell[j].Item1, res[j].Y);
                }
            }
        }

        private Size GetCellSize(UIElement cell)
        {
            return cell is IAllCellActions intro ? intro.GetCellSize() : throw new Exception("Type of cell is not exist... Just How?");

            /*
                        return cell is SquareCell square ? new Size(square.ChipsPlacer.Width, square.ChipsPlacer.Height) :
                            cell is UpperCell up ? new Size(up.ChipsPlacer.Width, up.ChipsPlacer.Height) :
                            cell is PrisonCell prison ? new Size(prison.ChipsPlacer.Width, prison.ChipsPlacer.Height) :
                            cell is RightCell right ? new Size(right.ChipsPlacer.Width, right.ChipsPlacer.Height) :
                            cell is BottomCell bot ? new Size(bot.ChipsPlacer.Width, bot.ChipsPlacer.Height) :
                            cell is LeftCell left ? new Size(left.ChipsPlacer.Width, left.ChipsPlacer.Height) :
                            throw new Exception("Type of cell is not exist... Just How?");*/

            /* if (cell is SquareCell square)
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
             throw new Exception("Type of cell is not exist... Just How?");*/
        }

        private void SetStartChipInPlacerCanvas(UIElement cell, Image chip)
        {
            if (cell is IAllCellActions intro)
            {
                intro.AddChip(chip);
            }

            /*            if (cell is SquareCell square)
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
                        }*/
        }

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
        private void SetImmortalImages()
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

        //20 25 30 35 45 50 55 65 75 95 100

        private void SetBasicGameImages()
        {
            SetRightCellImage(Helper.GetImageFromFolder("rockstarGames.png", "Games"), GamesFirstBus,
                new Size(SystemParamsService.GetNumByName("SeventhCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetBottomCellImage(Helper.GetImageFromFolder("rovio.png", "Games"), GamesSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("SecondCardImageSizeParam")));
        }

        private void SetBasicCarImages()
        {
            SetUpperCellImage(Helper.GetImageFromFolder("mercedes.png", "Cars"), UpCar,
                 new Size(SystemParamsService.GetNumByName("SixthCardImageSizeParam"),
                SystemParamsService.GetNumByName("SixthCardImageSizeParam")));

            SetRightCellImage(Helper.GetImageFromFolder("audi.png", "Cars"), RightCar,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("ThirdCardImageSizeParam")));

            SetBottomCellImage(Helper.GetImageFromFolder("ford.png", "Cars"), DownCars,
                new Size(SystemParamsService.GetNumByName("TensCardImageSizeParam"),
                SystemParamsService.GetNumByName("TwelveCardImageSizeParam")));

            SetLeftCellImage(Helper.GetImageFromFolder("land_rover.png", "Cars"), LeftCar,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

        }

        private void SetBasicPhoneImages()
        {
            SetLeftCellImage(Helper.GetImageFromFolder("apple.png", "Phones"), PhonesFirstBus,
                new Size(SystemParamsService.GetNumByName("TwelveCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetLeftCellImage(Helper.GetImageFromFolder("nokia.png", "Phones"), PhonesSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FirstCardImageSizeParam")));
        }

        private void SetBasicHotelImages()
        {
            SetLeftCellImage(Helper.GetImageFromFolder("holidayInn.png", "Hotels"), HotelFirstBus,
                new Size(SystemParamsService.GetNumByName("EighthCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetLeftCellImage(Helper.GetImageFromFolder("radissonBlu.png", "Hotels"), HotelSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FourthCardImageSizeParam")));

            SetLeftCellImage(Helper.GetImageFromFolder("novotel.png", "Hotels"), HotelThirdBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));
        }

        private void SetLeftCellImage(Image img, LeftCell cell, Size imageSize)
        {
            SetRightLeftImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicFoodImages()
        {
            SetBottomCellImage(Helper.GetImageFromFolder("maxBurgers.png", "Food"), FoodFirstBus,
                new Size(SystemParamsService.GetNumByName("SeventhCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetBottomCellImage(Helper.GetImageFromFolder("burgerKing.png", "Food"), FoodSecondBus,
                new Size(SystemParamsService.GetNumByName("SeventhCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetBottomCellImage(Helper.GetImageFromFolder("kfc.png", "Food"), FoodThirdBus,
                new Size(SystemParamsService.GetNumByName("SeventhCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));
        }

        private void SetBasicPlanesImages()
        {
            SetBottomCellImage(Helper.GetImageFromFolder("americanAirlines.png", "Planes"), PlanesFirstBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FirstCardImageSizeParam")));

            SetBottomCellImage(Helper.GetImageFromFolder("lufthansa.png", "Planes"), PlanesSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FirstCardImageSizeParam")));

            SetBottomCellImage(Helper.GetImageFromFolder("britishAirways.png", "Planes"), PlanesThirdBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FirstCardImageSizeParam")));
        }

        private void SetBottomCellImage(Image img, BottomCell cell, Size imageSize)
        {
            SetUpDownImage(img, imageSize);
            cell.ImagePlacer.Children.Add(img);
        }

        private void SetBasicDrinkingsImages() //Misha, it is right word
        {
            SetRightCellImage(Helper.GetImageFromFolder("cocaCola.png", "Drinkings"), DrinkFirstBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FourthCardImageSizeParam")));

            SetRightCellImage(Helper.GetImageFromFolder("pepsi.png", "Drinkings"), DrinkSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FourthCardImageSizeParam")));

            SetRightCellImage(Helper.GetImageFromFolder("fanta.png", "Drinkings"), DrinkThirdBus,
                new Size(SystemParamsService.GetNumByName("EighthCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));
        }

        private void SetBasicMessagerImages()
        {
            SetRightCellImage(Helper.GetImageFromFolder("vk.png", "Messenger"), MessagerFirstBus,
                new Size(SystemParamsService.GetNumByName("NinethCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetRightCellImage(Helper.GetImageFromFolder("facebook.png", "Messenger"), MessagerSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("ThirdCardImageSizeParam")));

            SetRightCellImage(Helper.GetImageFromFolder("twitter.png", "Messenger"), MessagerThirdBus,
                new Size(SystemParamsService.GetNumByName("SeventhCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));
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
            SetUpperCellImage(Helper.GetImageFromFolder("adidas.png", "Clothes"), ClothesFirstBus,
                new Size(SystemParamsService.GetNumByName("NinethCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetUpperCellImage(Helper.GetImageFromFolder("puma.png", "Clothes"), ClothesSeconBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetUpperCellImage(Helper.GetImageFromFolder("lacoste.png", "Clothes"), ClothesThirdBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));
        }

        public void SetBasicPerfumeImages()
        {
            SetUpperCellImage(Helper.GetImageFromFolder("chanel.png", "Perfume"), PerfumeFirstBus,
                new Size(SystemParamsService.GetNumByName("NinethCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));

            SetUpperCellImage(Helper.GetImageFromFolder("hugoBoss.png", "Perfume"), PerfumeSecondBus,
                new Size(SystemParamsService.GetNumByName("ElevensCardImageSizeParam"),
                SystemParamsService.GetNumByName("FifthCardImageSizeParam")));
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
            const double centerOrigin = 0.5;
            const int rotateAngle = 90;
            img.RenderTransformOrigin = new Point(centerOrigin, centerOrigin);

            RotateTransform transform = new RotateTransform()
            {
                Angle = -rotateAngle,
            };

            img.LayoutTransform = transform;
        }

        private Size _chanceSize = new Size(55, 80);
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
            return GetImageByNameFromOtherFolder("chanceRotated.png", _chanceSize.Height, ChanceFive.ImagePlacer.Height);

            /*            Image img = Helper.GetImageFromOtherFolder("chanceRotated.png");

                        img.Width = _chanceSize.Height;
                        img.Height = ChanceFive.ImagePlacer.Height;
                        return img;*/
        }

        private void SetRightChances()
        {
            Image img = GetImageByNameFromOtherFolder("chance.png", _chanceSize.Height, ChanceThree.ImagePlacer.Height);

            /*            Image img = Helper.GetImageFromOtherFolder("chance.png");

                        img.Width = _chanceSize.Height;
                        img.Height = ChanceThree.ImagePlacer.Height;*/

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
            return GetImageByNameFromOtherFolder("chanceUp.png", _chanceSize.Width, ChanceOne.ImagePlacer.Height);
            /*
                        Image img = Helper.GetImageFromOtherFolder("chanceUp.png");

                        img.Width = _chanceSize.Width;
                        img.Height = ChanceOne.ImagePlacer.Height;
                        return img;*/
        }

        private Image GetImageByNameFromOtherFolder(string name, double width, double height)
        {
            Image img = Helper.GetImageFromOtherFolder(name);

            img.Width = width;
            img.Height = height;
            return img;
        }

        private void SetTaxesImages()
        {
            const int taxAdder = 10;
            //Set little Tax
            Image little = Helper.GetTaxImageByName("taxIncome.png");
            little.Width = LittleTax.Width - _upperMargin;
            little.Height = LittleTax.Height / _centerDivider;

            little.HorizontalAlignment = HorizontalAlignment.Center;
            little.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(little, taxAdder);

            LittleTax.ImagePlacer.Children.Add(little);

            //Set big tax
            Image big = Helper.GetTaxImageByName("taxLuxury.png");
            big.Width = BigTax.Width / _centerDivider;
            big.Height = big.Height - taxAdder;

            big.HorizontalAlignment = HorizontalAlignment.Center;
            big.VerticalAlignment = VerticalAlignment.Center;

            Panel.SetZIndex(big, taxAdder);

            BigTax.ImagePlacer.Children.Add(big);
        }

        public void SetSquareCells()
        {
            SetStartImage();
            SetJackPotImage();
            SetGoToPrisonImage();
            SetPrisonImage();
        }

        private const int _basePrisonImagesSize = 50;
        private void SetPrisonImage()
        {
            const int jailTop = 65;
            const int jailLeft = 10;
            Image jail = new Image()
            {
                Source = Helper.GetSquareByName("jail.png").Source,
                Width = GameField._basePrisonImagesSize,
                Height = _basePrisonImagesSize
            };

            Canvas.SetLeft(jail, 0);
            Canvas.SetTop(jail, jailTop);

            PrisonCell.ImagesPlacer.Children.Add(jail);

            Image donat = GetSizedSquareImage(new Size(GameField._basePrisonImagesSize, _basePrisonImagesSize), "jailVisiting.png");

            /*            new Image()
                        {
                            Source = Helper.GetSquareByName("jailVisiting.png").Source,
                            Width = GameField._basePrisonImagesSize,
                            Height = _basePrisonImagesSize
                        };*/

            Canvas.SetLeft(donat, jailTop);
            Canvas.SetTop(donat, jailLeft);

            PrisonCell.ImagesPlacer.Children.Add(donat);
        }


        private const int _squareSize = 125;
        private void SetGoToPrisonImage()
        {
            Image img = GetSizedSquareImage(new Size(_squareSize, _squareSize), "goToJail.png");
            /*            new Image()
                        {
                            Source = Helper.GetSquareByName("goToJail.png").Source,
                            Width = toPrisonSizeParam,
                            Height = toPrisonSizeParam
                        };*/

            GoToPrisonCell.ImagesPlacer.Children.Add(img);
        }

        public Image GetSizedSquareImage(Size size, string imgName)
        {
            return new Image()
            {
                Source = Helper.GetSquareByName(imgName).Source,
                Width = size.Width,
                Height = size.Height
            };
        }

        private void SetJackPotImage()
        {
            /*            const int jackPotSizeParam = 125;
                        Image img = new Image()
                        {
                            Source = Helper.GetSquareByName("jackpot.png").Source,
                            Width = jackPotSizeParam,
                            Height = jackPotSizeParam
                        };*/

            Image img = GetSizedSquareImage(new Size(_squareSize, _squareSize), "jackpot.png");
            JackPotCell.ImagesPlacer.Children.Add(img);
        }

        private void SetStartImage()
        {
            /*            const int startSizeParam = 125;
                        Image img = new Image()
                        {
                            Source = Helper.GetSquareByName("start.png").Source,
                            Width = startSizeParam,
                            Height = startSizeParam
                        };*/

            Image img = GetSizedSquareImage(new Size(_squareSize, _squareSize), "start.png");
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

        private void AddWrapPanelToChatBox(string message, int playerIndex = -1, bool isPlayerWrote = false)
        {
            if (message == string.Empty) return;

            const int borderValue = 5;
            WrapPanel panel = new WrapPanel();
            if (playerIndex != -1) panel = GetWrapPanelForMessage(playerIndex);

            if (isPlayerWrote && playerIndex != -1)
            {
                TextBlock block = panel.Children.OfType<TextBlock>().First();

                panel.Children.Remove(block);

                block.Foreground = new SolidColorBrush(Colors.White);
                Border border = new Border()
                {
                    CornerRadius = new CornerRadius(borderValue),
                    Child = block,
                    Background = _colors[playerIndex],
                    Padding = new Thickness(borderValue)
                };
                panel.Children.Add(border);
            }

            if (playerIndex != -1 || panel.Children.Count == 0)
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
            const int leftMargin = 5;
            return new TextBlock()
            {
                Text = message,
                FontSize = _textFontSize,
                Foreground = Brushes.White,
                Margin = new Thickness(leftMargin, 0, 0, 0),
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

            //Type type = typeof(UpperCell);

            //if (FirstLinePanel.Children[0].GetType().Equals(type))

            /*            for (int i = 0; i < FirstLinePanel.Children.Count; i++)
                        {
                            if (FirstLinePanel.Children[i] is UpperCell)
                            {
                                _cells.Add(FirstLinePanel.Children[i]);
                            }
                        }*/

            _cells.AddRange(FirstLinePanel.Children.OfType<UpperCell>());

            _cells.Add(PrisonCell);

            /*           for (int i = 0; i < SecondLinePanel.Children.Count; i++)
                       {
                           if (SecondLinePanel.Children[i] is RightCell)
                           {
                               _cells.Add(SecondLinePanel.Children[i]);
                           }
                       }*/

            _cells.AddRange(SecondLinePanel.Children.OfType<RightCell>());

            _cells.Add(JackPotCell);

            /*            for (int i = ThirdLinePanel.Children.Count - 1; i >= 0; i--)
                        {
                            if (ThirdLinePanel.Children[i] is BottomCell)
                            {
                                _cells.Add(ThirdLinePanel.Children[i]);
                            }
                        }*/
            //_cells.AddRange(ThirdLinePanel.Children.OfType<BottomCell>());
            AddReversedCellsInList(ThirdLinePanel);

            _cells.Add(GoToPrisonCell);
            /*
                        for (int i = FourthLinePanel.Children.Count - 1; i >= 0; i--)
                        {
                            if (FourthLinePanel.Children[i] is LeftCell)
                            {
                                _cells.Add(FourthLinePanel.Children[i]);
                            }
                        }*/
            AddReversedCellsInList(FourthLinePanel);

            //_cells.AddRange(FourthLinePanel.Children.OfType<LeftCell>());
        }


        private void AddReversedCellsInList(StackPanel panel)
        {
            for (int i = panel.Children.Count - 1; i >= 0; i--)
            {
                if (panel.Children[i] is LeftCell ||
                    panel.Children[i] is BottomCell)
                {
                    _cells.Add(panel.Children[i]);
                }
            }
        }


        public void SetGiveUpActionToButton(Button but, int? playerIndex)
        {
            if (playerIndex is null)
            {
                playerIndex = _system.MonGame.StepperIndex;
            }

            but.Click += (sender, e) =>
            {
                PlayerGaveUp((int)playerIndex);
            };
        }

        public bool _gameEnded = false;
        public void PlayerGaveUp(int? playerIndex)
        {
            if (playerIndex is null)
            {
                playerIndex = _system.MonGame.StepperIndex;
            }
            SetGiveControl((int)playerIndex);
        }

        private void SetGiveControl(int playerIndex)
        {
            Image img = _cards[playerIndex].UserImageGrid.Children.OfType<Image>().FirstOrDefault();

            if (img is null) return;
            const int justHighZIndex = 1000;
            MainWindow wind = (MainWindow)Window.GetWindow(_frame);

            ShowGotPrize toGiveUpControl = new ShowGotPrize(img);

            wind.VisiableItems.Children.Add(toGiveUpControl);

            Canvas.SetLeft(toGiveUpControl, wind.ActualWidth / _centerDivider - toGiveUpControl.Width / _centerDivider);
            Canvas.SetTop(toGiveUpControl, wind.ActualHeight / _centerDivider - toGiveUpControl.Height / _centerDivider);

            Canvas.SetZIndex(wind.VisiableItems, justHighZIndex);

            this.IsEnabled = false;

            toGiveUpControl.ButtonsGrid.Visibility = Visibility.Hidden;
            toGiveUpControl.GiveUpGrid.Visibility = Visibility.Visible;

            toGiveUpControl.HeadText.Text = SystemParamsService.GetStringByName("IfGiveUp");

            toGiveUpControl.GiveUpBut.Click += (sender, e) =>
            {
                this.IsEnabled = true;
                wind.VisiableItems.Children.Clear();
                SetPlayerGaveUp(playerIndex);
            };

            toGiveUpControl.CancelGiveUpBut.Click += (sender, e) =>
            {
                this.IsEnabled = true;
                wind.VisiableItems.Children.Clear();
            };
        }

        public void SetPlayerGaveUp(int playerIndex)
        {
            ClearDropDown();

            HideAllPlayersDepositCounters(playerIndex);

            //If player owes money (bus bill)
            GiveAllMoneyToAnotherPlayer(playerIndex);

            RepaintAllPlayersCells(playerIndex);
            _system.MonGame.PlayerGaveUp(playerIndex);
            SetPlayerCardIfHeGaveUp(_cards[playerIndex]);
            _cards[playerIndex].StopTimer();

            if (IsSomeOneWon()) return;

            RemoveLostPlayersChip(playerIndex);

            ChatMessages.Children.Clear();

            if (_ifBirthdayChance)
            {
                SetNextGifterOnBirthday(_system.MonGame.GetBirthdayToPayPlayersIndexes(_playersIndexesThatPaid));
                return;
            }
            ChangeStepper();
        }

        public void HideAllPlayersDepositCounters(int playerIndex)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IsCellIndexIsBusiness(i) &&
                    _system.MonGame.IsPlayerIsBusOwner(playerIndex, i))
                {
                    Helper.ChangeDepositCounterVisibility(_cells[i], Visibility.Hidden);
                }
            }
        }

        public void GiveAllMoneyToAnotherPlayer(int playerIndex)
        {
            if (_ifBirthdayChance)
            {
                _system.MonGame.GiveAllMoneyToStepper(playerIndex);
                UpdatePlayersMoney();

                //_cards[playerIndex].SetAnimation(null, false);
                //MakeEveryCardThinner
                return;
            }

            if (!_system.MonGame.IsStepperOnEnemiesBus()) return;
            _system.MonGame.GiveAllSteppersMoneyToBusOwner();
            UpdatePlayersMoney();
        }

        public void RemoveLostPlayersChip(int playerIndex)
        {
            Image toRemove = _images[playerIndex];

            toRemove.Visibility = Visibility.Hidden;

            Canvas can = ((Canvas)toRemove.Parent);
            can.Children.Remove(toRemove);

            _system.MonGame.Players[playerIndex].Position = -1;
            _images[playerIndex] = null;
        }

        public void SetPlayerCardIfHeGaveUp(UserCard card)
        {
            const double gaveUpOpacity = 0.3;
            card.Opacity = gaveUpOpacity;

            card.UserLoginCanvas.Visibility = Visibility.Hidden;
            card.UserMoneyAmountCanvas.Visibility = Visibility.Hidden;

            card.SkullImg.Visibility = Visibility.Visible;
        }

        public bool IsSomeOneWon()
        {
            if (!_system.MonGame.IsSomeOneWon() || _gameEnded) return false;

            StopTimers();
            int winnerIndex = _system.MonGame.GetWinnerIndex();
            SetEndGameControl(_cards[winnerIndex].UserImageGrid.Children.OfType<Image>().FirstOrDefault());
            return true;
        }

        public void SetEndGameControl(Image img)
        {
            if (img is null) return;
            const int justHighZIndex = 1000;
            MainWindow wind = (MainWindow)Window.GetWindow(_frame);

            ShowGotPrize endGameControl = new ShowGotPrize(img);

            //GameChat.Items.Add(endGameControl);

            wind.VisiableItems.Children.Add(endGameControl);

            Canvas.SetLeft(endGameControl, wind.ActualWidth / _centerDivider - endGameControl.Width / _centerDivider);
            Canvas.SetTop(endGameControl, wind.ActualHeight / _centerDivider - endGameControl.Height / _centerDivider);

            Canvas.SetZIndex(wind.VisiableItems, justHighZIndex);

            this.IsEnabled = false;
            endGameControl.AcceptBut.Click += (sender, e) =>
            {
                _gameEnded = true;
                wind.VisiableItems.Children.Clear();
                _frame.Content = new Pages.MainPage(_frame, _system);
            };
        }

        public void RepaintAllPlayersCells(int playerIndex)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IsCellIsPlayersBusiness(i, playerIndex))
                {
                    ClearCell(_cells[i], i);
                }
            }
        }

        //Repaint bg and clear stars
        public void ClearCell(UIElement el, int cellIndex)
        {
            //const int clearOpacity = 1;

            if (el is IClearCellVis vis)
            {
                vis.ClearCell();
            }

            /* if (el is UpperCell up)
             {
                 up.ImagePlacer.Background = Brushes.White;
                 up.ImagePlacer.Opacity = clearOpacity;
                 up.StarsGrid.Children.Clear();
             }
             else if (el is RightCell right)
             {
                 right.ImagePlacer.Background = Brushes.White;
                 right.ImagePlacer.Opacity = clearOpacity;
                 right.StarsGrid.Children.Clear();
             }
             else if (el is BottomCell bot)
             {
                 bot.ImagePlacer.Background = Brushes.White;
                 bot.ImagePlacer.Opacity = clearOpacity;
                 bot.StarsGrid.Children.Clear();
             }
             else if (el is LeftCell left)
             {
                 left.ImagePlacer.Background = Brushes.White;
                 left.ImagePlacer.Opacity = clearOpacity;
                 left.StarsGrid.Children.Clear();
             }*/
            ChangePriceInBusiness(cellIndex, _system.MonGame.GetBusPrice(cellIndex), true);
        }

        private int _tradeReceiveIndex;
        private TradeOfferEl _tradeOffer;
        private bool _ifAcceptancePhase = false;
        public void CreateTrade(int traderIndex)
        {
            _ifAcceptancePhase = false;
            ClearDropDown();
            _ifBlockMenus = true;

            if (ChatMessages.Children.OfType<TradeOfferEl>().Any()) return;
            _cards[_system.MonGame.StepperIndex].UpdateTimer();
            _tradeReceiveIndex = traderIndex;

            _system.MonGame.CreateTrade();
            _system.MonGame.SetTradeReceiverIndex(traderIndex);

            ClearClickInfoEventForBusCells();
            SetTradeMouseDownForBusses(true);

            _tradeOffer = new TradeOfferEl();

            _tradeOffer.SenderMoney.SetMaxMoney(_system.MonGame.GetTradeSenderMaxMoney());
            _tradeOffer.ReciverMoney.SetMaxMoney(_system.MonGame.GetTradeReceiverMaxMoney());

            _tradeOffer.GiverItem.ItemName.Text = _system.MonGame.GetStepperLogin();
            _tradeOffer.GiverItem.ItemType.Text = SystemParamsService.GetStringByName("TradeSenderLogin");

            _tradeOffer.ReciverItem.ItemName.Text = _system.MonGame.GetPlayerLoginByIndex(traderIndex);
            _tradeOffer.ReciverItem.ItemType.Text = SystemParamsService.GetStringByName("TradeReceiverLogin");

            _tradeOffer.CloseTrade.MouseDown += (sender, e) =>
            {
                _ifBlockMenus = false;

                _cards[_system.MonGame.StepperIndex].UpdateTimer();
                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());
            };

            _tradeOffer.SendTradeBut.Click += (sender, e) =>
            {
                if (!_system.MonGame.IsTradersHasEnoughMoney() ||
                !_system.MonGame.IsTwiceRuleInTradeComplete()) return;

                _ifAcceptancePhase = true;

                ClearEventsOnTradeAcceptance();

                AddMessageWithTwoPlayers(_system.MonGame.StepperIndex, _tradeReceiveIndex,
                    $"{SystemParamsService.GetStringByName("TradePlayerOffers")} ",
                    $"{SystemParamsService.GetStringByName("TradeToSignOffer")}");

                _tradeOffer.SendTradeBut.IsEnabled = false;
                SwipeUsersAnimation(_system.MonGame._trade.GetSenderIndex(), _system.MonGame._trade.GetReceiverIndex());

                _cards[_system.MonGame._trade.GetSenderIndex()]._horizontalAnimation.Completed += (sen, ev) =>
                {
                    _tradeOffer.SendTradeBut.Visibility = Visibility.Hidden;
                    _tradeOffer.AcceptanceButtons.Visibility = Visibility.Visible;
                    SetEventsForAcceptanceButsInTrade();
                };
            };

            SetEventsForMoneyBoxesInTrade();
            ChatMessages.Children.Add(_tradeOffer);
        }

        private void ClearEventsOnTradeAcceptance()
        {
            SetTradeMouseDownForBusses(false);

            _tradeOffer.SenderMoney.AmountOfMoneyBox.IsReadOnly = true;
            _tradeOffer.ReciverMoney.AmountOfMoneyBox.IsReadOnly = true;
        }

        private void SetEventsForAcceptanceButsInTrade()
        {
            _tradeOffer.AcceptTradeBut.Click += (sender, e) =>
            {
                _ifBlockMenus = false;

                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("TradeAccept")}",
                    _system.MonGame._trade.ReceiverIndex);

                MakeInActiveTradAnswerButs();
                _system.MonGame.AcceptTrade();

                UpdatePlayersMoney();
                RepaintAfterTradeBuses();



                CheckTradeBusesForInventoryBuses();




                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());

                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);

                UpdatePaymentLevelAfterTrade(_system.MonGame._trade.SenderIndex);
                UpdatePaymentLevelAfterTrade(_system.MonGame._trade.ReceiverIndex);

                SwipeUsersAnimation(_system.MonGame._trade.GetReceiverIndex(), _system.MonGame._trade.GetSenderIndex());

                UpdateChatMessages();
            };

            _tradeOffer.DeclineTradeBut.Click += (sender, e) =>
            {
                _ifBlockMenus = false;

                AddWrapPanelToChatBox($"{SystemParamsService.GetStringByName("TradeDecline")}", _system.MonGame._trade.ReceiverIndex);

                MakeInActiveTradAnswerButs();
                SwipeUsersAnimation(_system.MonGame._trade.GetReceiverIndex(), _system.MonGame._trade.GetSenderIndex());

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());

                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);
            };
        }

        private void UpdatePaymentLevelAfterTrade(int playerIndex)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IsCellIsGameOrCar(i) &&
                    _system.MonGame.IsPlayerOwnsBusiness(playerIndex, i) &&
                    !_system.MonGame.IsDeposited(i))
                {
                    SetLevelPayment(playerIndex, i);
                }
            }
        }

        private void MakeInActiveTradAnswerButs()
        {
            _tradeOffer.AcceptTradeBut.IsEnabled = false;
            _tradeOffer.DeclineTradeBut.IsEnabled = false;
        }

        private void CheckTradeBusesForInventoryBuses()
        {
            TradePerformance trade = _system.MonGame.GetTrade();

            //Clear senders and receivers busses
            ClearInventoriedBuses(trade.SenderBusinessesIndexes, trade.ReceiverIndex);
            ClearInventoriedBuses(trade.ReceiverBusinessesIndexes, trade.SenderIndex);
            //Set new Inventory cells for Sender and receiver

            SetPlayersInventoryItems(trade.ReceiverIndex, trade.SenderBusinessesIndexes);
            SetPlayersInventoryItems(trade.SenderIndex, trade.ReceiverBusinessesIndexes);

            SetStartRentForBuses(trade.SenderBusinessesIndexes);
            SetStartRentForBuses(trade.ReceiverBusinessesIndexes);
        }

        private void SetStartRentForBuses(List<int> cellIndexes)
        {
            for (int i = 0; i < cellIndexes.Count; i++)
            {
                if (_system.MonGame.GameBoard.Cells[cellIndexes[i]] is Business business &&
                    !business.IsDeposited)
                {
                    int startRent = _system.MonGame.GetStartPaymentForBusinessByIndex(cellIndexes[i]);
                    ChangePriceInBusiness(cellIndexes[i], startRent);
                }
            }
        }

        private void SetPlayersInventoryItems(int playerIndex, List<int> cellsIndexes)
        {
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                if (_system.MonGame.IsPlayerHasInventoryItemOnIndex(playerIndex, cellsIndexes[i]))
                {
                    _system.MonGame.SetInventoryItemForPlayer(playerIndex, cellsIndexes[i]);

                    Business bus = _system.MonGame.GetBusinessByIndex(cellsIndexes[i]);

                    SetImageImg(bus, playerIndex);
                }
            }
        }

        private void ClearInventoriedBuses(List<int> cellsIndexes, int newOwnerIndex)
        {
            for (int i = 0; i < cellsIndexes.Count; i++)
            {
                /*                if (cellsIndexes[i] == 3 &&
                                    _system.MonGame.GameBoard.Cells[cellsIndexes[i]] is RegularBusiness asd)
                                {
                                    Console.WriteLine(asd);
                                }*/

                ClearInventoryBusImage(cellsIndexes[i]);
                _system.MonGame.SetBasicBusBack(cellsIndexes[i]);
                _system.MonGame.GetBusinessByIndex(cellsIndexes[i]).ChangeOwner(newOwnerIndex);

                /*                if (cellsIndexes[i] == 3 &&
                                    _system.MonGame.GameBoard.Cells[cellsIndexes[i]] is RegularBusiness bus)
                                {
                                    Console.WriteLine(bus);
                                }*/

            }
        }

        private void ClearInventoryBusImage(int cellIndex)
        {
            UIElement cell = _cells[cellIndex];
            const int minChildrenAmount = 1;
            const int firstElemIndex = 0;

            Grid imagePlacer = cell is IRegularCellsActions intro ? intro.GetImagePlacer() : null;

            /*            Grid imagePlacer = cell is UpperCell up ? up.ImagePlacer :
                            cell is RightCell right ? right.ImagePlacer :
                            cell is BottomCell bot ? bot.ImagePlacer :
                            cell is LeftCell left ? left.ImagePlacer : null;*/

            if (imagePlacer is null || imagePlacer.Children.Count <= minChildrenAmount) return;
            imagePlacer.Children.RemoveAt(imagePlacer.Children.Count - 1);
            imagePlacer.Children[firstElemIndex].Visibility = Visibility.Visible;

            /*            if (cell is UpperCell up && up.ImagePlacer.Children.Count > minChildrenAmount)
                        {
                            up.ImagePlacer.Children.RemoveAt(up.ImagePlacer.Children.Count - 1);
                            up.ImagePlacer.Children[0].Visibility = Visibility.Visible;
                        }
                        else if (cell is RightCell right && right.ImagePlacer.Children.Count > minChildrenAmount)
                        {
                            right.ImagePlacer.Children.RemoveAt(right.ImagePlacer.Children.Count - 1);
                            right.ImagePlacer.Children[0].Visibility = Visibility.Visible;
                        }
                        else if (cell is BottomCell bot && bot.ImagePlacer.Children.Count > minChildrenAmount)
                        {
                            bot.ImagePlacer.Children.RemoveAt(bot.ImagePlacer.Children.Count - 1);
                            bot.ImagePlacer.Children[0].Visibility = Visibility.Visible;
                        }
                        else if (cell is LeftCell left && left.ImagePlacer.Children.Count > minChildrenAmount)
                        {
                            left.ImagePlacer.Children.RemoveAt(left.ImagePlacer.Children.Count - 1);
                            left.ImagePlacer.Children[0].Visibility = Visibility.Visible;
                        }*/
        }

        private void RepaintAfterTradeBuses()
        {
            PaintCellsAfterTrade(_system.MonGame._trade.SenderIndex,
                _system.MonGame._trade.GetReceiverIndexes());

            PaintCellsAfterTrade(_system.MonGame._trade.ReceiverIndex,
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

                _tradeOffer.UpdateSenderTotalMoney(MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetTotalMoneyForTrade(true)));
                //_tradeOffer.UpdateSenderTotalMoney(GetConvertedPrice(_system.MonGame.GetSenderTotalMoneyForTrade()));

            };

            _tradeOffer.ReciverMoney.AmountOfMoneyBox.TextChanged += (sender, e) =>
            {
                int receiverMoney = _tradeOffer.GetReceiverTradeMoney();
                _system.MonGame.SetReceiverMoneyTrade(receiverMoney);

                _tradeOffer.UpdateReceiverTotalMoney(MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetTotalMoneyForTrade(false)));
                //_tradeOffer.UpdateReceiverTotalMoney(GetConvertedPrice(_system.MonGame.GetReceiverTotalMoneyForTrade()));
            };
        }

        private void UpdateTradeMoneyBoxes()
        {
            _tradeOffer.UpdateSenderTotalMoney(MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetTotalMoneyForTrade(true)));
            _tradeOffer.UpdateReceiverTotalMoney(MoneyConvertingService.GetConvertedPrice(_system.MonGame.GetTotalMoneyForTrade(false)));

            /*            _tradeOffer.UpdateSenderTotalMoney(GetConvertedPrice(_system.MonGame.GetSenderTotalMoneyForTrade()));
                        _tradeOffer.UpdateReceiverTotalMoney(GetConvertedPrice(_system.MonGame.GetReceiverTotalMoneyForTrade()));*/
        }


        private void SetTradeMouseDownForBusses(bool isAddEvents)
        {
            for (int i = 0; i < _cells.Count; i++)
            {
                SetTradeMouseDownForCell(_cells[i], isAddEvents);
            }
        }

        public void SetTradeMouseDownForCell(UIElement element, bool isAdd)
        {
            if (element is IRegularCellsActions
                /*element is UpperCell || element is RightCell ||
                element is BottomCell || element is LeftCell*/)
            {
                if (isAdd) element.PreviewMouseDown += TradeBuss_PreviewMouseDown;
                else element.PreviewMouseDown -= TradeBuss_PreviewMouseDown;
            }

            /*
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
                        }*/
        }

        public void TradeBuss_PreviewMouseDown(object sender, EventArgs e)
        {
            int cellIndex = GetCellIndex((UIElement)sender);
            if (!_system.MonGame.IsCellIndexIsBusiness(cellIndex)) return; //cell is business
            if (_system.MonGame.IsOneOfTheBusesContainsHouse(cellIndex)) return; // bus 

            if (!_system.MonGame.IsStepperOwnsBusiness(cellIndex) &&
                !_system.MonGame.IsPlayerOwnsBusiness(_tradeReceiveIndex, cellIndex)) return; //Cell owner is sender or receiver 

            Business bus = _system.MonGame.GetBusinessByIndex(cellIndex);
            if (_tradeOffer.IsTradeItemNameContainsInList(bus.Name)) return; // such bus is already exist
            TradeItem item = new TradeItem(bus, GetLastChipImageFromBusCell(_cells[cellIndex]));

            _system.MonGame.AddBusinessInTrade(cellIndex);
            UpdateTradeMoneyBoxes();

            item.PreviewMouseDown += (senderItem, action) =>
            {
                if (_ifAcceptancePhase) return;
                if (senderItem is TradeItem tradeItem)
                {
                    _tradeOffer.SenderListBox.Items.Remove(tradeItem);
                    _tradeOffer.ReciverListBox.Items.Remove(tradeItem);

                    _system.MonGame.RemoveBusinessFromTrade(cellIndex);
                    UpdateTradeMoneyBoxes();
                }
            };

            AddTradeItemToListBox(item, _system.MonGame.IsStepperOwnsBusiness(cellIndex));
        }

        public void AddTradeItemToListBox(TradeItem item, bool isAddToSender)
        {
            if (isAddToSender) _tradeOffer.SenderListBox.Items.Add(item);
            else _tradeOffer.ReciverListBox.Items.Add(item);
        }

        private Image GetLastChipImageFromBusCell(UIElement element)
        {
            if (element is IGetLastChipImage inter)
            {
                return inter.GetLastChipImageFromBusCell();
            }

            /*            if (element is UpperCell upper)
                            return upper.ImagePlacer.Children.OfType<Image>().Last();

                        if (element is RightCell right)
                            return right.ImagePlacer.Children.OfType<Image>().Last();

                        if (element is BottomCell bottom)
                            return bottom.ImagePlacer.Children.OfType<Image>().Last();

                        if (element is LeftCell left)
                            return left.ImagePlacer.Children.OfType<Image>().Last();*/

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
            element.PreviewMouseDown -= Business_PreviewMouseDown;

            /*            if (element is UpperCell upper)
                            upper.PreviewMouseDown -= Business_PreviewMouseDown;

                        if (element is RightCell right)
                            right.PreviewMouseDown -= Business_PreviewMouseDown;

                        if (element is BottomCell bottom)
                            bottom.PreviewMouseDown -= Business_PreviewMouseDown;

                        if (element is LeftCell left)
                            left.PreviewMouseDown -= Business_PreviewMouseDown;*/
        }

        public void UpdateDepositCounters()
        {
            _system.MonGame.SetNewDepositCircle();
            for (int i = 0; i < _cells.Count; i++)
            {
                if (_system.MonGame.IsDeposited(i))
                {
                    Helper.SetValueForDepositCounter(_cells[i], _system.MonGame.GetDepositCounter(i));
                    if (_system.MonGame.IsBusDepositCounterIsZero(i))
                    {
                        GameChat.Items.Add(GetTextBlockForMessage($"{_system.MonGame.GetBusNameOnGivenIndex(i)} " +
                            $"{SystemParamsService.GetStringByName("BusGoesToBank")}"));

                        ClearCell(_cells[i], i);
                        _system.MonGame.ClearBusiness(i);
                        Helper.ChangeDepositCounterVisibility(_cells[i], Visibility.Hidden);

                        ClearInventoryBusImage(i);
                        _system.MonGame.SetBasicBusBack(i);

                        _system.MonGame.GameBoard.ClearBusinessOwner(i);
                    }
                }
            }
        }

        //private const int _fieldSizeParam = 895;

        private const int _middleSizeSquareParam = 115;
        private const int _middleTopBotWidth = 55;
        private const int _middleTopBotHeight = 140;

        private const int _middlePrisonImagesSize = 40;

        private const int _baseSquareSizeParam = 125;
        private const int _baseTopBotWidth = 65;
        private const int _baseTopBotHeight = 150;

        private Size GetSquareCellSize(GamePageSize sizeType)
        {
            return sizeType == GamePageSize.MediumWindow ? new Size(_middleSizeSquareParam, _middleSizeSquareParam) :
                new Size(_baseSquareSizeParam, _baseSquareSizeParam);
        }

        private Size GetTopBotCelSize(GamePageSize sizeType)
        {
            return sizeType == GamePageSize.MediumWindow ? new Size(_middleTopBotWidth, _middleTopBotHeight) :
                new Size(_baseTopBotWidth, _baseTopBotHeight);
        }

        private Size GetLeftRightSize(GamePageSize sizeType)
        {
            return sizeType == GamePageSize.MediumWindow ? new Size(_middleTopBotHeight, _middleTopBotWidth) :
                new Size(_baseTopBotHeight, _baseTopBotWidth);
        }

        private void SetFieldSize(Size size)
        {
            this.Width = size.Width;
            this.Height = size.Height;
        }

        private void SetSizeOfRowAndCols(GamePageSize size)
        {
            /*            UpperRow.Height = new GridLength(size == GamePageSize.MediumWindow ?
                            _middleTopBotHeight : _baseTopBotHeight);

                        BottomRow.Height = new GridLength(size == GamePageSize.MediumWindow ?
                            _middleTopBotHeight : _baseTopBotHeight);

                        LeftColumn.Width = new GridLength(size == GamePageSize.MediumWindow ?
                            _middleTopBotHeight : _baseTopBotHeight);

                        RightColumn.Width = new GridLength(size == GamePageSize.MediumWindow ?
                            _middleTopBotHeight : _baseTopBotHeight);*/

            SetMiddleSizeParams(size);

            //9 - cells in row
            //10 - little margin in each cell
            int centerParam = (size == GamePageSize.MediumWindow ? _middleTopBotWidth : _baseTopBotWidth) * _busesInLine + _totalLeftMargin;

            MiddleRow.Height = new GridLength(centerParam);
            MiddleColumn.Width = new GridLength(centerParam);
        }

        public void SetMiddleSizeParams(GamePageSize size)
        {
            List<DefinitionBase> definitions = GetRowAndColsToChange();

            for (int i = 0; i < definitions.Count; i++)
            {
                if (definitions[i] is RowDefinition row)
                {
                    row.Height = new GridLength(size == GamePageSize.MediumWindow ?
                        _middleTopBotHeight : _baseTopBotHeight);
                }
                else if (definitions[i] is ColumnDefinition col)
                {
                    col.Width = new GridLength(size == GamePageSize.MediumWindow ?
                        _middleTopBotHeight : _baseTopBotHeight);
                }
            }
        }

        public List<DefinitionBase> GetRowAndColsToChange()
        {
            List<DefinitionBase> res = new List<DefinitionBase>();

            res.Add(UpperRow);
            res.Add(BottomRow);
            res.Add(LeftColumn);
            res.Add(RightColumn);
            return res;
        }

        private double _newCellSizeParam;

        private const int _busesInLine = 9;
        private const int _totalLeftMargin = 10;

        public void UpdateFieldSize(GamePageSize sizeType, Size newFieldSize)
        {
            List<UIElement> squares = GetSquares(); //Prison cell is other type
            List<UIElement> topBotElems = GetTopBotCells();
            List<UIElement> leftRightElems = GetLeftRightCells();

            //Change Cell size
            ChangeSizeForSquareElement(squares, GetSquareCellSize(sizeType));
            ChangeTopBotCellSize(topBotElems, GetTopBotCelSize(sizeType));
            ChangeLeftRightSize(leftRightElems, GetLeftRightSize(sizeType));

            //Other params 
            SetFieldSize(newFieldSize); //Set field size
            SetSizeOfRowAndCols(sizeType); //set cells set cell size
                                           //
                                           //Set size of cells
            _newCellSizeParam = (MiddleColumn.Width.Value - _totalLeftMargin) / _busesInLine;
            ChangeSizeForUpperCells(topBotElems);
            ChangeSizeForLeftRightCells(leftRightElems);
            ChangeSizeForSquares(squares, sizeType);

            //Set size for user cards
            ChangeUserCardsSize(sizeType);
        }

        /*        private readonly Size _basicCardGridSize = new Size(220, 165);
                private readonly Size _middleCardsGridSize = new Size(200, 145);

                private readonly Size _basicCardSize = new Size(250, 200);
                private readonly Size _middleCardSize = new Size(230, 180);*/

        private void ChangeUserCardsSize(GamePageSize windowSize)
        {
            //Size userGridSize = windowSize == GamePageSize.BigWindow ? _basicCardGridSize : _middleCardsGridSize;

            //Size size = windowSize == GamePageSize.BigWindow ? _basicCardSize : _middleCardSize;

            int sizeParam = windowSize == GamePageSize.BigWindow ?
                       _cards.First()._toMakeBigger : _cards.First()._toMakeThinner;

            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].Width += sizeParam;
                _cards[i].Height += sizeParam;

                /*                if (_cards[i].IsUserCardBgIsUsual())
                                {
                                    _cards[i].Width += sizeParam;
                                    _cards[i].Height += sizeParam;

                                }
                                else if (!_cards[i].IsUserCardBgIsUsual())
                                {
                                    _cards[i].Width += sizeParam;
                                    _cards[i].Height += sizeParam;
                                }*/
            }
        }

        private void ChangeSizeForSquares(List<UIElement> elems, GamePageSize size)
        {
            int sizeParam = size == GamePageSize.BigWindow ? _baseSquareSizeParam : _middleSizeSquareParam;

            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is SquareCell square)
                {
                    square.Height = sizeParam;
                    square.Width = sizeParam;

                    SetSizeForCanvasInSquare(square.ImagesPlacer, sizeParam);
                    SetSizeForCanvasInSquare(square.ChipsPlacer, sizeParam);
                    SetSetSizeForImagesInCanvas(square.ImagesPlacer, new Size(sizeParam, sizeParam));
                }
                else if (elems[i] is PrisonCell prison)
                {
                    prison.Height = sizeParam;
                    prison.Width = sizeParam;

                    SetSizeForCanvasInSquare(prison.ChipsPlacerVisit, sizeParam);
                    SetSizeForCanvasInSquare(prison.ChipsPlacerSitters, sizeParam);
                    SetSizeForCanvasInSquare(prison.ChipsPlacer, sizeParam);
                    SetSizeForCanvasInSquare(prison.ImagesPlacer, sizeParam);

                    UpdatePrisonImagesSize(prison.ImagesPlacer, size);
                }
            }
        }

        private void UpdatePrisonImagesSize(Canvas imagePlacer, GamePageSize size)
        {
            int sizeParam = size == GamePageSize.BigWindow ? _basePrisonImagesSize : _middlePrisonImagesSize;

            for (int i = 0; i < imagePlacer.Children.Count; i++)
            {
                if (imagePlacer.Children[i] is Image img)
                {
                    SetImageSize(img, new Size(sizeParam, sizeParam));
                    /*                  img.Width = sizeParam;
                                      img.Height = sizeParam;*/
                }
            }
        }

        private void SetSizeForCanvasInSquare(Canvas canvas, int sizeParam)
        {
            canvas.Width = sizeParam;
            canvas.Height = sizeParam;
        }

        private void SetImageSize(Image img, Size size)
        {
            img.Width = size.Width;
            img.Height = size.Height;
        }

        private void SetSetSizeForImagesInCanvas(Canvas can, Size size)
        {
            for (int i = 0; i < can.Children.Count; i++)
            {
                if (can.Children[i] is Image img)
                {
                    SetImageSize(img, size);
                    /*                    img.Width = size.Width;
                                        img.Height = size.Height;*/
                }
            }
        }

        private void ChangeSizeForLeftRightCells(List<UIElement> elems)
        {
            ChangeLeftRightSize(elems, new Size(LeftColumn.Width.Value, _newCellSizeParam));
            /*            for (int i = 0; i < elems.Count; i++)
                        {
                            if (elems[i] is LeftCell left)
                            {
                                left.Height = _newCellSizeParam;
                                left.Width = LeftColumn.Width.Value;
                            }
                            else if (elems[i] is RightCell right)
                            {
                                right.Height = _newCellSizeParam;
                                right.Width = LeftColumn.Width.Value;
                            }
                        }*/
        }

        private void ChangeLeftRightSize(List<UIElement> elems, Size size)
        {
            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is FrameworkElement elem &&
                    (elems[i] is LeftCell || elems[i] is RightCell)) SetElementSize(elem, size);
                /*                {
                                    elem.Width = size.Width;
                                    elem.Height = size.Height;
                                }*/
                /*                if (elems[i] is LeftCell left)
                                {

                                    left.Width = size.Width;
                                    left.Height = size.Height;
                                }
                                else if (elems[i] is RightCell right)
                                {
                                    right.Width = size.Width;
                                    right.Height = size.Height;
                                }*/
            }
        }


        private void ChangeSizeForUpperCells(List<UIElement> elems)
        {
            ChangeTopBotCellSize(elems, new Size(_newCellSizeParam, UpperRow.Height.Value));
            /*for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is UpperCell up)
                {
                    up.Height = UpperRow.Height.Value;
                    up.Width = _newCellSizeParam;
                }
                else if (elems[i] is BottomCell bot)
                {
                    bot.Height = BottomRow.Height.Value;
                    bot.Width = _newCellSizeParam;
                }
            }*/
        }

        public void SetElementSize(FrameworkElement elem, Size size)
        {
            elem.Width = size.Width;
            elem.Height = size.Height;
        }

        private void ChangeTopBotCellSize(List<UIElement> elems, Size size)
        {
            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is FrameworkElement elem &&
                    (elems[i] is UpperCell || elems[i] is BottomCell)) SetElementSize(elem, size);
                /*                {
                                    elem.Width = size.Width;
                                    elem.Height = size.Height;
                                }*/

                /* if (elems[i] is UpperCell upper)
                 {
                     upper.Width = size.Width;
                     upper.Height = size.Height;
                 }
                 else if (elems[i] is BottomCell bottom)
                 {
                     bottom.Width = size.Width;
                     bottom.Height = size.Height;
                 }*/
            }
        }

        private void ChangeSizeForSquareElement(List<UIElement> elems, Size size)
        {
            for (int i = 0; i < elems.Count; i++)
            {
                if (elems[i] is IChangeCellSize inter)
                {
                    inter.ChangeCellSize(size);
                }


                /*                if (elems[i] is SquareCell square)
                                {
                                    square.Width = size.Width;
                                    square.Height = size.Height;
                                }
                                else if (elems[i] is PrisonCell prison)
                                {
                                    prison.Width = size.Width;
                                    prison.Height = size.Height;
                                }*/
            }
        }

        public List<UIElement> GetLeftRightCells()
        {
            return _cells.Where(x => x is LeftCell || x is RightCell).ToList();

            /*            List<UIElement> res = new List<UIElement>();
                        for (int i = 0; i < _cells.Count; i++)
                        {
                            if (_cells[i] is LeftCell ||
                                _cells[i] is RightCell)
                            {
                                res.Add(_cells[i]);
                            }
                        }
                        return res;*/
        }

        public List<UIElement> GetTopBotCells()
        {
            return _cells.Where(x => x is UpperCell || x is BottomCell).ToList();

            /*            List<UIElement> res = new List<UIElement>();
                        for (int i = 0; i < _cells.Count; i++)
                        {
                            if (_cells[i] is UpperCell ||
                                _cells[i] is BottomCell)
                            {
                                res.Add(_cells[i]);
                            }
                        }
                        return res;*/
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

        public void StopTimers()
        {
            for (int i = 0; i < _cards.Count; i++)
            {
                if (!(_cards[i].UserTimer._timer is null))
                {
                    _cards[i].StopTimer();
                }
            }
        }

        public void ClearDropDown()
        {
            if (_frame.Content is GamePage game)
            {
                game._dropdownMenuPopup.IsOpen = false;
            }
        }
    }
}