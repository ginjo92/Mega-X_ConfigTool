using MegaXConfigTool.defaultDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool
{
    class Dictionaries
    {
        public static Dictionary<bool, string> GetKeyswitchType()
        {
            Dictionary<bool, string> KeyswitchTypes = new Dictionary<bool, string>();
            KeyswitchTypes.Add(false, Properties.Resources.Keyswitch_type_keyswitch);
            KeyswitchTypes.Add(true, Properties.Resources.Keyswitch_type_toggle);
            return KeyswitchTypes;
        }
        
        public static Dictionary<int, string> GetUserRole()
        {
            Dictionary<int, string> UserRoles = new Dictionary<int, string>();
            UserRoles.Add(0, Properties.Resources.User_role_no_access);
            UserRoles.Add(1, Properties.Resources.User_role_admin_user);
            UserRoles.Add(2, Properties.Resources.User_role_normal_user);
            UserRoles.Add(3, Properties.Resources.User_role_installer);
            UserRoles.Add(4, Properties.Resources.User_role_manufacturer);
            return UserRoles;
        }

        public static Dictionary<int, string> GetCircuitType()
        {
            Dictionary<int, string> CircuitTypes = new Dictionary<int, string>();
            CircuitTypes.Add(0, "R1");
            CircuitTypes.Add(1, "R1 + R2");
            CircuitTypes.Add(2, "R1 + R3");
            CircuitTypes.Add(3, "R1 + R2 + R3");
            return CircuitTypes;
        }

        public static Dictionary<int, string> GetResistorValue()
        {
            Dictionary<int, string> ResistorValues = new Dictionary<int, string>();
            ResistorValues.Add(0, "0R");
            ResistorValues.Add(1, "1K");
            ResistorValues.Add(2, "2K2");
            ResistorValues.Add(3, "3K3");
            ResistorValues.Add(4, "3K9");
            ResistorValues.Add(5, "4K7");
            ResistorValues.Add(6, "5K6");
            ResistorValues.Add(7, "6K8");
            ResistorValues.Add(8, "8K2");
            ResistorValues.Add(9, "10K");
            return ResistorValues;
        }

        public static Dictionary<int, string> GetCircuitFunction()
        {
            Dictionary<int, string> CircuitFunctions = new Dictionary<int, string>();
            CircuitFunctions.Add(16, Properties.Resources.Circuit_function_zone);
            CircuitFunctions.Add(32, Properties.Resources.Circuit_function_mask);
            CircuitFunctions.Add(48, Properties.Resources.Circuit_function_tamper);
            return CircuitFunctions;
        }

        public static Dictionary<int, string> GetCircuitFunctionWithoutZone()
        {
            Dictionary<int, string> CircuitFunctions = new Dictionary<int, string>();
            CircuitFunctions.Add(32, Properties.Resources.Circuit_function_mask);
            CircuitFunctions.Add(48, Properties.Resources.Circuit_function_tamper);
            return CircuitFunctions;
        }

        public static Dictionary<int, string> GetResistorContactType()
        {
            Dictionary<int, string> ResistorContactTypes = new Dictionary<int, string>();
            ResistorContactTypes.Add(1, Properties.Resources.Normally_Open);
            ResistorContactTypes.Add(2, Properties.Resources.Normally_Closed);
            return ResistorContactTypes;
        }

        public static Dictionary<int, string> GetOutputNumber()
        {
            Dictionary<int, string> OutputNumbers = new Dictionary<int, string>();
            OutputNumbers.Add(0, Properties.Resources.Output_number_1);
            OutputNumbers.Add(1, Properties.Resources.Output_number_2);
            OutputNumbers.Add(2, Properties.Resources.Output_number_3);
            OutputNumbers.Add(3, Properties.Resources.Output_number_4);
            OutputNumbers.Add(4, Properties.Resources.Output_number_5);
            OutputNumbers.Add(5, Properties.Resources.Output_number_6);
            OutputNumbers.Add(6, Properties.Resources.Output_number_7);
            OutputNumbers.Add(7, Properties.Resources.Output_number_8);
            OutputNumbers.Add(8, Properties.Resources.Output_number_9);
            OutputNumbers.Add(9, Properties.Resources.Output_number_10);
            OutputNumbers.Add(10, Properties.Resources.Output_number_11);
            OutputNumbers.Add(11, Properties.Resources.Output_number_12);
            OutputNumbers.Add(12, Properties.Resources.Output_number_13);
            OutputNumbers.Add(13, Properties.Resources.Output_number_14);
            OutputNumbers.Add(14, Properties.Resources.Output_number_15);
            OutputNumbers.Add(15, Properties.Resources.Output_number_16);
            OutputNumbers.Add(16, Properties.Resources.Output_number_17);
            OutputNumbers.Add(17, Properties.Resources.Output_number_18);
            OutputNumbers.Add(18, Properties.Resources.Output_number_19);
            OutputNumbers.Add(19, Properties.Resources.Output_number_20);
            OutputNumbers.Add(20, Properties.Resources.Output_number_21);
            OutputNumbers.Add(21, Properties.Resources.Output_number_22);
            OutputNumbers.Add(22, Properties.Resources.Output_number_23);
            OutputNumbers.Add(23, Properties.Resources.Output_number_24);
            OutputNumbers.Add(24, Properties.Resources.Output_number_25);
            OutputNumbers.Add(25, Properties.Resources.Output_number_26);
            OutputNumbers.Add(26, Properties.Resources.Output_number_27);
            OutputNumbers.Add(27, Properties.Resources.Output_number_28);
            OutputNumbers.Add(28, Properties.Resources.Output_number_29);
            OutputNumbers.Add(29, Properties.Resources.Output_number_30);
            OutputNumbers.Add(30, Properties.Resources.Output_number_31);
            OutputNumbers.Add(31, Properties.Resources.Output_number_32);
            OutputNumbers.Add(32, Properties.Resources.Output_number_33);
            OutputNumbers.Add(33, Properties.Resources.Output_number_34);
            OutputNumbers.Add(34, Properties.Resources.Output_number_35);
            OutputNumbers.Add(35, Properties.Resources.Output_number_36);
            OutputNumbers.Add(36, Properties.Resources.Output_number_37);
            OutputNumbers.Add(37, Properties.Resources.Output_number_38);
            OutputNumbers.Add(38, Properties.Resources.Output_number_39);
            OutputNumbers.Add(39, Properties.Resources.Output_number_40);
            OutputNumbers.Add(40, Properties.Resources.Output_number_41);
            OutputNumbers.Add(41, Properties.Resources.Output_number_42);
            OutputNumbers.Add(42, Properties.Resources.Output_number_43);
            OutputNumbers.Add(43, Properties.Resources.Output_number_44);
            OutputNumbers.Add(44, Properties.Resources.Output_number_45);

            OutputNumbers.Add(255, Properties.Resources.Output_number_none);
            return OutputNumbers;
        }

        public static Dictionary<short, string> GetKeypadFunctionButtons()
        {
            Dictionary<short, string> KeypadFunctionButtons = new Dictionary<short, string>();
            KeypadFunctionButtons.Add(0, Properties.Resources.Keypad_function_button_none);
            KeypadFunctionButtons.Add(1, Properties.Resources.Keypad_function_button_config);
            KeypadFunctionButtons.Add(2, Properties.Resources.Keypad_function_button_quick_arm_stay);
            KeypadFunctionButtons.Add(3, Properties.Resources.Keypad_function_button_quick_arm_away);
            KeypadFunctionButtons.Add(4, Properties.Resources.Keypad_function_button_quick_disarm);
            KeypadFunctionButtons.Add(5, Properties.Resources.Keypad_function_button_outputs);
            KeypadFunctionButtons.Add(6, Properties.Resources.Keypad_function_button_zones);
            KeypadFunctionButtons.Add(7, Properties.Resources.Keypad_function_button_wheel_options);
            KeypadFunctionButtons.Add(8, Properties.Resources.Keypad_function_button_events);
            KeypadFunctionButtons.Add(9, Properties.Resources.Keypad_function_button_bypass);
            KeypadFunctionButtons.Add(10, Properties.Resources.Keypad_function_button_alerts);
            return KeypadFunctionButtons;
        }

        public static Dictionary<bool, string> GetKeypadHourType()
        {
            Dictionary<bool, string> KeypadHourTypes = new Dictionary<bool, string>();
            KeypadHourTypes.Add(false, Properties.Resources.Keypad_date_hour_format_24h);
            KeypadHourTypes.Add(true, Properties.Resources.Keypad_date_hour_format_12h);
            return KeypadHourTypes;
        }

        public static Dictionary<bool, string> GetKeypadDateType()
        {
            Dictionary<bool, string> KeypadDateTypes = new Dictionary<bool, string>();
            KeypadDateTypes.Add(false, Properties.Resources.Keypad_date_format_day_month);
            KeypadDateTypes.Add(true, Properties.Resources.Keypad_date_format_month_day);
            return KeypadDateTypes;
        }

        public static Dictionary<int, string> GetTamperType()
        {
            Dictionary<int, string> TamperTypes = new Dictionary<int, string>();
            TamperTypes.Add(0, "NO");
            TamperTypes.Add(1, "NC");
            return TamperTypes;
        }

        public static Dictionary<bool, string> GetConfigPins()
        {
            Dictionary<bool, string> ConfigPins = new Dictionary<bool, string>();
            ConfigPins.Add(false, Properties.Resources.Output);
            ConfigPins.Add(true, Properties.Resources.Zone);
            return ConfigPins;
        }

        public static Dictionary<bool, string> GetVoiceCallType()
        {
            Dictionary<bool, string> VoiceCalltypes = new Dictionary<bool, string>();
            VoiceCalltypes.Add(false, "CID");
            VoiceCalltypes.Add(true, "AUDIO");
            return VoiceCalltypes;
        }

        public static Dictionary<int, string> GetUserLogin()
        {
            Dictionary<int, string> UserLogins = new Dictionary<int, string>();
            UserLogins.Add(0, "User");
            UserLogins.Add(1, "Manufacturer");
            UserLogins.Add(2, "Installer");
            return UserLogins;
        }

        public static Dictionary<short, string> GetPartitions()
        {
            Dictionary<short, string> Partitions = new Dictionary<short, string>();
            Partitions.Add(0, Properties.Resources.All);
            Partitions.Add(1, Properties.Resources.Area_1);
            Partitions.Add(2, Properties.Resources.Area_2);
            Partitions.Add(3, Properties.Resources.Area_3);
            Partitions.Add(4, Properties.Resources.Area_4);
            Partitions.Add(5, Properties.Resources.Area_5);
            Partitions.Add(6, Properties.Resources.Area_6);
            Partitions.Add(7, Properties.Resources.Area_7);
            Partitions.Add(8, Properties.Resources.Area_8);
            
            return Partitions;
        }

        public static Dictionary<short, string> GetEventsTime()
        {
            Dictionary<short, string> Time = new Dictionary<short, string>();
            Time.Add(0, Properties.Resources.All);
            Time.Add(1, Properties.Resources.Today);
            Time.Add(2, Properties.Resources.Last7Days);
            Time.Add(3, Properties.Resources.Last14Days);
            Time.Add(4, Properties.Resources.Last30Days);
            Time.Add(5, Properties.Resources.LastYear);
            return Time;
        }

        public static Dictionary<short, string> GetASCIIHEX()
        {
            Dictionary<short, string> AsciiHex = new Dictionary<short, string>();
            AsciiHex.Add(0,"ASCII");
            AsciiHex.Add(1,"HEX");
            return AsciiHex;
        }

        public static Dictionary<long, string> GetAudioMessages()
        {
            
            defaultDataSet.AudioDataTable a = new defaultDataSet.AudioDataTable();
            AudioTableAdapter databaseDataSetAudioTableAdapter = new AudioTableAdapter();
            databaseDataSetAudioTableAdapter.Fill(a);

            Dictionary<long, string> Audio = new Dictionary<long, string>();

            foreach (defaultDataSet.AudioRow row in a.Rows)
            {
                try
                {
                    Audio.Add(row.Id, row.Description);
                }
                catch { }
            }
            Audio.Add(0xffff, "");
            return Audio;
        }
    }
}
