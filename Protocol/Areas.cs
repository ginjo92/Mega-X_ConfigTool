using MegaXConfigTool;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace MegaXConfigTool.Protocol
{
    class Areas
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 0 },
                    { "data_grid_view_addr", 1 }
                }

            },
            {
                "call_code",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 64 },
                    { "data_grid_view_addr", 9 }
                }
            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 72 },
                    { "KP_PART_ARM_BOT_REQUIRED_BEFORE_CODE_TO_SET_AWAY", new uint[2] { 0x01, 4} },                 //bit0
                    { "KP_PART_ARM_BOT_REQUIRED_BEFORE_CODE_TO_SET_STAY", new uint[2] { 0x02, 4} },                 //bit1
                    { "area_code_required_to_unset", new uint[2] { 0x04, 2} },                                      //bit2
                    { "area_tag_required_to_set", new uint[2] { 0x08, 0} },                                         //bit3
                    { "area_can_not_arm_if_zone_unsealed_at_end_of_exit_delay", new uint[2] { 0x10, 4} },           //bit4
                    { "area_code_required_to_bypass_zones", new uint[2] { 0x20, 3} },                               //bit5
                    { "area_send_alarm_atend_exit_delay", new uint[2] { 0x40, 7} },                                 //bit6
                    { "area_can_arm_away_only_if_all_zones_sealed", new uint[2] { 0x80, 8} },                       //bit7
                                                                                                                    //----
                    { "area_can_arm_stay_only_if_all_zones_sealed", new uint[2] { 0x100, 9} },                      //bit8
                    { "area_use_near_and_verified_alarm_reporting_for_all_zones", new uint[2] { 0x200, 8} },        //bit9
                    { "area_assign_chirps_access_tags", new uint[2] { 0x400, 9} },                                  //bit10
                    { "area_code_or_tag", new uint[2] { 0x800 , 5} },                                               //bit11
                }
            },
            {
                "area_arm_disarm_outputs",
                new Dictionary<string, object>
                {
                    { "value", new byte[36] },
                    { "address", 76 },
                    { "number_of_options_by_output", 3 },
                    { "away_arm_outputs",new int[2] { 0, 80 } },
                    { "stay_arm_outputs", new int[2] { 3, 68 } },
                    { "away_disarm_outputs",new int[2] { 6, 80 } },
                    { "stay_disarm_outputs", new int[2] { 9, 68 } },
                    { "pulse_away_arm_outputs",new int[2] { 12, 80 } },
                    { "pulse_stay_arm_outputs", new int[2] { 15, 68 } },
                    { "pulse_away_disarm_outputs",new int[2] { 18, 80 } },
                    { "pulse_stay_disarm_outputs", new int[2] { 21, 68 } },
                    { "away_arming_outputs",new int[2] { 24, 80 } },
                    { "pulse_away_arming_outputs", new int[2] { 27, 68 } },
                    { "stay_arming_outputs",new int[2] { 30, 80 } },
                    { "pulse_stay_arming_outputs", new int[2] { 33, 68 } },
                }
            },
            {
                "code_length", //unused
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 112 },
                    { "data_grid_view_addr", 6 }
                }
            },
             {
                "reserved_1",//unused
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 113 },
                    { "data_grid_view_addr", 6 }
                }
            },
              {
                "reserved_2",//unused
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 114 },
                    { "data_grid_view_addr", 6 }
                }
            },
               {
                "reserved_3",//unused
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 115 },
                    { "data_grid_view_addr", 6 }
                }
            },
            {
                "away_entry_delay_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 116 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "stay_entry_delay_time",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 120 },
                    { "data_grid_view_addr", 8 }
                }
            },
            {
                "DRCV_client_code",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 124}, 
                    { "data_grid_view_addr", 5 }
                }
            },
            {
                "start_message_command_control",//unused
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 126}, 
                    { "data_grid_view_addr", 5 }
                }
            },
            {
                "Timezone_start_arm",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 128 },
                    { "data_grid_view_addr", 22 }
                }
            },
            {
                "Timezone_start_disarm",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 129},
                    { "data_grid_view_addr", 38 }
                }
            },
            {
                "Timezone_end_arm",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 130 },
                    { "data_grid_view_addr", 30 }
                }
            },
            {
                "Timezone_end_disarm",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 131 },
                    { "data_grid_view_addr", 46 }
                }
            },
            {
                "audio_tracks",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 132 },
                    { "data_grid_view_addr", 46 }
                }
            },
            {
                "area_beeps_arm_disarm_outputs",
                new Dictionary<string, object>
                {
                    { "value", new byte[12] },
                    { "address", 140 },
                    { "number_of_options_by_output", 3 },
                    { "beeps_away_arm_outputs",new int[2] { 0, 80 } },
                    { "beeps_away_disarm_outputs", new int[2] { 3, 68 } },
                    { "beeps_stay_arm_outputs",new int[2] { 6, 80 } },
                    { "beeps_stay_disarm_outputs", new int[2] { 9, 68 } },
                    
                }
            },
        };

        public void read(MainWindow mainForm, uint area_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint area_address = Constants.KP_AREAS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_PARTICOES_FLASH * area_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = Constants.READ_CODE;
            byte_array[i++] = (byte)((area_address >> 16) & 0xff);
            byte_array[i++] = (byte)((area_address >> 8) & 0xff);
            byte_array[i++] = (byte)((area_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainForm, uint area_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            string description = ((string)mainForm.databaseDataSet.Area.Rows[(int)area_number]["Description"]).ToUpper();

            #region Area options read from dataset
            ////Area OPTIONS
            uint area_options = 0;
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Code required to arm away"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_PART_ARM_BOT_REQUIRED_BEFORE_CODE_TO_SET_AWAY"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Code required to arm stay"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_PART_ARM_BOT_REQUIRED_BEFORE_CODE_TO_SET_STAY"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Code required to disarm"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_code_required_to_unset"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Tag required to arm"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_tag_required_to_set"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Tag or code required"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_code_or_tag"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Code to bypass"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_code_required_to_bypass_zones"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Send alarm atend exit time"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_send_alarm_atend_exit_delay"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Can arm away only if all zones sealed"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_can_arm_away_only_if_all_zones_sealed"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Can arm stay only if all zones sealed"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_can_arm_stay_only_if_all_zones_sealed"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Use near and verified alarm reporting for all zones"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_use_near_and_verified_alarm_reporting_for_all_zones"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Assign chirps access tags"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_assign_chirps_access_tags"])[0]));
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Not arm if zones open after exit delay"].Equals(true))
            {
                area_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["area_can_not_arm_if_zone_unsealed_at_end_of_exit_delay"])[0]));
            }
           
            #endregion

            #region Area arm disarm outputs
            byte[] area_arm_disarm_outputs = (byte[])attributes["area_arm_disarm_outputs"]["value"];
            //away_arm_outputs
            area_arm_disarm_outputs[(int)((int[])this.attributes["area_arm_disarm_outputs"]["away_arm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm away output - 1"]);
            area_arm_disarm_outputs[(int)((int[])this.attributes["area_arm_disarm_outputs"]["away_arm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm away output - 2"]);
            area_arm_disarm_outputs[(int)((int[])this.attributes["area_arm_disarm_outputs"]["away_arm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm away output - 3"]);

            //stay_arm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_arm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm stay output - 1"]); ;
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_arm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm stay output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_arm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm stay output - 3"]);

            //away_disarm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["away_disarm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm away output - 1"]); ;
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["away_disarm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm away output - 2"]); 
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["away_disarm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm away output - 3"]);
            //stay_disarm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_disarm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm stay output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_disarm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm stay output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_disarm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm stay output - 3"]);
            //pulse_away_arm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_arm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm away pulsed output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_arm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm away pulsed output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_arm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm away pulsed output - 3"]);
            //pulse_stay_arm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_arm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm stay pulsed output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_arm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm stay pulsed output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_arm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arm stay pulsed output - 3"]);
            //pulse_stay_disarm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_disarm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm pulsed stay output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_disarm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm pulsed stay output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_disarm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm pulsed stay output - 3"]);
            //pulse_away_disarm_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_disarm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm pulsed away output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_disarm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm pulsed away output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_disarm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Disarm pulsed away output - 3"]);
            //away_arming_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["away_arming_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming away output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["away_arming_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming away output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["away_arming_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming away output - 3"]);
            //pulse_away_arming_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_arming_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming away pulsed output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_arming_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming away pulsed output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_away_arming_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming away pulsed output - 3"]);
            //stay_arming_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_arming_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming stay output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_arming_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming stay output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["stay_arming_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming stay output - 3"]);
            //pulse_stay_arming_outputs
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_arming_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming stay pulsed output - 1"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_arming_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming stay pulsed output - 2"]);
            area_arm_disarm_outputs[((int[])this.attributes["area_arm_disarm_outputs"]["pulse_stay_arming_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Arming stay pulsed output - 3"]);
            #endregion

            #region Area beeps arm disarm outputs
            byte[] area_beeps_arm_disarm_outputs = (byte[])attributes["area_beeps_arm_disarm_outputs"]["value"];
            //beeps_away_arm_outputs
            area_beeps_arm_disarm_outputs[(int)((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_arm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Away arm beeps output - 1"]);
            area_beeps_arm_disarm_outputs[(int)((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_arm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Away arm beeps output - 2"]);
            area_beeps_arm_disarm_outputs[(int)((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_arm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Away arm beeps output - 3"]);

            //beeps_away_disarm_outputs
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_disarm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Away disarm beeps output - 1"]); ;
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_disarm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Away disarm beeps output - 2"]);
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_disarm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Away disarm beeps output - 3"]);

            //beeps_stay_arm_outputs
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_arm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Stay arm beeps output - 1"]); ;
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_arm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Stay arm beeps output - 2"]);
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_arm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Stay arm beeps output - 3"]);

            //beeps_stay_disarm_outputs
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_disarm_outputs"])[0]] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Stay disarm beeps output - 1"]);
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_disarm_outputs"])[0] + 1] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Stay disarm beeps output - 2"]);
            area_beeps_arm_disarm_outputs[((int[])this.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_disarm_outputs"])[0] + 2] = (byte)((int)mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Stay disarm beeps output - 3"]);
           
            #endregion

            #region Timezones Start arm.
            byte timezones_start_arm = 0;
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T1"].Equals(true))
            {
                timezones_start_arm += 0x01;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T2"].Equals(true))
            {
                timezones_start_arm += 0x02;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T3"].Equals(true))
            {
                timezones_start_arm += 0x04;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T4"].Equals(true))
            {
                timezones_start_arm += 0x08;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T5"].Equals(true))
            {
                timezones_start_arm += 0x10;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T6"].Equals(true))
            {
                timezones_start_arm += 0x20;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T7"].Equals(true))
            {
                timezones_start_arm += 0x40;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start arm - T8"].Equals(true))
            {
                timezones_start_arm += 0x80;
            }
            #endregion

            #region Timezones End arm
            byte timezones_end_arm = 0;
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T1"].Equals(true))
            {
                timezones_end_arm += 0x01;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T2"].Equals(true))
            {
                timezones_end_arm += 0x02;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T3"].Equals(true))
            {
                timezones_end_arm += 0x04;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T4"].Equals(true))
            {
                timezones_end_arm += 0x08;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T5"].Equals(true))
            {
                timezones_end_arm += 0x10;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T6"].Equals(true))
            {
                timezones_end_arm += 0x20;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T7"].Equals(true))
            {
                timezones_end_arm += 0x40;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End arm - T8"].Equals(true))
            {
                timezones_end_arm += 0x80;
            }
            #endregion

            #region Timezones Start disarm
            byte timezones_start_disarm = 0;
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T1"].Equals(true))
            {
                timezones_start_disarm += 0x01;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T2"].Equals(true))
            {
                timezones_start_disarm += 0x02;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T3"].Equals(true))
            {
                timezones_start_disarm += 0x04;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T4"].Equals(true))
            {
                timezones_start_disarm += 0x08;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T5"].Equals(true))
            {
                timezones_start_disarm += 0x10;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T6"].Equals(true))
            {
                timezones_start_disarm += 0x20;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T7"].Equals(true))
            {
                timezones_start_disarm += 0x40;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["Start disarm - T8"].Equals(true))
            {
                timezones_start_disarm += 0x80;
            }
            #endregion

            #region Timezones End disarm
            byte timezones_end_disarm = 0;
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T1"].Equals(true))
            {
                timezones_end_disarm += 0x01;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T2"].Equals(true))
            {
                timezones_end_disarm += 0x02;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T3"].Equals(true))
            {
                timezones_end_disarm += 0x04;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T4"].Equals(true))
            {
                timezones_end_disarm += 0x08;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T5"].Equals(true))
            {
                timezones_end_disarm += 0x10;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T6"].Equals(true))
            {
                timezones_end_disarm += 0x20;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T7"].Equals(true))
            {
                timezones_end_disarm += 0x40;
            }
            if (mainForm.databaseDataSet.Area.Rows[(int)area_number]["End disarm - T8"].Equals(true))
            {
                timezones_end_disarm += 0x80;
            }
            #endregion

            
            ulong area_call_code = ulong.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Voice call code"].ToString());
            uint exit_timer_away = uint.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Exit timer away"].ToString());
            uint exit_timer_stay = uint.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Exit timer stay"].ToString());
            short area_account_number = short.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["DRCV account number"].ToString());


            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);
            byte[] area_options_bytes = BitConverter.GetBytes(area_options);
            ulong[] area_call_code_bytes = GetIntArray(area_call_code);
            byte[] exit_timer_away_bytes = BitConverter.GetBytes(exit_timer_away);
            byte[] exit_timer_stay_bytes = BitConverter.GetBytes(exit_timer_stay);
            byte[] area_account_number_bytes = BitConverter.GetBytes(area_account_number);
            byte[] timezones_start_arm_bytes = BitConverter.GetBytes(timezones_start_arm);
            byte[] timezones_end_arm_bytes = BitConverter.GetBytes(timezones_end_arm);
            byte[] timezones_start_disarm_bytes = BitConverter.GetBytes(timezones_start_disarm);
            byte[] timezones_end_disarm_bytes = BitConverter.GetBytes(timezones_end_disarm);

            byte[] reserved_1_bytes = (byte[])mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Reserved 1"];
            byte[] reserved_2_bytes = (byte[])mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Reserved 2"];
            byte[] reserved_3_bytes = (byte[])mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Reserved 3"];
            byte[] start_message_command_control_bytes = (byte[])mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Start message command control"];

            #region AUDIO TRACKS
            byte[] audio_tracks = new byte[8];
            if((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 1"].ToString()).Equals(0xFFFF)))
            {
                audio_tracks[0] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 1"].ToString()) & 0xFF);
                audio_tracks[1] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 1"].ToString()) >> 8);
            }else
            {
                audio_tracks[0] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 1"].ToString()) - 1) & 0xFF);
                audio_tracks[1] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 1"].ToString()) - 1) >> 8);
            }

            if ((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 2"].ToString()).Equals(0xFFFF)))
            {
                audio_tracks[2] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 2"].ToString()) & 0xFF);
                audio_tracks[3] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 2"].ToString()) >> 8);
            }else
            {
                audio_tracks[2] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 2"].ToString()) - 1) & 0xFF);
                audio_tracks[3] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 2"].ToString()) - 1) >> 8);
            }

            if ((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 3"].ToString()).Equals(0xFFFF)))
            {
                audio_tracks[4] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 3"].ToString()) - 1) & 0xFF);
                audio_tracks[5] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 3"].ToString()) - 1) >> 8);
            }
            else
            {
                audio_tracks[4] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 3"].ToString()) & 0xFF);
                audio_tracks[5] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 3"].ToString()) >> 8);
            }

            if ((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 4"].ToString()).Equals(0xFFFF)))
            {
                audio_tracks[6] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 4"].ToString()) & 0xFF);
                audio_tracks[7] = (byte)(int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 4"].ToString()) >> 8);
            }
            else
            {
                audio_tracks[6] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 4"].ToString()) - 1) & 0xFF);
                audio_tracks[7] = (byte)((int.Parse(mainForm.databaseDataSet.Area.Rows[(int)area_number]["Audio track 4"].ToString()) - 1) >> 8);
            }
            #endregion

            byte[] area_code_length = (byte[])mainForm.databaseDataSet.Area.Rows[(int)(area_number)]["Code length"];

            int i = 0;
            uint j = 0;
            uint area_address = Constants.KP_AREAS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_PARTICOES_FLASH * area_number);
            byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;//0x40;
            byte_array[i++] = (byte)((area_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((area_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(area_address & 0xFF);

            byte_array[i++] = 240;
            int temp = i;
            //TODO: Create a function for this for 

            //Description
            for (i = temp + (int)attributes["description"]["address"], j = 0; i < (temp + (int)attributes["description"]["address"] + description_bytes.Length); i++, j++)
            {
                byte_array[i] = description_bytes[j];
                if((description_bytes.Length-1) == (j))
                {
                    for(i=i+1; i < ((int)attributes["description"]["address"] + temp); i++)
                    {
                        byte_array[i] = 0;
                    }
                }
            }

            //call code
            for (i = (int)attributes["call_code"]["address"] + temp, j = 0; i < ((int)attributes["call_code"]["address"] + temp + area_call_code_bytes.Length); i++, j++)
            {
                byte_array[i] = (byte)area_call_code_bytes[j];

                //Add 0xFF if code don't have maximum length
                if (i.Equals(((int)attributes["call_code"]["address"] + temp + (area_call_code_bytes.Length - 1))) && area_call_code_bytes.Length != 8)
                {
                    int counter = 8 - area_call_code_bytes.Length;

                    for(int k = counter; k > 0 ; k--)
                    {
                        byte_array[i + k] = 0xFF;
                    }
                }
            }

            //Options
            for (i = ((int)attributes["options"]["address"] + temp), j = 0; i < ((int)attributes["options"]["address"] + temp + area_options_bytes.Length); i++, j++)
            {
                byte_array[i] = area_options_bytes[j];
            }

            //code length
            for (i = ((int)attributes["code_length"]["address"] + temp), j = 0; i < ((int)attributes["code_length"]["address"] + temp + (area_code_length.Length-1)); i++, j++)
            {
                byte_array[i] = area_code_length[j];
            }

            // exit timer away
            for (i = ((int)attributes["away_entry_delay_time"]["address"] + temp), j = 0; i < ((int)attributes["away_entry_delay_time"]["address"] + temp + (exit_timer_away_bytes.Length)); i++, j++)
            {
                byte_array[i] = exit_timer_away_bytes[j];
            }

            //exit timer stay
            for (i = ((int)attributes["stay_entry_delay_time"]["address"] + temp), j = 0; i < ((int)attributes["stay_entry_delay_time"]["address"] + temp + (exit_timer_stay_bytes.Length)); i++, j++)
            {
                byte_array[i] = exit_timer_stay_bytes[j];
            }

            //area_arm_disarm_outputs
            for (i = ((int)attributes["area_arm_disarm_outputs"]["address"] + temp), j = 0; i < ((int)attributes["area_arm_disarm_outputs"]["address"] + temp + (area_arm_disarm_outputs.Length)); i++, j++)
            {
                byte_array[i] = area_arm_disarm_outputs[j];
            }

           

            //DRCV_client_code
            for (i = ((int)attributes["DRCV_client_code"]["address"] + temp), j = 0; i < ((int)attributes["DRCV_client_code"]["address"] + temp + (area_account_number_bytes.Length)); i++, j++)
            {
                byte_array[i] = area_account_number_bytes[j];
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
            //Start message command control
            for (i = (temp + (int)this.attributes["start_message_command_control"]["address"]), j = 0; i < (temp + (int)this.attributes["start_message_command_control"]["address"] + start_message_command_control_bytes.Length); i++, j++)
            {
                byte_array[i] = start_message_command_control_bytes[j];
            }

            #endregion


            #region Timezones
            for (i = ((int)attributes["Timezone_start_arm"]["address"] + temp), j = 0; i < ((int)attributes["Timezone_start_arm"]["address"] + temp + timezones_start_arm_bytes.Length); i++, j++)
            {
                byte_array[i] = timezones_start_arm_bytes[j];
            }
            for (i = ((int)attributes["Timezone_start_disarm"]["address"] + temp), j = 0; i < ((int)attributes["Timezone_start_disarm"]["address"] + temp + timezones_start_disarm_bytes.Length); i++, j++)
            {
                byte_array[i] = timezones_start_disarm_bytes[j];
            }
            for (i = ((int)attributes["Timezone_end_arm"]["address"] + temp), j = 0; i < ((int)attributes["Timezone_end_arm"]["address"] + temp + timezones_end_arm_bytes.Length); i++, j++)
            {
                byte_array[i] = timezones_end_arm_bytes[j];
            }
            for (i = ((int)attributes["Timezone_end_disarm"]["address"] + temp), j = 0; i < ((int)attributes["Timezone_end_disarm"]["address"] + temp + timezones_end_disarm_bytes.Length); i++, j++)
            {
                byte_array[i] = timezones_end_disarm_bytes[j];
            }
            #endregion

            #region Audio tracks
            for (i = ((int)attributes["audio_tracks"]["address"] + temp), j = 0; i < ((int)attributes["audio_tracks"]["address"] + temp + (audio_tracks.Length)); i++, j++)
            {
                byte_array[i] = audio_tracks[j];
            }
            #endregion

            #region area_beeps_arm_disarm_outputs
            //area_beeps_arm_disarm_outputs
            for (i = ((int)attributes["area_beeps_arm_disarm_outputs"]["address"] + temp), j = 0; i < ((int)attributes["area_beeps_arm_disarm_outputs"]["address"] + temp + (area_beeps_arm_disarm_outputs.Length)); i++, j++)
            {
                byte_array[i] = area_beeps_arm_disarm_outputs[j];
            }
            #endregion

            byte_array[4] = (byte)(i-temp);

            General protocol = new General();
            //protocol.send_msg((uint)(i), byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is needed

            protocol.send_msg_block((uint)i, byte_array, area_address, mainForm.cp_id, mainForm, Constants.KP_FLASH_TAMANHO_DADOS_PARTICOES_FLASH); // TODO: Check if cp_id is needed
            System.Threading.Thread.Sleep(mainForm.intervalsleeptime);

        }

        ulong[] GetIntArray(ulong num)
        {
            List<ulong> listOfInts = new List<ulong>();
            while (num > 0)
            {
                listOfInts.Add(num % 10);
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }
    }
}

