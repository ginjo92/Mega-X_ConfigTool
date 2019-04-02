using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Users
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
            { // TODO: Check user options
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 64 },
                    //OPTIONS 1                                                     //----
                    { "user_active", new int[2] { 0x01, 7} },                       //bit0
                    { "can_arm_away", new int[2] { 0x02, 0} },                      //bit1
                    { "can_arm_stay", new int[2] { 0x04, 1} },                      //bit2
                    { "can_disarm_away", new int[2] { 0x08, 2} },                   //bit3
                    { "can_disarm_stay", new int[2] { 0x10, 3} },                   //bit4 
                    { "check_timezones", new int[2] { 0x20, 14} },                  //bit5
                    { "can_divert", new int[2] { 0x40, 5} },                        //bit6
                    { "can_view_events", new int[2] { 0x80, 6} },                   //bit7
                    //OPTIONS 2                                                     //----    
                    { "can_change_clock", new int[2] { 0x100, 12} },                //bit8
                    { "check_date", new int[2] { 0x200, 15} },                      //bit9
                    { "can_change_own_code", new int[2] { 0x400, 8} },              //bit10
                    { "can_change_all_codes", new int[2] { 0x800, 9} },             //bit11
                    { "allow_installer_mode", new int[2] { 0x1000, 10} },           //bit12
                    { "can_change_phone_numbers", new int[2] { 0x2000, 11} },       //bit13
                                                                                    //bit14
                    { "allow_dtmf_codes", new int[2] { 0x8000, 13} },               //bit15
                }
            },

            {
                "user_code",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 68 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "user_type",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 76 },
                    { "data_grid_view_addr", 16 }
                }
            },
            //NOT USED
            {
                "group_id",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 77 },
                    { "data_grid_view_addr", 17 }
                }
            },
            {
                "partition_id",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 78 },
                    { "data_grid_view_addr", 18 }
                }
            },
            {
                "reserved",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 79 },
                    { "data_grid_view_addr", 26 }
                }
            },
            {
                "initial_date",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 80 },
                    { "data_grid_view_addr", 27 }
                }
            },
            {
                "final_date",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 84 },
                    { "data_grid_view_addr", 28 }
                }
            },
            {
                "timezones_while_active",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 88 },
                    { "data_grid_view_addr", 29 }
                }
            },
            {
                "timezones_while_inactive",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 89 },
                    { "data_grid_view_addr", 38 }
                }
            },
            {
                "reserved_2",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 90 },
                    { "data_grid_view_addr", 9 }
                }
            },
            {
                "reserved_3",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 91 },
                    { "data_grid_view_addr", 9 }
                }
            },
            {
                "particoes_bot_away",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 92 },
                    { "data_grid_view_addr", 38 }
                }
            },
            {
                "particoes_bot_stay",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 96 },
                    { "data_grid_view_addr", 38 }
                }
            },

            {
                "audio_tracks",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 100 },
                    { "data_grid_view_addr", 38 }
                }
            },
             {
                "reserved_4",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address",  108},
                    { "data_grid_view_addr", 9 }
                }
            },
            {
                "reserved_5",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 109 },
                    { "data_grid_view_addr", 9 }
                }
            },
            {
                "reserved_6",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 110 },
                    { "data_grid_view_addr", 9 }
                }
            },
            {
                "reserved_7",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 111 },
                    { "data_grid_view_addr", 9 }
                }
            },
             {
                "outputs_permissions",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 112 },
                }
             },
            //},

            //  {
            //    "full_user_code",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[8] },
            //        { "address", 68 },
            //        { "data_grid_view_addr", 7 }
            //    }
            //},

        };

        internal void read(MainWindow mainWindow, uint user_number)
        {
            byte[] byte_array = new byte[128];
            uint i = 0;
            uint user_address = Constants.KP_USERS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_USERS_FLASH * user_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((user_address >> 16) & 0xff);
            byte_array[i++] = (byte)((user_address >> 8) & 0xff);
            byte_array[i++] = (byte)((user_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainForm, uint user_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            //user_number = user_number - 1;
            uint user_address = Constants.KP_USERS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_USERS_FLASH * user_number);
            General protocol = new General();

            if (user_number < Constants.KP_MAX_USERS)
            {
                string description = "";
                try
                {
                    description = ((string)mainForm.databaseDataSet.User.Rows[(int)(user_number)]["Description"]).ToUpper();
                }
                catch
                {
                }

                #region User options read from dataset
                ////USER OPTIONS
                Int32 user_options = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can arm away"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_arm_away"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can arm stay"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_arm_stay"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can disarm away"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_disarm_away"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can disarm stay"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_disarm_stay"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Security guard code"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["security_guard_code"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can divert"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_divert"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can view event memory"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_view_events"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["User active"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["user_active"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can change own code"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_change_own_code"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can change all codes"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_change_all_codes"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Allow installer mode"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["allow_installer_mode"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can change phone numbers"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_change_phone_numbers"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Can change clock"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["can_change_clock"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Allow DTMF codes"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["allow_dtmf_codes"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Check timezones"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["check_timezones"])[0]));
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Check date"].Equals(true))
                {
                    user_options += (int)(0xFFFFFFFF & (((int[])this.attributes["options"]["check_date"])[0]));
                }
                #endregion

                string user_code = mainForm.databaseDataSet.User.Rows[(int)user_number]["UserCode"].ToString();

                short user_type = (short)((int)mainForm.databaseDataSet.User.Rows[(int)(user_number)]["User type"]);
                short group_id = (short)((int)mainForm.databaseDataSet.User.Rows[(int)(user_number)]["Group Id"]);
                short partition_id = 0;
                #region USER PARTITIONS FLAGS
                //User partitions
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 1"].Equals(true))
                {
                    partition_id += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 2"].Equals(true))
                {
                    partition_id += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 3"].Equals(true))
                {
                    partition_id += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 4"].Equals(true))
                {
                    partition_id += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 5"].Equals(true))
                {
                    partition_id += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 6"].Equals(true))
                {
                    partition_id += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 7"].Equals(true))
                {
                    partition_id += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Area 8"].Equals(true))
                {
                    partition_id += 0x80;
                }
                #endregion
                short reserved = (short)((int)mainForm.databaseDataSet.User.Rows[(int)(user_number)]["Reserved"]);

                #region USER TIMEZONES FLAGS
                short timezones_while_active = 0;
                ///TIMEZONE while active
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 1 while active"].Equals(true))
                {
                    timezones_while_active += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 2 while active"].Equals(true))
                {
                    timezones_while_active += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 3 while active"].Equals(true))
                {
                    timezones_while_active += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 4 while active"].Equals(true))
                {
                    timezones_while_active += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 5 while active"].Equals(true))
                {
                    timezones_while_active += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 6 while active"].Equals(true))
                {
                    timezones_while_active += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 7 while active"].Equals(true))
                {
                    timezones_while_active += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 8 while active"].Equals(true))
                {
                    timezones_while_active += 0x80;
                }
                ///TIMEZONE while inactive
                short timezones_while_inactive = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 1 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 2 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 3 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 4 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 5 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 6 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 7 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Timezone 8 while inactive"].Equals(true))
                {
                    timezones_while_inactive += 0x80;
                }
                #endregion
                DateTime initial_date = (DateTime)(mainForm.databaseDataSet.User.Rows[(int)(user_number)]["Initial date"]);
                DateTime final_date = (DateTime)(mainForm.databaseDataSet.User.Rows[(int)(user_number)]["Final date"]);

                #region USER BUTTONS CONFIGURATION
                #region PARTITIONS AWAY
                //BUTTON A
                byte user_button_a_part_away = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 1"].Equals(true))
                {
                    user_button_a_part_away += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 2"].Equals(true))
                {
                    user_button_a_part_away += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 3"].Equals(true))
                {
                    user_button_a_part_away += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 4"].Equals(true))
                {
                    user_button_a_part_away += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 5"].Equals(true))
                {
                    user_button_a_part_away += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 6"].Equals(true))
                {
                    user_button_a_part_away += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 7"].Equals(true))
                {
                    user_button_a_part_away += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part away 8"].Equals(true))
                {
                    user_button_a_part_away += 0x80;
                }

                //BUTTON B
                byte user_button_b_part_away = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 1"].Equals(true))
                {
                    user_button_b_part_away += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 2"].Equals(true))
                {
                    user_button_b_part_away += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 3"].Equals(true))
                {
                    user_button_b_part_away += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 4"].Equals(true))
                {
                    user_button_b_part_away += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 5"].Equals(true))
                {
                    user_button_b_part_away += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 6"].Equals(true))
                {
                    user_button_b_part_away += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 7"].Equals(true))
                {
                    user_button_b_part_away += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part away 8"].Equals(true))
                {
                    user_button_b_part_away += 0x80;
                }

                //BUTTON C
                byte user_button_c_part_away = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 1"].Equals(true))
                {
                    user_button_c_part_away += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 2"].Equals(true))
                {
                    user_button_c_part_away += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 3"].Equals(true))
                {
                    user_button_c_part_away += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 4"].Equals(true))
                {
                    user_button_c_part_away += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 5"].Equals(true))
                {
                    user_button_c_part_away += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 6"].Equals(true))
                {
                    user_button_c_part_away += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 7"].Equals(true))
                {
                    user_button_c_part_away += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part away 8"].Equals(true))
                {
                    user_button_c_part_away += 0x80;
                }

                //BUTTON D
                byte user_button_d_part_away = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 1"].Equals(true))
                {
                    user_button_d_part_away += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 2"].Equals(true))
                {
                    user_button_d_part_away += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 3"].Equals(true))
                {
                    user_button_d_part_away += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 4"].Equals(true))
                {
                    user_button_d_part_away += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 5"].Equals(true))
                {
                    user_button_d_part_away += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 6"].Equals(true))
                {
                    user_button_d_part_away += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 7"].Equals(true))
                {
                    user_button_d_part_away += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part away 8"].Equals(true))
                {
                    user_button_d_part_away += 0x80;
                }
                #endregion
                #region PARTITIONS STAY
                //BUTTON A
                byte user_button_a_part_stay = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 1"].Equals(true))
                {
                    user_button_a_part_stay += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 2"].Equals(true))
                {
                    user_button_a_part_stay += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 3"].Equals(true))
                {
                    user_button_a_part_stay += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 4"].Equals(true))
                {
                    user_button_a_part_stay += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 5"].Equals(true))
                {
                    user_button_a_part_stay += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 6"].Equals(true))
                {
                    user_button_a_part_stay += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 7"].Equals(true))
                {
                    user_button_a_part_stay += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button A part stay 8"].Equals(true))
                {
                    user_button_a_part_stay += 0x80;
                }

                //BUTTON B
                byte user_button_b_part_stay = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 1"].Equals(true))
                {
                    user_button_b_part_stay += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 2"].Equals(true))
                {
                    user_button_b_part_stay += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 3"].Equals(true))
                {
                    user_button_b_part_stay += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 4"].Equals(true))
                {
                    user_button_b_part_stay += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 5"].Equals(true))
                {
                    user_button_b_part_stay += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 6"].Equals(true))
                {
                    user_button_b_part_stay += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 7"].Equals(true))
                {
                    user_button_b_part_stay += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button B part stay 8"].Equals(true))
                {
                    user_button_b_part_stay += 0x80;
                }

                //BUTTON C
                byte user_button_c_part_stay = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 1"].Equals(true))
                {
                    user_button_c_part_stay += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 2"].Equals(true))
                {
                    user_button_c_part_stay += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 3"].Equals(true))
                {
                    user_button_c_part_stay += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 4"].Equals(true))
                {
                    user_button_c_part_stay += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 5"].Equals(true))
                {
                    user_button_c_part_stay += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 6"].Equals(true))
                {
                    user_button_c_part_stay += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 7"].Equals(true))
                {
                    user_button_c_part_stay += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button C part stay 8"].Equals(true))
                {
                    user_button_c_part_stay += 0x80;
                }

                //BUTTON D
                byte user_button_d_part_stay = 0;
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 1"].Equals(true))
                {
                    user_button_d_part_stay += 0x01;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 2"].Equals(true))
                {
                    user_button_d_part_stay += 0x02;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 3"].Equals(true))
                {
                    user_button_d_part_stay += 0x04;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 4"].Equals(true))
                {
                    user_button_d_part_stay += 0x08;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 5"].Equals(true))
                {
                    user_button_d_part_stay += 0x10;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 6"].Equals(true))
                {
                    user_button_d_part_stay += 0x20;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 7"].Equals(true))
                {
                    user_button_d_part_stay += 0x40;
                }
                if (mainForm.databaseDataSet.User.Rows[(int)user_number]["Button D part stay 8"].Equals(true))
                {
                    user_button_d_part_stay += 0x80;
                }
                #endregion
                #endregion
                byte[] buttons_part_away_bytes = new byte[5];
                buttons_part_away_bytes[0] = user_button_a_part_away;
                buttons_part_away_bytes[1] = user_button_b_part_away;
                buttons_part_away_bytes[2] = user_button_c_part_away;
                buttons_part_away_bytes[3] = user_button_d_part_away;
                byte[] buttons_part_stay_bytes = new byte[5];
                buttons_part_stay_bytes[0] = user_button_a_part_stay;
                buttons_part_stay_bytes[1] = user_button_b_part_stay;
                buttons_part_stay_bytes[2] = user_button_c_part_stay;
                buttons_part_stay_bytes[3] = user_button_d_part_stay;


                #region AUDIO TRACKS
                byte[] audio_tracks = new byte[8];
                if (user_number < 32)
                {
                    audio_tracks[0] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 1"].ToString()) - 1) & 0xFF);
                    audio_tracks[1] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 1"].ToString()) - 1) >> 8);
                    audio_tracks[2] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 2"].ToString()) - 1) & 0xFF);
                    audio_tracks[3] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 2"].ToString()) - 1) >> 8);
                    audio_tracks[4] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 3"].ToString()) - 1) & 0xFF);
                    audio_tracks[5] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 3"].ToString()) - 1) >> 8);
                    audio_tracks[6] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 4"].ToString()) - 1) & 0xFF);
                    audio_tracks[7] = (byte)((int.Parse(mainForm.databaseDataSet.User.Rows[(int)user_number]["Audio track 4"].ToString()) - 1) >> 8);
                }
                #endregion

                #region Output permissions
                long outputs_permissions = 0;
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output1Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x01;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output2Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x02;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output3Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x04;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output4Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x08;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output5Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x10;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output6Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x20;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output7Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x40;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output8Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x80;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output9Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x100;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output10Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x200;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output11Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x400;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output12Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x800;
                }
                if (Convert.ToBoolean(mainForm.databaseDataSet.User.Rows[(int)user_number]["Output13Permissions"]).Equals(true))
                {
                    outputs_permissions += 0x1000;
                }
                #endregion

                byte[] user_code_bytes = new byte[8];

                if (user_code.Length < 4 || user_code.Length > 8)
                {
                    for (int a = 0; a < 8; a++)
                        user_code_bytes[a] = 0xFF;
                }
                else
                {
                    if (user_code.Length == 4)
                    {
                        user_code_bytes[0] = (byte)((int)Char.GetNumericValue(user_code[0]));
                        user_code_bytes[1] = (byte)((int)Char.GetNumericValue(user_code[1]));
                        user_code_bytes[2] = (byte)((int)Char.GetNumericValue(user_code[2]));
                        user_code_bytes[3] = (byte)((int)Char.GetNumericValue(user_code[3]));
                        user_code_bytes[4] = 0xFF;
                        user_code_bytes[5] = 0xFF;
                        user_code_bytes[6] = 0xFF;
                        user_code_bytes[7] = 0xFF;
                    }
                    else if (user_code.Length == 5)
                    {
                        user_code_bytes[0] = (byte)((int)Char.GetNumericValue(user_code[0]));
                        user_code_bytes[1] = (byte)((int)Char.GetNumericValue(user_code[1]));
                        user_code_bytes[2] = (byte)((int)Char.GetNumericValue(user_code[2]));
                        user_code_bytes[3] = (byte)((int)Char.GetNumericValue(user_code[3]));
                        user_code_bytes[4] = (byte)((int)Char.GetNumericValue(user_code[4]));
                        user_code_bytes[5] = 0xFF;
                        user_code_bytes[6] = 0xFF;
                        user_code_bytes[7] = 0xFF;
                    }
                    else if (user_code.Length == 6)
                    {
                        user_code_bytes[0] = (byte)((int)Char.GetNumericValue(user_code[0]));
                        user_code_bytes[1] = (byte)((int)Char.GetNumericValue(user_code[1]));
                        user_code_bytes[2] = (byte)((int)Char.GetNumericValue(user_code[2]));
                        user_code_bytes[3] = (byte)((int)Char.GetNumericValue(user_code[3]));
                        user_code_bytes[4] = (byte)((int)Char.GetNumericValue(user_code[4]));
                        user_code_bytes[5] = (byte)((int)Char.GetNumericValue(user_code[5]));
                        user_code_bytes[6] = 0xFF;
                        user_code_bytes[7] = 0xFF;
                    }
                    else if (user_code.Length == 7)
                    {
                        user_code_bytes[0] = (byte)((int)Char.GetNumericValue(user_code[0]));
                        user_code_bytes[1] = (byte)((int)Char.GetNumericValue(user_code[1]));
                        user_code_bytes[2] = (byte)((int)Char.GetNumericValue(user_code[2]));
                        user_code_bytes[3] = (byte)((int)Char.GetNumericValue(user_code[3]));
                        user_code_bytes[4] = (byte)((int)Char.GetNumericValue(user_code[4]));
                        user_code_bytes[5] = (byte)((int)Char.GetNumericValue(user_code[5]));
                        user_code_bytes[6] = (byte)((int)Char.GetNumericValue(user_code[6]));
                        user_code_bytes[7] = 0xFF;
                    }
                    else if (user_code.Length == 8)
                    {
                        user_code_bytes[0] = (byte)((int)Char.GetNumericValue(user_code[0]));
                        user_code_bytes[1] = (byte)((int)Char.GetNumericValue(user_code[1]));
                        user_code_bytes[2] = (byte)((int)Char.GetNumericValue(user_code[2]));
                        user_code_bytes[3] = (byte)((int)Char.GetNumericValue(user_code[3]));
                        user_code_bytes[4] = (byte)((int)Char.GetNumericValue(user_code[4]));
                        user_code_bytes[5] = (byte)((int)Char.GetNumericValue(user_code[5]));
                        user_code_bytes[6] = (byte)((int)Char.GetNumericValue(user_code[6]));
                        user_code_bytes[7] = (byte)((int)Char.GetNumericValue(user_code[7]));
                    }
                }

                byte[] description_bytes = new byte[64];
                description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);
                byte[] user_options_bytes = BitConverter.GetBytes(user_options);
                byte[] user_type_bytes = BitConverter.GetBytes(user_type);
                byte[] user_group_id_bytes = BitConverter.GetBytes(group_id);
                byte[] user_partitions_bytes = BitConverter.GetBytes(partition_id);
                byte[] user_reserved_bytes = BitConverter.GetBytes(reserved);
                byte[] user_timezones_while_active_bytes = BitConverter.GetBytes(timezones_while_active);
                byte[] user_timezones_while_inactive_bytes = BitConverter.GetBytes(timezones_while_inactive);
                byte[] initial_date_bytes = new byte[4];
                initial_date_bytes[0] = (byte)(initial_date.Day);
                initial_date_bytes[1] = (byte)(initial_date.Month);
                initial_date_bytes[2] = (BitConverter.GetBytes(initial_date.Year))[0];
                initial_date_bytes[3] = (BitConverter.GetBytes(initial_date.Year))[1];
                byte[] final_date_bytes = new byte[4];
                final_date_bytes[0] = (byte)(final_date.Day);
                final_date_bytes[1] = (byte)(final_date.Month);
                final_date_bytes[2] = (BitConverter.GetBytes(final_date.Year))[0];
                final_date_bytes[3] = (BitConverter.GetBytes(final_date.Year))[1];

                byte[] outputs_permissions_bytes = BitConverter.GetBytes(outputs_permissions);

                int i = 0;
                uint j = 0;
                
                byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
                byte_array[i++] = (byte)((user_address >> 16) & 0xFF);
                byte_array[i++] = (byte)((user_address >> 8) & 0xFF);
                byte_array[i++] = (byte)(user_address & 0xFF);

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
                //Options
                for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + user_options_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_options_bytes[j];
                }

                //user code
                for (i = (int)this.attributes["user_code"]["address"] + temp, j = 0; i < ((int)this.attributes["user_code"]["address"] + temp + user_code_bytes.Length); i++, j++)
                {
                    byte_array[i] = 0xFF;
                    byte_array[i] = (byte)(user_code_bytes[j]);

                    //Add 0xFF if code doesn't have maximum length
                    if (i.Equals(((int)this.attributes["user_code"]["address"] + temp + (user_code_bytes.Length - 1))) && user_code_bytes.Length < 8)
                    {
                        int counter = 8 - user_code_bytes.Length;
                        for (int k = counter; k > 0; k--)
                        {
                            byte_array[i + k] = 0xFF;
                        }
                    }
                }

                //user type
                for (i = (temp + (int)this.attributes["user_type"]["address"]), j = 0; i < (temp + (int)this.attributes["user_type"]["address"] + user_type_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_type_bytes[j];
                }
                //group_id
                for (i = (temp + (int)this.attributes["group_id"]["address"]), j = 0; i < (temp + (int)this.attributes["group_id"]["address"] + user_group_id_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_group_id_bytes[j];
                }
                //partition_id
                for (i = (temp + (int)this.attributes["partition_id"]["address"]), j = 0; i < (temp + (int)this.attributes["partition_id"]["address"] + user_partitions_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_partitions_bytes[j];
                }
                //reserved
                for (i = (temp + (int)this.attributes["reserved"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved"]["address"] + user_reserved_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_reserved_bytes[j];
                }
                ////initial_date
                for (i = (temp + (int)this.attributes["initial_date"]["address"]), j = 0; i < (temp + (int)this.attributes["initial_date"]["address"] + initial_date_bytes.Length); i++, j++)
                {
                    byte_array[i] = initial_date_bytes[j];
                }
                //final date
                for (i = (temp + (int)this.attributes["final_date"]["address"]), j = 0; i < (temp + (int)this.attributes["final_date"]["address"] + final_date_bytes.Length); i++, j++)
                {
                    byte_array[i] = final_date_bytes[j];
                }
                //timezones while active
                for (i = (temp + (int)this.attributes["timezones_while_active"]["address"]), j = 0; i < (temp + (int)this.attributes["timezones_while_active"]["address"] + user_timezones_while_active_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_timezones_while_active_bytes[j];
                }
                //timezones while inactive
                for (i = (temp + (int)this.attributes["timezones_while_inactive"]["address"]), j = 0; i < (temp + (int)this.attributes["timezones_while_inactive"]["address"] + user_timezones_while_inactive_bytes.Length); i++, j++)
                {
                    byte_array[i] = user_timezones_while_inactive_bytes[j];
                }

                #region Button config
                //AWAY
                for (i = (temp + (int)this.attributes["particoes_bot_away"]["address"]), j = 0; i < (temp + (int)this.attributes["particoes_bot_away"]["address"] + buttons_part_away_bytes.Length); i++, j++)
                {
                    byte_array[i] = buttons_part_away_bytes[j];
                }
                //STAY
                for (i = (temp + (int)this.attributes["particoes_bot_stay"]["address"]), j = 0; i < (temp + (int)this.attributes["particoes_bot_stay"]["address"] + buttons_part_stay_bytes.Length); i++, j++)
                {
                    byte_array[i] = buttons_part_stay_bytes[j];
                }
                #endregion

                #region Audio tracks
                for (i = ((int)attributes["audio_tracks"]["address"] + temp), j = 0; i < ((int)attributes["audio_tracks"]["address"] + temp + (audio_tracks.Length)); i++, j++)
                {
                    byte_array[i] = audio_tracks[j];
                }
                #endregion

                #region Outputs permissions 
                for (i = ((int)attributes["outputs_permissions"]["address"] + temp), j = 0; i < ((int)attributes["outputs_permissions"]["address"] + temp + (outputs_permissions_bytes.Length)); i++, j++)
                {
                    byte_array[i] = outputs_permissions_bytes[j];
                }
                #endregion

                byte_array[4] = (byte)(i - temp);
                protocol.send_msg_block((uint)i, byte_array, user_address, mainForm.cp_id, mainForm, Constants.KP_FLASH_TAMANHO_DADOS_USERS_FLASH); // TODO: Check if cp_id is needed
                System.Threading.Thread.Sleep(mainForm.intervalsleeptime);
            }
            else
            {
                int k = 0;

                byte_array[k++] = Constants.WRITE_BLOCK_CODE_START;
                byte_array[k++] = (byte)((user_address >> 16) & 0xFF);
                byte_array[k++] = (byte)((user_address >> 8) & 0xFF);
                byte_array[k++] = (byte)(user_address & 0xFF);

                byte_array[k++] = 240;

                int temp = k;

                for (k = temp; k < 240; k++)
                    byte_array[k] = 0xFF;

                byte_array[4] = (byte)(k - temp);
                protocol.send_msg_block((uint)k, byte_array, user_address, mainForm.cp_id, mainForm, Constants.KP_FLASH_TAMANHO_DADOS_USERS_FLASH); // TODO: Check if cp_id is needed
                System.Threading.Thread.Sleep(mainForm.intervalsleeptime);
            }
            
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
