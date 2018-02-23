using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdigyConfigToolWPF
{
    static class Constants
    {
        //Data length
        public const short HEADER_SIZE = 2;
        public const short LENGTH_SIZE = 1;
        public const short CHECKSUM_SIZE = 4;
        public const short DESTINY_TYPE_SIZE = 1;
        public const short DESTINY_ADDRESS_SIZE = 1;

        //DATA STRUCTURE
        public const byte HEADER_1 = 0xA5;
        public const byte HEADER_2 = 0xC3;


        //Message Types
        public const byte CHECK_ID = 0x10;
        public const byte READ_CODE = 0x20;
        public const byte WRITE_CODE = 0x40;
        public const byte UPDATE_DATE_HOUR_CODE = 0x38;
        public const byte UPDATE_DONE_CODE = 0xB0;

        //Real time messages types
        public const byte RT_Partition_Message= 0xD1;
        public const byte RT_Zone_Message = 0xD2;
        public const byte RT_Output_Message = 0xD3;

        //REAL TIME MONITORING
        public const short ZONA_1 = 4;
        public const short ZONA_2 = 5;
        public const short ZONA_3 = 6;
        public const short ZONA_4 = 7;
        public const short ZONA_5 = 8;
        public const short ZONA_6 = 9;
        public const short ZONA_7 = 10;
        public const short ZONA_8 = 11;
        public const short ZONA_9 = 12;
        public const short ZONA_10 = 13;
        public const short ZONA_11 = 14;
        public const short ZONA_12 = 15;
        public const short ZONA_13 = 16;
        public const short ZONA_14 = 17;
        public const short ZONA_15 = 18;
        public const short ZONA_16 = 19;
        public const short ZONA_17 = 20;
        public const short ZONA_18 = 21;
        public const short ZONA_19 = 22;
        public const short ZONA_20 = 23;
        public const short ZONA_21 = 24;
        public const short ZONA_22 = 25;
        public const short ZONA_23 = 26;
        public const short ZONA_24 = 27;
        public const short ZONA_25 = 28;
        public const short ZONA_26 = 29;
        public const short ZONA_27 = 30;
        public const short ZONA_28 = 31;
        public const short ZONA_29 = 32;
        public const short ZONA_30 = 33;
        public const short ZONA_31 = 34;
        public const short ZONA_32 = 35;

        public const short PARTITION_1 = 36;
        public const short PARTITION_2 = 37;
        public const short PARTITION_3 = 38;
        public const short PARTITION_4 = 39;
        public const short PARTITION_5 = 40;
        public const short PARTITION_6 = 41;
        public const short PARTITION_7 = 42;
        public const short PARTITION_8 = 43;

        public const short OUTPUT_1 = 44;
        public const short OUTPUT_2 = 45;
        public const short OUTPUT_3 = 46;
        public const short OUTPUT_4 = 47;
        public const short OUTPUT_5 = 48;
        public const short OUTPUT_6 = 49;
        public const short OUTPUT_7 = 50;
        public const short OUTPUT_8 = 51;
        public const short OUTPUT_9 = 52;
        public const short OUTPUT_10 = 53;
        public const short OUTPUT_11 = 54;
        public const short OUTPUT_12 = 55;
        public const short OUTPUT_13 = 56;

        public const short PSTN_POS = 57;

        public const short TIMEZONE_1 = 58;
        public const short TIMEZONE_2 = 59;
        public const short TIMEZONE_3 = 60;
        public const short TIMEZONE_4 = 61;
        public const short TIMEZONE_5 = 62;
        public const short TIMEZONE_6 = 63;
        public const short TIMEZONE_7 = 64;
        public const short TIMEZONE_8 = 65;

        public const short MASK_ZONA_ALARM = 0x01;
        public const short MASK_ZONA_OPEN = 0x02;
        public const short MASK_ZONA_BYPASS = 0x04;
        public const short MASK_ZONA_TAMPER = 0x08;
        public const short MASK_ZONA_MASK = 0x10;
        public const short MASK_ZONA_ACTIVE = 0x20;

        public const short MASK_PARTITION_ALARM = 0x01;
        public const short MASK_PARTITION_ARMED_AWAY = 0x02;
        public const short MASK_PARTITION_ARMED_STAY = 0x04;

        public const short MASK_OUTPUT_ALARM = 0x01;
        public const short MASK_OUTPUT_ACTIVE = 0x02;

        public const short MASK_PSTN_CALL_ACTIVE = 0x01;
        public const short MASK_PSTN_LINE_PRESENT = 0x02;

        public const short TIMEZONE_IN_PERIOD = 0x01;

        // MAX NUMBER
        public const short KP_MAX_ZONES = 32;
        public const short KP_MAX_AREAS = 8;
        public const short KP_MAX_KEYPADS = 8;
        public const short KP_MAX_OUTPUTS = 13;
        public const short KP_MAX_USERS = 205;
        public const short KP_MAX_TIMEZONES = 8;
        public const short KP_MAX_PHONES = 16;
        public const short KP_MAX_EXPANDERS = 8;
        public const uint KP_MAX_EVENTS = 5120;
        public const short KP_MAX_AUDIO_SYSTEM_CONFIGURATION = 30;

        //ADDR
        public const int KP_ZONES_INIC_ADDR = 0x005000;
        public const int KP_ZONES_FINAL_ADDR = 0x045000;

        public const int KP_AREAS_INIC_ADDR = 0x3000;
        public const int KP_AREAS_FINAL_ADDR = 0x5000;

        public const int KP_USERS_INIC_ADDR = 0x45000;
        public const int KP_USERS_FINAL_ADDR = 0x65000;

        public const int KP_KEYPADS_INIC_ADDR = 0x1000;
        public const int KP_KEYPADS_FINAL_ADDR = 0x3000;

        public const int KP_OUTPUTS_INIC_ADDR = 0x72000;
        public const int KP_OUTPUTS_FINAL_ADDR = 0x82000;

        public const int KP_TIMEZONES_INIC_ADDR = 0x65000;
        public const int KP_TIMEZONES_FINAL_ADDR = 0x69000;

        public const int KP_PHONES_INIC_ADDR = 0x6A000;
        public const int KP_PHONES_FINAL_ADDR = 0x72000;

        public const int KP_DIALERS_INIC_ADDR = 0x69000;
        public const int KP_DIALERS_FINAL_ADDR = 0x6A000;

        public const int KP_GLOBAL_SYSTEM_INIC_ADDR = 0x00;
        public const int KP_GLOBAL_SYSTEM_FINAL_ADDR = 0x0C00;

        public const uint KP_EVENTS_INIC_ADDR = 0x82800;
        public const uint KP_EVENTS_FINAL_ADDR = 0x1C2800;

        public const uint KP_FLASH_INICIO_ESTRUTURA_AUDIO = 0x200320;
        public const uint KP_FLASH_FIM_FAIXAS_AUDIO = 0x5FFFFF;        

        public const uint KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_INICIO = 0X200000;
        public const uint KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_FIM = 0X200320;



        //SIZE IN BYTES
        public const uint KP_FLASH_TAMANHO_DADOS_PARTICOES_FLASH = 512;
        public const uint KP_FLASH_TAMANHO_DADOS_ZONAS_FLASH = 2048;

        //BYTES NUMBER
        public const uint KP_FLASH_TAMANHO_DADOS_EVENTOS_FLASH = 256; //bytes
        
        public static readonly Dictionary<int, string[]> EventDictionary = new Dictionary<int, string[]>()
        {
            {0, new string[] { "KP_EVT_CODE_MEDICAL", Properties.Resources.EventMessage_Start_KP_EVT_CODE_MEDICAL, Properties.Resources.EventMessage_End_KP_EVT_CODE_MEDICAL } },
            {1, new string[] { "KP_EVT_CODE_PERSONAL_EMERGENCY", "", "" } },
            {2, new string[] { "KP_EVT_CODE_FAIL_TO_REPORT_IN", "", "" } },
            {3, new string[] { "KP_EVT_CODE_FIRE", Properties.Resources.EventMessage_Start_KP_EVT_CODE_FIRE, Properties.Resources.EventMessage_End_KP_EVT_CODE_FIRE } },
            {4, new string[] { "KP_EVT_CODE_SMOKE", "", "" } },
            {5, new string[] { "KP_EVT_CODE_COMBUSTION", "", "" } },
            {6, new string[] { "KP_EVT_CODE_WATER_FLOW", "", "" } },
            {7, new string[] { "KP_EVT_CODE_HEAT", "", "" } },
            {8, new string[] { "KP_EVT_CODE_PULL_STATION", "", "" } },
            {9, new string[] { "KP_EVT_CODE_DUCT", "", "" } },
            {10, new string[] { "KP_EVT_CODE_FLAME", "", "" } },
            {11, new string[] { "KP_EVT_CODE_NEAR_ALARM_FIRE", "", "" } },
            {12, new string[] { "KP_EVT_CODE_PANIC", "", "" } },
            {13, new string[] { "KP_EVT_CODE_DURESS", "", "" } },
            {14, new string[] { "KP_EVT_CODE_SILENT", "", "" } },
            {15, new string[] { "KP_EVT_CODE_AUDIBLE", "", "" } },
            {16, new string[] { "KP_EVT_CODE_DURESS_ACCESS_GRANTED", Properties.Resources.EventMessage_Start_KP_EVT_CODE_DURESS_ACCESS_GRANTED, Properties.Resources.EventMessage_End_KP_EVT_CODE_DURESS_ACCESS_GRANTED } },
            {17, new string[] { "KP_EVT_CODE_DURESS_EGRESS_GRANTED", "", "" } },
            {18, new string[] { "KP_EVT_CODE_HOLDUP_SUSPICION_PRINT", "", "" } },
            {19, new string[] { "KP_EVT_CODE_BURGLARY", Properties.Resources.EventMessage_Start_KP_EVT_CODE_BURGLARY, Properties.Resources.EventMessage_End_KP_EVT_CODE_BURGLARY } },
            {20, new string[] { "KP_EVT_CODE_PERIMETER", "", "" } },
            {21, new string[] { "KP_EVT_CODE_INTERIOR", "", "" } },
            {22, new string[] { "KP_EVT_CODE_24_HOUR_SAFE", Properties.Resources.EventMessage_Start_KP_EVT_CODE_24_HOUR_SAFE, Properties.Resources.EventMessage_End_KP_EVT_CODE_24_HOUR_SAFE } },
            {23, new string[] { "KP_EVT_CODE_ENTRY_EXIT", "", "" } },
            {24, new string[] { "KP_EVT_CODE_DAY_NIGHT", "", "" } },
            {25, new string[] { "KP_EVT_CODE_OUTDOOR", "", "" } },
            {26, new string[] { "KP_EVT_CODE_TAMPER", Properties.Resources.EventMessage_Start_KP_EVT_CODE_TAMPER, Properties.Resources.EventMessage_End_KP_EVT_CODE_TAMPER } },
            {27, new string[] { "KP_EVT_CODE_NEAR_ALARM_WORSENS", Properties.Resources.EventMessage_Start_KP_EVT_CODE_NEAR_ALARM_WORSENS, Properties.Resources.EventMessage_End_KP_EVT_CODE_NEAR_ALARM_WORSENS } },
            {28, new string[] { "KP_EVT_CODE_INTRUSION_VERIFIER", "", "" } },
            {29, new string[] { "KP_EVT_CODE_GENERAL_ALARM", "", "" } },
            {30, new string[] { "KP_EVT_CODE_POLLING_LOOP_OPEN_ARMED", "", "" } },
            {31, new string[] { "KP_EVT_CODE_POLLING_LOOP_SHORT_ARMED", "", "" } },
            {32, new string[] { "KP_EVT_CODE_EXPANSION_MODULE_FAILURE_ARMED", "", "" } },
            {33, new string[] { "KP_EVT_CODE_SENSOR_TAMPER", "", "" } },
            {34, new string[] { "KP_EVT_CODE_EXPANSION_MODULE_TAMPER", Properties.Resources.EventMessage_Start_KP_EVT_CODE_EXPANSION_MODULE_TAMPER, Properties.Resources.EventMessage_End_KP_EVT_CODE_EXPANSION_MODULE_TAMPER } },
            {35, new string[] { "KP_EVT_CODE_SILENT_BURGLARY", "", "" } },
            {36, new string[] { "KP_EVT_CODE_SENSOR_SUPERVISION_FAILURE", "", "" } },
            {37, new string[] { "KP_EVT_CODE_24_HOUR_NON_BURGLARY", "", "" } },
            {38, new string[] { "KP_EVT_CODE_GAS_DETECTED", "", "" } },
            {39, new string[] { "KP_EVT_CODE_REFRIGERATION", "", "" } },
            {40, new string[] { "KP_EVT_CODE_LOSS_OF_HEAT", "", "" } },
            {41, new string[] { "KP_EVT_CODE_WATER_LEAKAGE", "", "" } },
            {42, new string[] { "KP_EVT_CODE_FOIL_BREAK", "", "" } },
            {43, new string[] { "KP_EVT_CODE_DAY_TROUBLE", "", "" } },
            {44, new string[] { "KP_EVT_CODE_LOW_BOTTLED_GAS_LEVEL", "", "" } },
            {45, new string[] { "KP_EVT_CODE_HIGH_TEMP", "", "" } },
            {46, new string[] { "KP_EVT_CODE_LOW_TEMP", "", "" } },
            {47, new string[] { "KP_EVT_CODE_LOSS_OF_AIR_FLOW", "", "" } },
            {48, new string[] { "KP_EVT_CODE_CARBON_MONOXIDE_DETECTED", "", "" } },
            {49, new string[] { "KP_EVT_CODE_TANK_LEVEL", "", "" } },
            {50, new string[] { "KP_EVT_CODE_HIGH_HUMIDITY", "", "" } },
            {51, new string[] { "KP_EVT_CODE_LOW_HUMIDITY", "", "" } },
            {52, new string[] { "KP_EVT_CODE_FIRE_SUPERVISORY", "", "" } },
            {53, new string[] { "KP_EVT_CODE_LOW_WATER_PRESSURE", "", "" } },
            {54, new string[] { "KP_EVT_CODE_LOW_CO2", "", "" } },
            {55, new string[] { "KP_EVT_CODE_GATE_VALVE_SENSOR", "", "" } },
            {56, new string[] { "KP_EVT_CODE_LOW_WATER_LEVEL", "", "" } },
            {57, new string[] { "KP_EVT_CODE_PUMP_ACTIVATED", "", "" } },
            {58, new string[] { "KP_EVT_CODE_PUMP_FAILURE", "", "" } },
            {59, new string[] { "KP_EVT_CODE_SYSTEM_TROUBLE", "", "" } },
            {60, new string[] { "KP_EVT_CODE_AC_LOSS", Properties.Resources.EventMessage_Start_KP_EVT_CODE_AC_LOSS, Properties.Resources.EventMessage_End_KP_EVT_CODE_AC_LOSS } },
            {61, new string[] { "KP_EVT_CODE_LOW_SYSTEM_BATTERY", Properties.Resources.EventMessage_Start_KP_EVT_CODE_LOW_SYSTEM_BATTERY, Properties.Resources.EventMessage_End_KP_EVT_CODE_LOW_SYSTEM_BATTERY } },
            {62, new string[] { "KP_EVT_CODE_RAM_CHECKSUM_BAD", "", "" } },
            {63, new string[] { "KP_EVT_CODE_ROM_CHECKSUM_BAD", "", "" } },
            {64, new string[] { "KP_EVT_CODE_SYSTEM_RESET", Properties.Resources.EventMessage_Start_KP_EVT_CODE_SYSTEM_RESET, Properties.Resources.EventMessage_End_KP_EVT_CODE_SYSTEM_RESET } },
            {65, new string[] { "KP_EVT_CODE_PANEL_PROGRAMMING_CHANGED", "", "" } },
            {66, new string[] { "KP_EVT_CODE_SELF_TEST_FAILURE", "", "" } },
            {67, new string[] { "KP_EVT_CODE_SYSTEM_SHUTDOWN", "", "" } },
            {68, new string[] { "KP_EVT_CODE_BATTERY_TEST_FAILURE", "", "" } },
            {69, new string[] { "KP_EVT_CODE_GROUND_FAULT", "", "" } },
            {70, new string[] { "KP_EVT_CODE_BATTERY_MISSING_DEAD", Properties.Resources.EventMessage_Start_KP_EVT_CODE_BATTERY_MISSING_DEAD, Properties.Resources.EventMessage_End_KP_EVT_CODE_BATTERY_MISSING_DEAD } },
            {71, new string[] { "KP_EVT_CODE_POWER_SUPPLY_OVERCURRENT", "", "" } },
            {72, new string[] { "KP_EVT_CODE_ENGINEER_RESET", "", "" } },
            {73, new string[] { "KP_EVT_CODE_PRIMARY_POWER_SUPPLY_FAILURE", "", "" } },
            {74, new string[] { "KP_EVT_CODE_SYSTEM_TAMPER", Properties.Resources.EventMessage_Start_KP_EVT_CODE_SYSTEM_TAMPER, Properties.Resources.EventMessage_End_KP_EVT_CODE_SYSTEM_TAMPER } },
            {75, new string[] { "KP_EVT_CODE_SOUNDER_RELAY_", "", "" } },
            {76, new string[] { "KP_EVT_CODE_BELL_1_", "", "" } },
            {77, new string[] { "KP_EVT_CODE_BELL_2_", "", "" } },
            {78, new string[] { "KP_EVT_CODE_ALARM_RELAY_", "", "" } },
            {79, new string[] { "KP_EVT_CODE_TROUBLE_RELAY", "", "" } },
            {80, new string[] { "KP_EVT_CODE_REVERSING_RELAY_", "", "" } },
            {81, new string[] { "KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_3", "", "" } },
            {82, new string[] { "KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_4", "", "" } },
            {83, new string[] { "KP_EVT_CODE_SYSTEM_PERIPHERAL_TROUBLE", "", "" } },
            {84, new string[] { "KP_EVT_CODE_POLLING_LOOP_OPEN_DISARMED", "", "" } },
            {85, new string[] { "KP_EVT_CODE_POLLING_LOOP_SHORT_DISARMED", "", "" } },
            {86, new string[] { "KP_EVT_CODE_EXPANSION_MODULE_FAILURE", "", "" } },
            {87, new string[] { "KP_EVT_CODE_REPEATER_FAILURE", "", "" } },
            {88, new string[] { "KP_EVT_CODE_LOCAL_PRINTER_OUT_OF_PAPER", "", "" } },
            {89, new string[] { "KP_EVT_CODE_LOCAL_PRINTER_FAILURE", "", "" } },
            {90, new string[] { "KP_EVT_CODE_EXP_MODULE_DC_LOSS", "", "" } },
            {91, new string[] { "KP_EVT_CODE_EXP_MODULE_LOW_BATT", "", "" } },
            {92, new string[] { "KP_EVT_CODE_EXP_MODULE_RESET", "", "" } },
            {93, new string[] { "KP_EVT_CODE_EXP_MODULE_TAMPER_", "", "" } },
            {94, new string[] { "KP_EVT_CODE_EXP_MODULE_AC_LOSS", "", "" } },
            {95, new string[] { "KP_EVT_CODE_EXP_MODULE_SELF_TEST_FAIL", "", "" } },
            {96, new string[] { "KP_EVT_CODE_RF_RECEIVER_JAM_DETECT", "", "" } },
            {97, new string[] { "KP_EVT_CODE_AES_ENCRYPTION_DIS_ENABLED", "", "" } },
            {98, new string[] { "KP_EVT_CODE_COMMUNICATION_TROUBLE", "", "" } },
            {99, new string[] { "KP_EVT_CODE_TELCO_1_FAULT", Properties.Resources.EventMessage_Start_KP_EVT_CODE_TELCO_1_FAULT, Properties.Resources.EventMessage_End_KP_EVT_CODE_TELCO_1_FAULT } },
            {100, new string[] { "KP_EVT_CODE_TELCO_2_FAULT", "", "" } },
            {101, new string[] { "KP_EVT_CODE_LONG_RANGE_RADIO_XMITTER_FAULT", "", "" } },
            {102, new string[] { "KP_EVT_CODE_FAILURE_TO_COMMUNICATE_EVENT", Properties.Resources.EventMessage_Start_KP_EVT_CODE_FAILURE_TO_COMMUNICATE_EVENT, Properties.Resources.EventMessage_End_KP_EVT_CODE_FAILURE_TO_COMMUNICATE_EVENT } },
            {103, new string[] { "KP_EVT_CODE_LOSS_OF_RADIO_SUPERVISION", "", "" } },
            {104, new string[] { "KP_EVT_CODE_LOSS_OF_CENTRAL_POLLING", "", "" } },
            {105, new string[] { "KP_EVT_CODE_LONG_RANGE_RADIO_VSWR_PROBLEM", "", "" } },
            {106, new string[] { "KP_EVT_CODE_PERIODIC_COMM_TEST_FAIL_RESTORE", "", "" } },
            {107, new string[] { "KP_EVT_CODE_PROTECTION_LOOP", "", "" } },
            {108, new string[] { "KP_EVT_CODE_PROTECTION_LOOP_OPEN", "", "" } },
            {109, new string[] { "KP_EVT_CODE_PROTECTION_LOOP_SHORT", "", "" } },
            {110, new string[] { "KP_EVT_CODE_FIRE_TROUBLE", "", "" } },
            {111, new string[] { "KP_EVT_CODE_EXIT_ERROR_ALARM_ZONE", "", "" } },
            {112, new string[] { "KP_EVT_CODE_PANIC_ZONE_TROUBLE", Properties.Resources.EventMessage_Start_KP_EVT_CODE_PANIC_ZONE_TROUBLE, Properties.Resources.EventMessage_End_KP_EVT_CODE_PANIC_ZONE_TROUBLE } },
            {113, new string[] { "KP_EVT_CODE_HOLD_UP", "", "" } },
            {114, new string[] { "KP_EVT_CODE_SWINGER_TROUBLE", "", "" } },
            {115, new string[] { "KP_EVT_CODE_CROSS", "", "" } },
            {116, new string[] { "KP_EVT_CODE_SENSOR_TROUBLE", "", "" } },
            {117, new string[] { "KP_EVT_CODE_LOSS_OF_SUPERVISION_RF", "", "" } },
            {118, new string[] { "KP_EVT_CODE_LOSS_OF_SUPERVISION_RPM", "", "" } },
            {119, new string[] { "KP_EVT_CODE_SENSOR_TAMPER_FAULT", "", "" } },
            {120, new string[] { "KP_EVT_CODE_RF_LOW_BATTERY", "", "" } },
            {121, new string[] { "KP_EVT_CODE_SMOKE_DETECTOR_HI_SENSITIVITY", "", "" } },
            {122, new string[] { "KP_EVT_CODE_SMOKE_DETECTOR_LOW_SENSITIVITY", "", "" } },
            {123, new string[] { "KP_EVT_CODE_INTRUSION_DETECTOR_HI_SENSITIVITY", "", "" } },
            {124, new string[] { "KP_EVT_CODE_INTRUSION_DETECTOR_LOW_SENSITIVITY", "", "" } },
            {125, new string[] { "KP_EVT_CODE_SENSOR_SELF_TEST_FAILURE", "", "" } },
            {126, new string[] { "KP_EVT_CODE_SENSOR_WATCH_TROUBLE", Properties.Resources.EventMessage_Start_KP_EVT_CODE_SENSOR_WATCH_TROUBLE, Properties.Resources.EventMessage_End_KP_EVT_CODE_SENSOR_WATCH_TROUBLE } },
            {127, new string[] { "KP_EVT_CODE_DRIFT_COMPENSATION_ERROR", "", "" } },
            {128, new string[] { "KP_EVT_CODE_MAINTENANCE_ALERT", "", "" } },
            {129, new string[] { "KP_EVT_CODE_CO_DETECTOR_NEEDS_REPLACEMENT", "", "" } },
            {130, new string[] { "KP_EVT_CODE_OPEN_CLOSE", "", "" } },
            {131, new string[] { "KP_EVT_CODE_OC_BY_USER", Properties.Resources.EventMessage_Start_KP_EVT_CODE_OC_BY_USER, Properties.Resources.EventMessage_End_KP_EVT_CODE_OC_BY_USER } },
            {132, new string[] { "KP_EVT_CODE_GROUP_OC_GROUPA_GROUP_OF", "", "" } },
            {133, new string[] { "KP_EVT_CODE_AUTOMATIC_OC", "", "" } },
            {134, new string[] { "KP_EVT_CODE_LATE_TO_OC", "", "" } },
            {135, new string[] { "KP_EVT_CODE_DEFERRED_OC", "", "" } },
            {136, new string[] { "KP_EVT_CODE_CANCEL", "", "" } },
            {137, new string[] { "KP_EVT_CODE_REMOTE_ARM_DISARM", "", "" } },
            {138, new string[] { "KP_EVT_CODE_QUICK_ARM", Properties.Resources.EventMessage_Start_KP_EVT_CODE_QUICK_ARM, Properties.Resources.EventMessage_End_KP_EVT_CODE_QUICK_ARM } },
            {139, new string[] { "KP_EVT_CODE_KEYSWITCH_OC", "", "" } },
            {140, new string[] { "KP_EVT_CODE_ARMED_STAY", Properties.Resources.EventMessage_Start_KP_EVT_CODE_ARMED_STAY, Properties.Resources.EventMessage_End_KP_EVT_CODE_ARMED_STAY } },
            {141, new string[] { "KP_EVT_CODE_KEYSWITCH_ARMED_STAY", "", "" } },
            {142, new string[] { "KP_EVT_CODE_ARMED_WITH_SYSTEM_TROUBLE_OVERRIDE", "", "" } },
            {143, new string[] { "KP_EVT_CODE_EXCEPTION_OC_", "", "" } },
            {144, new string[] { "KP_EVT_CODE_EARLY_OC", "", "" } },
            {145, new string[] { "KP_EVT_CODE_LATE_OC", "", "" } },
            {146, new string[] { "KP_EVT_CODE_FAILED_TO_OPEN", "", "" } },
            {147, new string[] { "KP_EVT_CODE_FAILED_TO_CLOSE", Properties.Resources.EventMessage_Start_KP_EVT_CODE_FAILED_TO_CLOSE, Properties.Resources.EventMessage_End_KP_EVT_CODE_FAILED_TO_CLOSE } },
            {148, new string[] { "KP_EVT_CODE_AUTO_ARM_FAILED", Properties.Resources.EventMessage_Start_KP_EVT_CODE_AUTO_ARM_FAILED, Properties.Resources.EventMessage_End_KP_EVT_CODE_AUTO_ARM_FAILED } },
            {149, new string[] { "KP_EVT_CODE_PARTIAL_ARM", "", "" } },
            {150, new string[] { "KP_EVT_CODE_EXIT_ERROR", "", "" } },     
            {151, new string[] { "KP_EVT_CODE_USER_ON_PREMISES", "", "" } },
            {152, new string[] { "KP_EVT_CODE_RECENT_CLOSE", "", "" } },
            {153, new string[] { "KP_EVT_CODE_WRONG_CODE_ENTRY", Properties.Resources.EventMessage_Start_KP_EVT_CODE_WRONG_CODE_ENTRY, Properties.Resources.EventMessage_End_KP_EVT_CODE_WRONG_CODE_ENTRY } },
            {154, new string[] { "KP_EVT_CODE_LEGAL_CODE_ENTRY", "", "" } },
            {155, new string[] { "KP_EVT_CODE_RE_ARM_AFTER_ALARM", "", "" } },
            {156, new string[] { "KP_EVT_CODE_AUTO_ARM_TIME_EXTENDED", "", "" } },
            {157, new string[] { "KP_EVT_CODE_PANIC_ALARM_RESET", "", "" } },
            {158, new string[] { "KP_EVT_CODE_SERVICE_ON_OFF_PREMISES", "", "" } },
            {159, new string[] { "KP_EVT_CODE_CALLBACK_REQUEST_MADE", "", "" } },
            {160, new string[] { "KP_EVT_CODE_SUCCESSFUL_DOWNLOAD_ACCESS", "", "" } },
            {161, new string[] { "KP_EVT_CODE_UNSUCCESSFUL_ACCESS", "", "" } },
            {162, new string[] { "KP_EVT_CODE_SYSTEM_SHUTDOWN_COMMAND_RECEIVED", Properties.Resources.EventMessage_Start_KP_EVT_CODE_SYSTEM_SHUTDOWN_COMMAND_RECEIVED, Properties.Resources.EventMessage_End_KP_EVT_CODE_SYSTEM_SHUTDOWN_COMMAND_RECEIVED } },
            {163, new string[] { "KP_EVT_CODE_DIALER_SHUTDOWN_COMMAND_RECEIVED", "", "" } },
            {164, new string[] { "KP_EVT_CODE_SUCCESSFUL_UPLOAD", "", "" } },
            {165, new string[] { "KP_EVT_CODE_ACCESS_DENIED", "", "" } },
            {166, new string[] { "KP_EVT_CODE_ACCESS_REPORT_BY_USER", "", "" } },
            {167, new string[] { "KP_EVT_CODE_FORCED_ACCESS_", "", "" } },
            {168, new string[] { "KP_EVT_CODE_EGRESS_DENIED", "", "" } },
            {169, new string[] { "KP_EVT_CODE_EGRESS_GRANTED", "", "" } },
            {170, new string[] { "KP_EVT_CODE_ACCESS_DOOR_PROPPED_OPEN", "", "" } },
            {171, new string[] { "KP_EVT_CODE_ACCESS_POINT_DOOR_STATUS_MONITOR_TROUBLE", "", "" } },
            {172, new string[] { "KP_EVT_CODE_ACCESS_POINT_REQUEST_TO_EXIT_TROUBLE", "", "" } },
            {173, new string[] { "KP_EVT_CODE_ACCESS_PROGRAM_MODE_ENTRY", Properties.Resources.EventMessage_Start_KP_EVT_CODE_ACCESS_PROGRAM_MODE_ENTRY, Properties.Resources.EventMessage_End_KP_EVT_CODE_ACCESS_PROGRAM_MODE_ENTRY } },
            {174, new string[] { "KP_EVT_CODE_ACCESS_PROGRAM_MODE_EXIT", Properties.Resources.EventMessage_Start_KP_EVT_CODE_ACCESS_PROGRAM_MODE_EXIT, Properties.Resources.EventMessage_End_KP_EVT_CODE_ACCESS_PROGRAM_MODE_EXIT } },
            {175, new string[] { "KP_EVT_CODE_ACCESS_THREAT_LEVEL_CHANGE", "", "" } },
            {176, new string[] { "KP_EVT_CODE_ACCESS_RELAY_TRIGGER_FAIL", "", "" } },
            {177, new string[] { "KP_EVT_CODE_ACCESS_RTE_SHUNT", "", "" } },
            {178, new string[] { "KP_EVT_CODE_ACCESS_DSM_SHUNT", "", "" } },
            {179, new string[] { "KP_EVT_CODE_SECOND_PERSON_ACCESS", "", "" } }, 
            {180, new string[] { "KP_EVT_CODE_IRREGULAR_ACCESS", "", "" } },
            {181, new string[] { "KP_EVT_CODE_ACCESS_READER_DISABLE", "", "" } },
            {182, new string[] { "KP_EVT_CODE_SOUNDER_RELAY_DISABLE", Properties.Resources.EventMessage_Start_KP_EVT_CODE_SOUNDER_RELAY_DISABLE, Properties.Resources.EventMessage_End_KP_EVT_CODE_SOUNDER_RELAY_DISABLE } },
            {183, new string[] { "KP_EVT_CODE_BELL_1_DISABLE", "", "" } },
            {184, new string[] { "KP_EVT_CODE_BELL_2_DISABLE", "", "" } },
            {185, new string[] { "KP_EVT_CODE_ALARM_RELAY_DISABLE", "", "" } },
            {186, new string[] { "KP_EVT_CODE_TROUBLE_RELAY_DISABLE", "", "" } },
            {187, new string[] { "KP_EVT_CODE_REVERSING_RELAY_DISABLE", "", "" } },
            {188, new string[] { "KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_3_DISABLE", "", "" } },
            {189, new string[] { "KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_4_DISABLE", "", "" } },
            {190, new string[] { "KP_EVT_CODE_MODULE_ADDED", "", "" } },
            {191, new string[] { "KP_EVT_CODE_MODULE_REMOVED", "", "" } },
            {192, new string[] { "KP_EVT_CODE_DIALER_DISABLED", "", "" } },
            {193, new string[] { "KP_EVT_CODE_RADIO_TRANSMITTER_DISABLED", "", "" } },
            {194, new string[] { "KP_EVT_CODE_REMOTE_UPDOWN_LOAD_DISABLE", "", "" } },
            {195, new string[] { "KP_EVT_CODE_ZONE_SENSOR_BYPASS", Properties.Resources.EventMessage_Start_KP_EVT_CODE_ZONE_SENSOR_BYPASS, Properties.Resources.EventMessage_End_KP_EVT_CODE_ZONE_SENSOR_BYPASS } },
            {196, new string[] { "KP_EVT_CODE_FIRE_BYPASS", "", "" } },
            {197, new string[] { "KP_EVT_CODE_24_HOUR", "", "" } },
            {198, new string[] { "KP_EVT_CODE_BURG_BYPASS", "", "" } },
            {199, new string[] { "KP_EVT_CODE_GROUP_BYPASS_USER_A_GROUP_OF", "", "" } },
            {200, new string[] { "KP_EVT_CODE_SWINGER_BYPASS", "", "" } },
            {201, new string[] { "KP_EVT_CODE_ACCESS", "", "" } },
            {202, new string[] { "KP_EVT_CODE_ACCESS_POINT_BYPASS", "", "" } },
            {203, new string[] { "KP_EVT_CODE_VAULT_BYPASS", "", "" } },
            {204, new string[] { "KP_EVT_CODE_VENT", "", "" } },
            {205, new string[] { "KP_EVT_CODE_MANUAL_TRIGGER_TEST_REPORT", "", "" } },
            {206, new string[] { "KP_EVT_CODE_PERIODIC_TEST_REPORT", Properties.Resources.EventMessage_Start_KP_EVT_CODE_PERIODIC_TEST_REPORT, Properties.Resources.EventMessage_End_KP_EVT_CODE_PERIODIC_TEST_REPORT } },
            {207, new string[] { "KP_EVT_CODE_PERIODIC_RF_TRANSMISSION", "", "" } },
            {208, new string[] { "KP_EVT_CODE_FIRE_TEST", "", "" } },
            {209, new string[] { "KP_EVT_CODE_STATUS_REPORT_TO_FOLLOW", "", "" } },
            {210, new string[] { "KP_EVT_CODE_LISTEN_IN_TO_FOLLOW", "", "" } },
            {211, new string[] { "KP_EVT_CODE_WALK_TEST_MODE_", "", "" } },
            {212, new string[] { "KP_EVT_CODE_PERIODIC_TEST_SYSTEM_TROUBLE_PRESENT", "", "" } },
            {213, new string[] { "KP_EVT_CODE_VIDEO_XMITTER_ACTIVE", "", "" } },
            {214, new string[] { "KP_EVT_CODE_POINT_TESTED_OK", "", "" } },
            {215, new string[] { "KP_EVT_CODE_POINT_NOT_TESTED", "", "" } },
            {216, new string[] { "KP_EVT_CODE_INTRUSION", "", "" } },
            {217, new string[] { "KP_EVT_CODE_FIRE_ZONE_WALK_TESTED", "", "" } },
            {218, new string[] { "KP_EVT_CODE_PANIC_ZONE_WALK_TESTED", "", "" } },
            {219, new string[] { "KP_EVT_CODE_SERVICE_REQUEST", "", "" } },
            {220, new string[] { "KP_EVT_CODE_EVENT_LOG_RESET", "", "" } },
            {221, new string[] { "KP_EVT_CODE_EVENT_LOG_50_FULL", "", "" } },
            {222, new string[] { "KP_EVT_CODE_EVENT_LOG_90_FULL", "", "" } },
            {223, new string[] { "KP_EVT_CODE_EVENT_LOG_OVERFLOW", "", "" } },
            {224, new string[] { "KP_EVT_CODE_TIMEDATE_RESET", "", "" } },
            {225, new string[] { "KP_EVT_CODE_TIMEDATE_INACCURATE", "", "" } },
            {226, new string[] { "KP_EVT_CODE_PROGRAM_MODE_ENTRY", "", "" } },
            {227, new string[] { "KP_EVT_CODE_PROGRAM_MODE_EXIT", "", "" } },
            {228, new string[] { "KP_EVT_CODE_32_HOUR_EVENT_LOG_MARKER", "", "" } },
            {229, new string[] { "KP_EVT_CODE_SCHEDULE_CHANGE", "", "" } },
            {230, new string[] { "KP_EVT_CODE_EXCEPTION_SCHEDULE_CHANGE", "", "" } },
            {231, new string[] { "KP_EVT_CODE_ACCESS_SCHEDULE_CHANGE", "", "" } },
            {232, new string[] { "KP_EVT_CODE_SENIOR_WATCH_TROUBLE", "", "" } },
            {233, new string[] { "KP_EVT_CODE_LATCH_KEY_SUPERVISION", "", "" } },
            {234, new string[] { "KP_EVT_CODE_SYSTEM_INACTIVITY", "", "" } },
            {235, new string[] { "KP_EVT_CODE_USER_CODE_X_MODIFIED_BY_INSTALLER", "", "" } },
            {236, new string[] { "KP_EVT_CODE_AUXILIARY_3", "", "" } },
            {237, new string[] { "KP_EVT_CODE_INSTALLER_TEST", "", "" } },
            {238, new string[] { "KP_EVT_CODE_USER_ASSIGNED_1", "", "" } },
            {239, new string[] { "KP_EVT_CODE_USER_ASSIGNED_2", "", "" } },
            {240, new string[] { "KP_EVT_CODE_USER_ASSIGNED_3", "", "" } },
            {241, new string[] { "KP_EVT_CODE_USER_ASSIGNED_4", "", "" } },
            {242, new string[] { "KP_EVT_CODE_USER_ASSIGNED_5", "", "" } },
            {243, new string[] { "KP_EVT_CODE_USER_ASSIGNED_6", "", "" } },
            {244, new string[] { "KP_EVT_CODE_USER_ASSIGNED_7", "", "" } },
            {245, new string[] { "KP_EVT_CODE_USER_ASSIGNED_8", "", "" } },
            {246, new string[] { "KP_EVT_CODE_USER_ASSIGNED_9", "", "" } },
            {247, new string[] { "KP_EVT_CODE_USER_ASSIGNED_10", "", "" } },
            {248, new string[] { "KP_EVT_CODE_USER_ASSIGNED_11", "", "" } },
            {249, new string[] { "KP_EVT_CODE_USER_ASSIGNED_12", "", "" } },
            {250, new string[] { "KP_EVT_CODE_USER_ASSIGNED_13", "", "" } },
            {251, new string[] { "KP_EVT_CODE_USER_ASSIGNED_14", "", "" } },
            {252, new string[] { "KP_EVT_CODE_USER_ASSIGNED_15", "", "" } },
            {253, new string[] { "KP_EVT_CODE_USER_ASSIGNED_16", "", "" } },
            {254, new string[] { "KP_EVT_CODE_USER_ASSIGNED_17", "", "" } },
            {255, new string[] { "KP_EVT_CODE_USER_ASSIGNED_18", "", "" } },
            {256, new string[] { "KP_EVT_CODE_USER_ASSIGNED_19", "", "" } },
            {257, new string[] { "KP_EVT_CODE_USER_ASSIGNED_20", "", "" } },
            {258, new string[] { "KP_EVT_CODE_USER_ASSIGNED_21", "", "" } },
            {259, new string[] { "KP_EVT_CODE_USER_ASSIGNED_22", "", "" } },
            {260, new string[] { "KP_EVT_CODE_USER_ASSIGNED_23", "", "" } },
            {261, new string[] { "KP_EVT_CODE_USER_ASSIGNED_24", "", "" } },
            {262, new string[] { "KP_EVT_CODE_USER_ASSIGNED_25", "", "" } },
            {263, new string[] { "KP_EVT_CODE_USER_ASSIGNED_26", "", "" } },
            {264, new string[] { "KP_EVT_CODE_USER_ASSIGNED_27", "", "" } },
            {265, new string[] { "KP_EVT_CODE_USER_ASSIGNED_28", "", "" } },
            {266, new string[] { "KP_EVT_CODE_USER_ASSIGNED_29", "", "" } },
            {267, new string[] { "KP_EVT_CODE_USER_ASSIGNED_30", "", "" } },
            {268, new string[] { "KP_EVT_CODE_USER_ASSIGNED_31", "", "" } },
            {269, new string[] { "KP_EVT_CODE_USER_ASSIGNED_32", "", "" } },
            {270, new string[] { "KP_EVT_CODE_USER_ASSIGNED_33", "", "" } },
            {271, new string[] { "KP_EVT_CODE_USER_ASSIGNED_34", "", "" } },
            {272, new string[] { "KP_EVT_CODE_USER_ASSIGNED_35", "", "" } },
            {273, new string[] { "KP_EVT_CODE_USER_ASSIGNED_36", "", "" } },
            {274, new string[] { "KP_EVT_CODE_USER_ASSIGNED_37", "", "" } },
            {275, new string[] { "KP_EVT_CODE_USER_ASSIGNED_38", "", "" } },
            {276, new string[] { "KP_EVT_CODE_USER_ASSIGNED_39", "", "" } },
            {277, new string[] { "KP_EVT_CODE_USER_ASSIGNED_40", "", "" } },
            {278, new string[] { "KP_EVT_CODE_UNABLE_TO_OUTPUT_SIGNAL_DERIVED_CHANNEL", "", "" } },
            {279, new string[] { "KP_EVT_CODE_STU_CONTROLLER_DOWN_DERIVED_CHANNEL", "", "" } },
            {280, new string[] { "KP_EVT_CODE_DOWNLOAD_ABORT", "", "" } },
            {281, new string[] { "KP_EVT_CODE_DOWNLOAD_START_END", "", "" } },
            {282, new string[] { "KP_EVT_CODE_DOWNLOAD_INTERRUPTED", "", "" } },
            {283, new string[] { "KP_EVT_CODE_AUTO_CLOSE_WITH_BYPASS", "", "" } },
            {284, new string[] { "KP_EVT_CODE_BYPASS_CLOSING", "", "" } },
            {285, new string[] { "KP_EVT_CODE_FIRE_ALARM_SILENCE_", "", "" } },
            {286, new string[] { "KP_EVT_CODE_SUPERVISORY_POINT_TEST_START_END", "", "" } },
            {287, new string[] { "KP_EVT_CODE_HOLD_UP_TEST_START_END", "", "" } },
            {288, new string[] { "KP_EVT_CODE_BURG_TEST_PRINT_START_END", "", "" } },
            {289, new string[] { "KP_EVT_CODE_SUPERVISORY_TEST_PRINT_START_END", "", "" } },
            {290, new string[] { "KP_EVT_CODE_BURG_DIAGNOSTICS_START_END", "", "" } },
            {291, new string[] { "KP_EVT_CODE_FIRE_DIAGNOSTICS_START_END", "", "" } },
            {292, new string[] { "KP_EVT_CODE_UNTYPED_DIAGNOSTICS", "", "" } },
            {293, new string[] { "KP_EVT_CODE_TROUBLE_CLOSING", "", "" } },
            {294, new string[] { "KP_EVT_CODE_ACCESS_DENIED_CODE_UNKNOWN", "", "" } },
            {295, new string[] { "KP_EVT_CODE_SUPERVISORY_POINT_ALARM", "", "" } },
            {296, new string[] { "KP_EVT_CODE_SUPERVISORY_POINT_BYPASS", "", "" } },
            {297, new string[] { "KP_EVT_CODE_SUPERVISORY_POINT_TROUBLE", "", "" } },
            {298, new string[] { "KP_EVT_CODE_HOLD_UP_POINT_BYPASS", "", "" } },
            {299, new string[] { "KP_EVT_CODE_AC_FAILURE_FOR_4_HOURS", "", "" } },
            {300, new string[] { "KP_EVT_CODE_OUTPUT_TROUBLE", "", "" } },
            {301, new string[] { "KP_EVT_CODE_USER_CODE_FOR_EVENT", "", "" } },
            {302, new string[] { "KP_EVT_CODE_LOG_OFF", "", "" } },
            {303, new string[] { "KP_EVT_CODE_CS_CONNECTION_FAILURE", "", "" } },
            {304, new string[] { "KP_EVT_CODE_RCVR_DATABASE_CONNECTION_FAIL_RESTORE", "", "" } },
            {305, new string[] { "KP_EVT_CODE_LICENSE_EXPIRATION_NOTIFY", "", "" } }
        };

        public const int KP_EVT_CODE_MEDICAL = 0;	//A non-specific medical condition exists
        public const int KP_EVT_CODE_PERSONAL_EMERGENCY = 1;	//Emergency Assistance request
        public const int KP_EVT_CODE_FAIL_TO_REPORT_IN = 2;	//A user has failed to activate a monitoring device
        public const int KP_EVT_CODE_FIRE = 3;	//A non-specific fire alarm condition exists
        public const int KP_EVT_CODE_SMOKE = 4;	//An alarm has been triggered by a smoke detector
        public const int KP_EVT_CODE_COMBUSTION = 5;	//An alarm has been triggered by a combustion detector
        public const int KP_EVT_CODE_WATER_FLOW = 6;	//An alarm has been triggered by a water flow detector
        public const int KP_EVT_CODE_HEAT = 7;	//An alarm has been triggered by a heat detector
        public const int KP_EVT_CODE_PULL_STATION = 8;	//A pull station has been activated
        public const int KP_EVT_CODE_DUCT = 9;	//An alarm has been triggered by a duct detector
        public const int KP_EVT_CODE_FLAME = 10;	 //An alarm has been triggered by a flame detector
        public const int KP_EVT_CODE_NEAR_ALARM_FIRE = 11;	//A near alarm condition has been detected on a fire sens
        public const int KP_EVT_CODE_PANIC = 12;	//A non-specific hold-up alarm exists
        public const int KP_EVT_CODE_DURESS = 13;	//A duress code has been entered by a user
        public const int KP_EVT_CODE_SILENT = 14;	//A silent holdup alarm exists
        public const int KP_EVT_CODE_AUDIBLE = 15;	//An audible hold-up alarm exists
        public const int KP_EVT_CODE_DURESS_ACCESS_GRANTED = 16;	//A duress code has been entered and granted at an entry
        public const int KP_EVT_CODE_DURESS_EGRESS_GRANTED = 17;	//A duress code has been entered and granted at an exit d
        public const int KP_EVT_CODE_HOLDUP_SUSPICION_PRINT = 18;	//A user has activated a trigger to indicate a suspicious
        public const int KP_EVT_CODE_BURGLARY = 19;	//A burglary zone has been violated while armed
        public const int KP_EVT_CODE_PERIMETER = 20;	//A perimeter zone has been violated while armed
        public const int KP_EVT_CODE_INTERIOR = 21;	//An interior zone has been violated while armed
        public const int KP_EVT_CODE_24_HOUR_SAFE = 22;	//A 24 hour burglary zone has been violated
        public const int KP_EVT_CODE_ENTRY_EXIT	= 23;	//An Entry/Exit zone has been violated while armed
        public const int KP_EVT_CODE_DAY_NIGHT = 24;	//An trouble by Day alarm by Night zone has been violated
        public const int KP_EVT_CODE_OUTDOOR = 25;	//An outdoor burglary zone has been violated while armed
        public const int KP_EVT_CODE_TAMPER	= 26;	//A burglary zone has been tampered with while armed
        public const int KP_EVT_CODE_NEAR_ALARM_WORSENS	= 27;	//A burg sensor has detected a condition which will cause
        public const int KP_EVT_CODE_INTRUSION_VERIFIER	= 28;	//The specified zone has verified that an intrusion has o
        public const int KP_EVT_CODE_GENERAL_ALARM = 29;	//The specified zone is in an alarm condition
        public const int KP_EVT_CODE_POLLING_LOOP_OPEN_ARMED = 30;	//An open circuit condition has been detected on a pollin
        public const int KP_EVT_CODE_POLLING_LOOP_SHORT_ARMED = 31;	//A short circuit condition has been detected on a pollin
        public const int KP_EVT_CODE_EXPANSION_MODULE_FAILURE_ARMED = 32;	//A general failure condition has been detected on an exp
        public const int KP_EVT_CODE_SENSOR_TAMPER = 33; //A sensor's tamper has been violated (case opened)
        public const int KP_EVT_CODE_EXPANSION_MODULE_TAMPER = 34; //An expansion module's tamper has been violated (cabinet
        public const int KP_EVT_CODE_SILENT_BURGLARY = 35; //A burglary zone has been violated while armed with no a
        public const int KP_EVT_CODE_SENSOR_SUPERVISION_FAILURE = 36; //A sensor's supervisory circuit has reported a failure w
        public const int KP_EVT_CODE_24_HOUR_NON_BURGLARY = 37; //A non-burglary zone has been faulted
        public const int KP_EVT_CODE_GAS_DETECTED = 38; //The gas detector assigned to the specified zone has rep
        public const int KP_EVT_CODE_REFRIGERATION = 39; //The refrigeration detector assigned to the specified zo
        public const int KP_EVT_CODE_LOSS_OF_HEAT = 40; //The temperature detector assigned to the specified zone
        public const int KP_EVT_CODE_WATER_LEAKAGE = 41; //The water leak detector assigned to the specified zone
        public const int KP_EVT_CODE_FOIL_BREAK = 42; //The specified zone which is assigned to foil used as gl
        public const int KP_EVT_CODE_DAY_TROUBLE = 43; //The specified zone which monitors trouble by day has re
        public const int KP_EVT_CODE_LOW_BOTTLED_GAS_LEVEL = 44; //The gas level detector assigned to the specified zone h
        public const int KP_EVT_CODE_HIGH_TEMP = 45; //The over-temperature detector assigned to the specified
        public const int KP_EVT_CODE_LOW_TEMP = 46; //The under-temperature detector assigned to the specifie
        public const int KP_EVT_CODE_LOSS_OF_AIR_FLOW = 47; //The air flow detector assigned to the specified zone ha
        public const int KP_EVT_CODE_CARBON_MONOXIDE_DETECTED = 48; //The carbon monoxide detector assigned to the specified
        public const int KP_EVT_CODE_TANK_LEVEL = 49; //The tank level detector assigned to the specified zone
        public const int KP_EVT_CODE_HIGH_HUMIDITY = 50; //A High Humidity condition has been detected
        public const int KP_EVT_CODE_LOW_HUMIDITY = 51; //A Low Humidity condition has been detected
        public const int KP_EVT_CODE_FIRE_SUPERVISORY = 52; //The supervisory circuit of the specified fire zone has
        public const int KP_EVT_CODE_LOW_WATER_PRESSURE = 53; //The water pressure sensor assigned to the specified zon
        public const int KP_EVT_CODE_LOW_CO2 = 54; //The CO2 pressure sensor assigned to the specified zone
        public const int KP_EVT_CODE_GATE_VALVE_SENSOR = 55; //The gate valve sensor in the fire sprinkler system assi
        public const int KP_EVT_CODE_LOW_WATER_LEVEL = 56; //The water level sensor assigned to the specified zone h
        public const int KP_EVT_CODE_PUMP_ACTIVATED = 57; //The pump activity detector assigned to the specified zo
        public const int KP_EVT_CODE_PUMP_FAILURE = 58; //A pump output monitor assigned to the specified zone ha
        public const int KP_EVT_CODE_SYSTEM_TROUBLE = 59; //A general system trouble condition has been reported by
        public const int KP_EVT_CODE_AC_LOSS = 60; //AC power loss has been detected at a control or expansi
        public const int KP_EVT_CODE_LOW_SYSTEM_BATTERY = 61; //A battery has failed a load test while the system was d
        public const int KP_EVT_CODE_RAM_CHECKSUM_BAD = 62; //A test of the system's memory has failed
        public const int KP_EVT_CODE_ROM_CHECKSUM_BAD = 63; //A test of the system's executable memory has failed
        public const int KP_EVT_CODE_SYSTEM_RESET = 64; //The system has reset and restarted
        public const int KP_EVT_CODE_PANEL_PROGRAMMING_CHANGED = 65; //The programmed configuration of the panel has changed
        public const int KP_EVT_CODE_SELF_TEST_FAILURE = 66; //The system has failed a self-test
        public const int KP_EVT_CODE_SYSTEM_SHUTDOWN = 67; //The system has been shut down and has stopped functioni
        public const int KP_EVT_CODE_BATTERY_TEST_FAILURE = 68; //The system backup battery has failed a load test while
        public const int KP_EVT_CODE_GROUND_FAULT = 69; //The panel has detected a ground fault condition
        public const int KP_EVT_CODE_BATTERY_MISSING_DEAD = 70; //The system has detected that the backup battery is eith
        public const int KP_EVT_CODE_POWER_SUPPLY_OVERCURRENT = 71; //The system power supply has reported an excessive curre
        public const int KP_EVT_CODE_ENGINEER_RESET = 72; //The specified service person has issued a system reset
        public const int KP_EVT_CODE_PRIMARY_POWER_SUPPLY_FAILURE = 73; //The system's primary power supply has failed a supervis
        public const int KP_EVT_CODE_SYSTEM_TAMPER = 74; //APL- System may have been tampered or compromised
        public const int KP_EVT_CODE_SOUNDER_RELAY_ = 75; //A trouble condition exists in the system's sounder/rela
        public const int KP_EVT_CODE_BELL_1_ = 76; //A trouble condition exists in the primary bell circuit
        public const int KP_EVT_CODE_BELL_2_ = 77; //A trouble condition exists in the secondary bell circui
        public const int KP_EVT_CODE_ALARM_RELAY_ = 78; //A trouble condition exists in the system's alarm relay
        public const int KP_EVT_CODE_TROUBLE_RELAY = 79; //A trouble condition exists in the system's trouble rela
        public const int KP_EVT_CODE_REVERSING_RELAY_ = 80; //The specified TELCO reversing relay has reported a trou
        public const int KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_3 = 81; //A trouble condition exists in the bell #3 circuit
        public const int KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_4 = 82; //A trouble condition exists in the bell #4 circuit
        public const int KP_EVT_CODE_SYSTEM_PERIPHERAL_TROUBLE = 83; //A system peripheral device has reported a trouble condi
        public const int KP_EVT_CODE_POLLING_LOOP_OPEN_DISARMED = 84; //An open circuit condition has been detected on a pollin
        public const int KP_EVT_CODE_POLLING_LOOP_SHORT_DISARMED = 85; //A short circuit condition has been detected on a pollin
        public const int KP_EVT_CODE_EXPANSION_MODULE_FAILURE = 86; //A general failure condition has been detected on an exp
        public const int KP_EVT_CODE_REPEATER_FAILURE = 87; //A repeater in the system has reported a failure conditi
        public const int KP_EVT_CODE_LOCAL_PRINTER_OUT_OF_PAPER = 88; //The printer attached to the panel has reported an Out O
        public const int KP_EVT_CODE_LOCAL_PRINTER_FAILURE = 89; //The printer attached to the panel has reported a failur
        public const int KP_EVT_CODE_EXP_MODULE_DC_LOSS = 90; //An expansion module has detected a DC power loss
        public const int KP_EVT_CODE_EXP_MODULE_LOW_BATT = 91; //An expansion module has detected a low battery conditio
        public const int KP_EVT_CODE_EXP_MODULE_RESET = 92; //An expansion module has reset
        public const int KP_EVT_CODE_EXP_MODULE_TAMPER_ = 93; //An expansion module has detected its taper switch has b
        public const int KP_EVT_CODE_EXP_MODULE_AC_LOSS = 94; //An expansion module has detected the loss of AC power
        public const int KP_EVT_CODE_EXP_MODULE_SELF_TEST_FAIL = 95; //An expansion module has failed a self-test
        public const int KP_EVT_CODE_RF_RECEIVER_JAM_DETECT = 96; //An RF receiver has detected the presence of a jamming s
        public const int KP_EVT_CODE_AES_ENCRYPTION_DIS_ENABLED = 97; //E345 AES Encryption has been disabled R345 AES Encrypti
        public const int KP_EVT_CODE_COMMUNICATION_TROUBLE = 98; //The system has experienced difficulties communicating w
        public const int KP_EVT_CODE_TELCO_1_FAULT = 99; //The system has detected a fault on the primary dial-up
        public const int KP_EVT_CODE_TELCO_2_FAULT = 100; //The system has detected a fault on the secondary dial-u
        public const int KP_EVT_CODE_LONG_RANGE_RADIO_XMITTER_FAULT = 101; //A fault has been detected in the long range radio subsy
        public const int KP_EVT_CODE_FAILURE_TO_COMMUNICATE_EVENT = 102; //The system was unable to communicate an event to the ce
        public const int KP_EVT_CODE_LOSS_OF_RADIO_SUPERVISION = 103; //The radio has not reported in its designated supervisio
        public const int KP_EVT_CODE_LOSS_OF_CENTRAL_POLLING = 104; //The radio has detected a loss in the polling signal fro
        public const int KP_EVT_CODE_LONG_RANGE_RADIO_VSWR_PROBLEM = 105; //The Long Range Radio has reported a transmitter/antenna
        public const int KP_EVT_CODE_PERIODIC_COMM_TEST_FAIL_RESTORE = 106; //A periodic Communication path test has failed
        public const int KP_EVT_CODE_PROTECTION_LOOP = 107; //The specified protection loop has reported a trouble co
        public const int KP_EVT_CODE_PROTECTION_LOOP_OPEN = 108; //The specified protection loop has reported an open-loop
        public const int KP_EVT_CODE_PROTECTION_LOOP_SHORT = 109; //The specified protection loop has reported a shorted-lo
        public const int KP_EVT_CODE_FIRE_TROUBLE = 110; //A fire sensor has detected a trouble condition on the s
        public const int KP_EVT_CODE_EXIT_ERROR_ALARM_ZONE = 111; //Zone An exit error condition has been reported for the
        public const int KP_EVT_CODE_PANIC_ZONE_TROUBLE = 112; //The system has detected a trouble condition on the pani
        public const int KP_EVT_CODE_HOLD_UP = 113; //trouble Zone The system has detected a trouble conditio
        public const int KP_EVT_CODE_SWINGER_TROUBLE = 114; //A fault has occurred on a zone that has been shut down
        public const int KP_EVT_CODE_CROSS = 115; //Trouble Zone The specified zone in a cross-zone configu
        public const int KP_EVT_CODE_SENSOR_TROUBLE = 116; //The specified sensor has reported a trouble condition
        public const int KP_EVT_CODE_LOSS_OF_SUPERVISION_RF = 117; //The specified zone has failed to report in during its d
        public const int KP_EVT_CODE_LOSS_OF_SUPERVISION_RPM = 118; //An Remote Polled Module assigned to the specified zone
        public const int KP_EVT_CODE_SENSOR_TAMPER_FAULT = 119; //The tamper switch on the specified sensor has been faul
        public const int KP_EVT_CODE_RF_LOW_BATTERY = 120; //The specified battery powered RF zone has reported a lo
        public const int KP_EVT_CODE_SMOKE_DETECTOR_HI_SENSITIVITY = 121; //A smoke detector's sensitivity level has drifted to the
        public const int KP_EVT_CODE_SMOKE_DETECTOR_LOW_SENSITIVITY = 122; //A smoke detector's sensitivity level has drifted to the
        public const int KP_EVT_CODE_INTRUSION_DETECTOR_HI_SENSITIVITY = 123; //An intrusion detector's sensitivity level has drifted t
        public const int KP_EVT_CODE_INTRUSION_DETECTOR_LOW_SENSITIVITY = 124; //An intrusion detector's sensitivity level has drifted t
        public const int KP_EVT_CODE_SENSOR_SELF_TEST_FAILURE = 125; //The specified sensor has failed a self-test
        public const int KP_EVT_CODE_SENSOR_WATCH_TROUBLE = 126; //A motion sensor has not been triggered within a pre-def
        public const int KP_EVT_CODE_DRIFT_COMPENSATION_ERROR = 127; //A smoke detector cannot automatically adjust its sensit
        public const int KP_EVT_CODE_MAINTENANCE_ALERT = 128; //The specified zone requires maintenance
        public const int KP_EVT_CODE_CO_DETECTOR_NEEDS_REPLACEMENT = 129; //The specified Carbon Monoxide detector has reached end-
        public const int KP_EVT_CODE_OPEN_CLOSE = 130; //The specified user has disarmed/armed the system
        public const int KP_EVT_CODE_OC_BY_USER = 131; //User The specified user has disarmed/armed the system
        public const int KP_EVT_CODE_GROUP_OC_GROUPA_GROUP_OF = 132; // has been armed or disarmed
        public const int KP_EVT_CODE_AUTOMATIC_OC = 133; //A partition has been automatically armed or disarmed
        public const int KP_EVT_CODE_LATE_TO_OC = 134; //(Note: use 453 or 454 Instead ) User
        public const int KP_EVT_CODE_DEFERRED_OC = 135; //(Obsolete- do not use ) User
        public const int KP_EVT_CODE_CANCEL = 136; //The specified user has cancelled the previously reporte
        public const int KP_EVT_CODE_REMOTE_ARM_DISARM = 137; //The specified user has armed or disarmed the system fro
        public const int KP_EVT_CODE_QUICK_ARM = 138; //The specified user has quick-armed the system
        public const int KP_EVT_CODE_KEYSWITCH_OC = 139; //The specified user has armed or disarmed the system usi
        public const int KP_EVT_CODE_ARMED_STAY = 140; //The specified user has armed the system in STAY mode
        public const int KP_EVT_CODE_KEYSWITCH_ARMED_STAY = 141; //The specified user has armed the system in STAY mode us
        public const int KP_EVT_CODE_ARMED_WITH_SYSTEM_TROUBLE_OVERRIDE = 142; // The specified user has armed the system while overridi
        public const int KP_EVT_CODE_EXCEPTION_OC_ = 143; //The system has been armed or disarmed outside of the co
        public const int KP_EVT_CODE_EARLY_OC = 144; //The system has been disarmed/armed by the specified use
        public const int KP_EVT_CODE_LATE_OC = 145; //The system has been disarmed/armed by the specified use
        public const int KP_EVT_CODE_FAILED_TO_OPEN = 146; //The system has failed to have been disarmed during the
        public const int KP_EVT_CODE_FAILED_TO_CLOSE = 147; //The system has failed to be armed during the designated
        public const int KP_EVT_CODE_AUTO_ARM_FAILED = 148; //The system has failed to automatically arm itself at th
        public const int KP_EVT_CODE_PARTIAL_ARM = 149; //The system has been only partially armed by the specifi
        public const int KP_EVT_CODE_EXIT_ERROR = 150; //User The specified user has made an error exiting the p
        public const int KP_EVT_CODE_USER_ON_PREMISES = 151; //A user has disarmed the system after an alarm has occur
        public const int KP_EVT_CODE_RECENT_CLOSE = 152; //The system had been armed within the last xx minutes
        public const int KP_EVT_CODE_WRONG_CODE_ENTRY = 153; //
        public const int KP_EVT_CODE_LEGAL_CODE_ENTRY = 154; //
        public const int KP_EVT_CODE_RE_ARM_AFTER_ALARM = 155; //
        public const int KP_EVT_CODE_AUTO_ARM_TIME_EXTENDED = 156; //A user has successfully requested that the system delay
        public const int KP_EVT_CODE_PANIC_ALARM_RESET = 157; //The specified panic zone has been reset
        public const int KP_EVT_CODE_SERVICE_ON_OFF_PREMISES = 158; //A service person has entered or left the premises
        public const int KP_EVT_CODE_CALLBACK_REQUEST_MADE = 159; //A remote site (central station) has requested the panel
        public const int KP_EVT_CODE_SUCCESSFUL_DOWNLOAD_ACCESS = 160; //The configuration data of the system has been successfu
        public const int KP_EVT_CODE_UNSUCCESSFUL_ACCESS = 161; //A number of failed attempts have been made to remotely
        public const int KP_EVT_CODE_SYSTEM_SHUTDOWN_COMMAND_RECEIVED = 162; // A central station has sent a system shutdown command t
        public const int KP_EVT_CODE_DIALER_SHUTDOWN_COMMAND_RECEIVED = 163; // A central station has sent a dialer shutdown command t
        public const int KP_EVT_CODE_SUCCESSFUL_UPLOAD = 164; //The configuration data of the system has been successfu
        public const int KP_EVT_CODE_ACCESS_DENIED = 165; //The access control system has denied access to the spec
        public const int KP_EVT_CODE_ACCESS_REPORT_BY_USER = 166; //User
        public const int KP_EVT_CODE_FORCED_ACCESS_ = 167; //The specified access control door has been forced open
        public const int KP_EVT_CODE_EGRESS_DENIED = 168; //The access control system has denied egress to the spec
        public const int KP_EVT_CODE_EGRESS_GRANTED = 169; //The access control system has granted egress for the sp
        public const int KP_EVT_CODE_ACCESS_DOOR_PROPPED_OPEN = 170; //The specified access control door has been held open
        public const int KP_EVT_CODE_ACCESS_POINT_DOOR_STATUS_MONITOR_TROUBLE = 171; //The specified Access Point's Door Status Monitor has re
        public const int KP_EVT_CODE_ACCESS_POINT_REQUEST_TO_EXIT_TROUBLE = 172; //The specified Access Point's Request To Exit zone has r
        public const int KP_EVT_CODE_ACCESS_PROGRAM_MODE_ENTRY = 173; //The access control system has been put into program mod
        public const int KP_EVT_CODE_ACCESS_PROGRAM_MODE_EXIT = 174; //The access control system has exited program mode
        public const int KP_EVT_CODE_ACCESS_THREAT_LEVEL_CHANGE = 175; //The access control system's threat level has been chang
        public const int KP_EVT_CODE_ACCESS_RELAY_TRIGGER_FAIL = 176; //The specified access control output device has failed t
        public const int KP_EVT_CODE_ACCESS_RTE_SHUNT = 177; //The specified Request To Exit zone has been shunted and
        public const int KP_EVT_CODE_ACCESS_DSM_SHUNT = 178; //The specified Door Status Monitor zone has been shunted
        public const int KP_EVT_CODE_SECOND_PERSON_ACCESS = 179; //A second person has accessed an access point conforming
        public const int KP_EVT_CODE_IRREGULAR_ACCESS = 180; //
        public const int KP_EVT_CODE_ACCESS_READER_DISABLE = 181; //The credential reader on the specified access point has
        public const int KP_EVT_CODE_SOUNDER_RELAY_DISABLE = 182; //The specified sounder or relay has been disabled
        public const int KP_EVT_CODE_BELL_1_DISABLE = 183; //The specified output for Bell 1 has been disabled
        public const int KP_EVT_CODE_BELL_2_DISABLE = 184; //The specified output for Bell 2 has been disabled
        public const int KP_EVT_CODE_ALARM_RELAY_DISABLE = 185; //The specified alarm relay has been disabled
        public const int KP_EVT_CODE_TROUBLE_RELAY_DISABLE = 186; //The specified trouble relay has been disabled
        public const int KP_EVT_CODE_REVERSING_RELAY_DISABLE = 187; //The specified reversing relay has been disabled
        public const int KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_3_DISABLE = 188; // The specified output for Bell 3 has been disabled
        public const int KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_4_DISABLE = 189; // The specified output for Bell 4 has been disabled
        public const int KP_EVT_CODE_MODULE_ADDED = 190; //The specified access control module has been added to t
        public const int KP_EVT_CODE_MODULE_REMOVED = 191; //The specified access control module has been removed fr
        public const int KP_EVT_CODE_DIALER_DISABLED = 192; //The specified dialer has been disabled
        public const int KP_EVT_CODE_RADIO_TRANSMITTER_DISABLED = 193; //The specified radio transmitter has been disabled
        public const int KP_EVT_CODE_REMOTE_UPDOWN_LOAD_DISABLE = 194; //Remote configuration has been enabled
        public const int KP_EVT_CODE_ZONE_SENSOR_BYPASS = 195; //The specified zone or sensor has been bypassed
        public const int KP_EVT_CODE_FIRE_BYPASS = 196; //The specified fire zone has been bypassed
        public const int KP_EVT_CODE_24_HOUR = 197; //bypass Zone The specified 24 hour zone has been bypasse
        public const int KP_EVT_CODE_BURG_BYPASS = 198; //The specified burglary zone has been bypassed
        public const int KP_EVT_CODE_GROUP_BYPASS_USER_A_GROUP_OF = 199; // has been bypassed
        public const int KP_EVT_CODE_SWINGER_BYPASS = 200; //The specified zone which has reported an excessive numb
        public const int KP_EVT_CODE_ACCESS = 201; //shunt Zone The specified zone in the access control sys
        public const int KP_EVT_CODE_ACCESS_POINT_BYPASS = 202; //The specified access point in the access control system
        public const int KP_EVT_CODE_VAULT_BYPASS = 203; //The specified vault zone has been bypassed
        public const int KP_EVT_CODE_VENT = 204; //Bypass Zone The specified vent zone has been bypassed a
        public const int KP_EVT_CODE_MANUAL_TRIGGER_TEST_REPORT = 205; //A test report has been triggered manually
        public const int KP_EVT_CODE_PERIODIC_TEST_REPORT = 206; //A periodic test report has been triggered
        public const int KP_EVT_CODE_PERIODIC_RF_TRANSMISSION = 207; //A periodic RF path test report has been triggered
        public const int KP_EVT_CODE_FIRE_TEST = 208; //The specified user has initiated a test of the fire ala
        public const int KP_EVT_CODE_STATUS_REPORT_TO_FOLLOW = 209; //
        public const int KP_EVT_CODE_LISTEN_IN_TO_FOLLOW = 210; //The system is about to activate a 2-way audio session
        public const int KP_EVT_CODE_WALK_TEST_MODE_ = 211; //The specified user has placed the system into the walk-
        public const int KP_EVT_CODE_PERIODIC_TEST_SYSTEM_TROUBLE_PRESENT = 212; // A periodic test has been triggered but the fire system
        public const int KP_EVT_CODE_VIDEO_XMITTER_ACTIVE = 213; //A video look-in session is about to begin
        public const int KP_EVT_CODE_POINT_TESTED_OK = 214; //The specified point tested successfully
        public const int KP_EVT_CODE_POINT_NOT_TESTED = 215; //The specified point has not been tested
        public const int KP_EVT_CODE_INTRUSION = 216; //Walk Tested Zone The specified intrusion zone has been
        public const int KP_EVT_CODE_FIRE_ZONE_WALK_TESTED = 217; //The specified fire zone has been successfully walk-test
        public const int KP_EVT_CODE_PANIC_ZONE_WALK_TESTED = 218; //The specified panic zone has been successfully walk-tes
        public const int KP_EVT_CODE_SERVICE_REQUEST = 219; //A request has been made for system servicing
        public const int KP_EVT_CODE_EVENT_LOG_RESET = 220; //The event log has been reset and all stored events have
        public const int KP_EVT_CODE_EVENT_LOG_50_FULL = 221; //The event log is 50% full
        public const int KP_EVT_CODE_EVENT_LOG_90_FULL = 222; //The event log is 90% full
        public const int KP_EVT_CODE_EVENT_LOG_OVERFLOW = 223; //The event log has overflowed and events have been lost
        public const int KP_EVT_CODE_TIMEDATE_RESET = 224; //The time and date have been reset to a new value by the
        public const int KP_EVT_CODE_TIMEDATE_INACCURATE = 225; //The system time and date is known to be in error
        public const int KP_EVT_CODE_PROGRAM_MODE_ENTRY = 226; //The system has been placed into program mode
        public const int KP_EVT_CODE_PROGRAM_MODE_EXIT = 227; //The system has exited program mode
        public const int KP_EVT_CODE_32_HOUR_EVENT_LOG_MARKER = 228; //
        public const int KP_EVT_CODE_SCHEDULE_CHANGE = 229; //The specified fire/burglary schedule has been changed
        public const int KP_EVT_CODE_EXCEPTION_SCHEDULE_CHANGE = 230; //The time schedule for event reporting by exception has
        public const int KP_EVT_CODE_ACCESS_SCHEDULE_CHANGE = 231; //The specified access control schedule has been changed
        public const int KP_EVT_CODE_SENIOR_WATCH_TROUBLE = 232; //A person has not activated a motion sensor in a specifi
        public const int KP_EVT_CODE_LATCH_KEY_SUPERVISION = 233; //A child has disarmed the system (after school)
        public const int KP_EVT_CODE_SYSTEM_INACTIVITY = 234; //System has not been operated for x days
        public const int KP_EVT_CODE_USER_CODE_X_MODIFIED_BY_INSTALLER = 235; //The Installer has modified the specified User's code
        public const int KP_EVT_CODE_AUXILIARY_3 = 236; //
        public const int KP_EVT_CODE_INSTALLER_TEST = 237; //
        public const int KP_EVT_CODE_USER_ASSIGNED_1 = 238; //
        public const int KP_EVT_CODE_USER_ASSIGNED_2 = 239; //
        public const int KP_EVT_CODE_USER_ASSIGNED_3 = 240; //
        public const int KP_EVT_CODE_USER_ASSIGNED_4 = 241; //
        public const int KP_EVT_CODE_USER_ASSIGNED_5 = 242; //
        public const int KP_EVT_CODE_USER_ASSIGNED_6 = 243; //
        public const int KP_EVT_CODE_USER_ASSIGNED_7 = 244; //
        public const int KP_EVT_CODE_USER_ASSIGNED_8 = 245; //
        public const int KP_EVT_CODE_USER_ASSIGNED_9 = 246; //
        public const int KP_EVT_CODE_USER_ASSIGNED_10 = 247; //
        public const int KP_EVT_CODE_USER_ASSIGNED_11 = 248; //
        public const int KP_EVT_CODE_USER_ASSIGNED_12 = 249; //
        public const int KP_EVT_CODE_USER_ASSIGNED_13 = 250; //
        public const int KP_EVT_CODE_USER_ASSIGNED_14 = 251; //
        public const int KP_EVT_CODE_USER_ASSIGNED_15 = 252; //
        public const int KP_EVT_CODE_USER_ASSIGNED_16 = 253; //
        public const int KP_EVT_CODE_USER_ASSIGNED_17 = 254; //
        public const int KP_EVT_CODE_USER_ASSIGNED_18 = 255; //
        public const int KP_EVT_CODE_USER_ASSIGNED_19 = 256; //
        public const int KP_EVT_CODE_USER_ASSIGNED_20 = 257; //
        public const int KP_EVT_CODE_USER_ASSIGNED_21 = 258; //
        public const int KP_EVT_CODE_USER_ASSIGNED_22 = 259; //
        public const int KP_EVT_CODE_USER_ASSIGNED_23 = 260; //
        public const int KP_EVT_CODE_USER_ASSIGNED_24 = 261; //
        public const int KP_EVT_CODE_USER_ASSIGNED_25 = 262; //
        public const int KP_EVT_CODE_USER_ASSIGNED_26 = 263; //
        public const int KP_EVT_CODE_USER_ASSIGNED_27 = 264; //
        public const int KP_EVT_CODE_USER_ASSIGNED_28 = 265; //
        public const int KP_EVT_CODE_USER_ASSIGNED_29 = 266; //
        public const int KP_EVT_CODE_USER_ASSIGNED_30 = 267; //
        public const int KP_EVT_CODE_USER_ASSIGNED_31 = 268; //
        public const int KP_EVT_CODE_USER_ASSIGNED_32 = 269; //
        public const int KP_EVT_CODE_USER_ASSIGNED_33 = 270; //
        public const int KP_EVT_CODE_USER_ASSIGNED_34 = 271; //
        public const int KP_EVT_CODE_USER_ASSIGNED_35 = 272; //
        public const int KP_EVT_CODE_USER_ASSIGNED_36 = 273; //
        public const int KP_EVT_CODE_USER_ASSIGNED_37 = 274; //
        public const int KP_EVT_CODE_USER_ASSIGNED_38 = 275; //
        public const int KP_EVT_CODE_USER_ASSIGNED_39 = 276; //
        public const int KP_EVT_CODE_USER_ASSIGNED_40 = 277; //
        public const int KP_EVT_CODE_UNABLE_TO_OUTPUT_SIGNAL_DERIVED_CHANNEL = 278; //
        public const int KP_EVT_CODE_STU_CONTROLLER_DOWN_DERIVED_CHANNEL = 279; //
        public const int KP_EVT_CODE_DOWNLOAD_ABORT = 280; //The specified Downloader ID has aborted a download sequ
        public const int KP_EVT_CODE_DOWNLOAD_START_END = 281; //Down loader has started or ended a download sequence to
        public const int KP_EVT_CODE_DOWNLOAD_INTERRUPTED = 282; //A do wnload sequence has been interrupted
        public const int KP_EVT_CODE_AUTO_CLOSE_WITH_BYPASS = 283; //An auto-close sequence has been started and the specifi
        public const int KP_EVT_CODE_BYPASS_CLOSING = 284; //
        public const int KP_EVT_CODE_FIRE_ALARM_SILENCE_ = 285; //The fire alarm has been silenced
        public const int KP_EVT_CODE_SUPERVISORY_POINT_TEST_START_END = 286; //A fire supervisory device has been tested
        public const int KP_EVT_CODE_HOLD_UP_TEST_START_END = 287; //The specified user has started or ended a hold-up test
        public const int KP_EVT_CODE_BURG_TEST_PRINT_START_END = 288; //The printed progress of a burglary test has been started or ended
        public const int KP_EVT_CODE_SUPERVISORY_TEST_PRINT_START_END = 289; //The printed progress of a supervisory test has been started or ended
        public const int KP_EVT_CODE_BURG_DIAGNOSTICS_START_END = 290; //A burglary system diagnostic test has been started or e
        public const int KP_EVT_CODE_FIRE_DIAGNOSTICS_START_END = 291; //A fire system diagnostic test has been started or ended
        public const int KP_EVT_CODE_UNTYPED_DIAGNOSTICS = 292; //
        public const int KP_EVT_CODE_TROUBLE_CLOSING = 293; //(closed with burg. during exit) User
        public const int KP_EVT_CODE_ACCESS_DENIED_CODE_UNKNOWN = 294; //Access has been denied because the system did not recog
        public const int KP_EVT_CODE_SUPERVISORY_POINT_ALARM = 295; //The specified supervisory point has reported an alarm c
        public const int KP_EVT_CODE_SUPERVISORY_POINT_BYPASS = 296; //The specified supervisory point has been bypassed
        public const int KP_EVT_CODE_SUPERVISORY_POINT_TROUBLE = 297; //The specified supervisory point has reported a trouble
        public const int KP_EVT_CODE_HOLD_UP_POINT_BYPASS = 298; //The specified hold-up point has been bypassed
        public const int KP_EVT_CODE_AC_FAILURE_FOR_4_HOURS = 299; //There has been a loss of AC power for at least four hou
        public const int KP_EVT_CODE_OUTPUT_TROUBLE = 300; //The specified output has reported a trouble condition
        public const int KP_EVT_CODE_USER_CODE_FOR_EVENT = 301; //This message contains the ID of the user who triggered
        public const int KP_EVT_CODE_LOG_OFF = 302; //The specified user has logged-off of the system
        public const int KP_EVT_CODE_CS_CONNECTION_FAILURE = 303; //The specified CS connection has failed/restored
        public const int KP_EVT_CODE_RCVR_DATABASE_CONNECTION_FAIL_RESTORE = 304; //The connection to the receiver's database has failed/re
        public const int KP_EVT_CODE_LICENSE_EXPIRATION_NOTIFY = 305; //The product license has been terminated (7810PC)




    }

    
}
