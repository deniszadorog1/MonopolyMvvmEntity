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
            box.PreviewDrop += ListBox_Drop;
            for (int i = 0; i < users.Count; i++)
            {
                WrapPanel panel = new WrapPanel()
                {
                    Width = this.Width / 2 - _itemsCut,
                    Background = Brushes.Transparent,
                    Orientation = Orientation.Horizontal,
                };

                panel.MouseDown += MoveWarpPanel_MouseDown;

                panel.Children.Add(MainWindowHelper.GetCircleImage(50, 50, DBQueries.GetPictureNameById(users[i].GetPictureId())));

                panel.Children.Add(GetTextBlockForCard(users[i].Login));

                box.Children.Add(panel);

                panel.MouseMove += Item_MouseMove;
            }

            UppdateHeightLocForItems(box);
        }

        private void Item_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton is MouseButtonState.Pressed)
            {
                _toDragDrop = (WrapPanel)sender;
                _panelOwner = (Canvas)((WrapPanel)sender).Parent;

                _toDragDrop.IsHitTestVisible = false;
                DragDrop.DoDragDrop(_toDragDrop, new DataObject(DataFormats.Serializable, _toDragDrop), DragDropEffects.Move);
                _toDragDrop.IsHitTestVisible = true;
            }
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
        private void MoveWarpPanel_MouseDown(object sender, MouseEventArgs e)
        {
            _toDragDrop = (WrapPanel)sender;
            _panelOwner = (Canvas)((WrapPanel)sender).Parent;
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            UppdateHeightLocForItems((Canvas)sender);

            SetDraggedPlayerInList(GetListToWorkWith(_panelOwner), GetListToWorkWith((Canvas)sender));

        }

        private void PlayersInGame_DragOver(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(DataFormats.Serializable);

            if (obj is UIElement elem)
            {
                Point point = e.GetPosition((Canvas)sender);

                Canvas.SetLeft(elem, point.X);
                Canvas.SetTop(elem, point.Y);

                if (!(((Canvas)(sender)).Children.Contains(elem)))
                {
                    ((Canvas)sender).Children.Add(elem);
                }
            }

            //UppdateHeightLocForItems((Canvas)sender);
        }



        public List<User> GetListToWorkWith(Canvas box)
        {
            return box == PlayersInGame ? _playersInGame : _notAddedPlayers;
        }

        public List<User> GetOtherListToWorkWith(Canvas box)
        {
            return box == PlayersInGame ? _notAddedPlayers : _playersInGame;

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

        private void RemovePlayerBut_Click(object sender, RoutedEventArgs e)
        {
            return;
            /*            if (PlayersInGame.SelectedItem is null) return;
                        TextBlock block = ((WrapPanel)PlayersInGame.SelectedItem).Children.OfType<TextBlock>().FirstOrDefault();
                        WrapPanel panel = ((WrapPanel)PlayersInGame.SelectedItem);
                        User toRemove = _playersInGame.Where(x => x.Login == block.Text).First();

                        PlayersInGame.Items.Remove(PlayersInGame.SelectedItem);
                        _playersInGame.Remove(toRemove);

                        _notAddedPlayers.Add(toRemove);
                        PlayersThatCanBeAdd.Items.Add(panel);*/
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

        private void AddPlayerInGameBut_Click(object sender, RoutedEventArgs e)
        {
            return;
            /*            if (PlayersThatCanBeAdd.SelectedItem is null) return;
                        TextBlock block = ((WrapPanel)PlayersThatCanBeAdd.SelectedItem).Children.OfType<TextBlock>().FirstOrDefault();
                        WrapPanel panel = ((WrapPanel)PlayersThatCanBeAdd.SelectedItem);
                        User toRemove = _notAddedPlayers.Where(x => x.Login == block.Text).First();

                        _notAddedPlayers.Remove(toRemove);
                        PlayersThatCanBeAdd.Items.Remove(panel);

                        PlayersInGame.Items.Add(panel);
                        _playersInGame.Add(toRemove);*/
        }

        private void Page_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void PlayersThatCanBeAdd_Drop(object sender, DragEventArgs e)
        {

        }

        private void PlayersThatCanBeAdd_DragLeave(object sender, DragEventArgs e)
        {
            //if (!(e.OriginalSource is Canvas)) return;
            object obj = e.Data.GetData(DataFormats.Serializable);

            if (obj is UIElement elem)
            {
                ((Canvas)sender).Children.Remove(elem);
            }

            UppdateHeightLocForItems((Canvas)sender);
        }

        private void Page_DragOver(object sender, DragEventArgs e)
        {

        }

        private void Page_Drop(object sender, DragEventArgs e)
        {
            object obj = e.Data.GetData(DataFormats.Serializable);

            if (obj is WrapPanel elem)
            {
                if (!(elem.Parent is null)) return;

                if (!(_panelOwner.Children.Contains(elem)))
                {
                    if(sender is Canvas) SetDraggedPlayerInList(GetListToWorkWith(_panelOwner), GetListToWorkWith((Canvas)sender));
                    _panelOwner.Children.Add(elem);
                }
            }
            UppdateHeightLocForItems(_panelOwner);

        }
    }
}
