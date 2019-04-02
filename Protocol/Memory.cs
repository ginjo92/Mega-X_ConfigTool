using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Memory
    {
        internal void read(MainWindow mainWindow, uint byte_to_read, uint address_init)
        {
            byte[] byte_array = new byte[128];
            uint i = 0;
            byte size = 235;
            uint address = address_init + (235 * byte_to_read);
            
            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((address >> 16) & 0xff);
            byte_array[i++] = (byte)((address >> 8) & 0xff);
            byte_array[i++] = (byte)((address) & 0xff);
            byte_array[i++] = (byte)((size) & 0xff);

            string StringByte = BitConverter.ToString(byte_array);

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow); //TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }

    }
}
