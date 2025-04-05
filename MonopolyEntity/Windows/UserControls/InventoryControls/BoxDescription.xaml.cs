using MonopolyDLL;
using MonopolyDLL.Monopoly.InventoryObjs;
using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;

namespace MonopolyEntity.Windows.UserControls.InventoryControls
{
    /// <summary>
    /// Логика взаимодействия для ItemDescription.xaml
    /// </summary>
    public partial class BoxDescription : UserControl
    {
        private Frame _frame;
        private CaseBox _caseBox;
        private string _loggedUserLogin;

        public BoxDescription(Frame workingFrame, CaseBox item, string loggedUserLogin)
        {
            _frame = workingFrame;
            _caseBox = item;
            _loggedUserLogin = loggedUserLogin;

            InitializeComponent();

            SetTextParams();
        }

        public void SetTextParams()
        {//SystemParamsServeses.GetStringByName("InPrisonQustion")
            ItemName.Text = _caseBox.Name;
            ItemType.Text = SystemParamsService.GetStringByName("LotBox");
            ItemDesctiption.Text = SystemParamsService.GetStringByName("LotBoxDesc");
            ColType.Text = SystemParamsService.GetStringByName("LotBoxColType");
            CanBeDroppedDescription.Text = SystemParamsService.GetStringByName("LotBoxCanDroppedDesc");
        }

        private void OpenCaseBut_Click(object sender, RoutedEventArgs e)
        {
            const int fullBlur = 100;
            //return;
            MainWindow obj =
                Helper.FindParent<MainWindow>(_frame);

            OpenCase inventory = new OpenCase(_caseBox, _loggedUserLogin);
            //_frame.Content = inventory;

            obj.CaseFrame.Content = inventory;

            BlurEffect blurEffect = new BlurEffect
            {
                Radius = fullBlur
            };
            obj.VisiableItems.Effect = blurEffect;

            _frame.Effect = blurEffect;
            obj.CaseFrame.Visibility = Visibility.Visible;
        }
    }
}
