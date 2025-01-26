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
                    break;
            }
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


            drop._first3dCube._horizontalAnimation.Completed += CubesDroped_Complited;

            ChatMessages.Children.Add(drop);

            //MakeAMoveByChip();
        }

        private void CubesDroped_Complited(object sender, EventArgs e)
        {
            MakeChipsMovementAction(_system.MonGame.GetStepperPosition(),
                _system.MonGame.GetPointToMoveOn(), _imgs[_system.MonGame.StepperIndex]);

            //Change temp point 
            _system.MonGame.SetPlayerPosition();

            //Need to understand which cell is Cell On
            //Get enum action to show whats is happening

            _chipMoveAnimation.Completed += (compSender, eve) =>
            {
                //Remove dice 
                ChatMessages.Children.Remove(ChatMessages.Children.OfType<DicesDrop>().First());

                CellAction action = _system.MonGame.GetAction();
                SetActionVisuals(action);
            };

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
                    break;
                case CellAction.SitInPrison:
                    break;
                case CellAction.VisitPrison:
                    break;
            }
            //trade
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
            MakeChipsMovementAction(_system.MonGame.GetStepperPosition(), cellIndex, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(cellIndex);
        }

        private void GoToPrisonFromChance(int cellIndexToMoveOn)
        {
            MakeChipsMovementAction(_system.MonGame.GetStepperPosition(), 10, _imgs[_system.MonGame.StepperIndex]);

            //MakeAMoveToInPrisonCell(true, _imgs[_system.MonGame.StepperIndex]);
            _system.MonGame.SetPlayerPositionAfterChanceMove(cellIndexToMoveOn);

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
            };

            JackPotCell.PreviewMouseDown += (sender, e) =>
            {
                ChatMessages.Children.Remove(jackpot);
                ChatMessages.Children.Add(jackpot);
            };
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
                PaintCellInColor(_system.MonGame.GetStepperPosition(), _colors[_system.MonGame._bidderIndex]);
                AddMessageTextBlock($"Business is buying = - {_system.MonGame.GetBidderLogin()}");

                //Get money form winner 
                _system.MonGame.GetMoneyFromAuctionWinner();

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
                string message = $"{_system.MonGame.GetBidderLogin()} doesnt wanna buy business";
                AddMessageTextBlock(message);

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<AuctionBid>().First());

                if (IfAuctionIsEnded()) return;
                if (_system.MonGame.RemoveAuctionBidderIfItsWasLast()) ;
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

            info.NameBusBorder.Background = GetColorForUsusalBusHeader(bus);
        }

        public SolidColorBrush GetColorForUsusalBusHeader(UsualBus bus)
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

            if (!(_squareIndexesToGoThrough is null))
            {
                cellIndexToMoveOn = _squareIndexesToGoThrough.First();
            }

            //const int moveDist = 10;
            //Image tempChipImg = _imgs[0];

            Point chipToFieldPoint = GetChipImageLocToField(chipImage);

            Point insidePointStartCell = new Point(Canvas.GetLeft(chipImage), Canvas.GetTop(chipImage));
            Point newInsideChipPoint = GetInsidePointToStepOn(_cells[cellIndexToMoveOn]);

            //Remove from cell
            ((Canvas)chipImage.Parent).Children.Remove(chipImage);

            //Add chip tom chipMove canvas
            AddChipToChipMovePanel(chipImage, chipToFieldPoint);

            //make move action
            MakeChipMove(startCellIndex, cellIndexToMoveOn, chipImage,
                insidePointStartCell, newInsideChipPoint);
            //MakeChipMoveToAnoutherCell(startCellIndex, cellIndexToMoveOn, chipImage, insidePointStartCell, newInsideChipPoint);


            //Reassign chip image to new cell (ON ANIMATION COMPLETE EVENT)
            //ReassignChipImageInNewCell(moveDist, tempChipImg, newInsideChipPoint);

            //Change Chips In cell
            SetNewPositionsToChipsInCell(startCellIndex);

        }

        private Point GetChipImageLocToField(Image img)
        {
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
                    ReassignChipImageInNewCell(_tempSquareValToGoOn, _imgs[0],
                        BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn]));

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

                        MakeChipMoveToAnoutherCell(toRemoveSquareIndex, _tempSquareValToGoOn, chipImg,
                        check, BoardHelper.GetCenterOfTheSquareForIamge(chipImg, _cells[_tempSquareValToGoOn]));

                        if (IfLastMoveIsPrison && _squareIndexesToGoThrough.Count == 1)
                        {
                            IfLastMoveIsPrison = false;
                            MakeAMoveToInPrisonCell(true, chipImg);
                        }

                    }
                };
                MakeChipMoveToAnoutherCell(startCellIndex, _tempSquareValToGoOn, chipImg,
                    insidePointStartCell, newInsideChipPoint);

            }
            else
            {
                if (_cells[cellIndexToMoveOn] is PrisonCell) IfLastMoveIsPrison = true;

                //usualMove
                MakeChipMoveToAnoutherCell(startCellIndex, cellIndexToMoveOn, chipImg, insidePointStartCell, newInsideChipPoint);
            }
        }

        //To Make a move in prison cell(sitter or visiter)
        private void MakeAMoveToInPrisonCell(bool ifVisiter, Image movedChip)
        {
            if (ifVisiter)
            {
                PrisonCell.ChipsPlacer.Children.Remove(movedChip);
                PrisonCell.ChipsPlacerVisit.Children.Add(movedChip);

                List<Point> points = BoardHelper.GetPointsForPrisonCellExcurs(PrisonCell.ChipsPlacerVisit.Children.Count,
                    new Size(PrisonCell.Width, PrisonCell.Height));

                SetChipsImagesInMovePanel(points, PrisonCell.ChipsPlacerVisit, 10);
            }
        }


        private void SetNewPositionsToChipsInCell(int cellIndex)
        {
            Canvas can = BoardHelper.GetChipCanvas(_cells[cellIndex]);

            //new points for chips
            List<Point> newChipsPoints = BoardHelper.GetPointsForChips(
                new Size(can.ActualWidth, can.ActualHeight), can.Children.OfType<Image>().ToList().Count);

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
                    SetChipToMoveAnimation(img, chipPointDiffer, cellIndex, points[i]);
                }
            }
        }

        private void ReassignChipImageInNewCell(int newCellIndex, Image chipImage, Point newCellInsideLoc)
        {
            ChipMovePanel.Children.Remove(chipImage);

            UIElement cellToMoveOn = _cells[newCellIndex];
            Canvas chipCan = BoardHelper.GetChipCanvas(cellToMoveOn);
            chipCan.Children.Add(chipImage);

            Canvas.SetLeft(chipImage, newCellInsideLoc.X);
            Canvas.SetTop(chipImage, newCellInsideLoc.Y);

            chipImage.RenderTransform = null;
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
            Point newCellPoint = BoardHelper.GetChipCanvas(cellToMoveOn).PointToScreen(new Point(0, 0));
            Point startcCell = BoardHelper.GetChipCanvas(_cells[startCellIndex]).PointToScreen(new Point(0, 0));

            Point pointToStepOn = new Point(newCellPoint.X - startcCell.X + newCellInsideChipPoint.X - prevCellInsideChipPoint.X,
                newCellPoint.Y - startcCell.Y + newCellInsideChipPoint.Y - prevCellInsideChipPoint.Y);


            //Set chip amimation 
            SetChipToMoveAnimation(chip, pointToStepOn, movePoint, newCellInsideChipPoint);
        }

        private Point GetInsidePointToStepOn(UIElement cellToMoveOn)
        {
            //amount of chips in cell to step on
            int amountOfChipsInCell = BoardHelper.GetAmountOfItemsInCell(cellToMoveOn);

            //Size of cell to step on
            Size cellSize = BoardHelper.GetSizeOfCell(cellToMoveOn);

            //Get Points for chips to place on cel to step on (need to replace all chips)
            List<Point> chipPoints = BoardHelper.GetPointsForChips(cellSize, amountOfChipsInCell);

            return chipPoints.Last();
        }

        private bool _ifChipMoves = false;
        private DoubleAnimation _chipMoveAnimation;

        private void SetChipToMoveAnimation(Image toMove, Point endPoint,
            int cellIndex, Point newInsideChipPoint)
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

            if (_ifGoesThroughSquareCell && toMove.Name == "One") // tempMove Chip Img
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
                        MakeAMoveToInPrisonCell(true, toMove);
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

        }

        public void SendTrade(int traderIndex)
        {
            if (ChatMessages.Children.OfType<TradeOfferEl>().Any()) return;
            
            ClearClickInfoEventForBusCells();
            SetTradeMouseDownForBusses(true);

            TradeOfferEl tradeControl = new TradeOfferEl();

            tradeControl.GiverItem.ItemName.Text = _system.MonGame.GetStepperLogin();
            tradeControl.GiverItem.ItemType.Text = "Sender Login";

            tradeControl.ReciverItem.ItemName.Text = _system.MonGame.GetPlayerLoginByIndex(traderIndex);
            tradeControl.ReciverItem.ItemType.Text = "Reciever login";

            tradeControl.CloseTrade.MouseDown += (sender, e) =>
            {
                SetClickEventForBusCells();
                SetTradeMouseDownForBusses(false);

                ChatMessages.Children.Remove(ChatMessages.Children.OfType<TradeOfferEl>().First());
            };

            ChatMessages.Children.Add(tradeControl);
        }

        private void SetTradeMouseDownForBusses(bool ifAddEvents)
        {
            for(int i = 0; i < _cells.Count; i++)
            {
                SetTradeMouseDownForCell(_cells[i], ifAddEvents);
            }
        }

        public void SetTradeMouseDownForCell(UIElement element, bool ifAdd)
        {
            if (element is UpperCell upper)
            {
                if(ifAdd) upper.PreviewMouseDown += TradeBuss_PreviewMouseDown;
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
           //sender - card(up, right, bootom or left)
           //set it as a trdae element 
           //set money counter
           //set money wirter
           

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
