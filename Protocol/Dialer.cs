using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Dialer
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 0 },

                    { "KP_COMUNIC_OPCOES_ENABLE", new uint[2] { 0x01, 2} },
                    { "KP_COMUNIC_OPCOES_FAX_DEFEAT", new uint[2] { 0x02, 3} }, //not used
                    { "KP_COMUNIC_OPCOES_LINE_MONITOR", new uint[2] { 0x04, 4} }, //not used
                    { "KP_COMUNIC_OPCOES_MAKE_TEST_CALL", new uint[2] { 0x08, 5} }, //not used
                    { "KP_COMUNIC_OPCOES_reservado2", new uint[2] { 0x10, 6} }, //not used
                    { "KP_COMUNIC_OPCOES_reservado3", new uint[2] { 0x20, 7} }, //not used
                    { "KP_COMUNIC_OPCOES_reservado4", new uint[2] { 0x40, 8} }, //not used
                    { "KP_COMUNIC_OPCOES_SEND_LONG_DTMF", new uint[2] { 0x80, 9} }, //not used

                    { "KP_COMUNIC_OPCOES_STEP_NUMB_EACH_CALL", new uint[2] { 0x100, 10} },//not used
                    { "KP_COMUNIC_OPCOES_reservado5", new uint[2] { 0x200, 11} }, //not used
                    { "KP_COMUNIC_OPCOES_reservado6", new uint[2] { 0x400, 12} }, //not used
                    { "KP_COMUNIC_OPCOES_TEST_CALL_ONLY_ARMED", new uint[2] { 0x800, 13} },//not used
                    { "KP_COMUNIC_OPCOES_HOLD_DOMESTIC_FOR_DTMF", new uint[2] { 0x1000, 14} },//not used
                    { "KP_COMUNIC_OPCOES_FIRST_OPEN_LAST_CLOSE", new uint[2] { 0x2000, 15} },//not used


                    { "KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_DIAL", new uint[2] { 0x10000, 18} },//not used
                    { "KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_DIAL", new uint[2] { 0x20000, 19} },//not used
                    { "KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_DIAL", new uint[2] { 0x40000, 12} },//not used
                    { "KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_CALL", new uint[2] { 0x80000, 13} },//not used
                    { "KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_CALL", new uint[2] { 0x100000, 14} },//not used
                    { "KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_CALL", new uint[2] { 0x200000, 15} },//not used
                }
            },
            {
                "description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 4 },
                    { "data_grid_view_addr", 1 }
                }

            },
            //not used
            {
                "active",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 68 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "week_days_for_test_call",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 69 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "hour_for_test_call",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 70 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "keypad_listening_options",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 72 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "output1_listening_options",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 76 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "area_dial_delay",
                new Dictionary<string, object>
                {
                    { "value", new byte[16] },
                    { "address", 80 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "area_cancel_window",
                new Dictionary<string, object>
                {
                    { "value", new byte[16] },
                    { "address", 96 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "ring_counter_max",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 112 },
                }
            }
        };

        internal void read(MainWindow mainWindow, uint dialer_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint dialer_adress = Constants.KP_DIALERS_INIC_ADDR;// + ((Constants.KP_FLASH_TAMANHO_DADOS_DIALER_FLASH/Constants.KP_GLOBAL_SYSTEM_DIV) * (dialer_number - 1));
            byte size = 146;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((dialer_adress >> 16) & 0xff);
            byte_array[i++] = (byte)((dialer_adress >> 8) & 0xff);
            byte_array[i++] = (byte)((dialer_adress) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow);
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainWindow, uint dialer_index)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho

            #region Dialer options read from dataset
            uint dialer_options = 0;

            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Active"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_ENABLE"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Fax defeat"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_FAX_DEFEAT"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Monitor line"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LINE_MONITOR"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Make test call"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_MAKE_TEST_CALL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Reserved 2"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_reservado2"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Reserved 3"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_reservado3"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Reserved 4"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_reservado4"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Long DTMF"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_SEND_LONG_DTMF"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Step number each call"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_STEP_NUMB_EACH_CALL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Reserved 5"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_reservado5"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Reserved 6"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_reservado6"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Test call only armed"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_TEST_CALL_ONLY_ARMED"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Hold domestic for DTMF"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_HOLD_DOMESTIC_FOR_DTMF"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["First open last close"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_FIRST_OPEN_LAST_CLOSE"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["List in output disarm dial"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_DIAL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["List in output arm dial"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_DIAL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["List in output stay dial"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_DIAL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["List in output disarm call"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_CALL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["List in output arm call"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_CALL"])[0]));
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["List in output stay call"].Equals(true))
            {
                dialer_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_CALL"])[0]));
            }

            #endregion

            string description = ((string)mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Description"]).ToUpper();

            #region UNUSED
            //active communicator
            byte[] active_communicator_bytes = (byte[])mainWindow.databaseDataSet.Dialer.Rows[(int)(dialer_index)]["Active communicator"];


            //WEEK DAYS
            byte week_days_for_test_call = 0;
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Monday"].Equals(true))
            {
                week_days_for_test_call += 0x01;
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Tuesday"].Equals(true))
            {
                week_days_for_test_call += 0x02;
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Wednesday"].Equals(true))
            {
                week_days_for_test_call += 0x04;
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Thursday"].Equals(true))
            {
                week_days_for_test_call += 0x08;
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Friday"].Equals(true))
            {
                week_days_for_test_call += 0x10;
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Saturday"].Equals(true))
            {
                week_days_for_test_call += 0x20;
            }
            if (mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["Sunday"].Equals(true))
            {
                week_days_for_test_call += 0x40;
            }

            //hour_for_test_call
            byte[] hour_for_test_call_bytes = (byte[])mainWindow.databaseDataSet.Dialer.Rows[(int)(dialer_index)]["Test call hour"];

            //keypad_listening_options
            byte[] keypad_listening_options_bytes = (byte[])mainWindow.databaseDataSet.Dialer.Rows[(int)(dialer_index)]["Keypad listening options"];

            //output1_listening_options
            byte[] output1_listening_options_bytes = (byte[])mainWindow.databaseDataSet.Dialer.Rows[(int)(dialer_index)]["Output 1 listening options"];

            //area_dial_delay
            byte[] area_dial_delay_bytes = (byte[])mainWindow.databaseDataSet.Dialer.Rows[(int)(dialer_index)]["Area dial delay"];

            //area_cancel_window
            byte[] area_cancel_window_bytes = (byte[])mainWindow.databaseDataSet.Dialer.Rows[(int)(dialer_index)]["Area cancel window"];
            #endregion


            byte[] dialer_options_bytes = BitConverter.GetBytes(dialer_options);
           
            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

            string ring_counter = ((string)mainWindow.databaseDataSet.Dialer.Rows[(int)dialer_index]["RingCounter"]).ToUpper();
            byte ring_counter_max = byte.Parse(ring_counter);

            int i = 0;
            uint j = 0;
            uint dialer_address = 0x69000;
            byte_array[i++] = 0x40;
            byte_array[i++] = (byte)((dialer_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((dialer_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(dialer_address & 0xFF);
            byte_array[i++] = 240;
            int temp = i;

            //options
            for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + dialer_options_bytes.Length); i++, j++)
            {
                byte_array[i] = dialer_options_bytes[j];
            }

            //Description
            for (i = (int)attributes["description"]["address"] + temp, j = 0; i < ((int)attributes["description"]["address"] + temp + description_bytes.Length); i++, j++)
            {
                byte_array[i] = description_bytes[j];
            }

            #region unused
            //Active communicator - unused
            for (i = (temp + (int)this.attributes["active"]["address"]), j = 0; i < (temp + (int)this.attributes["active"]["address"] + active_communicator_bytes.Length); i++, j++)
            {
                byte_array[i] = active_communicator_bytes[j];
            }

            //week days for test call
            i = (temp + (int)this.attributes["week_days_for_test_call"]["address"]);
            byte_array[i] = week_days_for_test_call;
            i++;


            //hour_for_test_call
            for (i = (temp + (int)this.attributes["hour_for_test_call"]["address"]), j = 0; i < (temp + (int)this.attributes["hour_for_test_call"]["address"] + hour_for_test_call_bytes.Length); i++, j++)
            {
                byte_array[i] = hour_for_test_call_bytes[j];
            }

            //keypad_listening_options
            for (i = (temp + (int)this.attributes["keypad_listening_options"]["address"]), j = 0; i < (temp + (int)this.attributes["keypad_listening_options"]["address"] + keypad_listening_options_bytes.Length); i++, j++)
            {
                byte_array[i] = keypad_listening_options_bytes[j];
            }

            //output1_listening_options
            for (i = (temp + (int)this.attributes["output1_listening_options"]["address"]), j = 0; i < (temp + (int)this.attributes["output1_listening_options"]["address"] + output1_listening_options_bytes.Length); i++, j++)
            {
                byte_array[i] = output1_listening_options_bytes[j];
            }

            //area_dial_delay
            for (i = (temp + (int)this.attributes["area_dial_delay"]["address"]), j = 0; i < (temp + (int)this.attributes["area_dial_delay"]["address"] + area_dial_delay_bytes.Length); i++, j++)
            {
                byte_array[i] = area_dial_delay_bytes[j];
            }

            //area_cancel_window
            for (i = (temp + (int)this.attributes["area_cancel_window"]["address"]), j = 0; i < (temp + (int)this.attributes["area_cancel_window"]["address"] + area_cancel_window_bytes.Length); i++, j++)
            {
                byte_array[i] = area_cancel_window_bytes[j];
            }
            #endregion

            //Ring counter max
            for (i = (int)attributes["ring_counter_max"]["address"] + temp, j = 0; i < ((int)attributes["ring_counter_max"]["address"] + temp + 1); i++, j++)
            {
                byte_array[i] = ring_counter_max;
            }

            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow);
        }
    }
}
