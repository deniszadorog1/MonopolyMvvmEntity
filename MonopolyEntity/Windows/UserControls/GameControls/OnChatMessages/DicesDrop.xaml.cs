using MonopolyEntity.Windows.UserControls.GameControls.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
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

namespace MonopolyEntity.Windows.UserControls.GameControls.OnChatMessages
{
    /// <summary>
    /// Логика взаимодействия для DicesDrop.xaml
    /// </summary>
    public partial class DicesDrop : UserControl
    {
        private int _firstCube;
        private int _secondCube;
        
        public Dice _first3dCube;
        public Dice _second3dCube;
        
        public DicesDrop(int firstCube, int secondCube)
        {
            _firstCube = firstCube;
            _secondCube = secondCube;

            InitializeComponent();

            CreateCubes();
            SetCubesIdGrids();
        }

        private void CreateCubes()
        {
            const int cubesTopThickness = 20;
            _first3dCube = GetDice(HorizontalAlignment.Right,
                new Thickness(0, -cubesTopThickness, 0, 0), _firstCube);

            _second3dCube = GetDice(HorizontalAlignment.Left,
                new Thickness(0, cubesTopThickness, 0, 0), _secondCube);
        }

        public void SetCubesIdGrids()
        {
            FirstCube.Children.Add(_first3dCube);
            SecondCube.Children.Add(_second3dCube);
        }

        public Dice GetDice(HorizontalAlignment alinment, Thickness thick ,int cubeRes)
        {
            const int cubeSizeParam = 175;
            Dice dice = new Dice(cubeRes)
            {
                Width = cubeSizeParam,
                Height = cubeSizeParam,
                HorizontalAlignment = alinment,
                Margin = thick
            };

            return dice;
        }
    }
}
