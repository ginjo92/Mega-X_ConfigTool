using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProdigyConfigToolWPF.Protocol
{
    class Timezones
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
                "inicial_hour",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 64 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "inicial_minute",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 65 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "final_hour",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 66 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "final_minute",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 67 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "week_days",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 68 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "excluded_days",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 69 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "reserved",
                new Dictionary<string, object>
                {
                    { "value", new byte[2] },
                    { "address", 70 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "excluded_initial_month",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 72 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "excluded_initial_day",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 76 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "excluded_final_month",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 80 },
                    { "data_grid_view_addr", 7 }
                }
            },
            {
                "excluded_final_day",
                new Dictionary<string, object>
                {
                    { "value", new byte[4] },
                    { "address", 84 },
                    { "data_grid_view_addr", 7 }
                }
            }
       };

        internal void read(MainWindow mainWindow, uint timezone_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint timezones_address = 0x65000 + (512 * (timezone_number - 1));
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((timezones_address >> 16) & 0xff);
            byte_array[i++] = (byte)((timezones_address >> 8) & 0xff);
            byte_array[i++] = (byte)((timezones_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is neededs
        }

        public void Write(MainWindow mainWindow, uint timezone_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            timezone_number = timezone_number - 1;
            string description = ((string)mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Description"]).ToUpper();

            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

            //Initial hour
            DateTime initial_hour_date_time = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Initial hour"].ToString());
            byte initial_hour = (byte)initial_hour_date_time.Hour;
            //Initial minute
            byte initial_minute = (byte)initial_hour_date_time.Minute;
            //Final hour
            DateTime final_hour_date_time = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Final hour"].ToString());
            byte final_hour = (byte)final_hour_date_time.Hour;
            //Final minute
            byte final_minute = (byte)final_hour_date_time.Minute;

            //WEEK DAYS
            byte week_days = 0;
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Monday"].Equals(true))
            {
                week_days += 0x01;
            }
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Tuesday"].Equals(true))
            {
                week_days += 0x02;
            }
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Wednesday"].Equals(true))
            {
                week_days += 0x04;
            }
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Thursday"].Equals(true))
            {
                week_days += 0x08;
            }
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Friday"].Equals(true))
            {
                week_days += 0x10;
            }
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Saturday"].Equals(true))
            {
                week_days += 0x20;
            }
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Sunday"].Equals(true))
            {
                week_days += 0x40;
            }

            //excluded_days
            byte excluded_days = 0;
            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Exception 1"].Equals(true))
            {
                excluded_days += 0x01;
            }

            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Exception 2"].Equals(true))
            {
                excluded_days += 0x02;
            }

            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Exception 3"].Equals(true))
            {
                excluded_days += 0x04;
            }

            if (mainWindow.databaseDataSet.Timezone.Rows[(int)timezone_number]["Exception 4"].Equals(true))
            {
                excluded_days += 0x08;
            }

            //reserved
            byte[] timezone_reserved = (byte[])mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Reserved"];

            //Excluded initial date
            //1
            DateTime excluded_initial_date_1 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 1 initial date"].ToString());
            //2
            DateTime excluded_initial_date_2 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 2 initial date"].ToString());
            //3
            DateTime excluded_initial_date_3 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 3 initial date"].ToString());
            //4
            DateTime excluded_initial_date_4 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 4 initial date"].ToString());

            byte[] excluded_initial_month = (byte[])attributes["excluded_initial_month"]["value"];
            excluded_initial_month[0] = (byte)excluded_initial_date_1.Month;
            excluded_initial_month[1] = (byte)excluded_initial_date_2.Month;
            excluded_initial_month[2] = (byte)excluded_initial_date_3.Month;
            excluded_initial_month[3] = (byte)excluded_initial_date_4.Month;

            byte[] excluded_initial_day = (byte[])attributes["excluded_initial_day"]["value"];
            excluded_initial_day[0] = (byte)excluded_initial_date_1.Day;
            excluded_initial_day[1] = (byte)excluded_initial_date_2.Day;
            excluded_initial_day[2] = (byte)excluded_initial_date_3.Day;
            excluded_initial_day[3] = (byte)excluded_initial_date_4.Day;

            //Excluded final date
            //1
            DateTime excluded_final_date_1 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 1 final date"].ToString());
            //2
            DateTime excluded_final_date_2 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 2 final date"].ToString());
            //3
            DateTime excluded_final_date_3 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 3 final date"].ToString());
            //4
            DateTime excluded_final_date_4 = Convert.ToDateTime(mainWindow.databaseDataSet.Timezone.Rows[(int)(timezone_number)]["Exception 4 final date"].ToString());

            byte[] excluded_final_month = (byte[])attributes["excluded_final_month"]["value"];
            excluded_final_month[0] = (byte)excluded_final_date_1.Month;
            excluded_final_month[1] = (byte)excluded_final_date_2.Month;
            excluded_final_month[2] = (byte)excluded_final_date_3.Month;
            excluded_final_month[3] = (byte)excluded_final_date_4.Month;

            byte[] excluded_final_day = (byte[])attributes["excluded_final_day"]["value"];
            excluded_final_day[0] = (byte)excluded_final_date_1.Day;
            excluded_final_day[1] = (byte)excluded_final_date_2.Day;
            excluded_final_day[2] = (byte)excluded_final_date_3.Day;
            excluded_final_day[3] = (byte)excluded_final_date_4.Day;



            int i = 0;
            uint j = 0;
            uint user_address = 0x65000 + (512 * (timezone_number));
            byte_array[i++] = 0x40;
            byte_array[i++] = (byte)((user_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((user_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(user_address & 0xFF);
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

            //initial hour
            i = (temp + (int)this.attributes["inicial_hour"]["address"]);
            byte_array[i] = initial_hour;
            i++;

            //initial minute
            i = (temp + (int)this.attributes["inicial_minute"]["address"]);
            byte_array[i] = initial_minute;
            i++;

            //final hour
            i = (temp + (int)this.attributes["final_hour"]["address"]);
            byte_array[i] = final_hour;
            i++;

            //final minute
            i = (temp + (int)this.attributes["final_minute"]["address"]);
            byte_array[i] = final_minute;
            i++;

            //Week days
            i = (temp + (int)this.attributes["week_days"]["address"]);
            byte_array[i] = week_days;
            i++;

            //Excluded days
            i = (temp + (int)this.attributes["excluded_days"]["address"]);
            byte_array[i] = excluded_days;
            i++;

            //Reserved
            for (i = (temp + (int)this.attributes["reserved"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved"]["address"] + timezone_reserved.Length); i++, j++)
            {
                byte_array[i] = timezone_reserved[j];
            }

            //Excluded initial month
            for (i = (temp + (int)this.attributes["excluded_initial_month"]["address"]), j = 0; i < (temp + (int)this.attributes["excluded_initial_month"]["address"] + excluded_initial_month.Length); i++, j++)
            {
                byte_array[i] = excluded_initial_month[j];
            }
            //Excluded initial day
            for (i = (temp + (int)this.attributes["excluded_initial_day"]["address"]), j = 0; i < (temp + (int)this.attributes["excluded_initial_day"]["address"] + excluded_initial_day.Length); i++, j++)
            {
                byte_array[i] = excluded_initial_day[j];
            }
            //Excluded final month
            for (i = (temp + (int)this.attributes["excluded_final_month"]["address"]), j = 0; i < (temp + (int)this.attributes["excluded_final_month"]["address"] + excluded_final_month.Length); i++, j++)
            {
                byte_array[i] = excluded_final_month[j];
            }
            //Excluded final day
            for (i = (temp + (int)this.attributes["excluded_final_day"]["address"]), j = 0; i < (temp + (int)this.attributes["excluded_final_day"]["address"] + excluded_final_day.Length); i++, j++)
            {
                byte_array[i] = excluded_final_day[j];
            }

            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow);
        }
    }
}
