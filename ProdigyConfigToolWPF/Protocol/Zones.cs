
using ProdigyConfigToolWPF;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ProdigyConfigToolWPF.Protocol
{
    class Zones
    {
        public enum ZoneOutputs
        {
            None,
            O1,
            O2,
            O3,
            O4,
            O5,
            O6,
            O7,
            O8,
            O9,
            O10,
            O11,
            O12,
            O13,
        }

        public List<KeyValuePair<string, int>> list = new List<KeyValuePair<string, int>>()
        {
            new KeyValuePair<string, int>("None", 0),
            new KeyValuePair<string, int>("O1", 1),
            new KeyValuePair<string, int>("O2", 2),
            new KeyValuePair<string, int>("O3", 3),
            new KeyValuePair<string, int>("O4", 4),
            new KeyValuePair<string, int>("O5", 5),
            new KeyValuePair<string, int>("O6", 6),
            new KeyValuePair<string, int>("O7", 7),
            new KeyValuePair<string, int>("O8", 8),
            new KeyValuePair<string, int>("O9", 9),
            new KeyValuePair<string, int>("O10", 10),
            new KeyValuePair<string, int>("O11", 11),
            new KeyValuePair<string, int>("O12", 12),
            new KeyValuePair<string, int>("O13", 13),
        };


        public Dictionary<string, Dictionary<string, object>> attributes =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "terminal_configuration",
                new Dictionary<string, object>
                {
                    { "value", new byte[7] },
                    { "address", 0 },
                    { "r1_configuration", new int[2] { 0, 51} },
                    { "r1_type", new int[2] { 1, 50 } },
                    { "r2_configuration", new int[2] { 2, 54 } },
                    { "r2_type", new int[2] { 3, 53 } },
                    { "r3_configuration", new int[2] { 4, 57 } },
                    { "r3_type", new int[2] { 5, 56 } },
                    { "circuit_configuration", new int[2] { 6, 49 } },
                    { "data_grid_view_addr", 49 }
                }
            },
            {
                "reserved_1",//changed read
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 7 },
                    { "data_grid_view_addr", 1 }
                }
            },
            {
                "description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 8 },
                    { "data_grid_view_addr", 1 }
                }
            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] }, 
                    { "address", 72 },
                    { "zone_active_mask", new long[2] { 0x01, 4} },                                 //bit0
                                                                                                    //bit1
                    { "zone_key_switch_mask", new long[2] { 0x04, 5 } },                            //bit2
                    { "zone_key_switch_type_mask", new long[2] { 0x08, 6 } },                       //bit3
                    { "zone_visible_keypad_mask", new long[2] { 0x10, 24 } },                       //bit4
                    { "zone_auto_bypass_mask", new long[2] { 0x20, 9 } },                           //bit5
                    { "zone_always_bypass_mask", new long[2] { 0x40, 7 } },                         //bit6
                    { "zone_manual_bypass_mask", new long[2] { 0x80, 8 } },                         //bit7
                                                                                                    //-----
                    { "zone_radio_mask", new long[2] { 0x100, 1 } },                                //bit8
                                                                                                    //bit9
                    { "zone_handover_mask", new long[2] { 0x400, 15 } },                            //bit10
                    { "zone_two_trigger_mask", new long[2] { 0x800, 16 } },                         //bit11
                    { "zone_24_hours_mask", new long[2] { 0x1000, 0 } },                            //bit12
                    { "zone_24_hours_auto_reset_mask", new long[2] { 0x2000, 0 } },                 //bit13
                    { "zone_24_hours_firezone_mask", new long[2] { 0x4000, 0 } },                   //bit14
                    { "zone_chime_arm_mask", new long[2] { 0x8000, 18 } },                          //bit15
                                                                                                    //-----
                    { "zone_chime_disarm_mask", new long[2] { 0x10000, 19 } },                      //bit16
                    { "zone_arm_if_not_ready_mask", new long[2] { 0x20000, 13 } },                  //bit17
                    { "zone_send_multiple_reports_via_dealer_mask", new long[2] { 0x40000, 0 } },   //bit18
                    { "zone_sensor_watch_mask", new long[2] { 0x80000, 22 } },                      //bit19
                    { "zone_soak_test_mask", new long[2] { 0x100000, 10 } },                        //bit20
                    { "zone_exit_terminator_mask", new long[2] { 0x200000, 20 } },                  //bit21
                    { "zone_silent_mask", new long[2] { 0x400000, 14 } },                           //bit22
                                                                                                    //bit23
                                                                                                    //-----
                    { "zone_report_to_area_account_mask", new long[2] { 0x1000000 , 11 } },         //bit24
                    { "zone_do_not_report_24hours_alarm_mask", new long[2] { 0x2000000, 0 } },      //bit25
                    { "zone_always_report_to_gsm_mask", new long[2] { 0x4000000, 12 } },            //bit26
                    
                }
            },
            {
                "reserved_2",//changed read
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 80 },
                    { "data_grid_view_addr", 21 }
                }
            },
            {
                "terminator_count",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 81 },
                    { "data_grid_view_addr", 21 }
                }
            },
            {
                "two_trigger_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 82 },
                    { "data_grid_view_addr", 17 }
                }
            },
            {
                "reserved_3",//changed read
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 83 },
                    { "data_grid_view_addr", 17 }
                }
            },
            {
                "away_entry_delay_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 84 },
                    { "data_grid_view_addr", 2 }
                }
            },
            {
                "stay_entry_delay_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 86 },
                    { "data_grid_view_addr", 3 }
                }
            },
            {
                "sensor_watch_timer",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 88 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "temperature_normal_low",//changed
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 90 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "temperature_normal_high",//changed
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 92 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "temperature_alarm_low",//changed
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 94 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "temperature_alarm_high",//changed
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 96 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "reserved_4",//changed read
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 98 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "report_code",//changed
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 100 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "partitions_away",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 104 },
                    { "data_grid_view_addr", 25 }
                }
            },
            {
                "partitions_stay",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 105 },
                    { "data_grid_view_addr", 33 }
                }
            },
            {
                "show_keypads",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 106 },
                    { "data_grid_view_addr", 41 }
                }
            },
            {
                "reserved_5",//changed read
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 107 },
                    { "data_grid_view_addr", 23 }
                }
            },
            {
                "zone_alarm_to_outputs",
                new Dictionary<string, object>
                {
                    { "value", new byte[84] },
                    { "address", 108 },
                    { "number_of_options_by_output", 3 },
                    { "tamper",new int[2] { 0, 80 } },
                    { "anti_mask", new int[2] { 3, 68 } },

                    { "arm_away_timezone", new int[2] { 6, 68 } },//changed
                    { "arm_away_code", new int[2] { 9, 68 } },//changed
                    { "arm_away_command", new int[2] { 12, 68 } },//changed
                    { "arm_away_key_switch", new int[2] { 15, 68 } },//changed
                    { "arm_away_remote", new int[2] { 18, 68 } },//changed
                    { "arm_stay_timezone", new int[2] { 21, 68 } },//changed
                    { "arm_stay_code", new int[2] { 24, 68 } },//changed
                    { "arm_stay_command", new int[2] { 27, 68 } },//changed
                    { "arm_stay_key_switch", new int[2] { 30, 68 } },//changed
                    { "arm_stay_remote", new int[2] { 33, 68 } },//changed
                    { "disarm_away_timezone", new int[2] { 36, 68 } },//changed
                    { "disarm_away_code", new int[2] { 39, 68 } },//changed
                    { "disarm_away_command", new int[2] { 42, 68 } },//changed
                    { "disarm_away_key_switch", new int[2] { 45, 68 } },//changed
                    { "disarm_away_remote", new int[2] { 48, 68 } },//changed
                    { "disarm_stay_timezone", new int[2] { 51, 68 } },//changed
                    { "disarm_stay_code", new int[2] { 54, 68 } },//changed
                    { "disarm_stay_command", new int[2] { 57, 68 } },//changed
                    { "disarm_stay_key_switch", new int[2] { 60, 68 } },//changed
                    { "disarm_stay_remote", new int[2] { 63, 68 } },//changed
                    

                    { "24h",new int[2] { 66, 71 } },
                    { "fire",new int[2] { 69,74 } },
                    { "entry_delay", new int[2] { 72, 65 } },
                    { "alarm",new int[2] { 75, 77 } },
                    { "chime", new int[2] { 78, 59 } },
                    { "sensor_watch", new int[2] { 81, 62 } },
                }
            },
            {
                "audio_tracks", //changed
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 192 },
                    { "data_grid_view_addr", 23 }
                }
            },
        };

        public void read(MainWindow mainForm, uint zone_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint zone_address = Constants.KP_ZONES_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_ZONAS_FLASH * (zone_number - 1));
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = Constants.READ_CODE;
            byte_array[i++] = (byte)((zone_address >> 16) & 0xff);
            byte_array[i++] = (byte)((zone_address >> 8) & 0xff);
            byte_array[i++] = (byte)((zone_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
        }

        public void Write(MainWindow mainform, uint zone_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            zone_number = zone_number - 1;
            string description = ((string)mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Description"]).ToUpper();
            short away_entry_delay_time = short.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Entry time away"].ToString());
            short stay_entry_delay_time = short.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Entry time stay"].ToString());
            short two_trigger_time = short.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Trigger Time"].ToString());
            short sensor_watch_timer = short.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Sensor watch time"].ToString());
            short terminator_count = short.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Terminator count"].ToString());

            #region Zone options read from dataset
            ////Zone OPTIONS
            ulong zone_options = 0;
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Zone active"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_active_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Keyswitch zone"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_key_switch_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Keyswitch type"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_key_switch_type_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Keypad visible"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_visible_keypad_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Radio zone"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_radio_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Always bypass"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_always_bypass_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Manual bypass"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_manual_bypass_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Auto bypass"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_auto_bypass_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Handover"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_handover_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Two trigger"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_two_trigger_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["24h zone"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_24_hours_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["24h zone - auto-reset"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_24_hours_auto_reset_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["24h zone - firezone"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_24_hours_firezone_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Chime - arm"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_chime_arm_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Chime - disarm"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_chime_disarm_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Arm if not ready"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_arm_if_not_ready_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Send multiple reports to dialer"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_send_multiple_reports_via_dealer_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Sensor watch"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_sensor_watch_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Soak test"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_soak_test_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Report to account"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_report_to_area_account_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Dont report 24h zone"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_do_not_report_24hours_alarm_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Exit terminator"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_exit_terminator_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Silent zone"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_silent_mask"])[0]));
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Always report"].Equals(true))
            {
                zone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["zone_always_report_to_gsm_mask"])[0]));
            }

            #endregion

            #region Zone PARTITIONS FLAGS
            byte zone_partitions_away = 0;
            byte zone_partitions_stay = 0;
            //Zones partitions away
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 1"].Equals(true))
            {
                zone_partitions_away += 0x01;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 2"].Equals(true))
            {
                zone_partitions_away += 0x02;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 3"].Equals(true))
            {
                zone_partitions_away += 0x04;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 4"].Equals(true))
            {
                zone_partitions_away += 0x08;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 5"].Equals(true))
            {
                zone_partitions_away += 0x10;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 6"].Equals(true))
            {
                zone_partitions_away += 0x20;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 7"].Equals(true))
            {
                zone_partitions_away += 0x40;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area away 8"].Equals(true))
            {
                zone_partitions_away += 0x80;
            }
            //Zones partitions stay
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 1"].Equals(true))
            {
                zone_partitions_stay += 0x01;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 2"].Equals(true))
            {
                zone_partitions_stay += 0x02;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 3"].Equals(true))
            {
                zone_partitions_stay += 0x04;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 4"].Equals(true))
            {
                zone_partitions_stay += 0x08;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 5"].Equals(true))
            {
                zone_partitions_stay += 0x10;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 6"].Equals(true))
            {
                zone_partitions_stay += 0x20;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 7"].Equals(true))
            {
                zone_partitions_stay += 0x40;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Area stay 8"].Equals(true))
            {
                zone_partitions_stay += 0x80;
            }


            #endregion

            #region Zone Show keypads flag
            byte zone_show_keypads = 0;
            //Zones partitions away
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 1"].Equals(true))
            {
                zone_show_keypads += 0x01;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 2"].Equals(true))
            {
                zone_show_keypads += 0x02;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 3"].Equals(true))
            {
                zone_show_keypads += 0x04;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 4"].Equals(true))
            {
                zone_show_keypads += 0x08;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 5"].Equals(true))
            {
                zone_show_keypads += 0x10;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 6"].Equals(true))
            {
                zone_show_keypads += 0x20;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 7"].Equals(true))
            {
                zone_show_keypads += 0x40;
            }
            if (mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Show keypad 8"].Equals(true))
            {
                zone_show_keypads += 0x80;
            }


            #endregion


            #region Circuit configuration
            byte[] zone_terminal_configuration_temp = new byte[10];
            byte[] zone_terminal_configuration = new byte[7];

            zone_terminal_configuration_temp[0] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Terminal circuit type"]);
            zone_terminal_configuration_temp[1] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R1 Value"]);
            zone_terminal_configuration_temp[2] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R1 Function"]);
            zone_terminal_configuration_temp[3] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R1 Contact type"]);
            zone_terminal_configuration_temp[4] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R2 Value"]);
            zone_terminal_configuration_temp[5] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R2 Function"]);
            zone_terminal_configuration_temp[6] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R2 Contact type"]);
            zone_terminal_configuration_temp[7] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R3 Value"]);
            zone_terminal_configuration_temp[8] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R3 Function"]);
            zone_terminal_configuration_temp[9] = Convert.ToByte(mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["R3 Contact type"]);


            for (int count = 0; count < 8; count++)
            {
                switch (count)
                {
                    case 0:
                        zone_terminal_configuration[count] = (byte)((zone_terminal_configuration_temp[2] & (byte)0xF0) + (zone_terminal_configuration_temp[3] & (byte)0x0F));
                        break;
                    case 2:
                        zone_terminal_configuration[count] = (byte)((zone_terminal_configuration_temp[5] & (byte)0xF0) + (zone_terminal_configuration_temp[6] & (byte)0x0F));
                        break;
                    case 4:
                        zone_terminal_configuration[count] = (byte)((zone_terminal_configuration_temp[8] & (byte)0xF0) + (zone_terminal_configuration_temp[9] & (byte)0x0F));
                        break;

                    case 1:
                        zone_terminal_configuration[count] = zone_terminal_configuration_temp[1];
                        break;
                    case 3:
                        zone_terminal_configuration[count] = zone_terminal_configuration_temp[4];
                        break;
                    case 5:
                        zone_terminal_configuration[count] = zone_terminal_configuration_temp[7];
                        break;
                    case 6:
                        zone_terminal_configuration[count] = zone_terminal_configuration_temp[0];
                        break;
                }
            }
            

            #endregion

            #region OUTPUTS config in alarm
            byte[] zone_alarms = new byte[84];
            //Tamper
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["tamper"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Tamper alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["tamper"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Tamper alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["tamper"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Tamper alarm output 3"]);

            //Anti mask
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["anti_mask"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Anti mask alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["anti_mask"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Anti mask alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["anti_mask"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Anti mask alarm output 3"]);

            //arm_away_timezone
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_timezone"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away by timezone - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_timezone"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away by timezone - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_timezone"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away by timezone - output 3"]);

            //arm_away_code
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_code"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with code - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_code"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with code - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_code"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with code - output 3"]);

            //arm_away_command
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_command"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with command - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_command"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with command - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_command"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with command - output 3"]);

            //arm_away_key_switch
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_key_switch"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with keyswtich - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_key_switch"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with keyswtich - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_key_switch"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away with keyswtich - output 3"]);

            //arm_away_remote
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_remote"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away remotely - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_remote"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away remotely - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_away_remote"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm away remotely - output 3"]);

            //arm_stay_timezone
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_timezone"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay by timezone - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_timezone"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay by timezone - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_timezone"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay by timezone - output 3"]);

            //arm_stay_code
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_code"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with code - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_code"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with code - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_code"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with code - output 3"]);

            //arm_stay_command
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_command"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with command - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_command"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with command - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_command"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with command - output 3"]);

            //arm_stay_key_switch
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_key_switch"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with keyswtich - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_key_switch"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with keyswtich - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_key_switch"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay with keyswtich - output 3"]);

            //arm_stay_remote
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_remote"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay remotely - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_remote"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay remotely - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["arm_stay_remote"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Arm stay remotely - output 3"]);

            //disarm_away_timezone
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_timezone"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away by timezone - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_timezone"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away by timezone - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_timezone"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away by timezone - output 3"]);

            //disarm_away_code
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_code"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with code - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_code"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with code - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_code"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with code - output 3"]);

            //disarm_away_command
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_command"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with command - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_command"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with command - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_command"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with command - output 3"]);

            //disarm_away_key_switch
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_key_switch"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with keyswtich - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_key_switch"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with keyswtich - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_key_switch"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away with keyswtich - output 3"]);

            //disarm_away_remote
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_remote"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away remotely - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_remote"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away remotely - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_away_remote"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm away remotely - output 3"]);

            //disarm_stay_timezone
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_timezone"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay by timezone - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_timezone"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay by timezone - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_timezone"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay by timezone - output 3"]);

            //disarm_stay_code
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_code"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with code - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_code"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with code - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_code"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with code - output 3"]);

            //disarm_stay_command
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_command"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with command - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_command"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with command - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_command"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with command - output 3"]);

            //disarm_stay_key_switch
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_key_switch"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with keyswtich - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_key_switch"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with keyswtich - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_key_switch"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay with keyswtich - output 3"]);

            //disarm_stay_remote
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_remote"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay remotely - output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_remote"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay remotely - output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["disarm_stay_remote"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Disarm stay remotely - output 3"]);

            //24h
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["24h"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["24 hour alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["24h"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["24 hour alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["24h"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["24 hour alarm output 3"]);

            //fire
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["fire"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Fire alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["fire"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Fire alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["fire"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Fire alarm output 3"]);

            //entry_delay
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["entry_delay"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Entry time alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["entry_delay"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Entry time alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["entry_delay"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Entry time alarm output 3"]);

            //alarm
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["alarm"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Zone alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["alarm"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Zone alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["alarm"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Zone alarm output 3"]);

            //chime
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["chime"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Chime alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["chime"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Chime alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["chime"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Chime alarm output 3"]);

            //sensor_watch
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["sensor_watch"])[0]] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Sensor watch alarm output 1"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["sensor_watch"])[0] + 1] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Sensor watch alarm output 2"]);
            zone_alarms[(int)((int[])this.attributes["zone_alarm_to_outputs"]["sensor_watch"])[0] + 2] = (byte)((int)mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Sensor watch alarm output 3"]);

            #endregion

            #region AUDIO TRACKS
            byte[] audio_tracks = new byte[8];
            audio_tracks[0] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 1"].ToString()) - 1) & 0xFF);
            audio_tracks[1] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 1"].ToString()) - 1) >> 8);
            audio_tracks[2] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 2"].ToString()) - 1) & 0xFF);
            audio_tracks[3] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 2"].ToString()) - 1) >> 8);
            audio_tracks[4] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 3"].ToString()) - 1) & 0xFF);
            audio_tracks[5] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 3"].ToString()) - 1) >> 8);
            audio_tracks[6] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 4"].ToString()) - 1) & 0xFF);
            audio_tracks[7] = (byte)((int.Parse(mainform.databaseDataSet.Zone.Rows[(int)zone_number]["Audio track 4"].ToString()) - 1) >> 8);
            #endregion

            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

            byte[] away_entry_delay_time_bytes = BitConverter.GetBytes(away_entry_delay_time);
            byte[] stay_entry_delay_time_bytes = BitConverter.GetBytes(stay_entry_delay_time);
            byte[] two_trigger_time_bytes = BitConverter.GetBytes(two_trigger_time);
            byte[] sensor_watch_timer_bytes = BitConverter.GetBytes(sensor_watch_timer);
            byte[] terminator_count_bytes = BitConverter.GetBytes(terminator_count);
            byte[] zone_options_bytes = BitConverter.GetBytes(zone_options);
            byte[] zone_partitions_away_bytes = BitConverter.GetBytes(zone_partitions_away);
            byte[] zone_partitions_stay_bytes = BitConverter.GetBytes(zone_partitions_stay);
            byte[] zone_show_keypads_bytes = BitConverter.GetBytes(zone_show_keypads);

            byte[] reserved_1_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Reserved 1"];
            byte[] reserved_2_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Reserved 2"];
            byte[] reserved_3_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Reserved 3"];
            byte[] reserved_4_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Reserved 4"];
            byte[] reserved_5_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Reserved 5"];
            byte[] temperature_normal_high_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Temperature normal low"];
            byte[] temperature_normal_low_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Temperature normal high"];
            byte[] temperature_alarm_high_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Temperature alarm high"];
            byte[] temperature_alarm_low_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Temperature alarm low"];
            byte[] report_code_bytes = (byte[])mainform.databaseDataSet.Zone.Rows[(int)(zone_number)]["Report code"];


            int i = 0;
            uint j = 0;
            uint zone_address = Constants.KP_ZONES_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_ZONAS_FLASH * (zone_number));

            byte_array[i++] = Constants.WRITE_CODE;
            byte_array[i++] = (byte)((zone_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((zone_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(zone_address & 0xFF);

            byte_array[i++] = 240;

            int temp = i;
            int description_addr = i + (int)attributes["description"]["address"];

            //TODO: Create a function for this for
            for (i = temp, j = 0; i < (temp + zone_terminal_configuration.Length); i++, j++)
            {
                byte_array[i] = zone_terminal_configuration[j];
            }


            for (i = description_addr, j = 0; i < (description_addr + description_bytes.Length); i++, j++)
            {
                byte_array[i] = description_bytes[j];
                if (description_bytes.Length == (i - 1))
                {
                    if ((description_bytes.Length - 1) == (j))
                    {
                        for (i = i + 1; i < (64 + description_addr); i++)
                        {
                            byte_array[i] = 0;
                        }
                    }
                }
            }
            for (i = ((int)this.attributes["options"]["address"] + temp), j = 0; i < ((int)this.attributes["options"]["address"] + temp + zone_options_bytes.Length); i++, j++)
            {
                byte_array[i] = zone_options_bytes[j];
            }
            for (i = ((int)this.attributes["away_entry_delay_time"]["address"] + temp), j = 0; i < ((int)this.attributes["away_entry_delay_time"]["address"] + temp + away_entry_delay_time_bytes.Length); i++, j++)
            {
                byte_array[i] = away_entry_delay_time_bytes[j];
            }
            for (i = ((int)this.attributes["stay_entry_delay_time"]["address"] + temp), j = 0; i < ((int)this.attributes["stay_entry_delay_time"]["address"] + temp + stay_entry_delay_time_bytes.Length); i++, j++)
            {
                byte_array[i] = stay_entry_delay_time_bytes[j];
            }


            for (i = ((int)this.attributes["two_trigger_time"]["address"] + temp), j = 0; i < ((int)this.attributes["two_trigger_time"]["address"] + temp + (two_trigger_time_bytes.Length-1)); i++, j++)
            {
                byte_array[i] = two_trigger_time_bytes[j];
            }
            for (i = ((int)this.attributes["sensor_watch_timer"]["address"] + temp), j = 0; i < ((int)this.attributes["sensor_watch_timer"]["address"] + temp + sensor_watch_timer_bytes.Length); i++, j++)
            {
                byte_array[i] = sensor_watch_timer_bytes[j];
            }

            for (i = ((int)this.attributes["terminator_count"]["address"] + temp), j = 0; i < ((int)this.attributes["terminator_count"]["address"] + temp + (terminator_count_bytes.Length-1)); i++, j++)
            {
                byte_array[i] = terminator_count_bytes[j];
            }
            for (i = ((int)this.attributes["partitions_away"]["address"] + temp), j = 0; i < ((int)this.attributes["partitions_away"]["address"] + temp + zone_partitions_away_bytes.Length); i++, j++)
            {
                byte_array[i] = zone_partitions_away_bytes[j];
            }
            for (i = ((int)this.attributes["partitions_stay"]["address"] + temp), j = 0; i < ((int)this.attributes["partitions_stay"]["address"] + temp + zone_partitions_stay_bytes.Length); i++, j++)
            {
                byte_array[i] = zone_partitions_stay_bytes[j];
            }
            for (i = ((int)this.attributes["show_keypads"]["address"] + temp), j = 0; i < ((int)this.attributes["show_keypads"]["address"] + temp + zone_show_keypads_bytes.Length); i++, j++)
            {
                byte_array[i] = zone_show_keypads_bytes[j];
            }

            #region MEMORY NOT USED - should be written always

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
            //Reserved 5
            for (i = (temp + (int)this.attributes["reserved_5"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_5"]["address"] + reserved_5_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_5_bytes[j];
            }

            //Report code
            for (i = (temp + (int)this.attributes["report_code"]["address"]), j = 0; i < (temp + (int)this.attributes["report_code"]["address"] + report_code_bytes.Length); i++, j++)
            {
                byte_array[i] = report_code_bytes[j];
            }

            //Temperature normal low
            for (i = (temp + (int)this.attributes["temperature_normal_low"]["address"]), j = 0; i < (temp + (int)this.attributes["temperature_normal_low"]["address"] + temperature_normal_low_bytes.Length); i++, j++)
            {
                byte_array[i] = temperature_normal_low_bytes[j];
            }
            //Temperature normal high
            for (i = (temp + (int)this.attributes["temperature_normal_high"]["address"]), j = 0; i < (temp + (int)this.attributes["temperature_normal_high"]["address"] + temperature_normal_high_bytes.Length); i++, j++)
            {
                byte_array[i] = temperature_normal_high_bytes[j];
            }
            //Temperature alarm low
            for (i = (temp + (int)this.attributes["temperature_alarm_low"]["address"]), j = 0; i < (temp + (int)this.attributes["temperature_alarm_low"]["address"] + temperature_alarm_low_bytes.Length); i++, j++)
            {
                byte_array[i] = temperature_alarm_low_bytes[j];
            }
            //Temperature alarm high
            for (i = (temp + (int)this.attributes["temperature_alarm_high"]["address"]), j = 0; i < (temp + (int)this.attributes["temperature_alarm_high"]["address"] + temperature_alarm_high_bytes.Length); i++, j++)
            {
                byte_array[i] = temperature_alarm_high_bytes[j];
            }
            #endregion

            for (i = (((int)attributes["zone_alarm_to_outputs"]["address"]) + temp), j = 0; i < (((int)attributes["zone_alarm_to_outputs"]["address"]) + temp + zone_alarms.Length); i++, j++)
            {
                byte_array[i] = zone_alarms[j];
            }

            for (i = (((int)attributes["audio_tracks"]["address"]) + temp), j = 0; i < (((int)attributes["audio_tracks"]["address"]) + temp + audio_tracks.Length); i++, j++)
            {
                byte_array[i] = audio_tracks[j];
            }

            byte_array[4] = (byte)(i-temp);
            General protocol = new General();
            protocol.send_msg((uint)i, byte_array, mainform.cp_id, mainform); // TODO: Check if cp_id is needed

        }
    }
}
