using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace ProdigyConfigToolWPF.Protocol
{
    class AudioSystemConfiguration
    {
        public Dictionary<string, Dictionary<string, object>> attributes = new Dictionary<string, Dictionary<string, object>>
        {
            {
                "faixas_audio_230_ok",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 0}
                }
            },{
                "faixas_audio_230_falha",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 1}
                }
            },{
                "faixas_audio_bateria_ok",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 2}
                }
            },{
                "faixas_audio_bateria_falha",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 3}
                }
            },{
                "faixas_audio_coacao",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 4}
                }
            },{
                "faixas_audio_config_start",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 5}
                }
            },{
                "faixas_audio_config_end",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 6}
                }
            },{
                "faixas_audio_tamper",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 7}
                }
            },
            {
                "faixas_audio_bypass",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 8}
                }
            },
            {
                "faixas_audio_alarme",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 9}
                }
            },
            {
                "faixas_audio_alarme_panico",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 10}
                }
            },
            {
                "faixas_audio_alarme_medico",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 11}
                }
            },
            {
                "faixas_audio_alarme_incendio",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 12}
                }
            },
            {
                "faixas_audio_alarme_mask",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 13}
                }
            },
            {
                "faixas_audio_alarme_supervisao",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 14}
                }
            },
            {
                "faixas_audio_alarme_24",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 15}
                }
            },
            {
                "faixas_audio_codigo_ok",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 16}
                }
            },
            {
                "faixas_audio_codigo_errado",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 17}
                }
            },
            {
                "faixas_audio_particao",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 18}
                }
            },
            {
                "faixas_audio_armada",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 19}
                }
            },
            {
                "faixas_audio_desarmada",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 20}
                }
            },
            {
                "faixas_audio_restauro",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 21}
                }
            },
            {
                "faixas_audio_falha_linha_telefone",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 22}
                }
            },
            {
                "faixas_audio_linha_telefone_ok",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 23}
                }
            },
            {
                "faixas_audio_falha_enviar_eventos",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 24}
                }
            },
            {
                "faixas_audio_reset_sistema",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0},
                    { "index", 25}
                }
            },
            {
                "faixas_audio_tamper_sistema",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 26}
                }
            },
            {
                "faixas_audio_tamper_sirene",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 27}
                }
            },
            {
                "faixas_audio_tamper_teclado",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 28}
                }
            },
            {
                "faixas_audio_boas_vindas",
                new Dictionary<string, object>
                {
                    { "value", new byte[8] },
                    { "address", 0 },
                    { "index", 29}
                }
            }
        };
        
        internal void read(MainWindow mainWindow, uint audio_system_number)
        {
            byte[] byte_array = new byte[63];
            uint i = 0;
            uint zone_1_address = 0x200000 + (8 * (audio_system_number - 1));
            byte size = 240;

            // Create first 5 bytes of the request            
            byte_array[i++] = 0x20;
            byte_array[i++] = (byte)((zone_1_address >> 16) & 0xff);
            byte_array[i++] = (byte)((zone_1_address >> 8) & 0xff);
            byte_array[i++] = (byte)((zone_1_address) & 0xff);
            byte_array[i++] = size;

            General protocol = new General();
            protocol.send_msg(i, byte_array, mainWindow.cp_id, mainWindow);            
        }

        internal void write(MainWindow mainform, uint audio_system_configuration_id)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            audio_system_configuration_id = audio_system_configuration_id - 1;

            int i = 0;
            uint j = 0;
            uint audio_system_configuration_address = 0x200000 + (8 * (audio_system_configuration_id));
            byte_array[i++] = 0x40;
            byte_array[i++] = (byte)((audio_system_configuration_address >> 16) & 0xFF);
            byte_array[i++] = (byte)((audio_system_configuration_address >> 8) & 0xFF);
            byte_array[i++] = (byte)(audio_system_configuration_address & 0xFF);
            byte_array[i++] = 240;
            int temp = i;

            #region AUDIO TRACKS
            byte[] audio_tracks = new byte[8];
            audio_tracks[0] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO1"].ToString())) & 0xFF);
            audio_tracks[1] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO1"].ToString()) - 1) >> 8);
            audio_tracks[2] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO2"].ToString())) & 0xFF);
            audio_tracks[3] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO2"].ToString()) - 1) >> 8);
            audio_tracks[4] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO3"].ToString())) & 0xFF);
            audio_tracks[5] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO3"].ToString()) - 1) >> 8);
            audio_tracks[6] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO4"].ToString())) & 0xFF);
            audio_tracks[7] = (byte)((int.Parse(mainform.databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_id]["AUDIO4"].ToString()) - 1) >> 8);
            #endregion
            
            for (i = temp, j = 0; i <  temp + audio_tracks.Length; i++, j++)
            {
                byte_array[i] = audio_tracks[j];
            }

            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            protocol.send_msg((uint)i, byte_array, mainform.cp_id, mainform); // TODO: Check if cp_id is needed
        }
    }
}
