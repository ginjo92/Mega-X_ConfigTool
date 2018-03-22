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
    class Audio
    {
        internal void write(MainWindow mainWindow, byte[] fragment_data_bytes, uint fragment_message_id)
        {
            //int KP_FLASH_NUM_MAX_AUDIO_SAMPLES = 1024;
            //byte[] KP_FLASH_TAMANHO_AUDIO_SISTEMA = new byte[byte.Parse("4") * 2 * 100];
            //byte[] KP_FLASH_INICIO_ESTRUTURA_AUDIO = new byte[2 * 1024 + 1024 + BitConverter.ToInt32(KP_FLASH_TAMANHO_AUDIO_SISTEMA,0)];

            //var _KP_FLASH_INICIO_ESTRUTURA_AUDIO = BitConverter.ToInt32(KP_FLASH_INICIO_ESTRUTURA_AUDIO, 0);
            //var _KP_FLASH_TAMANHO_AUDIO_SISTEMA = BitConverter.ToInt32(KP_FLASH_TAMANHO_AUDIO_SISTEMA, 0);
            //var sum = _KP_FLASH_INICIO_ESTRUTURA_AUDIO + _KP_FLASH_TAMANHO_AUDIO_SISTEMA * KP_FLASH_NUM_MAX_AUDIO_SAMPLES;
            //byte[] KP_FLASH_INICIO_AUDIO = BitConverter.GetBytes(sum);

            //byte[] KP_FLASH_FIM_FAIXAS_AUDIO = new byte[6 * 1024 * 1024 - 1];
            //byte[] KP_FLASH_INICIO_AUDIO_SISTEMA = new byte[2 * 1024 * 1024];

            
            byte[] byte_array = new byte[256]; // verificar este tamanho
            int i = 0;
            uint j = 0;
            uint address = 0x200320 + (fragment_message_id * 235);
            byte_array[i++] = 0x40;
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);

            byte_array[i++] = (byte)fragment_data_bytes.Length;
            int temp = i;
            //TODO: Create a function for this for 

            //Options
            for (i = temp, j = 0; i < (temp + fragment_data_bytes.Length); i++, j++)
            {
                byte_array[i] = fragment_data_bytes[j];
            }

            //byte_array[4] = (byte)fragment_data_bytes.Length;
            byte_array[4] = (byte)(i - temp);
            General protocol = new General();
            protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is needed
        }

        internal void CreateUnifiedFile(DataGrid reserved_grid, DataGrid customized_grid)
        {
            //Type aqwe = myDataGrid.Items.GetItemAt(0).GetType();
            //create new file
            var file = new FileInfo(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\DefaultAudioFiles\" + "new_audio_files_unified.mai");
            FileStream unified_file_stream = file.Create();
            byte[] input_audio_file_data = new byte[(8000 * 5)];//5 segundos máximo por mensagem de 8Kb/s
            int[] audio_ids = new int[1024];
            long[] audio_positions = new long[1024];
            long[] audio_size = new long[1024];
            int reserved_audios_counter = 0;

            //Add 0xFF to ID,POS and SIZE of audio tracks
            for (int i = 0; i < ( 1024 * 12); i++)
            {
                unified_file_stream.WriteByte(0xFF);
            }

            ///RESERVED DATAGRID
            //Read data from each audio file and write in output file
            for(int i = 0; i < reserved_grid.Items.Count; i++)
            {
                DataRowView input_audio_file = (DataRowView)reserved_grid.Items.GetItemAt(i);
                FileStream input_audio = new FileStream((System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\DefaultAudioFiles\" + input_audio_file["Description"] + ".raw"), FileMode.Open);
                int read_bytes_number = input_audio.Read(input_audio_file_data, 0, input_audio_file_data.Length);
                audio_ids[i] = i;
                audio_size[i] = input_audio.Length;
                audio_positions[i] = unified_file_stream.Length - (1024*12);

                for( int file_byte_counter = 0; file_byte_counter < (read_bytes_number - 1); file_byte_counter++)
                {
                    unified_file_stream.WriteByte(input_audio_file_data[file_byte_counter]);
                }

                input_audio.Close();
                reserved_audios_counter = i;
            }

            reserved_audios_counter = reserved_audios_counter + 1;
            for (int i = 0; i < customized_grid.Items.Count - 1; i++)
            {
                DataRowView input_audio_file = (DataRowView)customized_grid.Items.GetItemAt(i);
                if(File.Exists((System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\DefaultAudioFiles\" + input_audio_file["Description"] + ".raw")))
                {
                    FileStream input_audio = new FileStream((System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\DefaultAudioFiles\" + input_audio_file["Description"] + ".raw"), FileMode.Open);
                    int read_bytes_number = input_audio.Read(input_audio_file_data, 0, input_audio_file_data.Length);
                    audio_ids[i + reserved_audios_counter] = i + reserved_audios_counter;
                    audio_size[i + reserved_audios_counter] = input_audio.Length;
                    audio_positions[i + reserved_audios_counter] = unified_file_stream.Length - (1024 * 12);

                    for (int file_byte_counter = 0; file_byte_counter < (read_bytes_number - 1); file_byte_counter++)
                    {
                        unified_file_stream.WriteByte(input_audio_file_data[file_byte_counter]);
                    }

                    input_audio.Close();
                }
            }


            //write indexes
            unified_file_stream.Position = 0;
            for (int i = 0; i < reserved_grid.Items.Count; i++)
            {
                unified_file_stream.WriteByte((byte)((audio_ids[i] >> 24) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_ids[i] >> 16) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_ids[i] >> 8) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_ids[i] >> 0) & 0xFF));

                unified_file_stream.WriteByte((byte)((audio_size[i] >> 24) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_size[i] >> 16) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_size[i] >> 8) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_size[i] >> 0) & 0xFF));

                unified_file_stream.WriteByte((byte)((audio_positions[i] >> 24) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_positions[i] >> 16) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_positions[i] >> 8) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_positions[i] >> 0) & 0xFF));

            }

            for (int i = 0; i < customized_grid.Items.Count -1; i++)
            {
                unified_file_stream.WriteByte((byte)((audio_ids[i + reserved_audios_counter] >> 24) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_ids[i + reserved_audios_counter] >> 16) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_ids[i + reserved_audios_counter] >> 8) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_ids[i + reserved_audios_counter] >> 0) & 0xFF));

                unified_file_stream.WriteByte((byte)((audio_size[i + reserved_audios_counter] >> 24) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_size[i + reserved_audios_counter] >> 16) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_size[i + reserved_audios_counter] >> 8) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_size[i + reserved_audios_counter] >> 0) & 0xFF));

                unified_file_stream.WriteByte((byte)((audio_positions[i + reserved_audios_counter] >> 24) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_positions[i + reserved_audios_counter] >> 16) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_positions[i + reserved_audios_counter] >> 8) & 0xFF));
                unified_file_stream.WriteByte((byte)((audio_positions[i + reserved_audios_counter] >> 0) & 0xFF));

            }

            unified_file_stream.Close();
        }   
    }
}
