using MonopolyDLL.Monopoly;
using MonopolyDLL;
using MonopolyEntity.VisualHelper;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.CodeDom;

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
        private const int _centerDivider = 2;
        private void AddLoginsInBox(List<User> users, Canvas box)
        {
            //box.PreviewDrop += ListBox_Drop;
            const int imageSizeParam = 50;
            for (int i = 0; i < users.Count; i++)
            {
                WrapPanel panel = new WrapPanel()
                {
                    Width = this.Width / _centerDivider - _itemsCut,
                    Background = Brushes.Transparent,
                    Orientation = Orientation.Horizontal,
                };

                panel.MouseDown += MoveWarpPanel_MouseDown;
                panel.PreviewMouseMove += ToDrag_MouseMove;
                panel.PreviewMouseUp += ToDrag_MouseUp;

                panel.Children.Add(MainWindowHelper.GetCircleImage(imageSizeParam, imageSizeParam,
                    DBQueries.GetPictureNameById(users[i].GetPictureId())));

                panel.Children.Add(GetTextBlockForCard(users[i].Login));

                box.Children.Add(panel);               
            }
            UpdateHeightLocForItems(box);
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

        private const int _mouseMoveParam = 20;
        private void ToDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (_toDragDrop is null) return;
            Point point = e.GetPosition(DragCanvas);

            Canvas.SetLeft(_toDragDrop, point.X - _mouseMoveParam);
            Canvas.SetTop(_toDragDrop, point.Y - _mouseMoveParam);
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

            UpdateHeightLocForItems(PlayersInGame);
            UpdateHeightLocForItems(PlayersThatCanBeAdd);

            SetDraggedPlayerInList(GetListToWorkWith(_panelOwner), 
                GetListToWorkWith((Canvas)_toDragDrop.Parent));

            DragCanvas.Children.Clear();
            SetLoginsInLists();

            _toDragDrop = null;
            _panelOwner = null;
        }

        private const int _userItemHeight = 55;
        public void UpdateHeightLocForItems(Canvas can)
        {
            for (int i = 0; i < can.Children.Count; i++)
            {
                Canvas.SetTop(can.Children[i], _userItemHeight * i);
                Canvas.SetLeft(can.Children[i], 0);
            }
        }

        private TextBlock GetTextBlockForCard(string userLogin)
        {
            const int blockLeftMargin = 5;
            TextBlock block = GetTextBlock(userLogin);
            block.VerticalAlignment = VerticalAlignment.Center;
            block.Margin = new Thickness(blockLeftMargin, 0, 0, 0);

            return block;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            UpdateHeightLocForItems((Canvas)sender);

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
            const int _maxAmountOfPlayers = 5;
            if (!_users.Any(x => x.Login == _system.LoggedUser.Login) ||
                _playersInGame.Count < leastAmountOfPlayers ||
                _playersInGame.Count > _maxAmountOfPlayers) return;

            _system.MonGame.Players = _playersInGame;
            _frame.Content = new GamePage(_frame, _system);;
        }

        private void Page_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void Page_DragOver(object sender, DragEventArgs e)
        {

        }

    }
}
