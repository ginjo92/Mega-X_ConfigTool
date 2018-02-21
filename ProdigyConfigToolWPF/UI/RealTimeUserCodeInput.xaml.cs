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

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for RealTimeUserCodeInput.xaml
    /// </summary>
    public partial class RealTimeUserCodeInput : MetroWindow
    {
        byte id;
        MainWindow mainWindow;
        int previous_menu_action;
        RealTimeActions previous_menu;
        byte MessageType;
        public RealTimeUserCodeInput(RealTimeActions previous_menu, int object_id, MainWindow mainWindow, Button button_clicked, byte MessageType)
        {
            InitializeComponent();
            this.previous_menu = previous_menu;
            this.mainWindow = mainWindow;
            this.MessageType = MessageType;
            id = (byte)object_id;
            previous_menu_action = int.Parse((string)button_clicked.Tag);
        }

        private void ButtonSendUserCode_Click(object sender, RoutedEventArgs e)
        {
            Protocol.RealTime real_time = new Protocol.RealTime();
            byte action = Convert.ToByte(previous_menu_action);
            byte[] code = new byte[8];
            code = Encoding.ASCII.GetBytes(UserPasswordValue.Password);//new byte[] { (0x01 + 0x30), (0x02 + 0x30), (0x03 + 0x30), (0x04 + 0x30), 0xFF, 0xFF, 0xFF, 0xFF };
            
            if (code.Length < 8)
            {
                byte[] complementary_code = new byte[8-code.Length];

                for(int i = 0; i < (8 - code.Length); i++)
                {
                    complementary_code[i] = 0xFF;
                }
                Array.Resize<byte>(ref code, 8);
                Array.Copy(complementary_code, 0, code, (8-complementary_code.Length), complementary_code.Length);
            }
            real_time.send_action(id, action, code, mainWindow, MessageType);
            this.Close();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
