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

namespace ProdigyConfigToolWPF.Controls
{
    /// <summary>
    /// Interaction logic for UserControlStatus.xaml
    /// </summary>
    public partial class UserControlStatus : UserControl
    {
        public UserControlStatus()
        {
            InitializeComponent();
        }

        private void StatusPartition1_Click(object sender, RoutedEventArgs e)
        {
            var RealTimeActionsWindow = new RealTimeActions(this, sender as Tile, 0xD1);

            RealTimeActionsWindow.Show();
        }

        private void StatusZone_Click(object sender, RoutedEventArgs e)
        {
            var RealTimeActionsWindow = new RealTimeActions(this, sender as Tile, 0xD2);

            RealTimeActionsWindow.Show();
        }

        private void StatusOutputs_Click(object sender, RoutedEventArgs e)
        {
            var RealTimeActionsWindow = new RealTimeActions(this, sender as Tile, 0xD3);

            RealTimeActionsWindow.Show();
        }
    }
}
