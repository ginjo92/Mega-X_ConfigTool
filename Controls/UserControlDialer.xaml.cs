using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.IO.Ports;
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
using MahApps.Metro.Controls.Dialogs;


namespace ProdigyConfigToolWPF.Controls
{
    /// <summary>
    /// Interaction logic for UserControlDialer.xaml
    /// </summary>
    public partial class UserControlDialer : UserControl
    {
        SerialPort serialPort = new SerialPort();

        public UserControlDialer(SerialPort _serialPort)
        {
            InitializeComponent();

            this.serialPort = _serialPort;
        }

        private void DialerDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            
            if (MainWindow.serialPort.IsOpen)
            {
                DataChoose data_choose = new DataChoose(MainWindow.mainWindow, false, "dialer");
                data_choose.Show();
                this.IsEnabled = false;
            }
            else
            {
                //await MahApps.Metro.Controls.Dialogs.DialogManager.ShowMessageAsync(MainWindow.mainWindow, Properties.Resources.PleaseConnectFirst, "");
            }
        }

        private void DialerUploadTile_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.serialPort.IsOpen)
            {
                DataChoose data_choose = new DataChoose(MainWindow.mainWindow, true, "dialer");
                data_choose.Show();
                this.IsEnabled = false;
            }
            else
            {
                //await DialogManager.ShowMessageAsync(MainWindow.mainWindow, Properties.Resources.PleaseConnectFirst, "");
            }
        }
    }
}
