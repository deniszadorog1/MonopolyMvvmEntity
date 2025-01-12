using MonopolyEntity.VisualHelper;
using MonopolyEntity.Windows.Pages;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonopolyEntity.Windows.UserControls.InventoryControls
{
    /// <summary>
    /// Логика взаимодействия для ItemDescription.xaml
    /// </summary>
    public partial class BoxDescription : UserControl
    {
        private Frame _frame;
        public BoxDescription(Frame workingFrame)
        {
            _frame = workingFrame;
            InitializeComponent();
        }

        private void OpenCaseBut_Click(object sender, RoutedEventArgs e)
        {
            //return;
            WorkWindow obj = 
                Helper.FindParent<WorkWindow>(_frame);

            OpenCase inventory = new OpenCase();
            //_frame.Content = inventory;


            obj.CaseFrame.Content = inventory;

            BlurEffect blurEffect = new BlurEffect
            {
                Radius = 100 
            };
            obj.VisiableItems.Effect = blurEffect;

            _frame.Effect = blurEffect;
            obj.CaseFrame.Visibility = Visibility.Visible;
        }
    }
}
