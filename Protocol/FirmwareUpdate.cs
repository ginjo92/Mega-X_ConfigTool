using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class FirmwareUpdate
    {
       public Dictionary<string, Dictionary<string, object>> attributes =
       new Dictionary<string, Dictionary<string, object>>
       {
            {
                "byte_update_check",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 0 }
                }

            },
       };

        public void WriteReadyToUpdateByte(MainWindow mainForm)
        {
            General protocol = new General();
            byte[] byte_array = new byte[240];
            int i = 0;

            uint address = Constants.KP_FLASH_FIRMWARE_UPDATE_INICIO;

            byte_array[i++] = Constants.WRITE_CODE; 
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);
            byte_array[i++] = (byte)(0x01);
            byte_array[i++] = (byte)(0xCA);

            byte_array[4] = (byte)(0x01);

            protocol.send_msg((uint)(i), byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is needed
        }

        public void Write(MainWindow mainForm, byte[] fragment_update_array, uint address, uint data_fragment_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            int i = 0;
            uint j = 0;
            General protocol = new General();

            byte_array[i++] = Constants.WRITE_BLOCK_CODE_START; ;
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);

            byte_array[i++] = (byte)(fragment_update_array.Length & 0xFF);
            int temp = i;
            //TODO: Create a function for this for 


            //Options
            for (i = temp, j = 0; i < (temp + fragment_update_array.Length); i++, j++)
            {
                byte_array[i] = fragment_update_array[j];
            }

            byte_array[4] = (byte)(i - temp);

            protocol.send_msg((uint)(i), byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is needed
            
        }

        internal void write_block(MainWindow mainWindow, uint address, uint block_size)
        {
            byte[] byte_array = new byte[235]; // verificar este tamanho
            int i = 0;
            General protocol = new General();
            
            byte_array[i++] = Constants.WRITE_BLOCK_CODE;
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);
            byte_array[i++] = (byte)((block_size >> 8) & 0xFF);
            byte_array[i++] = (byte)(block_size & 0xFF);

            protocol.send_msg((uint)i, byte_array, mainWindow.cp_id, mainWindow);
                        
        }


        public void Read(MainWindow mainForm, uint request_fragment)
        {
            byte[] byte_array = new byte[63];

                uint i = 0;
                uint address = Constants.KP_FLASH_FIRMWARE_UPDATE_INICIO + (request_fragment * 240);
                byte size = 235;
                if(request_fragment == 4368)
                {
                    size = 16;
                }

                // Create first 5 bytes of the request
                byte_array[i++] = 0x20;
                byte_array[i++] = (byte)((address >> 16) & 0xff);
                byte_array[i++] = (byte)((address >> 8) & 0xff);
                byte_array[i++] = (byte)((address) & 0xff);
                byte_array[i++] = size;

                General protocol = new General();
                protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
                System.Threading.Thread.Sleep(250);

        }

        public void WriteUpdateDone(MainWindow mainForm)
        {
            byte[] trama_a_enviar = new byte[1];
            uint i = 0;
            trama_a_enviar[i++] = Constants.UPDATE_DONE_CODE;
            General protocol = new General();
            protocol.send_msg((uint)trama_a_enviar.Length, trama_a_enviar, mainForm.cp_id, mainForm);
            System.Threading.Thread.Sleep(250);
        }

        
    }
}
