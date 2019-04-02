using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MegaXConfigTool.Protocol
{
    class Audio
    {
        internal void write(MainWindow mainWindow, byte[] fragment_data_bytes, uint address, uint msg_size)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            int i = 0;
            uint j = 0;
            General protocol = new General();

            byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);
            byte_array[i++] = (byte)(fragment_data_bytes.Length & 0xFF);

            int temp = i;
            for (i = temp, j = 0; i < (temp + fragment_data_bytes.Length); i++, j++)
            {
                byte_array[i] = fragment_data_bytes[j];
            }
            byte_array[4] = (byte)(i - temp);
            
            protocol.send_msg((uint)i, byte_array, mainWindow.cp_id, mainWindow);

            //protocol.send_msg_block_audio((uint)i, byte_array, address, mainWindow.cp_id, mainWindow, bytes_left); // TODO: Check if cp_id is needed

            //if (mainWindow.blocks_written == 0)
            //{
            //    max_size = 4096 - Constants.KP_FLASH_TAMANHO_DADOS_AUDIO_CONFIG_FLASH;
            //    block_addr = 0x200320;
            //}
            //else
            //{
            //    max_size = 4096;
            //    block_addr = 0x200000 + mainWindow.blocks_written * max_size;
            //}

            //if (mainWindow.counter_blocks == 0 && mainWindow.blocks_written > 1)
            //    fragment_message_id = counter_blocks_acc;

            //bytes_left = max_size - mainWindow.counter_blocks * msg_size;
            //if (block_addr == 0x200320)
            //    address = block_addr + (fragment_message_id * msg_size);
            //else
            //    address = block_addr + (mainWindow.counter_blocks * msg_size);// + mainWindow.bytes_end_of_block;


            //uint max_blocks = max_size / msg_size;

            //if(mainWindow.block_complete != 0)
            //{
            //    mainWindow.block_complete = 0;
            //    counter_blocks_acc += mainWindow.counter_blocks;
            //    protocol.send_command_save_block(max_size, byte_array, block_addr, mainWindow.cp_id, mainWindow);
            //    mainWindow.blocks_written++;
            //    //counter_blocks_acc--;
            //    System.Threading.Thread.Sleep(150);
            //    return;
            //}

            ////bytes_left = msg_size * fragment_message_id;

            //if (bytes_left < msg_size)
            //{
            //    mainWindow.bytes_end_of_block = bytes_left;
            //    i = 0;
            //    j = bytes_left;
            //    //address += bytes_left;
            //    byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
            //    byte_array[i++] = (byte)((address >> 16) & 0xFF);
            //    byte_array[i++] = (byte)((address >> 8) & 0xFF);
            //    byte_array[i++] = (byte)(address & 0xFF);
            //    byte_array[i++] = (byte)bytes_left;// fragment_data_bytes.Length;
            //    int temp = i;
            //    for (i = temp, j = 0; i < (temp + bytes_left); i++, j++)
            //    {
            //        byte_array[i] = fragment_data_bytes[j];
            //    }
            //    byte_array[4] = (byte)(i - temp);

            //    address = block_addr + ((mainWindow.counter_blocks) * msg_size);
            //    protocol.send_msg_block_audio((uint)i, byte_array, address, mainWindow.cp_id, mainWindow, bytes_left); // TODO: Check if cp_id is needed


            //    //bytes_left = 0;
            //}
            //else
            //{
            //    i = 0;
            //    j = 0;
            //    byte_array[i++] = Constants.WRITE_BLOCK_CODE_START;
            //    byte_array[i++] = (byte)((address >> 16) & 0xFF);
            //    byte_array[i++] = (byte)((address >> 8) & 0xFF);
            //    byte_array[i++] = (byte)(address & 0xFF);

            //    byte_array[i++] = (byte)fragment_data_bytes.Length;// fragment_data_bytes.Length;
            //    int temp = i;
            //    //TODO: Create a function for this for 

            //    //Options
            //    for (i = temp, j = 0; i < (temp + fragment_data_bytes.Length); i++, j++)
            //    {
            //        byte_array[i] = fragment_data_bytes[j];
            //    }

            //    //byte_array[4] = (byte)fragment_data_bytes.Length;
            //    byte_array[4] = (byte)(i - temp);


            //    //protocol.send_msg((uint)(i), byte_array, mainWindow.cp_id, mainWindow); // TODO: Check if cp_id is needed
            //    protocol.send_msg_block_audio((uint)i, byte_array, address, mainWindow.cp_id, mainWindow, (uint)fragment_data_bytes.Length); // TODO: Check if cp_id is needed
            //    //bytes_left -= msg_size;
            //}

            ////System.Threading.Thread.Sleep(mainWindow.intervalsleeptime);
            ////System.Threading.Thread.Sleep(250);
        }

        internal void write_block(MainWindow mainWindow, uint address, uint block_size)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            int i = 0;
            General protocol = new General();
            
            byte_array[i++] = Constants.WRITE_BLOCK_CODE;
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);
            byte_array[i++] = (byte)((block_size >> 8) & 0xFF);
            byte_array[i++] = (byte)(block_size & 0xFF);

            protocol.send_msg((uint)i, byte_array, mainWindow.cp_id, mainWindow);
        }

        internal void write_full_block(MainWindow mainWindow, uint address, uint block_size)
        {
            byte[] byte_array = new byte[240]; // verificar este tamanho
            int i = 0;
            General protocol = new General();

            byte_array[i++] = Constants.WRITE_BLOCK_AUDIO_CODE;
            byte_array[i++] = (byte)((address >> 16) & 0xFF);
            byte_array[i++] = (byte)((address >> 8) & 0xFF);
            byte_array[i++] = (byte)(address & 0xFF);
            byte_array[i++] = (byte)((block_size >> 8) & 0xFF);
            byte_array[i++] = (byte)(block_size & 0xFF);

            protocol.send_msg((uint)i, byte_array, mainWindow.cp_id, mainWindow);
        }

        internal void CreateUnifiedFile(MainWindow mainWindow, DataGrid reserved_grid, DataGrid customized_grid)
        {
            

            //Type aqwe = myDataGrid.Items.GetItemAt(0).GetType();
            //create new file
            string path_audio = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";

            if (!Directory.Exists(path_audio))
                Directory.CreateDirectory(path_audio);
            
            string filename = (mainWindow.AppDbFile).Substring(0,(mainWindow.AppDbFile).Length - 5);
            var file = new FileInfo(path_audio + "unified_" + filename + ".mai");

            FileStream unified_file_stream = file.Create();
            byte[] input_audio_file_data = new byte[(8000 * 5)];//5 segundos máximo por mensagem de 8Kb/s
            int[] audio_ids = new int[1024];
            long[] audio_positions = new long[1024];
            long[] audio_size = new long[1024];
            int reserved_audios_counter = 0;
            int customized_audios_counter = 0;
            int audios_counter = 0;

            //Add 0xFF to ID,POS and SIZE of audio tracks
            for (int i = 0; i < (1024 * 12); i++)
            {
                unified_file_stream.WriteByte(0xFF);
            }

            //System.Threading.Thread.Sleep(250);
            System.Diagnostics.Debug.WriteLine("in " + file);

            ///RESERVED DATAGRID
            //Read data from each audio file and write in output file
            for (int i = 0; i < reserved_grid.Items.Count; i++)
            {
                DataRowView input_audio_file = (DataRowView)reserved_grid.Items.GetItemAt(i);
                

                String path_unified = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + input_audio_file["Filepath"];
                FileStream input_audio = new FileStream(path_unified, FileMode.Open);
                int read_bytes_number = input_audio.Read(input_audio_file_data, 0, input_audio_file_data.Length);
                audio_ids[i] = i;
                audio_size[i] = input_audio.Length;
                audio_positions[i] = unified_file_stream.Length - (1024*12);

                for (int file_byte_counter = 0; file_byte_counter < (read_bytes_number - 1); file_byte_counter++)
                {
                    unified_file_stream.WriteByte(input_audio_file_data[file_byte_counter]);
                }

                input_audio.Close();

                System.Diagnostics.Debug.WriteLine(" ID: " + i + " | AUDIO R: " + input_audio_file["Filepath"]);

                reserved_audios_counter = i;
            }

            reserved_audios_counter = reserved_audios_counter + 1;
            for (int i = 0; i < customized_grid.Items.Count - 1; i++)
            {
                DataRowView input_audio_file = (DataRowView)customized_grid.Items.GetItemAt(i);
                if (File.Exists((System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + input_audio_file["Filepath"])))
                {
                    FileStream input_audio = new FileStream((System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + input_audio_file["Filepath"]), FileMode.Open);
                    int read_bytes_number = input_audio.Read(input_audio_file_data, 0, input_audio_file_data.Length);
                    audio_ids[i + reserved_audios_counter] = i + reserved_audios_counter;
                    audio_size[i + reserved_audios_counter] = input_audio.Length;
                    audio_positions[i + reserved_audios_counter] = unified_file_stream.Length - (1024 * 12);

                    for (int file_byte_counter = 0; file_byte_counter < (read_bytes_number - 1); file_byte_counter++)
                    {
                        unified_file_stream.WriteByte(input_audio_file_data[file_byte_counter]);
                    }

                    System.Diagnostics.Debug.WriteLine(" ID: " + (reserved_audios_counter + i) + " | AUDIO C: " + input_audio_file["Filepath"]);

                    input_audio.Close();
                    customized_audios_counter = i;
                }
            }

            audios_counter = reserved_audios_counter + customized_audios_counter;

            //write indexes
            unified_file_stream.Position = 0;
            for (int i = 0; i < audios_counter; i++)
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

                //string AudioIDs = Convert.ToString(audio_ids[i + reserved_audios_counter]);
                //string AudioSizes = Convert.ToString(audio_size[i + reserved_audios_counter]);
                //string AudioPositions = Convert.ToString(audio_positions[i + reserved_audios_counter]);
                //System.Diagnostics.Debug.WriteLine("AUDIO --- ID: " + AudioIDs + " Size: " + AudioSizes + " Pos: " + AudioPositions);
            }

            unified_file_stream.Close();
        }
    }
}
