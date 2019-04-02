using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Outputs
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
       new Dictionary<string, Dictionary<string, object>>
       {
            {
                "device_id",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 0 }
                }

            },
            {
                "index",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 1 }
                }
            },
            {
                "output_on_delay",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 2 }
                }
            },
            {
                "output_on_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 4 }
                }
            },
            {
                "reset_state_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 6 }
                }
            },
            {
                "chime_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 8 }
                }
            },
            {
                "chime_delay",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 10 }
                }
            },
            {
                "pulses_number",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 12 }
                }
            },
            {
                "disarm_beeps",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 13 }
                }
            },
            {
                "arm_beeps",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 14 }
                }
            },
            {
                "timezones",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 15 }
                }
            },
            {
                "Time_on",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 16 }
                }
            },
            {
                "Time_off",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 18 }
                }
            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 24 }, // Should be 20---off course TODO: Test it
                    { "KP_OUTPUTS_OPCOES_SAIDA_INVERTIDA", new ulong[2] { 0x01, 4} },
                    { "KP_OUTPUTS_OPCOES_SAIDA_COM_IMPULSO", new ulong[2] { 0x02, 4} },
                    { "KP_OUTPUTS_OPCOES_SAIDA_SOURCE", new ulong[2] { 0x04, 2} } // unused
                }
            },
            {
                "description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 32 },
                }
            }
       };

        public void read(MainWindow mainWindow, uint output_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint output_address = Constants.KP_OUTPUTS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_OUTPUTS_FLASH * output_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((output_address >> 16) & 0xff);
            byte_array[i++] = (byte)((output_address >> 8) & 0xff);
            byte_array[i++] = (byte)((output_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow);
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainWindow, uint output_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            uint output_address = Constants.KP_OUTPUTS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_OUTPUTS_FLASH * output_number);

            General protocol = new General();
            
            if (output_number < Constants.KP_MAX_OUTPUTS)
            { 
                string description = "";
                try
                {
                    description = ((string)mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Description"]).ToUpper();
                }
                catch
                { }

                #region Timezones
                byte timezones = 0;
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone1"].Equals(true))
                {
                    timezones += 0x01;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone2"].Equals(true))
                {
                    timezones += 0x02;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone3"].Equals(true))
                {
                    timezones += 0x04;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone4"].Equals(true))
                {
                    timezones += 0x08;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone5"].Equals(true))
                {
                    timezones += 0x10;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone6"].Equals(true))
                {
                    timezones += 0x20;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone7"].Equals(true))
                {
                    timezones += 0x40;
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Timezone8"].Equals(true))
                {
                    timezones += 0x80;
                }
                #endregion

                byte[] output_device_id = (byte[])mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Device Id"];
                byte[] output_index = (byte[])mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Output index"];
                short output_delay_ON = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Output delay - ON"];
                short output_time_ON = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Time - ON"];
                short output_reset_time = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Reset time"];
                short output_chime_time = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Chime time"];
                short output_chime_delay = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Chime delay"];
                byte output_pulses_number = (byte)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Pulses number"];
                byte output_disarm_beeps = (byte)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Beeps number - Disarm"];
                byte output_arm_beeps = (byte)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Beeps number - Arm"];
                short output_pulse_time_on = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Pulse time on"];
                short output_pulse_time_off = (short)mainWindow.databaseDataSet.Output.Rows[(int)(output_number)]["Pulse time off"];
                byte[] timezones_bytes = BitConverter.GetBytes(timezones);
                byte[] description_bytes = new byte[64];
                description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

                ////Output options
                ulong output_options = 0;
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Inverted"].Equals(true))
                {
                    output_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_OUTPUTS_OPCOES_SAIDA_INVERTIDA"])[0]));
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Pulsed"].Equals(true))
                {
                    output_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_OUTPUTS_OPCOES_SAIDA_COM_IMPULSO"])[0]));
                }
                if (mainWindow.databaseDataSet.Output.Rows[(int)output_number]["Source"].Equals(true))
                {
                    output_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_OUTPUTS_OPCOES_SAIDA_SOURCE"])[0]));
                }

                byte[] output_delay_ON_bytes = BitConverter.GetBytes(output_delay_ON);
                byte[] output_time_ON_bytes = BitConverter.GetBytes(output_time_ON);
                byte[] output_reset_time_bytes = BitConverter.GetBytes(output_reset_time);
                byte[] output_chime_time_bytes = BitConverter.GetBytes(output_chime_time);
                byte[] output_chime_delay_bytes = BitConverter.GetBytes(output_chime_delay);
                byte[] output_pulse_time_on_bytes = BitConverter.GetBytes(output_pulse_time_on);
                byte[] output_pulse_time_off_bytes = BitConverter.GetBytes(output_pulse_time_off);
                byte[] output_options_bytes = BitConverter.GetBytes(output_options);
            

                int i = 0;
                uint j = 0;

                byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
                byte_array[i++] = (byte)((output_address >> 16) & 0xFF);
                byte_array[i++] = (byte)((output_address >> 8) & 0xFF);
                byte_array[i++] = (byte)(output_address & 0xFF);

                byte_array[i++] = 240;
                int temp = i;

                //Device ID
                for (i = (temp + (int)this.attributes["device_id"]["address"]), j = 0; i < (temp + (int)this.attributes["device_id"]["address"] + output_device_id.Length); i++, j++)
                {
                    byte_array[i] = output_device_id[j];
                }

                //Output index
                for (i = (temp + (int)this.attributes["index"]["address"]), j = 0; i < (temp + (int)this.attributes["index"]["address"] + output_index.Length); i++, j++)
                {
                    byte_array[i] = output_index[j];
                }

                //Output delay ON
                for (i = (temp + (int)this.attributes["output_on_delay"]["address"]), j = 0; i < (temp + (int)this.attributes["output_on_delay"]["address"] + output_delay_ON_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_delay_ON_bytes[j];
                }

                //Output Time ON
                for (i = (temp + (int)this.attributes["output_on_time"]["address"]), j = 0; i < (temp + (int)this.attributes["output_on_time"]["address"] + output_time_ON_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_time_ON_bytes[j];
                }

                //Output reset Time
                for (i = (temp + (int)this.attributes["reset_state_time"]["address"]), j = 0; i < (temp + (int)this.attributes["reset_state_time"]["address"] + output_reset_time_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_reset_time_bytes[j];
                }

                //Output Chime time
                for (i = (temp + (int)this.attributes["chime_time"]["address"]), j = 0; i < (temp + (int)this.attributes["chime_time"]["address"] + output_chime_time_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_chime_time_bytes[j];
                }

                //Output Chime delay
                for (i = (temp + (int)this.attributes["chime_delay"]["address"]), j = 0; i < (temp + (int)this.attributes["chime_delay"]["address"] + output_chime_delay_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_chime_delay_bytes[j];
                }

                //Output pulses number
                i = (temp + (int)this.attributes["pulses_number"]["address"]);
                byte_array[i] = output_pulses_number;
                i++;

                //Output arm beeps
                i = (temp + (int)this.attributes["arm_beeps"]["address"]);
                byte_array[i] = output_arm_beeps;
                i++;

                //Output disarm beeps
                i = (temp + (int)this.attributes["disarm_beeps"]["address"]);
                byte_array[i] = output_disarm_beeps;
                i++;

                //Reserved
                for (i = (temp + (int)this.attributes["timezones"]["address"]), j = 0; i < (temp + (int)this.attributes["timezones"]["address"] + timezones_bytes.Length); i++, j++)
                {
                    byte_array[i] = timezones_bytes[j];
                }

                //Output pulse time on
                for (i = (temp + (int)this.attributes["Time_on"]["address"]), j = 0; i < (temp + (int)this.attributes["Time_on"]["address"] + output_pulse_time_on_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_pulse_time_on_bytes[j];
                }

                //Output pulse time off
                for (i = (temp + (int)this.attributes["Time_off"]["address"]), j = 0; i < (temp + (int)this.attributes["Time_off"]["address"] + output_pulse_time_off_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_pulse_time_off_bytes[j];
                }

                //Options
                for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + output_options_bytes.Length); i++, j++)
                {
                    byte_array[i] = output_options_bytes[j];
                }

                //Description
                for (i = temp + (int)attributes["description"]["address"], j = 0; i < (temp + (int)attributes["description"]["address"] + description_bytes.Length); i++, j++)
                {
                    byte_array[i] = description_bytes[j];
                    if ((description_bytes.Length - 1) == (j))
                    {
                        for (i = i + 1; i < ((int)attributes["description"]["address"] + temp); i++)
                        {
                            byte_array[i] = 0;
                        }
                    }
                }

                byte_array[4] = (byte)(i - temp);
                //protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow);
                protocol.send_msg_block((uint)i, byte_array, output_address, mainWindow.cp_id, mainWindow, Constants.KP_FLASH_TAMANHO_DADOS_OUTPUTS_FLASH); // TODO: Check if cp_id is needed
                System.Threading.Thread.Sleep(mainWindow.intervalsleeptime);
            }
            else
            {
                int k = 0;

                byte_array[k++] = Constants.WRITE_BLOCK_CODE_START;
                byte_array[k++] = (byte)((output_address >> 16) & 0xFF);
                byte_array[k++] = (byte)((output_address >> 8) & 0xFF);
                byte_array[k++] = (byte)(output_address & 0xFF);

                byte_array[k++] = 240;

                int temp = k;

                for (k = temp; k < 240; k++)
                    byte_array[k] = 0xFF;

                byte_array[4] = (byte)(k - temp);
                protocol.send_msg_block((uint)k, byte_array, output_address, mainWindow.cp_id, mainWindow, Constants.KP_FLASH_TAMANHO_DADOS_OUTPUTS_FLASH); // TODO: Check if cp_id is needed
                System.Threading.Thread.Sleep(mainWindow.intervalsleeptime);
            }
        }
    }
}
