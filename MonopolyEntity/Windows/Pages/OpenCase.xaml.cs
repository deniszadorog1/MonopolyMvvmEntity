using System.Collections.Generic;
using System.Windows.Controls;


using MonopolyEntity;
using MonopolyEntity.Windows.UserControls;

namespace MonopolyEntity.Windows.Pages
{
    /// <summary>
    /// Логика взаимодействия для OpenCase.xaml
    /// </summary>
    public partial class OpenCase : Page
    {
        public OpenCase()
        {
            InitializeComponent();

            SetTestCaseDrops();
        }

        public void SetTestCaseDrops()
        {
            List<CaseCard> testCards = ThingForTest.GetTestCaseCards();
        
            for(int i = 0; i < testCards.Count; i++)
            {
                CanBeDropedPanel.Children.Add(testCards[i]);
            }
        }
        
        

    }
}
