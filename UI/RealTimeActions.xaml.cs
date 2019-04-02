using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace MegaXConfigTool
{
    /// <summary>
    /// Interaction logic for RealTimeActions.xaml
    /// </summary>
    public partial class RealTimeActions : MetroWindow
    {
        int clicked_tile_id;
        MainWindow mainWindow;
        byte MessageType;
        public RealTimeActions(MainWindow mainWindow, Tile clicked_tile, byte MessageType)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            clicked_tile_id = int.Parse((string)clicked_tile.Tag);
            RealTimeActionsLabelDescription.Content = clicked_tile.Title;
            this.MessageType = MessageType;
            switch (MessageType)
            {
                case (Constants.RT_Partition_Message):
                    RealTimeActionsButtonArmAway.Visibility = Visibility.Visible;
                    RealTimeActionsButtonArmStay.Visibility = Visibility.Visible;
                    RealTimeActionsButtonDisarm.Visibility = Visibility.Visible;
                    break;
                case (Constants.RT_Zone_Message):
                    RealTimeActionsButtonBypassOn.Visibility = Visibility.Visible;
                    RealTimeActionsButtonBypassOff.Visibility = Visibility.Visible;
                    break;

                case (Constants.RT_Output_Message):
                    RealTimeActionsButtonOutputOn.Visibility = Visibility.Visible;
                    RealTimeActionsButtonOutputOff.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void SendAction(object sender, RoutedEventArgs e)
        {
            var RealTimeUserCodeInput = new RealTimeUserCodeInput(this, clicked_tile_id, mainWindow, (Button)sender, MessageType);
            RealTimeUserCodeInput.Show();
            this.Close();
        }

    }
}
