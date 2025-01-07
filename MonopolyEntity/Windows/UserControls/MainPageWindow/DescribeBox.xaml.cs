using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

namespace MonopolyEntity.Windows.UserControls.MainPage
{
    /// <summary>
    /// Логика взаимодействия для DescribeBox.xaml
    /// </summary>
    public partial class DescribeBox : UserControl
    {
        public DescribeBox()
        {
            InitializeComponent();
        }

        public DescribeBox(string nameText, string descText, Image img)
        {
            InitializeComponent();

            SetCardParams(nameText, descText, img);
        }

        public void SetCardParams(string nameText, string descText, Image img)
        {
            CardImg.Source = img.Source;
            NameText.Text = nameText;
            DescribeText.Text = descText;
        }
    }
}
