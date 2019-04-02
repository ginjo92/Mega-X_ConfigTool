using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MegaXConfigTool.Protocol
{
    class Expanders
    {
        public Dictionary<string, Dictionary<string, object>> attributes =
        new Dictionary<string, Dictionary<string, object>>
        {
            {
                "description",
                new Dictionary<string, object>
                {
                    { "value", new byte[64] },
                    { "address", 0 }
                }

            },
            {
                "config_pins",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 64 }
                }
            },
            {
                "active_outputs",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 65 }
                }
            },
            {
                "options",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 66 },
                    //OPTIONS 1                                                                        //----
                    { "KP_EXPANDER_RS485_ATIVO", new uint[2] { 0x01, 0} },                             //bit0
                    { "KP_EXPANDER_ENABLE_TAMPER", new uint[2] { 0x02, 1} },                           //bit1     
                    { "KP_EXPANDER_CONFIG_TAMPER_NO_NC", new uint[2] { 0x04, 2} },                     //bit2
                    
                }
            },

            {
                "reserved_1",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 74 }
                }
            },
         
            {
                "reserved_2",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 75 }
                }
            },
            {
                "reserved_3",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 76 }
                }
            },
             {
                "reserved_4",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 77 }
                }
            },

            { 
                "circuit_type",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 78 }
                }
            },
             {
                "configs_r1",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 79 }
                }
            },
             {
                "type_r1",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 80 }
                }
            },
             {
                "configs_r2",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 81 }
                }
            },
             {
                "type_r2",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 82 }
                }
            },
             {
                "configs_r3",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 83 }
                }
            },
             {
                "type_r3",
                new Dictionary<string, object>
                {
                    { "value", new byte[1] },
                    { "address", 84 }
                }
            },
             
        }; 

        public void read(MainWindow mainForm, uint expander_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint expander_address = Constants.KP_EXPANDERS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_EXPANDER_FLASH * expander_number);
            byte size = 240;

            // Create first 5 bytes of the request
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((expander_address >> 16) & 0xff);
            byte_array[i++] = (byte)((expander_address >> 8) & 0xff);
            byte_array[i++] = (byte)((expander_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainForm.cp_id, mainForm); // TODO: Check if cp_id is neededs
            System.Threading.Thread.Sleep(250);
        }

        public void Write(MainWindow mainWindow, uint expander_number)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho

            string description = ((string)mainWindow.databaseDataSet.Expander.Rows[(int)(expander_number)]["Description"]).ToUpper();
            
            short expander_config_pins = 0;
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 1"].Equals(true))
            {
                expander_config_pins += 0x01;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 2"].Equals(true))
            {
                expander_config_pins += 0x02;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 3"].Equals(true))
            {
                expander_config_pins += 0x04;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 4"].Equals(true))
            {
                expander_config_pins += 0x08;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 5"].Equals(true))
            {
                expander_config_pins += 0x10;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 6"].Equals(true))
            {
                expander_config_pins += 0x20;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 7"].Equals(true))
            {
                expander_config_pins += 0x40;
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config pin 8"].Equals(true))
            {
                expander_config_pins += 0x80;
            }

            #region Expander options read from dataset
            ////EXPANDER OPTIONS
            ulong expander_options = 0;
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Active"].Equals(true))
            {
                expander_options += (uint)(0xFFFF & (((uint[])this.attributes["options"]["KP_EXPANDER_RS485_ATIVO"])[0]));
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Enable tamper"].Equals(true))
            {
                expander_options += (uint)(0xFFFF & (((uint[])this.attributes["options"]["KP_EXPANDER_ENABLE_TAMPER"])[0]));
            }
            if (mainWindow.databaseDataSet.Expander.Rows[(int)expander_number]["Config tamper"].Equals(true))
            {
                expander_options += (uint)(0xFFFF & (((uint[])this.attributes["options"]["KP_EXPANDER_CONFIG_TAMPER_NO_NC"])[0]));
            }
            #endregion

            byte[] description_bytes = new byte[64];
            description_bytes = Encoding.GetEncoding("UTF-8").GetBytes(description);

            //byte[] expander_reserved_1_bytes = (byte[])mainWindow.databaseDataSet.Expander.Rows[(int)(expander_number)]["Reserved 1"];
            //byte[] expander_reserved_2_bytes = (byte[])mainWindow.databaseDataSet.Expander.Rows[(int)(expander_number)]["Reserved 2"];
            //byte[] expander_reserved_3_bytes = (byte[])mainWindow.databaseDataSet.Expander.Rows[(int)(expander_number)]["Reserved 3"];
            //byte[] expander_reserved_4_bytes = (byte[])mainWindow.databaseDataSet.Expander.Rows[(int)(expander_number)]["Reserved 4"];

            byte[] expander_options_bytes = BitConverter.GetBytes(expander_options);
            byte[] expander_config_pins_bytes = BitConverter.GetBytes(expander_config_pins);
            //byte[] expander_config_pins_bytes = (byte[])attributes["config_pins"]["value"];
            
            int i = 0;
            uint j = 0;
            uint expander_address = Constants.KP_EXPANDERS_INIC_ADDR + (Constants.KP_FLASH_TAMANHO_DADOS_EXPANDER_FLASH * expander_number);
            byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
            byte_array[i++] = (byte)((expander_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((expander_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(expander_address & 0xFF);

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
            //////Reserved 1
            //for (i = (temp + (int)this.attributes["reserved_1"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_1"]["address"] + expander_reserved_1_bytes.Length); i++, j++)
            //{
            //    byte_array[i] = expander_reserved_1_bytes[j];
            //}

            ////reserved 2
            //for (i = (temp + (int)this.attributes["reserved_2"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_2"]["address"] + expander_reserved_2_bytes.Length); i++, j++)
            //{
            //    byte_array[i] = expander_reserved_2_bytes[j];
            //}
            ////reserved 3
            //for (i = (temp + (int)this.attributes["reserved_3"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_3"]["address"] + expander_reserved_3_bytes.Length); i++, j++)
            //{
            //    byte_array[i] = expander_reserved_3_bytes[j];
            //}
            ////reserved 4
            //for (i = (temp + (int)this.attributes["reserved_4"]["address"]), j = 0; i < (temp + (int)this.attributes["reserved_4"]["address"] + expander_reserved_4_bytes.Length); i++, j++)
            //{
            //    byte_array[i] = expander_reserved_4_bytes[j];
            //}

            //config_pins
            for (i = (temp + (int)this.attributes["config_pins"]["address"]), j = 0; i < (temp + (int)this.attributes["config_pins"]["address"] + expander_config_pins_bytes.Length); i++, j++)
            {
                byte_array[i] = expander_config_pins_bytes[j];
            }

            //Options
            for (i = (temp + (int)this.attributes["options"]["address"]), j = 0; i < (temp + (int)this.attributes["options"]["address"] + expander_options_bytes.Length); i++, j++)
            {
                byte_array[i] = expander_options_bytes[j];
            }
            
            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            //protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is needed

            protocol.send_msg_block((uint)i, byte_array, expander_address, mainWindow.cp_id, mainWindow, Constants.KP_FLASH_TAMANHO_DADOS_EXPANDER_FLASH); // TODO: Check if cp_id is needed
            System.Threading.Thread.Sleep(mainWindow.intervalsleeptime);
        }
    }
}
