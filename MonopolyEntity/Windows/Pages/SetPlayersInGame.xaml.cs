using MonopolyDLL.Monopoly;
using MonopolyDLL;
using MonopolyEntity.VisualHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using System.Data.OleDb;

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
            PlayersInGame.Items.Clear();
            PlayersThatCanBeAdd.Items.Clear();

            AddLoginsInBox(_playersInGame, PlayersInGame);
            AddLoginsInBox(_notAddedPlayers, PlayersThatCanBeAdd);
        }

        private void AddLoginsInBox(List<User> users, ListBox box)
        {
            box.Drop += ListBox_Drop;
            for (int i = 0; i < users.Count; i++)
            {
                WrapPanel panel = new WrapPanel()
                {
                    Width = this.Width / 2,
                    Background = Brushes.Transparent,
                    Orientation = Orientation.Horizontal
                };

                panel.MouseMove += DragItem_MouseMove;

                panel.Children.Add(MainWindowHelper.GetCircleImage(50, 50, DBQueries.GetPictureNameById(users[i].GetPictureId())));

                TextBlock block = GetTextBlock(users[i].Login);
                block.VerticalAlignment = VerticalAlignment.Center;
                block.Margin = new Thickness(5, 0, 0, 0);

                panel.Children.Add(block);

                box.Items.Add(panel);
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            SetDraggedPlayerInList(GetListToWorkWith(_panelOwner), GetListToWorkWith((ListBox)sender));

      
            _panelOwner.Items.Remove(_toDragDrop);
            ((ListBox)sender).Items.Add(_toDragDrop);
        }

        public List<User> GetListToWorkWith(ListBox box)
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
        private ListBox _panelOwner;

        private void DragItem_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                _toDragDrop = (WrapPanel)sender;
                _panelOwner = (ListBox)((WrapPanel)sender).Parent;

                DragDrop.DoDragDrop((WrapPanel)sender, (WrapPanel)sender, DragDropEffects.Move);
            }
        }

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
            if (PlayersInGame.SelectedItem is null) return;
            TextBlock block = ((WrapPanel)PlayersInGame.SelectedItem).Children.OfType<TextBlock>().FirstOrDefault();
            WrapPanel panel = ((WrapPanel)PlayersInGame.SelectedItem);
            User toRemove = _playersInGame.Where(x => x.Login == block.Text).First();

            PlayersInGame.Items.Remove(PlayersInGame.SelectedItem);
            _playersInGame.Remove(toRemove);

            _notAddedPlayers.Add(toRemove);
            PlayersThatCanBeAdd.Items.Add(panel);
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
            if (PlayersThatCanBeAdd.SelectedItem is null) return;
            TextBlock block = ((WrapPanel)PlayersThatCanBeAdd.SelectedItem).Children.OfType<TextBlock>().FirstOrDefault();
            WrapPanel panel = ((WrapPanel)PlayersThatCanBeAdd.SelectedItem);
            User toRemove = _notAddedPlayers.Where(x => x.Login == block.Text).First();

            _notAddedPlayers.Remove(toRemove);
            PlayersThatCanBeAdd.Items.Remove(panel);

            PlayersInGame.Items.Add(panel);
            _playersInGame.Add(toRemove);
        }
    }
}
