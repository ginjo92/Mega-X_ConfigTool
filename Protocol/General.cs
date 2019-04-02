using MegaXConfigTool;
using System;

namespace MegaXConfigTool.Protocol
{
    class General
    {
        //public byte[] tx_buffer = new byte[300]; //TODO: Check max length necessary
        public byte[] cp_id = { 0x00, 00 };
        // MSG Id's list
        public byte[] ask_watch = new byte[] { 0x30 };
        public byte[] check_id_message = new byte[] { Constants.CHECK_ID };

        public int blocks_written = 0;

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

        internal void check_ID(MainWindow current_form)
        {
            send_msg((uint)check_id_message.Length, check_id_message, current_form.cp_id, current_form);
        }

        internal void update_hour_and_date(MainWindow current_form)
        {
            byte[] trama_a_enviar = new byte[63];


            uint i = 0;
            trama_a_enviar[i++] = Constants.UPDATE_DATE_HOUR_CODE;
            int ano_int = DateTime.Now.Year;//ascii2int(ano_array, ano_array.Length);
            trama_a_enviar[i++] = (byte)(ano_int - 2000);
            int mes_int = DateTime.Now.Month;//ascii2int(mes_array, mes_array.Length);
            trama_a_enviar[i++] = (byte)(mes_int);
            int dia_int = DateTime.Now.Day;//ascii2int(dia_array, dia_array.Length);
            trama_a_enviar[i++] = (byte)(dia_int);
            int hora_int = DateTime.Now.Hour;//ascii2int(hora_array, hora_array.Length);
            trama_a_enviar[i++] = (byte)(hora_int);
            int minutos_int = DateTime.Now.Minute;//ascii2int(minutos_array, minutos_array.Length);
            trama_a_enviar[i++] = (byte)(minutos_int);
            int segundos_int = DateTime.Now.Second;// ascii2int(segundos_array, segundos_array.Length);
            trama_a_enviar[i++] = (byte)(segundos_int);
            int dia_semana_int = (int)DateTime.Now.DayOfWeek;//ascii2int(dia_array_semana, dia_array_semana.Length);
            trama_a_enviar[i++] = (byte)(dia_semana_int);

            send_msg((uint)trama_a_enviar.Length, trama_a_enviar, current_form.cp_id, current_form);
        }

        internal void check_all_events(MainWindow current_form)
        {
            byte[] trama_a_enviar = new byte[63];

            uint i = 0;
            trama_a_enviar[i++] = 0x91;

            send_msg((uint)trama_a_enviar.Length, trama_a_enviar, current_form.cp_id, current_form);

        }

        public uint send_msg_block(uint size, byte[] buf, uint address, byte[] cp_id, MainWindow current_form, uint block_size)
        {
            uint blocks_sent = 0;
            uint max_blocks = 0;
            uint block_addr = 0;
            uint max_size = 4096;
            int i = 0;
            uint address_final = address + block_size;

            short message_size = Constants.HEADER_SIZE + Constants.LENGTH_SIZE + Constants.DESTINY_TYPE_SIZE + Constants.DESTINY_ADDRESS_SIZE + Constants.CHECKSUM_SIZE;
            byte[] tx_buffer = new byte[message_size + size];
            byte[] temp_buf = new byte[(2 + size)];

            max_blocks = max_size / block_size;
            
            check_if_block_complete:
            if (current_form.counter_blocks == max_blocks)
            {
                current_form.RX_ACK = false;
                System.Diagnostics.Debug.WriteLine("BLOCK COMPLETE" + current_form.RX_ACK);

                buf[i++] = Constants.WRITE_BLOCK_CODE;
                buf[i++] = (byte)((block_addr >> 16) & 0xFF);
                buf[i++] = (byte)((block_addr >> 8) & 0xFF);
                buf[i++] = (byte)(block_addr & 0xFF);
                buf[i++] = (byte)((max_size >> 8) & 0xFF);
                buf[i++] = (byte)(max_size & 0xFF);

                temp_buf[0] = cp_id[0];
                temp_buf[1] = cp_id[1];

                for (i = 0; i < size; i++)
                    temp_buf[i + 2] = buf[i];

                i = 0;
                uint checksum = calculate_checksum(temp_buf);

                tx_buffer[i++] = Constants.HEADER_1;
                tx_buffer[i++] = Constants.HEADER_2;

                tx_buffer[i++] = (byte)(size + 2);

                for (i = 3; (i - 3) < temp_buf.Length; i++)
                    tx_buffer[i] = temp_buf[i - 3];

                tx_buffer[i++] = (byte)((checksum >> 24) & 0x000000ff);
                tx_buffer[i++] = (byte)((checksum >> 16) & 0x000000ff);
                tx_buffer[i++] = (byte)((checksum >> 8) & 0x000000ff);
                tx_buffer[i++] = (byte)((checksum >> 0) & 0x000000ff);

                System.Diagnostics.Debug.WriteLine("*** SAVING " + current_form.counter_blocks + " elements on flash @ 0x{0:X} ***", block_addr);

                blocks_sent++;

                current_form.counter_blocks = 0;
                current_form.send_serial_port_data(tx_buffer, 0, i);
                System.Threading.Thread.Sleep(current_form.delay_savingtime);

            }
            else
            {
                current_form.counter_blocks++;
                block_addr = address - (current_form.counter_blocks - 1) * block_size;
                send_msg(size, buf, cp_id, current_form);
                System.Diagnostics.Debug.WriteLine("* BLOCK #" + current_form.counter_blocks + " --- 0x{0:X} --- 0x{1:X} - 0x{2:X} ---", block_addr, address, address_final);
                

                if (address_final == block_addr + max_blocks * block_size)
                    goto check_if_block_complete;
                
            }
                    

            return 0;
        }

        public uint send_msg_block_audio(uint size, byte[] buf, uint address, byte[] cp_id, MainWindow current_form, uint block_size)
        {
            
            uint block_addr = 0;
            uint max_size = 0;

            

            if (current_form.blocks_written == 0)
            {
                max_size = 4096 - Constants.KP_FLASH_TAMANHO_DADOS_AUDIO_CONFIG_FLASH;
                block_addr = 0x200320;
            }
            else
            {
                max_size = 4096;
                block_addr = 0x200000 + current_form.blocks_written * max_size;
            }

            uint bytes_left = max_size - current_form.counter_blocks * block_size;
            current_form.counter_blocks++;

            uint address_final = address + block_size;

            System.Diagnostics.Debug.WriteLine("* BLOCK #" + current_form.counter_blocks + " --- 0x{0:X} --- 0x{1:X} - 0x{2:X} ---", block_addr, address, address_final);
            send_msg(size, buf, cp_id, current_form);

            if (address_final == block_addr + max_size)
            {
                current_form.block_complete = 1;
                //Sending command 0x48 to write a full block
                
            }
            return 0;
        }

      

        public void send_command_save_block(uint size, uint block_addr, byte[] cp_id, MainWindow current_form)
        {
            byte[] buf = new byte[240];

            uint i = 0;
            buf[i++] = Constants.WRITE_BLOCK_CODE;
            buf[i++] = (byte)((block_addr >> 16) & 0xFF);
            buf[i++] = (byte)((block_addr >> 8) & 0xFF);
            buf[i++] = (byte)(block_addr & 0xFF);
            buf[i++] = (byte)size;

            send_msg(i, buf, cp_id, current_form);

            System.Threading.Thread.Sleep(current_form.delay_savingtime);
            
        }

        public void send_command_save_block_big(uint size, uint block_addr, byte[] cp_id, MainWindow current_form)
        {
            byte[] buf = new byte[240];

            uint i = 0;
            buf[i++] = Constants.WRITE_BLOCK_CODE;
            buf[i++] = (byte)((block_addr >> 16) & 0xFF);
            buf[i++] = (byte)((block_addr >> 8) & 0xFF);
            buf[i++] = (byte)(block_addr & 0xFF);
            buf[i++] = (byte)((size >> 8) & 0xFF);
            buf[i++] = (byte)(size & 0xFF);

            send_msg(i, buf, cp_id, current_form);

            System.Threading.Thread.Sleep(current_form.delay_savingtime);

        }

        public uint send_msg(uint size, byte[] buf, byte[] cp_id, MainWindow current_form)
        {
            current_form.RX_ACK = false;
            //System.Diagnostics.Debug.WriteLine("[SendMsg] RX ACK: " + current_form.RX_ACK);

            int i = 0;
            short message_size = Constants.HEADER_SIZE + Constants.LENGTH_SIZE + Constants.DESTINY_TYPE_SIZE + Constants.DESTINY_ADDRESS_SIZE + Constants.CHECKSUM_SIZE;
            byte[] tx_buffer = new byte[message_size + size];

            if (size > 240)
                return 0xFFFFFFFF;

            byte[] temp_buf = new byte[(2 + size)];

            temp_buf[0] = cp_id[0];
            temp_buf[1] = cp_id[1];

            for (i = 0; i < size; i++)
                temp_buf[i + 2] = buf[i];

            i = 0;
            uint checksum = calculate_checksum(temp_buf);

            tx_buffer[i++] = Constants.HEADER_1;
            tx_buffer[i++] = Constants.HEADER_2;

            tx_buffer[i++] = (byte)(size + 2);

            for (i = 3; (i - 3) < temp_buf.Length; i++)
                tx_buffer[i] = temp_buf[i - 3];
            
            tx_buffer[i++] = (byte)((checksum >> 24) & 0x000000ff);
            tx_buffer[i++] = (byte)((checksum >> 16) & 0x000000ff);
            tx_buffer[i++] = (byte)((checksum >> 8) & 0x000000ff);
            tx_buffer[i++] = (byte)((checksum >> 0) & 0x000000ff);
            
            //handle with it on Form class
            current_form.send_serial_port_data(tx_buffer, 0, i);
            
            return 0;
        }

        //public uint send_msg_block(uint size, byte[] buf, byte[] cp_id, MainWindow current_form)
        //{
        //    current_form.RX_ACK = false;
        //    System.Diagnostics.Debug.WriteLine("[SendMsg] RX ACK: " + current_form.RX_ACK);

        //    int i = 0;
        //    short message_size = Constants.HEADER_SIZE + Constants.LENGTH_SIZE + Constants.DESTINY_TYPE_SIZE + Constants.DESTINY_ADDRESS_SIZE + Constants.CHECKSUM_SIZE;
        //    byte[] tx_buffer = new byte[message_size + size];

        //    if (size > 4000)
        //        return 0xFFFFFFFF;

        //    byte[] temp_buf = new byte[(2 + size)];

        //    temp_buf[0] = cp_id[0];
        //    temp_buf[1] = cp_id[1];

        //    for (i = 0; i < size; i++)
        //        temp_buf[i + 2] = buf[i];

        //    i = 0;
        //    uint checksum = calculate_checksum(temp_buf);

        //    tx_buffer[i++] = Constants.HEADER_1;
        //    tx_buffer[i++] = Constants.HEADER_2;

        //    tx_buffer[i++] = (byte)(size + 2);
            
        //    for (i = 3; (i - 3) < temp_buf.Length; i++)
        //        tx_buffer[i] = temp_buf[i - 3];

        //    tx_buffer[i++] = (byte)((checksum >> 24) & 0x000000ff);
        //    tx_buffer[i++] = (byte)((checksum >> 16) & 0x000000ff);
        //    tx_buffer[i++] = (byte)((checksum >> 8) & 0x000000ff);
        //    tx_buffer[i++] = (byte)((checksum >> 0) & 0x000000ff);


        //    //handle with it on Form class
        //    current_form.send_serial_port_data(tx_buffer, 0, i);

        //    return 0;
        //}

        public uint calculate_checksum(byte[] buf)
        {
            uint i;
            uint checksum = 0 ^ 0xFFFFFFFF;
            for (i = 0; i < (uint)buf.Length; i++)
                checksum = (checksum >> 8) ^ checksum_table[((checksum ^ buf[i]) & 0x000000FF)];

            return (checksum ^ 0xFFFFFFFF);
        }

        ////Handle with received bytes
        //internal void handle_with_received_bytes(byte[] rx_buffer, MainWindow mainForm)
        //{
        //    uint data_size = rx_buffer[2];

        //    uint i = 0;
        //    byte[] data = new byte[data_size];
        //    for (i = 0; i < data_size; i++)
        //        data[i] = rx_buffer[i + 3];

        //    uint chksum_rcvd = ((uint)(rx_buffer[i + 3] << 24) & 0xFF000000) |
        //                        (uint)((rx_buffer[i + 3 + 1] << 16) & 0x00FF0000) |
        //                        (uint)((rx_buffer[i + 3 + 2] << 8) & 0x0000FF00) |
        //                        (uint)((rx_buffer[i + 3 + 3] << 0) & 0x000000FF);

        //    // MSG ID is equal to ask_id
        //    if(rx_buffer[5] == check_id_message[0])
        //    {
        //        int control_panel_id = (rx_buffer[3] << 8) + rx_buffer[4]; // save control_panel_id; TODO: Add it to control panel class attribute
        //        //uint serial_number = (uint)((rx_buffer[5] << 24) + (rx_buffer[6] << 16) + (rx_buffer[7] << 8) + rx_buffer[8]); // save control_panel_id; TODO: Add it to control panel class attribute
        //        //int hardware_version = (rx_buffer[9] << 8) + rx_buffer[10]; // save control_panel_id; TODO: Add it to control panel class attribute
        //        //int software_version = (rx_buffer[11] << 8) + rx_buffer[12]; // save control_panel_id; TODO: Add it to control panel class attribute


        //        //Write it in a debug label
        //        //mainForm.write_control_panel_id_label(control_panel_id, hardware_version, software_version, serial_number);
        //    }
        //}
    }
}
