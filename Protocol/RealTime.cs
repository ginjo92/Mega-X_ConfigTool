using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class RealTime
    {
        public void read(MainWindow mainForm)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            byte_array[i++] = 0xD0;
           
            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
            
        }

        internal void send_action(byte id, byte action, byte[] code, MainWindow mainForm, byte MessageType)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            byte_array[i++] = MessageType;
            byte_array[i++] = id;
            byte_array[i++] = action;

            foreach (byte a in code)
                byte_array[i++] = a;

            if (byte_array[0] == 0xD1 && byte_array[2] == 0x01) //ARM AWAY 
            {
                mainForm.ArmMode = 0;
                mainForm.FLAG_PARTITION_IS_ARMING = 1;
            }
            if (byte_array[0] == 0xD1 && byte_array[2] == 0x02) //ARM STAY 
            {
                mainForm.FLAG_PARTITION_IS_ARMING = 1;
                mainForm.ArmMode = 1;
            }
            switch (byte_array[1])
            {
                case 0x00:
                    mainForm.partitionIsArming = Constants.PARTITION_1;
                    break;
                case 0x01:
                    mainForm.partitionIsArming = Constants.PARTITION_2;
                    break;
                case 0x02:
                    mainForm.partitionIsArming = Constants.PARTITION_3;
                    break;
                case 0x03:
                    mainForm.partitionIsArming = Constants.PARTITION_4;
                    break;
                case 0x04:
                    mainForm.partitionIsArming = Constants.PARTITION_5;
                    break;
                case 0x05:
                    mainForm.partitionIsArming = Constants.PARTITION_6;
                    break;
                case 0x06:
                    mainForm.partitionIsArming = Constants.PARTITION_7;
                    break;
                case 0x07:
                    mainForm.partitionIsArming = Constants.PARTITION_8;
                    break;
            }
            
            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }
    }
}
