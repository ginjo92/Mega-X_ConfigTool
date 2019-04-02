using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Phones
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
                "phone_number",
                new Dictionary<string, object>
                {
                    { "value", new byte[16] },
                    { "address", 64 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "prefix",
                new Dictionary<string, object>
                {
                    { "value", new byte[6] },
                    { "address", 80 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "max_com_attempts",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 86 },
                    { "data_grid_view_addr", 7 }
                }
            },
            //not used
            {
                "com_protocol",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 87 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "partition_id",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 88 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "reserved_1",
                new Dictionary<string, object>
                {
                    { "value", new byte[3] },
                    { "address", 89 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "reserved_2",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 92 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 96 },
                    //OPTIONS 1                                                                                     //----
                    { "KP_TELT_REP_ATIVO", new ulong[2] { 0x01, 63} },                                              //bit0
                    { "KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_VOICE", new ulong[2] { 0x02, 0} },                   //bit1
                    { "KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_CID", new ulong[2] { 0x04, 1} },                     //bit2
                    { "KP_TELF_REP_ALLWAYS_REPORT", new ulong[2] { 0x08, 2} },                                      //bit3
                    { "KP_TELF_REP_MONITOR_CALL_PROGRESS", new ulong[2] { 0x10, 3} },                               //bit4
                    { "KP_TELF_REP_BLIND_DIAL", new ulong[2] { 0x20, 4} },                                          //bit5
                    { "KP_TELF_REP_STAY_ON_LINE_ATFER_REPORT_2WAY_VOICE", new ulong[2] { 0x40, 5} },                //bit6
                    { "KP_TELF_REP_USE_DIALLING_PREFIX", new ulong[2] { 0x80, 6} },                                 //bit7
                    //OPTIONS 2                                                                                     //----
                    { "KP_TELF_REP_MAINS_FUSE_FAILURE", new ulong[2] { 0x100, 8} },                                 //bit8
                    { "KP_TELF_REP_BATTERY_LOW", new ulong[2] { 0x200, 9} },                                        //bit9
                    { "KP_TELF_REP_RADIO_BATTERY_LOW", new ulong[2] { 0x400, 10} },                                 //bit10
                    { "KP_TELF_REP_LINE_FAIL", new ulong[2] { 0x800, 11} },                                         //bit11
                    { "KP_TELF_REP_SYSTEM_TAMPER", new ulong[2] { 0x1000, 12} },                                    //bit12
                    { "KP_TELF_REP_KEYPAD_TAMPER", new ulong[2] { 0x2000, 13} },                                    //bit13
                    { "KP_TELF_REP_ZONE_TAMPER", new ulong[2] { 0x4000, 14} },                                      //bit14
                    { "KP_TELF_REP_RADIO_ZONE_TAMPER", new ulong[2] { 0x8000, 15} },                                //bit15    
                    //OPTIONS 3                                                                                     //----
                    { "KP_TELF_REP_DURESS_ALARM", new ulong[2] { 0x10000, 16} },                                    //bit16
                    { "KP_TELF_REP_SUPERVISED_RADIO_ALARM", new ulong[2] { 0x20000, 17} },                          //bit17
                    { "KP_TELF_REP_SENSOR_WATCH_ALARM", new ulong[2] { 0x40000, 18} },                              //bit18
                    { "KP_TELF_REP_MANUAL_PANIC_ALARM", new ulong[2] { 0x80000, 19} },                              //bit19    
                    { "KP_TELF_REP_MANUAL_FIRE_ALARM", new ulong[2] { 0x100000, 20} },                              //bit20
                    { "KP_TELF_REP_MANUAL_MEDICAL_ALARM", new ulong[2] { 0x200000, 21} },                           //bit21
                    { "KP_TELF_REP_MANUAL_PEDANT_PANIC_ALARM", new ulong[2] { 0x400000, 22} },                      //bit22
                    { "KP_TELF_REP_ZONE_BYPASS", new ulong[2] { 0x800000, 23} },                                    //bit23
                    //OPTIONS 4                                                                                     //----
                    { "KP_TELF_REP_ARM_DISARM", new ulong[2] { 0x1000000, 24} },                                    //bit24
                    { "KP_TELF_REP_TEST_CALL", new ulong[2] { 0x2000000, 61} },                                     //bit25
                    { "KP_TELT_REP_CHAMADA_VOZ", new ulong[2] { 0x4000000, 62} },                                   //bit26
                    { "KP_TELF_REP_RF_INTERFERENCE_DETECTED", new ulong[2] { 0x8000000, 40} },                      //bit27
                    { "KP_TELF_REP_STAY_MODE_ZONE_ALARM", new ulong[2] { 0x10000000, 28} },                         //bit28
                    { "KP_TELF_REP_SYSTEM_PROBLEMS", new ulong[2] { 0x20000000, 41} },                              //bit29
                    { "KP_TELF_REP_ACCESS_TO_INSTALLER_MODE", new ulong[2] { 0x40000000, 30} },                     //bit30
                    { "KP_TELF_REP_24HOUR_ALARMS_WHEN_SET_DOMESCTIC_VOICE", new ulong[2] { 0x80000000, 31} },       //bit31
                    //OPTIONS 5                                                                                     //----
                    { "KP_TELF_REP_ZONES_RESTORES", new ulong[2] { 0x100000000, 32} },                              //bit32
                    { "KP_TELF_REP_LATCH_KEYS", new ulong[2] { 0x200000000, 33} },                                  //bit33    
                    { "KP_TELF_REP_DELINQUENT", new ulong[2] { 0x400000000, 34} },                                  //bit34
                    { "KP_TELF_REP_TESTS", new ulong[2] { 0x800000000, 35} },                                       //bit35
                    { "KP_TELF_REP_FUSE_FAILURE", new ulong[2] { 0x1000000000, 36} },                               //bit36
                    { "KP_TELF_REP_OUTPUTS_FAIL", new ulong[2] { 0x2000000000, 37} },                               //bit37
                    { "KP_TELF_REP_RTC_TIME_CHANGE", new ulong[2] { 0x4000000000, 38} },                            //bit38
                    { "KP_TELF_REP_KEYPAD_BUS_TROUBLE", new ulong[2] { 0x8000000000, 39} },                         //bit39
                    //OPTIONS 6                                                                                     //----
                    { "KP_TELF_REP_STAY_MODE_ARM_DISARM", new ulong[2] { 0x10000000000, 25} },                      //bit40
                    { "KP_TELF_REP_DISARM_ONLY_AFTER_ACTIVATION", new ulong[2] { 0x20000000000, 26} },              //bit41    
                    { "KP_TELF_REP_STAY_MODE_DISARM_ONLY_AFTER_ACTIVATION", new ulong[2] { 0x40000000000, 27} },    //bit42
                    { "KP_TELF_REP_ACCESS_TO_PROGRAM_MODE", new ulong[2] { 0x80000000000, 29} },                    //bit43
                    { "KP_TELF_REP_CALL_BACK_NUMBER", new ulong[2] { 0x100000000000, 7} }                           //bit44
                                                                                                                    //bit45
                                                                                                                    //bit46
                                                                                                                    //bit47                            
                }
            },
            {
                "hour_test",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 104 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "minute_test",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 105 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "week_days",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 106 },
                    { "data_grid_view_addr", 7 }
                }
            }          
     };

        internal void read(MainWindow mainWindow, uint phone_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint phone_address = Constants.KP_PHONES_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_TELEFONE_FLASH * phone_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((phone_address >> 16) & 0xff);
            byte_array[i++] = (byte)((phone_address >> 8) & 0xff);
            byte_array[i++] = (byte)((phone_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow);
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainWindow, uint phone_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho

            string description = ((string)mainWindow.databaseDataSet.Phone.Rows[(int)(phone_number)]["Description"]).ToUpper();

            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

            //phone number
            ulong phone_dial_number = ulong.Parse(mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Phone number"].ToString());

            //prefix
            ulong phone_prefix = ulong.Parse(mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Prefix"].ToString());

            //Max Communication attempts
            byte com_attempts = byte.Parse(mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Communication attempts"].ToString());

            //communication protocol
            byte[] com_protocol = (byte[])mainWindow.databaseDataSet.Phone.Rows[(int)(phone_number)]["Communication protocol"];

            #region PARTITIONS
            short partition_id = 0;
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 1"].Equals(true))
            {
                partition_id += 0x01;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 2"].Equals(true))
            {
                partition_id += 0x02;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 3"].Equals(true))
            {
                partition_id += 0x04;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 4"].Equals(true))
            {
                partition_id += 0x08;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 5"].Equals(true))
            {
                partition_id += 0x10;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 6"].Equals(true))
            {
                partition_id += 0x20;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 7"].Equals(true))
            {
                partition_id += 0x40;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Area 8"].Equals(true))
            {
                partition_id += 0x80;
            }
            #endregion

            #region RESERVED
            byte[] reserved_1_bytes = (byte[])mainWindow.databaseDataSet.Phone.Rows[(int)(phone_number)]["Reserved 1"];
            byte[] reserved_2_bytes = (byte[])mainWindow.databaseDataSet.Phone.Rows[(int)(phone_number)]["Reserved 2"];
            #endregion

            #region Phone options read from dataset
            ////Phone OPTIONS
            ulong phone_options = 0;
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Stop dialling if kissed off voice"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_VOICE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Stop dialling if kissed off CID"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_CID"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Always report"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ALLWAYS_REPORT"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Monitor call progress"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_MONITOR_CALL_PROGRESS"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Blind dial"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_BLIND_DIAL"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Stay on line after report 2 way voice"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_STAY_ON_LINE_ATFER_REPORT_2WAY_VOICE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Use prefix"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_USE_DIALLING_PREFIX"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Call back number"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_CALL_BACK_NUMBER"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report 230V fail"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_MAINS_FUSE_FAILURE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report battery low"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_BATTERY_LOW"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report radio battery low"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_RADIO_BATTERY_LOW"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report line fail"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_LINE_FAIL"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report system tamper"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_SYSTEM_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report keypad tamper"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_KEYPAD_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report zone tamper"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ZONE_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report radio zone tamper"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_RADIO_ZONE_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report duress alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_DURESS_ALARM"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report supervised radio alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_SUPERVISED_RADIO_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report sensor watch alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_SENSOR_WATCH_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report manual panic alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_MANUAL_PANIC_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report manual fire alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_MANUAL_FIRE_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report manual medical alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_MANUAL_MEDICAL_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report manual pedant panic alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_MANUAL_PEDANT_PANIC_ALARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report zone bypass"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ZONE_BYPASS"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report arm/disarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ARM_DISARM"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report stay mode arm/disarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_STAY_MODE_ARM_DISARM"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report disarm only after activation"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_DISARM_ONLY_AFTER_ACTIVATION"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report stay mode disarm only after activation"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_STAY_MODE_DISARM_ONLY_AFTER_ACTIVATION"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report stay mode zone alarm"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_STAY_MODE_ZONE_ALARM"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report access to program mode"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ACCESS_TO_PROGRAM_MODE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report access to installer mode"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ACCESS_TO_INSTALLER_MODE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report 24h alarms when set domestic voice"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_24HOUR_ALARMS_WHEN_SET_DOMESCTIC_VOICE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report Zones restore"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_ZONES_RESTORES"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report latch keys"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_LATCH_KEYS"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report delinquent"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_DELINQUENT"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report tests"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_TESTS"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report fuse failure"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_FUSE_FAILURE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report outputs failure"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_OUTPUTS_FAIL"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report date/hour change"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_RTC_TIME_CHANGE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report keypad communication problems"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_KEYPAD_BUS_TROUBLE"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report RF problems detected"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_RF_INTERFERENCE_DETECTED"])[0]));
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report system problems"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_SYSTEM_PROBLEMS"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Test call"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELF_REP_TEST_CALL"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Voice call"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELT_REP_CHAMADA_VOZ"])[0]));
            }

            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Report on"].Equals(true))
            {
                phone_options += (ulong)(0xFFFFFFFFFFFFFFFF & (((ulong[])this.attributes["options"]["KP_TELT_REP_ATIVO"])[0]));
            }
            #endregion

            //Hour test
            DateTime test_hour = Convert.ToDateTime(mainWindow.databaseDataSet.Phone.Rows[(int)(phone_number)]["Test hour"].ToString());
            byte hour_test = (byte)test_hour.Hour;
            //minute test
            byte minute_test = (byte)test_hour.Minute;


            //WEEK DAYS
            byte week_days = 0;
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Monday"].Equals(true))
            {
                week_days += 0x01;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Tuesday"].Equals(true))
            {
                week_days += 0x02;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Wednesday"].Equals(true))
            {
                week_days += 0x04;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Thursday"].Equals(true))
            {
                week_days += 0x08;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Friday"].Equals(true))
            {
                week_days += 0x10;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Saturday"].Equals(true))
            {
                week_days += 0x20;
            }
            if (mainWindow.databaseDataSet.Phone.Rows[(int)phone_number]["Sunday"].Equals(true))
            {
                week_days += 0x40;
            }


            byte[] phone_number_bytes = GetIntArray(phone_dial_number);
            byte[] phone_prefix_bytes = GetIntArray(phone_prefix);
            byte[] phone_partitions_bytes = BitConverter.GetBytes(partition_id);
            byte[] phone_options_bytes = BitConverter.GetBytes(phone_options);

            int i = 0;
            uint j = 0;
            uint phone_address = Constants.KP_PHONES_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_TELEFONE_FLASH * phone_number);
            byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
            byte_array[i++] = (byte)((phone_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((phone_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(phone_address & 0xFF);
            byte_array[i++] = 240;
            int temp = i;

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

            //phone number
            for (i = (int)attributes["phone_number"]["address"] + temp, j = 0; i < ((int)attributes["phone_number"]["address"] + temp + phone_number_bytes.Length); i++, j++)
            {
                byte_array[i] = (byte)phone_number_bytes[j];
                if (phone_number_bytes.Length < 16 && i == ((int)attributes["phone_number"]["address"] + temp + phone_number_bytes.Length -1))
                {
                    byte_array[i + 1] = 0xFF;
                }
            }

            //phone prefix
            if (phone_prefix_bytes.Length == 0)
            {
                phone_prefix_bytes = new byte [] {0xff, 0xff, 0xff, 0xff, 0xff, 0xff};
                for (i = (int)attributes["prefix"]["address"] + temp, j = 0; i < ((int)attributes["prefix"]["address"] + temp + phone_prefix_bytes.Length); i++, j++)
                {
                    byte_array[i] = (byte)phone_prefix_bytes[j];
                }
            }
            else
            {
                for (i = (int)attributes["prefix"]["address"] + temp, j = 0; i < ((int)attributes["prefix"]["address"] + temp + phone_prefix_bytes.Length); i++, j++)
                {
                    byte_array[i] = (byte)phone_prefix_bytes[j];
                    if (phone_prefix_bytes.Length < 6 && i == ((int)attributes["prefix"]["address"] + temp + phone_prefix_bytes.Length - 1))
                    {
                        byte_array[i + 1] = 0xFF;
                    }
                }
            }
            
            

            // max com attempts
            i = (temp + (int)this.attributes["max_com_attempts"]["address"]);
            byte_array[i] = com_attempts;
            i++;

            //Com protocol
            for (i = (temp + (int)this.attributes["com_protocol"]["address"]), j = 0; i < (temp + (int)this.attributes["com_protocol"]["address"] + com_protocol.Length); i++, j++)
            {
                byte_array[i] = com_protocol[j];
            }

            //partitions
            for (i = (temp + (int)this.attributes["partition_id"]["address"]), j = 0; i < (temp + (int)this.attributes["partition_id"]["address"] + phone_partitions_bytes.Length); i++, j++)
            {
                byte_array[i] = phone_partitions_bytes[j];
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

            //options
            for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + phone_options_bytes.Length); i++, j++)
            {
                byte_array[i] = phone_options_bytes[j];
            }

            //hour test
            i = (temp + (int)this.attributes["hour_test"]["address"]);
            byte_array[i] = hour_test;
            i++;

            //minute test
            i = (temp + (int)this.attributes["minute_test"]["address"]);
            byte_array[i] = minute_test;
            i++;

            //Week days
            i = (temp + (int)this.attributes["week_days"]["address"]);
            byte_array[i] = week_days;
            i++;

            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            //protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow);
            protocol.send_msg_block((uint)i, byte_array, phone_address, mainWindow.cp_id, mainWindow, Constants.KP_FLASH_TAMANHO_DADOS_TELEFONE_FLASH); // TODO: Check if cp_id is needed
            System.Threading.Thread.Sleep(mainWindow.intervalsleeptime);
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
