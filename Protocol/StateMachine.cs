
using MahApps.Metro.Controls.Dialogs;
using ProdigyConfigToolWPF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ProdigyConfigToolWPF.Protocol
{
    class StateMachine
    {
        int state_machine_state = 1;
        int state_machine_data_lenght;
        public int state_machine_data_lenght_temp;
        int state_machine_data_counter;
        byte[] checksum = new byte[4];
        byte[] received_data_no_checksum;
        uint received_data_no_checksum_counter;
        public byte[] data_rx = new byte[300]; //TODO: Confirm this buffer's size 

        // Checksum table
        public uint[] checksum_table = {
            0x00000000, 0x77073096, 0xEE0E612C, 0x990951BA,
            0x076DC419, 0x706AF48F, 0xE963A535, 0x9E6495A3,
            0x0EDB8832, 0x79DCB8A4, 0xE0D5E91E, 0x97D2D988,
            0x09B64C2B, 0x7EB17CBD, 0xE7B82D07, 0x90BF1D91,
            0x1DB71064, 0x6AB020F2, 0xF3B97148, 0x84BE41DE,
            0x1ADAD47D, 0x6DDDE4EB, 0xF4D4B551, 0x83D385C7,
            0x136C9856, 0x646BA8C0, 0xFD62F97A, 0x8A65C9EC,
            0x14015C4F, 0x63066CD9, 0xFA0F3D63, 0x8D080DF5,
            0x3B6E20C8, 0x4C69105E, 0xD56041E4, 0xA2677172,
            0x3C03E4D1, 0x4B04D447, 0xD20D85FD, 0xA50AB56B,
            0x35B5A8FA, 0x42B2986C, 0xDBBBC9D6, 0xACBCF940,
            0x32D86CE3, 0x45DF5C75, 0xDCD60DCF, 0xABD13D59,
            0x26D930AC, 0x51DE003A, 0xC8D75180, 0xBFD06116,
            0x21B4F4B5, 0x56B3C423, 0xCFBA9599, 0xB8BDA50F,
            0x2802B89E, 0x5F058808, 0xC60CD9B2, 0xB10BE924,
            0x2F6F7C87, 0x58684C11, 0xC1611DAB, 0xB6662D3D,
            0x76DC4190, 0x01DB7106, 0x98D220BC, 0xEFD5102A,
            0x71B18589, 0x06B6B51F, 0x9FBFE4A5, 0xE8B8D433,
            0x7807C9A2, 0x0F00F934, 0x9609A88E, 0xE10E9818,
            0x7F6A0DBB, 0x086D3D2D, 0x91646C97, 0xE6635C01,
            0x6B6B51F4, 0x1C6C6162, 0x856530D8, 0xF262004E,
            0x6C0695ED, 0x1B01A57B, 0x8208F4C1, 0xF50FC457,
            0x65B0D9C6, 0x12B7E950, 0x8BBEB8EA, 0xFCB9887C,
            0x62DD1DDF, 0x15DA2D49, 0x8CD37CF3, 0xFBD44C65,
            0x4DB26158, 0x3AB551CE, 0xA3BC0074, 0xD4BB30E2,
            0x4ADFA541, 0x3DD895D7, 0xA4D1C46D, 0xD3D6F4FB,
            0x4369E96A, 0x346ED9FC, 0xAD678846, 0xDA60B8D0,
            0x44042D73, 0x33031DE5, 0xAA0A4C5F, 0xDD0D7CC9,
            0x5005713C, 0x270241AA, 0xBE0B1010, 0xC90C2086,
            0x5768B525, 0x206F85B3, 0xB966D409, 0xCE61E49F,
            0x5EDEF90E, 0x29D9C998, 0xB0D09822, 0xC7D7A8B4,
            0x59B33D17, 0x2EB40D81, 0xB7BD5C3B, 0xC0BA6CAD,
            0xEDB88320, 0x9ABFB3B6, 0x03B6E20C, 0x74B1D29A,
            0xEAD54739, 0x9DD277AF, 0x04DB2615, 0x73DC1683,
            0xE3630B12, 0x94643B84, 0x0D6D6A3E, 0x7A6A5AA8,
            0xE40ECF0B, 0x9309FF9D, 0x0A00AE27, 0x7D079EB1,
            0xF00F9344, 0x8708A3D2, 0x1E01F268, 0x6906C2FE,
            0xF762575D, 0x806567CB, 0x196C3671, 0x6E6B06E7,
            0xFED41B76, 0x89D32BE0, 0x10DA7A5A, 0x67DD4ACC,
            0xF9B9DF6F, 0x8EBEEFF9, 0x17B7BE43, 0x60B08ED5,
            0xD6D6A3E8, 0xA1D1937E, 0x38D8C2C4, 0x4FDFF252,
            0xD1BB67F1, 0xA6BC5767, 0x3FB506DD, 0x48B2364B,
            0xD80D2BDA, 0xAF0A1B4C, 0x36034AF6, 0x41047A60,
            0xDF60EFC3, 0xA867DF55, 0x316E8EEF, 0x4669BE79,
            0xCB61B38C, 0xBC66831A, 0x256FD2A0, 0x5268E236,
            0xCC0C7795, 0xBB0B4703, 0x220216B9, 0x5505262F,
            0xC5BA3BBE, 0xB2BD0B28, 0x2BB45A92, 0x5CB36A04,
            0xC2D7FFA7, 0xB5D0CF31, 0x2CD99E8B, 0x5BDEAE1D,
            0x9B64C2B0, 0xEC63F226, 0x756AA39C, 0x026D930A,
            0x9C0906A9, 0xEB0E363F, 0x72076785, 0x05005713,
            0x95BF4A82, 0xE2B87A14, 0x7BB12BAE, 0x0CB61B38,
            0x92D28E9B, 0xE5D5BE0D, 0x7CDCEFB7, 0x0BDBDF21,
            0x86D3D2D4, 0xF1D4E242, 0x68DDB3F8, 0x1FDA836E,
            0x81BE16CD, 0xF6B9265B, 0x6FB077E1, 0x18B74777,
            0x88085AE6, 0xFF0F6A70, 0x66063BCA, 0x11010B5C,
            0x8F659EFF, 0xF862AE69, 0x616BFFD3, 0x166CCF45,
            0xA00AE278, 0xD70DD2EE, 0x4E048354, 0x3903B3C2,
            0xA7672661, 0xD06016F7, 0x4969474D, 0x3E6E77DB,
            0xAED16A4A, 0xD9D65ADC, 0x40DF0B66, 0x37D83BF0,
            0xA9BCAE53, 0xDEBB9EC5, 0x47B2CF7F, 0x30B5FFE9,
            0xBDBDF21C, 0xCABAC28A, 0x53B39330, 0x24B4A3A6,
            0xBAD03605, 0xCDD70693, 0x54DE5729, 0x23D967BF,
            0xB3667A2E, 0xC4614AB8, 0x5D681B02, 0x2A6F2B94,
            0xB40BBE37, 0xC30C8EA1, 0x5A05DF1B, 0x2D02EF8D  };

        // Validate data from protocol
        public bool ValidateData(byte data)
        {
            switch (state_machine_state)
            {

                case 1:
                    if (data == Constants.HEADER_1)
                    {
                        state_machine_state++;
                    }
                    else
                    {
                        state_machine_state = 1;
                    }
                    break;

                case 2:
                    if (data == Constants.HEADER_2)
                    {
                        state_machine_state++;
                    }
                    else { state_machine_state = 1; }

                    break;

                case 3:
                    state_machine_data_lenght_temp = data + Constants.CHECKSUM_SIZE;
                    state_machine_data_lenght = data + Constants.CHECKSUM_SIZE;
                    received_data_no_checksum_counter = 0;
                    received_data_no_checksum = new byte[state_machine_data_lenght_temp - Constants.CHECKSUM_SIZE];
                    
                    state_machine_state++;
                    break;

                case 4:
                    state_machine_data_lenght--;

                    if(state_machine_data_lenght > 3)
                        received_data_no_checksum[received_data_no_checksum_counter++] = data;

                    data_rx[state_machine_data_counter++] = data;

                    if (state_machine_data_lenght == 0)
                    {
                        state_machine_state = 1;

                        for (int j = 1; j <= Constants.CHECKSUM_SIZE; j++)
                        {
                            checksum[Constants.CHECKSUM_SIZE - j] = data_rx[state_machine_data_counter - j];
                        }

                        state_machine_data_counter = 0;
                        state_machine_data_lenght = 0;
                        received_data_no_checksum_counter = 0;

                        return ValidateReceivedChecksum(checksum);
                    }
                    break;
            }
            return false;
        }

        private bool ValidateReceivedChecksum(byte[] checksum)
        {
            uint checksum_received_sum = (uint)(checksum[3] + (checksum[2] << 8) + (checksum[1] << 16) + (checksum[0] << 24));

            uint checksum_calculated = calculate_checksum(received_data_no_checksum);

            if (checksum_received_sum.Equals(checksum_calculated))
                return true;
            else
                return false;
        }

        public uint calculate_checksum(byte[] buf)
        {
            uint i;
            uint checksum = 0 ^ 0xFFFFFFFF;
            for (i = 0; i < (uint)buf.Length; i++)
                checksum = (checksum >> 8) ^ checksum_table[((checksum ^ buf[i]) & 0x000000FF)];

            return (checksum ^ 0xFFFFFFFF);
        }

        // Handle with data
        public void Operations(byte action, MainWindow mainform, int data_size)
        {
            
            switch (action)
            {
                case Constants.CHECK_ID:
                    int control_panel_id = (data_rx[0] << 8) + data_rx[1];

                    mainform.cp_id[0] = data_rx[0];
                    mainform.cp_id[1] = data_rx[1];

                    mainform.serial_number[0] = data_rx[3];
                    mainform.serial_number[1] = data_rx[4];
                    mainform.serial_number[2] = data_rx[5];
                    mainform.serial_number[3] = data_rx[6];
                    mainform.serial_number[4] = data_rx[7];
                    mainform.serial_number[5] = data_rx[8];
                    mainform.serial_number[6] = data_rx[9];
                    mainform.serial_number[7] = data_rx[10];
                    mainform.serial_number[8] = data_rx[11];
                    mainform.serial_number[9] = data_rx[12];

                    mainform.hw_version[0] = data_rx[13];
                    mainform.hw_version[1] = data_rx[14];
                    mainform.hw_version[2] = data_rx[15];

                    mainform.sw_version[0] = data_rx[16];
                    mainform.sw_version[1] = data_rx[17];
                    mainform.sw_version[2] = data_rx[18];

                    byte[] event_code = new byte[2];
                    event_code[0] = data_rx[19];
                    event_code[1] = data_rx[20];
                    //event_code[0] = data_rx[12];
                    //event_code[1] = data_rx[14];


                    //uint serial_number = (uint)((data_rx[3] << 24) + (data_rx[4] << 16) + (data_rx[5] << 8) + data_rx[6]);
                    string serial_number = Encoding.UTF8.GetString(mainform.serial_number);
                    mainform.event_code = (uint)(event_code[0] << 8) + event_code[1];
                    string hardware_version = Encoding.UTF8.GetString(mainform.hw_version);
                    string software_version = Encoding.UTF8.GetString(mainform.sw_version);
                    mainform.write_control_panel_id_label(control_panel_id, hardware_version, software_version, serial_number);

                    //string EventCode = BitConverter.ToString(event_code);
                    //System.Diagnostics.Debug.WriteLine("EVENT CODE: ");
                    //System.Diagnostics.Debug.WriteLine(EventCode);
                    string DataRX = BitConverter.ToString(data_rx);
                    System.Diagnostics.Debug.WriteLine("DATA RX: " + DataRX);


                    mainform.serial_port_connection_timer.Stop();
                    

                    // UI changes if connected to a Mega-X central
                    mainform.Dispatcher.Invoke((Action)(() => mainform.TextBoxConnectedDisconnected.Text = Properties.Resources.ComConnected));
                    mainform.Dispatcher.Invoke((Action)(() => mainform.TextBoxConnectedDisconnected.Foreground = Brushes.Green));
                    mainform.Dispatcher.Invoke((Action)(() => mainform.StatusBarDisconnectedIcon.Visibility = Visibility.Collapsed));
                    mainform.Dispatcher.Invoke((Action)(() => mainform.StatusBarConnectedIcon.Visibility = Visibility.Visible));
                    mainform.Dispatcher.Invoke((Action)(async () => await DialogManager.ShowMessageAsync(mainform, Properties.Resources.Connection_Successful, "")));
                    mainform.Dispatcher.Invoke((Action)(() => mainform.FlyCom.IsOpen = false));
                    break;

                case Constants.READ_CODE: // Create an enum or something like that
                    mainform.UpdateDataGridViews(data_rx, 251);
                    break;


                case 0xC0: // Create an enum or something like that
                case 0xC1:
                case 0xC2:
                case 0xC3:
                case 0xC4:
                case 0xC5:
                case 0xC6:
                case 0xC7:
                case 0xC8:
                case 0xC9:
                    mainform.updateDebugTextBox(data_rx, data_size, 0); // for test purposes only
                    break;

                case 0xD0:
                    mainform.UpdateRealTimeView(data_rx, 251);
                    break;

                    
            }
        }
    } 
}
