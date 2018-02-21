using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdigyConfigToolWPF.Protocol
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
            uint events_address = Constants.KP_EVENTS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_EVENTOS_FLASH * (event_number - 1));
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((events_address >> 16) & 0xff);
            byte_array[i++] = (byte)((events_address >> 8) & 0xff);
            byte_array[i++] = (byte)((events_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is neededs
        }
    }
}
