using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class GlobalSystem
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "output_call_code",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "data_grid_view_addr", 1 }
                }

            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 8 },
                    //OPTIONS 1                                                                                         //----
                    { "KP_GLOB_ARM_IF_BATT_LOW_OR_AC_FAILS", new uint[2] { 0x01, 0} },                                  //bit0
                    { "KP_GLOB_CANNOT_ARM_IF_THERE_IS_KEYPAD_FAULT", new uint[2] { 0x02, 13} },                         //bit1
                    { "KP_GLOB_CANNOT_ARM_IF_THERE_IS_LINE_FAULT", new uint[2] { 0x04, 14} },                           //bit2
                    { "KP_GLOB_CANNOT_ARM_IF_THERE_IS_COMMS_FAULT", new uint[2] { 0x08, 15} },                          //bit3
                    { "KP_GLOB_OUTPUT_CONTROL_FROM_KEYPAD_IS_DISABLED_WHEN_ARMED", new uint[2] { 0x10, 20} },           //bit4
                    { "KP_GLOB_DIRECT_INSTALLER_ACCESS", new uint[2] { 0x20, 5} },                                      //bit5
                    { "KP_GLOB_MAINS_FAIL_TEST", new uint[2] { 0x40, 6} },                                              //bit6
                    { "KP_GLOB_EXTENDED_OUTPUT", new uint[2] { 0x80, 7} },                                              //bit7
                    //OPTIONS 2                                                                                         //----
                    { "KP_GLOB_TEMPO_EM_MINUTOS", new uint[2] { 0x100, 8} },                                            //bit8
                    { "KP_GLOB_REVISION_DATE_ACTIVE", new uint[2] { 0x200, 9} },                                        //bit9   1
                    { "KP_GLOB_INSTALLER_INFO", new uint[2] { 0x400, 3} },                                              //bit10
                    { "KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM", new uint[2] { 0x800, 11} },                         //bit11
                    { "KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM", new uint[2] { 0x1000, 12} },                    //bit12
                                                                                                                        //bit13
                                                                                                                        //bit14
                    { "KP_GLOB_DEFAULT_CONFIGURATION", new uint[2] { 0x8000, 21} },                                     //bit15
                    //OPTIONS 3                                                                                         //----
                    { "KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS", new uint[2] { 0x10000, 16} },                //bit16
                    { "KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER", new uint[2] { 0x20000, 17} },                           //bit17
                    { "KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY", new uint[2] { 0x40000, 18} },                             //bit18
                    { "KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE", new uint[2] { 0x80000, 19} },               //bit19
                    { "KP_GLOB_PANEL_TAMPER_EOL", new uint[2] { 0x100000, 4} },                                         //bit20
                    { "KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM", new uint[2] { 0x200000, 10} },                       //bit21
                    { "KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT", new uint[2] { 0x400000, 2} },                          //bit22
                    { "KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM", new uint[2] { 0x800000, 23} },                      //bit23     9 
                    //OPTIONS 4                                                                                         //----
                    { "KP_GLOB_DATE_LOSS", new uint[2] { 0x40000000 , 30} },                                            //bit30
                    { "KP_GLOB_REVISION_DATE_REACHED", new uint[2] { 0x80000000, 31} }                                  //bit31
                }
            },
            
            {
                "installer_code",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 12 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "coation_digit",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 20 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "phone_prefix",
                new Dictionary<string, object>
                {
                    { "value", new byte[6] },
                    { "address", 21 },
                    { "data_grid_view_addr", 7 }
                }
            },
            { //not used
                "reserved_1",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 27 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "dial_report_delay",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 28 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "mains_fail_report_delay",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 30 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "receiver_fail_delay",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 32 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "radio_detector_supervised_timer",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 34 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "CRA_client_number",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 36 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "event_code_session",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 38 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "siren_tamper",
                new Dictionary<string, object>
                {
                    { "value", new byte[3] },
                    { "address", 40 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "siren_tamper_config",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 43 },
                    { "KP_TAMPER_CONFIG_ATIVO", new uint[2] { 0x01, 0} },
                    { "KP_TAMPER_CONFIG_NORMAL_CLOSE", new uint[2] { 0x02, 1} },
                }
            },
            {
                "panel_tamper",
                new Dictionary<string, object>
                {
                    { "value", new byte[3] },
                    { "address", 44 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "panel_tamper_config",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 47 },
                    { "KP_TAMPER_CONFIG_ATIVO", new uint[2] { 0x01, 0} },
                    { "KP_TAMPER_CONFIG_NORMAL_CLOSE", new uint[2] { 0x02, 1} },
                }
            },
            {
                "keyswitch_config",
                new Dictionary<string, object>
                {
                   { "value", new byte[1] },
                    { "address", 48 },
                    { "KP_KEYSWITCH_CONFIG_ATIVO", new uint[2] { 0x01, 0} },
                    { "KP_KEYSWITCH_CONFIG_NORMAL_CLOSE", new uint[2] { 0x02, 1} },
                    { "KP_KEYSWITCH_CONFIG_TOGGLE", new uint[2] { 0x04, 2} }
                }
            },
            {
                "keyswitch_area_away",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 49 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "keyswitch_area_stay",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 50 },
                    { "data_grid_view_addr", 7 }
                }
            },
             { //not used
                "reserved_2",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 51 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "audio_tracks",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 52 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "maintenance_description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 60 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "maintenance_phone_number",
                new Dictionary<string, object>
                {
                    { "value", new byte[16] },
                    { "address", 124 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "maintenance_date",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 140 },
                    { "data_grid_view_addr", 7 }
                }
            },
             {
                "outputs_permissions",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 144 },
                }
            },


        };

        internal void read(MainWindow mainWindow, uint globalsystem_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint globalsystem_address = Constants.KP_GLOBAL_SYSTEM_INIC_ADDR;// + ((Constants.KP_FLASH_TAMANHO_DADOS_GLOBALSYSTEM_FLASH/Constants.KP_GLOBAL_SYSTEM_DIV) * (globalsystem_number - 1));
            byte size = 210;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((globalsystem_address >> 16) & 0xff);
            byte_array[i++] = (byte)((globalsystem_address >> 8) & 0xff);
            byte_array[i++] = (byte)((globalsystem_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow);
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainWindow, uint global_system_index)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            //Output call code
            ulong output_call_code = ulong.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Outputs code"].ToString());

            #region Global system options read from dataset
            uint global_system_options = 0;

            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Arm if Battery low or 230V fail"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_ARM_IF_BATT_LOW_OR_AC_FAILS"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Revision date is active"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_REVISION_DATE_ACTIVE"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_INSTALLER_INFO"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_INSTALLER_INFO"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_PANEL_TAMPER_EOL"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_PANEL_TAMPER_EOL"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_DIRECT_INSTALLER_ACCESS"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_DIRECT_INSTALLER_ACCESS"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_MAINS_FAIL_TEST"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_MAINS_FAIL_TEST"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_EXTENDED_OUTPUT"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_EXTENDED_OUTPUT"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Time in minutes"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_TEMPO_EM_MINUTOS"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Not arm if keypad is faulty"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_CANNOT_ARM_IF_THERE_IS_KEYPAD_FAULT"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Not arm if line is faulty"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_CANNOT_ARM_IF_THERE_IS_LINE_FAULT"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Not arm if communications have problems"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_CANNOT_ARM_IF_THERE_IS_COMMS_FAULT"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output control is disabled when armed"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_OUTPUT_CONTROL_FROM_KEYPAD_IS_DISABLED_WHEN_ARMED"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_DATE_LOSS"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_DATE_LOSS"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_REVISION_DATE_REACHED"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_REVISION_DATE_REACHED"])[0]));
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["KP_GLOB_DEFAULT_CONFIGURATION"].Equals(true))
            {
                global_system_options += (uint)(0xFFFFFFFF & (((uint[])this.attributes["options"]["KP_GLOB_DEFAULT_CONFIGURATION"])[0]));
            }

            #endregion

            #region Output permissions
            long outputs_permissions = 0;
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output1Permissions"]).Equals(true))
            {
                outputs_permissions += 0x01;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output2Permissions"]).Equals(true))
            {
                outputs_permissions += 0x02;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output3Permissions"]).Equals(true))
            {
                outputs_permissions += 0x04;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output4Permissions"]).Equals(true))
            {
                outputs_permissions += 0x08;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output5Permissions"]).Equals(true))
            {
                outputs_permissions += 0x10;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output6Permissions"]).Equals(true))
            {
                outputs_permissions += 0x20;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output7Permissions"]).Equals(true))
            {
                outputs_permissions += 0x40;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output8Permissions"]).Equals(true))
            {
                outputs_permissions += 0x80;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output9Permissions"]).Equals(true))
            {
                outputs_permissions += 0x100;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output10Permissions"]).Equals(true))
            {
                outputs_permissions += 0x200;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output11Permissions"]).Equals(true))
            {
                outputs_permissions += 0x400;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output12Permissions"]).Equals(true))
            {
                outputs_permissions += 0x800;
            }
            if (Convert.ToBoolean(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Output13Permissions"]).Equals(true))
            {
                outputs_permissions += 0x1000;
            }
            #endregion

            //Installer code
            ulong installer_code = ulong.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Installer code"].ToString());

            //Coation digit
            byte coation_digit = byte.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Coation digit"].ToString());

            //prefix
            ulong phone_prefix = ulong.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Phone prefix"].ToString());

            #region RESERVED
            byte[] reserved_1_bytes = (byte[])mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Reserved 1"];
            byte[] reserved_2_bytes = (byte[])mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Reserved 2"];
            #endregion

            //Dial report delay
            uint dial_report_delay = uint.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Dial report delay"].ToString());

            //mains_fail_report_delay
            uint mains_fail_report_delay = uint.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["230V fail report delay"].ToString());

            //receiver_fail_delay
            uint receiver_fail_delay = uint.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Receiver fail report delay"].ToString());

            //radio_detector_supervised_timer
            uint radio_detector_supervised_timer = uint.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Radio detector supervised timer"].ToString());

            //receiver_fail_delay
            uint CRA_client_number = uint.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["CRA client number"].ToString());

            //receiver_fail_delay
            uint event_code_session = uint.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Event code session"].ToString());

            #region Siren Tamper outputs
            byte[] siren_tamper_outputs = (byte[])attributes["siren_tamper"]["value"];
            siren_tamper_outputs[0] = (byte)((int)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Siren tamper output - 1"]);
            siren_tamper_outputs[1] = (byte)((int)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Siren tamper output - 2"]);
            siren_tamper_outputs[2] = (byte)((int)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Siren tamper output - 3"]);
            #endregion

            //Siren tamper config
            byte[] siren_tamper_config_bytes = (byte[])attributes["siren_tamper_config"]["value"];
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Siren tamper active"].Equals(true))
            {
                siren_tamper_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["siren_tamper_config"]["KP_TAMPER_CONFIG_ATIVO"])[0]);
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Siren tamper type"].Equals(0x01))
            {
                siren_tamper_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["siren_tamper_config"]["KP_TAMPER_CONFIG_NORMAL_CLOSE"])[0]);
            }
          
            #region Panel Tamper outputs
            byte[] panel_tamper_outputs = (byte[])attributes["panel_tamper"]["value"];
            panel_tamper_outputs[0] = (byte)((int)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Panel tamper output - 1"]);
            panel_tamper_outputs[1] = (byte)((int)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Panel tamper output - 2"]);
            panel_tamper_outputs[2] = (byte)((int)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Panel tamper output - 3"]);
            #endregion

            //Panel tamper config
            byte[] panel_tamper_config_bytes = (byte[])attributes["panel_tamper_config"]["value"];
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Panel tamper active"].Equals(true))
            {
                panel_tamper_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["panel_tamper_config"]["KP_TAMPER_CONFIG_ATIVO"])[0]);
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Panel tamper type"].Equals(0x01))
            {
                panel_tamper_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["panel_tamper_config"]["KP_TAMPER_CONFIG_NORMAL_CLOSE"])[0]);
            }

            //keyswitch_config
            byte[] keyswitch_config_bytes = (byte[])attributes["keyswitch_config"]["value"];
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Keyswitch active"].Equals(true))
            {
                keyswitch_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["keyswitch_config"]["KP_KEYSWITCH_CONFIG_ATIVO"])[0]);
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Keyswitch type"].Equals(0x02))
            {
                keyswitch_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["keyswitch_config"]["KP_KEYSWITCH_CONFIG_NORMAL_CLOSE"])[0]);
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Keyswitch toggle"].Equals(true))
            {
                keyswitch_config_bytes[0] += (byte)(0xFF & (byte)((uint[])this.attributes["keyswitch_config"]["KP_KEYSWITCH_CONFIG_TOGGLE"])[0]);
            }

            #region Keyswitch area away
            byte keyswitch_area_away = 0;
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 1 away"].Equals(true))
            {
                keyswitch_area_away += 0x01;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 2 away"].Equals(true))
            {
                keyswitch_area_away += 0x02;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 3 away"].Equals(true))
            {
                keyswitch_area_away += 0x04;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 4 away"].Equals(true))
            {
                keyswitch_area_away += 0x08;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 5 away"].Equals(true))
            {
                keyswitch_area_away += 0x10;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 6 away"].Equals(true))
            {
                keyswitch_area_away += 0x20;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 7 away"].Equals(true))
            {
                keyswitch_area_away += 0x40;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 8 away"].Equals(true))
            {
                keyswitch_area_away += 0x80;
            }
            #endregion

            #region Keyswitch area stay
            byte keyswitch_area_stay = 0;
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 1 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x01;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 2 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x02;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 3 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x04;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 4 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x08;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 5 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x10;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 6 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x20;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 7 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x40;
            }
            if (mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Keyswitch - area 8 stay"].Equals(true))
            {
                keyswitch_area_stay += 0x80;
            }
            #endregion

            #region AUDIO TRACKS
            byte[] audio_tracks = new byte[8];
            audio_tracks[0] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 1"].ToString()) - 1) & 0xFF);
            audio_tracks[1] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 1"].ToString()) - 1) >> 8);
            audio_tracks[2] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 2"].ToString()) - 1) & 0xFF);
            audio_tracks[3] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 2"].ToString()) - 1) >> 8);
            audio_tracks[4] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 3"].ToString()) - 1) & 0xFF);
            audio_tracks[5] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 3"].ToString()) - 1) >> 8);
            audio_tracks[6] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 4"].ToString()) - 1) & 0xFF);
            audio_tracks[7] = (byte)((int.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Audio track 4"].ToString()) - 1) >> 8);
            #endregion

            string maintenance_description = ((string)mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Maintenance description"]).ToUpper();

            ulong maintenance_phone_number = ulong.Parse(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Maintenance phone number"].ToString());

            DateTime maintenance_date = (DateTime)(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)(global_system_index)]["Maintenance date"]);


            byte[] output_call_code_bytes = Encoding.ASCII.GetBytes(mainWindow.databaseDataSet.GlobalSystem.Rows[(int)global_system_index]["Outputs code"].ToString());
            byte[] installer_code_bytes = GetIntArray(installer_code);
            byte[] phone_prefix_bytes = GetIntArray(phone_prefix);
            byte[] global_system_options_bytes = BitConverter.GetBytes(global_system_options);
            byte[] dial_report_delay_bytes = BitConverter.GetBytes(dial_report_delay);
            byte[] mains_fail_report_delay_bytes = BitConverter.GetBytes(mains_fail_report_delay);
            byte[] receiver_fail_delay_bytes = BitConverter.GetBytes(receiver_fail_delay);
            byte[] radio_detector_supervised_timer_bytes = BitConverter.GetBytes(radio_detector_supervised_timer);
            byte[] CRA_client_number_bytes = BitConverter.GetBytes(CRA_client_number);
            byte[] event_code_session_bytes = BitConverter.GetBytes(event_code_session);
            byte[] maintenance_description_bytes = new byte[64];
            maintenance_description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(maintenance_description);
            byte[] maintenance_phone_number_bytes = GetIntArray(maintenance_phone_number);
            byte[] maintenance_date_bytes = new byte[4];
            maintenance_date_bytes[0] = (byte)(maintenance_date.Day);
            maintenance_date_bytes[1] = (byte)(maintenance_date.Month);
            maintenance_date_bytes[2] = (BitConverter.GetBytes(maintenance_date.Year))[0];
            maintenance_date_bytes[3] = (BitConverter.GetBytes(maintenance_date.Year))[1];
            byte[] outputs_permissions_bytes = BitConverter.GetBytes(outputs_permissions);

            int i = 0;
            uint j = 0;
            uint global_syste_addr = Constants.KP_GLOBAL_SYSTEM_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_GLOBALSYSTEM_FLASH * (global_system_index));

            byte_array[i++] = 0x40;
            byte_array[i++] = (byte)((global_syste_addr >> 16) & 0xFF);
            byte_array[i++] = (byte)((global_syste_addr >> 8) & 0xFF);
            byte_array[i++] = (byte)(global_syste_addr & 0xFF);
            byte_array[i++] = 240;
            int temp = i;

            //Output call code
            for (i = (int)attributes["output_call_code"]["address"] + temp, j = 0; i < ((int)attributes["output_call_code"]["address"] + temp + output_call_code_bytes.Length); i++, j++)
            {
                byte_array[i] = (byte)(output_call_code_bytes[j] - 0x30); 

                //Add 0xFF if code don't have maximum length
                if (i.Equals(((int)attributes["output_call_code"]["address"] + temp + (output_call_code_bytes.Length - 1))) && output_call_code_bytes.Length != 8)
                {
                    int counter = 8 - output_call_code_bytes.Length;

                    for (int k = counter; k > 0; k--)
                    {
                        byte_array[i + k] = 0xFF;
                    }
                }
            }

            //options
            for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + global_system_options_bytes.Length); i++, j++)
            {
                byte_array[i] = global_system_options_bytes[j];
            }

            //installer code
            for (i = (int)attributes["installer_code"]["address"] + temp, j = 0; i < ((int)attributes["installer_code"]["address"] + temp + installer_code_bytes.Length); i++, j++)
            {
                byte_array[i] = (byte)installer_code_bytes[j];
                if (installer_code_bytes.Length < 8 && i == ((int)attributes["installer_code"]["address"] + temp + installer_code_bytes.Length - 1))
                {
                    for (i = ((int)attributes["installer_code"]["address"] + temp + installer_code_bytes.Length); i < (int)attributes["installer_code"]["address"] + sizeof(ulong) + temp; i++)
                    {
                        byte_array[i] = 0xFF;
                    }
                }
            }

            //Coation digit
            i = (temp + (int)this.attributes["coation_digit"]["address"]);
            byte_array[i] = coation_digit;
            i++;

            //phone prefix
            if (phone_prefix_bytes.Length == 0)
            {
                phone_prefix_bytes = new byte[] { 0xff, 0xff, 0xff, 0xff, 0xff, 0xff };
                for (i = (int)attributes["phone_prefix"]["address"] + temp, j = 0; i < ((int)attributes["phone_prefix"]["address"] + temp + phone_prefix_bytes.Length); i++, j++)
                {
                    byte_array[i] = (byte)phone_prefix_bytes[j];
                }
            }
            else
            {
                for (i = (int)attributes["phone_prefix"]["address"] + temp, j = 0; i < ((int)attributes["phone_prefix"]["address"] + temp + phone_prefix_bytes.Length); i++, j++)
                {
                    byte_array[i] = (byte)phone_prefix_bytes[j];
                    if (phone_prefix_bytes.Length < 6 && i == ((int)attributes["phone_prefix"]["address"] + temp + phone_prefix_bytes.Length - 1))
                    {
                        for (i = ((int)attributes["phone_prefix"]["address"] + temp + phone_prefix_bytes.Length); i < (int)attributes["phone_prefix"]["address"] + 6 + temp; i++)
                        {
                            byte_array[i] = 0xFF;
                        }
                    }
                }
            }
           

            //Reserved 1
            for (i = (temp + (int)this.attributes["reserved_1"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_1"]["address"] + reserved_1_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_1_bytes[j];
            }

            //dial_report_delay 2
            for (i = (temp + (int)this.attributes["dial_report_delay"]["address"]), j = 0; i < (temp + (int)this.attributes["dial_report_delay"]["address"] + dial_report_delay_bytes.Length); i++, j++)
            {
                byte_array[i] = dial_report_delay_bytes[j];
            }

            //mains_fail_report_delay
            for (i = (temp + (int)this.attributes["mains_fail_report_delay"]["address"]), j = 0; i < (temp + (int)this.attributes["mains_fail_report_delay"]["address"] + mains_fail_report_delay_bytes.Length); i++, j++)
            {
                byte_array[i] = mains_fail_report_delay_bytes[j];
            }

            //receiver_fail_delay
            for (i = (temp + (int)this.attributes["receiver_fail_delay"]["address"]), j = 0; i < (temp + (int)this.attributes["receiver_fail_delay"]["address"] + receiver_fail_delay_bytes.Length); i++, j++)
            {
                byte_array[i] = receiver_fail_delay_bytes[j];
            }

            //radio_detector_supervised_timer
            for (i = (temp + (int)this.attributes["radio_detector_supervised_timer"]["address"]), j = 0; i < (temp + (int)this.attributes["radio_detector_supervised_timer"]["address"] + radio_detector_supervised_timer_bytes.Length); i++, j++)
            {
                byte_array[i] = radio_detector_supervised_timer_bytes[j];
            }

            //CRA_client_number
            for (i = (temp + (int)this.attributes["CRA_client_number"]["address"]), j = 0; i < (temp + (int)this.attributes["CRA_client_number"]["address"] + CRA_client_number_bytes.Length); i++, j++)
            {
                byte_array[i] = CRA_client_number_bytes[j];
            }

            //event_code_session
            for (i = (temp + (int)this.attributes["event_code_session"]["address"]), j = 0; i < (temp + (int)this.attributes["event_code_session"]["address"] + event_code_session_bytes.Length); i++, j++)
            {
                byte_array[i] = event_code_session_bytes[j];
            }

            //siren tamper outputs
            for (i = ((int)attributes["siren_tamper"]["address"] + temp), j = 0; i < ((int)attributes["siren_tamper"]["address"] + temp + (siren_tamper_outputs.Length)); i++, j++)
            {
                byte_array[i] = siren_tamper_outputs[j];
            }

            //siren_tamper_config
            i = (temp + (int)this.attributes["siren_tamper_config"]["address"]);
            byte_array[i] = siren_tamper_config_bytes[0];
            i++;

            //panel tamper outputs
            for (i = ((int)attributes["panel_tamper"]["address"] + temp), j = 0; i < ((int)attributes["panel_tamper"]["address"] + temp + (panel_tamper_outputs.Length)); i++, j++)
            {
                byte_array[i] = panel_tamper_outputs[j];
            }

            //panel_tamper_config
            i = (temp + (int)this.attributes["panel_tamper_config"]["address"]);
            byte_array[i] = panel_tamper_config_bytes[0];
            i++;

            //keyswitch_config
            i = (temp + (int)this.attributes["keyswitch_config"]["address"]);
            byte_array[i] = keyswitch_config_bytes[0];
            i++;

            //keyswitch_area_away
            i = (temp + (int)this.attributes["keyswitch_area_away"]["address"]);
            byte_array[i] = keyswitch_area_away;
            i++;

            //keyswitch_area_stay
            i = (temp + (int)this.attributes["keyswitch_area_stay"]["address"]);
            byte_array[i] = keyswitch_area_stay;
            i++;

            //Reserved 2
            for (i = (temp + (int)this.attributes["reserved_2"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_2"]["address"] + reserved_2_bytes.Length); i++, j++)
            {
                byte_array[i] = reserved_2_bytes[j];
            }

            //Audio tracks
            for (i = ((int)attributes["audio_tracks"]["address"] + temp), j = 0; i < ((int)attributes["audio_tracks"]["address"] + temp + (audio_tracks.Length)); i++, j++)
            {
                byte_array[i] = audio_tracks[j];
            }

            //Maintenance description
            for (i = (int)attributes["maintenance_description"]["address"] + temp, j = 0; i < ((int)attributes["maintenance_description"]["address"] + temp + maintenance_description_bytes.Length); i++, j++)
            {
                byte_array[i] = maintenance_description_bytes[j];
            }

            //maintenance_phone_number
            for (i = (int)attributes["maintenance_phone_number"]["address"] + temp, j = 0; i < ((int)attributes["maintenance_phone_number"]["address"] + temp + maintenance_phone_number_bytes.Length); i++, j++)
            {
                byte_array[i] = (byte)maintenance_phone_number_bytes[j];
                if (maintenance_phone_number_bytes.Length < 16 && i == ((int)attributes["maintenance_phone_number"]["address"] + temp + maintenance_phone_number_bytes.Length - 1))
                {
                    byte_array[i + 1] = 0xFF;
                }
            }
            //maintenance_date 
            for (i = (temp + (int)this.attributes["maintenance_date"]["address"]), j = 0; i < (temp + (int)this.attributes["maintenance_date"]["address"] + maintenance_date_bytes.Length); i++, j++)
            {
                byte_array[i] = maintenance_date_bytes[j];
            }

            #region Outputs permissions 
            for (i = ((int)attributes["outputs_permissions"]["address"] + temp), j = 0; i < ((int)attributes["outputs_permissions"]["address"] + temp + (outputs_permissions_bytes.Length)); i++, j++)
            {
                byte_array[i] = outputs_permissions_bytes[j];
            }
            #endregion

            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow);

            System.Threading.Thread.Sleep(250);
        }

        byte[] GetIntArray(ulong num)
        {
            List<byte> listOfInts = new List<byte>();
            while (num > 0)
            {
                listOfInts.Add((byte)(num % 10));
                num = num / 10;
            }
            listOfInts.Reverse();
            return listOfInts.ToArray();
        }
    }
}
