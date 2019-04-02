using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Event
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
       new Dictionary<string, Dictionary<string, object>>
       {
            //{
            //    "cra_account_number",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 2 },
            //    }
            //},
            //{
            //    "event_type",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 4 },
            //    }
            //},
            //{
            //    "id",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[4] },
            //        { "address", 6 }
            //    }

            //},
            //{
            //    "event_area",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[4] },
            //        { "address", 10 }
            //    }
            //},
            //{
            //    "event_user",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 14 },
            //    }
            //},
            //{
            //    "event_zone",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 16 },
            //    }
            //},
            //{
            //    "event_session",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 18 }
            //    }
            //},
            //{
            //    "event_start_or_end",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[1] },
            //        { "address", 20 }
            //    }
            //},
            ////event_time
            //{
            //    "event_hour",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[1] },
            //        { "address", 21}
            //    }
            //},
            //{
            //    "event_minute",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[1] },
            //        { "address", 22 }
            //    }
            //},
            //{
            //    "event_seconds",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[1] },
            //        { "address", 23 }
            //    }
            //},
            ////event date
            //{
            //    "event_month",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[1] },
            //        { "address", 24 }
            //    }
            //},
            //{
            //    "event_day",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[1] },
            //        { "address", 25 }
            //    }
            //},
            //{
            //    "event_year",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 26 }
            //    }
            //},
            //{
            //    "event_phone_numbers",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[2] },
            //        { "address", 28 }
            //    }
            //},
            //{
            //    "event_report_type",
            //    new Dictionary<string, object>
            //    {
            //        { "value", new byte[4] },
            //        { "address", 30 }
            //    }
            //}
              {
                "cra_account_number",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 0 },
                }
            },
            {
                "event_type",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 2 },
                }
            },
            {
                "id",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 4 }
                }

            },
            {
                "event_area",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 8 }
                }
            },
            {
                "event_user",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 12 },
                }
            },
            {
                "event_zone",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 14 },
                }
            },
            {
                "event_session",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 16 }
                }
            },
            {
                "event_start_or_end",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 18 }
                }
            },
            //event_time
            {
                "event_hour",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 19 }
                }
            },
            {
                "event_minute",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 20 }
                }
            },
            {
                "event_seconds",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 21 }
                }
            },
            //event date
            {
                "event_month",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 22 }
                }
            },
            {
                "event_day",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 23 }
                }
            },
            {
                "event_year",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 24 }
                }
            },
            {
                "event_phone_numbers",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 26 }
                }
            },
            {
                "event_report_type",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 28 }
                }
            },
            {
                "event_keypad_ack",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 32 }
                }
            }
        };

        internal void read(MainWindow mainWindow, uint event_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint events_address = Constants.KP_EVENTS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_EVENTOS_FLASH * event_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = Constants.READ_CODE;
            byte_array[i++] = (byte)((events_address >> 16) & 0xff);
            byte_array[i++] = (byte)((events_address >> 8) & 0xff);
            byte_array[i++] = (byte)((events_address) & 0xff);
            byte_array[i++] = size;
            
            string StringByte = BitConverter.ToString(byte_array);

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow); //TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }

        //public void Write(MainWindow mainWindow, uint event_number)
        //{
        //    byte[] byte_array = new byte[240];
        //    event_number = event_number - 1;

        //    //ulong event_keypad_ack = ulong.Parse(mainWindow.databaseDataSet.Event.Rows[(int)event_number]["Keypad_ack"].ToString());
        //    //byte[] event_keypad_ack_bytes = new byte[1];

        //    //if (mainWindow.databaseDataSet.Event.Rows[(int)(event_number)]["Keypad_ack"].Equals(0))
        //    //{
        //    //    event_keypad_ack_bytes[0] = 1;
        //    //}

        //    int i = 0;
        //    uint j = 0;
        //    uint events_address = 0x82800 + (256 * (event_number));  //Constants.KP_EVENTS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_EVENTOS_FLASH * (event_number - 1));


        //    // Create first 5 bytes of the request
        //    byte_array[i++] = 0x91;
        //    byte_array[i++] = (byte)((events_address >> 16) & 0xFF);
        //    byte_array[i++] = (byte)((events_address >> 8) & 0xFF);
        //    byte_array[i++] = (byte)((events_address) & 0xFF);
        //    byte_array[i++] = 240;
        //    int temp = i;
            
        //    byte_array[4] = (byte)(i - temp);
        //    General protocol = new General();
        //    protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow);
        //}

        //byte[] GetIntArray(ulong num)
        //{
        //    List<byte> listOfInts = new List<byte>();
        //    while (num > 0)
        //    {
        //        listOfInts.Add((byte)(num % 10));
        //        num = num / 10;
        //    }
        //    listOfInts.Reverse();
        //    return listOfInts.ToArray();
        //}
    }
}
