using MonopolyDLL;
using MonopolyDLL.Monopoly;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
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
using System.Windows.Shapes;

namespace MonopolyEntity.Windows
{
    /// <summary>
    /// Логика взаимодействия для SetPlayersForGame.xaml
    /// </summary>
    public partial class SetPlayersForGame : Window
    {
        List<User> _users;
        MonopolySystem _system;
        Frame _frame;

        List<User> _playersInGame;
        List<User> _notAddedPlayers;

        public SetPlayersForGame(MonopolySystem system, Frame frame)
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
            for (int i = 0; i < users.Count; i++)
            {
                WrapPanel panel = new WrapPanel()
                {
                    Orientation = Orientation.Horizontal
                };

                panel.Children.Add(MainWindowHelper.GetCircleImage(50, 50, DBQueries.GetPictureNameById(users[i].GetPictureId())));

                TextBlock block = GetTextBlock(users[i].Login);
                block.VerticalAlignment = VerticalAlignment.Center;
                block.Margin = new Thickness(5, 0, 0, 0);
   
                panel.Children.Add(block);
                
                box.Items.Add(panel);
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
            Close();
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
/*            const int leastAmountOfPlayers = 2;
            if (!_users.Any(x => x.Login == _system.LoggedUser.Login) ||
                _playersInGame.Count < leastAmountOfPlayers) return;


            _system.MonGame.Players = _playersInGame;*/

            GamePage page = new GamePage(_frame, _system);
            _frame.Content = page;

            Close();
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
