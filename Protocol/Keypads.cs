using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Keypads
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 0 }
                }

            },
            {
                "reserved_0",
                new Dictionary<string, object>
                {
                    { "value", new byte[16] },
                    { "address", 64 }
                }
            },
            {
                "reserved_2",
                new Dictionary<string, object>
                {
                    { "value", new byte[16] },
                    { "address", 80 }
                }
            },
            {
                "reserved_1",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 96 }
                }
            },
         
            {
                "reserved_3",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 97 }
                }
            },
            {
                "reserved_4",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 98 }
                }
            },
            {
                "partition_id",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 99 }
                }
            },
            {
                "report_partition_id",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 100 }
                }
            },
            { // Old reserved_5
                "lcd_backlight_timeout",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 101 }
                }
            },
            {
                "reserved_6",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 102 }
                }
            },
            {
                "reserved_7",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 103 }
                }
            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 104 },
                    //OPTIONS 1                                                                         //----
                    { "KP_KPAD_RS485_ATIVO", new ulong[2] { 0x01, 63} },                                //bit0
                    { "KP_KPAD_TIPO_HORA", new ulong[2] { 0x02, 1} },                                   //bit1     
                    { "KP_KPAD_TIPO_DATA", new ulong[2] { 0x04, 2} },                                   //bit2
                    { "KP_KPAD_IDIOMA", new ulong[2] { 0x08, 0} },                                      //bit3
                    { "KP_KPAD_BACKLIGHT_OFF_WHEN_ARMED", new ulong[2] { 0x10, 4} },                    //bit4 
                    { "KP_KPAD_BACKLIGHT_OFF_MAIN_POWER_FAIL", new ulong[2] { 0x20, 5} },               //bit5
                    { "KP_KPAD_ENABLE_KEYPAD_TAMPER", new ulong[2] { 0x40, 26} },                       //bit6
                    { "KP_KPAD_NO_KEYPAD_INDICATIONS_WHILE_ARMED", new ulong[2] { 0x80, 27} },          //bit7
                    //OPTIONS 2                                                                         //----
                    { "KP_KPAD_BEEPS_FIRE", new ulong[2] { 0x100, 8} },                                 //bit8                    
                    { "KP_KPAD_BEEPS_PANIC", new ulong[2] { 0x200, 9} },                                //bit9
                    { "KP_KPAD_BEEPS_MAIN_POWER_FAIL", new ulong[2] { 0x400, 10} },                     //bit10
                    { "KP_KPAD_BEEPS_OUTPUT_FUSE_FAIL", new ulong[2] { 0x800, 11} },                    //bit11
                    { "KP_KPAD_BEEPS_BATTERY_LOW", new ulong[2] { 0x1000, 12} },                        //bit12
                    { "KP_KPAD_BEEPS_LINE_FAIL", new ulong[2] { 0x2000, 13} },                          //bit13
                    { "KP_KPAD_BEEPS_SYSTEM_TAMPER", new ulong[2] { 0x4000, 14} },                      //bit14
                    { "KP_KPAD_BEEPS_RECEIVER_FAILS", new ulong[2] { 0x8000, 15} },                     //bit15
                    //OPTIONS 3                                                                         //----                                                                                    //----
                    { "KP_KPAD_BEEPS_FUNCOES", new ulong[2] { 0x10000, 32} },                           //bit16
                    { "KP_KPAD_BEEPS_EXIT_TIME", new ulong[2] { 0x20000, 33} },                         //bit17
                    { "KP_KPAD_BEEPS_ENTRY_TIME", new ulong[2] { 0x40000, 34} },                        //bit18
                    { "KP_KPAD_BEEPS_SENSOR_WATCH", new ulong[2] { 0x80000, 36} },                      //bit19
                    { "KP_KPAD_BEEPS_ALARM", new ulong[2] { 0x100000, 6} },                             //bit20
                    { "KP_KPAD_BEEPS_MEDICAL", new ulong[2] { 0x200000, 7} },                           //bit21
                                                                                                        //bit22
                                                                                                        //bit23
                    //OPTIONS 4                                                                         //----
                    { "KP_KPAD_CAN_ARM_AWAY", new ulong[2] { 0x1000000, 16 } },                         //bit24
                    { "KP_KPAD_CAN_ARM_STAY", new ulong[2] { 0x2000000, 17} },                          //bit25
                    { "KP_KPAD_DISARM_AWAY_ALL_TIMES", new ulong[2] { 0x4000000, 18} },                 //bit26
                    { "KP_KPAD_DISARM_STAY_ALL_TIMES", new ulong[2] { 0x8000000, 19} },                 //bit27
                    { "KP_KPAD_DISARM_AWAY_DURING_EXIT_TIME", new ulong[2] { 0x10000000, 20} },         //bit28 
                    { "KP_KPAD_DISARM_STAY_DURING_EXIT_TIME", new ulong[2] { 0x20000000, 21} },         //bit29
                    { "KP_KPAD_SILENT_DURING_ENTRY_TIME", new ulong[2] { 0x40000000, 22} },             //bit30
                    { "KP_KPAD_SILENT_DURING_EXIT_TIME", new ulong[2] {  0x80000000, 23} },             //bit31
                    //OPTIONS 5                                                                         //----
                    { "KP_KPAD_RESET_ALARMS", new ulong[2] { 0x100000000, 24} },                        //bit32
                    { "KP_KPAD_SHOW_AREA_NAME", new ulong[2] { 0x200000000, 25} }                       //bit33
                }
            },
            { // TODO: Used but should be implemented later
                "language",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 112 }
                }
            },
            {
                "reserved_8",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 113 }
                }
            },
            {
                "reserved_9",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 114 }
                }
            },
            {
                "reserved_10",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 115 }
                }
            },
          
          
            {
                "lcd_config_options",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 116 }
                }
            },
            {
                "audio_tracks",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 124 }
                }
            },
        }; 

        public void read(MainWindow mainForm, uint keypad_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint keypad_address = Constants.KP_KEYPADS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_KEYPAD_FLASH * keypad_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((keypad_address >> 16) & 0xff);
            byte_array[i++] = (byte)((keypad_address >> 8) & 0xff);
            byte_array[i++] = (byte)((keypad_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainWindow, uint keypad_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho

            string description = ((string)mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Description"]).ToUpper();

            #region Partition id
            short partition_id = 0;
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 1"].Equals(true))
            {
                partition_id += 0x01;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 2"].Equals(true))
            {
                partition_id += 0x02;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 3"].Equals(true))
            {
                partition_id += 0x04;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 4"].Equals(true))
            {
                partition_id += 0x08;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 5"].Equals(true))
            {
                partition_id += 0x10;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 6"].Equals(true))
            {
                partition_id += 0x20;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 7"].Equals(true))
            {
                partition_id += 0x40;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Area 8"].Equals(true))
            {
                partition_id += 0x80;
            }
            #endregion

            #region Report Partition id
            short report_partition_id = 0;
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 1"].Equals(true))
            {
                report_partition_id += 0x01;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 2"].Equals(true))
            {
                report_partition_id += 0x02;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 3"].Equals(true))
            {
                report_partition_id += 0x04;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 4"].Equals(true))
            {
                report_partition_id += 0x08;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 5"].Equals(true))
            {
                report_partition_id += 0x10;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 6"].Equals(true))
            {
                report_partition_id += 0x20;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 7"].Equals(true))
            {
                report_partition_id += 0x40;
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Report area 8"].Equals(true))
            {
                report_partition_id += 0x80;
            }
            #endregion

            //Lcd backlight timeout
            byte[] lcd_backlight_timeout = new byte[1];
            lcd_backlight_timeout[0] = (byte)mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Backlight time"];


            #region Keypad options read from dataset
            ////KEYPAD OPTIONS
            ulong keypad_options = 0;
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Language option"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_IDIOMA"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Hour format"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_TIPO_HORA"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Date format"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_TIPO_DATA"])[0]));
            }
            
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Backlight off when armed"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BACKLIGHT_OFF_WHEN_ARMED"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Backlight off on 230V fail"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BACKLIGHT_OFF_MAIN_POWER_FAIL"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on alarm"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on medical alarm"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_MEDICAL"])[0]));
            }

            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on fire alarm"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_FIRE"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on panic alarm"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_PANIC"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on 230V fail"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_MAIN_POWER_FAIL"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on output fuse fail"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_OUTPUT_FUSE_FAIL"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on battery low"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_BATTERY_LOW"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on PSTN fail"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_LINE_FAIL"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on system tamper alarm"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_SYSTEM_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on receiver fail"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_RECEIVER_FAILS"])[0]));
            }

            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Can arm away"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_CAN_ARM_AWAY"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Can arm stay"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_CAN_ARM_STAY"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Can disarm away"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_DISARM_AWAY_ALL_TIMES"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Can disarm stay"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_DISARM_STAY_ALL_TIMES"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Can disarm away during exit time"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_DISARM_AWAY_DURING_EXIT_TIME"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Can disarm stay during exit time"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_DISARM_STAY_DURING_EXIT_TIME"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Silent during entry time"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_SILENT_DURING_ENTRY_TIME"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Silent during exit time"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_SILENT_DURING_EXIT_TIME"])[0]));
            }

            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Reset alarms"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_RESET_ALARMS"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Show area name"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_SHOW_AREA_NAME"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Enable keypad tamper"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_ENABLE_KEYPAD_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["No indications while armed"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_NO_KEYPAD_INDICATIONS_WHILE_ARMED"])[0]));
            }

            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on functions"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_FUNCOES"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on exit time"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_EXIT_TIME"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on entry time"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_ENTRY_TIME"])[0]));
            }
            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Beeps on sensor watch"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_BEEPS_SENSOR_WATCH"])[0]));
            }

            if (mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Active"].Equals(true))
            {
                keypad_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_KPAD_RS485_ATIVO"])[0]));
            }
            #endregion

            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

            byte[] reserved_0_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 0"];
            byte[] reserved_1_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 1"];
            byte[] reserved_2_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 2"];
            byte[] reserved_3_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 3"];
            byte[] reserved_4_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 4"];
            byte[] keypad_partitions_bytes = BitConverter.GetBytes(partition_id);
            byte[] keypad_report_partitions_bytes = BitConverter.GetBytes(report_partition_id);

            byte[] reserved_6_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 6"];
            byte[] reserved_7_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 7"];
            byte[] reserved_8_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 8"];
            byte[] reserved_9_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 9"];
            byte[] reserved_10_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Reserved 10"];

            byte[] keypad_options_bytes = BitConverter.GetBytes(keypad_options);
            byte[] language_bytes = (byte[])mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["Language"];

            byte[] lcd_config_options_bytes = (byte[])attributes["lcd_config_options"]["value"];


            // Function buttons configuration
            short f1_button_value = (short)mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["F1 button"];
            byte[] f1_button_value_byte = new byte[2];
            f1_button_value_byte[0] = (byte)(f1_button_value >> 8);
            f1_button_value_byte[1] = (byte)(f1_button_value & 0x00FF);

            short f2_button_value = (short)mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["F2 button"];
            byte[] f2_button_value_byte = new byte[2];
            f2_button_value_byte[0] = (byte)(f2_button_value >> 8);
            f2_button_value_byte[1] = (byte)(f2_button_value & 0x00FF);

            short f3_button_value = (short)mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["F3 button"];
            byte[] f3_button_value_byte = new byte[2];
            f3_button_value_byte[0] = (byte)(f3_button_value >> 8);
            f3_button_value_byte[1] = (byte)(f3_button_value & 0x00FF);

            short f4_button_value = (short)mainWindow.databaseDataSet.Keypad.Rows[(int)(keypad_number)]["F4 button"];
            byte[] f4_button_value_byte = new byte[2];
            f4_button_value_byte[0] = (byte)(f4_button_value >> 8);
            f4_button_value_byte[1] = (byte)(f4_button_value & 0x00FF);

            lcd_config_options_bytes[0] = f1_button_value_byte[1];
            lcd_config_options_bytes[1] = f1_button_value_byte[0];
            lcd_config_options_bytes[2] = f2_button_value_byte[1];
            lcd_config_options_bytes[3] = f2_button_value_byte[0];
            lcd_config_options_bytes[4] = f3_button_value_byte[1];
            lcd_config_options_bytes[5] = f3_button_value_byte[0];
            lcd_config_options_bytes[6] = f4_button_value_byte[1];
            lcd_config_options_bytes[7] = f4_button_value_byte[0];


            #region AUDIO TRACKS
            byte[] audio_tracks = new byte[8];
            audio_tracks[0] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 1"].ToString()) - 1) & 0xFF);
            audio_tracks[1] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 1"].ToString()) - 1) >> 8);
            audio_tracks[2] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 2"].ToString()) - 1) & 0xFF);
            audio_tracks[3] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 2"].ToString()) - 1) >> 8);
            audio_tracks[4] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 3"].ToString()) - 1) & 0xFF);
            audio_tracks[5] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 3"].ToString()) - 1) >> 8);
            audio_tracks[6] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 4"].ToString()) - 1) & 0xFF);
            audio_tracks[7] = (byte)((int.Parse(mainWindow.databaseDataSet.Keypad.Rows[(int)keypad_number]["Audio track 4"].ToString()) - 1) >> 8);
            #endregion

            int i = 0;
            uint j = 0;
            uint keypad_address = Constants.KP_KEYPADS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_KEYPAD_FLASH * keypad_number);
            byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
            byte_array[i++] = (byte)((keypad_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((keypad_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(keypad_address & 0xFF);

            byte_array[i++] = 240;
            int temp = i;
            //TODO: Create a function for this for 

            //Description
            for (i = temp, j = 0; i < (temp + description_bytes.Length); i++, j++)
            {
                byte_array[i] = description_bytes[j];
                if ((description_bytes.Length - 1) == (j))
                {
                    for (i = i + 1; i < (64 + temp); i++)
                    {
                        byte_array[i] = 0;
                    }
                }
            }

            //Reserved 0
            for (i = (temp + (int)this.attributes["reserved_0"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_0"]["address"] + reserved_0_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_0_bytes[j];
            }
            //Reserved 1
            for (i = (temp + (int)this.attributes["reserved_1"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_1"]["address"] + reserved_1_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_1_bytes[j];
            }
           
            //Reserved 2
            for (i = (temp + (int)this.attributes["reserved_2"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_2"]["address"] + reserved_2_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_2_bytes[j];
            }
            //Reserved 3
            for (i = (temp + (int)this.attributes["reserved_3"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_3"]["address"] + reserved_3_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_3_bytes[j];
            }
            //Reserved 4
            for (i = (temp + (int)this.attributes["reserved_4"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_4"]["address"] + reserved_4_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_4_bytes[j];
            }

            //partition_id
            for (i = (temp + (int)this.attributes["partition_id"]["address"]), j = 0; i < (temp + (int)this.attributes["partition_id"]["address"] + keypad_partitions_bytes.Length); i++, j++)
            {
                byte_array[i] = keypad_partitions_bytes[j];
            }

            //report_partition_id
            for (i = (temp + (int)this.attributes["report_partition_id"]["address"]), j = 0; i < (temp + (int)this.attributes["report_partition_id"]["address"] + keypad_report_partitions_bytes.Length); i++, j++)
            {
                byte_array[i] = keypad_report_partitions_bytes[j];
            }

            //lcd_backlight_timeout
            for (i = (temp + (int)this.attributes["lcd_backlight_timeout"]["address"]), j = 0; i < (temp + (int)this.attributes["lcd_backlight_timeout"]["address"] + lcd_backlight_timeout.Length); i++, j++)
            {
                byte_array[i] = lcd_backlight_timeout[j];
            }

            //Reserved 6
            for (i = (temp + (int)this.attributes["reserved_6"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_6"]["address"] + reserved_6_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_6_bytes[j];
            }
            //Reserved 7
            for (i = (temp + (int)this.attributes["reserved_7"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_7"]["address"] + reserved_7_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_7_bytes[j];
            }

            //Reserved 8
            for (i = (temp + (int)this.attributes["reserved_8"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_8"]["address"] + reserved_8_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_8_bytes[j];
            }
            //Reserved 9
            for (i = (temp + (int)this.attributes["reserved_9"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_9"]["address"] + reserved_9_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_9_bytes[j];
            }
            //Reserved 10
            for (i = (temp + (int)this.attributes["reserved_10"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_10"]["address"] + reserved_10_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_10_bytes[j];
            }

            //Options
            for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + keypad_options_bytes.Length); i++, j++)
            {
                byte_array[i] = keypad_options_bytes[j];
            }

            //Language
            for (i = (temp + (int)this.attributes["language"]["address"]), j = 0; i < (temp + (int)this.attributes["language"]["address"] + language_bytes.Length); i++, j++)
            {
                byte_array[i] = language_bytes[j];
            }

            //Lcd config options
            for (i = (temp + (int)this.attributes["lcd_config_options"]["address"]), j = 0; i < (temp + (int)this.attributes["lcd_config_options"]["address"] + lcd_config_options_bytes.Length); i++, j++)
            {
                byte_array[i] = lcd_config_options_bytes[j];
            }

            //Audio tracks
            for (i = ((int)attributes["audio_tracks"]["address"] + temp), j = 0; i < ((int)attributes["audio_tracks"]["address"] + temp + (audio_tracks.Length)); i++, j++)
            {
                byte_array[i] = audio_tracks[j];
            }

            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            //protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is needed
            protocol.send_msg_block((uint)i, byte_array, keypad_address, mainWindow.cp_id, mainWindow, (int)Constants.KP_FLASH_TAMANHO_DADOS_KEYPAD_FLASH); // TODO: Check if cp_id is needed
            System.Threading.Thread.Sleep(mainWindow.intervalsleeptime);
        }
    }
}
