using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdigyConfigToolWPF.Protocol
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

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
        }
    }
}
