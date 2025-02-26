using MonopolyDLL.Monopoly;
using MonopolyDLL;
using MonopolyEntity.VisualHelper;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Net.Mail;
using MahApps.Metro.Controls;
using System;
using MahApps.Metro.Native;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Documents;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для SetPlayersInGame.xaml
    /// </summary>
    public partial class SetPlayersInGame : Page
    {
        List<User> _users;
        MonopolySystem _system;
        Frame _frame;

        List<User> _playersInGame;
        List<User> _notAddedPlayers;
        public SetPlayersInGame(MonopolySystem system, Frame frame)
        {
            _users = DBQueries.GetAllPlayers();
            _system = system;
            _frame = frame;
            InitializeComponent();

            FillStartListBoxes();
        }

        public void FillStartListBoxes()
        {
            FillStartPlayersLists();
            SetLoginsInLists();
        }

        public void FillStartPlayersLists()
        {
            _notAddedPlayers = _users.Where(x => x.Login != _system.LoggedUser.Login).ToList();
            _playersInGame = _users.Where(x => x.Login == _system.LoggedUser.Login).ToList();
        }

        private void SetLoginsInLists()
        {
            PlayersInGame.Children.Clear();
            PlayersThatCanBeAdd.Children.Clear();

            AddLoginsInBox(_playersInGame, PlayersInGame);
            AddLoginsInBox(_notAddedPlayers, PlayersThatCanBeAdd);
        }

        private const int _itemsCut = 100;
        private void AddLoginsInBox(List<User> users, Canvas box)
        {
            //box.PreviewDrop += ListBox_Drop;
            for (int i = 0; i < users.Count; i++)
            {
                WrapPanel panel = new WrapPanel()
                {
                    Width = this.Width / 2 - _itemsCut,
                    Background = Brushes.Transparent,
                    Orientation = Orientation.Horizontal,
                };

                panel.MouseDown += MoveWarpPanel_MouseDown;
                panel.PreviewMouseMove += ToDrag_MouseMove;
                panel.PreviewMouseUp += ToDrag_MouseUp;

                panel.Children.Add(MainWindowHelper.GetCircleImage(50, 50, DBQueries.GetPictureNameById(users[i].GetPictureId())));

                panel.Children.Add(GetTextBlockForCard(users[i].Login));

                box.Children.Add(panel);               
            }
            UppdateHeightLocForItems(box);
        }

        private void MoveWarpPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _toDragDrop = (WrapPanel)sender;
            _panelOwner = (Canvas)((WrapPanel)sender).Parent;

            Point point = _toDragDrop.TransformToAncestor(this).Transform(new Point(0, 0));

            _panelOwner.Children.Remove(_toDragDrop);

            DragCanvas.Children.Add(_toDragDrop);

            Canvas.SetLeft(_toDragDrop, point.X);
            Canvas.SetTop(_toDragDrop, point.Y);
        }

        private void ToDrag_MouseMove(object sedner, MouseEventArgs e)
        {
            if (_toDragDrop is null) return;
            Point point = e.GetPosition(DragCanvas);

            Canvas.SetLeft(_toDragDrop, point.X - 20);
            Canvas.SetTop(_toDragDrop, point.Y - 20);
        }

        private void ToDrag_MouseUp(object sender, MouseEventArgs e)
        {
            if (_toDragDrop is null || _panelOwner is null) return;
            DragCanvas.Children.Remove(_toDragDrop);

            PlayersInGame.Children.Clear();
            PlayersThatCanBeAdd.Children.Clear();

            Point point = e.GetPosition(this);
            HitTestResult hitTestResult = VisualTreeHelper.HitTest(this, point);

            if (hitTestResult.VisualHit is Canvas can)
            {
                if (can == PlayersInGame)
                {
                    PlayersInGame.Children.Add(_toDragDrop);
                }
                else if (can == PlayersThatCanBeAdd)
                {
                    PlayersThatCanBeAdd.Children.Add(_toDragDrop);
                }
            }

            if (_toDragDrop.Parent is null)
            {
                _panelOwner.Children.Add(_toDragDrop);
            }

            UppdateHeightLocForItems(PlayersInGame);
            UppdateHeightLocForItems(PlayersThatCanBeAdd);


            SetDraggedPlayerInList(GetListToWorkWith(_panelOwner), GetListToWorkWith((Canvas)_toDragDrop.Parent));

            DragCanvas.Children.Clear();
            SetLoginsInLists();

            _toDragDrop = null;
            _panelOwner = null;
        }

        private const int _userItemHeight = 55;
        public void UppdateHeightLocForItems(Canvas can)
        {
            for (int i = 0; i < can.Children.Count; i++)
            {
                Canvas.SetTop(can.Children[i], _userItemHeight * i);
                Canvas.SetLeft(can.Children[i], 0);
            }
        }

        private TextBlock GetTextBlockForCard(string userLogin)
        {
            TextBlock block = GetTextBlock(userLogin);
            block.VerticalAlignment = VerticalAlignment.Center;
            block.Margin = new Thickness(5, 0, 0, 0);

            return block;
        }

        private WrapPanel _checkWarpDarg;


        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            UppdateHeightLocForItems((Canvas)sender);

            SetDraggedPlayerInList(GetListToWorkWith(_panelOwner), GetListToWorkWith((Canvas)sender));

        }

        public List<User> GetListToWorkWith(Canvas box)
        {
            return box == PlayersInGame ? _playersInGame : _notAddedPlayers;
        }

        public void SetDraggedPlayerInList(List<User> toRemoveFrom, List<User> toAddIn)
        {
            TextBlock block = _toDragDrop.Children.OfType<TextBlock>().FirstOrDefault();

            if (_panelOwner == PlayersInGame)
            {
                User inGame = toRemoveFrom.Where(x => x.Login == block.Text).First();
                toRemoveFrom.Remove(inGame);
                toAddIn.Add(inGame);
                return;
            }
            //owner is PlayersThatCanBeAdd
            User notInGame = toRemoveFrom.Where(x => x.Login == block.Text).First();
            toRemoveFrom.Remove(notInGame);
            toAddIn.Add(notInGame);
        }

        private WrapPanel _toDragDrop;
        private Canvas _panelOwner;

        private TextBlock GetTextBlock(string name)
        {
            return new TextBlock()
            {
                Text = name,
                FontSize = 16
            };
        }

        private void GetBackBut_Click(object sender, RoutedEventArgs e)
        {
            //Close();
        }


        private void StartGameBut_Click(object sender, RoutedEventArgs e)
        {
            const int leastAmountOfPlayers = 2;
            const int _maxAmountofPlayers = 5;
            if (!_users.Any(x => x.Login == _system.LoggedUser.Login) ||
                _playersInGame.Count < leastAmountOfPlayers ||
                _playersInGame.Count > _maxAmountofPlayers) return;


            _system.MonGame.Players = _playersInGame;

            GamePage page = new GamePage(_frame, _system);
            _frame.Content = page;

            //Close();
        }

        private void Page_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Page_DragOver(object sender, DragEventArgs e)
        {

        }

    }
}
