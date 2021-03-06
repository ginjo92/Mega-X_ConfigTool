﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;
using System.IO.Ports;
using MegaXConfigTool.Protocol;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using System.Windows.Controls.Primitives;
using MahApps.Metro.Controls.Dialogs;
using System.Data;
using System.Diagnostics;
using NAudio.Wave;
using System.Threading;
using System.ComponentModel;
using MegaXConfigTool.defaultDataSetTableAdapters;
using MegaXConfigTool.SqliteLoginDataSetTableAdapters;
using System.Data.SQLite;
using System.Collections;
using System.Windows.Markup;
using System.Text.RegularExpressions;
using System.Security.Principal;
using System.Security.AccessControl;
using System.Data.SqlClient;

namespace MegaXConfigTool
{
    public partial class MainWindow : MetroWindow
    {
        public bool onlyDebug = false;
        private delegate void UpdateProgressBarDelegate(System.Windows.DependencyProperty dp, Object value);

        public int times_to_fail = 5;
        public int event_clicks = 0;

        public byte[] rx_buffer = new byte[500];

        public int last_event_number = 0;
        public bool RX_ACK = true;
        public uint counter_blocks = 0;
        public uint bytes_left = 0;
        public uint block_complete = 0;
        public uint bytes_end_of_block = 0;
        public uint blocks_written = 0;
        public uint block_incomplete = 0;
        
        public string user_code_password;
        public int ArmMode;

        string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        string version_part = (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 4) + "X";
        string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\V" + (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 4) + "X\\";

        private string AppLocale;
        public int AppRole;
        public string AppDbFile;
        private long FirmwareUpdateSize;

        ZoneTableAdapter databaseDataSetZoneTableAdapter = new ZoneTableAdapter();
        AreaTableAdapter databaseDataSetAreaTableAdapter = new AreaTableAdapter();
        AudioSystemConfigurationTableAdapter databaseDataSetAudioSystemConfigurationTableAdapter = new AudioSystemConfigurationTableAdapter();
        DialerTableAdapter databaseDataSetDialerTableAdapter = new DialerTableAdapter();
        EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
        ExpanderTableAdapter databaseDataSetExpanderTableAdapter = new ExpanderTableAdapter();
        GlobalSystemTableAdapter databaseDataSetGlobalSystemTableAdapter = new GlobalSystemTableAdapter();
        KeypadTableAdapter databaseDataSetKeypadTableAdapter = new KeypadTableAdapter();
        MainInfoTableAdapter databaseDataSetMainInfoTableAdapter = new MainInfoTableAdapter();
        OutputTableAdapter databaseDataSetOutputTableAdapter = new OutputTableAdapter();
        PhoneTableAdapter databaseDataSetPhoneTableAdapter = new PhoneTableAdapter();
        TimezoneTableAdapter databaseDataSetTimezoneTableAdapter = new TimezoneTableAdapter();
        UserTableAdapter databaseDataSetUserTableAdapter = new UserTableAdapter();

        public int intervalsleeptime = 0;
        public int delay_savingtime = 200;
        public short partitionIsArming;
        public bool arming_code_accepted;

        private bool EventFilterArmDisarm = false;
        private bool EventFilterAlarms = false;
        private bool EventFilterFaults = false;
        private bool EventFilterAreas = false;

        public SerialPort serialPort = new SerialPort();
        public byte[] cp_id = { 0x00, 0x00 };
        public byte[] serial_number = { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
        public byte[] hw_version = { 0x00, 0x00, 0x00 };
        public byte[] sw_version = { 0x00, 0x00, 0x00 };
        public defaultDataSet databaseDataSet { get; set; }

        public int FLAG_PARTITION_IS_ARMING = 0;

        byte[] combined_file_data_bytes = new byte[0];
        public uint event_code;
        WaveIn waveSource;
        WaveFileWriter waveFile;
        WaveOut waveOut = new WaveOut();
        DataRowView current_audio_row;
        Dictionary<int, bool> WizardKeypadSetup;
        Dictionary<int, bool> WizardDialerSetup;
        Dictionary<int, bool> WizardZonesSetup;
        Dictionary<int, bool> WizardPhonesSetup;
        Dictionary<int, bool> WizardUsersSetup;
        System.Windows.Threading.DispatcherTimer real_time_timer = new System.Windows.Threading.DispatcherTimer();

        System.Windows.Threading.DispatcherTimer timer_arming = new System.Windows.Threading.DispatcherTimer();

        public System.Windows.Threading.DispatcherTimer serial_port_connection_timer = new System.Windows.Threading.DispatcherTimer();

        AudioTableAdapter databaseDataSetAudioDefaultTableAdapter = new AudioTableAdapter();
        AudioTableAdapter databaseDataSetAudioCustomizedTableAdapter = new AudioTableAdapter();
        FileStream audio_stream;
        
        private bool default_restore_is_set = false;

        string mai_file = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\";
        System.Windows.Forms.Timer timer = null;
        bool audio_system_momory_isfull = false;
        bool audio_system_momory_istrigger = false;

        public MainWindow(string locale, int role, string ChoosenDbFile, Dictionary<int, bool> KeypadConfig, Dictionary<int, bool> DialerConfig, Dictionary<int, bool> ZonesConfig, Dictionary<int, bool> PhonesConfig, Dictionary<int, bool> UsersConfig)
        {
            AppLocale = locale;
            AppRole = role;
            AppDbFile = ChoosenDbFile;
            FirmwareUpdateSize = 0;
            WizardKeypadSetup = KeypadConfig;
            WizardDialerSetup = DialerConfig;
            WizardZonesSetup = ZonesConfig;
            WizardPhonesSetup = PhonesConfig;
            WizardUsersSetup = UsersConfig;
            
            Console.WriteLine("MainWindow: " + AppRole);

            //QueriesTableAdapter("attachdbfilename =| DataDirectory |\\Database\\" + ChoosenDbFile + "; data source = Database\\" + ChoosenDbFile);
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string version_part = (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 4) + "X";
            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\V" + version_part + "\\"; //My documents folder
            QueriesTableAdapter("attachdbfilename =" + configurations_folder + ChoosenDbFile + "; data source = " + configurations_folder + ChoosenDbFile);

            if(event_clicks != 0)
                LoadMoreEvents_button.Visibility = Visibility.Visible;

            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(AppLocale);


            XmlLanguageConverter c = new XmlLanguageConverter();
            XmlLanguage c2 = (XmlLanguage)c.ConvertFrom(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
            this.Resources.Add("GetSystemCulture", c2);

            string audio_path_documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";
            if (!Directory.Exists(audio_path_documents))
                Directory.CreateDirectory(audio_path_documents);

            //SetFullAccessPermissionsForEveryone(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName));

            try
            {
                InitializeComponent();
                this.DataContext = this;

                timer = new System.Windows.Forms.Timer();
                timer.Interval = 2000;
                timer.Tick += new EventHandler(OnTimedEvent);
                timer.Enabled = true;
                timer.Start();

                SoftwareVersion.Content = version;
                System.Diagnostics.Debug.WriteLine("Mega-X Configuration Tool v." + version);

                ConfigFileName.Content = AppDbFile;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); //TODO: delete/improve
            }

            //switch (Properties.Settings.Default.DefaultCulture)
            //{
            //    case "pt-PT":
            //        EN_Active.Visibility = Visibility.Collapsed;
            //        PT_Active.Visibility = Visibility.Visible;
            //        break;

            //    case "en-US":
            //        EN_Active.Visibility = Visibility.Visible;
            //        PT_Active.Visibility = Visibility.Collapsed;
            //        break;

            //    default:
            //        EN_Active.Visibility = Visibility.Visible;
            //        PT_Active.Visibility = Visibility.Collapsed;
            //        break;
            //}
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;

            if (null == itemsSource)
            {
                yield return null;
            }

            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;

                if (null != row)
                {
                    yield return row;
                }
            }
        }

        void OnTimedEvent(object source, EventArgs e)
        {
            long value = GetFolderSize();
            int percente = (int)((value * 100) / 4181216);

            if (percente >= 100)
            {
                audio_system_momory_isfull = true;

                if (audio_system_momory_istrigger == false && audio_system_momory_isfull == true)
                {
                    audio_system_momory_istrigger = true;
                    DialogManager.ShowMessageAsync(this, Properties.Resources.AudioSystemConfigurationSpace, "");
                }
            }
            else
            {
                progressBarAudioMeasure.Value = value;
                lblBarAudioMeasure.Content = percente + "%" + Properties.Resources.FreeSpace;
            }
        }

        public long GetFolderSize()
        {
            DirectoryInfo d = new DirectoryInfo(mai_file);
            FileInfo[] files = d.GetFiles();
            long size = 0;

            foreach (FileInfo item in files)
            {
                if (item.Name != "new_audio_files_unified.mai")
                {
                    size += item.Length;
                }
            }
            return size;
        }

        private void Real_time_Timer_Tick(object sender, EventArgs e)
        {
            //if (StatusPartitionsGrid.Visibility.Equals(Visibility.Visible))
            //    StatusPartitionsGrid.Visibility = Visibility.Collapsed;
            //else
            //    StatusPartitionsGrid.Visibility = Visibility.Visible;

            RealTime real_time = new RealTime();
            real_time_timer.Interval = new TimeSpan(0, 0, 3);

            if (serialPort.IsOpen)
            {
                real_time.read(this);
            }
            else
            {
                //TODO: What should I do if no serial port communication on real time????
            }
        }

        private async void Serial_ConnectionFailed(object sender, EventArgs e)
        {
            serialPort.Close();
            SerialConnectButton.Content = Properties.Resources.Button_Connect;
            ComPortComboBox.IsEnabled = true;

            TextBoxConnectedDisconnected.Text = Properties.Resources.ComDisconnected;
            TextBoxConnectedDisconnected.Foreground = Brushes.Red;
            StatusBarDisconnectedIcon.Visibility = Visibility.Visible;
            StatusBarConnectedIcon.Visibility = Visibility.Collapsed;
            serial_port_connection_timer.Stop();

            PanelLabels.Visibility = Visibility.Collapsed;

            await DialogManager.ShowMessageAsync(this, Properties.Resources.UnknownDevice, "");
        }

        private async void BaseWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            real_time_timer.Tick += new EventHandler(Real_time_Timer_Tick);
            real_time_timer.Interval = new TimeSpan(0, 0, 2);

            serial_port_connection_timer.Tick += new EventHandler(Serial_ConnectionFailed);
            serial_port_connection_timer.Interval = new TimeSpan(0, 0, 2);

            //Serial Port - Default communication labels
            TextBoxConnectedDisconnected.Text = Properties.Resources.ComDisconnected;
            TextBoxConnectedDisconnected.Foreground = Brushes.Red;
            StatusBarDisconnectedIcon.Visibility = Visibility.Visible;
            StatusBarConnectedIcon.Visibility = Visibility.Collapsed;
            //Serial number, FW Version and HW Version labels
            PanelLabels.Visibility = Visibility.Collapsed;

            // Serial Port
            string[] com_ports = SerialPort.GetPortNames();
            ushort i = 0;
            foreach (string com_port in com_ports)
            {
                ComPortComboBox.Items.Add(com_port);
                i++;
            }
            ComPortComboBox.SelectedIndex = i - 1;

            //TREEVIEWGINJO
            #region Populate treview items dinamically
            CreateTreeviewItemsfor(TreeviewZones, Constants.KP_MAX_ZONES, Properties.Resources.Zone);
            CreateTreeviewItemsfor(TreeviewAreas, Constants.KP_MAX_AREAS, Properties.Resources.Area);
            CreateTreeviewItemsfor(TreeviewKeypads, Constants.KP_MAX_KEYPADS, Properties.Resources.Keypad);
            CreateTreeviewItemsfor(TreeviewOutputs, Constants.KP_MAX_OUTPUTS, Properties.Resources.Output);
            CreateTreeviewItemsfor(TreeviewUsers, Constants.KP_MAX_USERS - 5, Properties.Resources.User); //Do not show last 5 [reserved] users
            CreateTreeviewItemsfor(TreeviewTimezones, Constants.KP_MAX_TIMEZONES, Properties.Resources.Timezone);
            CreateTreeviewItemsfor(TreeviewPhones, Constants.KP_MAX_PHONES, Properties.Resources.Phone);
            CreateTreeviewItemsfor(TreeviewExpanders, Constants.KP_MAX_EXPANDERS, Properties.Resources.Expansor);
            #endregion

            //USER
            if (AppRole == 0)
            {
                TopBar_User_Name.Text = Properties.Resources.User_role_admin_user;
                TopBar_User_Image.Source = new BitmapImage(new Uri("/images/login/0_user.png", UriKind.Relative));

                Open_Expanders.Visibility = Visibility.Collapsed;
                Open_Areas.Visibility = Visibility.Collapsed;
                Open_Zones.Visibility = Visibility.Collapsed;
                Open_Keypads.Visibility = Visibility.Collapsed;
                Open_Outputs.Visibility = Visibility.Collapsed;
                Open_Dialer.Visibility = Visibility.Collapsed;
                Open_GlobalConfig.Visibility = Visibility.Collapsed;
                Open_ClientInfo.Visibility = Visibility.Collapsed;
                Open_FirmwareUpdate.Visibility = Visibility.Collapsed;
                Open_Debug.Visibility = Visibility.Collapsed;
                Open_Audio.Visibility = Visibility.Collapsed;
                Open_Events.Visibility = Visibility.Collapsed;
                Open_Memory.Visibility = Visibility.Collapsed;

                TreeviewExpanders.Visibility = Visibility.Collapsed;
                TreeviewAreas.Visibility = Visibility.Collapsed;
                TreeviewZones.Visibility = Visibility.Collapsed;
                TreeviewKeypads.Visibility = Visibility.Collapsed;
                TreeviewOutputs.Visibility = Visibility.Collapsed;
                TreeviewDialer.Visibility = Visibility.Collapsed;
                TreeviewGlobalSystem.Visibility = Visibility.Collapsed;
                TreeviewClient.Visibility = Visibility.Collapsed;
                TreeviewFwUpdate.Visibility = Visibility.Collapsed;
                TreeviewDebug.Visibility = Visibility.Collapsed;
                TreeviewAudioMessages.Visibility = Visibility.Collapsed;
                TreeviewEvents.Visibility = Visibility.Collapsed;
                TreeviewMemory.Visibility = Visibility.Collapsed;

                ReadFWUpdateData.Visibility = Visibility.Collapsed;
                UpdateDateHour.Visibility = Visibility.Collapsed;

                // User_Code_Column.Visibility = Visibility.Collapsed;
                // User_UserCode_Button.Visibility = Visibility.Collapsed;
            }
            //MANUFACTURER
            else if (AppRole == 1)
            {
                TopBar_User_Name.Text = Properties.Resources.User_role_manufacturer;
                TopBar_User_Image.Source = new BitmapImage(new Uri("/images/login/1_manufacturer.png", UriKind.Relative));

            }
            //INSTALLER
            else if (AppRole == 2)
            {
                TopBar_User_Name.Text = Properties.Resources.User_role_installer;
                TopBar_User_Image.Source = new BitmapImage(new Uri("/images/login/2_installer.png", UriKind.Relative));

                TreeviewDebug.Visibility = Visibility.Collapsed;
                Open_Debug.Visibility = Visibility.Collapsed;
                TreeviewMemory.Visibility = Visibility.Collapsed;
                Open_Memory.Visibility = Visibility.Collapsed;

            }

            //Force 'Home' tree view item to be selected on loaded
            TreeViewItem treeview_home = (TreeViewItem)MainTreeView.ItemContainerGenerator.Items[0];
            treeview_home.IsSelected = true;

            waveOut.PlaybackStopped += wavePlayer_PlaybackStopped;

            try
            {
                //Dataset data
                //Initialize all data structures TODO: Put it in a function
                defaultDataSet databaseDataSet = ((defaultDataSet)(this.FindResource("databaseDataSet")));
                this.databaseDataSet = databaseDataSet;

                // ZONE
                databaseDataSetZoneTableAdapter.Fill(databaseDataSet.Zone);
                System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                zoneViewSource.View.MoveCurrentToFirst();

                // AREA
               // AreaTableAdapter databaseDataSetAreaTableAdapter = new AreaTableAdapter();
                databaseDataSetAreaTableAdapter.Fill(databaseDataSet.Area);
                System.Windows.Data.CollectionViewSource AreaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                AreaViewSource.View.MoveCurrentToFirst();

                // USER
               // UserTableAdapter databaseDataSetUserTableAdapter = new UserTableAdapter();
                databaseDataSetUserTableAdapter.Fill(databaseDataSet.User);
                System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                userViewSource.View.MoveCurrentToFirst();

                // KEYPAD
               // KeypadTableAdapter databaseDataSetKeypadTableAdapter = new KeypadTableAdapter();
                databaseDataSetKeypadTableAdapter.Fill(databaseDataSet.Keypad);
                System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                keypadViewSource.View.MoveCurrentToFirst();

                // OUTPUT
                //OutputTableAdapter databaseDataSetOutputTableAdapter = new OutputTableAdapter();
                databaseDataSetOutputTableAdapter.Fill(databaseDataSet.Output);
                System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                outputViewSource.View.MoveCurrentToFirst();

                // TIMEZONE
               // TimezoneTableAdapter databaseDataSetTimezoneTableAdapter = new TimezoneTableAdapter();
                databaseDataSetTimezoneTableAdapter.Fill(databaseDataSet.Timezone);
                System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                timezoneViewSource.View.MoveCurrentToFirst();

                // PHONE
                //PhoneTableAdapter databaseDataSetPhoneTableAdapter = new PhoneTableAdapter();
                databaseDataSetPhoneTableAdapter.Fill(databaseDataSet.Phone);
                System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                phoneViewSource.View.MoveCurrentToFirst();

                // DIALER
                //DialerTableAdapter databaseDataSetDialerTableAdapter = new DialerTableAdapter();
                databaseDataSetDialerTableAdapter.Fill(databaseDataSet.Dialer);
                System.Windows.Data.CollectionViewSource dialerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("dialerViewSource")));
                dialerViewSource.View.MoveCurrentToFirst();

                // GLOBAL SYSTEM
               // GlobalSystemTableAdapter databaseDataSetGlobalSystemTableAdapter = new GlobalSystemTableAdapter();
                databaseDataSetGlobalSystemTableAdapter.Fill(databaseDataSet.GlobalSystem);
                System.Windows.Data.CollectionViewSource globalSystemViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("globalSystemViewSource")));
                globalSystemViewSource.View.MoveCurrentToFirst();

                // CLIENT INFO
                //MainInfoTableAdapter databaseDataSetMainInfoTableAdapter = new MainInfoTableAdapter();
                databaseDataSetMainInfoTableAdapter.Fill(databaseDataSet.MainInfo);
                System.Windows.Data.CollectionViewSource mainInfoViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientInfoViewSource")));
                mainInfoViewSource.View.MoveCurrentToFirst();

                // EXPANDER
                // ExpanderTableAdapter databaseDataSetExpanderTableAdapter = new ExpanderTableAdapter();
                databaseDataSetExpanderTableAdapter.Fill(databaseDataSet.Expander);
                CollectionViewSource expanderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expanderViewSource")));
                keypadViewSource.View.MoveCurrentToFirst();

                //AUDIO
                databaseDataSetAudioDefaultTableAdapter.Fill(databaseDataSet.Audio);
                defaultDataSet.AudioDataTable audio_table = new defaultDataSet.AudioDataTable();
                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + configurations_folder + AppDbFile + ";Password=idsancoprodigy2017"))
                {
                    con.Open();
                    SQLiteCommand cmd = con.CreateCommand();
                    cmd.CommandText = string.Format("SELECT * FROM Audio WHERE Type = 0");
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(audio_table);
                        con.Close();
                    }
                }
                //databaseDataSetAudioDefaultTableAdapter.Fill(audio_table);
                System.Windows.Data.CollectionViewSource AudioDefaultViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioDefaultViewSource")));
                AudioDefaultViewSource.Source = audio_table;
                AudioDefaultViewSource.View.MoveCurrentToFirst();

                //AUDIO CUSTOMIZED
                databaseDataSetAudioCustomizedTableAdapter.Fill(databaseDataSet.Audio);
                defaultDataSet.AudioDataTable audio_customized_table = new defaultDataSet.AudioDataTable();
                using (SQLiteConnection con = new SQLiteConnection("Data Source=" + configurations_folder + AppDbFile + ";Password=idsancoprodigy2017"))
                {
                    con.Open();
                    SQLiteCommand cmd = con.CreateCommand();
                    cmd.CommandText = string.Format("SELECT * FROM Audio WHERE Type = 1");
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        adapter.Fill(audio_customized_table);
                        con.Close();
                    }
                }
                audio_customized_table.TypeColumn.DefaultValue = 1;
                audio_customized_table.VisibleColumn.DefaultValue = 1;

                System.Windows.Data.CollectionViewSource AudioCustomizedViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioCustomizedViewSource")));
                AudioCustomizedViewSource.Source = audio_customized_table;
                AudioCustomizedViewSource.View.MoveCurrentToFirst();
               // databaseDataSetAudioCustomizedTableAdapter.Fill(audio_customized_table);

                DebugTable(audio_customized_table);
                //databaseDataSet.Audio.AcceptChanges();

                //AudioTableAdapter databaseDataSetAudioTableAdapter = new defaultDataSetTableAdapters.AudioTableAdapter();
                System.Windows.Data.CollectionViewSource AudioViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioViewSource")));
                AudioViewSource.View.MoveCurrentToFirst();

                //AUDIO SYSTEM
                //AudioSystemConfigurationTableAdapter databaseDataSetAudioSystemConfigurationTableAdapter = new defaultDataSetTableAdapters.AudioSystemConfigurationTableAdapter();
                databaseDataSetAudioSystemConfigurationTableAdapter.Fill(databaseDataSet.AudioSystemConfiguration);
                System.Windows.Data.CollectionViewSource AudioSystemConfigurationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioSystemConfigurationViewSource")));
                AudioSystemConfigurationViewSource.View.MoveCurrentToFirst();

                //force update of the table after edit one cell
                audio_customized_table.RowDeleted += new DataRowChangeEventHandler(Update_table_after_edit);
                audio_customized_table.RowChanged += new DataRowChangeEventHandler(Update_table_after_edit);

                // Load data into the table Event. You can modify this code as needed.
                //EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
                databaseDataSetEventTableAdapter.Fill(databaseDataSet.Event);
                CollectionViewSource eventViewSource = ((CollectionViewSource)(this.FindResource("eventViewSource")));
                ICollectionView dataView = CollectionViewSource.GetDefaultView(eventDataGrid.ItemsSource);
                this.Dispatcher.Invoke((Action)(() => dataView.SortDescriptions.Clear()));
                this.Dispatcher.Invoke((Action)(() => dataView.SortDescriptions.Add(new SortDescription("EventId", ListSortDirection.Descending))));
                this.Dispatcher.Invoke((Action)(() => dataView.Refresh()));
                eventViewSource.View.MoveCurrentToFirst();

            }
            catch (Exception ex)
            {
                await DialogManager.ShowMessageAsync(this, ex.Message, "");
            }


            if (WizardKeypadSetup != null)
                SetKeypadConfigurationFromWizard(WizardKeypadSetup);

            if (WizardDialerSetup != null)
                SetDialerConfigurationFromWizard(WizardDialerSetup);

            if (WizardZonesSetup != null)
                SetZonesConfigurationFromWizard(WizardZonesSetup);

            if (WizardPhonesSetup != null)
                SetPhonesConfigurationFromWizard(WizardPhonesSetup);

            if (WizardUsersSetup != null)
                SetUsersConfigurationFromWizard(WizardUsersSetup);

        }

        #region WIZARD
        private void SetDialerConfigurationFromWizard(Dictionary<int, bool> wizardDialerSetup)
        {
            databaseDataSet.Dialer.Rows[0]["Active"] = wizardDialerSetup[0];
            //DialerTableAdapter databaseDataSetDialerTableAdapter = new DialerTableAdapter();
            databaseDataSetDialerTableAdapter.Update(databaseDataSet.Dialer);
        }

        private void SetKeypadConfigurationFromWizard(Dictionary<int, bool> keypadConfig)
        {
            for (int i = 0; i < Constants.KP_MAX_KEYPADS; i++)
            {
                databaseDataSet.Keypad.Rows[i]["Active"] = keypadConfig[i];
            }
           // KeypadTableAdapter databaseDataSetKeypadTableAdapter = new KeypadTableAdapter();
            databaseDataSetKeypadTableAdapter.Update(databaseDataSet.Keypad);
        }

        private void SetPhonesConfigurationFromWizard(Dictionary<int, bool> wizardPhonesSetup)
        {
            for (int i = 0; i < Constants.KP_MAX_PHONES; i++)
            {
                databaseDataSet.Phone.Rows[i]["Report on"] = wizardPhonesSetup[i];
            }
            //PhoneTableAdapter databaseDataSetPhoneTableAdapter = new PhoneTableAdapter();
            databaseDataSetPhoneTableAdapter.Update(databaseDataSet.Phone);
        }

        private void SetZonesConfigurationFromWizard(Dictionary<int, bool> wizardZonesSetup)
        {
            for (int i = 0; i < Constants.KP_MAX_ZONES; i++)
            {
                databaseDataSet.Zone.Rows[i]["Zone Active"] = wizardZonesSetup[i];
            }
            //ZoneTableAdapter databaseDataSetZoneTableAdapter = new ZoneTableAdapter();
            databaseDataSetZoneTableAdapter.Update(databaseDataSet.Zone);
        }

        private void SetUsersConfigurationFromWizard(Dictionary<int, bool> wizardUsersSetup)
        {
            for (int i = 0; i < 16; i++)
            {
                databaseDataSet.User.Rows[i]["User active"] = wizardUsersSetup[i];
            }
            //UserTableAdapter databaseDataSetUserTableAdapter = new UserTableAdapter();
            databaseDataSetUserTableAdapter.Update(databaseDataSet.User);
        }
        #endregion

        private void CreateTreeviewItemsfor(TreeViewItem TargetTreeview, short number_of_items, string treeview_item_translation)
        {
            for (int j = 0; j < number_of_items; j++)
            {
                var converter = new System.Windows.Media.BrushConverter();
                var treeview_item = new TreeViewItem();
                treeview_item.Header = treeview_item_translation + (j + 1).ToString();
                treeview_item.Background = (Brush)converter.ConvertFromString("#FF444444");
                treeview_item.Foreground = (Brush)converter.ConvertFromString("#FFFFFBFB");
                TargetTreeview.Items.Add(treeview_item);
            }
        }

        private void ComConfigTile_Click(object sender, RoutedEventArgs e)
        {
            if (this.FlyCom.IsOpen)
            {
                FlyCom.IsOpen = false;
            }
            else
            {
                ushort i = 0;
                int selected_port_index = ComPortComboBox.SelectedIndex;
                ComPortComboBox.Items.Clear();

                foreach (var port in SerialPort.GetPortNames())
                {
                    ComPortComboBox.Items.Add(port);
                    i++;
                }

                if (!serialPort.IsOpen)
                {
                    ComPortComboBox.SelectedIndex = i - 1;
                    SerialConnectButton.Content = Properties.Resources.Button_Connect;
                    ComPortComboBox.IsEnabled = true;
                }
                else
                {
                    ComPortComboBox.SelectedIndex = selected_port_index;

                }

                this.FlyCom.IsOpen = true;
            }
        }

        internal async void RequestDataFromProdigy(List<KeyValuePair<string, bool>> CheckboxesValues)
        {

            Protocol.Zones zones = new Protocol.Zones();
            Protocol.Areas areas = new Protocol.Areas();
            Protocol.Users users = new Protocol.Users();
            Protocol.Keypads keypads = new Protocol.Keypads();
            Protocol.Expanders expanders = new Protocol.Expanders();
            Protocol.Outputs outputs = new Protocol.Outputs();
            Protocol.Timezones timezones = new Protocol.Timezones();
            Protocol.Phones phones = new Protocol.Phones();
            Protocol.GlobalSystem global_system = new Protocol.GlobalSystem();
            Protocol.Dialer dialer = new Protocol.Dialer();
            Protocol.AudioSystemConfiguration audio_system_configuration = new AudioSystemConfiguration();

            var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");
            controller.Maximum = 100.0;
            controller.Minimum = 0.0;

            await Task.Run(() =>
            {
                //DISABLE WINDOW
                //this.Dispatcher.Invoke((Action)(() => this.IsEnabled = false));
                if ((CheckboxesValues.First(kvp => kvp.Key == "Zones").Value.Equals(true)))
                {
                    controller.SetMessage(Properties.Resources.ReadingZones);
                    for (int i = 0; i < Constants.KP_MAX_ZONES; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => zones.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => zones.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => zones.read(this, (uint)(i - 1))));
                        }

                        //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_ZONES + 1.0)))));
                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_ZONES)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "Areas").Value.Equals(true)))
                {
                    controller.SetMessage(Properties.Resources.ReadingPartitions);
                    for (int i = 0; i < Constants.KP_MAX_AREAS; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => areas.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => areas.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => areas.read(this, (uint)(i - 1))));
                        }
                        //msgflag = Constants.MSG_RX_READY;
                        //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_AREAS + 1.0)))));
                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_AREAS)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }
                if ((CheckboxesValues.First(kvp => kvp.Key == "Users").Value.Equals(true)))
                {
                    //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 100));
                    controller.SetMessage(Properties.Resources.ReadingUsers);
                    for (int i = 0; i < Constants.KP_MAX_USERS; i++) //TODO: Chyeck and define number of users
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => users.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => users.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => users.read(this, (uint)(i - 1))));
                        }
                        //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_USERS + 1)))));
                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_USERS)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "Keypads").Value.Equals(true)))
                {
                    //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 100));
                    controller.SetMessage(Properties.Resources.ReadingKeypads);
                    for (int i = 0; i < Constants.KP_MAX_KEYPADS; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => keypads.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => keypads.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => keypads.read(this, (uint)(i - 1))));
                        }

                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_KEYPADS)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }



                if ((CheckboxesValues.First(kvp => kvp.Key == "Outputs").Value.Equals(true)))
                {
                    //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 100));
                    controller.SetMessage(Properties.Resources.ReadingOutputs);
                    for (int i = 0; i < Constants.KP_MAX_OUTPUTS; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => outputs.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => outputs.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => outputs.read(this, (uint)(i - 1))));
                        }
                        //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (Constants.KP_MAX_OUTPUTS + 1.0)))));
                        controller.SetProgress(i * (float)(100.0 / (Constants.KP_MAX_OUTPUTS)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "Timezones").Value.Equals(true)))
                {
                    //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 100));
                    controller.SetMessage(Properties.Resources.ReadingTimezones);
                    for (int i = 0; i < Constants.KP_MAX_TIMEZONES; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => timezones.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => timezones.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => timezones.read(this, (uint)(i - 1))));
                        }
                        //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_TIMEZONES + 1.0)))));
                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_TIMEZONES + 1.0)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "Phones").Value.Equals(true)))
                {
                    //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 100));
                    controller.SetMessage(Properties.Resources.ReadingPhones);
                    for (int i = 0; i < Constants.KP_MAX_PHONES; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => phones.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => phones.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => phones.read(this, (uint)(i - 1))));
                        }
                        //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_PHONES + 1.0)))));
                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_PHONES + 1.0)));
                        //System.Threading.Thread.Sleep(intervalsleeptime);

                    }
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "System").Value.Equals(true)))
                {
                    controller.SetMessage(Properties.Resources.ReadingGlobal);
                    if (RX_ACK)
                        this.Dispatcher.Invoke((Action)(() => global_system.read(this, 0)));
                    else
                    {
                        int count = 0;
                        while (!RX_ACK)
                        {
                            count++;
                            if (count == 10)
                                RX_ACK = true;
                        }
                        this.Dispatcher.Invoke((Action)(() => global_system.read(this, 0)));
                    }
                    controller.SetProgress(100);
                    //System.Threading.Thread.Sleep(intervalsleeptime);
                    //for (int i = 1; i < (Constants.KP_GLOBAL_SYSTEM_DIV + 1); i++)
                    //{
                    //    this.Dispatcher.Invoke((Action)(() => global_system.read(this, (uint)i)));
                    //    //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_KEYPADS + 1.0)))));
                    //    controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_GLOBAL_SYSTEM_DIV + 1.0)));
                    //    System.Threading.Thread.Sleep(intervalsleeptime);
                    //}
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "Dialer").Value.Equals(true)))
                {

                    controller.SetMessage(Properties.Resources.ReadingDialer);
                    if (RX_ACK)
                        this.Dispatcher.Invoke((Action)(() => dialer.read(this, 0)));
                    else
                    {
                        int count = 0;
                        while (!RX_ACK)
                        {
                            count++;
                            if (count == 10)
                                RX_ACK = true;
                        }
                        this.Dispatcher.Invoke((Action)(() => dialer.read(this, 0)));
                    }
                    controller.SetProgress(100);
                    //System.Threading.Thread.Sleep(intervalsleeptime);
                    //for (int i = 1; i < (Constants.KP_GLOBAL_SYSTEM_DIV + 1); i++)
                    //{
                    //    this.Dispatcher.Invoke((Action)(() => dialer.read(this, (uint)i)));
                    //    //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_KEYPADS + 1.0)))));
                    //    controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_GLOBAL_SYSTEM_DIV + 1.0)));
                    //    System.Threading.Thread.Sleep(intervalsleeptime);
                    //}
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "AudioSystemConfiguration").Value.Equals(true)))
                {
                    controller.SetMessage(Properties.Resources.ReadingAudioSystemConfigurations);
                    if (RX_ACK)
                        this.Dispatcher.Invoke((Action)(() => audio_system_configuration.read(this, 0)));
                    else
                    {
                        int count = 0;
                        while (!RX_ACK)
                        {
                            count++;
                            if (count == 10)
                                RX_ACK = true;
                        }
                        this.Dispatcher.Invoke((Action)(() => audio_system_configuration.read(this, 0)));
                    }
                    //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(100)));                        
                    controller.SetProgress(100);
                        //System.Threading.Thread.Sleep(intervalsleeptime);
                    
                }

                if ((CheckboxesValues.First(kvp => kvp.Key == "Expanders").Value.Equals(true)))
                {
                    //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 100));
                    controller.SetMessage(Properties.Resources.ReadingExpanders);
                    for (int i = 0; i < Constants.KP_MAX_EXPANDERS; i++)
                    {
                        if (RX_ACK)
                            this.Dispatcher.Invoke((Action)(() => expanders.read(this, (uint)i)));
                        else
                        {
                            int count = 0;
                            while (!RX_ACK)
                            {
                                count++;
                                if (count == 10)
                                    RX_ACK = true;
                            }
                            if (i == 1)
                                this.Dispatcher.Invoke((Action)(() => expanders.read(this, 1)));
                            else
                                this.Dispatcher.Invoke((Action)(() => expanders.read(this, (uint)(i - 1))));
                        }

                        controller.SetProgress(i * (float)(100.0 / (float)(Constants.KP_MAX_EXPANDERS + 1.0)));
                        // System.Threading.Thread.Sleep(intervalsleeptime);
                    }
                }

                //ENABLE WINDOW             
                //this.Dispatcher.Invoke((Action)(() => this.IsEnabled = true));
                if (controller.IsOpen)
                {
                    controller.CloseAsync();
                }

                //MessageBox.Show(Properties.Resources.ReadWithSuccess, "", MessageBoxButton.OK, MessageBoxImage.Information);
                //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 0));

            });
            await DialogManager.ShowMessageAsync(this, Properties.Resources.ReadWithSuccess, "");
        }

        internal async void SendDataToProdigy(List<KeyValuePair<string, bool>> CheckboxesValues)
        {
            double[] data_to_send = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            double[] data_sent = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            Protocol.Event events = new Protocol.Event();
            Protocol.Zones zones = new Protocol.Zones();
            Protocol.Areas areas = new Protocol.Areas();
            Protocol.Users users = new Protocol.Users();
            Protocol.Keypads keypads = new Protocol.Keypads();
            Protocol.Expanders expanders = new Protocol.Expanders();
            Protocol.Outputs outputs = new Protocol.Outputs();
            Protocol.Timezones timezones = new Protocol.Timezones();
            Protocol.Phones phones = new Protocol.Phones();
            Protocol.GlobalSystem global_system = new Protocol.GlobalSystem();
            Protocol.Dialer dialer = new Protocol.Dialer();
            Protocol.AudioSystemConfiguration audio_system_configuration = new AudioSystemConfiguration();
            Protocol.General protocol = new General();


            int send_msg_error = 0;
            int error_counter = 0;


            var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");
            controller.Maximum = 100.0;
            controller.Minimum = 0.0;

            await Task.Run(() =>
            {

                //ZONES
                if ((CheckboxesValues.First(kvp => kvp.Key == "Zones").Value.Equals(true)))
                {
                    data_to_send[0] = 1;
                    data_sent[0] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingZones);

                    for (int i = 0; i < Constants.KP_MAX_ZONES; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => zones.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => zones.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => zones.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_ZONES));
                    }
                    if (send_msg_error == 0)
                        data_sent[0] = 1;

                    System.Threading.Thread.Sleep(1000);
                }

                //AREAS
                if ((CheckboxesValues.First(kvp => kvp.Key == "Areas").Value.Equals(true)))
                {
                    data_to_send[1] = 1;
                    data_sent[1] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingPartitions);

                    for (int i = 0; i < Constants.KP_MAX_AREAS; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => areas.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => areas.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => areas.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_AREAS));
                    }
                    if (send_msg_error == 0)
                        data_sent[1] = 1;

                    System.Threading.Thread.Sleep(1000);
                }
                //USERS
                if ((CheckboxesValues.First(kvp => kvp.Key == "Users").Value.Equals(true)))
                {
                    data_to_send[2] = 1;
                    data_sent[2] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingUsers);

                    for (int i = 0; i < Constants.KP_MAX_USERS + 3; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => users.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => users.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => users.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / (Constants.KP_MAX_USERS)));
                    }

                    if (send_msg_error == 0)
                        data_sent[2] = 1;

                    System.Threading.Thread.Sleep(1000);
                }
                //KEYPADS
                if ((CheckboxesValues.First(kvp => kvp.Key == "Keypads").Value.Equals(true)))
                {
                    data_to_send[3] = 1;
                    data_sent[3] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingKeypads);
                    for (int i = 0; i < Constants.KP_MAX_KEYPADS; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => keypads.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => keypads.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => keypads.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_KEYPADS));
                    }
                    if (send_msg_error == 0)
                        data_sent[3] = 1;

                    System.Threading.Thread.Sleep(1000);
                }

                //OUTPUTS
                if ((CheckboxesValues.First(kvp => kvp.Key == "Outputs").Value.Equals(true)))
                {
                    data_to_send[4] = 1;
                    data_sent[4] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingOutputs);

                    for (int i = 0; i < Constants.KP_MAX_OUTPUTS + 3; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => outputs.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => outputs.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => outputs.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_OUTPUTS));
                    }
                    if (send_msg_error == 0)
                        data_sent[4] = 1;

                    System.Threading.Thread.Sleep(1000);

                }
                //TIMEZONES
                if ((CheckboxesValues.First(kvp => kvp.Key == "Timezones").Value.Equals(true)))
                {
                    data_to_send[5] = 1;
                    data_sent[5] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingTimezones);

                    for (int i = 0; i < Constants.KP_MAX_TIMEZONES; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => timezones.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => timezones.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => timezones.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_TIMEZONES));
                    }
                    if (send_msg_error == 0)
                        data_sent[5] = 1;

                    System.Threading.Thread.Sleep(1000);
                }
                //PHONES
                if ((CheckboxesValues.First(kvp => kvp.Key == "Phones").Value.Equals(true)))
                {
                    data_to_send[6] = 1;
                    data_sent[6] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingPhones);

                    for (int i = 0; i < Constants.KP_MAX_PHONES; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => phones.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => phones.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => phones.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_PHONES));
                    }
                    if (send_msg_error == 0)
                        data_sent[6] = 1;

                    System.Threading.Thread.Sleep(1000);
                }
                //GLOBAL SYSTEM
                if ((CheckboxesValues.First(kvp => kvp.Key == "System").Value.Equals(true)))
                {
                    data_to_send[7] = 1;
                    data_sent[7] = 0;
                    send_msg_error = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingGlobal);

                    if (RX_ACK)
                    {
                        error_counter = 0;
                        this.Dispatcher.Invoke((Action)(() => global_system.Write(this, 0)));
                    }
                    else
                    {
                        error_counter++;
                        RX_ACK = true;
                        this.Dispatcher.Invoke((Action)(() => global_system.Write(this, 0)));
                        counter_blocks--;

                    }
                    if (error_counter == times_to_fail)
                    {
                        send_msg_error = 1;
                    }
                    else
                        controller.SetProgress(100);

                    if (send_msg_error == 0)
                        data_sent[7] = 1;

                    System.Threading.Thread.Sleep(1000);
                    //System.Threading.Thread.Sleep(intervalsleeptime);
                    //for (int i = 1; i < (Constants.KP_GLOBAL_SYSTEM_DIV + 1); i++)
                    //{
                    //    this.Dispatcher.Invoke((Action)(() => global_system.Write(this, (uint)i)));
                    //    //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (100 / (Constants.KP_MAX_ZONES + 1)))));
                    //    controller.SetProgress(i * (100 / (Constants.KP_GLOBAL_SYSTEM_DIV + 1)));
                    //    System.Threading.Thread.Sleep(intervalsleeptime);
                    //}
                }
                //Dialer
                if ((CheckboxesValues.First(kvp => kvp.Key == "Dialer").Value.Equals(true)))
                {
                    data_to_send[8] = 1;
                    data_sent[8] = 0;
                    send_msg_error = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingDialer);

                    if (RX_ACK)
                    {
                        error_counter = 0;
                        this.Dispatcher.Invoke((Action)(() => dialer.Write(this, 0)));
                    }
                    else
                    {
                        counter_blocks--;
                        error_counter++;
                        this.Dispatcher.Invoke((Action)(() => dialer.Write(this, 0)));
                    }
                    if (error_counter == times_to_fail)
                    {
                        send_msg_error = 1;
                    }
                    else
                        controller.SetProgress(100);

                    if (send_msg_error == 0)
                        data_sent[8] = 1;

                    System.Threading.Thread.Sleep(1000);

                }


                //AUDIO SYSTEM CONFIG
                if ((CheckboxesValues.First(kvp => kvp.Key == "AudioSystemConfiguration").Value.Equals(true)))
                {
                    data_to_send[9] = 1;
                    data_sent[9] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingAudioConfig);

                    int audio_configs_count = AudioConfigDataGrid.Items.Count - 1;

                    for (int i = 0; i < audio_configs_count; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => audio_system_configuration.write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => audio_system_configuration.write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => audio_system_configuration.write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / audio_configs_count));// Constants.KP_MAX_AUDIO_SYSTEM_CONFIGURATION));
                    }
                    Audio audio = new Protocol.Audio();
                    uint size = Constants.KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_FIM - Constants.KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_INICIO;

                    audio.write_block(this, Constants.KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_INICIO, size);

                    if (send_msg_error == 0)
                        data_sent[9] = 1;

                    System.Threading.Thread.Sleep(1000);
                }

                //EXPANDERS
                if ((CheckboxesValues.First(kvp => kvp.Key == "Expanders").Value.Equals(true)))
                {
                    data_to_send[10] = 1;
                    data_sent[10] = 0;
                    send_msg_error = 0;
                    error_counter = 0;
                    counter_blocks = 0;
                    controller.SetMessage(Properties.Resources.WritingExpanders);

                    for (int i = 0; i < Constants.KP_MAX_EXPANDERS; i++)
                    {
                        if (RX_ACK)
                        {
                            error_counter = 0;
                            this.Dispatcher.Invoke((Action)(() => expanders.Write(this, (uint)i)));
                        }
                        else
                        {
                            error_counter++;
                            counter_blocks--;

                            if (i == 0)
                                this.Dispatcher.Invoke((Action)(() => expanders.Write(this, 0)));
                            else
                            {
                                i--;
                                this.Dispatcher.Invoke((Action)(() => expanders.Write(this, (uint)(i))));
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            controller.SetProgress(i * (100 / Constants.KP_MAX_EXPANDERS));
                    }
                    if (send_msg_error == 0)
                        data_sent[10] = 1;

                    System.Threading.Thread.Sleep(1000);
                }

                controller.CloseAsync();
            });

            if (send_msg_error == 1)
                await DialogManager.ShowMessageAsync(this, Properties.Resources.ErrorWhileWritting, "");
            else
                await DialogManager.ShowMessageAsync(this, Properties.Resources.WriteWithSuccess, "");

            System.Threading.Thread.Sleep(500);

            Report report = new Report(this, data_sent, data_to_send);
            report.Show();
        }

        private async void SerialConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (ComPortIsRemote.IsChecked == true)
            {
                delay_savingtime = 1500;
                intervalsleeptime = 500;
                times_to_fail = 10;
            }
            else
            {
                delay_savingtime = 200;
                intervalsleeptime = 100;
                times_to_fail = 3;
            }

            //msgflag = Constants.MSG_TX_READY;

            // Connect to Serial Port
            try
            {
                if (!serialPort.IsOpen)
                {
                    serialPort.PortName = ComPortComboBox.SelectedItem.ToString();
                    serialPort.BaudRate = 115200;
                    serialPort.Parity = Parity.None;
                    serialPort.DataBits = 8;
                    serialPort.StopBits = StopBits.One;
                    serialPort.Handshake = Handshake.None;

                    try
                    {
                        serialPort.DataReceived += new SerialDataReceivedEventHandler(SerialPort_DataReceived);
                        serialPort.Open();
                        SerialConnectButton.Content = Properties.Resources.Button_Disconnect;
                        ComPortComboBox.IsEnabled = false;

                        cp_id[0] = 0x00;
                        cp_id[1] = 0x00;
                        Protocol.General protocol = new Protocol.General();
                        protocol.check_ID(this);

                        serial_port_connection_timer.Start();

                        PanelLabels.Visibility = Visibility.Visible;
                        //TextBoxConnectedDisconnected.Text = Properties.Resources.ComConnected;
                        //TextBoxConnectedDisconnected.Foreground = Brushes.Green;
                        //StatusBarDisconnectedIcon.Visibility = Visibility.Collapsed;
                        //StatusBarConnectedIcon.Visibility = Visibility.Visible;

                        ////MessageBox.Show(Properties.Resources.Connection_Successful, "Message", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve 
                        //await DialogManager.ShowMessageAsync(this, Properties.Resources.Connection_Successful, "");
                    }
                    catch (Exception ex)
                    {
                        await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    }
                }
                else
                {
                    try
                    {
                        serialPort.Close();
                        SerialConnectButton.Content = Properties.Resources.Button_Connect;
                        ComPortComboBox.IsEnabled = true;

                        TextBoxConnectedDisconnected.Text = Properties.Resources.ComDisconnected;
                        TextBoxConnectedDisconnected.Foreground = Brushes.Red;
                        StatusBarDisconnectedIcon.Visibility = Visibility.Visible;
                        StatusBarConnectedIcon.Visibility = Visibility.Collapsed;

                        PanelLabels.Visibility = Visibility.Collapsed;

                        await DialogManager.ShowMessageAsync(this, Properties.Resources.Disconnection_Successfull, "");
                    }
                    catch (Exception ex)
                    {
                        await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    }
                    FlyCom.IsOpen = false;
                }
            }
            catch (Exception ex)
            {
                await DialogManager.ShowMessageAsync(this, ex.Message, "");
                //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            
            Protocol.StateMachine state_machine = new Protocol.StateMachine(); // Create state machine to handle with received data

            // State machine
            while (serialPort.BytesToRead > 0)
            {
                serialPort.Read(rx_buffer, 0, rx_buffer.Length);
                string DataRX = BitConverter.ToString(rx_buffer);
                System.Diagnostics.Debug.WriteLine("DATA RX: " + DataRX);
                if (state_machine.ValidateData(rx_buffer))
                {
                    RX_ACK = true;
                    if(!onlyDebug)
                        state_machine.Operations(state_machine.data_rx[2], this, state_machine.state_machine_data_lenght_temp);
                    System.Diagnostics.Debug.WriteLine(" RX ACK: " + RX_ACK);
                    System.Diagnostics.Debug.WriteLine("");
                }
                else
                {
                    RX_ACK = false;
                    System.Diagnostics.Debug.WriteLine("RX ACK: " + RX_ACK);
                    System.Diagnostics.Debug.WriteLine("");
                }
            
                System.Threading.Thread.Sleep(50);

            }
        }

        public async void send_serial_port_data(byte[] tx_buffer, int offset, int count)
        {
            try
            {
                if (serialPort.IsOpen)
                {
                    serialPort.Write(tx_buffer, offset, count);
                    string DataTX = BitConverter.ToString(tx_buffer);
                    System.Diagnostics.Debug.WriteLine("DATA TX: " + DataTX);
                    //updateDebugTextBox(tx_buffer, count, 1); //test purposes only
                }
                else
                    await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
            }
            catch (Exception ex)
            {
                real_time_timer.Stop();
                await DialogManager.ShowMessageAsync(this, ex.Message, "");
            }
        }

        internal void write_control_panel_id_label(int control_panel_id, string hardware_version, string software_version, string serial_number)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                //ControlPanelIDLabel.Text = control_panel_id.ToString();
                //BoardSerialNumber.Text = serial_number.ToString();
                //HwVersion.Text = hardware_version;
                //SwVersion.Text = software_version;

                //ControlPanelIDLabel.Text = control_panel_id.ToString();
                SerialNumberValueLabel.Content = serial_number;
                HWNumberValueLabel.Content = hardware_version;
                FWNumberValueLabel.Content = software_version;

            }));
        }

        private void MainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var tree = sender as TreeView;

            if (tree.SelectedItem is TreeViewItem)
            {
                #region HomePage
                //Home page
                var var_main = MainTreeView.ItemContainerGenerator.Items[0] as TreeViewItem;
                if (e.NewValue == var_main)
                {
                    MainTabControl.SelectedItem = MainHomeTab;
                }
                #endregion

                #region Areas
                // Areas                
                var a = MainTreeView.ItemContainerGenerator.Items[1] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainAreasTab;
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[0]) //Area 1
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(0);

                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[1]) // Area 2
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[2]) // Area 3
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[3]) // Area 4
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[4]) // Area 5
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[5]) // Area 6
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[6]) // Area 7
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[7]) // Area 8
                {
                    MainTabControl.SelectedItem = MainAreaPVTTab;
                    System.Windows.Data.CollectionViewSource areaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                    areaViewSource.View.MoveCurrentToPosition(7);
                }
                #endregion

                #region Zones
                // Zones
                var var_zone = MainTreeView.ItemContainerGenerator.Items[2] as TreeViewItem;
                if (e.NewValue == var_zone)
                {
                    MainTabControl.SelectedItem = MainZonesTab;
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[0]) // Zone 1
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(0);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[1]) // Zone 2
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[2]) // Zone 3
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[3]) // Zone 4
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[4]) // Zone 5
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[5]) // Zone 6
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[6]) // Zone 7
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[7]) // Zone 8
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(7);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[8]) // Zone 9
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(8);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[9]) // Zone 10
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(9);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[10]) // Zone 11
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(10);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[11]) // Zone 12
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(11);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[12]) // Zone 13
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(12);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[13]) // Zone 14
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(13);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[14]) // Zone 15
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(14);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[15]) // Zone 16
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(15);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[16]) // Zone 17
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(16);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[17]) // Zone 18
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(17);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[18]) // Zone 19
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(18);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[19]) // Zone 20
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(19);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[20]) // Zone 21
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(20);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[21]) // Zone 22
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(21);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[22]) // Zone 23
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(22);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[23]) // Zone 24
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(23);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[24]) // Zone 25
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(24);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[25]) // Zone 26
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(25);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[26]) // Zone 27
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(26);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[27]) // Zone 28
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(27);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[28]) // Zone 29
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(28);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[29]) // Zone 30
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(29);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[30]) // Zone 31
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(30);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[31]) // Zone 32
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(31);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[32]) // Zone 1
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(32);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[33]) // Zone 2
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(33);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[34]) // Zone 3
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(34);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[35]) // Zone 4
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(35);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[36]) // Zone 5
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(36);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[37]) // Zone 6
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(37);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[38]) // Zone 7
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(38);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[39]) // Zone 8
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(39);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[40]) // Zone 9
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(40);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[41]) // Zone 10
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(41);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[42]) // Zone 11
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(42);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[43]) // Zone 12
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(43);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[44]) // Zone 13
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(44);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[45]) // Zone 14
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(45);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[46]) // Zone 15
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(46);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[47]) // Zone 16
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(47);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[48]) // Zone 17
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(48);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[49]) // Zone 18
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(49);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[50]) // Zone 19
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(50);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[51]) // Zone 20
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(51);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[52]) // Zone 21
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(52);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[53]) // Zone 22
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(53);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[54]) // Zone 23
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(54);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[55]) // Zone 24
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(55);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[56]) // Zone 25
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(56);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[57]) // Zone 26
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(57);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[58]) // Zone 27
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(58);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[59]) // Zone 28
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(59);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[60]) // Zone 29
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(60);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[61]) // Zone 30
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(61);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[62]) // Zone 31
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(62);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[63]) // Zone 32
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(63);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[64]) // Zone 1
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(64);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[65]) // Zone 2
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(65);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[66]) // Zone 3
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(66);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[67]) // Zone 4
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(67);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[68]) // Zone 5
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(68);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[69]) // Zone 6
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(69);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[70]) // Zone 7
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(70);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[71]) // Zone 8
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(71);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[72]) // Zone 9
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(72);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[73]) // Zone 10
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(73);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[74]) // Zone 11
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(74);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[75]) // Zone 12
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(75);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[76]) // Zone 13
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(76);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[77]) // Zone 14
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(77);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[78]) // Zone 15
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(78);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[79]) // Zone 16
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(79);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[80]) // Zone 17
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(80);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[81]) // Zone 18
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(81);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[82]) // Zone 19
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(82);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[83]) // Zone 20
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(83);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[84]) // Zone 21
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(84);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[85]) // Zone 22
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(85);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[86]) // Zone 23
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(86);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[87]) // Zone 24
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(87);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[88]) // Zone 25
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(8);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[89]) // Zone 26
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(89);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[90]) // Zone 27
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(90);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[91]) // Zone 28
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(91);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[92]) // Zone 29
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(92);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[93]) // Zone 30
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(93);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[94]) // Zone 31
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(94);
                }
                else if (e.NewValue == var_zone.ItemContainerGenerator.Items[95]) // Zone 32
                {
                    MainTabControl.SelectedItem = MainZonePVTTab;
                    System.Windows.Data.CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                    zoneViewSource.View.MoveCurrentToPosition(95);
                }

                #endregion

                #region Users
                // Users
                var var_user = MainTreeView.ItemContainerGenerator.Items[6] as TreeViewItem;

                if (e.NewValue == var_user)
                {
                    MainTabControl.SelectedItem = MainUsersTab;
                }

                else if (e.NewValue == var_user.ItemContainerGenerator.Items[0]) // User 1
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(0);
                }

                else if (e.NewValue == var_user.ItemContainerGenerator.Items[1]) // User 1
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(1);
                }

                else if (e.NewValue == var_user.ItemContainerGenerator.Items[2]) // User 2
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[3]) // User 3
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[4]) // User 4
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[5]) // User 5
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[6]) // User 6
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[7]) // User 7
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(7);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[8]) // User 8
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(8);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[9]) // User 9
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(9);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[10]) // User 10
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(10);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[11]) // User 11
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(11);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[12]) // User 12
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(12);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[13]) // User 13
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(13);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[14]) // User 14
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(14);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[15]) // User 15
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(15);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[16]) // User 16
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(16);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[17]) // User 17
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(17);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[18]) // User 18
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(18);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[19]) // User 19
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(19);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[20]) // User 20
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(20);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[21]) // User 21
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(21);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[22]) // User 22
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(22);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[23]) // User 23
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(23);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[24]) // User 24
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(24);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[25]) // User 25
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(25);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[26]) // User 26
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(26);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[27]) // User 27
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(27);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[28]) // User 28
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(28);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[29]) // User 29
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(29);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[30]) // User 30
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(30);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[31]) // User 31
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(31);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[32]) // User 32
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(32);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[33]) // User 33
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(33);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[34]) // User 34
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(34);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[35]) // User 35
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(35);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[36]) // User 36
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(36);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[37]) // User 37
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(37);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[38]) // User 38
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(38);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[39]) // User 39
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(39);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[40]) // User 40
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(40);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[41]) // User 41
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(41);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[42]) // User 42
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(42);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[43]) // User 43
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(43);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[44]) // User 44
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(44);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[45]) // User45
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(45);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[46]) // User 46
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(46);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[47]) // User 47
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(47);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[48]) // User 48
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(48);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[49]) // User 49
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(49);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[50]) // User 50
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(50);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[51]) // User 51
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(51);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[52]) // User 52
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(52);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[53]) // User 53
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(53);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[54]) // User 54
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(54);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[55]) // User 55
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(55);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[56]) // User 56
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(56);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[57]) // User 57
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(57);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[58]) // User 58
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(58);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[59]) // User 59
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(59);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[60]) // User 60
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(60);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[61]) // User 61
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(61);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[62]) // User 62
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(62);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[63]) // User 63
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(63);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[64]) // User 64
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(64);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[65]) // User 65
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(65);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[66]) // User 66
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(66);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[67]) // User 67
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(67);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[68]) // User 68
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(68);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[69]) // User 69
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(69);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[70]) // User 70
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(70);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[71]) // User 71
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(71);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[72]) // User 72
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(72);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[73]) // User 73
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(73);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[74]) // User 74
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(74);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[75]) // User 75
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(75);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[76]) // User 76
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(76);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[77]) // User 77
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(77);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[78]) // User 78
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(78);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[79]) // User 79
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(79);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[80]) // User 80
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(80);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[81]) // User 81
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(81);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[82]) // User 82
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(82);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[83]) // User 83
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(83);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[84]) // User 84
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(84);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[85]) // User 85
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(85);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[86]) // User 86
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(86);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[87]) // User 87
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(87);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[88]) // User 88
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(88);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[89]) // User 89
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(89);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[90]) // User 90
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(90);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[91]) // User 91
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(91);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[92]) // User 92
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(92);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[93]) // User 93
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(93);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[94]) // User 94
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(94);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[95]) // User 95
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(95);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[96]) // User 96
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(96);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[97]) // User 97
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(97);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[98]) // User 98
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(98);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[99]) // User 99
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(99);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[100]) // User 100
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(100);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[101]) // User 101
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(101);
                }

                else if (e.NewValue == var_user.ItemContainerGenerator.Items[102]) // User 102
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(102);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[103]) // User 103
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(103);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[104]) // User 104
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(104);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[105]) // User 105
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(105);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[106]) // User 106
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(106);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[107]) // User 107
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(107);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[108]) // User 108
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(108);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[109]) // User 109
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(109);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[110]) // User 110
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(110);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[111]) // User 111
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(111);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[112]) // User 112
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(112);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[113]) // User 113
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(113);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[114]) // User 114
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(114);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[115]) // User 115
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(115);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[116]) // User 116
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(116);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[117]) // User 117
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(117);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[118]) // User 118
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(118);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[119]) // User 119
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(119);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[120]) // User 120
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(120);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[121]) // User 121
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(121);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[122]) // User 122
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(122);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[123]) // User 123
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(123);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[124]) // User 124
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(124);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[125]) // User 125
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(125);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[126]) // User 126
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(126);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[127]) // User 127
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(127);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[128]) // User 128
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(128);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[129]) // User 129
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(129);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[130]) // User 130
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(130);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[131]) // User 131
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(131);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[132]) // User 132
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(132);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[133]) // User 133
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(133);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[134]) // User 134
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(134);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[135]) // User 135
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(135);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[136]) // User 136
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(136);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[137]) // User 137
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(137);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[138]) // User 138
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(138);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[139]) // User 139
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(139);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[140]) // User 140
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(140);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[141]) // User 141
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(141);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[142]) // User 142
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(142);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[143]) // User 143
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(143);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[144]) // User 144
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(144);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[145]) // User145
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(145);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[146]) // User 146
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(146);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[147]) // User 147
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(147);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[148]) // User 148
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(148);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[149]) // User 149
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(149);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[150]) // User 150
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(150);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[151]) // User 151
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(151);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[152]) // User 152
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(152);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[153]) // User 153
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(153);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[154]) // User 154
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(154);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[155]) // User 155
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(155);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[156]) // User 156
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(156);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[157]) // User 157
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(157);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[158]) // User 158
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(158);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[159]) // User 159
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(159);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[160]) // User 160
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(160);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[161]) // User 161
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(161);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[162]) // User 162
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(162);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[163]) // User 163
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(163);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[164]) // User 164
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(164);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[165]) // User 165
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(165);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[166]) // User 166
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(166);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[167]) // User 167
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(167);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[168]) // User 168
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(168);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[169]) // User 169
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(169);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[170]) // User 170
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(170);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[171]) // User 171
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(171);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[172]) // User 172
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(172);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[173]) // User 173
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(173);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[174]) // User 174
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(174);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[175]) // User 175
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(175);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[176]) // User 176
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(176);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[177]) // User 177
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(177);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[178]) // User 178
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(178);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[179]) // User 179
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(179);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[180]) // User 180
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(180);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[181]) // User 181
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(181);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[182]) // User 182
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(182);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[183]) // User 183
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(183);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[184]) // User 184
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(184);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[185]) // User 185
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(185);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[186]) // User 186
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(186);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[187]) // User 187
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(187);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[188]) // User 188
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(188);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[189]) // User 189
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(189);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[190]) // User 190
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(190);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[191]) // User 191
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(191);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[192]) // User 192
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(192);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[193]) // User 193
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(193);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[194]) // User 194
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(194);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[195]) // User 195
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(195);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[196]) // User 196
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(196);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[197]) // User 197
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(197);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[198]) // User 198
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(198);
                }
                else if (e.NewValue == var_user.ItemContainerGenerator.Items[199]) // User 199
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));
                    userViewSource.View.MoveCurrentToPosition(199);
                }
                #endregion

                #region Keypads
                // Keypads
                a = MainTreeView.ItemContainerGenerator.Items[3] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainKeypadsTab;
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[0]) // Keypad 1
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(0);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[1]) // Keypad 2
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[2]) // Keypad 3
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[3]) // Keypad 4
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[4]) // Keypad 5
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[5]) // Keypad 6
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[6]) // Keypad 7
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[7]) // Keypad 8
                {
                    MainTabControl.SelectedItem = MainKeypadsPVTTab;
                    System.Windows.Data.CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                    keypadViewSource.View.MoveCurrentToPosition(7);
                }
                #endregion

                #region Outputs
                // Outputs
                a = MainTreeView.ItemContainerGenerator.Items[4] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainOutputsTab;
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[0]) // Output 1
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(0);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[1]) // Output 2
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[2]) // Output 3
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[3]) // Output 4
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[4]) // Output 5
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[5]) // Output 6
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[6]) // Output 7
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[7]) // Output 8
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(7);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[8]) // Output 9
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(8);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[9]) // Output 10
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(9);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[10]) // Output 11
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(10);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[11]) // Output 12
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(11);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[12]) // Output 13
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(12);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[13]) // Output 1
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(13);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[14]) // Output 2
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(14);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[15]) // Output 3
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(15);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[16]) // Output 4
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(16);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[17]) // Output 5
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(17);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[18]) // Output 6
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(18);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[19]) // Output 7
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(19);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[20]) // Output 8
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(20);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[21]) // Output 9
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(21);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[22]) // Output 10
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(22);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[23]) // Output 11
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(23);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[24]) // Output 12
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(24);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[25]) // Output 13
                {
                    MainTabControl.SelectedItem = MainOutputsPVTTab;
                    System.Windows.Data.CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                    outputViewSource.View.MoveCurrentToPosition(25);
                }

                #endregion

                #region Timezones
                // Timezones
                a = MainTreeView.ItemContainerGenerator.Items[5] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainTimezonesTab;
                }

                else if (e.NewValue == a.ItemContainerGenerator.Items[0]) // Timezone 1
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(0);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[1]) // Timezone 2
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[2]) // Timezone 3
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[3]) // Timezone 4
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[4]) // Timezone 5
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[5]) // Timezone 6
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[6]) // Timezone 7
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[7]) // Timezone 8
                {
                    MainTabControl.SelectedItem = MainTimezonesPVTTab;
                    System.Windows.Data.CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                    timezoneViewSource.View.MoveCurrentToPosition(7);
                }
                #endregion

                #region Phones

                // Phones
                a = MainTreeView.ItemContainerGenerator.Items[7] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainPhonesTab;
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[0]) // Phone 1
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(0);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[1]) // Phone 2
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[2]) // Phone 3
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[3]) // Phone 4
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(3);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[4]) // Phone 5
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(4);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[5]) // Phone 6
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(5);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[6]) // Phone 7
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(6);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[7]) // Phone 8
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(7);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[8]) // Phone 9
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(8);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[9]) // Phone 10
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(9);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[10]) // Phone 11
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(10);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[11]) // Phone 12
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(11);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[12]) // Phone 13
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(12);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[13]) // Phone 14
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(13);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[14]) // Phone 15
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(14);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[15]) // Phone 16
                {
                    MainTabControl.SelectedItem = MainPhonesPVTTab;
                    System.Windows.Data.CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                    phoneViewSource.View.MoveCurrentToPosition(15);
                }
                #endregion

                #region Dialer
                // Dialer
                a = MainTreeView.ItemContainerGenerator.Items[8] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainDialerTab;
                }
                #endregion

                #region GlobalSystem
                //Global System
                a = MainTreeView.ItemContainerGenerator.Items[9] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainglobalSystemPVTTab;
                }
                #endregion

                #region Debug
                // Debug
                a = MainTreeView.ItemContainerGenerator.Items[14] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainDebugTab;
                }
                #endregion

                #region Client
                // Client
                a = MainTreeView.ItemContainerGenerator.Items[10] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = ClientConfigTab;
                }
                #endregion

                #region FirmwareUpdate
                // Firmware update
                a = MainTreeView.ItemContainerGenerator.Items[15] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainUpdateFirmwareTab;
                }
                #endregion

                #region Events
                //Events
                a = MainTreeView.ItemContainerGenerator.Items[11] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = EventsTab;
                }
                #endregion

                #region Audio
                //Audio
                a = MainTreeView.ItemContainerGenerator.Items[12] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = AudioMessagesTab;
                }
                #endregion

                #region Status
                //Status
                a = MainTreeView.ItemContainerGenerator.Items[13] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = StatusTab;
                    real_time_timer.Interval = new TimeSpan(0, 0, 0);
                    real_time_timer.Start();
                }
                else
                {
                    real_time_timer.Stop();
                }


                #endregion

                #region Expanders
                // Expanders
                a = MainTreeView.ItemContainerGenerator.Items[16] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainExpandersTab;
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[0]) // Expander 1
                {
                    MainTabControl.SelectedItem = MainExpandersPVTTab;
                    System.Windows.Data.CollectionViewSource expanderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expanderViewSource")));
                    expanderViewSource.View.MoveCurrentToPosition(0);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[1]) // Expander 2
                {
                    MainTabControl.SelectedItem = MainExpandersPVTTab;
                    System.Windows.Data.CollectionViewSource expanderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expanderViewSource")));
                    expanderViewSource.View.MoveCurrentToPosition(1);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[2]) // Expander 3
                {
                    MainTabControl.SelectedItem = MainExpandersPVTTab;
                    System.Windows.Data.CollectionViewSource expanderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expanderViewSource")));
                    expanderViewSource.View.MoveCurrentToPosition(2);
                }
                else if (e.NewValue == a.ItemContainerGenerator.Items[3]) // Expander 4
                {
                    MainTabControl.SelectedItem = MainExpandersPVTTab;
                    System.Windows.Data.CollectionViewSource expanderViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("expanderViewSource")));
                    expanderViewSource.View.MoveCurrentToPosition(3);
                }

                #endregion

                #region Memory
                // Memory
                a = MainTreeView.ItemContainerGenerator.Items[17] as TreeViewItem;
                if (e.NewValue == a)
                {
                    MainTabControl.SelectedItem = MainMemoryTab;
                }
                #endregion
            }
        }

        #region *** Tile Clicks ***

        private void UsersTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[6]).IsSelected = true;
        }

        private void ZonesTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[2]).IsSelected = true;
        }

        private void AreasTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[1]).IsSelected = true;
        }

        private void KeypadsTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[3]).IsSelected = true;
        }

        private void ExpandersTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[16]).IsSelected = true;
        }

        private void FirmwareUpdate_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedItem = MainUpdateFirmwareTab;
        }

        private void OutputsTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[4]).IsSelected = true;
        }

        private void TimersTile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[7]).IsSelected = true;
        }

        private void Phones_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[8]).IsSelected = true;
        }

        private void Dialer_Tile_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[11]).IsSelected = true;
        }

        private void ClientInfoTile_Click(object sender, RoutedEventArgs e)
        {
            TreeviewClient.IsSelected = true;
        }

        #endregion

        /* for test purposes only*/
        public void updateDebugTextBox(byte[] buf, int size, uint type)
        {
            Thread.Sleep(100);
            TextRange t_hex = new TextRange(TextboxDebugHEX.Document.ContentEnd, TextboxDebugHEX.Document.ContentEnd);
            TextRange t_ascii = new TextRange(TextboxDebugASCII.Document.ContentEnd, TextboxDebugASCII.Document.ContentEnd);
            if (buf[2] == 0xC0 && (bool)DebugGlobalCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugGlobalCheckbox.IsChecked)))//(bool)(DebugGlobalCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Global: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Global: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Red);
                }));
            }
            else if (buf[2] == 0xC1 && (bool)DebugUsersCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugUsersCheckbox.IsChecked)))
            {

                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Users: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Blue);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Users: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Blue);
                }));

            }
            else if (buf[2] == 0xC2 && (bool)DebugAlarmCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugAlarmCheckbox.IsChecked)))//(bool)(DebugAlarmCheckbox.IsChecked))
            {

                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';


                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Alarms: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Navy);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Alarms: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Navy);
                }));

            }
            else if (buf[2] == 0xC3 && (bool)DebugFaultsCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugFaultsCheckbox.IsChecked)))//(bool)(DebugFaultsCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Faults: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Green);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Faults: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Green);
                }));
            }
            else if (buf[2] == 0xC4 && (bool)DebugTimezonesCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugTimezonesCheckbox.IsChecked))) //(bool)(DebugTimezonesCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';


                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Timezones: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Timezones: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                }));
            }
            else if (buf[2] == 0xC5 && (bool)DebugAreasCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugAreasCheckbox.IsChecked))) //(bool)(DebugAreasCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Areas: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightCyan);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Areas: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightCyan);
                }));
            }
            else if (buf[2] == 0xC6 && (bool)DebugKeypadsCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugKeypadsCheckbox.IsChecked))) //(bool)(DebugKeypadsCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Keypads: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGreen);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Keypads: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightGreen);
                }));
            }
            else if (buf[2] == 0xC7 && (bool)DebugOutputsCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugOutputsCheckbox.IsChecked))) //(bool)(DebugOutputsCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Outputs: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightPink);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Outputs: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightPink);
                }));
            }
            else if (buf[2] == 0xC8 && (bool)DebugCommunicationsCheckbox.Dispatcher.Invoke(new Func<bool?>(() => DebugCommunicationsCheckbox.IsChecked))) //(bool)(DebugCommunicationsCheckbox.IsChecked))
            {
                var new_buf = buf.Skip(3).Take(size - 7).ToArray();
                string indata = ByteArrayToString(new_buf, (int)(size - 7), 1) + '\n';


                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_hex.Text = "Communications: " + indata;
                    t_hex.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightYellow);
                    //TextboxDebugHEX.AppendText("Global:");
                }));


                indata = System.Text.Encoding.ASCII.GetString(new_buf) + '\n';

                this.Dispatcher.Invoke((Action)(() =>
                {
                    t_ascii.Text = "Communications: " + indata;
                    t_ascii.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.LightYellow);
                }));
            }

        }

        public string ByteArrayToString(byte[] ba, int size, int separation)
        {
            if (separation == 0)
            {
                StringBuilder hex = new StringBuilder(size * 2);
                for (uint i = 0; i < size; i++)
                { hex.AppendFormat("{0:x2}", ba[i]); }
                return hex.ToString();
            }
            else
            {
                StringBuilder hex = new StringBuilder(size * 3);
                for (uint i = 0; i < size; i++)
                {
                    hex.AppendFormat("{0:x2}", ba[i]);
                    hex.Append(',');
                }
                return hex.ToString();
            }
        }

        public async void UpdateDataGridViews(byte[] buf, int size)
        {
            // Get zone address
            int addr_MSB = buf[3] << 16;
            int addr_mSB = buf[4] << 8; // TODO:Improve it
            int addr_LSB = buf[5];

            int addr = addr_MSB + addr_mSB + addr_LSB;

            if (addr >= Constants.KP_ZONES_INIC_ADDR && addr < Constants.KP_ZONES_FINAL_ADDR) // it's a zone read TODO: Need to be improved
            {
                #region Zones
                int zone_to_read = ((addr - Constants.KP_ZONES_INIC_ADDR) / (int)Constants.KP_FLASH_TAMANHO_DADOS_ZONAS_FLASH);

                Protocol.Zones zone = new Protocol.Zones();


                byte[] description = (byte[])zone.attributes["description"]["value"];
                //TODO: Extract this for to a function
                for (int i = (7 + (int)zone.attributes["description"]["address"]), j = 0; i < (7 + (int)zone.attributes["description"]["address"] + description.Length); i++, j++)
                { description[j] = buf[i]; }

                byte[] away_entry_delay_time = (byte[])zone.attributes["away_entry_delay_time"]["value"];
                for (int i = (7 + (int)zone.attributes["away_entry_delay_time"]["address"]), j = 0; i < (7 + (int)zone.attributes["away_entry_delay_time"]["address"] + away_entry_delay_time.Length); i++, j++)
                { away_entry_delay_time[j] = buf[i]; }

                byte[] stay_entry_delay_time = (byte[])zone.attributes["stay_entry_delay_time"]["value"];
                for (int i = (7 + (int)zone.attributes["stay_entry_delay_time"]["address"]), j = 0; i < (7 + (int)zone.attributes["stay_entry_delay_time"]["address"] + stay_entry_delay_time.Length); i++, j++)
                { stay_entry_delay_time[j] = buf[i]; }

                byte[] two_trigger_time = (byte[])zone.attributes["two_trigger_time"]["value"];
                for (int i = (7 + (int)zone.attributes["two_trigger_time"]["address"]), j = 0; i < (7 + (int)zone.attributes["two_trigger_time"]["address"] + two_trigger_time.Length); i++, j++)
                { two_trigger_time[j] = buf[i]; }

                byte[] sensor_watch_timer = (byte[])zone.attributes["sensor_watch_timer"]["value"];
                for (int i = (7 + (int)zone.attributes["sensor_watch_timer"]["address"]), j = 0; i < (7 + (int)zone.attributes["sensor_watch_timer"]["address"] + sensor_watch_timer.Length); i++, j++)
                { sensor_watch_timer[j] = buf[i]; }

                byte[] terminator_count = (byte[])zone.attributes["terminator_count"]["value"];
                for (int i = (7 + (int)zone.attributes["terminator_count"]["address"]), j = 0; i < (7 + (int)zone.attributes["terminator_count"]["address"] + terminator_count.Length); i++, j++)
                { terminator_count[j] = buf[i]; }

                byte[] options = (byte[])zone.attributes["options"]["value"];
                for (int i = (7 + (int)zone.attributes["options"]["address"]), j = 0; i < (7 + (int)zone.attributes["options"]["address"] + options.Length); i++, j++)
                { options[j] = buf[i]; }

                byte[] partitions_away = (byte[])zone.attributes["partitions_away"]["value"];
                for (int i = (7 + (int)zone.attributes["partitions_away"]["address"]), j = 0; i < (7 + (int)zone.attributes["partitions_away"]["address"] + partitions_away.Length); i++, j++)
                { partitions_away[j] = buf[i]; }
                byte[] partitions_stay = (byte[])zone.attributes["partitions_stay"]["value"];
                for (int i = (7 + (int)zone.attributes["partitions_stay"]["address"]), j = 0; i < (7 + (int)zone.attributes["partitions_stay"]["address"] + partitions_stay.Length); i++, j++)
                { partitions_stay[j] = buf[i]; }

                byte[] show_keypads = (byte[])zone.attributes["show_keypads"]["value"];
                for (int i = (7 + (int)zone.attributes["show_keypads"]["address"]), j = 0; i < (7 + (int)zone.attributes["show_keypads"]["address"] + show_keypads.Length); i++, j++)
                { show_keypads[j] = buf[i]; }

                byte[] terminal_configuration_values = (byte[])zone.attributes["terminal_configuration"]["value"];
                for (int i = (7 + (int)zone.attributes["terminal_configuration"]["address"]), j = 0; i < (7 + (int)zone.attributes["terminal_configuration"]["address"] + terminal_configuration_values.Length); i++, j++)
                { terminal_configuration_values[j] = buf[i]; }

                byte[] zone_alarm_to_outputs = (byte[])zone.attributes["zone_alarm_to_outputs"]["value"];
                for (int i = (7 + (int)zone.attributes["zone_alarm_to_outputs"]["address"]), j = 0; i < (7 + (int)zone.attributes["zone_alarm_to_outputs"]["address"] + zone_alarm_to_outputs.Length); i++, j++)
                { zone_alarm_to_outputs[j] = buf[i]; }

                byte[] zone_reserved_1 = (byte[])zone.attributes["reserved_1"]["value"];
                for (int i = (7 + (int)zone.attributes["reserved_1"]["address"]), j = 0; i < (7 + (int)zone.attributes["reserved_1"]["address"] + zone_reserved_1.Length); i++, j++)
                { zone_reserved_1[j] = buf[i]; }

                byte[] zone_reserved_2 = (byte[])zone.attributes["reserved_2"]["value"];
                for (int i = (7 + (int)zone.attributes["reserved_2"]["address"]), j = 0; i < (7 + (int)zone.attributes["reserved_2"]["address"] + zone_reserved_2.Length); i++, j++)
                { zone_reserved_2[j] = buf[i]; }

                byte[] zone_reserved_3 = (byte[])zone.attributes["reserved_3"]["value"];
                for (int i = (7 + (int)zone.attributes["reserved_3"]["address"]), j = 0; i < (7 + (int)zone.attributes["reserved_3"]["address"] + zone_reserved_3.Length); i++, j++)
                { zone_reserved_3[j] = buf[i]; }

                byte[] zone_reserved_4 = (byte[])zone.attributes["reserved_4"]["value"];
                for (int i = (7 + (int)zone.attributes["reserved_4"]["address"]), j = 0; i < (7 + (int)zone.attributes["reserved_4"]["address"] + zone_reserved_4.Length); i++, j++)
                { zone_reserved_4[j] = buf[i]; }

                byte[] zone_reserved_5 = (byte[])zone.attributes["reserved_5"]["value"];
                for (int i = (7 + (int)zone.attributes["reserved_5"]["address"]), j = 0; i < (7 + (int)zone.attributes["reserved_5"]["address"] + zone_reserved_5.Length); i++, j++)
                { zone_reserved_5[j] = buf[i]; }

                byte[] temperature_normal_high = (byte[])zone.attributes["temperature_normal_high"]["value"];
                for (int i = (7 + (int)zone.attributes["temperature_normal_high"]["address"]), j = 0; i < (7 + (int)zone.attributes["temperature_normal_high"]["address"] + temperature_normal_high.Length); i++, j++)
                { temperature_normal_high[j] = buf[i]; }

                byte[] temperature_normal_low = (byte[])zone.attributes["temperature_normal_low"]["value"];
                for (int i = (7 + (int)zone.attributes["temperature_normal_low"]["address"]), j = 0; i < (7 + (int)zone.attributes["temperature_normal_low"]["address"] + temperature_normal_low.Length); i++, j++)
                { temperature_normal_low[j] = buf[i]; }

                byte[] temperature_alarm_low = (byte[])zone.attributes["temperature_alarm_low"]["value"];
                for (int i = (7 + (int)zone.attributes["temperature_alarm_low"]["address"]), j = 0; i < (7 + (int)zone.attributes["temperature_alarm_low"]["address"] + temperature_alarm_low.Length); i++, j++)
                { temperature_alarm_low[j] = buf[i]; }

                byte[] temperature_alarm_high = (byte[])zone.attributes["temperature_alarm_high"]["value"];
                for (int i = (7 + (int)zone.attributes["temperature_alarm_high"]["address"]), j = 0; i < (7 + (int)zone.attributes["temperature_alarm_high"]["address"] + temperature_alarm_high.Length); i++, j++)
                { temperature_alarm_high[j] = buf[i]; }

                byte[] report_code = (byte[])zone.attributes["report_code"]["value"];
                for (int i = (7 + (int)zone.attributes["report_code"]["address"]), j = 0; i < (7 + (int)zone.attributes["report_code"]["address"] + report_code.Length); i++, j++)
                { report_code[j] = buf[i]; }

                byte[] audio_tracks = (byte[])zone.attributes["audio_tracks"]["value"];
                for (int i = (7 + (int)zone.attributes["audio_tracks"]["address"]), j = 0; i < (7 + (int)zone.attributes["audio_tracks"]["address"] + audio_tracks.Length); i++, j++)
                { audio_tracks[j] = buf[i]; }

                //TODO: Find a better solution to calculate this
                //Int64 zone_options = (options[7] << 56) + (options[6] << 48) + (options[5] << 40) + (options[4] << 32) + (options[3] << 24) + (options[2] << 16) + (options[1] << 8) + options[0];
                UInt64 zone_options = ((((ulong)(options[7] << 24) + (ulong)(options[6] << 16) + (ulong)(options[5] << 8) + (options[4])) << 32) + ((ulong)(options[3] << 24) + (ulong)(options[2] << 16) + (ulong)(options[1] << 8) + (ulong)options[0]));
                //Get the meaning from options
                UInt64 zone_active = zone_options & ((ulong[])zone.attributes["options"]["zone_active_mask"])[0];
                UInt64 zone_key_switch = zone_options & ((ulong[])zone.attributes["options"]["zone_key_switch_mask"])[0];
                UInt64 zone_key_switch_type = zone_options & ((ulong[])zone.attributes["options"]["zone_key_switch_type_mask"])[0];
                UInt64 zone_always_bypass = zone_options & ((ulong[])zone.attributes["options"]["zone_always_bypass_mask"])[0];
                UInt64 zone_manual_bypass = zone_options & ((ulong[])zone.attributes["options"]["zone_manual_bypass_mask"])[0];
                UInt64 zone_auto_bypass = zone_options & ((ulong[])zone.attributes["options"]["zone_auto_bypass_mask"])[0];
                UInt64 zone_soak_test = zone_options & ((ulong[])zone.attributes["options"]["zone_soak_test_mask"])[0];
                UInt64 zone_send_report = zone_options & ((ulong[])zone.attributes["options"]["zone_report_to_area_account_mask"])[0];
                UInt64 zone_always_report = zone_options & ((ulong[])zone.attributes["options"]["zone_always_report_to_gsm_mask"])[0];

                UInt64 zone_arm_if_not_ready = zone_options & ((ulong[])zone.attributes["options"]["zone_arm_if_not_ready_mask"])[0];
                UInt64 zone_silent_zone = zone_options & ((ulong[])zone.attributes["options"]["zone_silent_mask"])[0];
                UInt64 zone_handover = zone_options & ((ulong[])zone.attributes["options"]["zone_handover_mask"])[0];
                UInt64 zone_two_trigger = zone_options & ((ulong[])zone.attributes["options"]["zone_two_trigger_mask"])[0];
                UInt64 zone_chime_arm = zone_options & ((ulong[])zone.attributes["options"]["zone_chime_arm_mask"])[0];
                UInt64 zone_chime_disarm = zone_options & ((ulong[])zone.attributes["options"]["zone_chime_disarm_mask"])[0];
                UInt64 zone_exit_terminator = zone_options & ((ulong[])zone.attributes["options"]["zone_exit_terminator_mask"])[0];
                UInt64 zone_sensor_watch = zone_options & ((ulong[])zone.attributes["options"]["zone_sensor_watch_mask"])[0];
                UInt64 zone_keypad_visible = zone_options & ((ulong[])zone.attributes["options"]["zone_visible_keypad_mask"])[0];

                UInt64 zone_radio = zone_options & ((ulong[])zone.attributes["options"]["zone_radio_mask"])[0];
                UInt64 zone_24_hours_mask = zone_options & ((ulong[])zone.attributes["options"]["zone_24_hours_mask"])[0];
                UInt64 zone_24_hours_auto_reset_mask = zone_options & ((ulong[])zone.attributes["options"]["zone_24_hours_auto_reset_mask"])[0];
                UInt64 zone_24_hours_firezone_mask = zone_options & ((ulong[])zone.attributes["options"]["zone_24_hours_firezone_mask"])[0];
                UInt64 zone_send_multiple_reports_via_dealer_mask = zone_options & ((ulong[])zone.attributes["options"]["zone_send_multiple_reports_via_dealer_mask"])[0];
                UInt64 zone_report_to_area_account_mask = zone_options & ((ulong[])zone.attributes["options"]["zone_report_to_area_account_mask"])[0];
                UInt64 zone_do_not_report_24hours_alarm_mask = zone_options & ((ulong[])zone.attributes["options"]["zone_do_not_report_24hours_alarm_mask"])[0];

                // Write in Data Grid View table
                try
                {
                    #region OUTPUTS CONFIGURATION IN ALARM
                    //chime
                    databaseDataSet.Zone.Rows[zone_to_read]["Chime alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["chime"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Chime alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["chime"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Chime alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["chime"])[0] + 2];
                    //sensor watch
                    databaseDataSet.Zone.Rows[zone_to_read]["Sensor watch alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["sensor_watch"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Sensor watch alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["sensor_watch"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Sensor watch alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["sensor_watch"])[0] + 2];
                    //entry time
                    databaseDataSet.Zone.Rows[zone_to_read]["Entry time alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["entry_delay"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Entry time alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["entry_delay"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Entry time alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["entry_delay"])[0] + 2];
                    // anti mask
                    databaseDataSet.Zone.Rows[zone_to_read]["Anti mask alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["anti_mask"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Anti mask alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["anti_mask"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Anti mask alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["anti_mask"])[0] + 2];
                    //24h alarm
                    databaseDataSet.Zone.Rows[zone_to_read]["24 hour alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["24h"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["24 hour alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["24h"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["24 hour alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["24h"])[0] + 2];
                    //Fire alarm
                    databaseDataSet.Zone.Rows[zone_to_read]["Fire alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["fire"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Fire alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["fire"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Fire alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["fire"])[0] + 2];
                    //Zone alarm
                    databaseDataSet.Zone.Rows[zone_to_read]["Zone alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["alarm"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Zone alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["alarm"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Zone alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["alarm"])[0] + 2];
                    //Tamper alarm
                    databaseDataSet.Zone.Rows[zone_to_read]["Tamper alarm output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["tamper"])[0]];
                    databaseDataSet.Zone.Rows[zone_to_read]["Tamper alarm output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["tamper"])[0] + 1];
                    databaseDataSet.Zone.Rows[zone_to_read]["Tamper alarm output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["tamper"])[0] + 2];

                    //arm_away_timezone
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away by timezone - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_timezone"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away by timezone - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_timezone"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away by timezone - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_timezone"])[0] + 2];

                    //arm_away_code
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with code - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_code"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with code - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_code"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with code - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_code"])[0] + 2];

                    //arm_away_command
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with command - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_command"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with command - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_command"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with command - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_command"])[0] + 2];

                    //arm_away_key_switch
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with keyswtich - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_key_switch"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with keyswtich - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_key_switch"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away with keyswtich - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_key_switch"])[0] + 2];

                    //arm_away_remote
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away remotely - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_remote"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away remotely - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_remote"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm away remotely - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_away_remote"])[0] + 2];

                    //arm_stay_timezone
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay by timezone - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_timezone"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay by timezone - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_timezone"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay by timezone - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_timezone"])[0] + 2];

                    //arm_stay_code
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with code - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_code"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with code - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_code"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with code - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_code"])[0] + 2];

                    //arm_stay_command
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with command - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_command"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with command - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_command"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with command - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_command"])[0] + 2];

                    //arm_stay_key_switch
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with keyswtich - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_key_switch"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with keyswtich - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_key_switch"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay with keyswtich - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_key_switch"])[0] + 2];

                    //arm_stay_remote
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay remotely - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_remote"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay remotely - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_remote"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Arm stay remotely - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["arm_stay_remote"])[0] + 2];

                    //disarm_away_timezone
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away by timezone - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_timezone"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away by timezone - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_timezone"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away by timezone - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_timezone"])[0] + 2];

                    //disarm_away_code
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with code - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_code"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with code - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_code"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with code - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_code"])[0] + 2];

                    //disarm_away_command
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with command - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_command"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with command - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_command"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with command - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_command"])[0] + 2];

                    //disarm_away_key_switch
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with keyswtich - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_key_switch"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with keyswtich - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_key_switch"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away with keyswtich - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_key_switch"])[0] + 2];

                    //disarm_away_remote
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away remotely - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_remote"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away remotely - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_remote"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm away remotely - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_away_remote"])[0] + 2];

                    //disarm_stay_timezone
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay by timezone - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_timezone"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay by timezone - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_timezone"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay by timezone - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_timezone"])[0] + 2];

                    //disarm_stay_code
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with code - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_code"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with code - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_code"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with code - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_code"])[0] + 2];

                    //disarm_stay_command
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with command - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_command"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with command - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_command"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with command - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_command"])[0] + 2];

                    //disarm_stay_key_switch
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with keyswtich - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_key_switch"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with keyswtich - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_key_switch"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay with keyswtich - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_key_switch"])[0] + 2];

                    //disarm_stay_remote
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay remotely - output 1"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_remote"])[0]];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay remotely - output 2"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_remote"])[0] + 1];
                    databaseDataSet.Zone.Rows[(int)(zone_to_read)]["Disarm stay remotely - output 3"] = zone_alarm_to_outputs[((int[])zone.attributes["zone_alarm_to_outputs"]["disarm_stay_remote"])[0] + 2];

                    #endregion

                    #region CIRCUIT TERMINAL CONFIGURATION
                    databaseDataSet.Zone.Rows[zone_to_read]["Terminal circuit type"] = terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["circuit_configuration"])[0]].ToString();
                    //R1
                    databaseDataSet.Zone.Rows[zone_to_read]["R1 Value"] = terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r1_type"])[0]].ToString();
                    databaseDataSet.Zone.Rows[zone_to_read]["R1 Function"] = (terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r1_configuration"])[0]] & 0xF0).ToString();
                    databaseDataSet.Zone.Rows[zone_to_read]["R1 Contact type"] = (terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r1_configuration"])[0]] & 0x0F).ToString();
                    //R2
                    databaseDataSet.Zone.Rows[zone_to_read]["R2 Value"] = terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r2_type"])[0]].ToString();
                    databaseDataSet.Zone.Rows[zone_to_read]["R2 Function"] = (terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r2_configuration"])[0]] & 0xF0).ToString();
                    databaseDataSet.Zone.Rows[zone_to_read]["R2 Contact type"] = (terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r2_configuration"])[0]] & 0x0F).ToString();
                    //R3
                    databaseDataSet.Zone.Rows[zone_to_read]["R3 Value"] = terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r3_type"])[0]].ToString();
                    databaseDataSet.Zone.Rows[zone_to_read]["R3 Function"] = (terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r3_configuration"])[0]] & 0xF0).ToString();
                    databaseDataSet.Zone.Rows[zone_to_read]["R3 Contact type"] = (terminal_configuration_values[((int[])zone.attributes["terminal_configuration"]["r3_configuration"])[0]] & 0x0F).ToString();
                    #endregion

                    #region NOT USED
                    #region RESERVED
                    databaseDataSet.Zone.Rows[zone_to_read]["Reserved 1"] = zone_reserved_1;
                    databaseDataSet.Zone.Rows[zone_to_read]["Reserved 2"] = zone_reserved_2;
                    databaseDataSet.Zone.Rows[zone_to_read]["Reserved 3"] = zone_reserved_3;
                    databaseDataSet.Zone.Rows[zone_to_read]["Reserved 4"] = zone_reserved_4;
                    databaseDataSet.Zone.Rows[zone_to_read]["Reserved 5"] = zone_reserved_5;
                    #endregion
                    #region OPTIONS
                    databaseDataSet.Zone.Rows[zone_to_read]["Radio zone"] = (zone_radio > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["24h zone"] = (zone_24_hours_mask > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["24h zone - auto-reset"] = (zone_24_hours_auto_reset_mask > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["24h zone - firezone"] = (zone_24_hours_firezone_mask > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Send multiple reports to dialer"] = (zone_send_multiple_reports_via_dealer_mask > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Report to account"] = (zone_report_to_area_account_mask > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Dont report 24h zone"] = (zone_do_not_report_24hours_alarm_mask > 0);
                    #endregion
                    databaseDataSet.Zone.Rows[zone_to_read]["Temperature normal low"] = temperature_normal_low;
                    databaseDataSet.Zone.Rows[zone_to_read]["Temperature normal high"] = temperature_normal_high;
                    databaseDataSet.Zone.Rows[zone_to_read]["Temperature alarm low"] = temperature_alarm_low;
                    databaseDataSet.Zone.Rows[zone_to_read]["Temperature alarm high"] = temperature_alarm_high;
                    databaseDataSet.Zone.Rows[zone_to_read]["Report code"] = report_code;
                    #endregion

                    #region OUTPUTS

                    #endregion

                    //String
                    databaseDataSet.Zone.Rows[zone_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    //Number
                    databaseDataSet.Zone.Rows[zone_to_read]["Entry time away"] = (away_entry_delay_time[1] << 8) + away_entry_delay_time[0];
                    databaseDataSet.Zone.Rows[zone_to_read]["Entry time stay"] = (stay_entry_delay_time[1] << 8) + stay_entry_delay_time[0];
                    databaseDataSet.Zone.Rows[zone_to_read]["Trigger Time"] = two_trigger_time[0];
                    databaseDataSet.Zone.Rows[zone_to_read]["Terminator count"] = terminator_count[0];
                    databaseDataSet.Zone.Rows[zone_to_read]["Sensor watch time"] = (int)(sensor_watch_timer[1] << 8) + sensor_watch_timer[0];

                    //Checkbox
                    //Options
                    databaseDataSet.Zone.Rows[zone_to_read]["Zone active"] = (zone_active > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Keyswitch zone"] = (zone_key_switch > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Keyswitch type"] = (zone_key_switch_type > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Always bypass"] = (zone_always_bypass > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Manual bypass"] = (zone_manual_bypass > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Auto bypass"] = (zone_auto_bypass > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Soak test"] = (zone_soak_test > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Send report"] = (zone_send_report > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Always report"] = (zone_always_report > 0);

                    databaseDataSet.Zone.Rows[zone_to_read]["Arm if not ready"] = (zone_arm_if_not_ready > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Silent zone"] = (zone_silent_zone > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Handover"] = (zone_handover > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Two trigger"] = (zone_two_trigger > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Chime - arm"] = (zone_chime_arm > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Chime - disarm"] = (zone_chime_disarm > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Exit terminator"] = (zone_exit_terminator > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Sensor watch"] = (zone_sensor_watch > 0);
                    databaseDataSet.Zone.Rows[zone_to_read]["Keypad visible"] = (zone_keypad_visible > 0);
                    //Areas away
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 1"] = (partitions_away[0] & 0x01) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 2"] = (partitions_away[0] & 0x02) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 3"] = (partitions_away[0] & 0x04) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 4"] = (partitions_away[0] & 0x08) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 5"] = (partitions_away[0] & 0x10) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 6"] = (partitions_away[0] & 0x20) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 7"] = (partitions_away[0] & 0x40) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area away 8"] = (partitions_away[0] & 0x80) > 0;

                    //Areas Stay
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 1"] = (partitions_stay[0] & 0x01) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 2"] = (partitions_stay[0] & 0x02) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 3"] = (partitions_stay[0] & 0x04) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 4"] = (partitions_stay[0] & 0x08) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 5"] = (partitions_stay[0] & 0x10) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 6"] = (partitions_stay[0] & 0x20) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 7"] = (partitions_stay[0] & 0x40) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Area stay 8"] = (partitions_stay[0] & 0x80) > 0;

                    //Show Keypads
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 1"] = (show_keypads[0] & 0x01) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 2"] = (show_keypads[0] & 0x02) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 3"] = (show_keypads[0] & 0x04) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 4"] = (show_keypads[0] & 0x08) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 5"] = (show_keypads[0] & 0x10) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 6"] = (show_keypads[0] & 0x20) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 7"] = (show_keypads[0] & 0x40) > 0;
                    databaseDataSet.Zone.Rows[zone_to_read]["Show keypad 8"] = (show_keypads[0] & 0x80) > 0;

                    //Audio tracks
                    if (((audio_tracks[1] << 8) + audio_tracks[0]).Equals(0xffff))
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0];
                    }
                    else
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0] + 1;
                    }

                    if (((audio_tracks[3] << 8) + audio_tracks[2]).Equals(0xffff))
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2];
                    }
                    else
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2] + 1;
                    }
                    if (((audio_tracks[5] << 8) + audio_tracks[4]).Equals(0xffff))
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4];
                    }
                    else
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4] + 1;
                    }
                    if (((audio_tracks[7] << 8) + audio_tracks[6]).Equals(0xffff))
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6];
                    }
                    else
                    {
                        databaseDataSet.Zone.Rows[zone_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6] + 1;
                    }

                    databaseDataSet.Zone.AcceptChanges();
                    databaseDataSetZoneTableAdapter.Fill(databaseDataSet.Zone);
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improvee
                }
                #endregion
            }
            else if (addr >= Constants.KP_AREAS_INIC_ADDR && addr < Constants.KP_AREAS_FINAL_ADDR)//TODO: Change to areas and improve this process
            {
                #region Areas
                int area_to_read = ((addr - 0x3000) / 512);

                Protocol.Areas area = new Protocol.Areas();
                byte[] description = (byte[])area.attributes["description"]["value"];
                //TODO: Extract this for to a function
                for (int i = (7 + (int)area.attributes["description"]["address"]), j = 0; i < (7 + (int)area.attributes["description"]["address"] + description.Length); i++, j++)
                { description[j] = buf[i]; }

                byte[] options = (byte[])area.attributes["options"]["value"];
                for (int i = (7 + (int)area.attributes["options"]["address"]), j = 0; i < (7 + (int)area.attributes["options"]["address"] + options.Length); i++, j++)
                { options[j] = buf[i]; }

                byte[] away_entry_delay_time = (byte[])area.attributes["away_entry_delay_time"]["value"];
                for (int i = (7 + (int)area.attributes["away_entry_delay_time"]["address"]), j = 0; i < (7 + (int)area.attributes["away_entry_delay_time"]["address"] + away_entry_delay_time.Length); i++, j++)
                { away_entry_delay_time[j] = buf[i]; }

                byte[] stay_entry_delay_time = (byte[])area.attributes["stay_entry_delay_time"]["value"];
                for (int i = (7 + (int)area.attributes["stay_entry_delay_time"]["address"]), j = 0; i < (7 + (int)area.attributes["stay_entry_delay_time"]["address"] + stay_entry_delay_time.Length); i++, j++)
                { stay_entry_delay_time[j] = buf[i]; }

                byte[] DRCV_client_code = (byte[])area.attributes["DRCV_client_code"]["value"];
                for (int i = (7 + (int)area.attributes["DRCV_client_code"]["address"]), j = 0; i < (7 + (int)area.attributes["DRCV_client_code"]["address"] + DRCV_client_code.Length); i++, j++)
                { DRCV_client_code[j] = buf[i]; }

                byte[] code_length = (byte[])area.attributes["code_length"]["value"];
                for (int i = (7 + (int)area.attributes["code_length"]["address"]), j = 0; i < (7 + (int)area.attributes["code_length"]["address"] + code_length.Length); i++, j++)
                { code_length[j] = buf[i]; }

                byte[] call_code = (byte[])area.attributes["call_code"]["value"];
                for (int i = (7 + (int)area.attributes["call_code"]["address"]), j = 0; i < (7 + (int)area.attributes["call_code"]["address"] + call_code.Length); i++, j++)
                { call_code[j] = buf[i]; }

                byte[] area_arm_disarm_outputs = (byte[])area.attributes["area_arm_disarm_outputs"]["value"];
                for (int i = (7 + (int)area.attributes["area_arm_disarm_outputs"]["address"]), j = 0; i < (7 + (int)area.attributes["area_arm_disarm_outputs"]["address"] + area_arm_disarm_outputs.Length); i++, j++)
                { area_arm_disarm_outputs[j] = buf[i]; }

                byte[] area_beeps_arm_disarm_outputs = (byte[])area.attributes["area_beeps_arm_disarm_outputs"]["value"];
                for (int i = (7 + (int)area.attributes["area_beeps_arm_disarm_outputs"]["address"]), j = 0; i < (7 + (int)area.attributes["area_beeps_arm_disarm_outputs"]["address"] + area_beeps_arm_disarm_outputs.Length); i++, j++)
                { area_beeps_arm_disarm_outputs[j] = buf[i]; }

                byte[] Timezone_start_arm = (byte[])area.attributes["Timezone_start_arm"]["value"];
                for (int i = (7 + (int)area.attributes["Timezone_start_arm"]["address"]), j = 0; i < (7 + (int)area.attributes["Timezone_start_arm"]["address"] + Timezone_start_arm.Length); i++, j++)
                { Timezone_start_arm[j] = buf[i]; }

                byte[] Timezone_start_disarm = (byte[])area.attributes["Timezone_start_disarm"]["value"];
                for (int i = (7 + (int)area.attributes["Timezone_start_disarm"]["address"]), j = 0; i < (7 + (int)area.attributes["Timezone_start_disarm"]["address"] + Timezone_start_disarm.Length); i++, j++)
                { Timezone_start_disarm[j] = buf[i]; }

                byte[] Timezone_end_arm = (byte[])area.attributes["Timezone_end_arm"]["value"];
                for (int i = (7 + (int)area.attributes["Timezone_end_arm"]["address"]), j = 0; i < (7 + (int)area.attributes["Timezone_end_arm"]["address"] + Timezone_end_arm.Length); i++, j++)
                { Timezone_end_arm[j] = buf[i]; }

                byte[] Timezone_end_disarm = (byte[])area.attributes["Timezone_end_disarm"]["value"];
                for (int i = (7 + (int)area.attributes["Timezone_end_disarm"]["address"]), j = 0; i < (7 + (int)area.attributes["Timezone_end_disarm"]["address"] + Timezone_end_disarm.Length); i++, j++)
                { Timezone_end_disarm[j] = buf[i]; }

                //RESERVED
                byte[] area_reserved_1 = (byte[])area.attributes["reserved_1"]["value"];
                for (int i = (7 + (int)area.attributes["reserved_1"]["address"]), j = 0; i < (7 + (int)area.attributes["reserved_1"]["address"] + area_reserved_1.Length); i++, j++)
                { area_reserved_1[j] = buf[i]; }

                byte[] area_reserved_2 = (byte[])area.attributes["reserved_2"]["value"];
                for (int i = (7 + (int)area.attributes["reserved_2"]["address"]), j = 0; i < (7 + (int)area.attributes["reserved_2"]["address"] + area_reserved_2.Length); i++, j++)
                { area_reserved_2[j] = buf[i]; }

                byte[] area_reserved_3 = (byte[])area.attributes["reserved_3"]["value"];
                for (int i = (7 + (int)area.attributes["reserved_3"]["address"]), j = 0; i < (7 + (int)area.attributes["reserved_3"]["address"] + area_reserved_3.Length); i++, j++)
                { area_reserved_3[j] = buf[i]; }

                byte[] start_message_command_control = (byte[])area.attributes["start_message_command_control"]["value"];
                for (int i = (7 + (int)area.attributes["start_message_command_control"]["address"]), j = 0; i < (7 + (int)area.attributes["start_message_command_control"]["address"] + start_message_command_control.Length); i++, j++)
                { start_message_command_control[j] = buf[i]; }

                byte[] audio_tracks = (byte[])area.attributes["audio_tracks"]["value"];
                for (int i = (7 + (int)area.attributes["audio_tracks"]["address"]), j = 0; i < (7 + (int)area.attributes["audio_tracks"]["address"] + audio_tracks.Length); i++, j++)
                { audio_tracks[j] = buf[i]; }

                //TODO: Find a better solution to calculate this
                Int32 area_options = (options[3] << 24) + (options[2] << 16) + (options[1] << 8) + options[0];

                #region Get the meaning from options
                Int32 area_code_required_to_arm_away = area_options & ((int[])area.attributes["options"]["KP_PART_ARM_BOT_REQUIRED_BEFORE_CODE_TO_SET_AWAY"])[0];
                Int32 area_code_required_to_arm_stay = area_options & ((int[])area.attributes["options"]["KP_PART_ARM_BOT_REQUIRED_BEFORE_CODE_TO_SET_STAY"])[0];
                Int32 area_code_required_to_unset = area_options & ((int[])area.attributes["options"]["area_code_required_to_unset"])[0];
                Int32 area_tag_required_to_set = area_options & ((int[])area.attributes["options"]["area_tag_required_to_set"])[0];
                Int32 area_code_or_tag = area_options & ((int[])area.attributes["options"]["area_code_or_tag"])[0];
                Int32 area_code_required_to_bypass_zones = area_options & ((int[])area.attributes["options"]["area_code_required_to_bypass_zones"])[0];
                Int32 area_send_alarm_atend_exit_delay = area_options & ((int[])area.attributes["options"]["area_send_alarm_atend_exit_delay"])[0];
                Int32 area_can_arm_away_only_if_all_zones_sealed = area_options & ((int[])area.attributes["options"]["area_can_arm_away_only_if_all_zones_sealed"])[0];
                Int32 area_can_arm_stay_only_if_all_zones_sealed = area_options & ((int[])area.attributes["options"]["area_can_arm_stay_only_if_all_zones_sealed"])[0];
                Int32 area_use_near_and_verified_alarm_reporting_for_all_zones = area_options & ((int[])area.attributes["options"]["area_use_near_and_verified_alarm_reporting_for_all_zones"])[0];
                Int32 area_assign_chirps_access_tags = area_options & ((int[])area.attributes["options"]["area_assign_chirps_access_tags"])[0];
                Int32 area_can_not_arm_if_zone_unsealed_at_end_of_exit_delay = area_options & ((int[])area.attributes["options"]["area_can_not_arm_if_zone_unsealed_at_end_of_exit_delay"])[0];

                #endregion

                try
                {
                    //String
                    databaseDataSet.Area.Rows[area_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    #region Options
                    databaseDataSet.Area.Rows[area_to_read]["Code required to disarm"] = (area_code_required_to_unset > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Code to bypass"] = (area_code_required_to_bypass_zones > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Not arm if zones open after exit delay"] = (area_can_not_arm_if_zone_unsealed_at_end_of_exit_delay > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Code required to arm away"] = (area_code_required_to_arm_away > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Code required to arm stay"] = (area_code_required_to_arm_stay > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Tag required to arm"] = (area_tag_required_to_set > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Tag or code required"] = (area_code_or_tag > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Send alarm atend exit time"] = (area_can_not_arm_if_zone_unsealed_at_end_of_exit_delay > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Can arm away only if all zones sealed"] = (area_can_arm_away_only_if_all_zones_sealed > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Can arm stay only if all zones sealed"] = (area_can_arm_stay_only_if_all_zones_sealed > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Use near and verified alarm reporting for all zones"] = (area_use_near_and_verified_alarm_reporting_for_all_zones > 0);
                    databaseDataSet.Area.Rows[area_to_read]["Assign chirps access tags"] = (area_assign_chirps_access_tags > 0);

                    #endregion

                    #region RESERVED
                    databaseDataSet.Area.Rows[area_to_read]["Reserved 1"] = area_reserved_1;
                    databaseDataSet.Area.Rows[area_to_read]["Reserved 2"] = area_reserved_2;
                    databaseDataSet.Area.Rows[area_to_read]["Reserved 3"] = area_reserved_3;
                    #region NOT USED
                    databaseDataSet.Area.Rows[area_to_read]["Start message command control"] = start_message_command_control;
                    #endregion
                    #endregion

                    #region Outputs
                    //away_arm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arm away output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_arm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arm away output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_arm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arm away output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_arm_outputs"])[0] + 2];
                    //stay_arm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arm stay output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_arm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arm stay output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_arm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arm stay output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_arm_outputs"])[0] + 2];
                    //away_disarm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Disarm away output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_disarm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm away output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_disarm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm away output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_disarm_outputs"])[0] + 2];
                    //stay_disarm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Disarm stay output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_disarm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm stay output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_disarm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm stay output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_disarm_outputs"])[0] + 2];
                    //pulse_away_arm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arm away pulsed output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_arm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arm away pulsed output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_arm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arm away pulsed output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_arm_outputs"])[0] + 2];
                    //pulse_stay_arm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arm stay pulsed output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_arm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arm stay pulsed output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_arm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arm stay pulsed output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_arm_outputs"])[0] + 2];
                    //pulse_away_disarm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Disarm pulsed away output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_disarm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm pulsed away output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_disarm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm pulsed away output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_disarm_outputs"])[0] + 2];
                    //pulse_stay_disarm_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Disarm pulsed stay output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_disarm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm pulsed stay output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_disarm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Disarm pulsed stay output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_disarm_outputs"])[0] + 2];
                    //away_arming_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arming away output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_arming_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arming away output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_arming_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arming away output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["away_arming_outputs"])[0] + 2];
                    //pulse_away_arming_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arming away pulsed output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_arming_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arming away pulsed output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_arming_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arming away pulsed output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_away_arming_outputs"])[0] + 2];
                    //stay_arming_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arming stay output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_arming_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arming stay output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_arming_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arming stay output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["stay_arming_outputs"])[0] + 2];
                    //pulse_stay_arming_outputs
                    databaseDataSet.Area.Rows[area_to_read]["Arming stay pulsed output - 1"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_arming_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Arming stay pulsed output - 2"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_arming_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Arming stay pulsed output - 3"] = area_arm_disarm_outputs[((int[])area.attributes["area_arm_disarm_outputs"]["pulse_stay_arming_outputs"])[0] + 2];

                    #endregion

                    #region TIMEZONES CONFIGURATION

                    //START ARM
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T1"] = (Timezone_start_arm[0] & 0x01) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T2"] = (Timezone_start_arm[0] & 0x02) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T3"] = (Timezone_start_arm[0] & 0x04) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T4"] = (Timezone_start_arm[0] & 0x08) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T5"] = (Timezone_start_arm[0] & 0x10) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T6"] = (Timezone_start_arm[0] & 0x20) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T7"] = (Timezone_start_arm[0] & 0x40) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start arm - T8"] = (Timezone_start_arm[0] & 0x80) > 0;
                    //END ARM
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T1"] = (Timezone_end_arm[0] & 0x01) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T2"] = (Timezone_end_arm[0] & 0x02) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T3"] = (Timezone_end_arm[0] & 0x04) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T4"] = (Timezone_end_arm[0] & 0x08) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T5"] = (Timezone_end_arm[0] & 0x10) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T6"] = (Timezone_end_arm[0] & 0x20) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T7"] = (Timezone_end_arm[0] & 0x40) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End arm - T8"] = (Timezone_end_arm[0] & 0x80) > 0;
                    //START DISARM
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T1"] = (Timezone_start_disarm[0] & 0x01) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T2"] = (Timezone_start_disarm[0] & 0x02) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T3"] = (Timezone_start_disarm[0] & 0x04) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T4"] = (Timezone_start_disarm[0] & 0x08) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T5"] = (Timezone_start_disarm[0] & 0x10) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T6"] = (Timezone_start_disarm[0] & 0x20) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T7"] = (Timezone_start_disarm[0] & 0x40) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["Start disarm - T8"] = (Timezone_start_disarm[0] & 0x80) > 0;
                    //END DISARM
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T1"] = (Timezone_end_disarm[0] & 0x01) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T2"] = (Timezone_end_disarm[0] & 0x02) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T3"] = (Timezone_end_disarm[0] & 0x04) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T4"] = (Timezone_end_disarm[0] & 0x08) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T5"] = (Timezone_end_disarm[0] & 0x10) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T6"] = (Timezone_end_disarm[0] & 0x20) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T7"] = (Timezone_end_disarm[0] & 0x40) > 0;
                    databaseDataSet.Area.Rows[area_to_read]["End disarm - T8"] = (Timezone_end_disarm[0] & 0x80) > 0;

                    #endregion

                    #region AUDIO TRACKS
                    if (((audio_tracks[1] << 8) + audio_tracks[0]).Equals(0xffff))
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0];
                    }
                    else
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0] + 1;
                    }

                    if (((audio_tracks[3] << 8) + audio_tracks[2]).Equals(0xffff))
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2];
                    }
                    else
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2] + 1;
                    }
                    if (((audio_tracks[5] << 8) + audio_tracks[4]).Equals(0xffff))
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4];
                    }
                    else
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4] + 1;
                    }
                    if (((audio_tracks[7] << 8) + audio_tracks[6]).Equals(0xffff))
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6];
                    }
                    else
                    {
                        databaseDataSet.Area.Rows[area_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6] + 1;
                    }

                    #endregion

                    #region AREA BEEPS OUTPUTS
                    //AWAY ARM
                    databaseDataSet.Area.Rows[area_to_read]["Away arm beeps output - 1"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_arm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Away arm beeps output - 2"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_arm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Away arm beeps output - 3"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_arm_outputs"])[0] + 2];
                    //AWAY DISARM
                    databaseDataSet.Area.Rows[area_to_read]["Away disarm beeps output - 1"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_disarm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Away disarm beeps output - 2"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_disarm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Away disarm beeps output - 3"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_away_disarm_outputs"])[0] + 2];
                    //STAY ARM
                    databaseDataSet.Area.Rows[area_to_read]["Stay arm beeps output - 1"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_arm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Stay arm beeps output - 2"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_arm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Stay arm beeps output - 3"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_arm_outputs"])[0] + 2];
                    //STAY DISARM
                    databaseDataSet.Area.Rows[area_to_read]["Stay disarm beeps output - 1"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_disarm_outputs"])[0]];
                    databaseDataSet.Area.Rows[area_to_read]["Stay disarm beeps output - 2"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_disarm_outputs"])[0] + 1];
                    databaseDataSet.Area.Rows[area_to_read]["Stay disarm beeps output - 3"] = area_beeps_arm_disarm_outputs[((int[])area.attributes["area_beeps_arm_disarm_outputs"]["beeps_stay_disarm_outputs"])[0] + 2];
                    #endregion

                    //Number
                    databaseDataSet.Area.Rows[area_to_read]["Exit timer away"] = (away_entry_delay_time[3] << 24) + (away_entry_delay_time[2] << 16) + (away_entry_delay_time[1] << 8) + away_entry_delay_time[0];
                    databaseDataSet.Area.Rows[area_to_read]["Exit timer stay"] = (stay_entry_delay_time[3] << 24) + (stay_entry_delay_time[2] << 16) + (stay_entry_delay_time[1] << 8) + stay_entry_delay_time[0];
                    databaseDataSet.Area.Rows[area_to_read]["DRCV account number"] = (DRCV_client_code[1] << 8) + DRCV_client_code[0];
                    databaseDataSet.Area.Rows[area_to_read]["Code length"] = code_length;

                    double datagrid_call_code = 0;

                    call_code = call_code.Where(val => val != 0xFF).ToArray();

                    for (int j = 0; j < call_code.Length; j++)
                    {
                        datagrid_call_code = datagrid_call_code + call_code[j] * Math.Pow(10, (call_code.Length - j - 1));
                    }

                    databaseDataSet.Area.Rows[area_to_read]["Voice call code"] = datagrid_call_code;//call_code[0] * 10000000 + call_code[1] * 1000000 + call_code[2] * 100000 + call_code[3] * 10000 + call_code[4] * 1000 + call_code[5] * 100 + call_code[6] * 10 + call_code[7];

                    databaseDataSet.Area.AcceptChanges();

                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_USERS_INIC_ADDR && addr < Constants.KP_USERS_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Users
                int user_to_read = ((addr - 0x45000) / 256);

                Protocol.Users user = new Protocol.Users();

                byte[] reserved = (byte[])user.attributes["reserved"]["value"];
                for (int i = (7 + (int)user.attributes["reserved"]["address"]), j = 0; i < (7 + (int)user.attributes["reserved"]["address"] + reserved.Length); i++, j++)
                { reserved[j] = buf[i]; }

                byte[] description = (byte[])user.attributes["description"]["value"];
                //TODO: Extract this for to a function
                for (int i = (7 + (int)user.attributes["description"]["address"]), j = 0; i < (7 + (int)user.attributes["description"]["address"] + description.Length); i++, j++)
                { description[j] = buf[i]; }

                byte[] options = (byte[])user.attributes["options"]["value"];
                for (int i = (7 + (int)user.attributes["options"]["address"]), j = 0; i < (7 + (int)user.attributes["options"]["address"] + options.Length); i++, j++)
                { options[j] = buf[i]; }

                byte[] user_code = (byte[])user.attributes["user_code"]["value"];
                for (int i = (7 + (int)user.attributes["user_code"]["address"]), j = 0; i < (7 + (int)user.attributes["user_code"]["address"] + user_code.Length); i++, j++)
                { user_code[j] = buf[i]; }

                byte[] user_type = (byte[])user.attributes["user_type"]["value"];
                for (int i = (7 + (int)user.attributes["user_type"]["address"]), j = 0; i < (7 + (int)user.attributes["user_type"]["address"] + user_type.Length); i++, j++)
                { user_type[j] = buf[i]; }

                byte[] user_partitions = (byte[])user.attributes["partition_id"]["value"];
                for (int i = (7 + (int)user.attributes["partition_id"]["address"]), j = 0; i < (7 + (int)user.attributes["partition_id"]["address"] + user_partitions.Length); i++, j++)
                { user_partitions[j] = buf[i]; }

                byte[] user_timezones_while_active = (byte[])user.attributes["timezones_while_active"]["value"];
                for (int i = (7 + (int)user.attributes["timezones_while_active"]["address"]), j = 0; i < (7 + (int)user.attributes["timezones_while_active"]["address"] + user_timezones_while_active.Length); i++, j++)
                { user_timezones_while_active[j] = buf[i]; }

                byte[] user_timezones_while_inactive = (byte[])user.attributes["timezones_while_inactive"]["value"];
                for (int i = (7 + (int)user.attributes["timezones_while_inactive"]["address"]), j = 0; i < (7 + (int)user.attributes["timezones_while_inactive"]["address"] + user_timezones_while_inactive.Length); i++, j++)
                { user_timezones_while_inactive[j] = buf[i]; }

                byte[] initial_date = (byte[])user.attributes["initial_date"]["value"];
                for (int i = (7 + (int)user.attributes["initial_date"]["address"]), j = 0; i < (7 + (int)user.attributes["initial_date"]["address"] + initial_date.Length); i++, j++)
                { initial_date[j] = buf[i]; }

                byte[] final_date = (byte[])user.attributes["final_date"]["value"];
                for (int i = (7 + (int)user.attributes["final_date"]["address"]), j = 0; i < (7 + (int)user.attributes["final_date"]["address"] + final_date.Length); i++, j++)
                { final_date[j] = buf[i]; }

                byte[] particoes_buttton_away = (byte[])user.attributes["particoes_bot_away"]["value"];
                for (int i = (7 + (int)user.attributes["particoes_bot_away"]["address"]), j = 0; i < (7 + (int)user.attributes["particoes_bot_away"]["address"] + particoes_buttton_away.Length); i++, j++)
                { particoes_buttton_away[j] = buf[i]; }

                byte[] particoes_buttton_stay = (byte[])user.attributes["particoes_bot_stay"]["value"];
                for (int i = (7 + (int)user.attributes["particoes_bot_stay"]["address"]), j = 0; i < (7 + (int)user.attributes["particoes_bot_stay"]["address"] + particoes_buttton_stay.Length); i++, j++)
                { particoes_buttton_stay[j] = buf[i]; }

                byte[] audio_tracks = (byte[])user.attributes["audio_tracks"]["value"];
                for (int i = (7 + (int)user.attributes["audio_tracks"]["address"]), j = 0; i < (7 + (int)user.attributes["audio_tracks"]["address"] + audio_tracks.Length); i++, j++)
                { audio_tracks[j] = buf[i]; }

                byte[] outputs_permissions = (byte[])user.attributes["outputs_permissions"]["value"];
                for (int i = (7 + (int)user.attributes["outputs_permissions"]["address"]), j = 0; i < (7 + (int)user.attributes["outputs_permissions"]["address"] + outputs_permissions.Length); i++, j++)
                { outputs_permissions[j] = buf[i]; }

                #region UNUSED DATA - READ FROM BUFFER
                // GROUP ID
                byte[] group_id = (byte[])user.attributes["group_id"]["value"];
                for (int i = (7 + (int)user.attributes["group_id"]["address"]), j = 0; i < (7 + (int)user.attributes["group_id"]["address"] + group_id.Length); i++, j++)
                {
                    group_id[j] = buf[i];
                }
                //USER CODE

                //byte[] full_user_code = (byte[])user.attributes["full_user_code"]["value"];
                //for (int i = (7 + (int)user.attributes["full_user_code"]["address"]), j = 0; i < (7 + (int)user.attributes["full_user_code"]["address"] + full_user_code.Length); i++, j++)
                //{
                //    full_user_code[j] = buf[i];
                //}

                #endregion

                ////TODO: Find a better solution to calculate this
                Int32 user_options = (options[3] << 24) + (options[2] << 16) + (options[1] << 8) + options[0];
                ////Get the meaning from options
                Int32 user_can_arm_away = user_options & ((int[])user.attributes["options"]["can_arm_away"])[0];
                Int32 user_can_arm_stay = user_options & ((int[])user.attributes["options"]["can_arm_stay"])[0];
                Int32 user_can_disarm_away = user_options & ((int[])user.attributes["options"]["can_disarm_away"])[0];
                Int32 user_can_disarm_stay = user_options & ((int[])user.attributes["options"]["can_disarm_stay"])[0];
                Int32 user_can_change_clock = user_options & ((int[])user.attributes["options"]["can_change_clock"])[0];
                Int32 user_check_timezones = user_options & ((int[])user.attributes["options"]["check_timezones"])[0];
                Int32 user_check_date = user_options & ((int[])user.attributes["options"]["check_date"])[0];

                #region UNUSED OPTIONS
                //////UNUSED OPTIONS/////

                Int32 USER_CAN_DIVERT = user_options & ((int[])user.attributes["options"]["can_divert"])[0];
                Int32 USER_CAN_VIEW_EVENTS = user_options & ((int[])user.attributes["options"]["can_view_events"])[0];
                Int32 USER_ACTIVE = user_options & ((int[])user.attributes["options"]["user_active"])[0];
                Int32 USER_CAN_CHANGE_OWN_CODE = user_options & ((int[])user.attributes["options"]["can_change_own_code"])[0];
                Int32 USER_CAN_CHANGE_ALL_CODES = user_options & ((int[])user.attributes["options"]["can_change_all_codes"])[0];
                Int32 USER_ALLOW_INSTALLER_MODE = user_options & ((int[])user.attributes["options"]["allow_installer_mode"])[0];
                Int32 USER_CAN_CHANGE_PHONE_NUMBERS = user_options & ((int[])user.attributes["options"]["can_change_phone_numbers"])[0];
                Int32 USER_ALLOW_DTMF_CODES = user_options & ((int[])user.attributes["options"]["allow_dtmf_codes"])[0];
                #endregion


                string string_initial_date = initial_date[0].ToString("00") + initial_date[1].ToString("00") + (initial_date[2] + (initial_date[3] << 8)).ToString("0000");
                DateTime date_initial_date = default(DateTime);

                try
                {
                    date_initial_date = DateTime.ParseExact(string_initial_date, "ddMMyyyy",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_initial_date = default(DateTime);
                }

                string string_final_date = final_date[0].ToString("00") + final_date[1].ToString("00") + (final_date[2] + (final_date[3] << 8)).ToString("0000");
                DateTime date_final_date = default(DateTime);

                try
                {
                    date_final_date = DateTime.ParseExact(string_final_date, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }
                catch
                {
                    date_final_date = default(DateTime);
                }

                try
                {

                    //String
                    databaseDataSet.User.Rows[user_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    ////Checkbox
                    databaseDataSet.User.Rows[user_to_read]["Can arm away"] = (user_can_arm_away > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can arm stay"] = (user_can_arm_stay > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can disarm away"] = (user_can_disarm_away > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can disarm stay"] = (user_can_disarm_stay > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can change clock"] = (user_can_change_clock > 0);
                    databaseDataSet.User.Rows[user_to_read]["Check timezones"] = (user_check_timezones > 0);
                    databaseDataSet.User.Rows[user_to_read]["Check date"] = (user_check_date > 0);

                    //USER CODE
                    int codecounter = 0;
                    String usercode = new String('0', codecounter);

                    for (int i = 0; i < 8; i++)
                    {
                        if (user_code[i] != 0xFF)
                        {
                            if (user_code[i] == 0)
                                usercode += '0';
                            else
                                usercode += user_code[i].ToString();

                            codecounter++;
                        }
                    }

                    //ValidatePassword(usercode);

                    databaseDataSet.User.Rows[user_to_read]["UserCode"] = usercode.PadLeft(codecounter, '0');

                    //usercode.PadRight(codecounter);

                    System.Diagnostics.Debug.WriteLine("\nUserCode: " + databaseDataSet.User.Rows[user_to_read]["UserCode"]);
                    //usercode = user_code_password;


                    //USER TYPE
                    databaseDataSet.User.Rows[user_to_read]["User type"] = user_type[0];

                    //Partitions
                    databaseDataSet.User.Rows[user_to_read]["Area 1"] = ((user_partitions[0] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 2"] = ((user_partitions[0] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 3"] = ((user_partitions[0] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 4"] = ((user_partitions[0] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 5"] = ((user_partitions[0] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 6"] = ((user_partitions[0] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 7"] = ((user_partitions[0] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Area 8"] = ((user_partitions[0] & 0x80) > 0);

                    //Timezones while active
                    databaseDataSet.User.Rows[user_to_read]["Timezone 1 while active"] = (user_timezones_while_active[0] & 0x01) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 2 while active"] = (user_timezones_while_active[0] & 0x02) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 3 while active"] = (user_timezones_while_active[0] & 0x04) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 4 while active"] = (user_timezones_while_active[0] & 0x08) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 5 while active"] = (user_timezones_while_active[0] & 0x10) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 6 while active"] = (user_timezones_while_active[0] & 0x20) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 7 while active"] = (user_timezones_while_active[0] & 0x40) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 8 while active"] = (user_timezones_while_active[0] & 0x80) > 0;

                    //Timezones while inactive
                    databaseDataSet.User.Rows[user_to_read]["Timezone 1 while inactive"] = (user_timezones_while_inactive[0] & 0x01) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 2 while inactive"] = (user_timezones_while_inactive[0] & 0x02) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 3 while inactive"] = (user_timezones_while_inactive[0] & 0x04) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 4 while inactive"] = (user_timezones_while_inactive[0] & 0x08) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 5 while inactive"] = (user_timezones_while_inactive[0] & 0x10) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 6 while inactive"] = (user_timezones_while_inactive[0] & 0x20) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 7 while inactive"] = (user_timezones_while_inactive[0] & 0x40) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Timezone 8 while inactive"] = (user_timezones_while_inactive[0] & 0x80) > 0;

                    //Initial and final dates
                    databaseDataSet.User.Rows[user_to_read]["Initial date"] = date_initial_date;
                    databaseDataSet.User.Rows[user_to_read]["Final date"] = date_final_date;

                    //Output permissions
                    databaseDataSet.User.Rows[user_to_read]["Output1Permissions"] = (outputs_permissions[0] & 0x01) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output2Permissions"] = (outputs_permissions[0] & 0x02) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output3Permissions"] = (outputs_permissions[0] & 0x04) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output4Permissions"] = (outputs_permissions[0] & 0x08) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output5Permissions"] = (outputs_permissions[0] & 0x10) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output6Permissions"] = (outputs_permissions[0] & 0x20) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output7Permissions"] = (outputs_permissions[0] & 0x40) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output8Permissions"] = (outputs_permissions[0] & 0x80) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output9Permissions"] = (outputs_permissions[1] & 0x01) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output10Permissions"] = (outputs_permissions[1] & 0x02) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output11Permissions"] = (outputs_permissions[1] & 0x04) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output12Permissions"] = (outputs_permissions[1] & 0x08) > 0;
                    databaseDataSet.User.Rows[user_to_read]["Output13Permissions"] = (outputs_permissions[1] & 0x10) > 0;

                    #region User Code

                    //databaseDataSet.User.Rows[user_to_read]["Code 1"] = user_code[0];
                    //databaseDataSet.User.Rows[user_to_read]["Code 2"] = user_code[1];
                    //databaseDataSet.User.Rows[user_to_read]["Code 3"] = user_code[2];
                    //databaseDataSet.User.Rows[user_to_read]["Code 4"] = user_code[3];
                    //databaseDataSet.User.Rows[user_to_read]["Code 5"] = user_code[4];
                    //databaseDataSet.User.Rows[user_to_read]["Code 6"] = user_code[5];
                    //databaseDataSet.User.Rows[user_to_read]["Code 7"] = user_code[6];
                    //databaseDataSet.User.Rows[user_to_read]["Code 8"] = user_code[7];

                    //int codecounter = 0;
                    //ulong user_code_tmp = 0;

                    //String usercode_full = new String(' ',8);

                    //for (int i = 0; i < 8; i++)
                    //{
                    //    if (user_code[i] != 0xFF)
                    //        user_code_password += user_code[i].ToString();
                    //}

                    //Debug.Write(user_code_password);
                    //for (int i = 0; i < user_code.Length; i++)
                    //{
                    //    if (user_code[i] != 0xFF)
                    //        codecounter++;
                    //}

                    //if (codecounter >= 4 && codecounter <= 8)
                    //{
                    //    for (int i = 0; i < codecounter; i++)
                    //        user_code_tmp += (ulong)(user_code[i] * Math.Pow(10, codecounter - 1 - i));
                    //}
                    //else user_code_tmp += 0;


                    #endregion

                    #region Buttons configuration
                    #region PARTITIONS AWAY
                    //Button A Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 1"] = ((particoes_buttton_away[0] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 2"] = ((particoes_buttton_away[0] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 3"] = ((particoes_buttton_away[0] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 4"] = ((particoes_buttton_away[0] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 5"] = ((particoes_buttton_away[0] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 6"] = ((particoes_buttton_away[0] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 7"] = ((particoes_buttton_away[0] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part away 8"] = ((particoes_buttton_away[0] & 0x80) > 0);

                    //Button B Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 1"] = ((particoes_buttton_away[1] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 2"] = ((particoes_buttton_away[1] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 3"] = ((particoes_buttton_away[1] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 4"] = ((particoes_buttton_away[1] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 5"] = ((particoes_buttton_away[1] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 6"] = ((particoes_buttton_away[1] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 7"] = ((particoes_buttton_away[1] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part away 8"] = ((particoes_buttton_away[1] & 0x80) > 0);

                    //Button C Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 1"] = ((particoes_buttton_away[2] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 2"] = ((particoes_buttton_away[2] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 3"] = ((particoes_buttton_away[2] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 4"] = ((particoes_buttton_away[2] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 5"] = ((particoes_buttton_away[2] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 6"] = ((particoes_buttton_away[2] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 7"] = ((particoes_buttton_away[2] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part away 8"] = ((particoes_buttton_away[2] & 0x80) > 0);

                    //Button D Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 1"] = ((particoes_buttton_away[3] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 2"] = ((particoes_buttton_away[3] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 3"] = ((particoes_buttton_away[3] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 4"] = ((particoes_buttton_away[3] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 5"] = ((particoes_buttton_away[3] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 6"] = ((particoes_buttton_away[3] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 7"] = ((particoes_buttton_away[3] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part away 8"] = ((particoes_buttton_away[3] & 0x80) > 0);
                    #endregion
                    #region PARTITIONS STAY
                    //Button A Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 1"] = ((particoes_buttton_stay[0] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 2"] = ((particoes_buttton_stay[0] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 3"] = ((particoes_buttton_stay[0] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 4"] = ((particoes_buttton_stay[0] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 5"] = ((particoes_buttton_stay[0] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 6"] = ((particoes_buttton_stay[0] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 7"] = ((particoes_buttton_stay[0] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button A part stay 8"] = ((particoes_buttton_stay[0] & 0x80) > 0);

                    //Button B Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 1"] = ((particoes_buttton_stay[1] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 2"] = ((particoes_buttton_stay[1] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 3"] = ((particoes_buttton_stay[1] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 4"] = ((particoes_buttton_stay[1] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 5"] = ((particoes_buttton_stay[1] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 6"] = ((particoes_buttton_stay[1] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 7"] = ((particoes_buttton_stay[1] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button B part stay 8"] = ((particoes_buttton_stay[1] & 0x80) > 0);

                    //Button C Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 1"] = ((particoes_buttton_stay[2] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 2"] = ((particoes_buttton_stay[2] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 3"] = ((particoes_buttton_stay[2] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 4"] = ((particoes_buttton_stay[2] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 5"] = ((particoes_buttton_stay[2] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 6"] = ((particoes_buttton_stay[2] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 7"] = ((particoes_buttton_stay[2] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button C part stay 8"] = ((particoes_buttton_stay[2] & 0x80) > 0);

                    //Button D Partitions away
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 1"] = ((particoes_buttton_stay[3] & 0x01) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 2"] = ((particoes_buttton_stay[3] & 0x02) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 3"] = ((particoes_buttton_stay[3] & 0x04) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 4"] = ((particoes_buttton_stay[3] & 0x08) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 5"] = ((particoes_buttton_stay[3] & 0x10) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 6"] = ((particoes_buttton_stay[3] & 0x20) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 7"] = ((particoes_buttton_stay[3] & 0x40) > 0);
                    databaseDataSet.User.Rows[user_to_read]["Button D part stay 8"] = ((particoes_buttton_stay[3] & 0x80) > 0);
                    #endregion
                    #endregion

                    #region UNUSED DATA - NEED TO BE SAVED DATA BUT NON EDITABLE
                    /////////////////////UNUSED MEMORY - Need to save it to write without changes////////////////////////
                    //OPTIONS

                    databaseDataSet.User.Rows[user_to_read]["Can divert"] = (USER_CAN_DIVERT > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can view event memory"] = (USER_CAN_VIEW_EVENTS > 0);
                    databaseDataSet.User.Rows[user_to_read]["User active"] = (USER_ACTIVE > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can change own code"] = (USER_CAN_CHANGE_OWN_CODE > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can change all codes"] = (USER_CAN_CHANGE_ALL_CODES > 0);
                    databaseDataSet.User.Rows[user_to_read]["Allow installer mode"] = (USER_ALLOW_INSTALLER_MODE > 0);
                    databaseDataSet.User.Rows[user_to_read]["Can change phone numbers"] = (USER_CAN_CHANGE_PHONE_NUMBERS > 0);
                    databaseDataSet.User.Rows[user_to_read]["Allow DTMF codes"] = (USER_ALLOW_DTMF_CODES > 0);
                    //GROUP ID
                    databaseDataSet.User.Rows[user_to_read]["Group Id"] = group_id[0];
                    //USER CODE



                    #endregion

                    //Audio tracks
                    if (((audio_tracks[1] << 8) + audio_tracks[0]).Equals(0xffff))
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0];
                    }
                    else
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0] + 1;
                    }

                    if (((audio_tracks[3] << 8) + audio_tracks[2]).Equals(0xffff))
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2];
                    }
                    else
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2] + 1;
                    }
                    if (((audio_tracks[5] << 8) + audio_tracks[4]).Equals(0xffff))
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4];
                    }
                    else
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4] + 1;
                    }
                    if (((audio_tracks[7] << 8) + audio_tracks[6]).Equals(0xffff))
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6];
                    }
                    else
                    {
                        databaseDataSet.User.Rows[user_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6] + 1;
                    }
                    
                    databaseDataSet.User.Rows[user_to_read]["Reserved"] = (int)reserved[0];

                    databaseDataSet.User.AcceptChanges();
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                #endregion
            }
            else if (addr >= Constants.KP_KEYPADS_INIC_ADDR && addr < Constants.KP_KEYPADS_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Keypads
                int keypad_to_read = ((addr - 0x1000) / 512);

                Protocol.Keypads keypad = new Protocol.Keypads();
                byte[] description = (byte[])keypad.attributes["description"]["value"];
                for (int i = (7 + (int)keypad.attributes["description"]["address"]), j = 0; i < (7 + (int)keypad.attributes["description"]["address"] + description.Length); i++, j++)
                {
                    description[j] = buf[i];
                }

                byte[] keypad_reserved_0 = (byte[])keypad.attributes["reserved_0"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_0"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_0"]["address"] + keypad_reserved_0.Length); i++, j++)
                {
                    keypad_reserved_0[j] = buf[i];
                }

                byte[] keypad_reserved_1 = (byte[])keypad.attributes["reserved_1"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_1"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_1"]["address"] + keypad_reserved_1.Length); i++, j++)
                {
                    keypad_reserved_1[j] = buf[i];
                }

                byte[] keypad_reserved_2 = (byte[])keypad.attributes["reserved_2"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_2"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_2"]["address"] + keypad_reserved_2.Length); i++, j++)
                {
                    keypad_reserved_2[j] = buf[i];
                }

                byte[] keypad_reserved_3 = (byte[])keypad.attributes["reserved_3"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_3"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_3"]["address"] + keypad_reserved_3.Length); i++, j++)
                {
                    keypad_reserved_3[j] = buf[i];
                }

                byte[] keypad_reserved_4 = (byte[])keypad.attributes["reserved_4"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_4"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_4"]["address"] + keypad_reserved_4.Length); i++, j++)
                {
                    keypad_reserved_4[j] = buf[i];
                }

                byte[] keypad_partitions = (byte[])keypad.attributes["partition_id"]["value"];
                for (int i = (7 + (int)keypad.attributes["partition_id"]["address"]), j = 0; i < (7 + (int)keypad.attributes["partition_id"]["address"] + keypad_partitions.Length); i++, j++)
                {
                    keypad_partitions[j] = buf[i];
                }

                byte[] keypad_report_partitions = (byte[])keypad.attributes["report_partition_id"]["value"];
                for (int i = (7 + (int)keypad.attributes["report_partition_id"]["address"]), j = 0; i < (7 + (int)keypad.attributes["report_partition_id"]["address"] + keypad_report_partitions.Length); i++, j++)
                {
                    keypad_report_partitions[j] = buf[i];
                }

                byte[] keypad_lcd_backlight_timeout = (byte[])keypad.attributes["lcd_backlight_timeout"]["value"];
                for (int i = (7 + (int)keypad.attributes["lcd_backlight_timeout"]["address"]), j = 0; i < (7 + (int)keypad.attributes["lcd_backlight_timeout"]["address"] + keypad_lcd_backlight_timeout.Length); i++, j++)
                {
                    keypad_lcd_backlight_timeout[j] = buf[i];
                }

                byte[] keypad_reserved_6 = (byte[])keypad.attributes["reserved_6"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_6"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_6"]["address"] + keypad_reserved_6.Length); i++, j++)
                {
                    keypad_reserved_6[j] = buf[i];
                }

                byte[] keypad_reserved_7 = (byte[])keypad.attributes["reserved_7"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_7"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_7"]["address"] + keypad_reserved_7.Length); i++, j++)
                {
                    keypad_reserved_7[j] = buf[i];
                }

                byte[] keypad_reserved_8 = (byte[])keypad.attributes["reserved_8"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_8"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_8"]["address"] + keypad_reserved_8.Length); i++, j++)
                {
                    keypad_reserved_8[j] = buf[i];
                }

                byte[] keypad_reserved_9 = (byte[])keypad.attributes["reserved_9"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_9"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_9"]["address"] + keypad_reserved_9.Length); i++, j++)
                {
                    keypad_reserved_9[j] = buf[i];
                }

                byte[] keypad_reserved_10 = (byte[])keypad.attributes["reserved_10"]["value"];
                for (int i = (7 + (int)keypad.attributes["reserved_10"]["address"]), j = 0; i < (7 + (int)keypad.attributes["reserved_10"]["address"] + keypad_reserved_10.Length); i++, j++)
                {
                    keypad_reserved_10[j] = buf[i];
                }

                byte[] options = (byte[])keypad.attributes["options"]["value"];
                for (int i = (7 + (int)keypad.attributes["options"]["address"]), j = 0; i < (7 + (int)keypad.attributes["options"]["address"] + options.Length); i++, j++)
                {
                    options[j] = buf[i];
                }

                byte[] keypad_language = (byte[])keypad.attributes["language"]["value"];
                for (int i = (7 + (int)keypad.attributes["language"]["address"]), j = 0; i < (7 + (int)keypad.attributes["language"]["address"] + keypad_language.Length); i++, j++)
                {
                    keypad_language[j] = (byte)buf[i];
                }

                byte[] keypad_button_config_options = (byte[])keypad.attributes["lcd_config_options"]["value"];
                for (int i = (7 + (int)keypad.attributes["lcd_config_options"]["address"]), j = 0; i < (7 + (int)keypad.attributes["lcd_config_options"]["address"] + keypad_button_config_options.Length); i++, j++)
                {
                    keypad_button_config_options[j] = (byte)buf[i];
                }

                byte[] audio_tracks = (byte[])keypad.attributes["audio_tracks"]["value"];
                for (int i = (7 + (int)keypad.attributes["audio_tracks"]["address"]), j = 0; i < (7 + (int)keypad.attributes["audio_tracks"]["address"] + audio_tracks.Length); i++, j++)
                {
                    audio_tracks[j] = buf[i];
                }


                UInt64 keyboard_options = ((((ulong)(options[7] << 24) + (ulong)(options[6] << 16) + (ulong)(options[5] << 8) + (options[4])) << 32) + ((ulong)(options[3] << 24) + (ulong)(options[2] << 16) + (ulong)(options[1] << 8) + (ulong)options[0]));

                UInt64 keypad_language_option = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_IDIOMA"])[0];
                UInt64 keypad_hour_type = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_TIPO_HORA"])[0];
                UInt64 keypad_date_type = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_TIPO_DATA"])[0];

                UInt64 keypad_backlight_off_when_armed = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BACKLIGHT_OFF_WHEN_ARMED"])[0];
                UInt64 keypad_backlight_off_main_power_fail = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BACKLIGHT_OFF_MAIN_POWER_FAIL"])[0];
                UInt64 keypad_beeps_alarm = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_ALARM"])[0];
                UInt64 keypad_beeps_medical = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_MEDICAL"])[0];
                UInt64 keypad_beeps_fire = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_FIRE"])[0];
                UInt64 keypad_beeps_panic = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_PANIC"])[0];
                UInt64 keypad_beeps_main_power_fail = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_MAIN_POWER_FAIL"])[0];
                UInt64 keypad_beeps_output_fuse_fail = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_OUTPUT_FUSE_FAIL"])[0];
                UInt64 keypad_beeps_battery_low = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_BATTERY_LOW"])[0];
                UInt64 keypad_beeps_line_fail = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_LINE_FAIL"])[0];
                UInt64 keypad_beeps_system_tamper = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_SYSTEM_TAMPER"])[0];
                UInt64 keypad_beeps_receiver_fails = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_RECEIVER_FAILS"])[0];
                UInt64 keypad_can_arm_away = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_CAN_ARM_AWAY"])[0];
                UInt64 keypad_can_arm_stay = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_CAN_ARM_STAY"])[0];
                UInt64 keypad_disarm_away_all_times = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_DISARM_AWAY_ALL_TIMES"])[0];
                UInt64 keypad_disarm_stay_all_times = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_DISARM_STAY_ALL_TIMES"])[0];
                UInt64 keypad_disarm_away_during_exit_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_DISARM_AWAY_DURING_EXIT_TIME"])[0];
                UInt64 keypad_disarm_stay_during_exit_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_DISARM_STAY_DURING_EXIT_TIME"])[0];
                UInt64 keypad_silent_during_entry_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_SILENT_DURING_ENTRY_TIME"])[0];
                UInt64 keypad_silent_during_exit_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_SILENT_DURING_EXIT_TIME"])[0];
                UInt64 keypad_reset_alarms = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_RESET_ALARMS"])[0];
                UInt64 keypad_show_area_name = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_SHOW_AREA_NAME"])[0];
                UInt64 keypad_enable_keypad_tamper = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_ENABLE_KEYPAD_TAMPER"])[0];
                UInt64 keypad_no_keypad_indications_while_armed = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_NO_KEYPAD_INDICATIONS_WHILE_ARMED"])[0];
                UInt64 keypad_beeps_functions = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_FUNCOES"])[0];
                UInt64 keypad_exit_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_EXIT_TIME"])[0];
                UInt64 keypad_beeps_entry_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_ENTRY_TIME"])[0];
                UInt64 keypad_beeps_sensor_time = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_BEEPS_SENSOR_WATCH"])[0];
                UInt64 keypad_active = keyboard_options & ((ulong[])keypad.attributes["options"]["KP_KPAD_RS485_ATIVO"])[0];

                try
                {

                    //String
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    //Reserved  0-4
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 0"] = keypad_reserved_0;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 1"] = keypad_reserved_1;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 2"] = keypad_reserved_2;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 3"] = keypad_reserved_3;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 4"] = keypad_reserved_4;

                    ////Partitions
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 1"] = ((keypad_partitions[0] & 0x01) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 2"] = ((keypad_partitions[0] & 0x02) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 3"] = ((keypad_partitions[0] & 0x04) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 4"] = ((keypad_partitions[0] & 0x08) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 5"] = ((keypad_partitions[0] & 0x10) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 6"] = ((keypad_partitions[0] & 0x20) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 7"] = ((keypad_partitions[0] & 0x40) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Area 8"] = ((keypad_partitions[0] & 0x80) > 0);

                    ////Report Partitions
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 1"] = ((keypad_report_partitions[0] & 0x01) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 2"] = ((keypad_report_partitions[0] & 0x02) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 3"] = ((keypad_report_partitions[0] & 0x04) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 4"] = ((keypad_report_partitions[0] & 0x08) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 5"] = ((keypad_report_partitions[0] & 0x10) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 6"] = ((keypad_report_partitions[0] & 0x20) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 7"] = ((keypad_report_partitions[0] & 0x40) > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Report area 8"] = ((keypad_report_partitions[0] & 0x80) > 0);

                    //LCD Backlight timeout
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Backlight time"] = keypad_lcd_backlight_timeout[0];

                    //Reserved  6-10
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 6"] = keypad_reserved_6;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 7"] = keypad_reserved_7;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 8"] = keypad_reserved_8;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 9"] = keypad_reserved_9;
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reserved 10"] = keypad_reserved_10;

                    //////OPTIONS
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Language option"] = (keypad_language_option > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Hour format"] = (keypad_hour_type > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Date format"] = (keypad_date_type > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Backlight off when armed"] = (keypad_backlight_off_when_armed > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Backlight off on 230V fail"] = (keypad_backlight_off_main_power_fail > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on alarm"] = (keypad_beeps_alarm > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on medical alarm"] = (keypad_beeps_medical > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on fire alarm"] = (keypad_beeps_fire > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on panic alarm"] = (keypad_beeps_panic > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on 230V fail"] = (keypad_beeps_main_power_fail > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on output fuse fail"] = (keypad_beeps_output_fuse_fail > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on battery low"] = (keypad_beeps_battery_low > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on PSTN fail"] = (keypad_beeps_line_fail > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on system tamper alarm"] = (keypad_beeps_system_tamper > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on receiver fail"] = (keypad_beeps_receiver_fails > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Can arm away"] = (keypad_can_arm_away > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Can arm stay"] = (keypad_can_arm_stay > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Can disarm away"] = (keypad_disarm_away_all_times > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Can disarm stay"] = (keypad_disarm_stay_all_times > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Can disarm away during exit time"] = (keypad_disarm_away_during_exit_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Can disarm stay during exit time"] = (keypad_disarm_stay_during_exit_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Silent during entry time"] = (keypad_silent_during_entry_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Silent during exit time"] = (keypad_silent_during_exit_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Reset alarms"] = (keypad_reset_alarms > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Show area name"] = (keypad_show_area_name > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Enable keypad tamper"] = (keypad_enable_keypad_tamper > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["No indications while armed"] = (keypad_no_keypad_indications_while_armed > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on functions"] = (keypad_beeps_functions > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on exit time"] = (keypad_exit_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on entry time"] = (keypad_beeps_entry_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Beeps on sensor watch"] = (keypad_beeps_sensor_time > 0);
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Active"] = (keypad_active > 0);

                    //Keypad Language
                    databaseDataSet.Keypad.Rows[keypad_to_read]["Language"] = keypad_language;

                    //Keypad Button options
                    databaseDataSet.Keypad.Rows[keypad_to_read]["F1 button"] = keypad_button_config_options[0] + keypad_button_config_options[1];
                    databaseDataSet.Keypad.Rows[keypad_to_read]["F2 button"] = keypad_button_config_options[2] + keypad_button_config_options[3];
                    databaseDataSet.Keypad.Rows[keypad_to_read]["F3 button"] = keypad_button_config_options[4] + keypad_button_config_options[5];
                    databaseDataSet.Keypad.Rows[keypad_to_read]["F4 button"] = keypad_button_config_options[6] + keypad_button_config_options[7];

                    //Audio tracks
                    if (((audio_tracks[1] << 8) + audio_tracks[0]).Equals(0xffff))
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0];
                    }
                    else
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0] + 1;
                    }

                    if (((audio_tracks[3] << 8) + audio_tracks[2]).Equals(0xffff))
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2];
                    }
                    else
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2] + 1;
                    }
                    if (((audio_tracks[5] << 8) + audio_tracks[4]).Equals(0xffff))
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4];
                    }
                    else
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4] + 1;
                    }
                    if (((audio_tracks[7] << 8) + audio_tracks[6]).Equals(0xffff))
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6];
                    }
                    else
                    {
                        databaseDataSet.Keypad.Rows[keypad_to_read]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6] + 1;
                    }

                    databaseDataSet.Keypad.AcceptChanges();
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_OUTPUTS_INIC_ADDR && addr < Constants.KP_OUTPUTS_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Outputs
                int output_to_read = ((addr - 0x72000) / 256);

                Protocol.Outputs output = new Protocol.Outputs();

                byte[] description = (byte[])output.attributes["description"]["value"];
                //TODO: Extract this for to a function
                for (int i = (7 + (int)output.attributes["description"]["address"]), j = 0; i < (7 + description.Length); i++, j++)
                {
                    description[j] = buf[i];
                }

                byte[] output_device_id = (byte[])output.attributes["device_id"]["value"];
                for (int i = (7 + (int)output.attributes["device_id"]["address"]), j = 0; i < (7 + (int)output.attributes["device_id"]["address"] + output_device_id.Length); i++, j++)
                {
                    output_device_id[j] = buf[i];
                }

                byte[] output_index = (byte[])output.attributes["index"]["value"];
                for (int i = (7 + (int)output.attributes["index"]["address"]), j = 0; i < (7 + (int)output.attributes["index"]["address"] + output_index.Length); i++, j++)
                {
                    output_index[j] = buf[i];
                }

                byte[] output_delay_ON = (byte[])output.attributes["output_on_delay"]["value"];
                for (int i = (7 + (int)output.attributes["output_on_delay"]["address"]), j = 0; i < (7 + (int)output.attributes["output_on_delay"]["address"] + output_delay_ON.Length); i++, j++)
                {
                    output_delay_ON[j] = buf[i];
                }

                byte[] output_time_ON = (byte[])output.attributes["output_on_time"]["value"];
                for (int i = (7 + (int)output.attributes["output_on_time"]["address"]), j = 0; i < (7 + (int)output.attributes["output_on_time"]["address"] + output_time_ON.Length); i++, j++)
                {
                    output_time_ON[j] = buf[i];
                }

                byte[] output_reset_time = (byte[])output.attributes["reset_state_time"]["value"];
                for (int i = (7 + (int)output.attributes["reset_state_time"]["address"]), j = 0; i < (7 + (int)output.attributes["reset_state_time"]["address"] + output_reset_time.Length); i++, j++)
                {
                    output_reset_time[j] = buf[i];
                }

                byte[] output_chime_time = (byte[])output.attributes["chime_time"]["value"];
                for (int i = (7 + (int)output.attributes["chime_time"]["address"]), j = 0; i < (7 + (int)output.attributes["chime_time"]["address"] + output_chime_time.Length); i++, j++)
                {
                    output_chime_time[j] = buf[i];
                }

                byte[] output_chime_delay = (byte[])output.attributes["chime_delay"]["value"];
                for (int i = (7 + (int)output.attributes["chime_delay"]["address"]), j = 0; i < (7 + (int)output.attributes["chime_delay"]["address"] + output_chime_delay.Length); i++, j++)
                {
                    output_chime_delay[j] = buf[i];
                }

                byte[] output_pulses_number = (byte[])output.attributes["pulses_number"]["value"];
                for (int i = (7 + (int)output.attributes["pulses_number"]["address"]), j = 0; i < (7 + (int)output.attributes["pulses_number"]["address"] + output_pulses_number.Length); i++, j++)
                {
                    output_pulses_number[j] = buf[i];
                }

                byte[] output_disarm_beeps = (byte[])output.attributes["disarm_beeps"]["value"];
                for (int i = (7 + (int)output.attributes["disarm_beeps"]["address"]), j = 0; i < (7 + (int)output.attributes["disarm_beeps"]["address"] + output_disarm_beeps.Length); i++, j++)
                {
                    output_disarm_beeps[j] = buf[i];
                }

                byte[] output_arm_beeps = (byte[])output.attributes["arm_beeps"]["value"];
                for (int i = (7 + (int)output.attributes["arm_beeps"]["address"]), j = 0; i < (7 + (int)output.attributes["arm_beeps"]["address"] + output_arm_beeps.Length); i++, j++)
                {
                    output_arm_beeps[j] = buf[i];
                }

                byte[] output_timezones = (byte[])output.attributes["timezones"]["value"];
                for (int i = (7 + (int)output.attributes["timezones"]["address"]), j = 0; i < (7 + (int)output.attributes["timezones"]["address"] + output_timezones.Length); i++, j++)
                {
                    output_timezones[j] = buf[i];
                }

                byte[] output_pulse_time_on = (byte[])output.attributes["Time_on"]["value"];
                for (int i = (7 + (int)output.attributes["Time_on"]["address"]), j = 0; i < (7 + (int)output.attributes["Time_on"]["address"] + output_pulse_time_on.Length); i++, j++)
                {
                    output_pulse_time_on[j] = buf[i];
                }

                byte[] output_pulse_time_off = (byte[])output.attributes["Time_off"]["value"];
                for (int i = (7 + (int)output.attributes["Time_off"]["address"]), j = 0; i < (7 + (int)output.attributes["Time_off"]["address"] + output_pulse_time_off.Length); i++, j++)
                {
                    output_pulse_time_off[j] = buf[i];
                }

                byte[] options = (byte[])output.attributes["options"]["value"];
                for (int i = (7 + (int)output.attributes["options"]["address"]), j = 0; i < (7 + (int)output.attributes["options"]["address"] + options.Length); i++, j++)
                {
                    options[j] = buf[i];
                }

                UInt64 output_options = ((((ulong)(options[7] << 24) + (ulong)(options[6] << 16) + (ulong)(options[5] << 8) + (options[4])) << 32) + ((ulong)(options[3] << 24) + (ulong)(options[2] << 16) + (ulong)(options[1] << 8) + (ulong)options[0]));
                //ulong output_options = (ulong)((((ulong)options[7] << 24) + (ulong)(options[6] << 16) + (ulong)(options[5] << 8) + (ulong)(options[4]) << 32) + (ulong)(options[3] << 24) + (ulong)(options[2] << 16) + (ulong)(options[1] << 8) + (ulong)options[0]);
                //Get the meaning from options
                UInt64 output_inverted_option = output_options & ((ulong[])output.attributes["options"]["KP_OUTPUTS_OPCOES_SAIDA_INVERTIDA"])[0];
                UInt64 output_pulsed_option = output_options & ((ulong[])output.attributes["options"]["KP_OUTPUTS_OPCOES_SAIDA_COM_IMPULSO"])[0];
                UInt64 output_source_option = output_options & ((ulong[])output.attributes["options"]["KP_OUTPUTS_OPCOES_SAIDA_SOURCE"])[0];

                try
                {

                    if (output_to_read > Constants.KP_MAX_OUTPUTS)
                        output_to_read = Constants.KP_MAX_OUTPUTS;

                    //Device Id
                    databaseDataSet.Output.Rows[output_to_read]["Device Id"] = output_device_id;

                    //Index
                    databaseDataSet.Output.Rows[output_to_read]["Output index"] = output_index;

                    //Output Delay - On
                    databaseDataSet.Output.Rows[output_to_read]["Output delay - ON"] = output_delay_ON[0] + output_delay_ON[1];

                    //Time - On
                    databaseDataSet.Output.Rows[output_to_read]["Time - ON"] = output_time_ON[0] + output_time_ON[1];

                    //Reset Time
                    databaseDataSet.Output.Rows[output_to_read]["Reset time"] = output_reset_time[0] + output_reset_time[1];

                    //Chime Time
                    databaseDataSet.Output.Rows[output_to_read]["Chime time"] = output_chime_time[0] + output_chime_time[1];

                    //Chime delay
                    databaseDataSet.Output.Rows[output_to_read]["Chime delay"] = output_chime_delay[0] + output_chime_delay[1];

                    //Number of Pulses
                    databaseDataSet.Output.Rows[output_to_read]["Pulses number"] = output_pulses_number[0];

                    //Disarm beeps
                    databaseDataSet.Output.Rows[output_to_read]["Beeps number - Disarm"] = output_disarm_beeps[0];

                    //Arm beeps
                    databaseDataSet.Output.Rows[output_to_read]["Beeps number - Arm"] = output_arm_beeps[0];

                    //Timezones
                    databaseDataSet.Output.Rows[output_to_read]["Timezone1"] = (output_timezones[0] & 0x01) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone2"] = (output_timezones[0] & 0x02) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone3"] = (output_timezones[0] & 0x04) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone4"] = (output_timezones[0] & 0x08) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone5"] = (output_timezones[0] & 0x10) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone6"] = (output_timezones[0] & 0x20) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone7"] = (output_timezones[0] & 0x40) > 0;
                    databaseDataSet.Output.Rows[output_to_read]["Timezone8"] = (output_timezones[0] & 0x80) > 0;

                    // Time on
                    databaseDataSet.Output.Rows[output_to_read]["Pulse time on"] = output_pulse_time_on[0] + output_pulse_time_on[1];

                    // Time off
                    databaseDataSet.Output.Rows[output_to_read]["Pulse time off"] = output_pulse_time_off[0] + output_pulse_time_off[1];

                    ////////OPTIONS
                    databaseDataSet.Output.Rows[output_to_read]["Inverted"] = (output_inverted_option > 0);
                    databaseDataSet.Output.Rows[output_to_read]["Pulsed"] = (output_pulsed_option > 0);
                    databaseDataSet.Output.Rows[output_to_read]["Source"] = (output_source_option > 0);

                    //Description
                    databaseDataSet.Output.Rows[output_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    databaseDataSet.Output.AcceptChanges();
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_TIMEZONES_INIC_ADDR && addr < Constants.KP_TIMEZONES_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Timezones
                int timezone_to_read = ((addr - 0x65000) / 512);

                Protocol.Timezones timezone = new Protocol.Timezones();

                byte[] description = (byte[])timezone.attributes["description"]["value"];
                for (int i = (7 + (int)timezone.attributes["description"]["address"]), j = 0; i < (7 + (int)timezone.attributes["description"]["address"] + description.Length); i++, j++)
                {
                    description[j] = buf[i];
                }

                byte[] inicial_hour = (byte[])timezone.attributes["inicial_hour"]["value"];
                for (int i = (7 + (int)timezone.attributes["inicial_hour"]["address"]), j = 0; i < (7 + (int)timezone.attributes["inicial_hour"]["address"] + inicial_hour.Length); i++, j++)
                {
                    inicial_hour[j] = buf[i];
                }

                byte[] inicial_minute = (byte[])timezone.attributes["inicial_minute"]["value"];
                for (int i = (7 + (int)timezone.attributes["inicial_minute"]["address"]), j = 0; i < (7 + (int)timezone.attributes["inicial_minute"]["address"] + inicial_minute.Length); i++, j++)
                {
                    inicial_minute[j] = buf[i];
                }

                byte[] final_hour = (byte[])timezone.attributes["final_hour"]["value"];
                for (int i = (7 + (int)timezone.attributes["final_hour"]["address"]), j = 0; i < (7 + (int)timezone.attributes["final_hour"]["address"] + final_hour.Length); i++, j++)
                {
                    final_hour[j] = buf[i];
                }

                byte[] final_minute = (byte[])timezone.attributes["final_minute"]["value"];
                for (int i = (7 + (int)timezone.attributes["final_minute"]["address"]), j = 0; i < (7 + (int)timezone.attributes["final_minute"]["address"] + final_minute.Length); i++, j++)
                {
                    final_minute[j] = buf[i];
                }

                byte[] week_days = (byte[])timezone.attributes["week_days"]["value"];
                for (int i = (7 + (int)timezone.attributes["week_days"]["address"]), j = 0; i < (7 + (int)timezone.attributes["week_days"]["address"] + week_days.Length); i++, j++)
                {
                    week_days[j] = buf[i];
                }

                byte[] excluded_days = (byte[])timezone.attributes["excluded_days"]["value"];
                for (int i = (7 + (int)timezone.attributes["excluded_days"]["address"]), j = 0; i < (7 + (int)timezone.attributes["excluded_days"]["address"] + excluded_days.Length); i++, j++)
                {
                    excluded_days[j] = buf[i];
                }

                byte[] reserved = (byte[])timezone.attributes["reserved"]["value"];
                for (int i = (7 + (int)timezone.attributes["reserved"]["address"]), j = 0; i < (7 + (int)timezone.attributes["reserved"]["address"] + reserved.Length); i++, j++)
                {
                    reserved[j] = buf[i];
                }

                byte[] excluded_initial_month = (byte[])timezone.attributes["excluded_initial_month"]["value"];
                for (int i = (7 + (int)timezone.attributes["excluded_initial_month"]["address"]), j = 0; i < (7 + (int)timezone.attributes["excluded_initial_month"]["address"] + excluded_initial_month.Length); i++, j++)
                {
                    excluded_initial_month[j] = buf[i];
                }

                byte[] excluded_initial_day = (byte[])timezone.attributes["excluded_initial_day"]["value"];
                for (int i = (7 + (int)timezone.attributes["excluded_initial_day"]["address"]), j = 0; i < (7 + (int)timezone.attributes["excluded_initial_day"]["address"] + excluded_initial_day.Length); i++, j++)
                {
                    excluded_initial_day[j] = buf[i];
                }

                byte[] excluded_final_month = (byte[])timezone.attributes["excluded_final_month"]["value"];
                for (int i = (7 + (int)timezone.attributes["excluded_final_month"]["address"]), j = 0; i < (7 + (int)timezone.attributes["excluded_final_month"]["address"] + excluded_final_month.Length); i++, j++)
                {
                    excluded_final_month[j] = buf[i];
                }

                byte[] excluded_final_day = (byte[])timezone.attributes["excluded_final_day"]["value"];
                for (int i = (7 + (int)timezone.attributes["excluded_final_day"]["address"]), j = 0; i < (7 + (int)timezone.attributes["excluded_final_day"]["address"] + excluded_final_day.Length); i++, j++)
                {
                    excluded_final_day[j] = buf[i];
                }

                //Join initial hour with initial minutes
                string string_initial_hour = inicial_hour[0].ToString("00") + inicial_minute[0].ToString("00");
                TimeSpan date_initial_hour = default(TimeSpan);

                try
                {
                    date_initial_hour = new TimeSpan(inicial_hour[0], inicial_minute[0], 0);
                }
                catch
                {
                    date_initial_hour = default(TimeSpan);
                }

                //Join final hour with final minutes
                string string_final_hour = final_hour[0].ToString("00") + final_minute[0].ToString("00");
                TimeSpan date_final_hour = default(TimeSpan);

                try
                {
                    date_final_hour = new TimeSpan(final_hour[0], final_minute[0], 0);
                }
                catch
                {
                    date_final_hour = default(TimeSpan);
                }

                //Join initial month with initial day
                // 1
                string string_excluded_initial_month_1 = excluded_initial_day[0].ToString("00") + excluded_initial_month[0].ToString("00");
                DateTime date_excluded_initial_month_1 = default(DateTime);

                try
                {
                    date_excluded_initial_month_1 = DateTime.ParseExact(string_excluded_initial_month_1, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_initial_month_1 = default(DateTime);
                }
                // 2
                string string_excluded_initial_month_2 = excluded_initial_day[1].ToString("00") + excluded_initial_month[1].ToString("00");
                DateTime date_excluded_initial_month_2 = default(DateTime);

                try
                {
                    date_excluded_initial_month_2 = DateTime.ParseExact(string_excluded_initial_month_2, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_initial_month_2 = default(DateTime);
                }
                // 3
                string string_excluded_initial_month_3 = excluded_initial_day[2].ToString("00") + excluded_initial_month[2].ToString("00");
                DateTime date_excluded_initial_month_3 = default(DateTime);

                try
                {
                    date_excluded_initial_month_3 = DateTime.ParseExact(string_excluded_initial_month_3, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_initial_month_3 = default(DateTime);
                }
                // 4
                string string_excluded_initial_month_4 = excluded_initial_day[3].ToString("00") + excluded_initial_month[3].ToString("00");
                DateTime date_excluded_initial_month_4 = default(DateTime);

                try
                {
                    date_excluded_initial_month_4 = DateTime.ParseExact(string_excluded_initial_month_4, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_initial_month_4 = default(DateTime);
                }

                //Join final month with final day
                // 1
                string string_excluded_final_month_1 = excluded_final_day[0].ToString("00") + excluded_final_month[0].ToString("00");
                DateTime date_excluded_final_month_1 = default(DateTime);

                try
                {
                    date_excluded_final_month_1 = DateTime.ParseExact(string_excluded_final_month_1, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_final_month_1 = default(DateTime);
                }
                // 2
                string string_excluded_final_month_2 = excluded_final_day[1].ToString("00") + excluded_final_month[1].ToString("00");
                DateTime date_excluded_final_month_2 = default(DateTime);

                try
                {
                    date_excluded_final_month_2 = DateTime.ParseExact(string_excluded_final_month_2, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_final_month_2 = default(DateTime);
                }
                // 3
                string string_excluded_final_month_3 = excluded_final_day[2].ToString("00") + excluded_final_month[2].ToString("00");
                DateTime date_excluded_final_month_3 = default(DateTime);

                try
                {
                    date_excluded_final_month_3 = DateTime.ParseExact(string_excluded_final_month_3, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_final_month_3 = default(DateTime);
                }
                // 4
                string string_excluded_final_month_4 = excluded_final_day[3].ToString("00") + excluded_final_month[3].ToString("00");
                DateTime date_excluded_final_month_4 = default(DateTime);

                try
                {
                    date_excluded_final_month_4 = DateTime.ParseExact(string_excluded_final_month_4, "ddMM",
                          CultureInfo.InvariantCulture,
                          DateTimeStyles.None);
                }
                catch
                {
                    date_excluded_final_month_4 = default(DateTime);
                }


                try
                {

                    //String
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    // Week days
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Monday"] = ((week_days[0] & 0x01) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Tuesday"] = ((week_days[0] & 0x02) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Wednesday"] = ((week_days[0] & 0x04) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Thursday"] = ((week_days[0] & 0x08) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Friday"] = ((week_days[0] & 0x10) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Saturday"] = ((week_days[0] & 0x20) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Sunday"] = ((week_days[0] & 0x40) > 0);

                    //Initial hour
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Initial hour"] = date_initial_hour;

                    //Final hour
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Final hour"] = date_final_hour;

                    //Excluded days
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 1"] = ((excluded_days[0] & 0x01) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 2"] = ((excluded_days[0] & 0x02) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 3"] = ((excluded_days[0] & 0x04) > 0);
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 4"] = ((excluded_days[0] & 0x08) > 0);

                    //Reserved
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Reserved"] = reserved;

                    //Excluded initial date
                    //1
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 1 initial date"] = date_excluded_initial_month_1;
                    //2
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 2 initial date"] = date_excluded_initial_month_2;
                    //3
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 3 initial date"] = date_excluded_initial_month_3;
                    //4
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 4 initial date"] = date_excluded_initial_month_4;

                    //Excluded final date 
                    //1
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 1 final date"] = date_excluded_final_month_1;
                    //2
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 2 final date"] = date_excluded_final_month_2;
                    //3
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 3 final date"] = date_excluded_final_month_3;
                    //4
                    databaseDataSet.Timezone.Rows[timezone_to_read]["Exception 4 final date"] = date_excluded_final_month_4;

                    databaseDataSet.Timezone.AcceptChanges();

                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_PHONES_INIC_ADDR && addr < Constants.KP_PHONES_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Phones
                int phone_to_read = ((addr - 0x6A000) / 256);

                Protocol.Phones phone = new Protocol.Phones();

                byte[] description = (byte[])phone.attributes["description"]["value"];
                for (int i = (7 + (int)phone.attributes["description"]["address"]), j = 0; i < (7 + (int)phone.attributes["description"]["address"] + description.Length); i++, j++)
                {
                    description[j] = buf[i];
                }

                byte[] phone_number = (byte[])phone.attributes["phone_number"]["value"];
                for (int i = (7 + (int)phone.attributes["phone_number"]["address"]), j = 0; i < (7 + (int)phone.attributes["phone_number"]["address"] + phone_number.Length); i++, j++)
                {
                    phone_number[j] = buf[i];
                }

                byte[] prefix_number = (byte[])phone.attributes["prefix"]["value"];
                for (int i = (7 + (int)phone.attributes["prefix"]["address"]), j = 0; i < (7 + (int)phone.attributes["prefix"]["address"] + prefix_number.Length); i++, j++)
                {
                    prefix_number[j] = buf[i];
                }

                byte[] max_com_attempts = (byte[])phone.attributes["max_com_attempts"]["value"];
                for (int i = (7 + (int)phone.attributes["max_com_attempts"]["address"]), j = 0; i < (7 + (int)phone.attributes["max_com_attempts"]["address"] + max_com_attempts.Length); i++, j++)
                {
                    max_com_attempts[j] = buf[i];
                }

                byte[] partitions = (byte[])phone.attributes["partition_id"]["value"];
                for (int i = (7 + (int)phone.attributes["partition_id"]["address"]), j = 0; i < (7 + (int)phone.attributes["partition_id"]["address"] + partitions.Length); i++, j++)
                {
                    partitions[j] = buf[i];
                }

                byte[] options = (byte[])phone.attributes["options"]["value"];
                for (int i = (7 + (int)phone.attributes["options"]["address"]), j = 0; i < (7 + (int)phone.attributes["options"]["address"] + options.Length); i++, j++)
                {
                    options[j] = buf[i];
                }

                byte[] hour_test = (byte[])phone.attributes["hour_test"]["value"];
                for (int i = (7 + (int)phone.attributes["hour_test"]["address"]), j = 0; i < (7 + (int)phone.attributes["hour_test"]["address"] + hour_test.Length); i++, j++)
                {
                    hour_test[j] = buf[i];
                }

                byte[] minute_test = (byte[])phone.attributes["minute_test"]["value"];
                for (int i = (7 + (int)phone.attributes["minute_test"]["address"]), j = 0; i < (7 + (int)phone.attributes["minute_test"]["address"] + minute_test.Length); i++, j++)
                {
                    minute_test[j] = buf[i];
                }

                byte[] week_days = (byte[])phone.attributes["week_days"]["value"];
                for (int i = (7 + (int)phone.attributes["week_days"]["address"]), j = 0; i < (7 + (int)phone.attributes["week_days"]["address"] + week_days.Length); i++, j++)
                {
                    week_days[j] = buf[i];
                }

                #region NOT USED
                byte[] com_protocol = (byte[])phone.attributes["com_protocol"]["value"];
                for (int i = (7 + (int)phone.attributes["com_protocol"]["address"]), j = 0; i < (7 + (int)phone.attributes["com_protocol"]["address"] + com_protocol.Length); i++, j++)
                {
                    com_protocol[j] = buf[i];
                }
                byte[] reserved_1 = (byte[])phone.attributes["reserved_1"]["value"];
                for (int i = (7 + (int)phone.attributes["reserved_1"]["address"]), j = 0; i < (7 + (int)phone.attributes["reserved_1"]["address"] + reserved_1.Length); i++, j++)
                {
                    reserved_1[j] = buf[i];
                }
                byte[] reserved_2 = (byte[])phone.attributes["reserved_2"]["value"];
                for (int i = (7 + (int)phone.attributes["reserved_2"]["address"]), j = 0; i < (7 + (int)phone.attributes["reserved_2"]["address"] + reserved_2.Length); i++, j++)
                {
                    reserved_2[j] = buf[i];
                }
                #endregion

                //TODO: Find a better solution to calculate this
                //UInt64 phone_options = (ulong)((((ulong)options[7] << 24) + (ulong)(options[6] << 16) + (ulong)(options[5] << 8) + (ulong)(options[4]) << 32) + (ulong)(options[3] << 24) + (ulong)(options[2] << 16) + (ulong)(options[1] << 8) + (ulong)options[0]);
                UInt64 phone_options = ((((ulong)(options[7] << 24) + (ulong)(options[6] << 16) + (ulong)(options[5] << 8) + (options[4])) << 32) + ((ulong)(options[3] << 24) + (ulong)(options[2] << 16) + (ulong)(options[1] << 8) + (ulong)options[0]));

                UInt64 KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_VOICE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_VOICE"])[0];
                UInt64 KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_CID = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_CID"])[0];
                UInt64 KP_TELF_REP_ALLWAYS_REPORT = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ALLWAYS_REPORT"])[0];
                UInt64 KP_TELF_REP_MONITOR_CALL_PROGRESS = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_MONITOR_CALL_PROGRESS"])[0];
                UInt64 KP_TELF_REP_BLIND_DIAL = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_BLIND_DIAL"])[0];
                UInt64 KP_TELF_REP_STAY_ON_LINE_ATFER_REPORT_2WAY_VOICE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_STAY_ON_LINE_ATFER_REPORT_2WAY_VOICE"])[0];
                UInt64 KP_TELF_REP_USE_DIALLING_PREFIX = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_USE_DIALLING_PREFIX"])[0];
                UInt64 KP_TELF_REP_CALL_BACK_NUMBER = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_CALL_BACK_NUMBER"])[0];

                UInt64 KP_TELF_REP_MAINS_FUSE_FAILURE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_MAINS_FUSE_FAILURE"])[0];
                UInt64 KP_TELF_REP_BATTERY_LOW = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_BATTERY_LOW"])[0];
                UInt64 KP_TELF_REP_RADIO_BATTERY_LOW = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_RADIO_BATTERY_LOW"])[0];
                UInt64 KP_TELF_REP_LINE_FAIL = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_LINE_FAIL"])[0];
                UInt64 KP_TELF_REP_SYSTEM_TAMPER = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_SYSTEM_TAMPER"])[0];
                UInt64 KP_TELF_REP_KEYPAD_TAMPER = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_KEYPAD_TAMPER"])[0];
                UInt64 KP_TELF_REP_ZONE_TAMPER = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ZONE_TAMPER"])[0];
                UInt64 KP_TELF_REP_RADIO_ZONE_TAMPER = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_RADIO_ZONE_TAMPER"])[0];
                UInt64 KP_TELF_REP_DURESS_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_DURESS_ALARM"])[0];

                UInt64 KP_TELF_REP_SUPERVISED_RADIO_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_SUPERVISED_RADIO_ALARM"])[0];
                UInt64 KP_TELF_REP_SENSOR_WATCH_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_SENSOR_WATCH_ALARM"])[0];
                UInt64 KP_TELF_REP_MANUAL_PANIC_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_MANUAL_PANIC_ALARM"])[0];
                UInt64 KP_TELF_REP_MANUAL_FIRE_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_MANUAL_FIRE_ALARM"])[0];
                UInt64 KP_TELF_REP_MANUAL_MEDICAL_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_MANUAL_MEDICAL_ALARM"])[0];
                UInt64 KP_TELF_REP_MANUAL_PEDANT_PANIC_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_MANUAL_PEDANT_PANIC_ALARM"])[0];
                UInt64 KP_TELF_REP_ZONE_BYPASS = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ZONE_BYPASS"])[0];
                UInt64 KP_TELF_REP_ARM_DISARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ARM_DISARM"])[0];

                UInt64 KP_TELF_REP_STAY_MODE_ARM_DISARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_STAY_MODE_ARM_DISARM"])[0];
                UInt64 KP_TELF_REP_DISARM_ONLY_AFTER_ACTIVATION = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_DISARM_ONLY_AFTER_ACTIVATION"])[0];
                UInt64 KP_TELF_REP_STAY_MODE_DISARM_ONLY_AFTER_ACTIVATION = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_STAY_MODE_DISARM_ONLY_AFTER_ACTIVATION"])[0];
                UInt64 KP_TELF_REP_STAY_MODE_ZONE_ALARM = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_STAY_MODE_ZONE_ALARM"])[0];
                UInt64 KP_TELF_REP_ACCESS_TO_PROGRAM_MODE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ACCESS_TO_PROGRAM_MODE"])[0];
                UInt64 KP_TELF_REP_ACCESS_TO_INSTALLER_MODE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ACCESS_TO_INSTALLER_MODE"])[0];
                UInt64 KP_TELF_REP_24HOUR_ALARMS_WHEN_SET_DOMESCTIC_VOICE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_24HOUR_ALARMS_WHEN_SET_DOMESCTIC_VOICE"])[0];
                UInt64 KP_TELF_REP_ZONES_RESTORES = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_ZONES_RESTORES"])[0];


                UInt64 KP_TELF_REP_LATCH_KEYS = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_LATCH_KEYS"])[0];
                UInt64 KP_TELF_REP_DELINQUENT = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_DELINQUENT"])[0];
                UInt64 KP_TELF_REP_TESTS = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_TESTS"])[0];
                UInt64 KP_TELF_REP_FUSE_FAILURE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_FUSE_FAILURE"])[0];
                UInt64 KP_TELF_REP_OUTPUTS_FAIL = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_OUTPUTS_FAIL"])[0];
                UInt64 KP_TELF_REP_RTC_TIME_CHANGE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_RTC_TIME_CHANGE"])[0];
                UInt64 KP_TELF_REP_KEYPAD_BUS_TROUBLE = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_KEYPAD_BUS_TROUBLE"])[0];
                UInt64 KP_TELF_REP_RF_INTERFERENCE_DETECTED = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_RF_INTERFERENCE_DETECTED"])[0];

                UInt64 KP_TELF_REP_SYSTEM_PROBLEMS = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_SYSTEM_PROBLEMS"])[0];

                UInt64 KP_TELF_REP_TEST_CALL = phone_options & ((ulong[])phone.attributes["options"]["KP_TELF_REP_TEST_CALL"])[0];
                UInt64 KP_TELT_REP_CHAMADA_VOZ = phone_options & ((ulong[])phone.attributes["options"]["KP_TELT_REP_CHAMADA_VOZ"])[0];
                UInt64 KP_TELT_REP_ATIVO = phone_options & ((ulong[])phone.attributes["options"]["KP_TELT_REP_ATIVO"])[0];

                //Join hour test with minute test
                string string_test_hour = hour_test[0].ToString("00") + minute_test[0].ToString("00");
                TimeSpan test_hour = default(TimeSpan);

                try
                {
                    test_hour = new TimeSpan(hour_test[0], minute_test[0], 0);
                }
                catch
                {
                    test_hour = default(TimeSpan);
                }

                try
                {

                    //String
                    databaseDataSet.Phone.Rows[phone_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0');

                    //Phone number
                    ulong phone_number_temp = 0;
                    if (phone_number[0] != 255)
                    {
                        int count = 0;
                        for (count = 0; count < 16; count++)
                        {
                            if (phone_number[count] == 255)
                            {
                                break;
                            }
                        }
                        for (int decremental_count = count - 1; decremental_count >= 0; decremental_count--)
                        {
                            phone_number_temp += (ulong)phone_number[decremental_count] * (ulong)Math.Pow(10, ((count - 1) - decremental_count));
                        }
                        databaseDataSet.Phone.Rows[phone_to_read]["Phone number"] = phone_number_temp;
                    }
                    else
                    {
                        databaseDataSet.Phone.Rows[phone_to_read]["Phone number"] = phone_number_temp.ToString("0000000000000000");
                    }

                    //Phone prefix
                    uint prefix_number_temp = 0;
                    if (prefix_number[0] != 255)
                    {
                        int count = 0;
                        for (count = 0; count < 6; count++)
                        {
                            if (prefix_number[count] == 255)
                            {

                                break;
                            }
                        }
                        for (int decremental_count = count - 1; decremental_count >= 0; decremental_count--)
                        {
                            prefix_number_temp += (uint)prefix_number[decremental_count] * (uint)Math.Pow(10, ((count - 1) - decremental_count));
                        }
                        databaseDataSet.Phone.Rows[phone_to_read]["Prefix"] = prefix_number_temp;//(ulong)(phone_number[0] * 100000000 + phone_number[1] * 10000000 + phone_number[2] * 1000000 + phone_number[3] * 100000 + phone_number[4] * 10000 + phone_number[5] * 1000 + phone_number[6] * 100 + phone_number[7] * 10 + phone_number[8]);
                    }
                    else
                    {
                        databaseDataSet.Phone.Rows[phone_to_read]["Prefix"] = prefix_number_temp.ToString("0000000000000000");
                    }

                    //Max of comminucation attempts
                    databaseDataSet.Phone.Rows[phone_to_read]["Communication attempts"] = max_com_attempts[0];

                    //Areas
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 1"] = (partitions[0] & 0x01) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 2"] = (partitions[0] & 0x02) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 3"] = (partitions[0] & 0x04) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 4"] = (partitions[0] & 0x08) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 5"] = (partitions[0] & 0x10) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 6"] = (partitions[0] & 0x20) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 7"] = (partitions[0] & 0x40) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Area 8"] = (partitions[0] & 0x80) > 0;

                    //Options
                    databaseDataSet.Phone.Rows[phone_to_read]["Stop dialling if kissed off voice"] = (KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_VOICE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Stop dialling if kissed off CID"] = (KP_TELF_REP_STOP_DIALLING_IF_KISSED_OFF_CID > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Always report"] = (KP_TELF_REP_ALLWAYS_REPORT > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Monitor call progress"] = (KP_TELF_REP_MONITOR_CALL_PROGRESS > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Blind dial"] = (KP_TELF_REP_BLIND_DIAL > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Stay on line after report 2 way voice"] = (KP_TELF_REP_STAY_ON_LINE_ATFER_REPORT_2WAY_VOICE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Use prefix"] = (KP_TELF_REP_USE_DIALLING_PREFIX > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Call back number"] = (KP_TELF_REP_CALL_BACK_NUMBER > 0);

                    databaseDataSet.Phone.Rows[phone_to_read]["Report 230V fail"] = (KP_TELF_REP_MAINS_FUSE_FAILURE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report battery low"] = (KP_TELF_REP_BATTERY_LOW > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report radio battery low"] = (KP_TELF_REP_RADIO_BATTERY_LOW > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report line fail"] = (KP_TELF_REP_LINE_FAIL > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report system tamper"] = (KP_TELF_REP_SYSTEM_TAMPER > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report keypad tamper"] = (KP_TELF_REP_KEYPAD_TAMPER > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report zone tamper"] = (KP_TELF_REP_ZONE_TAMPER > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report radio zone tamper"] = (KP_TELF_REP_RADIO_ZONE_TAMPER > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report duress alarm"] = (KP_TELF_REP_DURESS_ALARM > 0);

                    databaseDataSet.Phone.Rows[phone_to_read]["Report supervised radio alarm"] = (KP_TELF_REP_SUPERVISED_RADIO_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report sensor watch alarm"] = (KP_TELF_REP_SENSOR_WATCH_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report manual panic alarm"] = (KP_TELF_REP_MANUAL_PANIC_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report manual fire alarm"] = (KP_TELF_REP_MANUAL_FIRE_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report manual medical alarm"] = (KP_TELF_REP_MANUAL_MEDICAL_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report manual pedant panic alarm"] = (KP_TELF_REP_MANUAL_PEDANT_PANIC_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report zone bypass"] = (KP_TELF_REP_ZONE_BYPASS > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report arm/disarm"] = (KP_TELF_REP_ARM_DISARM > 0);

                    databaseDataSet.Phone.Rows[phone_to_read]["Report stay mode arm/disarm"] = (KP_TELF_REP_STAY_MODE_ARM_DISARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report disarm only after activation"] = (KP_TELF_REP_DISARM_ONLY_AFTER_ACTIVATION > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report stay mode disarm only after activation"] = (KP_TELF_REP_STAY_MODE_DISARM_ONLY_AFTER_ACTIVATION > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report stay mode zone alarm"] = (KP_TELF_REP_STAY_MODE_ZONE_ALARM > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report access to program mode"] = (KP_TELF_REP_ACCESS_TO_PROGRAM_MODE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report access to installer mode"] = (KP_TELF_REP_ACCESS_TO_INSTALLER_MODE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report 24h alarms when set domestic voice"] = (KP_TELF_REP_24HOUR_ALARMS_WHEN_SET_DOMESCTIC_VOICE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report Zones restore"] = (KP_TELF_REP_ZONES_RESTORES > 0);

                    databaseDataSet.Phone.Rows[phone_to_read]["Report latch keys"] = (KP_TELF_REP_LATCH_KEYS > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report delinquent"] = (KP_TELF_REP_DELINQUENT > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report tests"] = (KP_TELF_REP_TESTS > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report fuse failure"] = (KP_TELF_REP_FUSE_FAILURE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report outputs failure"] = (KP_TELF_REP_OUTPUTS_FAIL > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report date/hour change"] = (KP_TELF_REP_RTC_TIME_CHANGE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report keypad communication problems"] = (KP_TELF_REP_KEYPAD_BUS_TROUBLE > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report RF problems detected"] = (KP_TELF_REP_RF_INTERFERENCE_DETECTED > 0);

                    databaseDataSet.Phone.Rows[phone_to_read]["Report system problems"] = (KP_TELF_REP_SYSTEM_PROBLEMS > 0);

                    databaseDataSet.Phone.Rows[phone_to_read]["Test call"] = (KP_TELF_REP_TEST_CALL > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Voice call"] = (KP_TELT_REP_CHAMADA_VOZ > 0);
                    databaseDataSet.Phone.Rows[phone_to_read]["Report on"] = (KP_TELT_REP_ATIVO > 0);

                    //Test hour
                    databaseDataSet.Phone.Rows[phone_to_read]["Test hour"] = test_hour;


                    //Week days
                    databaseDataSet.Phone.Rows[phone_to_read]["Monday"] = (week_days[0] & 0x01) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Tuesday"] = (week_days[0] & 0x02) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Wednesday"] = (week_days[0] & 0x04) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Thursday"] = (week_days[0] & 0x08) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Friday"] = (week_days[0] & 0x10) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Saturday"] = (week_days[0] & 0x20) > 0;
                    databaseDataSet.Phone.Rows[phone_to_read]["Sunday"] = (week_days[0] & 0x40) > 0;

                    #region SAVE ON DATABASE - NOT USED
                    // Communication protocol
                    databaseDataSet.Phone.Rows[phone_to_read]["Communication protocol"] = com_protocol;
                    databaseDataSet.Phone.Rows[phone_to_read]["Reserved 1"] = reserved_1;
                    databaseDataSet.Phone.Rows[phone_to_read]["Reserved 2"] = reserved_2;
                    #endregion

                    databaseDataSet.Phone.AcceptChanges();
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_DIALERS_INIC_ADDR && addr < Constants.KP_DIALERS_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Dialers
                int dialer_index = ((addr - Constants.KP_DIALERS_INIC_ADDR) / (int)Constants.KP_FLASH_TAMANHO_DADOS_DIALER_FLASH);

                Protocol.Dialer dialer = new Protocol.Dialer();

                byte[] options = (byte[])dialer.attributes["options"]["value"];
                for (int i = (7 + (int)dialer.attributes["options"]["address"]), j = 0; i < (7 + (int)dialer.attributes["options"]["address"] + options.Length); i++, j++)
                {
                    options[j] = buf[i];
                }

                byte[] description = (byte[])dialer.attributes["description"]["value"];
                for (int i = (7 + (int)dialer.attributes["description"]["address"]), j = 0; i < (7 + (int)dialer.attributes["description"]["address"] + description.Length); i++, j++)
                {
                    description[j] = buf[i];
                }


                #region unused
                byte[] active = (byte[])dialer.attributes["active"]["value"];
                for (int i = (7 + (int)dialer.attributes["active"]["address"]), j = 0; i < (7 + (int)dialer.attributes["active"]["address"] + active.Length); i++, j++)
                {
                    active[j] = buf[i];
                }

                byte[] week_days_for_test_call = (byte[])dialer.attributes["week_days_for_test_call"]["value"];
                for (int i = (7 + (int)dialer.attributes["week_days_for_test_call"]["address"]), j = 0; i < (7 + (int)dialer.attributes["week_days_for_test_call"]["address"] + week_days_for_test_call.Length); i++, j++)
                {
                    week_days_for_test_call[j] = buf[i];
                }

                byte[] hour_for_test_call = (byte[])dialer.attributes["hour_for_test_call"]["value"];
                for (int i = (7 + (int)dialer.attributes["hour_for_test_call"]["address"]), j = 0; i < (7 + (int)dialer.attributes["hour_for_test_call"]["address"] + hour_for_test_call.Length); i++, j++)
                {
                    hour_for_test_call[j] = buf[i];
                }

                byte[] keypad_listening_options = (byte[])dialer.attributes["keypad_listening_options"]["value"];
                for (int i = (7 + (int)dialer.attributes["keypad_listening_options"]["address"]), j = 0; i < (7 + (int)dialer.attributes["keypad_listening_options"]["address"] + keypad_listening_options.Length); i++, j++)
                {
                    keypad_listening_options[j] = buf[i];
                }

                byte[] output1_listening_options = (byte[])dialer.attributes["output1_listening_options"]["value"];
                for (int i = (7 + (int)dialer.attributes["output1_listening_options"]["address"]), j = 0; i < (7 + (int)dialer.attributes["output1_listening_options"]["address"] + output1_listening_options.Length); i++, j++)
                {
                    output1_listening_options[j] = buf[i];
                }

                byte[] area_dial_delay = (byte[])dialer.attributes["area_dial_delay"]["value"];
                for (int i = (7 + (int)dialer.attributes["area_dial_delay"]["address"]), j = 0; i < (7 + (int)dialer.attributes["area_dial_delay"]["address"] + area_dial_delay.Length); i++, j++)
                {
                    area_dial_delay[j] = buf[i];
                }

                byte[] area_cancel_window = (byte[])dialer.attributes["area_cancel_window"]["value"];
                for (int i = (7 + (int)dialer.attributes["area_cancel_window"]["address"]), j = 0; i < (7 + (int)dialer.attributes["area_cancel_window"]["address"] + area_cancel_window.Length); i++, j++)
                {
                    area_cancel_window[j] = buf[i];
                }
                #endregion

                byte[] ring_counter_max = (byte[])dialer.attributes["ring_counter_max"]["value"];
                for (int i = (7 + (int)dialer.attributes["ring_counter_max"]["address"]), j = 0; i < (7 + (int)dialer.attributes["ring_counter_max"]["address"] + ring_counter_max.Length); i++, j++)
                {
                    ring_counter_max[j] = buf[i];
                }


                ////OPTIONS HANDLING
                UInt32 global_system_options = (uint)((uint)(options[3] << 24) + (uint)(options[2] << 16) + (uint)(options[1] << 8) + (uint)options[0]);
                UInt32 KP_COMUNIC_OPCOES_ENABLE = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_ENABLE"])[0];
                UInt32 KP_COMUNIC_OPCOES_FAX_DEFEAT = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_FAX_DEFEAT"])[0];
                UInt32 KP_COMUNIC_OPCOES_LINE_MONITOR = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LINE_MONITOR"])[0];
                UInt32 KP_COMUNIC_OPCOES_MAKE_TEST_CALL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_MAKE_TEST_CALL"])[0];
                UInt32 KP_COMUNIC_OPCOES_reservado2 = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_reservado2"])[0];
                UInt32 KP_COMUNIC_OPCOES_reservado3 = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_reservado3"])[0];
                UInt32 KP_COMUNIC_OPCOES_reservado4 = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_reservado4"])[0];
                UInt32 KP_COMUNIC_OPCOES_SEND_LONG_DTMF = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_SEND_LONG_DTMF"])[0];

                UInt32 KP_COMUNIC_OPCOES_STEP_NUMB_EACH_CALL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_STEP_NUMB_EACH_CALL"])[0];
                UInt32 KP_COMUNIC_OPCOES_reservado5 = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_reservado5"])[0];
                UInt32 KP_COMUNIC_OPCOES_reservado6 = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_reservado6"])[0];
                UInt32 KP_COMUNIC_OPCOES_TEST_CALL_ONLY_ARMED = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_TEST_CALL_ONLY_ARMED"])[0];
                UInt32 KP_COMUNIC_OPCOES_HOLD_DOMESTIC_FOR_DTMF = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_HOLD_DOMESTIC_FOR_DTMF"])[0];
                UInt32 KP_COMUNIC_OPCOES_FIRST_OPEN_LAST_CLOSE = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_FIRST_OPEN_LAST_CLOSE"])[0];

                UInt32 KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_DIAL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_DIAL"])[0];
                UInt32 KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_DIAL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_DIAL"])[0];
                UInt32 KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_DIAL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_DIAL"])[0];
                UInt32 KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_CALL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_CALL"])[0];
                UInt32 KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_CALL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_CALL"])[0];
                UInt32 KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_CALL = global_system_options & ((uint[])dialer.attributes["options"]["KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_CALL"])[0];

                byte[] active_communicator = (byte[])dialer.attributes["active"]["value"];
                for (int i = (7 + (int)dialer.attributes["active"]["address"]), j = 0; i < (7 + (int)dialer.attributes["active"]["address"] + active_communicator.Length); i++, j++)
                {
                    active_communicator[j] = buf[i];
                }

                try
                {
                    #region Options
                    //Options
                    databaseDataSet.Dialer.Rows[dialer_index]["Active"] = (KP_COMUNIC_OPCOES_ENABLE > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Fax defeat"] = (KP_COMUNIC_OPCOES_FAX_DEFEAT > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Monitor line"] = (KP_COMUNIC_OPCOES_LINE_MONITOR > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Make test call"] = (KP_COMUNIC_OPCOES_MAKE_TEST_CALL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Reserved 2"] = (KP_COMUNIC_OPCOES_reservado2 > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Reserved 3"] = (KP_COMUNIC_OPCOES_reservado3 > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Reserved 4"] = (KP_COMUNIC_OPCOES_reservado4 > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Long DTMF"] = (KP_COMUNIC_OPCOES_SEND_LONG_DTMF > 0);

                    databaseDataSet.Dialer.Rows[dialer_index]["Step number each call"] = (KP_COMUNIC_OPCOES_STEP_NUMB_EACH_CALL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Reserved 5"] = (KP_COMUNIC_OPCOES_reservado5 > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Reserved 6"] = (KP_COMUNIC_OPCOES_reservado6 > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Test call only armed"] = (KP_COMUNIC_OPCOES_TEST_CALL_ONLY_ARMED > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Hold domestic for DTMF"] = (KP_COMUNIC_OPCOES_HOLD_DOMESTIC_FOR_DTMF > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["First open last close"] = (KP_COMUNIC_OPCOES_FIRST_OPEN_LAST_CLOSE > 0);


                    databaseDataSet.Dialer.Rows[dialer_index]["List in output disarm dial"] = (KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_DIAL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["List in output arm dial"] = (KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_DIAL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["List in output stay dial"] = (KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_DIAL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["List in output disarm call"] = (KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_DISARM_CALL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["List in output arm call"] = (KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_ARM_CALL > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["List in output stay call"] = (KP_COMUNIC_OPCOES_LIST_IN_OUTPUT_STAY_CALL > 0);
                    #endregion

                    //Description
                    databaseDataSet.Dialer.Rows[dialer_index]["Description"] = Encoding.ASCII.GetString(description).Trim('\0');

                    //Active Comunnicator
                    databaseDataSet.Dialer.Rows[dialer_index]["Active communicator"] = active_communicator;

                    // week_days_for_test_call days
                    databaseDataSet.Dialer.Rows[dialer_index]["Monday"] = ((week_days_for_test_call[0] & 0x01) > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Tuesday"] = ((week_days_for_test_call[0] & 0x02) > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Wednesday"] = ((week_days_for_test_call[0] & 0x04) > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Thursday"] = ((week_days_for_test_call[0] & 0x08) > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Friday"] = ((week_days_for_test_call[0] & 0x10) > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Saturday"] = ((week_days_for_test_call[0] & 0x20) > 0);
                    databaseDataSet.Dialer.Rows[dialer_index]["Sunday"] = ((week_days_for_test_call[0] & 0x40) > 0);

                    //hour_for_test_call
                    databaseDataSet.Dialer.Rows[dialer_index]["Test call hour"] = hour_for_test_call;

                    //keypad_listening_options
                    databaseDataSet.Dialer.Rows[dialer_index]["Keypad listening options"] = keypad_listening_options;

                    //output1_listening_options
                    databaseDataSet.Dialer.Rows[dialer_index]["Output 1 listening options"] = output1_listening_options;

                    //area_dial_delay
                    databaseDataSet.Dialer.Rows[dialer_index]["Area dial delay"] = area_dial_delay;

                    //area_cancel_window
                    databaseDataSet.Dialer.Rows[dialer_index]["Area cancel window"] = area_cancel_window;

                    //Ring number max
                    databaseDataSet.Dialer.Rows[dialer_index]["RingCounter"] = ring_counter_max[0].ToString();

                    databaseDataSet.Dialer.AcceptChanges();
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_GLOBAL_SYSTEM_INIC_ADDR && addr < Constants.KP_GLOBAL_SYSTEM_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region GlobalSystem
                int global_system_index = ((addr - Constants.KP_GLOBAL_SYSTEM_INIC_ADDR) / (int)Constants.KP_FLASH_TAMANHO_DADOS_GLOBALSYSTEM_FLASH);


                Protocol.GlobalSystem global_system = new Protocol.GlobalSystem();

                byte[] output_call_code = (byte[])global_system.attributes["output_call_code"]["value"];
                for (int i = (7 + (int)global_system.attributes["output_call_code"]["address"]), j = 0; i < (7 + (int)global_system.attributes["output_call_code"]["address"] + output_call_code.Length); i++, j++)
                {
                    output_call_code[j] = buf[i];
                }

                byte[] options = (byte[])global_system.attributes["options"]["value"];
                for (int i = (7 + (int)global_system.attributes["options"]["address"]), j = 0; i < (7 + (int)global_system.attributes["options"]["address"] + options.Length); i++, j++)
                {
                    options[j] = buf[i];
                }

                byte[] installer_code = (byte[])global_system.attributes["installer_code"]["value"];
                for (int i = (7 + (int)global_system.attributes["installer_code"]["address"]), j = 0; i < (7 + (int)global_system.attributes["installer_code"]["address"] + installer_code.Length); i++, j++)
                {
                    installer_code[j] = buf[i];
                }

                byte[] coation_digit = (byte[])global_system.attributes["coation_digit"]["value"];
                for (int i = (7 + (int)global_system.attributes["coation_digit"]["address"]), j = 0; i < (7 + (int)global_system.attributes["coation_digit"]["address"] + coation_digit.Length); i++, j++)
                {
                    coation_digit[j] = buf[i];
                }

                byte[] phone_prefix = (byte[])global_system.attributes["phone_prefix"]["value"];
                for (int i = (7 + (int)global_system.attributes["phone_prefix"]["address"]), j = 0; i < (7 + (int)global_system.attributes["phone_prefix"]["address"] + phone_prefix.Length); i++, j++)
                {
                    phone_prefix[j] = buf[i];
                }

                #region RESERVED
                byte[] reserved_1 = (byte[])global_system.attributes["reserved_1"]["value"];
                for (int i = (7 + (int)global_system.attributes["reserved_1"]["address"]), j = 0; i < (7 + (int)global_system.attributes["reserved_1"]["address"] + reserved_1.Length); i++, j++)
                {
                    reserved_1[j] = buf[i];
                }

                byte[] reserved_2 = (byte[])global_system.attributes["reserved_2"]["value"];
                for (int i = (7 + (int)global_system.attributes["reserved_2"]["address"]), j = 0; i < (7 + (int)global_system.attributes["reserved_2"]["address"] + reserved_2.Length); i++, j++)
                {
                    reserved_2[j] = buf[i];
                }
                #endregion

                byte[] dial_report_delay = (byte[])global_system.attributes["dial_report_delay"]["value"];
                for (int i = (7 + (int)global_system.attributes["dial_report_delay"]["address"]), j = 0; i < (7 + (int)global_system.attributes["dial_report_delay"]["address"] + dial_report_delay.Length); i++, j++)
                {
                    dial_report_delay[j] = buf[i];
                }

                byte[] mains_fail_report_delay = (byte[])global_system.attributes["mains_fail_report_delay"]["value"];
                for (int i = (7 + (int)global_system.attributes["mains_fail_report_delay"]["address"]), j = 0; i < (7 + (int)global_system.attributes["mains_fail_report_delay"]["address"] + mains_fail_report_delay.Length); i++, j++)
                {
                    mains_fail_report_delay[j] = buf[i];
                }

                byte[] receiver_fail_delay = (byte[])global_system.attributes["receiver_fail_delay"]["value"];
                for (int i = (7 + (int)global_system.attributes["receiver_fail_delay"]["address"]), j = 0; i < (7 + (int)global_system.attributes["receiver_fail_delay"]["address"] + receiver_fail_delay.Length); i++, j++)
                {
                    receiver_fail_delay[j] = buf[i];
                }

                byte[] radio_detector_supervised_timer = (byte[])global_system.attributes["radio_detector_supervised_timer"]["value"];
                for (int i = (7 + (int)global_system.attributes["radio_detector_supervised_timer"]["address"]), j = 0; i < (7 + (int)global_system.attributes["radio_detector_supervised_timer"]["address"] + radio_detector_supervised_timer.Length); i++, j++)
                {
                    radio_detector_supervised_timer[j] = buf[i];
                }

                byte[] CRA_client_number = (byte[])global_system.attributes["CRA_client_number"]["value"];
                for (int i = (7 + (int)global_system.attributes["CRA_client_number"]["address"]), j = 0; i < (7 + (int)global_system.attributes["CRA_client_number"]["address"] + CRA_client_number.Length); i++, j++)
                {
                    CRA_client_number[j] = buf[i];
                }

                byte[] event_code_session = (byte[])global_system.attributes["event_code_session"]["value"];
                for (int i = (7 + (int)global_system.attributes["event_code_session"]["address"]), j = 0; i < (7 + (int)global_system.attributes["event_code_session"]["address"] + event_code_session.Length); i++, j++)
                {
                    event_code_session[j] = buf[i];
                }

                byte[] siren_tamper = (byte[])global_system.attributes["siren_tamper"]["value"];
                for (int i = (7 + (int)global_system.attributes["siren_tamper"]["address"]), j = 0; i < (7 + (int)global_system.attributes["siren_tamper"]["address"] + siren_tamper.Length); i++, j++)
                {
                    siren_tamper[j] = buf[i];
                }

                byte[] siren_tamper_config = (byte[])global_system.attributes["siren_tamper_config"]["value"];
                for (int i = (7 + (int)global_system.attributes["siren_tamper_config"]["address"]), j = 0; i < (7 + (int)global_system.attributes["siren_tamper_config"]["address"] + siren_tamper_config.Length); i++, j++)
                {
                    siren_tamper_config[j] = buf[i];
                }

                byte[] panel_tamper = (byte[])global_system.attributes["panel_tamper"]["value"];
                for (int i = (7 + (int)global_system.attributes["panel_tamper"]["address"]), j = 0; i < (7 + (int)global_system.attributes["panel_tamper"]["address"] + panel_tamper.Length); i++, j++)
                {
                    panel_tamper[j] = buf[i];
                }

                byte[] panel_tamper_config = (byte[])global_system.attributes["panel_tamper_config"]["value"];
                for (int i = (7 + (int)global_system.attributes["panel_tamper_config"]["address"]), j = 0; i < (7 + (int)global_system.attributes["panel_tamper_config"]["address"] + panel_tamper_config.Length); i++, j++)
                {
                    panel_tamper_config[j] = buf[i];
                }


                byte[] keyswitch_config = (byte[])global_system.attributes["keyswitch_config"]["value"];
                for (int i = (7 + (int)global_system.attributes["keyswitch_config"]["address"]), j = 0; i < (7 + (int)global_system.attributes["keyswitch_config"]["address"] + keyswitch_config.Length); i++, j++)
                {
                    keyswitch_config[j] = buf[i];
                }

                byte[] keyswitch_area_away = (byte[])global_system.attributes["keyswitch_area_away"]["value"];
                for (int i = (7 + (int)global_system.attributes["keyswitch_area_away"]["address"]), j = 0; i < (7 + (int)global_system.attributes["keyswitch_area_away"]["address"] + keyswitch_area_away.Length); i++, j++)
                {
                    keyswitch_area_away[j] = buf[i];
                }

                byte[] keyswitch_area_stay = (byte[])global_system.attributes["keyswitch_area_stay"]["value"];
                for (int i = (7 + (int)global_system.attributes["keyswitch_area_stay"]["address"]), j = 0; i < (7 + (int)global_system.attributes["keyswitch_area_stay"]["address"] + keyswitch_area_stay.Length); i++, j++)
                {
                    keyswitch_area_stay[j] = buf[i];
                }

                byte[] audio_tracks = (byte[])global_system.attributes["audio_tracks"]["value"];
                for (int i = (7 + (int)global_system.attributes["audio_tracks"]["address"]), j = 0; i < (7 + (int)global_system.attributes["audio_tracks"]["address"] + audio_tracks.Length); i++, j++)
                {
                    audio_tracks[j] = buf[i];
                }

                byte[] maintenance_description = (byte[])global_system.attributes["maintenance_description"]["value"];
                for (int i = (7 + (int)global_system.attributes["maintenance_description"]["address"]), j = 0; i < (7 + (int)global_system.attributes["maintenance_description"]["address"] + maintenance_description.Length); i++, j++)
                {
                    maintenance_description[j] = buf[i];
                }

                byte[] maintenance_phone_number = (byte[])global_system.attributes["maintenance_phone_number"]["value"];
                for (int i = (7 + (int)global_system.attributes["maintenance_phone_number"]["address"]), j = 0; i < (7 + (int)global_system.attributes["maintenance_phone_number"]["address"] + maintenance_phone_number.Length); i++, j++)
                {
                    maintenance_phone_number[j] = buf[i];
                }

                byte[] maintenance_date = (byte[])global_system.attributes["maintenance_date"]["value"];
                for (int i = (7 + (int)global_system.attributes["maintenance_date"]["address"]), j = 0; i < (7 + (int)global_system.attributes["maintenance_date"]["address"] + maintenance_date.Length); i++, j++)
                {
                    maintenance_date[j] = buf[i];
                }

                byte[] outputs_permissions = (byte[])global_system.attributes["outputs_permissions"]["value"];
                for (int i = (7 + (int)global_system.attributes["outputs_permissions"]["address"]), j = 0; i < (7 + (int)global_system.attributes["outputs_permissions"]["address"] + outputs_permissions.Length); i++, j++)
                {
                    outputs_permissions[j] = buf[i];
                }


                ////OPTIONS HANDLING
                UInt32 global_system_options = (uint)((uint)(options[3] << 24) + (uint)(options[2] << 16) + (uint)(options[1] << 8) + (uint)options[0]);
                UInt32 KP_GLOB_ARM_IF_BATT_LOW_OR_AC_FAILS = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_ARM_IF_BATT_LOW_OR_AC_FAILS"])[0];
                UInt32 KP_GLOB_REVISION_DATE_ACTIVE = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_REVISION_DATE_ACTIVE"])[0];
                UInt32 KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT"])[0];
                UInt32 KP_GLOB_INSTALLER_INFO = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_INSTALLER_INFO"])[0];
                UInt32 KP_GLOB_PANEL_TAMPER_EOL = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_PANEL_TAMPER_EOL"])[0];
                UInt32 KP_GLOB_DIRECT_INSTALLER_ACCESS = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_DIRECT_INSTALLER_ACCESS"])[0];
                UInt32 KP_GLOB_MAINS_FAIL_TEST = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_MAINS_FAIL_TEST"])[0];
                UInt32 KP_GLOB_EXTENDED_OUTPUT = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_EXTENDED_OUTPUT"])[0];
                UInt32 KP_GLOB_TEMPO_EM_MINUTOS = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_TEMPO_EM_MINUTOS"])[0];
                UInt32 KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM"])[0];
                UInt32 KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM"])[0];
                UInt32 KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM"])[0];
                UInt32 KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM"])[0];
                UInt32 KP_GLOB_CANNOT_ARM_IF_THERE_IS_KEYPAD_FAULT = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_CANNOT_ARM_IF_THERE_IS_KEYPAD_FAULT"])[0];
                UInt32 KP_GLOB_CANNOT_ARM_IF_THERE_IS_LINE_FAULT = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_CANNOT_ARM_IF_THERE_IS_LINE_FAULT"])[0];
                UInt32 KP_GLOB_CANNOT_ARM_IF_THERE_IS_COMMS_FAULT = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_CANNOT_ARM_IF_THERE_IS_COMMS_FAULT"])[0];
                UInt32 KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS"])[0];
                UInt32 KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER"])[0];
                UInt32 KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY"])[0];
                UInt32 KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE"])[0];
                UInt32 KP_GLOB_OUTPUT_CONTROL_FROM_KEYPAD_IS_DISABLED_WHEN_ARMED = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_OUTPUT_CONTROL_FROM_KEYPAD_IS_DISABLED_WHEN_ARMED"])[0];
                UInt32 KP_GLOB_DEFAULT_CONFIGURATION = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_DEFAULT_CONFIGURATION"])[0];
                UInt32 KP_GLOB_DATE_LOSS = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_DATE_LOSS"])[0];
                UInt32 KP_GLOB_REVISION_DATE_REACHED = global_system_options & ((uint[])global_system.attributes["options"]["KP_GLOB_REVISION_DATE_REACHED"])[0];


                try
                {
                    //Outputs call code
                    ulong output_call_code_temp = 0;
                    if (output_call_code[0] != 255)
                    {
                        int count = 0;
                        for (count = 0; count < 8; count++)
                        {
                            if (output_call_code[count] > 9)
                            {
                                break;
                            }
                        }
                        for (int decremental_count = count - 1; decremental_count >= 0; decremental_count--)
                        {
                            output_call_code_temp += (ulong)output_call_code[decremental_count] * (ulong)Math.Pow(10, ((count - 1) - decremental_count));
                        }
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Outputs code"] = output_call_code_temp;
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Outputs code"] = output_call_code_temp.ToString("00000000");
                    }


                    #region Options
                    //Options
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Arm if Battery low or 230V fail"] = (KP_GLOB_ARM_IF_BATT_LOW_OR_AC_FAILS > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Revision date is active"] = (KP_GLOB_REVISION_DATE_ACTIVE > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT"] = (KP_GLOB_RECEIVER_FAIL_RF_JAMMED_LOCKOUT > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_INSTALLER_INFO"] = (KP_GLOB_INSTALLER_INFO > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_REVISION_DATE_REACHED"] = (KP_GLOB_REVISION_DATE_REACHED > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_PANEL_TAMPER_EOL"] = (KP_GLOB_PANEL_TAMPER_EOL > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_DIRECT_INSTALLER_ACCESS"] = (KP_GLOB_DIRECT_INSTALLER_ACCESS > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_MAINS_FAIL_TEST"] = (KP_GLOB_MAINS_FAIL_TEST > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_EXTENDED_OUTPUT"] = (KP_GLOB_EXTENDED_OUTPUT > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Time in minutes"] = (KP_GLOB_TEMPO_EM_MINUTOS > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM"] = (KP_GLOB_INSTALLER_MODE_RESET_CONFIRM_ALARM > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM"] = (KP_GLOB_INSTALLER_MODE_RESET_TAMPER_ALARM > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM"] = (KP_GLOB_INSTALLER_MODE_RESET_LOW_BAT_ALARM > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM"] = (KP_GLOB_INSTALLER_MODE_RESET_SUPERVISORY_ALARM > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Not arm if keypad is faulty"] = (KP_GLOB_CANNOT_ARM_IF_THERE_IS_KEYPAD_FAULT > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Not arm if line is faulty"] = (KP_GLOB_CANNOT_ARM_IF_THERE_IS_LINE_FAULT > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Not arm if communications have problems"] = (KP_GLOB_CANNOT_ARM_IF_THERE_IS_COMMS_FAULT > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS"] = (KP_GLOB_LOCK_KEYPAD_FOR90SEC_ATFER_10_CODE_ERRORS > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER"] = (KP_GLOB_HIDE_USER_CODES_FROM_INSTALLER > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY"] = (KP_GLOB_CODE_REQUIRED_TO_VIEW_MEMORY > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE"] = (KP_GLOB_CANCEL_HANDOVER_ZONE_FUNCTION_IN_STAY_MODE > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output control is disabled when armed"] = (KP_GLOB_OUTPUT_CONTROL_FROM_KEYPAD_IS_DISABLED_WHEN_ARMED > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_DEFAULT_CONFIGURATION"] = (KP_GLOB_DATE_LOSS > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_DATE_LOSS"] = (KP_GLOB_DATE_LOSS > 0);
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["KP_GLOB_REVISION_DATE_REACHED"] = (KP_GLOB_REVISION_DATE_REACHED > 0);

                    #endregion

                    //Installer code
                    uint instaler_code_temp = 0;
                    if (installer_code[0] != 255)
                    {
                        int count = 0;
                        for (count = 0; count < 8; count++)
                        {
                            if (installer_code[count] == 255)
                            {

                                break;
                            }
                        }
                        for (int decremental_count = count - 1; decremental_count >= 0; decremental_count--)
                        {
                            instaler_code_temp += (uint)installer_code[decremental_count] * (uint)Math.Pow(10, ((count - 1) - decremental_count));
                        }
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Installer code"] = instaler_code_temp;
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Installer code"] = instaler_code_temp.ToString("0000000000000000");
                    }

                    //Coation digit
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Coation digit"] = coation_digit[0];

                    //Phone prefix
                    uint phone_prefix_temp = 0;
                    if (phone_prefix[0] != 255)
                    {
                        int count = 0;
                        for (count = 0; count < 6; count++)
                        {
                            if (phone_prefix[count] == 255)
                            {

                                break;
                            }
                        }
                        for (int decremental_count = count - 1; decremental_count >= 0; decremental_count--)
                        {
                            phone_prefix_temp += (uint)phone_prefix[decremental_count] * (uint)Math.Pow(10, ((count - 1) - decremental_count));
                        }
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Phone prefix"] = phone_prefix_temp;
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Phone prefix"] = phone_prefix_temp.ToString("0000000000000000");
                    }

                    //Reserved 1
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Reserved 1"] = reserved_1;

                    //Dialer report delay
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Dial report delay"] = (dial_report_delay[1] << 8) + dial_report_delay[0];

                    //Main fail report delay
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["230V fail report delay"] = (mains_fail_report_delay[1] << 8) + mains_fail_report_delay[0];

                    //Receiver fail delay
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Receiver fail report delay"] = (receiver_fail_delay[1] << 8) + receiver_fail_delay[0];

                    //Radio detector supervised timer
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Radio detector supervised timer"] = (radio_detector_supervised_timer[1] << 8) + radio_detector_supervised_timer[0];

                    //CRA client number
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["CRA client number"] = (CRA_client_number[1] << 8) + CRA_client_number[0];

                    //Event code session
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Event code session"] = (event_code_session[1] << 8) + event_code_session[0];

                    //Siren tamper output - 1
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Siren tamper output - 1"] = siren_tamper[0];

                    //Siren tamper output - 2
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Siren tamper output - 2"] = siren_tamper[1];

                    //Siren tamper output - 3
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Siren tamper output - 3"] = siren_tamper[2];

                    //Siren tamper active
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Siren tamper active"] = siren_tamper_config[0] & ((uint[])global_system.attributes["siren_tamper_config"]["KP_TAMPER_CONFIG_ATIVO"])[0];

                    //Siren tamper active
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Siren tamper type"] = siren_tamper_config[0] & ((uint[])global_system.attributes["siren_tamper_config"]["KP_TAMPER_CONFIG_NORMAL_CLOSE"])[0]; ;

                    //Panel tamper output - 1
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Panel tamper output - 1"] = panel_tamper[0];

                    //Panel tamper output - 2
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Panel tamper output - 2"] = panel_tamper[1];

                    //Panel tamper output - 3
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Panel tamper output - 3"] = panel_tamper[2];

                    //Panel tamper active
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Panel tamper active"] = panel_tamper_config[0] & ((uint[])global_system.attributes["panel_tamper_config"]["KP_TAMPER_CONFIG_ATIVO"])[0];

                    //Panel tamper type
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Panel tamper type"] = panel_tamper_config[0] & ((uint[])global_system.attributes["panel_tamper_config"]["KP_TAMPER_CONFIG_NORMAL_CLOSE"])[0];

                    //Keyswitch active
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch active"] = keyswitch_config[0] & ((uint[])global_system.attributes["keyswitch_config"]["KP_KEYSWITCH_CONFIG_ATIVO"])[0];
                    //Keyswitch type
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch type"] = keyswitch_config[0] & ((uint[])global_system.attributes["keyswitch_config"]["KP_KEYSWITCH_CONFIG_NORMAL_CLOSE"])[0];
                    //Keyswitch toggle
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch toggle"] = keyswitch_config[0] & ((uint[])global_system.attributes["keyswitch_config"]["KP_KEYSWITCH_CONFIG_TOGGLE"])[0];

                    //Keyswitch Area away
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 1 away"] = (keyswitch_area_away[0] & 0x01) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 2 away"] = (keyswitch_area_away[0] & 0x02) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 3 away"] = (keyswitch_area_away[0] & 0x04) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 4 away"] = (keyswitch_area_away[0] & 0x08) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 5 away"] = (keyswitch_area_away[0] & 0x10) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 6 away"] = (keyswitch_area_away[0] & 0x20) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 7 away"] = (keyswitch_area_away[0] & 0x40) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 8 away"] = (keyswitch_area_away[0] & 0x80) > 0;

                    //Keyswitch Area stay
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 1 stay"] = (keyswitch_area_stay[0] & 0x01) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 2 stay"] = (keyswitch_area_stay[0] & 0x02) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 3 stay"] = (keyswitch_area_stay[0] & 0x04) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 4 stay"] = (keyswitch_area_stay[0] & 0x08) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 5 stay"] = (keyswitch_area_stay[0] & 0x10) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 6 stay"] = (keyswitch_area_stay[0] & 0x20) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 7 stay"] = (keyswitch_area_stay[0] & 0x40) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Keyswitch - area 8 stay"] = (keyswitch_area_stay[0] & 0x80) > 0;

                    //Reserved 2
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Reserved 2"] = reserved_2;

                    //Audio tracks
                    //Audio tracks
                    if (((audio_tracks[1] << 8) + audio_tracks[0]).Equals(0xffff))
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0];
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 1"] = (audio_tracks[1] << 8) + audio_tracks[0] + 1;
                    }

                    if (((audio_tracks[3] << 8) + audio_tracks[2]).Equals(0xffff))
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2];
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 2"] = (audio_tracks[3] << 8) + audio_tracks[2] + 1;
                    }
                    if (((audio_tracks[5] << 8) + audio_tracks[4]).Equals(0xffff))
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4];
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 3"] = (audio_tracks[5] << 8) + audio_tracks[4] + 1;
                    }
                    if (((audio_tracks[7] << 8) + audio_tracks[6]).Equals(0xffff))
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6];
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Audio track 4"] = (audio_tracks[7] << 8) + audio_tracks[6] + 1;
                    }

                    //Maintenance description
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Maintenance description"] = Encoding.ASCII.GetString(maintenance_description).Trim('\0');

                    //Maintenance phone number
                    ulong maintenance_phone_number_temp = 0;
                    if (maintenance_phone_number[0] != 255)
                    {
                        int count = 0;
                        for (count = 0; count < 16; count++)
                        {
                            if (maintenance_phone_number[count] == 255)
                            {
                                break;
                            }
                        }
                        for (int decremental_count = count - 1; decremental_count >= 0; decremental_count--)
                        {
                            maintenance_phone_number_temp += (ulong)maintenance_phone_number[decremental_count] * (ulong)Math.Pow(10, ((count - 1) - decremental_count));
                        }
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Maintenance phone number"] = maintenance_phone_number_temp;
                    }
                    else
                    {
                        databaseDataSet.GlobalSystem.Rows[global_system_index]["Maintenance phone number"] = maintenance_phone_number_temp.ToString("00000000");
                    }

                    //Maintenance date
                    string string_maintenance_date = maintenance_date[0].ToString("00") + maintenance_date[1].ToString("00") + (maintenance_date[2] + (maintenance_date[3] << 8)).ToString("0000");
                    DateTime date_maintenance_date = default(DateTime);

                    try
                    {
                        DateTime dt = new DateTime();
                        bool fault = false;

                        char[] c = string_maintenance_date.ToArray();
                        string aux = string.Empty;
                        for (int i = 0; i < c.Length; i++)
                        {
                            if (i == 2)
                            {
                                aux += "/" + c[i];
                            }
                            else if (i == 4)
                            {
                                aux += "/" + c[i];
                            }
                            else
                            {
                                aux += c[i];
                            }
                        }

                        fault = DateTime.TryParse(aux, out dt);

                        if (!fault)
                        {
                            dt = DateTime.MinValue;
                            string_maintenance_date = dt.ToString("ddMMyyyy");
                            date_maintenance_date = DateTime.ParseExact(string_maintenance_date, "ddMMyyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
                        }
                        else
                        {
                            string formatted = string.Empty;
                            int ano = 0;
                            int mes = 0;
                            int dia = 0;

                            formatted = dt.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);

                            string[] data = formatted.Split('-');

                            ano = Convert.ToInt32(data[2]);
                            mes = Convert.ToInt32(data[1]);
                            dia = Convert.ToInt32(data[0]);

                            date_maintenance_date = new DateTime();
                            date_maintenance_date = new DateTime(ano, mes, dia);
                            databaseDataSet.GlobalSystem.Rows[global_system_index]["Maintenance date"] = date_maintenance_date;
                        }
                    }
                    catch
                    {
                        date_maintenance_date = default(DateTime);
                    }
                    //                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Maintenance date"] = date_maintenance_date;

                    //Output permissions
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output1Permissions"] = (outputs_permissions[0] & 0x01) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output2Permissions"] = (outputs_permissions[0] & 0x02) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output3Permissions"] = (outputs_permissions[0] & 0x04) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output4Permissions"] = (outputs_permissions[0] & 0x08) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output5Permissions"] = (outputs_permissions[0] & 0x10) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output6Permissions"] = (outputs_permissions[0] & 0x20) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output7Permissions"] = (outputs_permissions[0] & 0x40) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output8Permissions"] = (outputs_permissions[0] & 0x80) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output9Permissions"] = (outputs_permissions[1] & 0x01) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output10Permissions"] = (outputs_permissions[1] & 0x02) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output11Permissions"] = (outputs_permissions[1] & 0x04) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output12Permissions"] = (outputs_permissions[1] & 0x08) > 0;
                    databaseDataSet.GlobalSystem.Rows[global_system_index]["Output13Permissions"] = (outputs_permissions[1] & 0x10) > 0;

                    databaseDataSet.GlobalSystem.AcceptChanges();
                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_EVENTS_INIC_ADDR && addr < Constants.KP_EVENTS_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region EVENTS
                int event_to_read = ((addr - 0x82800) / 256);
                Event system_event = new Event();

                byte[] cra_account_number = (byte[])system_event.attributes["cra_account_number"]["value"];
                for (int i = (7 + (int)system_event.attributes["cra_account_number"]["address"]), j = 0; i < (7 + (int)system_event.attributes["cra_account_number"]["address"] + cra_account_number.Length); i++, j++)
                {
                    cra_account_number[j] = buf[i];
                }

                byte[] event_type = (byte[])system_event.attributes["event_type"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_type"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_type"]["address"] + event_type.Length); i++, j++)
                {
                    event_type[j] = buf[i];
                }

                byte[] id = (byte[])system_event.attributes["id"]["value"];
                for (int i = (7 + (int)system_event.attributes["id"]["address"]), j = 0; i < (7 + (int)system_event.attributes["id"]["address"] + id.Length); i++, j++)
                {
                    id[j] = buf[i];
                }

                byte[] event_area = (byte[])system_event.attributes["event_area"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_area"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_area"]["address"] + event_area.Length); i++, j++)
                {
                    event_area[j] = buf[i];
                }

                byte[] event_user = (byte[])system_event.attributes["event_user"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_user"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_user"]["address"] + event_user.Length); i++, j++)
                {
                    event_user[j] = buf[i];
                }

                byte[] event_zone = (byte[])system_event.attributes["event_zone"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_zone"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_zone"]["address"] + event_zone.Length); i++, j++)
                {
                    event_zone[j] = buf[i];
                }

                byte[] event_session = (byte[])system_event.attributes["event_session"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_session"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_session"]["address"] + event_session.Length); i++, j++)
                {
                    event_session[j] = buf[i];
                }

                byte[] event_start_or_end = (byte[])system_event.attributes["event_start_or_end"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_start_or_end"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_start_or_end"]["address"] + event_start_or_end.Length); i++, j++)
                {
                    event_start_or_end[j] = buf[i];
                }

                byte[] event_hour = (byte[])system_event.attributes["event_hour"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_hour"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_hour"]["address"] + event_hour.Length); i++, j++)
                {
                    event_hour[j] = buf[i];
                }

                byte[] event_minute = (byte[])system_event.attributes["event_minute"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_minute"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_minute"]["address"] + event_minute.Length); i++, j++)
                {
                    event_minute[j] = buf[i];
                }


                byte[] event_seconds = (byte[])system_event.attributes["event_seconds"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_seconds"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_seconds"]["address"] + event_seconds.Length); i++, j++)
                {
                    event_seconds[j] = buf[i];
                }

                byte[] event_month = (byte[])system_event.attributes["event_month"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_month"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_month"]["address"] + event_month.Length); i++, j++)
                {
                    event_month[j] = buf[i];
                }

                byte[] event_day = (byte[])system_event.attributes["event_day"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_day"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_day"]["address"] + event_day.Length); i++, j++)
                {
                    event_day[j] = buf[i];
                }

                byte[] event_year = (byte[])system_event.attributes["event_year"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_year"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_year"]["address"] + event_year.Length); i++, j++)
                {
                    event_year[j] = buf[i];
                }

                byte[] event_phone_numbers = (byte[])system_event.attributes["event_phone_numbers"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_phone_numbers"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_phone_numbers"]["address"] + event_phone_numbers.Length); i++, j++)
                {
                    event_phone_numbers[j] = buf[i];
                }

                byte[] event_report_type = (byte[])system_event.attributes["event_report_type"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_report_type"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_report_type"]["address"] + event_report_type.Length); i++, j++)
                {
                    event_report_type[j] = buf[i];
                }

                //add new event
                byte[] event_keypad_ack = (byte[])system_event.attributes["event_keypad_ack"]["value"];
                for (int i = (7 + (int)system_event.attributes["event_keypad_ack"]["address"]), j = 0; i < (7 + (int)system_event.attributes["event_keypad_ack"]["address"] + event_keypad_ack.Length); i++, j++)
                {
                    event_keypad_ack[j] = buf[i];
                }


                uint event_id = ((uint)id[3] << 24) + ((uint)id[2] << 16) + ((uint)id[1] << 8) + (uint)id[0];
                int session = (event_session[1] << 8) + event_session[0];

                //Check if event belongs to this session or if isn't relevant
                if ((event_id == (0xffffffff | 0)) || session != event_code)
                {
                    if (addr == (Constants.KP_EVENTS_FINAL_ADDR - Constants.KP_FLASH_TAMANHO_DADOS_EVENTOS_FLASH))
                        AddEventItemToDatagrid();
                    return;
                }

                int event_cra_account_number = (cra_account_number[1] << 8) + cra_account_number[0];

                int type = (event_type[1] << 8) + event_type[0];

                int? area = 0;
                if (event_area[1] < 8)
                    area = (event_area[1] << 8) + event_area[0];
                else area = null;

                int user = (event_user[1] << 8) + event_user[0];

                int? zone;
                if (event_zone[1] < 128)
                    zone = (event_zone[1] << 8) + event_zone[0];
                else zone = null;


                int start_or_end = event_start_or_end[0];

                //Datetime
                int hour = event_hour[0];
                int minute = event_minute[0];
                int seconds = event_seconds[0];
                int month = event_month[0];
                int day = event_day[0];
                int year = event_year[0] + 1792;

                try
                {
                    DataRow new_event = databaseDataSet.Event.NewRow();

                    //Id
                    new_event["EventId"] = event_id;
                    //cra_account_number
                    new_event["CraAccountNumber"] = event_cra_account_number;
                    //type message
                    new_event["EventTypeDescription"] = (Constants.EventDictionary[type])[start_or_end + 1];
                    //type
                    new_event["Type"] = type;
                    //area
                    new_event["Area"] = area + 1;
                    //User
                    new_event["User"] = user + 1;
                    //Zone
                    new_event["Zone"] = zone + 1;
                    //Session
                    new_event["Session"] = session;
                    //Start or End
                    new_event["StartOrEnd"] = start_or_end;
                    //PhoneNumbers
                    new_event["PhoneNumbers"] = event_phone_numbers;
                    //ReportType
                    new_event["ReportType"] = event_report_type;

                    //Keypad_ack
                    //add new event
                    new_event["Keypad_ack"] = event_keypad_ack[0];

                    //DateTime
                    DateTime event_date_time = new DateTime();
                    event_date_time = event_date_time.AddHours(hour - event_date_time.Hour);
                    event_date_time = event_date_time.AddMinutes(minute - event_date_time.Minute);
                    event_date_time = event_date_time.AddSeconds(seconds - event_date_time.Second);
                    event_date_time = event_date_time.AddDays(day - event_date_time.Day);
                    event_date_time = event_date_time.AddMonths(month - event_date_time.Month);
                    event_date_time = event_date_time.AddYears(year - event_date_time.Year);
                    new_event["DateTime"] = event_date_time.ToString();

                    databaseDataSet.Event.Rows.Add(new_event);

                    if (addr == (Constants.KP_EVENTS_FINAL_ADDR - Constants.KP_FLASH_TAMANHO_DADOS_EVENTOS_FLASH))
                        AddEventItemToDatagrid();

                    databaseDataSet.Event.AcceptChanges();

                    EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
                    databaseDataSetEventTableAdapter.Update(databaseDataSet.Event);

                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
                #endregion
            }
            else if (addr >= Constants.KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_INICIO && addr < Constants.KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_FIM)
            {
                #region AUDIO_SYSTEM_CONFIGURATION

                AudioSystemConfiguration audio_system_configuration = new AudioSystemConfiguration();

                int audio_system_configuration_to_read = ((int)(addr - Constants.KP_FLASH_AUDIO_SYSTEM_CONFIGUATION_INICIO) / (int)8);

                if ((int)(audio_system_configuration.attributes["faixas_audio_230_ok"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_230_ok = (byte[])audio_system_configuration.attributes["faixas_audio_230_ok"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_230_ok"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_230_ok"]["address"] + faixas_audio_230_ok.Length); i++, j++)
                    { faixas_audio_230_ok[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_230_ok[1] << 8) + faixas_audio_230_ok[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_230_ok[1] << 8) + faixas_audio_230_ok[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_230_ok[1] << 8) + faixas_audio_230_ok[0];
                        }

                        if (((faixas_audio_230_ok[3] << 8) + faixas_audio_230_ok[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_230_ok[3] << 8) + faixas_audio_230_ok[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_230_ok[3] << 8) + faixas_audio_230_ok[2];
                        }

                        if (((faixas_audio_230_ok[5] << 8) + faixas_audio_230_ok[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_230_ok[5] << 8) + faixas_audio_230_ok[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_230_ok[5] << 8) + faixas_audio_230_ok[4];
                        }

                        if (((faixas_audio_230_ok[7] << 8) + faixas_audio_230_ok[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_230_ok[7] << 8) + faixas_audio_230_ok[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_230_ok[7] << 8) + faixas_audio_230_ok[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_230_falha"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_230_falha = (byte[])audio_system_configuration.attributes["faixas_audio_230_falha"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_230_falha"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_230_falha"]["address"] + faixas_audio_230_falha.Length); i++, j++)
                    { faixas_audio_230_falha[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_230_falha[1] << 8) + faixas_audio_230_falha[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_230_falha[1] << 8) + faixas_audio_230_falha[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_230_falha[1] << 8) + faixas_audio_230_falha[0];
                        }

                        if (((faixas_audio_230_falha[3] << 8) + faixas_audio_230_falha[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_230_falha[3] << 8) + faixas_audio_230_falha[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_230_falha[3] << 8) + faixas_audio_230_falha[2];
                        }

                        if (((faixas_audio_230_falha[5] << 8) + faixas_audio_230_falha[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_230_falha[5] << 8) + faixas_audio_230_falha[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_230_falha[5] << 8) + faixas_audio_230_falha[4];
                        }

                        if (((faixas_audio_230_falha[7] << 8) + faixas_audio_230_falha[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_230_falha[7] << 8) + faixas_audio_230_falha[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_230_falha[7] << 8) + faixas_audio_230_falha[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_bateria_ok"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_bateria_ok = (byte[])audio_system_configuration.attributes["faixas_audio_bateria_ok"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_bateria_ok"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_bateria_ok"]["address"] + faixas_audio_bateria_ok.Length); i++, j++)
                    { faixas_audio_bateria_ok[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_bateria_ok[1] << 8) + faixas_audio_bateria_ok[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_bateria_ok[1] << 8) + faixas_audio_bateria_ok[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_bateria_ok[1] << 8) + faixas_audio_bateria_ok[0];
                        }

                        if (((faixas_audio_bateria_ok[3] << 8) + faixas_audio_bateria_ok[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_bateria_ok[3] << 8) + faixas_audio_bateria_ok[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_bateria_ok[3] << 8) + faixas_audio_bateria_ok[2];
                        }

                        if (((faixas_audio_bateria_ok[5] << 8) + faixas_audio_bateria_ok[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_bateria_ok[5] << 8) + faixas_audio_bateria_ok[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_bateria_ok[5] << 8) + faixas_audio_bateria_ok[4];
                        }

                        if (((faixas_audio_bateria_ok[7] << 8) + faixas_audio_bateria_ok[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_bateria_ok[7] << 8) + faixas_audio_bateria_ok[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_bateria_ok[7] << 8) + faixas_audio_bateria_ok[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_bateria_falha"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_bateria_falha = (byte[])audio_system_configuration.attributes["faixas_audio_bateria_falha"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_bateria_falha"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_bateria_falha"]["address"] + faixas_audio_bateria_falha.Length); i++, j++)
                    { faixas_audio_bateria_falha[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_bateria_falha[1] << 8) + faixas_audio_bateria_falha[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_bateria_falha[1] << 8) + faixas_audio_bateria_falha[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_bateria_falha[1] << 8) + faixas_audio_bateria_falha[0];
                        }

                        if (((faixas_audio_bateria_falha[3] << 8) + faixas_audio_bateria_falha[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_bateria_falha[3] << 8) + faixas_audio_bateria_falha[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_bateria_falha[3] << 8) + faixas_audio_bateria_falha[2];
                        }

                        if (((faixas_audio_bateria_falha[5] << 8) + faixas_audio_bateria_falha[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_bateria_falha[5] << 8) + faixas_audio_bateria_falha[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_bateria_falha[5] << 8) + faixas_audio_bateria_falha[4];
                        }

                        if (((faixas_audio_bateria_falha[7] << 8) + faixas_audio_bateria_falha[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_bateria_falha[7] << 8) + faixas_audio_bateria_falha[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_bateria_falha[7] << 8) + faixas_audio_bateria_falha[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_coacao"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_coacao = (byte[])audio_system_configuration.attributes["faixas_audio_coacao"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_coacao"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_coacao"]["address"] + faixas_audio_coacao.Length); i++, j++)
                    { faixas_audio_coacao[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_coacao[1] << 8) + faixas_audio_coacao[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_coacao[1] << 8) + faixas_audio_coacao[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_coacao[1] << 8) + faixas_audio_coacao[0];
                        }

                        if (((faixas_audio_coacao[3] << 8) + faixas_audio_coacao[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_coacao[3] << 8) + faixas_audio_coacao[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_coacao[3] << 8) + faixas_audio_coacao[2];
                        }

                        if (((faixas_audio_coacao[5] << 8) + faixas_audio_coacao[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_coacao[5] << 8) + faixas_audio_coacao[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_coacao[5] << 8) + faixas_audio_coacao[4];
                        }

                        if (((faixas_audio_coacao[7] << 8) + faixas_audio_coacao[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_coacao[7] << 8) + faixas_audio_coacao[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_coacao[7] << 8) + faixas_audio_coacao[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_config_start"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_config_start = (byte[])audio_system_configuration.attributes["faixas_audio_config_start"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_config_start"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_config_start"]["address"] + faixas_audio_config_start.Length); i++, j++)
                    { faixas_audio_config_start[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_config_start[1] << 8) + faixas_audio_config_start[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_config_start[1] << 8) + faixas_audio_config_start[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_config_start[1] << 8) + faixas_audio_config_start[0];
                        }

                        if (((faixas_audio_config_start[3] << 8) + faixas_audio_config_start[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_config_start[3] << 8) + faixas_audio_config_start[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_config_start[3] << 8) + faixas_audio_config_start[2];
                        }

                        if (((faixas_audio_config_start[5] << 8) + faixas_audio_config_start[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_config_start[5] << 8) + faixas_audio_config_start[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_config_start[5] << 8) + faixas_audio_config_start[4];
                        }

                        if (((faixas_audio_config_start[7] << 8) + faixas_audio_config_start[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_config_start[7] << 8) + faixas_audio_config_start[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_config_start[7] << 8) + faixas_audio_config_start[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_config_end"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_config_end = (byte[])audio_system_configuration.attributes["faixas_audio_config_end"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_config_end"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_config_end"]["address"] + faixas_audio_config_end.Length); i++, j++)
                    { faixas_audio_config_end[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_config_end[1] << 8) + faixas_audio_config_end[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_config_end[1] << 8) + faixas_audio_config_end[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_config_end[1] << 8) + faixas_audio_config_end[0];
                        }

                        if (((faixas_audio_config_end[3] << 8) + faixas_audio_config_end[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_config_end[3] << 8) + faixas_audio_config_end[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_config_end[3] << 8) + faixas_audio_config_end[2];
                        }

                        if (((faixas_audio_config_end[5] << 8) + faixas_audio_config_end[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_config_end[5] << 8) + faixas_audio_config_end[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_config_end[5] << 8) + faixas_audio_config_end[4];
                        }

                        if (((faixas_audio_config_end[7] << 8) + faixas_audio_config_end[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_config_end[7] << 8) + faixas_audio_config_end[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_config_end[7] << 8) + faixas_audio_config_end[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_tamper"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_tamper = (byte[])audio_system_configuration.attributes["faixas_audio_tamper"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper"]["address"] + faixas_audio_tamper.Length); i++, j++)
                    { faixas_audio_tamper[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_tamper[1] << 8) + faixas_audio_tamper[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper[1] << 8) + faixas_audio_tamper[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper[1] << 8) + faixas_audio_tamper[0];
                        }

                        if (((faixas_audio_tamper[3] << 8) + faixas_audio_tamper[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper[3] << 8) + faixas_audio_tamper[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper[3] << 8) + faixas_audio_tamper[2];
                        }

                        if (((faixas_audio_tamper[5] << 8) + faixas_audio_tamper[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper[5] << 8) + faixas_audio_tamper[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper[5] << 8) + faixas_audio_tamper[4];
                        }

                        if (((faixas_audio_tamper[7] << 8) + faixas_audio_tamper[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper[7] << 8) + faixas_audio_tamper[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper[7] << 8) + faixas_audio_tamper[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_bypass"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_bypass = (byte[])audio_system_configuration.attributes["faixas_audio_bypass"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_bypass"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_bypass"]["address"] + faixas_audio_bypass.Length); i++, j++)
                    { faixas_audio_bypass[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_bypass[1] << 8) + faixas_audio_bypass[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_bypass[1] << 8) + faixas_audio_bypass[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_bypass[1] << 8) + faixas_audio_bypass[0];
                        }

                        if (((faixas_audio_bypass[3] << 8) + faixas_audio_bypass[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_bypass[3] << 8) + faixas_audio_bypass[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_bypass[3] << 8) + faixas_audio_bypass[2];
                        }

                        if (((faixas_audio_bypass[5] << 8) + faixas_audio_bypass[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_bypass[5] << 8) + faixas_audio_bypass[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_bypass[5] << 8) + faixas_audio_bypass[4];
                        }

                        if (((faixas_audio_bypass[7] << 8) + faixas_audio_bypass[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_bypass[7] << 8) + faixas_audio_bypass[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_bypass[7] << 8) + faixas_audio_bypass[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme = (byte[])audio_system_configuration.attributes["faixas_audio_alarme"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme"]["address"] + faixas_audio_alarme.Length); i++, j++)
                    { faixas_audio_alarme[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme[1] << 8) + faixas_audio_alarme[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme[1] << 8) + faixas_audio_alarme[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme[1] << 8) + faixas_audio_alarme[0];
                        }

                        if (((faixas_audio_alarme[3] << 8) + faixas_audio_alarme[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme[3] << 8) + faixas_audio_alarme[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme[3] << 8) + faixas_audio_alarme[2];
                        }

                        if (((faixas_audio_alarme[5] << 8) + faixas_audio_alarme[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme[5] << 8) + faixas_audio_alarme[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme[5] << 8) + faixas_audio_alarme[4];
                        }

                        if (((faixas_audio_alarme[7] << 8) + faixas_audio_alarme[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme[7] << 8) + faixas_audio_alarme[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme[7] << 8) + faixas_audio_alarme[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_panico"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_panico = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_panico"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_panico"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_panico"]["address"] + faixas_audio_alarme_panico.Length); i++, j++)
                    { faixas_audio_alarme_panico[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_panico[1] << 8) + faixas_audio_alarme_panico[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_panico[1] << 8) + faixas_audio_alarme_panico[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_panico[1] << 8) + faixas_audio_alarme_panico[0];
                        }

                        if (((faixas_audio_alarme_panico[3] << 8) + faixas_audio_alarme_panico[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_panico[3] << 8) + faixas_audio_alarme_panico[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_panico[3] << 8) + faixas_audio_alarme_panico[2];
                        }

                        if (((faixas_audio_alarme_panico[5] << 8) + faixas_audio_alarme_panico[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_panico[5] << 8) + faixas_audio_alarme_panico[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_panico[5] << 8) + faixas_audio_alarme_panico[4];
                        }

                        if (((faixas_audio_alarme_panico[7] << 8) + faixas_audio_alarme_panico[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_panico[7] << 8) + faixas_audio_alarme_panico[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_panico[7] << 8) + faixas_audio_alarme_panico[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme = (byte[])audio_system_configuration.attributes["faixas_audio_alarme"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme"]["address"] + faixas_audio_alarme.Length); i++, j++)
                    { faixas_audio_alarme[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme[1] << 8) + faixas_audio_alarme[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme[1] << 8) + faixas_audio_alarme[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme[1] << 8) + faixas_audio_alarme[0];
                        }

                        if (((faixas_audio_alarme[3] << 8) + faixas_audio_alarme[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme[3] << 8) + faixas_audio_alarme[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme[3] << 8) + faixas_audio_alarme[2];
                        }

                        if (((faixas_audio_alarme[5] << 8) + faixas_audio_alarme[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme[5] << 8) + faixas_audio_alarme[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme[5] << 8) + faixas_audio_alarme[4];
                        }

                        if (((faixas_audio_alarme[7] << 8) + faixas_audio_alarme[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme[7] << 8) + faixas_audio_alarme[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme[7] << 8) + faixas_audio_alarme[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_panico"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_panico = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_panico"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_panico"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_panico"]["address"] + faixas_audio_alarme_panico.Length); i++, j++)
                    { faixas_audio_alarme_panico[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_panico[1] << 8) + faixas_audio_alarme_panico[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_panico[1] << 8) + faixas_audio_alarme_panico[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_panico[1] << 8) + faixas_audio_alarme_panico[0];
                        }

                        if (((faixas_audio_alarme_panico[3] << 8) + faixas_audio_alarme_panico[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_panico[3] << 8) + faixas_audio_alarme_panico[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_panico[3] << 8) + faixas_audio_alarme_panico[2];
                        }

                        if (((faixas_audio_alarme_panico[5] << 8) + faixas_audio_alarme_panico[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_panico[5] << 8) + faixas_audio_alarme_panico[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_panico[5] << 8) + faixas_audio_alarme_panico[4];
                        }

                        if (((faixas_audio_alarme_panico[7] << 8) + faixas_audio_alarme_panico[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_panico[7] << 8) + faixas_audio_alarme_panico[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_panico[7] << 8) + faixas_audio_alarme_panico[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_medico"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_medico = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_medico"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_medico"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_medico"]["address"] + faixas_audio_alarme_medico.Length); i++, j++)
                    { faixas_audio_alarme_medico[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_medico[1] << 8) + faixas_audio_alarme_medico[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_medico[1] << 8) + faixas_audio_alarme_medico[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_medico[1] << 8) + faixas_audio_alarme_medico[0];
                        }

                        if (((faixas_audio_alarme_medico[3] << 8) + faixas_audio_alarme_medico[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_medico[3] << 8) + faixas_audio_alarme_medico[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_medico[3] << 8) + faixas_audio_alarme_medico[2];
                        }

                        if (((faixas_audio_alarme_medico[5] << 8) + faixas_audio_alarme_medico[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_medico[5] << 8) + faixas_audio_alarme_medico[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_medico[5] << 8) + faixas_audio_alarme_medico[4];
                        }

                        if (((faixas_audio_alarme_medico[7] << 8) + faixas_audio_alarme_medico[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_medico[7] << 8) + faixas_audio_alarme_medico[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_medico[7] << 8) + faixas_audio_alarme_medico[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_incendio"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_incendio = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_incendio"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_incendio"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_incendio"]["address"] + faixas_audio_alarme_incendio.Length); i++, j++)
                    { faixas_audio_alarme_incendio[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_incendio[1] << 8) + faixas_audio_alarme_incendio[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_incendio[1] << 8) + faixas_audio_alarme_incendio[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_incendio[1] << 8) + faixas_audio_alarme_incendio[0];
                        }

                        if (((faixas_audio_alarme_incendio[3] << 8) + faixas_audio_alarme_incendio[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_incendio[3] << 8) + faixas_audio_alarme_incendio[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_incendio[3] << 8) + faixas_audio_alarme_incendio[2];
                        }

                        if (((faixas_audio_alarme_incendio[5] << 8) + faixas_audio_alarme_incendio[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_incendio[5] << 8) + faixas_audio_alarme_incendio[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_incendio[5] << 8) + faixas_audio_alarme_incendio[4];
                        }

                        if (((faixas_audio_alarme_incendio[7] << 8) + faixas_audio_alarme_incendio[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_incendio[7] << 8) + faixas_audio_alarme_incendio[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_incendio[7] << 8) + faixas_audio_alarme_incendio[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_mask"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_mask = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_mask"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_mask"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_mask"]["address"] + faixas_audio_alarme_mask.Length); i++, j++)
                    { faixas_audio_alarme_mask[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_mask[1] << 8) + faixas_audio_alarme_mask[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_mask[1] << 8) + faixas_audio_alarme_mask[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_mask[1] << 8) + faixas_audio_alarme_mask[0];
                        }

                        if (((faixas_audio_alarme_mask[3] << 8) + faixas_audio_alarme_mask[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_mask[3] << 8) + faixas_audio_alarme_mask[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_mask[3] << 8) + faixas_audio_alarme_mask[2];
                        }

                        if (((faixas_audio_alarme_mask[5] << 8) + faixas_audio_alarme_mask[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_mask[5] << 8) + faixas_audio_alarme_mask[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_mask[5] << 8) + faixas_audio_alarme_mask[4];
                        }

                        if (((faixas_audio_alarme_mask[7] << 8) + faixas_audio_alarme_mask[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_mask[7] << 8) + faixas_audio_alarme_mask[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_mask[7] << 8) + faixas_audio_alarme_mask[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_supervisao"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_supervisao = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_supervisao"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_supervisao"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_supervisao"]["address"] + faixas_audio_alarme_supervisao.Length); i++, j++)
                    { faixas_audio_alarme_supervisao[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_supervisao[1] << 8) + faixas_audio_alarme_supervisao[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_supervisao[1] << 8) + faixas_audio_alarme_supervisao[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_supervisao[1] << 8) + faixas_audio_alarme_supervisao[0];
                        }

                        if (((faixas_audio_alarme_supervisao[3] << 8) + faixas_audio_alarme_supervisao[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_supervisao[3] << 8) + faixas_audio_alarme_supervisao[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_supervisao[3] << 8) + faixas_audio_alarme_supervisao[2];
                        }

                        if (((faixas_audio_alarme_supervisao[5] << 8) + faixas_audio_alarme_supervisao[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_supervisao[5] << 8) + faixas_audio_alarme_supervisao[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_supervisao[5] << 8) + faixas_audio_alarme_supervisao[4];
                        }

                        if (((faixas_audio_alarme_supervisao[7] << 8) + faixas_audio_alarme_supervisao[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_supervisao[7] << 8) + faixas_audio_alarme_supervisao[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_supervisao[7] << 8) + faixas_audio_alarme_supervisao[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_alarme_24"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_alarme_24 = (byte[])audio_system_configuration.attributes["faixas_audio_alarme_24"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_24"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_alarme_24"]["address"] + faixas_audio_alarme_24.Length); i++, j++)
                    { faixas_audio_alarme_24[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_alarme_24[1] << 8) + faixas_audio_alarme_24[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_24[1] << 8) + faixas_audio_alarme_24[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_alarme_24[1] << 8) + faixas_audio_alarme_24[0];
                        }

                        if (((faixas_audio_alarme_24[3] << 8) + faixas_audio_alarme_24[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_24[3] << 8) + faixas_audio_alarme_24[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_alarme_24[3] << 8) + faixas_audio_alarme_24[2];
                        }

                        if (((faixas_audio_alarme_24[5] << 8) + faixas_audio_alarme_24[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_24[5] << 8) + faixas_audio_alarme_24[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_alarme_24[5] << 8) + faixas_audio_alarme_24[4];
                        }

                        if (((faixas_audio_alarme_24[7] << 8) + faixas_audio_alarme_24[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_24[7] << 8) + faixas_audio_alarme_24[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_alarme_24[7] << 8) + faixas_audio_alarme_24[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_codigo_ok"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_codigo_ok = (byte[])audio_system_configuration.attributes["faixas_audio_codigo_ok"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_codigo_ok"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_codigo_ok"]["address"] + faixas_audio_codigo_ok.Length); i++, j++)
                    { faixas_audio_codigo_ok[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_codigo_ok[1] << 8) + faixas_audio_codigo_ok[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_codigo_ok[1] << 8) + faixas_audio_codigo_ok[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_codigo_ok[1] << 8) + faixas_audio_codigo_ok[0];
                        }

                        if (((faixas_audio_codigo_ok[3] << 8) + faixas_audio_codigo_ok[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_codigo_ok[3] << 8) + faixas_audio_codigo_ok[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_codigo_ok[3] << 8) + faixas_audio_codigo_ok[2];
                        }

                        if (((faixas_audio_codigo_ok[5] << 8) + faixas_audio_codigo_ok[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_codigo_ok[5] << 8) + faixas_audio_codigo_ok[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_codigo_ok[5] << 8) + faixas_audio_codigo_ok[4];
                        }

                        if (((faixas_audio_codigo_ok[7] << 8) + faixas_audio_codigo_ok[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_codigo_ok[7] << 8) + faixas_audio_codigo_ok[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_codigo_ok[7] << 8) + faixas_audio_codigo_ok[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_codigo_errado"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_codigo_errado = (byte[])audio_system_configuration.attributes["faixas_audio_codigo_errado"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_codigo_errado"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_codigo_errado"]["address"] + faixas_audio_codigo_errado.Length); i++, j++)
                    { faixas_audio_codigo_errado[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_codigo_errado[1] << 8) + faixas_audio_codigo_errado[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_codigo_errado[1] << 8) + faixas_audio_codigo_errado[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_codigo_errado[1] << 8) + faixas_audio_codigo_errado[0];
                        }

                        if (((faixas_audio_codigo_errado[3] << 8) + faixas_audio_codigo_errado[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_codigo_errado[3] << 8) + faixas_audio_codigo_errado[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_codigo_errado[3] << 8) + faixas_audio_codigo_errado[2];
                        }

                        if (((faixas_audio_codigo_errado[5] << 8) + faixas_audio_codigo_errado[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_codigo_errado[5] << 8) + faixas_audio_codigo_errado[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_codigo_errado[5] << 8) + faixas_audio_codigo_errado[4];
                        }

                        if (((faixas_audio_codigo_errado[7] << 8) + faixas_audio_codigo_errado[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_codigo_errado[7] << 8) + faixas_audio_codigo_errado[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_codigo_errado[7] << 8) + faixas_audio_codigo_errado[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_particao"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_particao = (byte[])audio_system_configuration.attributes["faixas_audio_particao"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_particao"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_particao"]["address"] + faixas_audio_particao.Length); i++, j++)
                    { faixas_audio_particao[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_particao[1] << 8) + faixas_audio_particao[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_particao[1] << 8) + faixas_audio_particao[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_particao[1] << 8) + faixas_audio_particao[0];
                        }

                        if (((faixas_audio_particao[3] << 8) + faixas_audio_particao[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_particao[3] << 8) + faixas_audio_particao[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_particao[3] << 8) + faixas_audio_particao[2];
                        }

                        if (((faixas_audio_particao[5] << 8) + faixas_audio_particao[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_particao[5] << 8) + faixas_audio_particao[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_particao[5] << 8) + faixas_audio_particao[4];
                        }

                        if (((faixas_audio_particao[7] << 8) + faixas_audio_particao[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_particao[7] << 8) + faixas_audio_particao[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_particao[7] << 8) + faixas_audio_particao[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_armada"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_armada = (byte[])audio_system_configuration.attributes["faixas_audio_armada"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_armada"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_armada"]["address"] + faixas_audio_armada.Length); i++, j++)
                    { faixas_audio_armada[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_armada[1] << 8) + faixas_audio_armada[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_armada[1] << 8) + faixas_audio_armada[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_armada[1] << 8) + faixas_audio_armada[0];
                        }

                        if (((faixas_audio_armada[3] << 8) + faixas_audio_armada[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_armada[3] << 8) + faixas_audio_armada[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_armada[3] << 8) + faixas_audio_armada[2];
                        }

                        if (((faixas_audio_armada[5] << 8) + faixas_audio_armada[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_armada[5] << 8) + faixas_audio_armada[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_armada[5] << 8) + faixas_audio_armada[4];
                        }

                        if (((faixas_audio_armada[7] << 8) + faixas_audio_armada[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_armada[7] << 8) + faixas_audio_armada[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_armada[7] << 8) + faixas_audio_armada[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_desarmada"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_desarmada = (byte[])audio_system_configuration.attributes["faixas_audio_desarmada"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_desarmada"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_desarmada"]["address"] + faixas_audio_desarmada.Length); i++, j++)
                    { faixas_audio_desarmada[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_desarmada[1] << 8) + faixas_audio_desarmada[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_desarmada[1] << 8) + faixas_audio_desarmada[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_desarmada[1] << 8) + faixas_audio_desarmada[0];
                        }

                        if (((faixas_audio_desarmada[3] << 8) + faixas_audio_desarmada[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_desarmada[3] << 8) + faixas_audio_desarmada[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_desarmada[3] << 8) + faixas_audio_desarmada[2];
                        }

                        if (((faixas_audio_desarmada[5] << 8) + faixas_audio_desarmada[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_desarmada[5] << 8) + faixas_audio_desarmada[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_desarmada[5] << 8) + faixas_audio_desarmada[4];
                        }

                        if (((faixas_audio_desarmada[7] << 8) + faixas_audio_desarmada[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_desarmada[7] << 8) + faixas_audio_desarmada[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_desarmada[7] << 8) + faixas_audio_desarmada[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_restauro"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_restauro = (byte[])audio_system_configuration.attributes["faixas_audio_restauro"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_restauro"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_restauro"]["address"] + faixas_audio_restauro.Length); i++, j++)
                    { faixas_audio_restauro[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_restauro[1] << 8) + faixas_audio_restauro[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_restauro[1] << 8) + faixas_audio_restauro[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_restauro[1] << 8) + faixas_audio_restauro[0];
                        }

                        if (((faixas_audio_restauro[3] << 8) + faixas_audio_restauro[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_restauro[3] << 8) + faixas_audio_restauro[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_restauro[3] << 8) + faixas_audio_restauro[2];
                        }

                        if (((faixas_audio_restauro[5] << 8) + faixas_audio_restauro[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_restauro[5] << 8) + faixas_audio_restauro[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_restauro[5] << 8) + faixas_audio_restauro[4];
                        }

                        if (((faixas_audio_restauro[7] << 8) + faixas_audio_restauro[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_restauro[7] << 8) + faixas_audio_restauro[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_restauro[7] << 8) + faixas_audio_restauro[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_falha_linha_telefone"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_falha_linha_telefone = (byte[])audio_system_configuration.attributes["faixas_audio_falha_linha_telefone"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_falha_linha_telefone"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_falha_linha_telefone"]["address"] + faixas_audio_falha_linha_telefone.Length); i++, j++)
                    { faixas_audio_falha_linha_telefone[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_falha_linha_telefone[1] << 8) + faixas_audio_falha_linha_telefone[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_falha_linha_telefone[1] << 8) + faixas_audio_falha_linha_telefone[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_falha_linha_telefone[1] << 8) + faixas_audio_falha_linha_telefone[0];
                        }

                        if (((faixas_audio_falha_linha_telefone[3] << 8) + faixas_audio_falha_linha_telefone[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_falha_linha_telefone[3] << 8) + faixas_audio_falha_linha_telefone[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_falha_linha_telefone[3] << 8) + faixas_audio_falha_linha_telefone[2];
                        }

                        if (((faixas_audio_falha_linha_telefone[5] << 8) + faixas_audio_falha_linha_telefone[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_falha_linha_telefone[5] << 8) + faixas_audio_falha_linha_telefone[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_falha_linha_telefone[5] << 8) + faixas_audio_falha_linha_telefone[4];
                        }

                        if (((faixas_audio_falha_linha_telefone[7] << 8) + faixas_audio_falha_linha_telefone[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_falha_linha_telefone[7] << 8) + faixas_audio_falha_linha_telefone[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_falha_linha_telefone[7] << 8) + faixas_audio_falha_linha_telefone[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_linha_telefone_ok"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_linha_telefone_ok = (byte[])audio_system_configuration.attributes["faixas_audio_linha_telefone_ok"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_linha_telefone_ok"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_linha_telefone_ok"]["address"] + faixas_audio_linha_telefone_ok.Length); i++, j++)
                    { faixas_audio_linha_telefone_ok[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_linha_telefone_ok[1] << 8) + faixas_audio_linha_telefone_ok[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_linha_telefone_ok[1] << 8) + faixas_audio_linha_telefone_ok[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_linha_telefone_ok[1] << 8) + faixas_audio_linha_telefone_ok[0];
                        }

                        if (((faixas_audio_linha_telefone_ok[3] << 8) + faixas_audio_linha_telefone_ok[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_linha_telefone_ok[3] << 8) + faixas_audio_linha_telefone_ok[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_linha_telefone_ok[3] << 8) + faixas_audio_linha_telefone_ok[2];
                        }

                        if (((faixas_audio_linha_telefone_ok[5] << 8) + faixas_audio_linha_telefone_ok[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_linha_telefone_ok[5] << 8) + faixas_audio_linha_telefone_ok[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_linha_telefone_ok[5] << 8) + faixas_audio_linha_telefone_ok[4];
                        }

                        if (((faixas_audio_linha_telefone_ok[7] << 8) + faixas_audio_linha_telefone_ok[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_linha_telefone_ok[7] << 8) + faixas_audio_linha_telefone_ok[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_linha_telefone_ok[7] << 8) + faixas_audio_linha_telefone_ok[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_falha_enviar_eventos"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_falha_enviar_eventos = (byte[])audio_system_configuration.attributes["faixas_audio_falha_enviar_eventos"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_falha_enviar_eventos"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_falha_enviar_eventos"]["address"] + faixas_audio_falha_enviar_eventos.Length); i++, j++)
                    { faixas_audio_falha_enviar_eventos[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_falha_enviar_eventos[1] << 8) + faixas_audio_falha_enviar_eventos[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_falha_enviar_eventos[1] << 8) + faixas_audio_falha_enviar_eventos[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_falha_enviar_eventos[1] << 8) + faixas_audio_falha_enviar_eventos[0];
                        }

                        if (((faixas_audio_falha_enviar_eventos[3] << 8) + faixas_audio_falha_enviar_eventos[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_falha_enviar_eventos[3] << 8) + faixas_audio_falha_enviar_eventos[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_falha_enviar_eventos[3] << 8) + faixas_audio_falha_enviar_eventos[2];
                        }

                        if (((faixas_audio_falha_enviar_eventos[5] << 8) + faixas_audio_falha_enviar_eventos[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_falha_enviar_eventos[5] << 8) + faixas_audio_falha_enviar_eventos[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_falha_enviar_eventos[5] << 8) + faixas_audio_falha_enviar_eventos[4];
                        }

                        if (((faixas_audio_falha_enviar_eventos[7] << 8) + faixas_audio_falha_enviar_eventos[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_falha_enviar_eventos[7] << 8) + faixas_audio_falha_enviar_eventos[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_falha_enviar_eventos[7] << 8) + faixas_audio_falha_enviar_eventos[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_reset_sistema"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_reset_sistema = (byte[])audio_system_configuration.attributes["faixas_audio_reset_sistema"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_reset_sistema"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_reset_sistema"]["address"] + faixas_audio_reset_sistema.Length); i++, j++)
                    { faixas_audio_reset_sistema[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_reset_sistema[1] << 8) + faixas_audio_reset_sistema[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_reset_sistema[1] << 8) + faixas_audio_reset_sistema[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_reset_sistema[1] << 8) + faixas_audio_reset_sistema[0];
                        }

                        if (((faixas_audio_reset_sistema[3] << 8) + faixas_audio_reset_sistema[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_reset_sistema[3] << 8) + faixas_audio_reset_sistema[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_reset_sistema[3] << 8) + faixas_audio_reset_sistema[2];
                        }

                        if (((faixas_audio_reset_sistema[5] << 8) + faixas_audio_reset_sistema[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_reset_sistema[5] << 8) + faixas_audio_reset_sistema[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_reset_sistema[5] << 8) + faixas_audio_reset_sistema[4];
                        }

                        if (((faixas_audio_reset_sistema[7] << 8) + faixas_audio_reset_sistema[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_reset_sistema[7] << 8) + faixas_audio_reset_sistema[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_reset_sistema[7] << 8) + faixas_audio_reset_sistema[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_tamper_sistema"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_tamper_sistema = (byte[])audio_system_configuration.attributes["faixas_audio_tamper_sistema"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper_sistema"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper_sistema"]["address"] + faixas_audio_tamper_sistema.Length); i++, j++)
                    { faixas_audio_tamper_sistema[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_tamper_sistema[1] << 8) + faixas_audio_tamper_sistema[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper_sistema[1] << 8) + faixas_audio_tamper_sistema[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper_sistema[1] << 8) + faixas_audio_tamper_sistema[0];
                        }

                        if (((faixas_audio_tamper_sistema[3] << 8) + faixas_audio_tamper_sistema[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper_sistema[3] << 8) + faixas_audio_tamper_sistema[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper_sistema[3] << 8) + faixas_audio_tamper_sistema[2];
                        }

                        if (((faixas_audio_tamper_sistema[5] << 8) + faixas_audio_tamper_sistema[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper_sistema[5] << 8) + faixas_audio_tamper_sistema[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper_sistema[5] << 8) + faixas_audio_tamper_sistema[4];
                        }

                        if (((faixas_audio_tamper_sistema[7] << 8) + faixas_audio_tamper_sistema[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper_sistema[7] << 8) + faixas_audio_tamper_sistema[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper_sistema[7] << 8) + faixas_audio_tamper_sistema[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_tamper_sirene"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_tamper_sirene = (byte[])audio_system_configuration.attributes["faixas_audio_tamper_sirene"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper_sirene"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper_sirene"]["address"] + faixas_audio_tamper_sirene.Length); i++, j++)
                    { faixas_audio_tamper_sirene[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_tamper_sirene[1] << 8) + faixas_audio_tamper_sirene[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper_sirene[1] << 8) + faixas_audio_tamper_sirene[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper_sirene[1] << 8) + faixas_audio_tamper_sirene[0];
                        }

                        if (((faixas_audio_tamper_sirene[3] << 8) + faixas_audio_tamper_sirene[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper_sirene[3] << 8) + faixas_audio_tamper_sirene[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper_sirene[3] << 8) + faixas_audio_tamper_sirene[2];
                        }

                        if (((faixas_audio_tamper_sirene[5] << 8) + faixas_audio_tamper_sirene[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper_sirene[5] << 8) + faixas_audio_tamper_sirene[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper_sirene[5] << 8) + faixas_audio_tamper_sirene[4];
                        }

                        if (((faixas_audio_tamper_sirene[7] << 8) + faixas_audio_tamper_sirene[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper_sirene[7] << 8) + faixas_audio_tamper_sirene[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper_sirene[7] << 8) + faixas_audio_tamper_sirene[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_tamper_teclado"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_tamper_teclado = (byte[])audio_system_configuration.attributes["faixas_audio_tamper_teclado"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper_teclado"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_tamper_teclado"]["address"] + faixas_audio_tamper_teclado.Length); i++, j++)
                    { faixas_audio_tamper_teclado[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_tamper_teclado[1] << 8) + faixas_audio_tamper_teclado[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper_teclado[1] << 8) + faixas_audio_tamper_teclado[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_tamper_teclado[1] << 8) + faixas_audio_tamper_teclado[0];
                        }

                        if (((faixas_audio_tamper_teclado[3] << 8) + faixas_audio_tamper_teclado[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper_teclado[3] << 8) + faixas_audio_tamper_teclado[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_tamper_teclado[3] << 8) + faixas_audio_tamper_teclado[2];
                        }

                        if (((faixas_audio_tamper_teclado[5] << 8) + faixas_audio_tamper_teclado[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper_teclado[5] << 8) + faixas_audio_tamper_teclado[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_tamper_teclado[5] << 8) + faixas_audio_tamper_teclado[4];
                        }

                        if (((faixas_audio_tamper_teclado[7] << 8) + faixas_audio_tamper_teclado[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper_teclado[7] << 8) + faixas_audio_tamper_teclado[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_tamper_teclado[7] << 8) + faixas_audio_tamper_teclado[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }
                else if (((int)audio_system_configuration.attributes["faixas_audio_boas_vindas"]["index"]) == audio_system_configuration_to_read)
                {
                    byte[] faixas_audio_boas_vindas = (byte[])audio_system_configuration.attributes["faixas_audio_boas_vindas"]["value"];
                    for (int i = (7 + (int)audio_system_configuration.attributes["faixas_audio_boas_vindas"]["address"]), j = 0; i < (7 + (int)audio_system_configuration.attributes["faixas_audio_boas_vindas"]["address"] + faixas_audio_boas_vindas.Length); i++, j++)
                    { faixas_audio_boas_vindas[j] = buf[i]; }

                    try
                    {
                        #region AUDIO TRACKS
                        if (((faixas_audio_boas_vindas[1] << 8) + faixas_audio_boas_vindas[0]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_boas_vindas[1] << 8) + faixas_audio_boas_vindas[0];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO1"] = (faixas_audio_boas_vindas[1] << 8) + faixas_audio_boas_vindas[0];
                        }

                        if (((faixas_audio_boas_vindas[3] << 8) + faixas_audio_boas_vindas[2]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_boas_vindas[3] << 8) + faixas_audio_boas_vindas[2];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO2"] = (faixas_audio_boas_vindas[3] << 8) + faixas_audio_boas_vindas[2];
                        }

                        if (((faixas_audio_boas_vindas[5] << 8) + faixas_audio_boas_vindas[4]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_boas_vindas[5] << 8) + faixas_audio_boas_vindas[4];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO3"] = (faixas_audio_boas_vindas[5] << 8) + faixas_audio_boas_vindas[4];
                        }

                        if (((faixas_audio_boas_vindas[7] << 8) + faixas_audio_boas_vindas[6]).Equals(0xffff))
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_boas_vindas[7] << 8) + faixas_audio_boas_vindas[6];
                        }
                        else
                        {
                            databaseDataSet.AudioSystemConfiguration.Rows[(int)audio_system_configuration_to_read]["AUDIO4"] = (faixas_audio_boas_vindas[7] << 8) + faixas_audio_boas_vindas[6];
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    { await DialogManager.ShowMessageAsync(this, ex.Message, ""); }
                }

                databaseDataSet.AudioSystemConfiguration.AcceptChanges();

               // EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
                //databaseDataSetEventTableAdapter.Update(databaseDataSet.Event);
                #endregion
            }
            else if (addr >= Constants.KP_EXPANDERS_INIC_ADDR && addr < Constants.KP_EXPANDERS_FINAL_ADDR) //TODO: Change to areas and improve this process
            {
                #region Expanders
                int expander_to_read = ((addr - Constants.KP_EXPANDERS_INIC_ADDR) / (int)Constants.KP_FLASH_TAMANHO_DADOS_EXPANDER_FLASH);

                Protocol.Expanders expander = new Protocol.Expanders();
                byte[] description = (byte[])expander.attributes["description"]["value"];
                for (int i = (7 + (int)expander.attributes["description"]["address"]), j = 0; i < (7 + (int)expander.attributes["description"]["address"] + description.Length); i++, j++)
                {
                    description[j] = buf[i];
                }

                byte[] expander_config_pins = (byte[])expander.attributes["config_pins"]["value"];
                for (int i = (7 + (int)expander.attributes["config_pins"]["address"]), j = 0; i < (7 + (int)expander.attributes["config_pins"]["address"] + expander_config_pins.Length); i++, j++)
                {
                    expander_config_pins[j] = buf[i];
                }

                byte[] expander_active_outputs = (byte[])expander.attributes["active_outputs"]["value"];
                for (int i = (7 + (int)expander.attributes["active_outputs"]["address"]), j = 0; i < (7 + (int)expander.attributes["active_outputs"]["address"] + expander_active_outputs.Length); i++, j++)
                {
                    expander_active_outputs[j] = buf[i];
                }


                byte[] options = (byte[])expander.attributes["options"]["value"];
                for (int i = (7 + (int)expander.attributes["options"]["address"]), j = 0; i < (7 + (int)expander.attributes["options"]["address"] + options.Length); i++, j++)
                {
                    options[j] = buf[i];
                }

                //byte[] expander_reserved_1 = (byte[])expander.attributes["reserved_1"]["value"];
                //for (int i = (7 + (int)expander.attributes["reserved_1"]["address"]), j = 0; i < (7 + (int)expander.attributes["reserved_1"]["address"] + expander_reserved_1.Length); i++, j++)
                //{
                //    expander_reserved_1[j] = buf[i];
                //}

                //byte[] expander_reserved_2 = (byte[])expander.attributes["reserved_2"]["value"];
                //for (int i = (7 + (int)expander.attributes["reserved_2"]["address"]), j = 0; i < (7 + (int)expander.attributes["reserved_2"]["address"] + expander_reserved_2.Length); i++, j++)
                //{
                //    expander_reserved_2[j] = buf[i];
                //}

                //byte[] expander_reserved_3 = (byte[])expander.attributes["reserved_3"]["value"];
                //for (int i = (7 + (int)expander.attributes["reserved_3"]["address"]), j = 0; i < (7 + (int)expander.attributes["reserved_3"]["address"] + expander_reserved_3.Length); i++, j++)
                //{
                //    expander_reserved_3[j] = buf[i];
                //}

                //byte[] expander_reserved_4 = (byte[])expander.attributes["reserved_4"]["value"];
                //for (int i = (7 + (int)expander.attributes["reserved_4"]["address"]), j = 0; i < (7 + (int)expander.attributes["reserved_4"]["address"] + expander_reserved_4.Length); i++, j++)
                //{
                //    expander_reserved_4[j] = buf[i];
                //}

                byte[] expander_circuit_type = (byte[])expander.attributes["circuit_type"]["value"];
                for (int i = (7 + (int)expander.attributes["circuit_type"]["address"]), j = 0; i < (7 + (int)expander.attributes["circuit_type"]["address"] + expander_circuit_type.Length); i++, j++)
                {
                    expander_circuit_type[j] = (byte)buf[i];
                }

                byte[] expander_configs_r1 = (byte[])expander.attributes["configs_r1"]["value"];
                for (int i = (7 + (int)expander.attributes["configs_r1"]["address"]), j = 0; i < (7 + (int)expander.attributes["configs_r1"]["address"] + expander_configs_r1.Length); i++, j++)
                {
                    expander_configs_r1[j] = (byte)buf[i];
                }

                byte[] expander_type_r1 = (byte[])expander.attributes["type_r1"]["value"];
                for (int i = (7 + (int)expander.attributes["type_r1"]["address"]), j = 0; i < (7 + (int)expander.attributes["type_r1"]["address"] + expander_type_r1.Length); i++, j++)
                {
                    expander_configs_r1[j] = (byte)buf[i];
                }

                byte[] expander_configs_r2 = (byte[])expander.attributes["configs_r2"]["value"];
                for (int i = (7 + (int)expander.attributes["configs_r2"]["address"]), j = 0; i < (7 + (int)expander.attributes["configs_r2"]["address"] + expander_configs_r2.Length); i++, j++)
                {
                    expander_configs_r2[j] = (byte)buf[i];
                }

                byte[] expander_type_r2 = (byte[])expander.attributes["type_r2"]["value"];
                for (int i = (7 + (int)expander.attributes["type_r2"]["address"]), j = 0; i < (7 + (int)expander.attributes["type_r2"]["address"] + expander_type_r2.Length); i++, j++)
                {
                    expander_configs_r2[j] = (byte)buf[i];
                }
                byte[] expander_configs_r3 = (byte[])expander.attributes["configs_r3"]["value"];
                for (int i = (7 + (int)expander.attributes["configs_r3"]["address"]), j = 0; i < (7 + (int)expander.attributes["configs_r3"]["address"] + expander_configs_r3.Length); i++, j++)
                {
                    expander_configs_r3[j] = (byte)buf[i];
                }

                byte[] expander_type_r3 = (byte[])expander.attributes["type_r3"]["value"];
                for (int i = (7 + (int)expander.attributes["type_r3"]["address"]), j = 0; i < (7 + (int)expander.attributes["type_r3"]["address"] + expander_type_r3.Length); i++, j++)
                {
                    expander_configs_r3[j] = (byte)buf[i];
                }

                uint expander_options = (uint)(options[3] << 2) + (uint)(options[2] << 1) + (uint)(options[0]);

                uint expander_active = expander_options & ((uint[])expander.attributes["options"]["KP_EXPANDER_RS485_ATIVO"])[0];
                uint expander_enable_tamper = expander_options & ((uint[])expander.attributes["options"]["KP_EXPANDER_ENABLE_TAMPER"])[0];
                uint expander_tamper_type = expander_options & ((uint[])expander.attributes["options"]["KP_EXPANDER_CONFIG_TAMPER_NO_NC"])[0];

                try
                {
                    //String
                    databaseDataSet.Expander.Rows[expander_to_read]["Description"] = Encoding.ASCII.GetString(description).Trim('\0'); ;

                    //Reserved  1-4
                    //databaseDataSet.Expander.Rows[expander_to_read]["Reserved 1"] = expander_reserved_1;
                    //databaseDataSet.Expander.Rows[expander_to_read]["Reserved 2"] = expander_reserved_2;
                    //databaseDataSet.Expander.Rows[expander_to_read]["Reserved 3"] = expander_reserved_3;
                    //databaseDataSet.Expander.Rows[expander_to_read]["Reserved 4"] = expander_reserved_4;

                    ////ConfigPinos
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 1"] = (expander_config_pins[0] & 0x01) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 2"] = (expander_config_pins[0] & 0x02) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 3"] = (expander_config_pins[0] & 0x04) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 4"] = (expander_config_pins[0] & 0x08) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 5"] = (expander_config_pins[0] & 0x10) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 6"] = (expander_config_pins[0] & 0x20) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 7"] = (expander_config_pins[0] & 0x40) > 0;
                    databaseDataSet.Expander.Rows[expander_to_read]["Config pin 8"] = (expander_config_pins[0] & 0x80) > 0;

                    //////OPTIONS
                    databaseDataSet.Expander.Rows[expander_to_read]["Active"] = (expander_active > 0);
                    databaseDataSet.Expander.Rows[expander_to_read]["Enable tamper"] = (expander_enable_tamper > 0);
                    databaseDataSet.Expander.Rows[expander_to_read]["Config tamper"] = (expander_tamper_type > 0);

                }
                catch (Exception ex)
                {
                    await DialogManager.ShowMessageAsync(this, ex.Message, "");
                    //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }

                databaseDataSet.Expander.AcceptChanges();

               // EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
               // databaseDataSetEventTableAdapter.Update(databaseDataSet.Event);
                #endregion
            }
            else if (addr >= 0x600000 && addr < 0x700000) //TODO: Change to areas and improve this process
            {
                #region DEBUG
                /////DEBUG TO READ UPDATE DATA FROM FLASH TO A FILE//////////////

                //if (FirmwareUpdateSize != 0)
                //{
                //    for (int count = 7; count < 247; count++)
                //    {
                //        int a = buf[count];
                //        //for (long count = 0; count < combined_file_data_bytes.Length; count++)
                //        //{
                //            string path = @"c:\temp\MyTestRead.txt";
                //            // This text is added only once to the file.
                //            if (!File.Exists(path))
                //            {
                //                // Create a file to write to.
                //                using (StreamWriter sw = File.CreateText(path))
                //                {
                //                    sw.Write("x" + a.ToString("x2"));
                //                }
                //            }

                //            // This text is always added, making the file longer over time
                //            // if it is not deleted.
                //            using (StreamWriter sw = File.AppendText(path))
                //            {
                //                sw.Write("x" + a.ToString("x2"));
                //            }
                //            //this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = count * (float)(100.0 / (float)((combined_file_data_bytes.Length)))));
                //            //this.Dispatcher.Invoke((Action)(() => RichTextBoxUpdateDebug.AppendText("x" + combined_file_data_bytes[count].ToString("x2"))));
                //        //}
                //        //this.Dispatcher.Invoke((Action)(() => RichTextBoxUpdateDebug.AppendText("x"+a.ToString("x2"))));
                //    }
                //}
                //else
                //{
                //    FirmwareUpdateSize = buf[8] + (buf[9] << 8) + (buf[10] << 16) + (buf[11] << 24);
                //    Protocol.FirmwareUpdate update = new Protocol.FirmwareUpdate();
                //    //update.Read(this);

                //    //this.Dispatcher.Invoke((Action)(() => await this.ShowProgressAsync(Properties.Resources.PleaseWait, ""));
                //    //controller.Maximum = 100.0;
                //    //controller.Minimum = 0.0;

                //    await Task.Run(() =>
                //    {                       
                //        //controller.SetMessage(Properties.Resources.ReadingZones);
                //        for (int i = 0; i < ((FirmwareUpdateSize/239) + 5); i++)
                //        {
                //            this.Dispatcher.Invoke((Action)(() => update.Read(this, (uint)i)));
                //            this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = i * (float)(100.0 / (float)((FirmwareUpdateSize / 239) + 5.0))));
                //            //this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)((FirmwareUpdateSize/240) + 6.0)))));
                //            System.Threading.Thread.Sleep(200);
                //        }
                //        MessageBox.Show("Concluído");
                //        this.Dispatcher.Invoke((Action)(() => BaseProgressBar.Value = 0));
                //        //controller.CloseAsync();
                //    });

                //}
                #endregion
            }
        }

        void AddEventItemToDatagrid()
        {
            this.Dispatcher.Invoke((Action)(() => eventDataGrid.Items.Refresh()));
            ICollectionView dataView = CollectionViewSource.GetDefaultView(eventDataGrid.ItemsSource);
            this.Dispatcher.Invoke((Action)(() => dataView.SortDescriptions.Clear()));
            this.Dispatcher.Invoke((Action)(() => dataView.SortDescriptions.Add(new SortDescription("EventId", ListSortDirection.Descending))));
            this.Dispatcher.Invoke((Action)(() => dataView.Refresh()));

            //EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
            //databaseDataSetEventTableAdapter.Fill(databaseDataSet.Event);

            //this.Dispatcher.Invoke((Action)(() => eventDataGrid.Items.Refresh()));
            //DataTable dtEvent = (DataTable)eventDataGrid.ItemsSource;
            //ICollectionView dataView = CollectionViewSource.GetDefaultView(eventDataGrid.ItemsSource);
            //this.Dispatcher.Invoke((Action)(() => dataView.SortDescriptions.Clear()));
            //this.Dispatcher.Invoke((Action)(() => dataView.SortDescriptions.Add(new SortDescription("EventId", ListSortDirection.Descending))));
            //this.Dispatcher.Invoke((Action)(() => dataView.Refresh()));
            //this.Dispatcher.Invoke((Action)(() => dtEvent.DefaultView.ToTable()));

            //databaseDataSetEventTableAdapter.Fill(databaseDataSet.Event);
        }

        private async void UpdateDateHourTile_Click(object sender, RoutedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                Protocol.General protocol = new Protocol.General();
                protocol.update_hour_and_date(this);

                await DialogManager.ShowMessageAsync(this, Properties.Resources.DateTimeUpdated, "");
            }
            else await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
        }

        private void togglelanguage_Click(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentUICulture =
           new System.Globalization.CultureInfo("en");
        }

        private async void read_update_Click(object sender, RoutedEventArgs e)
        {
            Protocol.FirmwareUpdate update = new Protocol.FirmwareUpdate();
            //update.Read(this);

            await Task.Run(() =>
            {
                if (FirmwareUpdateSize == 0)
                {
                    this.Dispatcher.Invoke((Action)(() => update.Read(this, 0)));
                }

            });
        }

        public static void SetFullAccessPermissionsForEveryone(string directoryPath)
        {
            //Everyone Identity
            IdentityReference everyoneIdentity = new SecurityIdentifier(WellKnownSidType.WorldSid,
                                                       null);

            DirectorySecurity dir_security = Directory.GetAccessControl(directoryPath);

            FileSystemAccessRule full_access_rule = new FileSystemAccessRule(everyoneIdentity,
                            FileSystemRights.FullControl, InheritanceFlags.ContainerInherit |
                             InheritanceFlags.ObjectInherit, PropagationFlags.None,
                             AccessControlType.Allow);
            dir_security.AddAccessRule(full_access_rule);

            Directory.SetAccessControl(directoryPath, dir_security);
        }

        private async void ChooseFileButton_Click(object sender, RoutedEventArgs e)
        {
            combined_file_data_bytes = new byte[0];
            Stream myStream = null;
            OpenFileDialog ofd = new OpenFileDialog();

            string app_directory = Environment.GetCommandLineArgs()[0];
            string directory = System.IO.Path.GetDirectoryName(app_directory);

            

            //ofd.InitialDirectory = directory;
            ofd.Filter = "Mega-X FW files (*.hex)|*.hex";
            ofd.FilterIndex = 2;
            ofd.RestoreDirectory = true;
            string newFileName;
            int line_counter = 0;
            string line;
            string[] file_line_bytes = new string[50000];
            byte[] file_line_data_bytes = new byte[500000];

            if (ofd.ShowDialog().Equals(true)) // If dialog opened
            {
                try
                {
                    if ((myStream = ofd.OpenFile()) != null) //if a file was selected
                    {
                        this.Dispatcher.Invoke((Action)(() => upload_update_file_button.IsEnabled = false));
                        newFileName = ofd.InitialDirectory + ofd.FileName;//directory + "\\DoControl.X.production.hex";/////    <<<<<<<<<<<<<<<<--------------------------------- get selected file path
                        //Add name to the text box
                        this.Dispatcher.Invoke((Action)(() => upload_file_path_textbox.Text = newFileName));
                        //Open the file
                        System.IO.StreamReader file = new System.IO.StreamReader(newFileName);

                        var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");

                        await Task.Run(() =>
                        {
                            controller.SetMessage(Properties.Resources.PleaseWait);
                            //Read each line
                            while ((line = file.ReadLine()) != null)
                            {
                                byte[] converted_file_line_data_bytes = new byte[500];
                                //Remove ':' from string
                                line = line.Trim(':');
                                //Add it to a string array
                                file_line_bytes[line_counter] = line;
                                //Get bytes from file's line
                                file_line_data_bytes = Encoding.ASCII.GetBytes(file_line_bytes[line_counter]);



                                //Do the Fernando's proccessment
                                // Change characters ASCII to hexadecimal values
                                //Add header
                                converted_file_line_data_bytes[0] = 0x01;
                                //Save the converted bytes to a new array
                                for (int i = 0, j = 1; i < file_line_data_bytes.Length; i += 2, j++)
                                {
                                    converted_file_line_data_bytes[j] = (byte)(((GetHexFromAscii(file_line_data_bytes[i]) << 4) & 0xF0) | ((GetHexFromAscii(file_line_data_bytes[i + 1])) & 0x0F));
                                }
                                //ADD Footer
                                converted_file_line_data_bytes[(file_line_data_bytes.Length / 2) + 1] = 0x04;

                                //Resize converted file array
                                Array.Resize<byte>(ref converted_file_line_data_bytes, (file_line_data_bytes.Length / 2) + 2);

                                //Check if have data with 0x01 or 0x04 or 0x10 and add an 0x10 if have
                                for (int i = 1; i < (converted_file_line_data_bytes.Length - 1); i++)
                                {
                                    //Check if have data with 0x01 or 0x04 or 0x10 and add an 0x10 if have
                                    switch (converted_file_line_data_bytes[i])
                                    {
                                        case 0x01:
                                        case 0x04:
                                        case 0x10:
                                            /// add a 0x10 to the next byte
                                            Array.Resize<byte>(ref converted_file_line_data_bytes, converted_file_line_data_bytes.Length + 1);
                                            byte[] auxiliar_converted_file_line_data_bytes = new byte[converted_file_line_data_bytes.Length];
                                            converted_file_line_data_bytes.CopyTo(auxiliar_converted_file_line_data_bytes, 0);
                                            for (int j = i + 1; j < converted_file_line_data_bytes.Length; j++)
                                            {
                                                converted_file_line_data_bytes[j] = auxiliar_converted_file_line_data_bytes[j - 1];
                                            }
                                            converted_file_line_data_bytes[i] = 0x10;
                                            i++;
                                            break;
                                    }
                                }


                                //add new bytes to the array
                                combined_file_data_bytes = AppendTwoByteArrays(combined_file_data_bytes, converted_file_line_data_bytes);
                                line_counter++;
                            }

                            controller.CloseAsync();
                            file.Close();

                        });
                        upload_update_file_button.IsEnabled = true;
                        string path_fw = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\fw";
                        string path_fw_file = path_fw + "\\firmware_" + DateTime.Now.ToString("yyMMdd-HHmm") + ".txt";

                        if (!Directory.Exists(path_fw))
                            Directory.CreateDirectory(path_fw);

                        //if (!File.Exists(path_fw_file))
                        //    File.Create(path_fw_file);

                        
                        string combined_file_string = ByteArrayToString(combined_file_data_bytes, combined_file_data_bytes.Length, 1);
                        File.WriteAllText(path_fw_file, combined_file_string);

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Properties.Resources.FileNotFound);
                }
            }
        }

        private async void UploadFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                FirmwareUpdate update_firmware = new FirmwareUpdate();

                var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");
                controller.Maximum = 100.0;
                controller.Minimum = 0.0;
                                
                await Task.Run(() =>
                {
                    int send_msg_error = 0;
                    int error_counter = 0;
                    int msg_size = 235;
                    int bytes_to_send = 0;
                    int bytes_aux_msg = 0;
                    int bytes_aux_block = 0;
                    int msg_complete = 0;
                    int msg_counter = 0;
                    int block_complete = 0;
                    int block_incomplete = 0;
                    int block_counter = 0;
                    int block_counter_aux = 0;
                    int max_block_size = 4096;
                    int block_size = 0;
                    uint address = 0x600000;
                    uint block_address_live = 0;
                    int bytes_left = 0;
                    int max_msg = 0;
                    int savingTime = 200;
                    int max_blocks = 0;


                    string app_directory = Environment.GetCommandLineArgs()[0];
                    string directory = System.IO.Path.GetDirectoryName(app_directory);
                    
                    //Add first five bytes to the array
                    byte[] hex_file_bytes_number = new byte[4];
                    //Get size of file in bytes
                    hex_file_bytes_number = BitConverter.GetBytes(combined_file_data_bytes.Length);
                    //prepare last byte array before send
                    byte[] update_firmware_header = new byte[5];
                    update_firmware_header[0] = 0x00;

                    for (int i = 1; i < update_firmware_header.Length; i++)
                    {
                        update_firmware_header[i] = hex_file_bytes_number[i - 1];
                    }
                    // Add header and address to the byte array
                    combined_file_data_bytes = AppendTwoByteArrays(update_firmware_header, combined_file_data_bytes);

                    max_blocks = (combined_file_data_bytes.Length / max_block_size) + 1;

                    controller.SetMessage(Properties.Resources.UpdatingFirmware);

                    for (bytes_to_send = 0, bytes_aux_msg = 0, bytes_aux_block = 0; bytes_to_send <= combined_file_data_bytes.Length; bytes_to_send++, bytes_aux_msg++, bytes_aux_block++)
                    {
                        block_address_live = 0x600000 + ((uint)max_block_size * (uint)block_counter);

                        if (msg_counter == 0)
                            address = block_address_live;

                        if (bytes_to_send == combined_file_data_bytes.Length)
                            msg_complete = 1;

                        // if(block_counter == max_blocks && )

                        //if (combined_file_data_bytes.Length - bytes_to_send < msg_size && bytes_aux_msg == msg_size)
                        //    msg_size = combined_file_data_bytes.Length - bytes_to_send;
                        msg_size = 235;

                        byte[] fragment_data_bytes = new byte[msg_size];

                        block_size = max_block_size;

                        block_counter_aux = block_counter;

                        //MSG COMPLETE
                        if (bytes_aux_msg == msg_size)
                        {
                            msg_complete = 1;
                            bytes_aux_msg = 0;
                        }

                        if (msg_complete == 1)
                        {
                            msg_complete = 0;
                            msg_counter++;

                            if (!RX_ACK)
                            {
                                if (bytes_to_send < msg_size) //primeira mensagem
                                {
                                    bytes_to_send = 0;
                                    bytes_aux_msg = 0;
                                    bytes_aux_block = 0;
                                }
                                else
                                {
                                    msg_counter--;
                                    bytes_to_send -= msg_size;
                                    bytes_aux_block -= msg_size;
                                }

                                fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();

                                System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);
                                this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(intervalsleeptime);

                            }
                            else
                            {
                                if (bytes_to_send == msg_size) //primeira mensagem
                                {
                                    fragment_data_bytes = (combined_file_data_bytes.Skip(0).Take(msg_size)).ToArray();
                                }
                                else
                                {
                                    if (bytes_to_send == combined_file_data_bytes.Length) //ultima mensagem
                                    {
                                        //msg_size = combined_file_data_bytes.Length - bytes_to_send;
                                        
                                        fragment_data_bytes = (combined_file_data_bytes.Skip(combined_file_data_bytes.Length - bytes_aux_msg).Take(bytes_aux_msg)).ToArray();
                                        block_complete = 1;
                                        block_size = bytes_aux_block;
                                    }
                                    else
                                    {
                                        fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();
                                    }

                                }
                                    

                                System.Diagnostics.Debug.WriteLine("* MSG #" + msg_counter + " | BLOCK ADDRESS: 0x{0:X}  |  0x{1:X} - 0x{2:X} ", block_address_live, address, address + msg_size);
                                this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(intervalsleeptime);

                                if (!RX_ACK)
                                {
                                    fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();

                                    System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);

                                    this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                    System.Threading.Thread.Sleep(intervalsleeptime);
                                }
                                address += (uint)msg_size;
                            }
                        }



                        //BLOCK WITH BYTES LEFT - BLOCK INCOMPLETE    
                        msg_size = 235;
                        max_msg = block_size / msg_size;
                        bytes_left = block_size - (max_msg * msg_size);

                        if (bytes_aux_block == block_size - bytes_left && msg_complete == 0)//bytes_aux_msg == bytes_left && bytes_aux_block == block_size)
                        {
                            block_incomplete = 1;
                            bytes_aux_msg = 0;
                        }

                        if (block_incomplete == 1)
                        {
                            block_incomplete = 0;
                            msg_counter++;

                            if (!RX_ACK)
                            {
                                error_counter++;
                                msg_counter--;
                                bytes_to_send -= msg_size * error_counter;
                                bytes_aux_block -= msg_size * error_counter;
                                address -= (uint)(msg_size * error_counter);

                                fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();
                                System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + (msg_counter - error_counter) + "       ({0})", bytes_to_send);
                                this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(intervalsleeptime);
                            }
                            else
                            {
                                error_counter = 0;
                                //address -= (uint)bytes_left;
                                msg_size = bytes_left;

                                fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();

                                System.Diagnostics.Debug.WriteLine("* MSG #" + msg_counter + " | BLOCK ADDRESS: 0x{0:X}  |  0x{1:X} - 0x{2:X} *", block_address_live, address, address + msg_size);
                                this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(intervalsleeptime);
                            }
                        }
                        
                        

                        //BLOCK COMPLETE
                        if (bytes_aux_block == block_size && block_counter != max_blocks - 1) //ready to start a new block
                        {
                            block_complete = 1;
                            if (!RX_ACK)
                            {
                                //address -= (uint)bytes_left;

                                fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send - bytes_left).Take(bytes_left)).ToArray();
                                System.Diagnostics.Debug.WriteLine("* MSG #" + msg_counter + " | BLOCK ADDRESS: 0x{0:X}  |  0x{1:X} - 0x{2:X} *", block_address_live, address, address + bytes_left);

                                this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)bytes_left)));
                                System.Threading.Thread.Sleep(intervalsleeptime);
                            }

                        }

                        //if (bytes_to_send == combined_file_data_bytes.Length)
                        //{
                        //    block_size = bytes_aux_block;
                        //    msg_size = bytes_aux_msg;
                        //    fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();
                        //    this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                        //    block_complete = 1;
                        //}



                        if (block_complete == 1)
                        {
                            block_complete = 0;
                            if (!RX_ACK)
                            {
                                bytes_to_send -= msg_size;
                                bytes_aux_block -= msg_size;
                                block_incomplete = 1;

                                fragment_data_bytes = (combined_file_data_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();
                                System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);
                                this.Dispatcher.Invoke((Action)(() => update_firmware.Write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(intervalsleeptime);
                            }
                            else
                            { 
                                System.Diagnostics.Debug.WriteLine("######## SAVING BLOCK #" + block_counter + " @ 0x{0:X} ########", block_address_live);
                                
                                this.Dispatcher.Invoke((Action)(() => update_firmware.write_block(this, (uint)block_address_live, (uint)block_size)));
                                System.Threading.Thread.Sleep(savingTime + intervalsleeptime);
                                                                
                                block_counter++;

                                if (block_counter == max_blocks - 1)
                                    block_counter = max_blocks - 1;

                                bytes_aux_msg = 0;
                                bytes_aux_block = 0;
                                msg_counter = 0;

                                address += (uint)bytes_left;
                                //block_address = address;
                                msg_size = 235;
                            }
                        }

                        if (error_counter == times_to_fail)
                        {
                            send_msg_error = 1;
                            break;
                        }
                        else
                            this.Dispatcher.Invoke((Action)(() => controller.SetProgress(block_counter * (100 / (combined_file_data_bytes.Length / 4096)))));
                        
                    }

                   // update_firmware_header[0] = 0xCA;

                    this.Dispatcher.Invoke((Action)(() => update_firmware.write_block(this, (uint)block_address_live, (uint)bytes_aux_block)));
                    System.Threading.Thread.Sleep(savingTime + intervalsleeptime);

                    if (send_msg_error == 1)
                        MessageBox.Show(Properties.Resources.ErrorWhileWritting, "", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve 
                    else
                    {
                        MessageBox.Show(Properties.Resources.UpdateFirmwareDone, "", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve 
                        this.Dispatcher.Invoke((Action)(() => update_firmware.WriteReadyToUpdateByte(this)));
                        System.Threading.Thread.Sleep(500);
                        this.Dispatcher.Invoke((Action)(() => update_firmware.WriteUpdateDone(this)));
                        System.Threading.Thread.Sleep(500);
                    }

                    //serialPort.Close();
                    controller.CloseAsync();

                });

            }
            else
            {
                await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
            }
        }

        public byte GetHexFromAscii(byte origin)
        {
            if (origin >= '0' && origin <= '9')
                return (byte)(origin - 0x30);
            else
                return (byte)(origin - 0x57);

        }

        static byte[] AppendTwoByteArrays(byte[] arrayA, byte[] arrayB)
        {
            byte[] outputBytes = new byte[arrayA.Length + arrayB.Length];
            Buffer.BlockCopy(arrayA, 0, outputBytes, 0, arrayA.Length);
            Buffer.BlockCopy(arrayB, 0, outputBytes, arrayA.Length, arrayB.Length);
            return outputBytes;
        }

        private void DebugClearButton_Click(object sender, RoutedEventArgs e)
        {
            TextboxDebugHEX.Document.Blocks.Clear();
            TextboxDebugASCII.Document.Blocks.Clear();
        }

        public void QueriesTableAdapter(string connectionString)
        {
            Properties.Settings.Default["defaultConnectionString"] = connectionString + ";password = idsancoprodigy2017";

        }

        #region FILE MENU COMMANDS
        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            FileManager file_manager = new FileManager(AppLocale, AppRole, this);
            var CreateNewFileWindow = new CreateNewFile(file_manager.DBListBox.Items, AppLocale, AppRole, this);
            file_manager.Close();
            CreateNewFileWindow.ShowDialog();
        }
        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var prodigy_configtool_window = new FileManager(AppLocale, AppRole, this);
            prodigy_configtool_window.WindowState = WindowState.Normal;
            prodigy_configtool_window.ShowDialog();
        }
        private async void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (databaseDataSet.HasChanges())
            {
                var controller = await this.ShowProgressAsync(Properties.Resources.Saving, "");
                await Task.Run(() =>
                {
                    Save_Database_data();
                    controller.CloseAsync();
                });
                
            }
        }
        private async void SaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\V" + version_part + "\\"; //My documents folder
            string filename = "MegaXConfig";
            dlg.FileName = filename; // Default file name
            dlg.DefaultExt = ".prgy"; // Default file extension
            //dlg.InitialDirectory = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\database\";
            dlg.InitialDirectory = configurations_folder;
            
            //QueriesTableAdapter("attachdbfilename =" + configurations_folder + ChoosenDbFile + "; data source = " + configurations_folder + ChoosenDbFile);
            dlg.Filter = "Mega-X Config Tool files (.prgy)|*.prgy"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            string old_file_path = configurations_folder + AppDbFile;
            string temp_file_path = configurations_folder + "temp_" + AppDbFile;
            string backup_file_path = configurations_folder + "backup\\" + AppDbFile;
            string new_file_path = dlg.FileName;

            string old_file_name = Path.GetFileName(old_file_path);
            string temp_file_name = Path.GetFileName(temp_file_path);
            string new_file_name = Path.GetFileName(new_file_path);


            // Process save file dialog box results
            if (result == true)
            {
                if (File.Exists(temp_file_path))
                    File.Delete(temp_file_path);

                System.IO.File.Copy(old_file_path, temp_file_path);


                //AppDbFile = Path.GetFileName(temp_file_name);

                if (databaseDataSet.HasChanges())
                {
                    var controller = await this.ShowProgressAsync(Properties.Resources.Saving, "");
                    await Task.Run(() =>
                    {
                        Save_Database_data();
                        controller.CloseAsync();
                    });
                    await DialogManager.ShowMessageAsync(this, Properties.Resources.SaveWithSuccess, "");
                }


                if (System.IO.File.Exists(new_file_path))
                {
                    System.IO.File.Delete(new_file_path);
                    System.IO.File.Move(old_file_path, new_file_path);
                    System.Console.WriteLine("File already exists. Deleting " + new_file_name + " and changing name from  " + old_file_name + " to " + new_file_name);
                }
                else
                {
                    //System.IO.File.Create(new_file_path);
                    System.IO.File.Move(old_file_path, new_file_path);
                    //System.IO.File.Move(old_file_path, new_file_path);
                    System.Console.WriteLine("File doesn't exists. Creating a copy from " + old_file_name + " to " + new_file_name);
                }

                AppDbFile = Path.GetFileName(new_file_path);
                System.Console.WriteLine("AppDBFile is now " + new_file_name);
                ConfigFileName.Content = AppDbFile;

                
                //File.Replace(temp_file_path, old_file_path, backup_file_path);
                if(File.Exists(old_file_path))
                    System.IO.File.Delete(old_file_path);

                System.IO.File.Move(temp_file_path, old_file_path);
                System.IO.File.Delete(temp_file_path);
                System.Console.WriteLine("Deleting " + temp_file_name);
                //Não gravar antigo, mesmo que haja alterações
                //Gravar novo ficheiro com alterações actuais (mesmo que não gravadas)
                //Abrir novo ficheiro de config e fechar o antigo
            }

            System.Console.WriteLine("***SAVE AS***\n" +
                "OLD:  " + old_file_path + "\n" +
                "TEMP: " + temp_file_path + "\n" +
                "NEW:  " + new_file_path);

            //this.ShowProgressAsync(Properties.Resources.SaveWithSuccess, "");

            //MessageBox.Show(Properties.Resources.SaveWithSuccess, "", MessageBoxButton.OK);
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        private void NewFile_Click(object sender, RoutedEventArgs e)
        {
            FileManager file_manager = new FileManager(AppLocale, AppRole, this);
            var CreateNewFileWindow = new CreateNewFile(file_manager.DBListBox.Items, AppLocale, AppRole, this);
            file_manager.Close();
            CreateNewFileWindow.ShowDialog();
        }
        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var prodigy_configtool_window = new FileManager(AppLocale, AppRole, this);
            prodigy_configtool_window.WindowState = WindowState.Normal;
            prodigy_configtool_window.ShowDialog();
        }
   
        //private void SaveAsFile_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog dlg = new SaveFileDialog();
        //    dlg.FileName = "ProdigyConfig"; // Default file name
        //    dlg.DefaultExt = ".prgy"; // Default file extension
        //    dlg.InitialDirectory = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\database\";
        //    dlg.Filter = "Mega-X Config Tool files (.prgy)|*.prgy"; // Filter files by extension

        //    // Show save file dialog box
        //    Nullable<bool> result = dlg.ShowDialog();

        //    // Process save file dialog box results
        //    if (result == true)
        //    {
        //        System.IO.File.Copy(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + AppDbFile,
        //                            dlg.InitialDirectory + dlg.FileName);

        //        //Não gravar antigo, mesmo que haja alterações
        //        //Gravar novo ficheiro com alterações actuais (mesmo que não gravadas)
        //        //Abrir novo ficheiro de config e fechar o antigo
        //    }
        //}
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
        #endregion
        
        public void Save_Database_data()
        {
            #region Zones
            try
            {
                
                
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        //create command
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Zone WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                //databaseDataSet.Zone.AcceptChanges();

                //string update_syntax = "UPDATE Zone SET [Id] = @Id, [Description] = @Description, [Entry time away] = @Entry_time_away, " +
                //"[Entry time stay] = @Entry_time_stay, [Zone active] = @Zone_active, [Keyswitch zone]= @keyswitch_zone, " +
                //"[Keyswitch type]= @keyswitch_type , [Always bypass]= @Always_bypass, [Manual bypass]= @Manual_bypass, " +
                //"[Auto bypass] = @Auto_bypass, [Soak test]= @Soak_test, [Send report]= @Send_report, [Always report]= @Always_report, " +
                //"[Arm if not ready]= @Arm_if_not_ready, [Silent zone]= @Silent_zone, [Handover]= @Handover, [Two trigger]= @Two_trigger, " +
                //"[Trigger Time]= @Trigger_time, [Chime - arm]= @param1, [Chime - disarm]= @param2, [Exit terminator]= @Exit_terminator, " +
                //"[Terminator count]= @Terminator_count, [Sensor watch]= @Sensor_watch, [Sensor watch time]= @Sensor_watch_time, " +
                //"[Keypad visible]= @Keypad_visible, [Area away 1]= @Area_away_1, [Area away 2]= @Area_away_2, [Area away 3]= @Area_away_3, " +
                //"[Area away 4]= @Area_away_4, [Area away 5]= @Area_away_5, [Area away 6]= @Area_away_6, [Area away 7]= @Area_away_1, " +
                //"[Area away 8]= @Area_away_8, [Area stay 1]= @Area_stay_1, [Area stay 2]= @Area_stay_2, [Area stay 3]= @Area_stay_3, " +
                //"[Area stay 4]= @Area_stay_4, [Area stay 5]= @Area_stay_5, [Area stay 6]= @Area_stay_6, [Area stay 7]= @Area_stay_7, " +
                //"[Area stay 8]= @Area_stay_8, [Show keypad 1]= @Show_keypad_1, [Show keypad 2]= @Show_keypad_2, [Show keypad 3]= @Show_keypad_3, " +
                //"[Show keypad 4]= @Show_keypad_4, [Show keypad 5]= @Show_keypad_5, [Show keypad 6] = @Show_keypad_6, [Show keypad 7] = @Show_keypad_7, " +
                //"[Show keypad 8] = @Show_keypad_8, [Terminal circuit type] = @Terminal_circuit_type, [R1 Value] = @R1_Value, " +
                //"[R1 Function] = @R1_Function, [R1 Contact type] = @R1_Contact_type, [R2 Value] = @R2_Value, [R2 Function] = @R2_Function, " +
                //"[R2 Contact type] = @R2_Contact_type, [R3 Value] = @R3_Value, [R3 Function] = @R3_Function, [R3 Contact type] = @R3_Contact_type, " +
                //"[Chime alarm output 1] = @Chime_alarm_output_1, [Chime alarm output 2] = @Chime_alarm_output_2, " +
                //"[Chime alarm output 3] = @Chime_alarm_output_3, [Sensor watch alarm output 1] = @Sensor_watch_alarm_output_1, " +
                //"[Sensor watch alarm output 2] = @Sensor_watch_alarm_output_2, [Sensor watch alarm output 3] =@Sensor_watch_alarm_output_3, " +
                //"[Entry time alarm output 1] = @Entry_time_alarm_output_1, [Entry time alarm output 2] = @Entry_time_alarm_output_2, " +
                //"[Entry time alarm output 3] = @Entry_time_alarm_output_3, [Anti mask alarm output 1] = @Anti_mask_alarm_output_1, " +
                //"[Anti mask alarm output 2] =  @Anti_mask_alarm_output_2, [Anti mask alarm output 3] =  @Anti_mask_alarm_output_3, " +
                //"[24 hour alarm output 1] =  @param7, [24 hour alarm output 2] = @param10, [24 hour alarm output 3] = @param13, " +
                //"[Fire alarm output 1] = @Fire_alarm_output_1, [Fire alarm output 2] = @Fire_alarm_output_2, " +
                //"[Fire alarm output 3] = @Fire_alarm_output_3, [Zone alarm output 1] = @Zone_alarm_output_1, " +
                //"[Zone alarm output 2] = @Zone_alarm_output_2, [Zone alarm output 3] = @Zone_alarm_output_3, " +
                //"[Tamper alarm output 1] = @Tamper_alarm_output_1, [Tamper alarm output 2] = @Tamper_alarm_output_2, " +
                //"[Tamper alarm output 3] = @Tamper_alarm_output_3, [Reserved 1] = @Reserved_1, [Radio zone] = @Radio_zone, " +
                //"[24h zone] = @param16, [24h zone - auto-reset] = @param19, [24h zone - firezone] = @param22, " +
                //"[Send multiple reports to dialer] = @Send_multiple_reports_to_dialer, [Report to account] = @Report_to_account, " +
                //"[Dont report 24h zone] = @Dont_report_24h_zone, [Reserved 2] = @Reserved_2, [Reserved 3] =@Reserved_3, " +
                //"[Temperature normal low] = @Temperature_normal_low, [Temperature normal high] = @Temperature_normal_high, " +
                //"[Temperature alarm low] = @Temperature_alarm_low, [Temperature alarm high] = @Temperature_alarm_high, " +
                //"[Reserved 4] = @Reserved_4, [Report code] = @Report_code, [Reserved 5] = @Reserved_5, " +
                //"[Arm away by timezone - output 1] = @param25, [Arm away by timezone - output 2] = @param28, " +
                //"[Arm away by timezone - output 3] = @param31, [Arm away with code - output 1] = @param34, " +
                //"[Arm away with code - output 2] = @param37, [Arm away with code - output 3] = @param40, " +
                //"[Arm away with command - output 1] = @param43, [Arm away with command - output 2] = @param46, " +
                //"[Arm away with command - output 3] = @param49, [Arm away with keyswtich - output 1] = @param52, " +
                //"[Arm away with keyswtich - output 2] = @param55, [Arm away with keyswtich - output 3] = @param58, " +
                //"[Arm away remotely - output 1] = @param61, [Arm away remotely - output 2] = @param64, " +
                //"[Arm away remotely - output 3] = @param67, [Arm stay by timezone - output 1] = @param70, " +
                //"[Arm stay by timezone - output 2] = @param73, [Arm stay by timezone - output 3] = @param76, " +
                //"[Arm stay with code - output 1] = @param79, [Arm stay with code - output 2] = @param82, " +
                //"[Arm stay with code - output 3] = @param85, [Arm stay with command - output 1] = @param88, " +
                //"[Arm stay with command - output 2] = @param91, [Arm stay with command - output 3] = @param94, " +
                //"[Arm stay with keyswtich - output 1] = @param97, [Arm stay with keyswtich - output 2] = @param100, " +
                //"[Arm stay with keyswtich - output 3] = @param103, [Arm stay remotely - output 1] = @param106, " +
                //"[Arm stay remotely - output 2] = @param109, [Arm stay remotely - output 3] = @param112, " +
                //"[Disarm away by timezone - output 1] = @param115, [Disarm away by timezone - output 2] = @param118, " +
                //"[Disarm away by timezone - output 3] = @param121, [Disarm away with code - output 1] = @param124, " +
                //"[Disarm away with code - output 2] = @param127, [Disarm away with code - output 3] = @param130, " +
                //"[Disarm away with command - output 1] = @param133, [Disarm away with command - output 2] = @param136, " +
                //"[Disarm away with command - output 3] = @param139, [Disarm away with keyswtich - output 1] = @param142, " +
                //"[Disarm away with keyswtich - output 2] = @param145, [Disarm away with keyswtich - output 3] = @param148, " +
                //"[Disarm away remotely - output 1] = @param151, [Disarm away remotely - output 2] = @param154, " +
                //"[Disarm away remotely - output 3] = @param157, [Disarm stay by timezone - output 1] = @param160, " +
                //"[Disarm stay by timezone - output 2] = @param163, [Disarm stay by timezone - output 3] = @param166, " +
                //"[Disarm stay with code - output 1] = @param169, [Disarm stay with code - output 2] = @param172, " +
                //"[Disarm stay with code - output 3] = @param175, [Disarm stay with command - output 1] = @param178, " +
                //"[Disarm stay with command - output 2] = @param181, [Disarm stay with command - output 3] = @param184, " +
                //"[Disarm stay with keyswtich - output 1] = @param187, [Disarm stay with keyswtich - output 2] = @param190, " +
                //"[Disarm stay with keyswtich - output 3] = @param193, [Disarm stay remotely - output 1] = @param196, " +
                //"[Disarm stay remotely - output 2] = @param199, [Disarm stay remotely - output 3] = @param202, " +
                //"[Audio track 1] = @Audio_track_1, [Audio track 2] = @Audio_track_2, [Audio track 3] = @Audio_track_3, " +
                //"[Audio track 4] = @Audio_track_4, [HardwarePosition] = @HardwarePosition";

                //SQLiteConnection sql_con = new SQLiteConnection("Data Source=" + configurations_folder + AppDbFile + ";Password=idsancoprodigy2017");

                //databaseDataSetZoneTableAdapter.Adapter.UpdateCommand = new SQLiteCommand(update_syntax, sql_con);
                
                databaseDataSetZoneTableAdapter.Update(databaseDataSet.Zone);
                //DebugTable(databaseDataSet.Zone);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region AREAS
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        //create command
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Area WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                databaseDataSetAreaTableAdapter.Update(databaseDataSet.Area);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion
            
            #region MAININFO
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM MainInfo WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                databaseDataSetMainInfoTableAdapter.Update(databaseDataSet.MainInfo);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region GLOBAL CONFIG 
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM GlobalSystem WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                databaseDataSetGlobalSystemTableAdapter.Update(databaseDataSet.GlobalSystem);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region DIALER
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Dialer WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                databaseDataSetDialerTableAdapter.Update(databaseDataSet.Dialer);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region PHONE
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Phone WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                databaseDataSetPhoneTableAdapter.Update(databaseDataSet.Phone);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region TIMEZONE
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Timezone WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                TimezoneTableAdapter databaseDataSetTimezoneTableAdapter = new TimezoneTableAdapter();
                //databaseDataSet.Timezone.AcceptChanges();
                databaseDataSetTimezoneTableAdapter.Update(databaseDataSet.Timezone);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region OUTPUT
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Output WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                OutputTableAdapter databaseDataSetOutputTableAdapter = new OutputTableAdapter();
                //databaseDataSet.Output.AcceptChanges();
                databaseDataSetOutputTableAdapter.Update(databaseDataSet.Output);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region KEYPADS
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Keypad WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                KeypadTableAdapter databaseDataSetKeypadTableAdapter = new KeypadTableAdapter();
                //databaseDataSet.Keypad.AcceptChanges();
                databaseDataSetKeypadTableAdapter.Update(databaseDataSet.Keypad);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region EXPANDERS
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Expander WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }
                //ExpanderTableAdapter databaseDataSetExpanderTableAdapter = new ExpanderTableAdapter();
               // databaseDataSet.Expander.AcceptChanges();
                databaseDataSetExpanderTableAdapter.Update(databaseDataSet.Expander);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region USER
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM User WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }
               // UserTableAdapter databaseDataSetUserTableAdapter = new UserTableAdapter();
                //databaseDataSet.User.AcceptChanges();
                databaseDataSetUserTableAdapter.Update(databaseDataSet.User);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region EVENT
            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Event WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }

                }

                //EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
               // databaseDataSet.Event.AcceptChanges();
                databaseDataSetEventTableAdapter.Update(databaseDataSet.Event);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            #region AUDIO_SYSTEM

            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM AudioSystemConfiguration WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                    }
                }

                //AudioSystemConfigurationTableAdapter databaseDatasetTableAdapter = new AudioSystemConfigurationTableAdapter();
                //databaseDataSet.AudioSystemConfiguration.AcceptChanges();
                databaseDataSetAudioSystemConfigurationTableAdapter.Update(databaseDataSet.AudioSystemConfiguration);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }

            #endregion

            #region AUDIO

            try
            {
                if (default_restore_is_set)
                {
                    using (SQLiteConnection sqlConn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\Database\" + AppDbFile + ";Password=idsancoprodigy2017"))
                    {
                        sqlConn.Open();
                        SQLiteCommand sqlCommand = sqlConn.CreateCommand();
                        sqlCommand.CommandText = "DELETE FROM Audio WHERE Id <> 0";
                        sqlCommand.ExecuteNonQuery();
                        sqlConn.Close();
                    }
                }
                //var a = databaseDataSet.Audio.GetChanges();
                AudioTableAdapter databaseDatasetTableAdapter = new AudioTableAdapter();
                System.Windows.Data.CollectionViewSource AudioCustomizedViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioCustomizedViewSource")));

                //databaseDataSet.Audio.AcceptChanges();
                databaseDatasetTableAdapter.Update(databaseDataSet.Audio);
                //AudioCustomizedViewSource.View.MoveCurrentToFirst();
                //AudioTableAdapter databaseDataSetAudioTableAdapter = new AudioTableAdapter();
                //databaseDataSetAudioDefaultTableAdapter.Update(databaseDataSet.Audio);
                //databaseDataSetAudioCustomizedTableAdapter.Update(databaseDataSet.Audio);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            #endregion

            if (default_restore_is_set)
            {
                default_restore_is_set = false;
            }
        }

        //private void RadioLocalePT_Click(object sender, RoutedEventArgs e)
        //{
        //    Preferences_ContextMenu.IsOpen = false;
        //    Close();
        //    MainWindow window1 = new MainWindow("pt-PT", AppRole, AppDbFile, null, null, null, null, null);
        //    window1.Show();
        //}

        //private void RadioLocaleEN_Click(object sender, RoutedEventArgs e)
        //{
        //    Preferences_ContextMenu.IsOpen = false;
        //    Close();
        //    MainWindow window1 = new MainWindow("en-US", AppRole, AppDbFile, null, null, null, null, null);
        //    window1.Show();
        //}

        public async void BaseWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (databaseDataSet.HasChanges() == true)
            {

                var messageBox = MessageBox.Show(Properties.Resources.QuestionSaveChangesExtended + "\n" + "\n" + Properties.Resources.InfoDataWillBeLost, Properties.Resources.QuestionSaveChanges, MessageBoxButton.YesNoCancel);
                if (messageBox == MessageBoxResult.Cancel)
                {
                    e.Cancel = true;
                }
                else if (messageBox == MessageBoxResult.Yes)
                {
                    var controller = await this.ShowProgressAsync(Properties.Resources.Saving, "");
                    await Task.Run(() =>
                    {
                        Save_Database_data();
                        controller.CloseAsync();
                        this.ShowProgressAsync(Properties.Resources.SaveWithSuccess, "");
                    });
                }
            }
            else
            {
                if (serialPort.IsOpen)
                    serialPort.Close();
            }


            //if(helpWindow.IsActive)
            //    Environment.Exit(1);
        }

        private void BaseFileMenu_Initialized(object sender, EventArgs e)
        {
            MenuItemSave.IsEnabled = databaseDataSet.HasChanges();
        }

        private void MenuItem_Checked(object sender, RoutedEventArgs e)
        {
            MenuItemSave.IsEnabled = databaseDataSet.HasChanges();
        }

        private void MenuItem_SubmenuOpened(object sender, RoutedEventArgs e)
        {
            MenuItemSave.IsEnabled = databaseDataSet.HasChanges();
        }

        private void TitleBarHelpButton_Click(object sender, RoutedEventArgs e)
        {
            Help helpWindow = new Help(AppRole);

            if (MainHomeTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/home.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainAreasTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/areas.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainZonesTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/zones.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainKeypadsTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/keypads.png", UriKind.Relative));
                helpWindow.Show();
            }
            //else if (MainExpandersTab.IsSelected)
            //{
            //    helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/expanders.png", UriKind.Relative));
            //    helpWindow.Show();
            //}
            else if (MainOutputsTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/outputs.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainUsersTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/users.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainTimezonesTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/timezones.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainPhonesTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/phones.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainDialerTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/dialer.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainglobalSystemPVTTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/system.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (ClientConfigTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/clientinfo.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (AudioMessagesTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/audios.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (EventsTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/events.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (StatusTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/status.png", UriKind.Relative));
                helpWindow.Show();
            }
            else if (MainUpdateFirmwareTab.IsSelected)
            {
                helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/fwupdate.png", UriKind.Relative));
                helpWindow.Show();
            }
            //else if (MainUpdateExpanderTab.IsSelected)
            //{
            //    helpWindow.manual.Source = new BitmapImage(new Uri(@"/manuals/images/expander.png", UriKind.Relative));
            //    helpWindow.Show();
            //}
        }

        #region *** Event Events and Filters ***

        private async void Reab_Events_Button_click(object sender, RoutedEventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                EventTableAdapter databaseDataSetEventTableAdapter = new EventTableAdapter();
                
                if (event_clicks == 0)
                {
                    //Clear all existing events 
                    databaseDataSet.Event.Clear();
                    databaseDataSetEventTableAdapter.Update(databaseDataSet.Event);
                }

                event_clicks++;

                Event events = new Event();

                var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");
                controller.Maximum = 100.0;
                controller.Minimum = 0.0;

                int events_to_read = 200;
                int i = 0;

                int temp = last_event_number; 

                await Task.Run(() =>
                {
                    while (last_event_number < temp + events_to_read)
                    {
                        controller.SetMessage(Properties.Resources.ReadingEvents);
                        for (i = temp; i <= temp + events_to_read; i++)
                        {
                            if (RX_ACK)
                                this.Dispatcher.Invoke((Action)(() => events.read(this, (uint)i)));
                            else
                            {
                                int count = 0;
                                while (!RX_ACK)
                                {
                                    count++;
                                    if (count == 10)
                                        RX_ACK = true;
                                }
                                if (i == 1)
                                    this.Dispatcher.Invoke((Action)(() => events.read(this, 1)));
                                else
                                    this.Dispatcher.Invoke((Action)(() => events.read(this, (uint)(i - 1))));
                            }
                            if (i == temp + events_to_read)
                            { 
                                last_event_number += events_to_read;
                                
                            }
                            this.Dispatcher.Invoke((Action)(() => controller.SetProgress(i * (float)(100.0 / (float)(temp + events_to_read)))));
                        }
                    }
                    this.Dispatcher.Invoke((Action)(() => eventDataGrid.Items.Refresh()));
                    this.Dispatcher.Invoke((Action)(() => eventDataGrid.Items.SortDescriptions.Add(new SortDescription("EventId", ListSortDirection.Descending))));
                    databaseDataSetEventTableAdapter.Update(databaseDataSet.Event);
                    

                    controller.CloseAsync();
                });

                eventDataGrid.Visibility = Visibility.Visible;

                if(databaseDataSet.Event.Rows.Count - temp == events_to_read)
                    LoadMoreEvents_button.Visibility = Visibility.Visible;

                await DialogManager.ShowMessageAsync(this, Properties.Resources.ReadWithSuccess, "");

            }
            else
            {
                await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
            }

        }

        private void ButtonFilterArmDisarm_Click(object sender, RoutedEventArgs e)
        {
            if (EventFilterArmDisarm.Equals(false))
            {
                EventFilterArmDisarm = true;
                string events_filter = events_filter_constructor();
                databaseDataSet.Event.DefaultView.RowFilter =
                    databaseDataSet.Event.EventIdColumn.ColumnName + " IN(" + events_filter + ")";

                ButtonFilterArmDisarm.Background = Brushes.LightGreen;
            }
            else
            {
                EventFilterArmDisarm = false;
                string events_filter = events_filter_constructor();
                if (events_filter.Equals(String.Empty))
                    databaseDataSet.Event.DefaultView.RowFilter = String.Empty;
                else
                    databaseDataSet.Event.DefaultView.RowFilter = databaseDataSet.Event.EventIdColumn.ColumnName + " IN(" + events_filter + ")";

                ButtonFilterArmDisarm.Background = Brushes.WhiteSmoke;
            }
        }

        private void ButtonFilterAlarms_Click(object sender, RoutedEventArgs e)
        {

            if (EventFilterAlarms.Equals(false))
            {
                EventFilterAlarms = true;
                string events_filter = events_filter_constructor();

                databaseDataSet.Event.DefaultView.RowFilter =
                   databaseDataSet.Event.EventIdColumn.ColumnName + " IN(" + events_filter + ")";

                ButtonFilterAlarms.Background = Brushes.LightGreen;
            }
            else
            {
                EventFilterAlarms = false;
                string events_filter = events_filter_constructor();
                if (events_filter.Equals(String.Empty))
                    databaseDataSet.Event.DefaultView.RowFilter = String.Empty;
                else
                    databaseDataSet.Event.DefaultView.RowFilter = databaseDataSet.Event.EventIdColumn.ColumnName + " IN(" + events_filter + ")";

                ButtonFilterAlarms.Background = Brushes.WhiteSmoke;
            }
        }

        private void ButtonFilterFaults_Click(object sender, RoutedEventArgs e)
        {
            if (EventFilterFaults.Equals(false))
            {
                EventFilterFaults = true;
                string events_filter = events_filter_constructor();
                databaseDataSet.Event.DefaultView.RowFilter =
                    databaseDataSet.Event.EventIdColumn.ColumnName + " IN(" + events_filter + ")";

                EventFilterFaults = true;
                ButtonFilterFaults.Background = Brushes.LightGreen;
            }
            else
            {
                EventFilterFaults = false;
                string events_filter = events_filter_constructor();
                if (events_filter.Equals(String.Empty))
                    databaseDataSet.Event.DefaultView.RowFilter = String.Empty;
                else
                    databaseDataSet.Event.DefaultView.RowFilter = databaseDataSet.Event.EventIdColumn.ColumnName + " IN(" + events_filter + ")";

                ButtonFilterFaults.Background = Brushes.WhiteSmoke;

            }

        }

        private string events_filter_constructor()
        {
            string return_filter = String.Empty;
            if (EventFilterFaults.Equals(true))
            {
                return_filter += Constants.KP_EVT_CODE_PUMP_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_SYSTEM_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_AC_LOSS.ToString() + ","
                    + Constants.KP_EVT_CODE_LOW_SYSTEM_BATTERY.ToString() + ","
                    + Constants.KP_EVT_CODE_RAM_CHECKSUM_BAD.ToString() + ","
                    + Constants.KP_EVT_CODE_ROM_CHECKSUM_BAD.ToString() + ","
                    + Constants.KP_EVT_CODE_SYSTEM_RESET.ToString() + ","
                    + Constants.KP_EVT_CODE_PANEL_PROGRAMMING_CHANGED.ToString() + ","
                    + Constants.KP_EVT_CODE_SELF_TEST_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_SYSTEM_SHUTDOWN.ToString() + ","
                    + Constants.KP_EVT_CODE_BATTERY_TEST_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_GROUND_FAULT.ToString() + ","
                    + Constants.KP_EVT_CODE_BATTERY_MISSING_DEAD.ToString() + ","
                    + Constants.KP_EVT_CODE_POWER_SUPPLY_OVERCURRENT.ToString() + ","
                    + Constants.KP_EVT_CODE_ENGINEER_RESET.ToString() + ","
                    + Constants.KP_EVT_CODE_PRIMARY_POWER_SUPPLY_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_SYSTEM_TAMPER.ToString() + ","
                    + Constants.KP_EVT_CODE_SOUNDER_RELAY_.ToString() + ","
                    + Constants.KP_EVT_CODE_BELL_1_.ToString() + ","
                    + Constants.KP_EVT_CODE_BELL_2_.ToString() + ","
                    + Constants.KP_EVT_CODE_ALARM_RELAY_.ToString() + ","
                    + Constants.KP_EVT_CODE_TROUBLE_RELAY.ToString() + ","
                    + Constants.KP_EVT_CODE_REVERSING_RELAY_.ToString() + ","
                    + Constants.KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_3.ToString() + ","
                    + Constants.KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_4.ToString() + ","
                    + Constants.KP_EVT_CODE_SYSTEM_PERIPHERAL_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_POLLING_LOOP_OPEN_DISARMED.ToString() + ","
                    + Constants.KP_EVT_CODE_POLLING_LOOP_SHORT_DISARMED.ToString() + ","
                    + Constants.KP_EVT_CODE_EXPANSION_MODULE_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_REPEATER_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_LOCAL_PRINTER_OUT_OF_PAPER.ToString() + ","
                    + Constants.KP_EVT_CODE_LOCAL_PRINTER_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_EXP_MODULE_DC_LOSS.ToString() + ","
                    + Constants.KP_EVT_CODE_EXP_MODULE_LOW_BATT.ToString() + ","
                    + Constants.KP_EVT_CODE_EXP_MODULE_RESET.ToString() + ","
                    + Constants.KP_EVT_CODE_EXP_MODULE_TAMPER_.ToString() + ","
                    + Constants.KP_EVT_CODE_EXP_MODULE_AC_LOSS.ToString() + ","
                    + Constants.KP_EVT_CODE_EXP_MODULE_SELF_TEST_FAIL.ToString() + ","
                    + Constants.KP_EVT_CODE_RF_RECEIVER_JAM_DETECT.ToString() + ","
                    + Constants.KP_EVT_CODE_AES_ENCRYPTION_DIS_ENABLED.ToString() + ","
                    + Constants.KP_EVT_CODE_COMMUNICATION_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_TELCO_1_FAULT.ToString() + ","
                    + Constants.KP_EVT_CODE_TELCO_2_FAULT.ToString() + ","
                    + Constants.KP_EVT_CODE_LONG_RANGE_RADIO_XMITTER_FAULT.ToString() + ","
                    + Constants.KP_EVT_CODE_FAILURE_TO_COMMUNICATE_EVENT.ToString() + ","
                    + Constants.KP_EVT_CODE_LOSS_OF_RADIO_SUPERVISION.ToString() + ","
                    + Constants.KP_EVT_CODE_LOSS_OF_CENTRAL_POLLING.ToString() + ","
                    + Constants.KP_EVT_CODE_LONG_RANGE_RADIO_VSWR_PROBLEM.ToString() + ","
                    + Constants.KP_EVT_CODE_PERIODIC_COMM_TEST_FAIL_RESTORE.ToString() + ","
                    + Constants.KP_EVT_CODE_PROTECTION_LOOP.ToString() + ","
                    + Constants.KP_EVT_CODE_PROTECTION_LOOP_OPEN.ToString() + ","
                    + Constants.KP_EVT_CODE_PROTECTION_LOOP_SHORT.ToString() + ","
                    + Constants.KP_EVT_CODE_FIRE_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_EXIT_ERROR_ALARM_ZONE.ToString() + ","
                    + Constants.KP_EVT_CODE_PANIC_ZONE_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_HOLD_UP.ToString() + ","
                    + Constants.KP_EVT_CODE_SWINGER_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_CROSS.ToString() + ","
                    + Constants.KP_EVT_CODE_SENSOR_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_LOSS_OF_SUPERVISION_RF.ToString() + ","
                    + Constants.KP_EVT_CODE_LOSS_OF_SUPERVISION_RPM.ToString() + ","
                    + Constants.KP_EVT_CODE_SENSOR_TAMPER_FAULT.ToString() + ","
                    + Constants.KP_EVT_CODE_RF_LOW_BATTERY.ToString() + ","
                    + Constants.KP_EVT_CODE_SMOKE_DETECTOR_HI_SENSITIVITY.ToString() + ","
                    + Constants.KP_EVT_CODE_SMOKE_DETECTOR_LOW_SENSITIVITY.ToString() + ","
                    + Constants.KP_EVT_CODE_INTRUSION_DETECTOR_HI_SENSITIVITY.ToString() + ","
                    + Constants.KP_EVT_CODE_INTRUSION_DETECTOR_LOW_SENSITIVITY.ToString() + ","
                    + Constants.KP_EVT_CODE_SENSOR_SELF_TEST_FAILURE.ToString() + ","
                    + Constants.KP_EVT_CODE_SENSOR_WATCH_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_DRIFT_COMPENSATION_ERROR.ToString() + ","
                    + Constants.KP_EVT_CODE_MAINTENANCE_ALERT.ToString() + ","
                    + Constants.KP_EVT_CODE_CO_DETECTOR_NEEDS_REPLACEMENT.ToString();
            }

            if (EventFilterAlarms.Equals(true))
            {
                if (EventFilterFaults.Equals(true))
                {
                    return_filter += ",";
                }
                return_filter += Constants.KP_EVT_CODE_MEDICAL.ToString() + ","
                   + Constants.KP_EVT_CODE_PERSONAL_EMERGENCY.ToString() + ","
                   + Constants.KP_EVT_CODE_FIRE.ToString() + ","
                   + Constants.KP_EVT_CODE_SMOKE.ToString() + ","
                   + Constants.KP_EVT_CODE_COMBUSTION.ToString() + ","
                   + Constants.KP_EVT_CODE_WATER_FLOW.ToString() + ","
                   + Constants.KP_EVT_CODE_HEAT.ToString() + ","
                   + Constants.KP_EVT_CODE_PULL_STATION.ToString() + ","
                   + Constants.KP_EVT_CODE_DUCT.ToString() + ","
                   + Constants.KP_EVT_CODE_FLAME.ToString() + ","
                   + Constants.KP_EVT_CODE_NEAR_ALARM_FIRE.ToString() + ","
                   + Constants.KP_EVT_CODE_PANIC.ToString() + ","
                   + Constants.KP_EVT_CODE_DURESS.ToString() + ","
                   + Constants.KP_EVT_CODE_SILENT.ToString() + ","
                   + Constants.KP_EVT_CODE_AUDIBLE.ToString() + ","
                   + Constants.KP_EVT_CODE_BURGLARY.ToString() + ","
                   + Constants.KP_EVT_CODE_PERIMETER.ToString() + ","
                   + Constants.KP_EVT_CODE_INTERIOR.ToString() + ","
                   + Constants.KP_EVT_CODE_24_HOUR_SAFE.ToString() + ","
                   + Constants.KP_EVT_CODE_ENTRY_EXIT.ToString() + ","
                   + Constants.KP_EVT_CODE_DAY_NIGHT.ToString() + ","
                   + Constants.KP_EVT_CODE_OUTDOOR.ToString() + ","
                   + Constants.KP_EVT_CODE_TAMPER.ToString() + ","
                   + Constants.KP_EVT_CODE_NEAR_ALARM_WORSENS.ToString() + ","
                   + Constants.KP_EVT_CODE_INTRUSION_VERIFIER.ToString() + ","
                   + Constants.KP_EVT_CODE_GENERAL_ALARM.ToString() + ","
                   + Constants.KP_EVT_CODE_POLLING_LOOP_OPEN_ARMED.ToString() + ","
                   + Constants.KP_EVT_CODE_POLLING_LOOP_SHORT_ARMED.ToString() + ","
                   + Constants.KP_EVT_CODE_EXPANSION_MODULE_FAILURE_ARMED.ToString() + ","
                   + Constants.KP_EVT_CODE_SENSOR_TAMPER.ToString() + ","
                   + Constants.KP_EVT_CODE_EXPANSION_MODULE_TAMPER.ToString() + ","
                   + Constants.KP_EVT_CODE_SILENT_BURGLARY.ToString() + ","
                   + Constants.KP_EVT_CODE_SENSOR_SUPERVISION_FAILURE.ToString() + ","
                   + Constants.KP_EVT_CODE_24_HOUR_NON_BURGLARY.ToString() + ","
                   + Constants.KP_EVT_CODE_GAS_DETECTED.ToString() + ","
                   + Constants.KP_EVT_CODE_REFRIGERATION.ToString() + ","
                   + Constants.KP_EVT_CODE_LOSS_OF_HEAT.ToString() + ","
                   + Constants.KP_EVT_CODE_WATER_LEAKAGE.ToString() + ","
                   + Constants.KP_EVT_CODE_FOIL_BREAK.ToString() + ","
                   + Constants.KP_EVT_CODE_DAY_TROUBLE.ToString() + ","
                   + Constants.KP_EVT_CODE_LOW_BOTTLED_GAS_LEVEL.ToString() + ","
                   + Constants.KP_EVT_CODE_HIGH_TEMP.ToString() + ","
                   + Constants.KP_EVT_CODE_LOW_TEMP.ToString() + ","
                   + Constants.KP_EVT_CODE_LOSS_OF_AIR_FLOW.ToString() + ","
                   + Constants.KP_EVT_CODE_CARBON_MONOXIDE_DETECTED.ToString() + ","
                   + Constants.KP_EVT_CODE_TANK_LEVEL.ToString() + ","
                   + Constants.KP_EVT_CODE_HIGH_HUMIDITY.ToString() + ","
                   + Constants.KP_EVT_CODE_LOW_HUMIDITY.ToString() + ","
                   + Constants.KP_EVT_CODE_FIRE_SUPERVISORY.ToString() + ","
                   + Constants.KP_EVT_CODE_LOW_WATER_PRESSURE.ToString() + ","
                   + Constants.KP_EVT_CODE_LOW_CO2.ToString() + ","
                   + Constants.KP_EVT_CODE_GATE_VALVE_SENSOR.ToString() + ","
                   + Constants.KP_EVT_CODE_LOW_WATER_LEVEL.ToString() + ","
                   + Constants.KP_EVT_CODE_PUMP_ACTIVATED.ToString();
            }

            if (EventFilterArmDisarm.Equals(true))
            {
                if (EventFilterFaults.Equals(true) || EventFilterAlarms.Equals(true))
                {
                    return_filter += ",";
                }
                return_filter += Constants.KP_EVT_CODE_OPEN_CLOSE.ToString() + ","
                    + Constants.KP_EVT_CODE_OC_BY_USER.ToString() + ","
                    + Constants.KP_EVT_CODE_GROUP_OC_GROUPA_GROUP_OF.ToString() + ","
                    + Constants.KP_EVT_CODE_AUTOMATIC_OC.ToString() + ","
                    + Constants.KP_EVT_CODE_LATE_TO_OC.ToString() + ","
                    + Constants.KP_EVT_CODE_DEFERRED_OC.ToString() + ","
                    + Constants.KP_EVT_CODE_CANCEL.ToString() + ","
                    + Constants.KP_EVT_CODE_REMOTE_ARM_DISARM.ToString() + ","
                    + Constants.KP_EVT_CODE_QUICK_ARM.ToString() + ","
                    + Constants.KP_EVT_CODE_KEYSWITCH_OC.ToString() + ","
                    + Constants.KP_EVT_CODE_ARMED_STAY.ToString() + ","
                    + Constants.KP_EVT_CODE_KEYSWITCH_ARMED_STAY.ToString() + ","
                    + Constants.KP_EVT_CODE_ARMED_WITH_SYSTEM_TROUBLE_OVERRIDE.ToString() + ","
                    + Constants.KP_EVT_CODE_EXCEPTION_OC_.ToString() + ","
                    + Constants.KP_EVT_CODE_EARLY_OC.ToString() + ","
                    + Constants.KP_EVT_CODE_LATE_OC.ToString() + ","
                    + Constants.KP_EVT_CODE_FAILED_TO_OPEN.ToString() + ","
                    + Constants.KP_EVT_CODE_FAILED_TO_CLOSE.ToString() + ","
                    + Constants.KP_EVT_CODE_AUTO_ARM_FAILED.ToString() + ","
                    + Constants.KP_EVT_CODE_PARTIAL_ARM.ToString() + ","
                    + Constants.KP_EVT_CODE_EXIT_ERROR.ToString() + ","
                    + Constants.KP_EVT_CODE_USER_ON_PREMISES.ToString() + ","
                    + Constants.KP_EVT_CODE_RECENT_CLOSE.ToString() + ","
                    + Constants.KP_EVT_CODE_WRONG_CODE_ENTRY.ToString() + ","
                    + Constants.KP_EVT_CODE_LEGAL_CODE_ENTRY.ToString() + ","
                    + Constants.KP_EVT_CODE_RE_ARM_AFTER_ALARM.ToString() + ","
                    + Constants.KP_EVT_CODE_AUTO_ARM_TIME_EXTENDED.ToString() + ","
                    + Constants.KP_EVT_CODE_PANIC_ALARM_RESET.ToString() + ","
                    + Constants.KP_EVT_CODE_SERVICE_ON_OFF_PREMISES.ToString() + ","
                    + Constants.KP_EVT_CODE_CALLBACK_REQUEST_MADE.ToString() + ","
                    + Constants.KP_EVT_CODE_SUCCESSFUL_DOWNLOAD_ACCESS.ToString() + ","
                    + Constants.KP_EVT_CODE_UNSUCCESSFUL_ACCESS.ToString() + ","
                    + Constants.KP_EVT_CODE_SYSTEM_SHUTDOWN_COMMAND_RECEIVED.ToString() + ","
                    + Constants.KP_EVT_CODE_DIALER_SHUTDOWN_COMMAND_RECEIVED.ToString() + ","
                    + Constants.KP_EVT_CODE_SUCCESSFUL_UPLOAD.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_DENIED.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_REPORT_BY_USER.ToString() + ","
                    + Constants.KP_EVT_CODE_FORCED_ACCESS_.ToString() + ","
                    + Constants.KP_EVT_CODE_EGRESS_DENIED.ToString() + ","
                    + Constants.KP_EVT_CODE_EGRESS_GRANTED.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_DOOR_PROPPED_OPEN.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_POINT_DOOR_STATUS_MONITOR_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_POINT_REQUEST_TO_EXIT_TROUBLE.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_PROGRAM_MODE_ENTRY.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_PROGRAM_MODE_EXIT.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_THREAT_LEVEL_CHANGE.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_RELAY_TRIGGER_FAIL.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_RTE_SHUNT.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_DSM_SHUNT.ToString() + ","
                    + Constants.KP_EVT_CODE_SECOND_PERSON_ACCESS.ToString() + ","
                    + Constants.KP_EVT_CODE_IRREGULAR_ACCESS.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_READER_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_SOUNDER_RELAY_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_BELL_1_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_BELL_2_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_ALARM_RELAY_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_TROUBLE_RELAY_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_REVERSING_RELAY_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_3_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_NOTIFICATION_APPLIANCE_CKT_4_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_MODULE_ADDED.ToString() + ","
                    + Constants.KP_EVT_CODE_MODULE_REMOVED.ToString() + ","
                    + Constants.KP_EVT_CODE_DIALER_DISABLED.ToString() + ","
                    + Constants.KP_EVT_CODE_RADIO_TRANSMITTER_DISABLED.ToString() + ","
                    + Constants.KP_EVT_CODE_REMOTE_UPDOWN_LOAD_DISABLE.ToString() + ","
                    + Constants.KP_EVT_CODE_ZONE_SENSOR_BYPASS.ToString() + ","
                    + Constants.KP_EVT_CODE_FIRE_BYPASS.ToString() + ","
                    + Constants.KP_EVT_CODE_24_HOUR.ToString() + ","
                    + Constants.KP_EVT_CODE_BURG_BYPASS.ToString() + ","
                    + Constants.KP_EVT_CODE_GROUP_BYPASS_USER_A_GROUP_OF.ToString() + ","
                    + Constants.KP_EVT_CODE_SWINGER_BYPASS.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS.ToString() + ","
                    + Constants.KP_EVT_CODE_ACCESS_POINT_BYPASS.ToString();
            }

            if (EventFilterAreas.Equals(true))
            {
                if (EventFilterAreas.Equals(true))
                {
                    return_filter += ",";
                }
                return_filter +=
                     Constants.PARTITION_1.ToString() + ","
                   + Constants.PARTITION_2.ToString() + ","
                   + Constants.PARTITION_3.ToString() + ","
                   + Constants.PARTITION_4.ToString() + ","
                   + Constants.PARTITION_5.ToString() + ","
                   + Constants.PARTITION_6.ToString() + ","
                   + Constants.PARTITION_7.ToString() + ","
                   + Constants.PARTITION_8.ToString();
            }

            return return_filter;
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            string clearfilter = string.Empty;
            databaseDataSet.Event.DefaultView.RowFilter = clearfilter;

            PartitionComboBox.SelectedIndex = 0;
            //EventTimeComboBox.SelectedIndex = 0;

            EventFilterArmDisarm = false;
            ButtonFilterArmDisarm.Background = Brushes.WhiteSmoke;
            EventFilterAlarms = false;
            ButtonFilterAlarms.Background = Brushes.WhiteSmoke;
            EventFilterFaults = false;
            ButtonFilterFaults.Background = Brushes.WhiteSmoke;
        }

        private void SelectRangeDate_Click(object sender, RoutedEventArgs e)
        {
            if (Event_DateFrom.SelectedDate != null || Event_DateTo.SelectedDate != null)
            {
                DateTime date_from = Event_DateFrom.SelectedDate.Value.Date;
                DateTime date_to = Event_DateTo.SelectedDate.Value.Date;

                DateTime datetime_from = new DateTime(date_from.Year, date_from.Month, date_from.Day, 00, 00, 00);
                DateTime datetime_to = new DateTime(date_to.Year, date_to.Month, date_to.Day, 23, 59, 59);

                string filter_date = databaseDataSet.Event.DateTimeColumn.ColumnName.ToString() + " >= #" + datetime_from + "# AND "
                                         + databaseDataSet.Event.DateTimeColumn.ColumnName.ToString() + " <= #" + datetime_to + "#";

                databaseDataSet.Event.DefaultView.RowFilter = filter_date;

                System.Diagnostics.Debug.WriteLine("FILTER: " + filter_date);
            }
        }

        private void CheckAllEvents_Click(object sender, RoutedEventArgs e)
        {
            General protocol = new General();
            Event events = new Event();

            string columnName = databaseDataSet.Event.Keypad_ackColumn.ColumnName;

            foreach (DataRow row in databaseDataSet.Event.Rows)
            {
                row[columnName] = 1;
            }

            int event_cnt = eventDataGrid.Items.Count;

            protocol.check_all_events(this);

            //for (int i = 1; i < event_cnt; i++) //Constants.KP_MAX_ZONES
            //{
            //    events.Write(this, (uint)i);

            //}
            //eventDataGrid.Items.Clear();
            //eventDataGrid.DataContext = databaseDataSet.Tables["Event"];

            eventDataGrid.Items.Refresh();
        }

        #endregion

        private void RestoreDefaultMenuItem()
        {
            var messageBox = MessageBox.Show(Properties.Resources.QuestionRestoreDefaultsExtended, "", MessageBoxButton.YesNo);
            if (messageBox == MessageBoxResult.Yes)
            {
                #region ZONES
                defaultDataSet.ZoneDataTable zone_table = new defaultDataSet.ZoneDataTable();
                SQLiteConnection con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                SQLiteCommand cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Zone");
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(zone_table);
                con.Close();
                databaseDataSet.Zone.Clear();

                //delete table
                foreach (defaultDataSet.ZoneRow row in (databaseDataSet.Zone.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.ZoneRow row in zone_table)
                {
                    databaseDataSet.Zone.Rows.Add(row.ItemArray);
                }
                #endregion

                #region AREAS
                defaultDataSet.AreaDataTable area_table = new defaultDataSet.AreaDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Area");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(area_table);
                con.Close();
                databaseDataSet.Area.Clear();

                //delete table
                foreach (defaultDataSet.AreaRow row in (databaseDataSet.Area.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AreaRow row in area_table)
                {
                    databaseDataSet.Area.Rows.Add(row.ItemArray);
                }

                #endregion

                #region USERS
                defaultDataSet.UserDataTable user_table = new defaultDataSet.UserDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM User");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(user_table);
                con.Close();
                databaseDataSet.User.Clear();

                //delete table
                foreach (defaultDataSet.UserRow row in (databaseDataSet.User.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.UserRow row in user_table)
                {
                    databaseDataSet.User.Rows.Add(row.ItemArray);
                }
                #endregion

                #region KEYPADS
                defaultDataSet.KeypadDataTable keypad_table = new defaultDataSet.KeypadDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Keypad");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(keypad_table);
                con.Close();
                databaseDataSet.Keypad.Clear();

                //delete table
                foreach (defaultDataSet.KeypadRow row in (databaseDataSet.Keypad.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.KeypadRow row in keypad_table)
                {
                    databaseDataSet.Keypad.Rows.Add(row.ItemArray);
                }
                #endregion

                #region EXPANDERS
                defaultDataSet.ExpanderDataTable expander_table = new defaultDataSet.ExpanderDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Expander");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(expander_table);
                con.Close();
                databaseDataSet.Expander.Clear();

                //delete table
                foreach (defaultDataSet.ExpanderRow row in (databaseDataSet.Expander.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.ExpanderRow row in expander_table)
                {
                    databaseDataSet.Expander.Rows.Add(row.ItemArray);
                }
                #endregion

                #region OUTPUTS
                defaultDataSet.OutputDataTable output_table = new defaultDataSet.OutputDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Output");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(output_table);
                con.Close();
                databaseDataSet.Output.Clear();

                //delete table
                foreach (defaultDataSet.OutputRow row in (databaseDataSet.Output.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.OutputRow row in output_table)
                {
                    databaseDataSet.Output.Rows.Add(row.ItemArray);
                }
                #endregion

                #region TIMEZONES
                defaultDataSet.TimezoneDataTable timezone_table = new defaultDataSet.TimezoneDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Timezone");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(timezone_table);
                con.Close();
                databaseDataSet.Timezone.Clear();

                //delete table
                foreach (defaultDataSet.TimezoneRow row in (databaseDataSet.Timezone.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.TimezoneRow row in timezone_table)
                {
                    databaseDataSet.Timezone.Rows.Add(row.ItemArray);
                }
                #endregion

                #region PHONES
                defaultDataSet.PhoneDataTable phone_table = new defaultDataSet.PhoneDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Phone");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(phone_table);
                con.Close();
                databaseDataSet.Phone.Clear();

                //delete table
                foreach (defaultDataSet.PhoneRow row in (databaseDataSet.Phone.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.PhoneRow row in phone_table)
                {
                    databaseDataSet.Phone.Rows.Add(row.ItemArray);
                }
                #endregion

                #region DIALER
                defaultDataSet.DialerDataTable dialer_table = new defaultDataSet.DialerDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Dialer");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(dialer_table);
                con.Close();
                databaseDataSet.Dialer.Clear();

                //delete table
                foreach (defaultDataSet.DialerRow row in (databaseDataSet.Dialer.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.DialerRow row in dialer_table)
                {
                    databaseDataSet.Dialer.Rows.Add(row.ItemArray);
                }

                #endregion

                #region GLOBAL SYSTEM
                defaultDataSet.GlobalSystemDataTable system_table = new defaultDataSet.GlobalSystemDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM GlobalSystem");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(system_table);
                con.Close();
                databaseDataSet.GlobalSystem.Clear();

                //delete table
                foreach (defaultDataSet.GlobalSystemRow row in (databaseDataSet.GlobalSystem.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.GlobalSystemRow row in system_table)
                {
                    databaseDataSet.GlobalSystem.Rows.Add(row.ItemArray);
                }

                #endregion

                #region MAIN INFO
                defaultDataSet.MainInfoDataTable main_info_table = new defaultDataSet.MainInfoDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM MainInfo");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(main_info_table);
                con.Close();
                databaseDataSet.MainInfo.Clear();

                //delete table
                foreach (defaultDataSet.MainInfoRow row in (databaseDataSet.MainInfo.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.MainInfoRow row in main_info_table)
                {
                    databaseDataSet.MainInfo.Rows.Add(row.ItemArray);
                }

                #endregion

                #region EVENTS
                defaultDataSet.EventDataTable event_table = new defaultDataSet.EventDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Event");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(event_table);
                con.Close();
                databaseDataSet.Event.Clear();

                //delete table
                foreach (defaultDataSet.EventRow row in (databaseDataSet.Event.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.EventRow row in event_table)
                {
                    databaseDataSet.Event.Rows.Add(row.ItemArray);
                }
                #endregion

                #region AUDIO
                defaultDataSet.AudioDataTable audio_table = new defaultDataSet.AudioDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Audio WHERE Type = 0");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(audio_table);
                con.Close();
                databaseDataSet.Audio.Clear();

                //delete table
                foreach (defaultDataSet.AudioRow row in (databaseDataSet.Audio.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AudioRow row in audio_table)
                {
                    databaseDataSet.Audio.Rows.Add(row.ItemArray);
                }
                #endregion

                #region AUDIO CUSTOMIZED
                defaultDataSet.AudioDataTable audio_customized_table = new defaultDataSet.AudioDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM Audio WHERE Type = 1");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(audio_customized_table);
                con.Close();
                databaseDataSet.Audio.Clear();

                DataTable audio_full_table = new DataTable();
                audio_full_table = audio_table.Copy();
                audio_full_table.Merge(audio_customized_table);


                AudioTableAdapter databaseDatasetTableAdapter = new AudioTableAdapter();
                databaseDatasetTableAdapter.Fill(audio_table);
                databaseDatasetTableAdapter.Update(databaseDataSet.Audio);

                //delete table
                foreach (defaultDataSet.AudioRow row in (databaseDataSet.Audio.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AudioRow row in audio_customized_table)
                {
                    databaseDataSet.Audio.Rows.Add(row.ItemArray);
                }


                #endregion

                #region AUDIO SYSTEM CONFIG
                defaultDataSet.AudioSystemConfigurationDataTable audio_system_table = new defaultDataSet.AudioSystemConfigurationDataTable();
                con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                con.Open();
                cmd = con.CreateCommand();
                cmd.CommandText = string.Format("SELECT * FROM AudioSystemConfiguration");
                adapter = new SQLiteDataAdapter(cmd);
                builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(audio_system_table);
                con.Close();
                databaseDataSet.AudioSystemConfiguration.Clear();

                //delete table
                foreach (defaultDataSet.AudioSystemConfigurationRow row in (databaseDataSet.Audio.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AudioSystemConfigurationRow row in audio_system_table)
                {
                    databaseDataSet.AudioSystemConfiguration.Rows.Add(row.ItemArray);
                }
                #endregion

                default_restore_is_set = true;
            }
            else if (messageBox == MessageBoxResult.No)
            {
                return;
            }
        }

        private void TitleBarLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var messageBox = MessageBox.Show(Properties.Resources.QuestionLogoutExtended, "", MessageBoxButton.YesNo);
            if (messageBox == MessageBoxResult.Yes)
            {
                AppLogin login_window = new AppLogin();
                this.Close();
                login_window.Show();
            }
            else if (messageBox == MessageBoxResult.No)
            {
                return;
            }
        }

        private static void Update_table_after_edit(object sender, DataRowChangeEventArgs e)
        {
            {
                try
                {
                    AudioTableAdapter databaseDataSetAudioTableAdapter = new AudioTableAdapter();
                    databaseDataSetAudioTableAdapter.Update((defaultDataSet.AudioDataTable)e.Row.Table);
                    //((defaultDataSet.AudioDataTable)e.Row.Table).AcceptChanges();
                    //databaseDataSetAudioTableAdapter.Fill((defaultDataSet.AudioDataTable)e.Row.Table);
                    ZoneTableAdapter databaseDataSetZoneTableAdapter = new ZoneTableAdapter();
                    databaseDataSetZoneTableAdapter.Update((defaultDataSet.ZoneDataTable)e.Row.Table);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                }
            }
        }

        #region DOWNLOAD/UPLOAD TILES
        private async void isDownloadORUploadClick(Boolean isUpload, string structure)
        {
            if (this.serialPort.IsOpen)
            {
                DataChoose data_choose = new DataChoose(this, isUpload, structure, AppRole);
                data_choose.Show();
                //msgflag = Constants.MSG_TX_READY;
                this.IsEnabled = false;
            }
            else
            {
                await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
            }
        }

        private void DownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, String.Empty);
        }
        private void UploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, String.Empty);
        }

        private void ZonesDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "zones");
        }
        private void ZonesUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "zones");
        }

        private void AreasDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "areas");
        }
        private void AreasUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "areas");
        }

        private void KeypadsDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "keypads");
        }
        private void KeypadsUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "keypads");
        }

        private void ExpanderDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "expanders");
        }
        private void ExpanderUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "expanders");
        }

        private void OutputsDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "outputs");
        }
        private void OutputsUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "outputs");
        }

        private void UsersDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "users");
        }
        private void UsersUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "users");
        }

        private void TimezonesDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "timezones");
        }
        private void TimezonesUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "timezones");
        }

        private void PhonesDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "phones");
        }
        private void PhonesUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "phones");
        }

        private void DialerDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "dialer");
        }
        private void DialerUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "dialer");
        }

        private void SystemDownloadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(false, "system");
        }
        private void SystemUploadTile_Click(object sender, RoutedEventArgs e)
        {
            isDownloadORUploadClick(true, "system");
        }
        #endregion

        #region AUDIO

        void waveSource_DataAvailable(object sender, WaveInEventArgs e)
        {
            if (waveFile != null)
            {
                waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                waveFile.Flush();
            }
        }

        void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
        {
            DataRowView row = (DataRowView)AudioCustomizedDataGrid.CurrentItem;

            waveFile.Close();//close writter

            WaveFileReader temporary_file = new WaveFileReader(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + row["Description"] + ".wav");
            byte[] audio_wav_file_bytes = new byte[(int)(8000 * ((double)temporary_file.TotalTime.TotalSeconds))];

            temporary_file.Read(audio_wav_file_bytes, 0, audio_wav_file_bytes.Length);


            using (audio_stream = new FileStream(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + row["Description"] + ".raw", FileMode.Create))
            {



                audio_stream.Write(audio_wav_file_bytes, 0, audio_wav_file_bytes.Length); // Requires System.IO
                                                                                          ////fs.Close();
                                                                                          //WaveFormat waveFormat = new WaveFormat(8000, 8, 1); // Same format.
                                                                                          //RawSourceWaveStream rawSource = new NAudio.Wave.RawSourceWaveStream(fs, waveFormat);
                                                                                          //WaveOut waveOut = new WaveOut();
                                                                                          //waveOut.Init(rawSource);
                audio_stream.Close();                                                               //waveOut.Play();
                temporary_file.Close();

                File.Delete((System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + row["Description"] + ".wav"));

            }

            if (waveSource != null)
            {
                waveSource.Dispose();
                waveSource = null;
            }

            if (waveFile != null)
            {
                waveFile.Dispose();
                waveFile = null;
            }
        }

        private void Load_WAV_File_Click(object sender, RoutedEventArgs e)
        {
            GetDataGridRows(AudioCustomizedDataGrid);
            defaultDataSet.AudioDataTable AudioDT = new defaultDataSet.AudioDataTable();
            databaseDataSetAudioCustomizedTableAdapter.Fill(AudioDT);
            DebugTable(AudioDT);

            
            
            // Create OpenFileDialog 
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            // Set filter for file extension and default file extension 
            dlg.DefaultExt = ".wav";
            string path_audio = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";


            DataRowView row = (DataRowView)AudioCustomizedDataGrid.Items.CurrentItem;
            current_audio_row = row;

            if (dlg.ShowDialog() == true)
            {
                string[] selectedFiles = dlg.SafeFileNames;
                string[] filePaths = dlg.FileNames;

                if(row["Description"] == System.DBNull.Value)
                    row["Description"] = (dlg.SafeFileName).Substring(0, dlg.SafeFileName.Length - 4);

                for (int i = 0; i < dlg.FileNames.Count(); i++)
                {
                    WaveFormat target = new WaveFormat(8000, 8, 1);
                    WaveStream stream = new WaveFileReader(filePaths[i]);
                    
                    WaveFormatConversionStream str = new WaveFormatConversionStream(target, stream);
                    System.Diagnostics.Debug.WriteLine("New Customized Audio: " + path_audio + row["Description"].ToString() + "_8bit.wav");
                    WaveFileWriter.CreateWaveFile(path_audio + row["Description"].ToString() + "_8bit.wav", str); //or the path of .wav file

                    WaveFileReader temporary_file = new WaveFileReader(path_audio + row["Description"] + "_8bit.wav");
                    byte[] audio_wav_file_bytes = new byte[(int)(8000 * ((double)temporary_file.TotalTime.TotalSeconds))];

                    temporary_file.Read(audio_wav_file_bytes, 0, audio_wav_file_bytes.Length);

                    using (audio_stream = new FileStream(path_audio + row["Description"].ToString() + ".raw", FileMode.Create))
                    {
                        audio_stream.Write(audio_wav_file_bytes, 0, audio_wav_file_bytes.Length); // Requires System.IO

                        temporary_file.Close();
                        File.Delete(path_audio + row["Description"].ToString() + "_8bit.wav");
                        ;
                    }

                }
                var row_list = GetDataGridRows(AudioCustomizedDataGrid);
                foreach (DataGridRow single_row in row_list)
                {
                    Button load_wav_button = single_row.FindChild<Button>("buttonLoadWav");
                    Button start_record_button = single_row.FindChild<Button>("buttonRecordWav");
                    Button stop_record_button = single_row.FindChild<Button>("buttonStopRecordWav");
                    Button delete_button = single_row.FindChild<Button>("buttonDeleteRecordWav");

                    Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                    if (single_row.IsSelected == true)
                    {
                        load_wav_button.IsEnabled = false;
                        start_record_button.IsEnabled = false;
                        stop_record_button.IsEnabled = false;
                        delete_button.IsEnabled = true;

                        play_button.IsEnabled = true;
                    }

                    row["FilePath"] = path_audio + row["Description"].ToString() + ".raw";

                    if (row["Filepath"] != System.DBNull.Value)
                        delete_button.IsEnabled = true;

                    AudioCustomizedDataGrid.UpdateLayout();
                }
                

            }

        }

        private void buttonRecordWav_Click(object sender, RoutedEventArgs e)
        {
            int waveInDevices = WaveIn.DeviceCount;
            if (waveInDevices != 0)
            {
                string path_audio = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";

                DataRowView row = (DataRowView)AudioCustomizedDataGrid.CurrentItem;

                waveSource = new WaveIn();
                waveSource.WaveFormat = new WaveFormat(8000, 8, 1);

                waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                waveFile = new WaveFileWriter((path_audio + row["Description"] + ".wav"), waveSource.WaveFormat);

                var row_list = GetDataGridRows(AudioCustomizedDataGrid);
                foreach (DataGridRow single_row in row_list)
                {
                    Button load_wav_button = single_row.FindChild<Button>("buttonLoadWav");
                    Button start_record_button = single_row.FindChild<Button>("buttonRecordWav");
                    Button stop_record_button = single_row.FindChild<Button>("buttonStopRecordWav");
                    Button delete_button = single_row.FindChild<Button>("buttonDeleteRecordWav");

                    Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                    if (single_row.IsSelected == true)
                    {
                        load_wav_button.IsEnabled = false;
                        start_record_button.IsEnabled = false;
                        stop_record_button.IsEnabled = true;
                        delete_button.IsEnabled = false;

                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = false;
                        play_stop.IsEnabled = false;

                        start_record_button.Visibility = Visibility.Collapsed;
                        stop_record_button.Visibility = Visibility.Visible;
                        
                    }
                }
                waveSource.StartRecording();
                row["FilePath"] = path_audio + row["Description"].ToString() + ".raw";

                AudioCustomizedDataGrid.UpdateLayout();
            }
            else
            {
                MessageBox.Show(Properties.Resources.PleaseConnectMicroFirst, "", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void buttonStopRecordWav_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioCustomizedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                Button load_wav_button = single_row.FindChild<Button>("buttonLoadWav");
                Button start_record_button = single_row.FindChild<Button>("buttonRecordWav");
                Button stop_record_button = single_row.FindChild<Button>("buttonStopRecordWav");
                Button delete_button = single_row.FindChild<Button>("buttonDeleteRecordWav");

                Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                if (single_row.IsSelected == true)
                {
                    load_wav_button.IsEnabled = false;
                    start_record_button.IsEnabled = false;
                    stop_record_button.IsEnabled = false;
                    delete_button.IsEnabled = true;

                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;

                    start_record_button.Visibility = Visibility.Visible;
                    stop_record_button.Visibility = Visibility.Collapsed;
                    AudioCustomizedDataGrid.UpdateLayout();
                }
            }
            waveSource.StopRecording();
        }

        private void Update_audio_comboboxes_list(object sender, EventArgs e)
        {
            ComboBox a = (ComboBox)sender;
            a.ItemsSource = Dictionaries.GetAudioMessages();
        }

        void wavePlayer_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            Button play_button = new Button();
            Button play_pause = new Button();
            Button play_stop = new Button();

            set_button_UI_for_costumized_audio();

            var row_list = GetDataGridRows(AudioReservedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                play_stop = single_row.FindChild<Button>("ButtonPauseReserved");

                play_button.IsEnabled = true;
                play_pause.IsEnabled = false;
                play_stop.IsEnabled = false;

                AudioCustomizedDataGrid.UpdateLayout();
            }

            if (waveOut != null)
            {
                waveOut.Dispose();
                waveOut = new WaveOut();
                waveOut.PlaybackStopped += wavePlayer_PlaybackStopped;
            }
        }

        
        private async void Send_Audio_messages(object sender, RoutedEventArgs e)
        {
            if (this.serialPort.IsOpen)
            {
                if (audio_stream != null)
                    audio_stream.Close();

                Protocol.Audio audio = new Protocol.Audio();
                // Make a new unified file
                audio.CreateUnifiedFile(this,AudioReservedDataGrid, AudioCustomizedDataGrid);

                // Write to Mega-X Unit
                var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");
                controller.Maximum = 100.0;
                controller.Minimum = 0.0;

                await Task.Run(() =>
                {
                    int msg_size = 235;
                    int bytes_to_send = 0;
                    int bytes_aux_msg = 0;
                    int bytes_aux_block = 0;
                    int msg_complete = 0;
                    int msg_counter = 0;
                    int block_complete = 0;
                    int block_incomplete = 0;
                    int block_counter = 0;
                    int block_counter_aux = 0;
                    int max_block_size = 4096;
                    int block_size = 0;
                    uint address = 0x200320;
                    uint block_address = 0x200320;
                    int bytes_left = 0;
                    int max_msg = 0;
                    int delayTime = 20;
                    int savingTime = 200;
                    int max_blocks = 0;
                    int error_counter = 0;

                    string path_audio = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";
                    string filename = AppDbFile.Substring(0, AppDbFile.Length - 5);
                    var file = new FileInfo(path_audio + "unified_" + filename + ".mai");

                    byte[] audio_file_bytes = File.ReadAllBytes(file.ToString());
                    System.Diagnostics.Debug.WriteLine("READ ALL BYTES from " + file.ToString());

                    max_blocks = (audio_file_bytes.Length / max_block_size) + 1;

                    //audio_number_of_msgs = (audio_file_bytes.Length / msg_size) + 1; // + 1

                    //int error_counter = 0;
                    //int fragment_message_id = 0;

                    controller.SetMessage("A escrever faixas de audio");

                    for (bytes_to_send = 0, bytes_aux_msg = 0, bytes_aux_block = 0; bytes_to_send < audio_file_bytes.Length; bytes_to_send++, bytes_aux_msg++, bytes_aux_block++)
                    {
                        msg_size = 235;
                        byte[] fragment_data_bytes = new byte[msg_size];


                        if (block_counter < 1)
                        {
                            block_size = max_block_size - (int)Constants.KP_FLASH_TAMANHO_DADOS_AUDIO_CONFIG_FLASH;
                            block_address = 0x200320;
                        }
                        else
                        {
                            block_size = max_block_size;
                            block_address = 0x200000 + (uint)(block_counter * block_size);
                        }

                        block_counter_aux = block_counter;

                        //if (block_complete == 1)
                        //{
                        //    if (!RX_ACK)
                        //    {
                        //        System.Diagnostics.Debug.WriteLine("ERROR: Cant save BLOCK #" + "{0}. Trying again...", block_counter);
                        //       // if (block_counter > 1)
                        //       //     this.Dispatcher.Invoke((Action)(() => audio.write_full_block(this, (uint)block_address, (uint)block_size)));
                        //       // else
                        //        this.Dispatcher.Invoke((Action)(() => audio.write_block(this, (uint)block_address, (uint)block_size)));
                        //        System.Threading.Thread.Sleep(savingTime);
                        //    }
                        //    else
                        //    {
                        //        block_complete = 0;
                        //    }
                        //}

                        //MSG COMPLETE
                        if (bytes_aux_msg == msg_size)
                        {
                            msg_complete = 1;
                            bytes_aux_msg = 0;
                        }

                        if (msg_complete == 1)
                        {
                            msg_complete = 0;
                            msg_counter++;

                            if (!RX_ACK)
                            {
                                if (bytes_to_send < msg_size)
                                {
                                    bytes_to_send = 0;
                                    bytes_aux_msg = 0;
                                    bytes_aux_block = 0;

                                    fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();
                                    System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);
                                    this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                    System.Threading.Thread.Sleep(delayTime);
                                }
                                else
                                {
                                    msg_counter--;
                                    bytes_to_send -= msg_size;
                                    bytes_aux_block -= msg_size;

                                    fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();
                                    System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);
                                    this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                    System.Threading.Thread.Sleep(delayTime);
                                }
                            }
                            else
                            {
                                if (bytes_to_send == msg_size)
                                {
                                    fragment_data_bytes = (audio_file_bytes.Skip(0).Take(msg_size)).ToArray();
                                }
                                else if (audio_file_bytes.Length - bytes_to_send < msg_size)
                                {
                                    msg_size = audio_file_bytes.Length - bytes_to_send;
                                    fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();
                                    block_complete = 1;
                                }
                                else
                                    fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();

                                System.Diagnostics.Debug.WriteLine("* MSG #" + msg_counter + " | BLOCK ADDRESS: 0x{0:X}  |  0x{1:X} - 0x{2:X} ", block_address, address, address + msg_size);
                                this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(delayTime);

                                if (!RX_ACK)
                                {
                                    fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send - msg_size).Take(msg_size)).ToArray();
                                    System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);
                                    this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                    System.Threading.Thread.Sleep(delayTime);
                                }
                                address += (uint)msg_size;
                            }
                        }

                        //BLOCK WITH BYTES LEFT - BLOCK INCOMPLETE    
                        max_msg = block_size / msg_size;
                        bytes_left = block_size - (max_msg * msg_size);

                        if (bytes_aux_block == block_size - bytes_left && msg_complete == 0)//bytes_aux_msg == bytes_left && bytes_aux_block == block_size)
                        {
                            block_incomplete = 1;
                            bytes_aux_msg = 0;
                        }

                        if (block_incomplete == 1)
                        {
                            block_incomplete = 0;
                            msg_counter++;

                            if (!RX_ACK)
                            {
                                error_counter++;
                                msg_counter--;
                                bytes_to_send -= msg_size * error_counter;
                                bytes_aux_block -= msg_size * error_counter;
                                address -= (uint)(msg_size * error_counter);

                                fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();
                                System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + (msg_counter - error_counter) + "       ({0})", bytes_to_send);
                                this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(delayTime);
                            }
                            else
                            {
                                error_counter = 0;
                                msg_size = bytes_left;

                                fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();
                                System.Diagnostics.Debug.WriteLine("* MSG #" + msg_counter + " | BLOCK ADDRESS: 0x{0:X}  |  0x{1:X} - 0x{2:X} *", block_address, address, address + msg_size);
                                this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(delayTime);


                            }
                        }

                        //BLOCK COMPLETE
                        if (bytes_aux_block == block_size) //(bytes_aux_block + bytes_left == block_size)
                        {
                            block_complete = 1;

                            if (!RX_ACK)
                            {
                                //address -= (uint)bytes_left;

                                fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send - bytes_left).Take(bytes_left)).ToArray();
                                System.Diagnostics.Debug.WriteLine("* MSG #" + msg_counter + " | BLOCK ADDRESS: 0x{0:X}  |  0x{1:X} - 0x{2:X} *", block_address, address, address + bytes_left);
                                this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)bytes_left)));
                                System.Threading.Thread.Sleep(delayTime);
                            }

                        }
                        if (block_complete == 1)
                        {
                            block_complete = 0;
                            if (!RX_ACK)
                            {
                                bytes_to_send -= msg_size;
                                bytes_aux_block -= msg_size;
                                block_incomplete = 1;

                                fragment_data_bytes = (audio_file_bytes.Skip(bytes_to_send).Take(msg_size)).ToArray();
                                System.Diagnostics.Debug.WriteLine("ERROR: Resending MSG #" + msg_counter + "       ({0})", bytes_to_send);
                                this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)address, (uint)msg_size)));
                                System.Threading.Thread.Sleep(delayTime);
                            }
                            else
                            {


                                System.Diagnostics.Debug.WriteLine("######## SAVING BLOCK #" + block_counter + " @ 0x{0:X} ########", block_address);
                                //if (block_counter > 1)
                                //    this.Dispatcher.Invoke((Action)(() => audio.write_full_block(this, (uint)block_address, (uint)msg_size)));
                                //else
                                this.Dispatcher.Invoke((Action)(() => audio.write_block(this, (uint)block_address, (uint)block_size)));
                                System.Threading.Thread.Sleep(savingTime);

                                block_counter++;

                                bytes_aux_msg = 0;
                                bytes_aux_block = 0;
                                msg_counter = 0;

                                address += (uint)bytes_left;
                                msg_size = 235;
                            }
                        }


                        //if (!RX_ACK)
                        //{
                        //    bytes_to_send--;
                        //    bytes_aux_msg--;
                        //    bytes_aux_block--;
                        //}

                        this.Dispatcher.Invoke((Action)(() => controller.SetProgress(bytes_to_send * (100.0 / audio_file_bytes.Length))));
                    }

                    //for (fragment_message_id = 0; fragment_message_id < (audio_number_of_msgs); fragment_message_id++)
                    //{                        
                    //    if (block_complete == 1)
                    //        fragment_message_id--;

                    //    byte[] fragment_data_bytes = new byte[msg_size];
                    //    if (fragment_message_id == (audio_number_of_msgs - 1))
                    //    {
                    //        fragment_data_bytes = (audio_file_bytes.Skip((0 + (fragment_message_id * msg_size))).Take(audio_file_bytes.Length - (msg_size * fragment_message_id) - 1)).ToArray();
                    //    }
                    //    else
                    //    {
                    //        fragment_data_bytes = (audio_file_bytes.Skip((0 + (fragment_message_id * msg_size))).Take((msg_size + (fragment_message_id * msg_size) - (0 + (fragment_message_id * msg_size))))).ToArray();
                    //    }

                    //    if (RX_ACK)
                    //    {
                    //        error_counter = 0;
                    //        this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)fragment_message_id)));
                    //    }
                    //    else
                    //    {
                    //        error_counter++;
                    //        counter_blocks--;
                    //        if (fragment_message_id == 0)
                    //            fragment_message_id = 0;
                    //        else fragment_message_id--;

                    //        fragment_data_bytes = (audio_file_bytes.Skip((0 + (fragment_message_id * msg_size))).Take((msg_size + (fragment_message_id * msg_size) - (0 + (fragment_message_id * msg_size))))).ToArray();
                    //        if (fragment_message_id == 0)
                    //            this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, 0)));
                    //        else
                    //        { 
                    //            this.Dispatcher.Invoke((Action)(() => audio.write(this, fragment_data_bytes, (uint)(fragment_message_id - error_counter))));
                    //            //fragment_message_id--;
                    //        }
                    //}

                    //    this.Dispatcher.Invoke((Action)(() => controller.SetProgress(fragment_message_id * (100.0 / (audio_number_of_msgs)))));
                    //    System.Threading.Thread.Sleep(10);
                    //}
                    controller.CloseAsync();
                    //MessageBox.Show(Properties.Resources.WriteWithSuccess, "", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve 
                });

                await DialogManager.ShowMessageAsync(this, Properties.Resources.WriteWithSuccess, "");
            }
            else
            {
                await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
                //MessageBox.Show("Please select a connection first!", "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
        }

        private void AudioReservedDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeComponent();

            try
            {
                var row_list = GetDataGridRows(AudioReservedDataGrid);

                foreach (DataGridRow single_row in row_list)
                {
                    DataRowView row_view = single_row.Item as DataRowView;

                    Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");

                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;

                }
                AudioReservedDataGrid.UpdateLayout();
            }
            catch
            {

            }
        }

        private void AudioReservedPlayButton_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioReservedDataGrid);

            DataRowView row = (DataRowView)AudioReservedDataGrid.CurrentItem;
            current_audio_row = row;
            var file = new FileInfo(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + row["FilePath"].ToString());

            audio_stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);

            WaveFormat waveFormat = new WaveFormat(8000, 8, 1); // Same format.
            RawSourceWaveStream rawSource = new NAudio.Wave.RawSourceWaveStream(audio_stream, waveFormat);

            waveOut.Init(rawSource);

            if (waveOut.PlaybackState == PlaybackState.Stopped)
            {
                foreach (DataGridRow single_row in row_list)
                {
                    DataRowView row_view = single_row.Item as DataRowView;

                    Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");


                    if (single_row.IsSelected == true)
                    {
                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = true;
                        play_stop.IsEnabled = true;

                        AudioReservedDataGrid.UpdateLayout();
                    }
                    waveOut.Play();
                }
            }
            else if (((DataRowView)AudioReservedDataGrid.CurrentItem).Equals(current_audio_row))
            {
                foreach (DataGridRow single_row in row_list)
                {
                    DataRowView row_view = single_row.Item as DataRowView;
                    if (single_row.IsSelected == true)
                    {
                        Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                        Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                        Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");

                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = true;
                        play_stop.IsEnabled = true;

                        AudioReservedDataGrid.UpdateLayout();
                    }
                }
                waveOut.Resume();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = new WaveOut();
                waveOut.PlaybackStopped += wavePlayer_PlaybackStopped;

                foreach (DataGridRow single_row in row_list)
                {
                    Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");

                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;

                    AudioReservedDataGrid.UpdateLayout();
                }
                foreach (DataGridRow single_row in row_list)
                {
                    if (single_row.IsSelected == true)
                    {
                        Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                        Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                        Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");

                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = true;
                        play_stop.IsEnabled = true;

                        AudioReservedDataGrid.UpdateLayout();
                    }
                }
                waveOut.Play();
            }
        }

        private void AudioReservedStopButton_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioReservedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");


                if (single_row.IsSelected == true)
                {
                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;

                    AudioReservedDataGrid.UpdateLayout();
                }
            }
            waveOut.Stop();
        }

        private void AudioReservedPauseButton_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioReservedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                Button play_button = single_row.FindChild<Button>("ButtonPlayReserved");
                Button play_pause = single_row.FindChild<Button>("ButtonPauseReserved");
                Button play_stop = single_row.FindChild<Button>("ButtonStopReserved");

                if (single_row.IsSelected == true)
                {
                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = true;

                    AudioReservedDataGrid.UpdateLayout();
                }
            }
            waveOut.Pause();
        }

        private void AudioCustomizedPlayButton_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioCustomizedDataGrid);

            if (waveOut.PlaybackState.Equals(PlaybackState.Stopped))
            {
                foreach (DataGridRow single_row in row_list)
                {
                    Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                    if (single_row.IsSelected == true)
                    {
                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = true;
                        play_stop.IsEnabled = true;

                        AudioCustomizedDataGrid.UpdateLayout();
                    }
                }

                DataRowView row = (DataRowView)AudioCustomizedDataGrid.CurrentItem;
                current_audio_row = row;
                string path_audio = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";
                var file = new FileInfo(path_audio + row["Description"].ToString() + ".raw");

                row["FilePath"] = file.ToString();

                audio_stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);

                WaveFormat waveFormat = new WaveFormat(8000, 8, 1); // Same format.
                RawSourceWaveStream rawSource = new NAudio.Wave.RawSourceWaveStream(audio_stream, waveFormat);
                waveOut.Init(rawSource);
                waveOut.Play();

            }
            else if (((DataRowView)AudioCustomizedDataGrid.CurrentItem).Equals(current_audio_row))
            {
                foreach (DataGridRow single_row in row_list)
                {
                    Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                    if (single_row.IsSelected == true)
                    {
                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = true;
                        play_stop.IsEnabled = true;

                        AudioCustomizedDataGrid.UpdateLayout();
                    }
                }
                waveOut.Resume();
            }
            else
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = new WaveOut();
                waveOut.PlaybackStopped += wavePlayer_PlaybackStopped;

                foreach (DataGridRow single_row in row_list)
                {
                    Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;

                    AudioCustomizedDataGrid.UpdateLayout();
                }

                foreach (DataGridRow single_row in row_list)
                {
                    Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                    Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                    Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                    if (single_row.IsSelected == true)
                    {
                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = true;
                        play_stop.IsEnabled = true;

                        AudioCustomizedDataGrid.UpdateLayout();
                    }
                }

                DataRowView row = (DataRowView)AudioCustomizedDataGrid.CurrentItem;
                current_audio_row = row;
                var file = new FileInfo(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\audio\" + row["FilePath"].ToString());

                audio_stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);

                WaveFormat waveFormat = new WaveFormat(8000, 8, 1); // Same format.
                RawSourceWaveStream rawSource = new NAudio.Wave.RawSourceWaveStream(audio_stream, waveFormat);
                waveOut.Init(rawSource);
                waveOut.Play();
            }
        }

        private void AudioCustomizedStopButton_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioCustomizedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                if (single_row.IsSelected == true)
                {
                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;

                    AudioCustomizedDataGrid.UpdateLayout();
                }
            }
            waveOut.Stop();
        }

        private void AudioCustomizedPauseButton_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioCustomizedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                if (single_row.IsSelected == true)
                {
                    play_button.IsEnabled = true;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = true;

                    AudioCustomizedDataGrid.UpdateLayout();
                }
            }
            waveOut.Pause();
        }

        private void buttonDeleteRecordWav_Click(object sender, RoutedEventArgs e)
        {
            var row_list = GetDataGridRows(AudioCustomizedDataGrid);
            foreach (DataGridRow single_row in row_list)
            {
                Button load_wav_button = single_row.FindChild<Button>("buttonLoadWav");
                Button start_record_button = single_row.FindChild<Button>("buttonRecordWav");
                Button stop_record_button = single_row.FindChild<Button>("buttonStopRecordWav");
                Button delete_button = single_row.FindChild<Button>("buttonDeleteRecordWav");

                Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                if (single_row.IsSelected == true)
                {
                    load_wav_button.IsEnabled = true;
                    start_record_button.IsEnabled = true;
                    stop_record_button.IsEnabled = false;
                    delete_button.IsEnabled = false;

                    play_button.IsEnabled = false;
                    play_pause.IsEnabled = false;
                    play_stop.IsEnabled = false;
                }
            }
            DataRowView row = (DataRowView)AudioCustomizedDataGrid.CurrentItem;
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();
            string path_audio = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";
            if(row["Description"] != System.DBNull.Value)
                if (File.Exists(path_audio + row["Description"].ToString() + ".raw"))
                    File.Delete((path_audio + row["Description"].ToString() + ".raw"));
            row.Delete();
            
            AudioCustomizedDataGrid.UpdateLayout();
        }

        private void AudioCustomizedDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            set_button_UI_for_costumized_audio();
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            //Get all needed information from Form
            var applogin = new AppLogin();
            int role = AppRole;

            //sanitize locale
            string locale = AppLocale;

            var password_change_window = new PasswordChange(locale, role, this);
            password_change_window.Show();

        }

        private void set_button_UI_for_costumized_audio()
        {
            InitializeComponent();
            try
            {
                defaultDataSet.AudioDataTable AudioDT = new defaultDataSet.AudioDataTable();
                databaseDataSetAudioCustomizedTableAdapter.Fill(AudioDT);
                DebugTable(AudioDT);

                var row_list = GetDataGridRows(AudioCustomizedDataGrid);

                foreach (DataGridRow single_row in row_list)
                {
                    DataRowView row_view = single_row.Item as DataRowView;


                    string audiofolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\audio\\";
                    Debug.WriteLine(audiofolder + row_view["Description"] + ".raw");

                    if (File.Exists(audiofolder + row_view["Description"] + ".raw") || row_view["FilePath"] == System.DBNull.Value)
                    {
                        Button load_wav_button = single_row.FindChild<Button>("buttonLoadWav");
                        Button start_record_button = single_row.FindChild<Button>("buttonRecordWav");
                        Button stop_record_button = single_row.FindChild<Button>("buttonStopRecordWav");
                        Button delete_button = single_row.FindChild<Button>("buttonDeleteRecordWav");

                        Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                        Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                        Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                        load_wav_button.IsEnabled = false;
                        start_record_button.IsEnabled = false;
                        stop_record_button.IsEnabled = false;
                        delete_button.IsEnabled = true;

                        play_button.IsEnabled = true;
                        play_pause.IsEnabled = false;
                        play_stop.IsEnabled = false;

                        AudioCustomizedDataGrid.UpdateLayout();
                    }
                    else
                    {
                        Button load_wav_button = single_row.FindChild<Button>("buttonLoadWav");
                        Button start_record_button = single_row.FindChild<Button>("buttonRecordWav");
                        Button stop_record_button = single_row.FindChild<Button>("buttonStopRecordWav");
                        Button delete_button = single_row.FindChild<Button>("buttonDeleteRecordWav");

                        Button play_button = single_row.FindChild<Button>("ButtonPlayCustomized");
                        Button play_pause = single_row.FindChild<Button>("ButtonPauseCustomized");
                        Button play_stop = single_row.FindChild<Button>("ButtonStopCustomized");

                        load_wav_button.IsEnabled = true;
                        start_record_button.IsEnabled = true;
                        stop_record_button.IsEnabled = false;
                        delete_button.IsEnabled = false;

                        play_button.IsEnabled = false;
                        play_pause.IsEnabled = false;
                        play_stop.IsEnabled = false;


                        AudioCustomizedDataGrid.UpdateLayout();
                    }


                }
            }
            catch
            {

            }
        }

        private void AudioCustomizedDataGrid_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            set_button_UI_for_costumized_audio();
        }

        private void AudioCustomizedDataGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {

            DataRowView a = (DataRowView)e.Row.Item;

            databaseDataSet.Audio.TypeColumn.DefaultValue = 1;

            if (a.IsNew)
            {
                set_button_UI_for_costumized_audio();
            }
            else
            {
                var datag = (DataGrid)sender;
                var p = (DataRowView)datag.SelectedValue;
                var p1 = (DataRowView)e.Row.Item;

                defaultDataSet.AudioRow qw = p.Row as defaultDataSet.AudioRow;
                defaultDataSet.AudioRow qw1 = p1.Row as defaultDataSet.AudioRow;
                Debug.WriteLine(qw.Description);
                Debug.WriteLine(qw1.Description);

                qw1.EndEdit();
                Debug.WriteLine(qw1.Description);


            }

        }

        private async void AudioSystemConfigurationUpload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.serialPort.IsOpen)
                {
                    DataChoose data_choose = new DataChoose(this, true, "audio_system_configuration", AppRole);
                    data_choose.Show();
                    this.IsEnabled = false;
                }
                else
                {
                    await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void AudioSystemConfigurationDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.serialPort.IsOpen)
                {
                    DataChoose data_choose = new DataChoose(this, false, "audio_system_configuration", AppRole);
                    data_choose.Show();
                    this.IsEnabled = false;
                }
                else
                {
                    await DialogManager.ShowMessageAsync(this, Properties.Resources.PleaseConnectFirst, "");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region STATUS
        private void StatusPartitionsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Status_Partitions.Visibility.Equals(Visibility.Visible))
            {
                Status_Partitions.Visibility = Visibility.Collapsed;
                Status_Partitions_plus.Visibility = Visibility.Visible;
                Status_Partitions_minus.Visibility = Visibility.Collapsed;
            }
            else
            {
                Status_Partitions.Visibility = Visibility.Visible;
                Status_Partitions_plus.Visibility = Visibility.Collapsed;
                Status_Partitions_minus.Visibility = Visibility.Visible;
            }


        }
        private void StatusZonesButton_Click(object sender, RoutedEventArgs e)
        {
            if (Status_Zones.Visibility.Equals(Visibility.Visible))
            {
                Status_Zones.Visibility = Visibility.Collapsed;
                Status_Zones_plus.Visibility = Visibility.Visible;
                Status_Zones_minus.Visibility = Visibility.Collapsed;
            }
            else
            {
                Status_Zones.Visibility = Visibility.Visible;
                Status_Zones_plus.Visibility = Visibility.Collapsed;
                Status_Zones_minus.Visibility = Visibility.Visible;
            }
        }
        private void StatusOutputsButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusOutputsGrid.Visibility.Equals(Visibility.Visible))
            {
                StatusOutputsGrid.Visibility = Visibility.Collapsed;
                Status_Outputs_plus.Visibility = Visibility.Visible;
                Status_Outputs_minus.Visibility = Visibility.Collapsed;
            }
            else
            {
                StatusOutputsGrid.Visibility = Visibility.Visible;
                Status_Outputs_plus.Visibility = Visibility.Collapsed;
                Status_Outputs_minus.Visibility = Visibility.Visible;
            }
        }
        private void StatusDialerButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusDialerGrid.Visibility.Equals(Visibility.Visible))
            {
                StatusDialerGrid.Visibility = Visibility.Collapsed;
                Status_Dialer_plus.Visibility = Visibility.Visible;
                Status_Dialer_minus.Visibility = Visibility.Collapsed;
            }
            else
            {
                StatusDialerGrid.Visibility = Visibility.Visible;
                Status_Dialer_plus.Visibility = Visibility.Collapsed;
                Status_Dialer_minus.Visibility = Visibility.Visible;
            }
        }
        private void StatusTimezonesButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusTimezonesGrid.Visibility.Equals(Visibility.Visible))
            {
                StatusTimezonesGrid.Visibility = Visibility.Collapsed;
                Status_Timezones_plus.Visibility = Visibility.Visible;
                Status_Timezones_minus.Visibility = Visibility.Collapsed;
            }
            else
            {
                StatusTimezonesGrid.Visibility = Visibility.Visible;
                Status_Timezones_plus.Visibility = Visibility.Collapsed;
                Status_Timezones_minus.Visibility = Visibility.Visible;
            }
        }

        public void UpdateRealTimeView(byte[] buf, int size)
        {
            #region PARTITIONS
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_1], StatusPartition1, Constants.PARTITION_1);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_2], StatusPartition2, Constants.PARTITION_2);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_3], StatusPartition3, Constants.PARTITION_3);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_4], StatusPartition4, Constants.PARTITION_4);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_5], StatusPartition5, Constants.PARTITION_5);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_6], StatusPartition6, Constants.PARTITION_6);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_7], StatusPartition7, Constants.PARTITION_7);
            UpdatePartitionsRealTimeObjects(buf[Constants.PARTITION_8], StatusPartition8, Constants.PARTITION_8);
            #endregion
            #region ZONES
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_1], StatusZone1);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_2], StatusZone2);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_3], StatusZone3);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_4], StatusZone4);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_5], StatusZone5);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_6], StatusZone6);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_7], StatusZone7);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_8], StatusZone8);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_9], StatusZone9);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_10], StatusZone10);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_11], StatusZone11);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_12], StatusZone12);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_13], StatusZone13);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_14], StatusZone14);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_15], StatusZone15);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_16], StatusZone16);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_17], StatusZone17);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_18], StatusZone18);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_19], StatusZone19);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_20], StatusZone20);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_21], StatusZone21);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_22], StatusZone22);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_23], StatusZone23);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_24], StatusZone24);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_25], StatusZone25);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_26], StatusZone26);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_27], StatusZone27);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_28], StatusZone28);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_29], StatusZone29);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_30], StatusZone30);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_31], StatusZone31);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_32], StatusZone32);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_33], StatusZone33);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_34], StatusZone34);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_35], StatusZone35);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_36], StatusZone36);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_37], StatusZone37);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_38], StatusZone38);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_39], StatusZone39);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_40], StatusZone40);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_41], StatusZone41);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_42], StatusZone42);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_43], StatusZone43);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_44], StatusZone44);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_45], StatusZone45);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_46], StatusZone46);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_47], StatusZone47);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_48], StatusZone48);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_49], StatusZone49);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_50], StatusZone50);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_51], StatusZone51);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_52], StatusZone52);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_53], StatusZone53);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_54], StatusZone54);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_55], StatusZone55);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_56], StatusZone56);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_57], StatusZone57);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_58], StatusZone58);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_59], StatusZone59);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_60], StatusZone60);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_61], StatusZone61);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_62], StatusZone62);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_63], StatusZone63);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_64], StatusZone64);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_65], StatusZone65);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_66], StatusZone66);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_67], StatusZone67);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_68], StatusZone68);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_69], StatusZone69);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_70], StatusZone70);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_71], StatusZone71);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_72], StatusZone72);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_73], StatusZone73);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_74], StatusZone74);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_75], StatusZone75);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_76], StatusZone76);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_77], StatusZone77);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_78], StatusZone78);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_79], StatusZone79);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_80], StatusZone80);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_81], StatusZone81);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_82], StatusZone82);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_83], StatusZone83);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_84], StatusZone84);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_85], StatusZone85);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_86], StatusZone86);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_87], StatusZone87);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_88], StatusZone88);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_89], StatusZone89);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_90], StatusZone90);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_91], StatusZone91);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_92], StatusZone92);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_93], StatusZone93);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_94], StatusZone94);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_95], StatusZone95);
            UpdateZonesRealTimeObjects(buf[Constants.ZONA_96], StatusZone96);
            #endregion
            #region OUTPUTS
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_1], StatusOutput1);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_2], StatusOutput2);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_3], StatusOutput3);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_4], StatusOutput4);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_5], StatusOutput5);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_6], StatusOutput6);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_7], StatusOutput7);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_8], StatusOutput8);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_9], StatusOutput9);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_10], StatusOutput10);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_11], StatusOutput11);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_12], StatusOutput12);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_13], StatusOutput13);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_14], StatusOutput14);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_15], StatusOutput15);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_16], StatusOutput16);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_17], StatusOutput17);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_18], StatusOutput18);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_19], StatusOutput19);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_20], StatusOutput20);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_21], StatusOutput21);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_22], StatusOutput22);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_23], StatusOutput23);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_24], StatusOutput24);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_25], StatusOutput25);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_26], StatusOutput26);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_27], StatusOutput27);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_28], StatusOutput28);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_29], StatusOutput29);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_30], StatusOutput30);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_31], StatusOutput31);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_32], StatusOutput32);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_33], StatusOutput33);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_34], StatusOutput34);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_35], StatusOutput35);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_36], StatusOutput36);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_37], StatusOutput37);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_38], StatusOutput38);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_39], StatusOutput39);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_40], StatusOutput40);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_41], StatusOutput41);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_42], StatusOutput42);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_43], StatusOutput43);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_44], StatusOutput44);
            UpdateOutputsRealTimeObjects(buf[Constants.OUTPUT_45], StatusOutput45);
            #endregion
            #region DIALER
            byte dialer = buf[Constants.PSTN_POS];
            if ((dialer & Constants.MASK_PSTN_LINE_PRESENT) > 0)
            {
                this.Dispatcher.Invoke((Action)(() => StatusPSTN.ToolTip = Properties.Resources.PSTN + Properties.Resources.Connected));
                this.Dispatcher.Invoke((Action)(() => StatusPSTN.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => StatusPSTN.BorderBrush = Brushes.LightSeaGreen));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => StatusPSTN.ToolTip = Properties.Resources.PSTN + Properties.Resources.Disconnected));
                this.Dispatcher.Invoke((Action)(() => StatusPSTN.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => StatusPSTN.BorderBrush = Brushes.Gray));
            }


            if ((dialer & Constants.MASK_PSTN_CALL_ACTIVE) > 0)
            {
                this.Dispatcher.Invoke((Action)(() => StatusCall.ToolTip = Properties.Resources.InCall + Properties.Resources.Yes));
                this.Dispatcher.Invoke((Action)(() => StatusCall.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => StatusCall.BorderBrush = Brushes.LightSeaGreen));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => StatusCall.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => StatusCall.BorderBrush = Brushes.Gray));
            }

            #endregion
            #region TIMEZONES
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_1], StatusTimezone1);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_2], StatusTimezone2);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_3], StatusTimezone3);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_4], StatusTimezone4);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_5], StatusTimezone5);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_6], StatusTimezone6);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_7], StatusTimezone7);
            UpdateTimezonesRealTimeObjects(buf[Constants.TIMEZONE_8], StatusTimezone8);
            #endregion
        }

        private async void UpdatePartitionsRealTimeObjects(byte partition, Tile partition_object, short arming_partition)
        {
            if ((FLAG_PARTITION_IS_ARMING == 1 && arming_partition == partitionIsArming) && arming_code_accepted)
            {
                this.Dispatcher.Invoke((Action)(() => partition_object.BorderBrush = Brushes.Khaki));
                this.Dispatcher.Invoke((Action)(() => partition_object.ToolTip = Properties.Resources.Arming));
                if (ArmMode == 0)
                   await Task.Delay(Convert.ToInt32(databaseDataSet.Area.Rows[arming_partition - 100]["Exit timer away"]) * 1000);
                else if (ArmMode == 1)
                   await Task.Delay(Convert.ToInt32(databaseDataSet.Area.Rows[arming_partition - 100]["Exit timer stay"]) * 1000);
            }

            if ((partition & Constants.MASK_PARTITION_ALARM) > 0)
            {
                if (FLAG_PARTITION_IS_ARMING == 1 && arming_partition == partitionIsArming)
                {
                    FLAG_PARTITION_IS_ARMING = 0;
                    partitionIsArming = 0;
                }

                this.Dispatcher.Invoke((Action)(() => partition_object.ToolTip = Properties.Resources.RTAlarm + Properties.Resources.Yes));
                this.Dispatcher.Invoke((Action)(() => partition_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => partition_object.BorderBrush = Brushes.IndianRed));
            }
            else
            {
                if (FLAG_PARTITION_IS_ARMING == 1 && arming_partition == partitionIsArming)
                {
                    FLAG_PARTITION_IS_ARMING = 0;
                    partitionIsArming = 0;
                }

                this.Dispatcher.Invoke((Action)(() => partition_object.ToolTip = Properties.Resources.RTAlarm + Properties.Resources.No));
                this.Dispatcher.Invoke((Action)(() => partition_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => partition_object.BorderBrush = Brushes.LightSeaGreen));
            }

            if ((partition & Constants.MASK_PARTITION_ARMED_AWAY) > 0)
            {

                if (!((partition & Constants.MASK_PARTITION_ALARM) > 0))
                {
                    if (FLAG_PARTITION_IS_ARMING == 1 && arming_partition == partitionIsArming)
                    {
                        FLAG_PARTITION_IS_ARMING = 0;
                        partitionIsArming = 0;
                    }

                    this.Dispatcher.Invoke((Action)(() => partition_object.ToolTip += "\r\n " + Properties.Resources.RTArmed + Properties.Resources.Away));
                    this.Dispatcher.Invoke((Action)(() => partition_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => partition_object.BorderBrush = Brushes.LightSeaGreen));
                }
            }
            else if ((partition & Constants.MASK_PARTITION_ARMED_STAY) > 0)
            {

                if (!((partition & Constants.MASK_PARTITION_ALARM) > 0))
                {

                    if (FLAG_PARTITION_IS_ARMING == 1 && arming_partition == partitionIsArming)
                    {
                        FLAG_PARTITION_IS_ARMING = 0;
                        partitionIsArming = 0;
                    }
                    this.Dispatcher.Invoke((Action)(() => partition_object.ToolTip += "\r\n " + Properties.Resources.RTArmed + Properties.Resources.Stay));
                    this.Dispatcher.Invoke((Action)(() => partition_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => partition_object.BorderBrush = Brushes.LightSeaGreen));
                }
            }
            else
            {
                if (!((partition & Constants.MASK_PARTITION_ALARM) > 0))
                {
                    if (FLAG_PARTITION_IS_ARMING == 1 && arming_partition == partitionIsArming)
                    {
                        FLAG_PARTITION_IS_ARMING = 0;
                        partitionIsArming = 0;
                    }
                    this.Dispatcher.Invoke((Action)(() => partition_object.ToolTip += "\r\n " + Properties.Resources.RTArmed + Properties.Resources.No));
                    this.Dispatcher.Invoke((Action)(() => partition_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => partition_object.BorderBrush = Brushes.Gray));
                }
            }

        }
        private void UpdateZonesRealTimeObjects(byte zone, Tile zone_object)
        {
            if ((zone & Constants.MASK_ZONA_ACTIVE) > 0)
            {
                this.Dispatcher.Invoke((Action)(() => zone_object.Visibility = Visibility.Visible));
                if ((zone & Constants.MASK_ZONA_ALARM) > 0)
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip = Properties.Resources.State + Properties.Resources.Alarm));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderBrush = Brushes.IndianRed));

                    if ((zone & Constants.MASK_ZONA_OPEN) > 0)
                    {
                        this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip = Properties.Resources.State + Properties.Resources.Open));
                        this.Dispatcher.Invoke((Action)(() => zone_object.BorderThickness = new Thickness(2)));
                        this.Dispatcher.Invoke((Action)(() => zone_object.BorderBrush = Brushes.Khaki));
                    }
                }
                else if ((zone & Constants.MASK_ZONA_OPEN) > 0)
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip = Properties.Resources.State + Properties.Resources.Open));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderBrush = Brushes.Khaki));
                }
                else if ((zone & Constants.MASK_ZONA_TAMPER) > 0)
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip = Properties.Resources.State + Properties.Resources.TamperAlarm));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderBrush = Brushes.IndianRed));
                }
                else if ((zone & Constants.MASK_ZONA_MASK) > 0)
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip = Properties.Resources.State + Properties.Resources.MaskAlarm));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderBrush = Brushes.IndianRed));
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip = Properties.Resources.State + Properties.Resources.Ok));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderThickness = new Thickness(2)));
                    this.Dispatcher.Invoke((Action)(() => zone_object.BorderBrush = Brushes.LightSeaGreen));
                }


                if ((zone & Constants.MASK_ZONA_BYPASS) > 0)
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip += "\r\n " + Properties.Resources.Bypass + Properties.Resources.Yes));
                    this.Dispatcher.Invoke((Action)(() => zone_object.Background = Brushes.BurlyWood));
                }
                else
                {
                    this.Dispatcher.Invoke((Action)(() => zone_object.ToolTip += "\r\n " + Properties.Resources.Bypass + Properties.Resources.No));
                    this.Dispatcher.Invoke((Action)(() => zone_object.Background = Brushes.LightGray));
                }
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => zone_object.Visibility = Visibility.Collapsed));
            }


        }
        private void UpdateOutputsRealTimeObjects(byte output, Tile output_object)
        {
            if ((output & Constants.MASK_OUTPUT_ALARM) > 0)
            {
                this.Dispatcher.Invoke((Action)(() => output_object.ToolTip = Properties.Resources.RTAlarm + Properties.Resources.Yes));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderBrush = Brushes.IndianRed));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => output_object.ToolTip = Properties.Resources.RTAlarm + Properties.Resources.No));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderBrush = Brushes.Gray));
            }


            if ((output & Constants.MASK_OUTPUT_ACTIVE) > 0)
            {
                this.Dispatcher.Invoke((Action)(() => output_object.ToolTip += "\r\n " + Properties.Resources.RTActive + Properties.Resources.Yes));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderBrush = Brushes.LightSeaGreen));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => output_object.ToolTip += "\r\n " + Properties.Resources.RTActive + Properties.Resources.No));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => output_object.BorderBrush = Brushes.Gray));
            }

        }
        private void UpdateTimezonesRealTimeObjects(byte timezone, Tile timezone_object)
        {
            if ((timezone & Constants.TIMEZONE_IN_PERIOD) > 0)
            {
                this.Dispatcher.Invoke((Action)(() => timezone_object.ToolTip = Properties.Resources.RTActive + Properties.Resources.Yes));
                this.Dispatcher.Invoke((Action)(() => timezone_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => timezone_object.BorderBrush = Brushes.LightSeaGreen));
            }
            else
            {
                this.Dispatcher.Invoke((Action)(() => timezone_object.ToolTip = Properties.Resources.RTActive + Properties.Resources.No));
                this.Dispatcher.Invoke((Action)(() => timezone_object.BorderThickness = new Thickness(2)));
                this.Dispatcher.Invoke((Action)(() => timezone_object.BorderBrush = Brushes.Gray));
            }

        }

        private void StatusPartition1_Click(object sender, RoutedEventArgs e)
        {
            var RealTimeActionsWindow = new RealTimeActions(this, sender as Tile, 0xD1);

            RealTimeActionsWindow.Show();
        }
        private void StatusZone_Click(object sender, RoutedEventArgs e)
        {
            var RealTimeActionsWindow = new RealTimeActions(this, sender as Tile, 0xD2);

            RealTimeActionsWindow.Show();
        }
        private void StatusOutputs_Click(object sender, RoutedEventArgs e)
        {
            var RealTimeActionsWindow = new RealTimeActions(this, sender as Tile, 0xD3);

            RealTimeActionsWindow.Show();
        }
        #endregion

        private async void OutputsCleanTile_Click(object sender, RoutedEventArgs e)
        {
            int selected_output = outputDataGrid.SelectedIndex;

            if (!selected_output.Equals(0xFF))
            {

                var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");

                await Task.Run(() =>
                {
                    controller.SetMessage(Properties.Resources.PleaseWait);
                    #region GLOBAL SYSTEM CONFIG
                    //siren tamper
                    if (databaseDataSet.GlobalSystem.Rows[0]["Siren tamper output - 1"].Equals(selected_output))
                        databaseDataSet.GlobalSystem.Rows[0]["Siren tamper output - 1"] = 0xFF;
                    if (databaseDataSet.GlobalSystem.Rows[0]["Siren tamper output - 2"].Equals(selected_output))
                        databaseDataSet.GlobalSystem.Rows[0]["Siren tamper output - 2"] = 0xFF;
                    if (databaseDataSet.GlobalSystem.Rows[0]["Siren tamper output - 3"].Equals(selected_output))
                        databaseDataSet.GlobalSystem.Rows[0]["Siren tamper output - 3"] = 0xFF;
                    //panel_tamper
                    if (databaseDataSet.GlobalSystem.Rows[0]["Panel tamper output - 1"].Equals(selected_output))
                        databaseDataSet.GlobalSystem.Rows[0]["Panel tamper output - 1"] = 0xFF;
                    if (databaseDataSet.GlobalSystem.Rows[0]["Panel tamper output - 2"].Equals(selected_output))
                        databaseDataSet.GlobalSystem.Rows[0]["Panel tamper output - 2"] = 0xFF;
                    if (databaseDataSet.GlobalSystem.Rows[0]["Panel tamper output - 3"].Equals(selected_output))
                        databaseDataSet.GlobalSystem.Rows[0]["Panel tamper output - 3"] = 0xFF;
                    #endregion

                    #region Partitions

                    for (int i = 0; i < Constants.KP_MAX_AREAS; i++)
                    {
                        //beeps_away_arm_outputs
                        if (databaseDataSet.Area.Rows[i]["Away arm beeps output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Away arm beeps output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Away arm beeps output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Away arm beeps output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Away arm beeps output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Away arm beeps output - 3"] = 0xFF;
                        //beeps_away_disarm_outputs
                        if (databaseDataSet.Area.Rows[i]["Away disarm beeps output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Away disarm beeps output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Away disarm beeps output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Away disarm beeps output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Away disarm beeps output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Away disarm beeps output - 3"] = 0xFF;
                        //beeps_stay_arm_outputs
                        if (databaseDataSet.Area.Rows[i]["Stay arm beeps output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Stay arm beeps output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Stay arm beeps output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Stay arm beeps output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Stay arm beeps output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Stay arm beeps output - 3"] = 0xFF;
                        //>beeps_stay_disarm_outputs
                        if (databaseDataSet.Area.Rows[i]["Stay disarm beeps output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Stay disarm beeps output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Stay disarm beeps output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Stay disarm beeps output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Stay disarm beeps output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Stay disarm beeps output - 3"] = 0xFF;
                        //>away_arm_outputs
                        if (databaseDataSet.Area.Rows[i]["Arm away output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm away output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm away output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm away output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm away output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm away output - 3"] = 0xFF;
                        //>stay_arm_outputs
                        if (databaseDataSet.Area.Rows[i]["Arm stay output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm stay output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm stay output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm stay output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm stay output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm stay output - 3"] = 0xFF;
                        //>away_disarm_outputs
                        if (databaseDataSet.Area.Rows[i]["Disarm away output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm away output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm away output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm away output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm away output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm away output - 3"] = 0xFF;
                        //> stay_disarm_outputs
                        if (databaseDataSet.Area.Rows[i]["Disarm stay output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm stay output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm stay output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm stay output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm stay output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm stay output - 3"] = 0xFF;
                        //>pulse_away_arm_outputs
                        if (databaseDataSet.Area.Rows[i]["Arm away pulsed output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm away pulsed output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm away pulsed output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm away pulsed output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm away pulsed output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm away pulsed output - 3"] = 0xFF;
                        //>pulse_stay_arm_outputs
                        if (databaseDataSet.Area.Rows[i]["Arm stay pulsed output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm stay pulsed output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm stay pulsed output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm stay pulsed output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arm stay pulsed output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arm stay pulsed output - 3"] = 0xFF;
                        //>pulse_away_disarm_outputs
                        if (databaseDataSet.Area.Rows[i]["Disarm pulsed away output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm pulsed away output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm pulsed away output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm pulsed away output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm pulsed away output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm pulsed away output - 3"] = 0xFF;
                        //>pulse_stay_disarm_outputs
                        if (databaseDataSet.Area.Rows[i]["Disarm pulsed stay output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm pulsed stay output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm pulsed stay output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm pulsed stay output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Disarm pulsed stay output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Disarm pulsed stay output - 3"] = 0xFF;
                        //>away_arming_outputs
                        if (databaseDataSet.Area.Rows[i]["Arming away output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming away output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming away output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming away output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming away output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming away output - 3"] = 0xFF;
                        //>pulse_away_arming_outputs
                        if (databaseDataSet.Area.Rows[i]["Arming away pulsed output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming away pulsed output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming away pulsed output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming away pulsed output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming away pulsed output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming away pulsed output - 3"] = 0xFF;
                        //>stay_arming_outputs
                        if (databaseDataSet.Area.Rows[i]["Arming stay output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming stay output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming stay output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming stay output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming stay output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming stay output - 3"] = 0xFF;
                        //>pulse_stay_arming_outputs
                        if (databaseDataSet.Area.Rows[i]["Arming stay pulsed output - 1"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming stay pulsed output - 1"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming stay pulsed output - 2"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming stay pulsed output - 2"] = 0xFF;
                        if (databaseDataSet.Area.Rows[i]["Arming stay pulsed output - 3"].Equals(selected_output))
                            databaseDataSet.Area.Rows[i]["Arming stay pulsed output - 3"] = 0xFF;
                    }

                    #endregion

                    #region ZONES
                    for (int i = 0; i < Constants.KP_MAX_ZONES; i++)
                    {
                        //>tamper
                        if (databaseDataSet.Zone.Rows[i]["Tamper alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Tamper alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Tamper alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Tamper alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Tamper alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Tamper alarm output 3"] = 0xFF;
                        //>anti_mask
                        if (databaseDataSet.Zone.Rows[i]["Anti mask alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Anti mask alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Anti mask alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Anti mask alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Anti mask alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Anti mask alarm output 3"] = 0xFF;
                        //>arm_away_timezone
                        if (databaseDataSet.Zone.Rows[i]["Arm away by timezone - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away by timezone - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away by timezone - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away by timezone - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away by timezone - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away by timezone - output 3"] = 0xFF;
                        //>arm_away_code
                        if (databaseDataSet.Zone.Rows[i]["Arm away with code - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with code - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away with code - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with code - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away with code - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with code - output 3"] = 0xFF;
                        //>arm_away_command
                        if (databaseDataSet.Zone.Rows[i]["Arm away with command - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with command - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away with command - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with command - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away with command - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with command - output 3"] = 0xFF;
                        //>arm_away_key_switch
                        if (databaseDataSet.Zone.Rows[i]["Arm away with keyswtich - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with keyswtich - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away with keyswtich - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with keyswtich - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away with keyswtich - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away with keyswtich - output 3"] = 0xFF;
                        //>arm_away_remote
                        if (databaseDataSet.Zone.Rows[i]["Arm away remotely - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away remotely - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away remotely - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away remotely - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm away remotely - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm away remotely - output 3"] = 0xFF;
                        //>arm_stay_timezone
                        if (databaseDataSet.Zone.Rows[i]["Arm stay by timezone - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay by timezone - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay by timezone - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay by timezone - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay by timezone - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay by timezone - output 3"] = 0xFF;
                        //>arm_stay_code
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with code - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with code - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with code - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with code - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with code - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with code - output 3"] = 0xFF;
                        //>arm_stay_command
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with command - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with command - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with command - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with command - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with command - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with command - output 3"] = 0xFF;
                        //>arm_stay_key_switch
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with keyswtich - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with keyswtich - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with keyswtich - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with keyswtich - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay with keyswtich - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay with keyswtich - output 3"] = 0xFF;
                        //>arm_stay_remote
                        if (databaseDataSet.Zone.Rows[i]["Arm stay remotely - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay remotely - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay remotely - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay remotely - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Arm stay remotely - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Arm stay remotely - output 3"] = 0xFF;
                        //>disarm_away_timezone
                        if (databaseDataSet.Zone.Rows[i]["Disarm away by timezone - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away by timezone - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away by timezone - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away by timezone - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away by timezone - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away by timezone - output 3"] = 0xFF;
                        //>disarm_away_code
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with code - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with code - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with code - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with code - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with code - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with code - output 3"] = 0xFF;
                        //>disarm_away_command
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with command - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with command - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with command - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with command - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with command - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with command - output 3"] = 0xFF;
                        //>disarm_away_key_switch
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with keyswtich - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with keyswtich - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with keyswtich - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with keyswtich - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away with keyswtich - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away with keyswtich - output 3"] = 0xFF;
                        //>disarm_away_remote
                        if (databaseDataSet.Zone.Rows[i]["Disarm away remotely - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away remotely - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away remotely - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away remotely - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm away remotely - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm away remotely - output 3"] = 0xFF;
                        //>disarm_stay_timezone
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay by timezone - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay by timezone - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay by timezone - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay by timezone - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay by timezone - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay by timezone - output 3"] = 0xFF;
                        //>disarm_stay_code
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 3"] = 0xFF;
                        //>disarm_stay_command
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with code - output 3"] = 0xFF;
                        //disarm_stay_command
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with command - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with command - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with command - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with command - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with command - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with command - output 3"] = 0xFF;
                        //>disarm_stay_key_switch
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 3"] = 0xFF;
                        //>disarm_stay_remote
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Disarm stay with keyswtich - output 3"] = 0xFF;
                        //>_24_hour
                        if (databaseDataSet.Zone.Rows[i]["24 hour alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["24 hour alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["24 hour alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["24 hour alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["24 hour alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["24 hour alarm output 3"] = 0xFF;
                        //>fire
                        if (databaseDataSet.Zone.Rows[i]["Fire alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Fire alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Fire alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Fire alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Fire alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Fire alarm output 3"] = 0xFF;
                        //>zone_entry_delay
                        if (databaseDataSet.Zone.Rows[i]["Entry time alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Entry time alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Entry time alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Entry time alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Entry time alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Entry time alarm output 3"] = 0xFF;
                        //>zone_alarm
                        if (databaseDataSet.Zone.Rows[i]["Zone alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Zone alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Zone alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Zone alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Zone alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Zone alarm output 3"] = 0xFF;
                        //>chime_alarm
                        if (databaseDataSet.Zone.Rows[i]["Chime alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Chime alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Chime alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Chime alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Chime alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Chime alarm output 3"] = 0xFF;
                        //>sensor_watch
                        if (databaseDataSet.Zone.Rows[i]["Sensor watch alarm output 1"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Sensor watch alarm output 1"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Sensor watch alarm output 2"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Sensor watch alarm output 2"] = 0xFF;
                        if (databaseDataSet.Zone.Rows[i]["Sensor watch alarm output 3"].Equals(selected_output))
                            databaseDataSet.Zone.Rows[i]["Sensor watch alarm output 3"] = 0xFF;
                    }
                    #endregion
                    controller.CloseAsync();
                });
            }
        }

        private void ckZoneKeypadVisible_Click(object sender, RoutedEventArgs e)
        {
            bool flag = (bool)ckZoneKeypadVisible.IsChecked;

            if (flag)
            {
                ckBypass1.IsEnabled = ckBypass2.IsEnabled = ckBypass3.IsEnabled = ckBypass4.IsEnabled = ckBypass5.IsEnabled = ckBypass6.IsEnabled =
                    ckBypass7.IsEnabled = ckBypass8.IsEnabled = true;
            }
            else
            {
                ckBypass1.IsEnabled = ckBypass2.IsEnabled = ckBypass3.IsEnabled = ckBypass4.IsEnabled = ckBypass5.IsEnabled = ckBypass6.IsEnabled =
                    ckBypass7.IsEnabled = ckBypass8.IsEnabled = false;

                ckBypass1.IsChecked = false;
                ckBypass2.IsChecked = false;
                ckBypass3.IsChecked = false;
                ckBypass4.IsChecked = false;
                ckBypass5.IsChecked = false;
                ckBypass6.IsChecked = false;
                ckBypass7.IsChecked = false;
                ckBypass8.IsChecked = false;
            }
        }

        #region Index MouseDoubleClick

        private void SelectPVT_onMouseDoubleClick(DataGrid dg, TabItem ti, TabItem ti_not, string vs_string, TreeViewItem tvi)
        {
            int row = dg.SelectedIndex;
            System.Windows.Data.CollectionViewSource vs = ((System.Windows.Data.CollectionViewSource)(this.FindResource(vs_string)));

            int column = dg.CurrentColumn.DisplayIndex;

            if (column == 0 && row != -1)
            {
                MainTabControl.SelectedItem = ti;
                vs.View.MoveCurrentToPosition(dg.SelectedIndex);

                tvi.IsExpanded = true;
                (tvi.Items[dg.SelectedIndex] as TreeViewItem).IsSelected = true;
            }
            else
            {
                MainTabControl.SelectedItem = ti_not;
                vs.View.MoveCurrentToPosition(dg.SelectedIndex);

                tvi.IsExpanded = false;
                (tvi.Items[dg.SelectedIndex] as TreeViewItem).IsSelected = false;
            }

        }

        private void zoneDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (zoneDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(zoneDataGrid, MainZonePVTTab, MainZonesTab, "zoneViewSource", TreeviewZones);
        }

        private void areaDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (areaDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(areaDataGrid, MainAreaPVTTab, MainAreasTab, "areaViewSource", TreeviewAreas);
        }

        private void userDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (userDataGrid.SelectedIndex != -1)
            {
                int row = userDataGrid.SelectedIndex;
                int column = userDataGrid.CurrentColumn.DisplayIndex;

                System.Windows.Data.CollectionViewSource userViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("userViewSource")));

                if (column == 0 && row <= 200)
                {
                    MainTabControl.SelectedItem = MainUsersPVTTab;
                    userViewSource.View.MoveCurrentToPosition(userDataGrid.SelectedIndex);

                    TreeviewUsers.IsExpanded = true;
                    (TreeviewUsers.Items[userDataGrid.SelectedIndex] as TreeViewItem).IsSelected = true;
                }
                else
                {
                    MainTabControl.SelectedItem = MainUsersTab;
                    userViewSource.View.MoveCurrentToPosition(userDataGrid.SelectedIndex);

                    TreeviewUsers.IsExpanded = false;
                    (TreeviewUsers.Items[userDataGrid.SelectedIndex] as TreeViewItem).IsSelected = false;
                }
            }
        }

        private void keypadDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (keypadDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(keypadDataGrid, MainKeypadsPVTTab, MainKeypadsTab, "keypadViewSource", TreeviewKeypads);
        }

        private void expanderDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (expanderDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(expanderDataGrid, MainExpandersPVTTab, MainExpandersTab, "expanderViewSource", TreeviewExpanders);
        }

        private void outputDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (outputDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(outputDataGrid, MainOutputsPVTTab, MainOutputsTab, "outputViewSource", TreeviewOutputs);
        }

        private void timezoneDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (timezoneDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(timezoneDataGrid, MainTimezonesPVTTab, MainTimezonesTab, "timezoneViewSource", TreeviewTimezones);
        }

        private void phoneDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (phoneDataGrid.SelectedIndex != -1)
                SelectPVT_onMouseDoubleClick(phoneDataGrid, MainPhonesPVTTab, MainPhonesTab, "phoneViewSource", TreeviewPhones);
        }

        #endregion

        #region HomeWindow

        private void Open_Home_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[0]).IsSelected = true;
        }

        private void Open_Areas_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[1]).IsSelected = true;
        }

        private void Open_Zones_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[2]).IsSelected = true;
        }

        private void Open_Keypads_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[3]).IsSelected = true;
        }

        private void Open_Expanders_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[16]).IsSelected = true;
        }

        private void Open_Outputs_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[4]).IsSelected = true;
        }

        private void Open_Timezones_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[5]).IsSelected = true;
        }

        private void Open_Users_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[6]).IsSelected = true;
        }

        private void Open_Phones_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[7]).IsSelected = true;
        }

        private void Open_Dialer_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[8]).IsSelected = true;
        }

        private void Open_GlobalConfig_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[9]).IsSelected = true;
        }

        private void Open_ClientInfo_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[10]).IsSelected = true;
        }

        private void Open_Events_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[11]).IsSelected = true;
        }

        private void Open_Audio_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[12]).IsSelected = true;
        }

        private void Open_Status_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[13]).IsSelected = true;
        }

        private void Open_Debug_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[14]).IsSelected = true;
        }

        private void Open_FWUpdate_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[15]).IsSelected = true;
        }
        private void Open_Memory_Click(object sender, RoutedEventArgs e)
        {
            ((TreeViewItem)MainTreeView.Items[17]).IsSelected = true;
        }


        #endregion

        private void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        #region Home/Back/Next Buttons

        private void Button_GoHome_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 0;
            TreeviewHome.IsSelected = true;
            TreeviewAreas.IsExpanded = false;
            TreeviewZones.IsExpanded = false;
            TreeviewKeypads.IsExpanded = false;
            TreeviewExpanders.IsExpanded = false;
            TreeviewOutputs.IsExpanded = false;
            TreeviewUsers.IsExpanded = false;
            TreeviewTimezones.IsExpanded = false;
            TreeviewPhones.IsExpanded = false;
        }
        private void Button_Next_Click(TreeViewItem tvi)
        {
            int index = 0;
            foreach (TreeViewItem i in tvi.Items)
            {
                if (i.Equals(MainTreeView.SelectedItem))
                {
                    break;
                }
                index++;
            }
            ((tvi).Items[(index + 1) % (tvi as TreeViewItem).Items.Count] as TreeViewItem).IsSelected = true;
        }
        private void Button_Back_Click(TreeViewItem tvi, int maxitems)
        {
            int index = 0;
            foreach (TreeViewItem i in tvi.Items)
            {
                if (i.Equals(MainTreeView.SelectedItem))
                {
                    break;
                }
                index++;
            }
            if (index > 0)
                ((tvi).Items[index - 1] as TreeViewItem).IsSelected = true;
            else if (index == 0)
                ((tvi).Items[(maxitems - 1) % (tvi as TreeViewItem).Items.Count] as TreeViewItem).IsSelected = true;

        }

        #region Areas
        private void MainArea_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewAreas);
        }
        private void MainArea_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewAreas, Constants.KP_MAX_AREAS);
        }
        private void MainArea_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 2;
            TreeviewAreas.IsSelected = true;
            TreeviewAreas.IsExpanded = false;
        }
        #endregion

        #region Zones
        private void MainZone_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewZones);
        }
        private void MainZone_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewZones, Constants.KP_MAX_ZONES);
        }
        private void MainZone_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 1;
            TreeviewZones.IsSelected = true;
            TreeviewZones.IsExpanded = false;
        }
        #endregion

        #region Keypads
        private void MainKeypad_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewKeypads);
        }
        private void MainKeypad_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewKeypads, Constants.KP_MAX_KEYPADS);
        }
        private void MainKeypad_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 4;
            TreeviewKeypads.IsSelected = true;
            TreeviewKeypads.IsExpanded = false;
        }
        #endregion

        #region Expanders
        private void MainExpander_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewExpanders);
        }
        private void MainExpander_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewExpanders, Constants.KP_MAX_EXPANDERS);
        }
        private void MainExpander_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 16;
            TreeviewExpanders.IsSelected = true;
            TreeviewExpanders.IsExpanded = false;
        }
        #endregion

        #region Outputs
        private void MainOutput_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewOutputs);
        }
        private void MainOutput_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewOutputs, Constants.KP_MAX_OUTPUTS);
        }
        private void MainOutput_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 6;
            TreeviewOutputs.IsSelected = true;
            TreeviewOutputs.IsExpanded = false;
        }
        #endregion

        #region Users
        private void MainUser_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewUsers);
        }
        private void MainUser_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewUsers, Constants.KP_MAX_USERS - 5);
        }
        private void MainUser_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 3;
            TreeviewUsers.IsSelected = true;
            TreeviewUsers.IsExpanded = false;
        }
        #endregion

        #region Timezones
        private void MainTimezone_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewTimezones);
        }
        private void MainTimezone_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewTimezones, Constants.KP_MAX_TIMEZONES);
        }
        private void MainTimezone_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 7;
            TreeviewTimezones.IsSelected = true;
            TreeviewTimezones.IsExpanded = false;
        }
        #endregion

        #region Phones
        private void MainPhone_button_next_Click(object sender, RoutedEventArgs e)
        {
            Button_Next_Click(TreeviewPhones);
        }
        private void MainPhone_button_back_Click(object sender, RoutedEventArgs e)
        {
            Button_Back_Click(TreeviewPhones, Constants.KP_MAX_PHONES);
        }
        private void MainPhone_button_undo_Click(object sender, RoutedEventArgs e)
        {
            MainTabControl.SelectedIndex = 8;
            TreeviewPhones.IsSelected = true;
            TreeviewPhones.IsExpanded = false;
        }

        #endregion

        #endregion

        #region Terminals Circuit
        private void terminal_circuit_typeColumn1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = terminal_circuit_typeColumn1.SelectedIndex;

            switch (id)
            {
                case 0: //R1
                    R1_Grid.Visibility = Visibility.Visible;
                    R2_Grid.Visibility = Visibility.Hidden;
                    R3_Grid.Visibility = Visibility.Hidden;
                    Terminals_R1.Visibility = Visibility.Visible;
                    Terminals_R2.Visibility = Visibility.Hidden;
                    Terminals_R3.Visibility = Visibility.Hidden;
                    R1_NC.Visibility = Visibility.Visible;
                    R1_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Hidden;
                    R2_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Hidden;
                    R3_NO.Visibility = Visibility.Hidden;
                    Circuit_R1.Visibility = Visibility.Visible;
                    Circuit_R1R2.Visibility = Visibility.Hidden;
                    Circuit_R1R3.Visibility = Visibility.Hidden;
                    Circuit_R1R2R3.Visibility = Visibility.Hidden;
                    break;
                case 1: //R1+R2
                    R1_Grid.Visibility = Visibility.Visible;
                    R2_Grid.Visibility = Visibility.Visible;
                    R3_Grid.Visibility = Visibility.Hidden;
                    Terminals_R1.Visibility = Visibility.Visible;
                    Terminals_R2.Visibility = Visibility.Visible;
                    Terminals_R3.Visibility = Visibility.Hidden;
                    R1_NC.Visibility = Visibility.Visible;
                    R1_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Visible;
                    R2_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Hidden;
                    R3_NO.Visibility = Visibility.Hidden;
                    Circuit_R1.Visibility = Visibility.Hidden;
                    Circuit_R1R2.Visibility = Visibility.Visible;
                    Circuit_R1R3.Visibility = Visibility.Hidden;
                    Circuit_R1R2R3.Visibility = Visibility.Hidden;
                    break;
                case 2: //R1+R3
                    R1_Grid.Visibility = Visibility.Visible;
                    R2_Grid.Visibility = Visibility.Hidden;
                    R3_Grid.Visibility = Visibility.Visible;
                    Terminals_R1.Visibility = Visibility.Visible;
                    Terminals_R2.Visibility = Visibility.Hidden;
                    Terminals_R3.Visibility = Visibility.Visible;
                    R1_NC.Visibility = Visibility.Visible;
                    R1_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Hidden;
                    R2_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Visible;
                    R3_NO.Visibility = Visibility.Hidden;
                    Circuit_R1.Visibility = Visibility.Hidden;
                    Circuit_R1R2.Visibility = Visibility.Hidden;
                    Circuit_R1R3.Visibility = Visibility.Visible;
                    Circuit_R1R2R3.Visibility = Visibility.Hidden;
                    break;
                case 3: //R1+R2+R3
                    R1_Grid.Visibility = Visibility.Visible;
                    R2_Grid.Visibility = Visibility.Visible;
                    R3_Grid.Visibility = Visibility.Visible;
                    Terminals_R1.Visibility = Visibility.Visible;
                    Terminals_R2.Visibility = Visibility.Visible;
                    Terminals_R3.Visibility = Visibility.Visible;
                    R1_NC.Visibility = Visibility.Visible;
                    R1_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Visible;
                    R2_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Visible;
                    R3_NO.Visibility = Visibility.Hidden;
                    Circuit_R1.Visibility = Visibility.Hidden;
                    Circuit_R1R2.Visibility = Visibility.Hidden;
                    Circuit_R1R3.Visibility = Visibility.Hidden;
                    Circuit_R1R2R3.Visibility = Visibility.Visible;
                    break;
                default:
                    R1_Grid.Visibility = Visibility.Hidden;
                    R2_Grid.Visibility = Visibility.Hidden;
                    R3_Grid.Visibility = Visibility.Hidden;
                    Terminals_R1.Visibility = Visibility.Hidden;
                    Terminals_R2.Visibility = Visibility.Hidden;
                    Terminals_R3.Visibility = Visibility.Hidden;
                    R1_NC.Visibility = Visibility.Hidden;
                    R1_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Hidden;
                    R2_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Hidden;
                    R3_NO.Visibility = Visibility.Hidden;
                    Circuit_R1.Visibility = Visibility.Hidden;
                    Circuit_R1R2.Visibility = Visibility.Hidden;
                    Circuit_R1R3.Visibility = Visibility.Hidden;
                    Circuit_R1R2R3.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void r1value_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = r1value_combobox.SelectedIndex;

            switch (id)
            {
                case 0: //0k
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 1: //1k
                    R1_1k.Visibility = Visibility.Visible;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 2: //2k2
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Visible;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 3: //3k3
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Visible;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 4: //3k9
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Visible;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 5: //4k7
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Visible;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 6: //5k6
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Visible;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 7: //6k8
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Visible;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 8: //8k2
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Visible;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 9: //10k
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Visible;
                    break;
                default:
                    R1_1k.Visibility = Visibility.Hidden;
                    R1_2k2.Visibility = Visibility.Hidden;
                    R1_3k3.Visibility = Visibility.Hidden;
                    R1_3k9.Visibility = Visibility.Hidden;
                    R1_4k7.Visibility = Visibility.Hidden;
                    R1_5k6.Visibility = Visibility.Hidden;
                    R1_6k8.Visibility = Visibility.Hidden;
                    R1_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void r1contact_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = r1contact_combobox.SelectedIndex;

            switch (id)
            {
                case 0: //NO
                    R1_NO.Visibility = Visibility.Visible;
                    R1_NC.Visibility = Visibility.Hidden;
                    break;
                case 1: //NC
                    R1_NO.Visibility = Visibility.Hidden;
                    R1_NC.Visibility = Visibility.Visible;
                    break;
                default:
                    R1_NO.Visibility = Visibility.Hidden;
                    R1_NC.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void r2value_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = r2value_combobox.SelectedIndex;

            switch (id)
            {
                case 0: //0k
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 1: //1k
                    R2_1k.Visibility = Visibility.Visible;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 2: //2k2
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Visible;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 3: //3k3
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Visible;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R1_10k.Visibility = Visibility.Hidden;
                    break;
                case 4: //3k9
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Visible;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 5: //4k7
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Visible;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 6: //5k6
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Visible;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 7: //6k8
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Visible;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 8: //8k2
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Visible;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
                case 9: //10k
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Visible;
                    break;
                default:
                    R2_1k.Visibility = Visibility.Hidden;
                    R2_2k2.Visibility = Visibility.Hidden;
                    R2_3k3.Visibility = Visibility.Hidden;
                    R2_3k9.Visibility = Visibility.Hidden;
                    R2_4k7.Visibility = Visibility.Hidden;
                    R2_5k6.Visibility = Visibility.Hidden;
                    R2_6k8.Visibility = Visibility.Hidden;
                    R2_8k2.Visibility = Visibility.Hidden;
                    R2_10k.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void r2contact_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = r2contact_combobox.SelectedIndex;

            switch (id)
            {
                case 0: //NO
                    R2_NO.Visibility = Visibility.Visible;
                    R2_NC.Visibility = Visibility.Hidden;
                    break;
                case 1: //NC
                    R2_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Visible;
                    break;
                default:
                    R2_NO.Visibility = Visibility.Hidden;
                    R2_NC.Visibility = Visibility.Hidden;
                    break;
            }
        }

        private void r3value_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = r3value_combobox.SelectedIndex;

            switch (id)
            {
                case 0: //0k
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 1: //1k
                    R3_1k.Visibility = Visibility.Visible;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 2: //2k2
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Visible;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 3: //3k3
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Visible;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 4: //3k9
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Visible;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 5: //4k7
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Visible;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 6: //5k6
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Visible;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 7: //6k8
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Visible;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 8: //8k2
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Visible;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
                case 9: //10k
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Visible;
                    break;
                default:
                    R3_1k.Visibility = Visibility.Hidden;
                    R3_2k2.Visibility = Visibility.Hidden;
                    R3_3k3.Visibility = Visibility.Hidden;
                    R3_3k9.Visibility = Visibility.Hidden;
                    R3_4k7.Visibility = Visibility.Hidden;
                    R3_5k6.Visibility = Visibility.Hidden;
                    R3_6k8.Visibility = Visibility.Hidden;
                    R3_8k2.Visibility = Visibility.Hidden;
                    R3_10k.Visibility = Visibility.Hidden;
                    break;
            }
        }
        private void r3contact_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = r3contact_combobox.SelectedIndex;

            switch (id)
            {
                case 0: //NO
                    R3_NO.Visibility = Visibility.Visible;
                    R3_NC.Visibility = Visibility.Hidden;
                    break;
                case 1: //NC
                    R3_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Visible;
                    break;
                default:
                    R3_NO.Visibility = Visibility.Hidden;
                    R3_NC.Visibility = Visibility.Hidden;
                    break;
            }
        }


        #endregion

        #region Shortcuts

        private void Shortcuts_SingleFilter(DataGridTemplateColumn a)
        {
            a.Visibility = a.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        private void Shortcuts_MinusToPlus(DataGridTemplateColumn minus, DataGridTemplateColumn plus)
        {
            if (minus.Visibility == Visibility.Visible)
                minus.Visibility = Visibility.Hidden;
            else
                plus.Visibility = plus.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
        }
        private void Shortcuts_Disabling(Tile enabled, Tile disabled)
        {
            if (enabled.Visibility == Visibility.Visible)
            {
                enabled.Visibility = Visibility.Collapsed;
                disabled.Visibility = Visibility.Visible;
            }
            else
            {
                enabled.Visibility = Visibility.Visible;
                disabled.Visibility = Visibility.Collapsed;
            }
        }

        #region Zones
        private void ZoneShortcuts_GeneralSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(zone_activeColumn);
            Shortcuts_SingleFilter(soak_testColumn);
            Shortcuts_SingleFilter(always_reportColumn);
            Shortcuts_SingleFilter(arm_if_not_readyColumn);

            Shortcuts_MinusToPlus(EntryTimes_minus, EntryTimes_plus);
            Shortcuts_MinusToPlus(_24H_Config_minus, _24H_Config_plus);
            Shortcuts_MinusToPlus(Keyswitch_Config_minus, Keyswitch_Config_plus);
            Shortcuts_MinusToPlus(Bypass_Config_minus, Bypass_Config_plus);
            Shortcuts_MinusToPlus(Zone_Type_minus, Zone_Type_plus);
            Shortcuts_MinusToPlus(Chime_Config_minus, Chime_Config_plus);

            Shortcuts_Disabling(ZoneShortcuts_SettingsTile, ZoneShortcuts_SettingsTileDISABLED);

        }
        private void ZoneShortcuts_AreasKeypadsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Zones_Areas_Away_minus, Zones_Areas_Away_plus);
            Shortcuts_MinusToPlus(Zones_Areas_Stay_minus, Zones_Areas_Stay_plus);
            Shortcuts_MinusToPlus(Zones_ShowKeypad_minus, Zones_ShowKeypad_plus);

            Zones_KeypadBypass.Visibility = Zones_KeypadBypass.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            Shortcuts_Disabling(ZoneShortcuts_AreasKeypadsTile, ZoneShortcuts_AreasKeypadsTileDISABLED);

        }
        private void ZoneShortcuts_TerminalsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(terminal_circuit_typeColumn);

            Shortcuts_MinusToPlus(R1_Config_minus, R1_Config_plus);
            Shortcuts_MinusToPlus(R2_Config_minus, R2_Config_plus);
            Shortcuts_MinusToPlus(R3_Config_minus, R3_Config_plus);

            Shortcuts_Disabling(ZoneShortcuts_TerminalsTile, ZoneShortcuts_TerminalsTileDISABLED);
        }
        private void ZoneShortcuts_OutputsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Zone_Alarm_Output_minus, Zone_Alarm_Output_plus);
            Shortcuts_MinusToPlus(Chime_Output_minus, Chime_Output_plus);
            Shortcuts_MinusToPlus(Sensor_Watch_Output_minus, Sensor_Watch_Output_plus);
            Shortcuts_MinusToPlus(Entry_Time_Output_minus, Entry_Time_Output_plus);
            Shortcuts_MinusToPlus(Anti_Mask_Output_minus, Anti_Mask_Output_plus);
            Shortcuts_MinusToPlus(_24H_Alarm_Output_minus, _24H_Alarm_Output_plus);
            Shortcuts_MinusToPlus(Fire_Alarm_Output_minus, Fire_Alarm_Output_plus);
            Shortcuts_MinusToPlus(Tamper_Alarm_Output_minus, Tamper_Alarm_Output_plus);

            Shortcuts_Disabling(ZoneShortcuts_OutputsTile, ZoneShortcuts_OutputsTileDISABLED);
        }
        private void ZoneShortcuts_AudioButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Zone_Audio_minus, Zone_Audio_plus);

            Shortcuts_Disabling(ZoneShortcuts_AudioTile, ZoneShortcuts_AudioTileDISABLED);
        }
        #endregion

        #region Areas
        private void AreasShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(not_arm_if_zones_open_after_exit_delayColumn);
            dRCV_account_numberColumn.Visibility = dRCV_account_numberColumn.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            voice_call_codeColumn.Visibility = voice_call_codeColumn.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            Shortcuts_MinusToPlus(Area_Code_Required_minus, Area_Code_Required_plus);
            Shortcuts_MinusToPlus(Exit_Times_minus, Exit_Times_plus);

            Shortcuts_Disabling(AreasShortcuts_SettingsTile, AreasShortcuts_SettingsTileDISABLED);
        }
        private void AreasShortcuts_TimezonesButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Timezones_Start_Arm_minus, Timezones_Start_Arm_plus);
            Shortcuts_MinusToPlus(Timezones_End_Disarm_minus, Timezones_End_Disarm_plus);

            Shortcuts_Disabling(AreasShortcuts_TimezonesTile, AreasShortcuts_TimezonesTileDISABLED);
        }
        private void AreasShortcuts_OutputsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Arm_Away_Outputs_minus, Arm_Away_Outputs_plus);
            Shortcuts_MinusToPlus(Arm_Stay_Outputs_minus, Arm_Stay_Outputs_plus);
            Shortcuts_MinusToPlus(Disarm_Away_Outputs_minus, Disarm_Away_Outputs_plus);
            Shortcuts_MinusToPlus(Disarm_Stay_Outputs_minus, Disarm_Stay_Outputs_plus);
            Shortcuts_MinusToPlus(Arm_Away_Pulsed_Outputs_minus, Arm_Away_Pulsed_Outputs_plus);
            Shortcuts_MinusToPlus(Arm_Stay_Pulsed_Outputs_minus, Arm_Stay_Pulsed_Outputs_plus);
            Shortcuts_MinusToPlus(Disarm_Away_Pulsed_Outputs_minus, Disarm_Away_Pulsed_Outputs_plus);
            Shortcuts_MinusToPlus(Disarm_Stay_Pulsed_Outputs_minus, Disarm_Stay_Pulsed_Outputs_plus);
            Shortcuts_MinusToPlus(Arm_Away_Beeps_Outputs_minus, Arm_Away_Beeps_Outputs_plus);
            Shortcuts_MinusToPlus(Arm_Stay_Beeps_Outputs_minus, Arm_Stay_Beeps_Outputs_plus);
            Shortcuts_MinusToPlus(Disarm_Away_Beeps_Outputs_minus, Disarm_Away_Beeps_Outputs_plus);
            Shortcuts_MinusToPlus(Disarm_Stay_Beeps_Outputs_minus, Disarm_Stay_Beeps_Outputs_plus);

            Shortcuts_Disabling(AreasShortcuts_OutputsTile, AreasShortcuts_OutputsTileDISABLED);
        }
        private void AreasShortcuts_AudioButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Area_Audio_minus, Area_Audio_plus);

            Shortcuts_Disabling(AreasShortcuts_AudioTile, AreasShortcuts_AudioTileDISABLED);
        }
        #endregion

        #region Users
        private void UserShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(User_Type_Column);
            Shortcuts_SingleFilter(User_Active_Column);
            // if (AppRole != 0) Shortcuts_SingleFilter(User_Code_Column);
            if (AppRole != 0) User_Code_Column.Visibility = User_Code_Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            Shortcuts_SingleFilter(User_CanChangeClock);
            Shortcuts_SingleFilter(User_ValidateCalendar);
            Shortcuts_SingleFilter(User_InitialDate);
            Shortcuts_SingleFilter(User_FinalDate);

            Shortcuts_Disabling(UserShortcuts_SettingsTile, UserShortcuts_SettingsTileDISABLED);
        }
        private void UserShortcuts_TimezonesButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(User_Timezone_Permissions_minus, User_Timezone_Permissions_plus);

            Shortcuts_Disabling(UserShortcuts_TimezonesTile, UserShortcuts_TimezonesTileDISABLED);
        }
        private void UserShortcuts_PermissionsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(User_ArmDisarm_Permissions_minus, User_ArmDisarm_Permissions_plus);
            Shortcuts_MinusToPlus(User_Area_Permissions_minus, User_Area_Permissions_plus);
            Shortcuts_MinusToPlus(User_Output_Permissions_minus, User_Output_Permissions_plus);

            Shortcuts_Disabling(UserShortcuts_PermissionsTile, UserShortcuts_PermissionsTileDISABLED);
        }
        private void UserShortcuts_ButtonsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Button_A_AreaAway_minus, Button_A_AreaAway_plus);
            Shortcuts_MinusToPlus(Button_A_AreaStay_minus, Button_A_AreaStay_plus);
            Shortcuts_MinusToPlus(Button_B_AreaAway_minus, Button_B_AreaAway_plus);
            Shortcuts_MinusToPlus(Button_B_AreaStay_minus, Button_B_AreaStay_plus);
            Shortcuts_MinusToPlus(Button_C_AreaAway_minus, Button_C_AreaAway_plus);
            Shortcuts_MinusToPlus(Button_C_AreaStay_minus, Button_C_AreaStay_plus);
            Shortcuts_MinusToPlus(Button_D_AreaAway_minus, Button_D_AreaAway_plus);
            Shortcuts_MinusToPlus(Button_D_AreaStay_minus, Button_D_AreaStay_plus);

            Shortcuts_Disabling(UserShortcuts_ButtonsTile, UserShortcuts_ButtonsTileDISABLED);
        }
        private void UserShortcuts_AudioButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(User_Audio_minus, User_Audio_plus);

            Shortcuts_Disabling(UserShortcuts_AudioTile, UserShortcuts_AudioTileDISABLED);
        }
        #endregion

        #region Keypads
        private void KeypadShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(Keypad_Active_Column);
            Shortcuts_SingleFilter(Backlight_OFF_230V_Column);
            Shortcuts_SingleFilter(Keypad_Enable_Tamper_Column);
            Shortcuts_SingleFilter(NoIndicationsWhileArmed_Column);
            Shortcuts_SingleFilter(SilentDuring_EntryTime_Column);
            Shortcuts_SingleFilter(SilentDuring_ExitTime_Column);
            //Shortcuts_SingleFilter(Keypad_HourFormat_Column);
            Shortcuts_SingleFilter(Keypad_DateFormat_Column);
            Backlight_Time_Column.Visibility = Backlight_Time_Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            Shortcuts_Disabling(KeypadShortcuts_SettingsTile, KeypadShortcuts_SettingsTileDISABLED);
        }
        private void KeypadShortcuts_AreasButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Keypad_Areas_minus, Keypad_Areas_plus);

            Shortcuts_Disabling(KeypadShortcuts_AreasTile, KeypadShortcuts_AreasTileDISABLED);
        }
        private void KeypadShortcuts_BeepsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Keypad_Beeps_minus, Keypad_Beeps_plus);

            Shortcuts_Disabling(KeypadShortcuts_BeepsTile, KeypadShortcuts_BeepsTileDISABLED);
        }
        private void KeypadShortcuts_ArmDisarmButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Keypad_ArmDisarm_Permissions_minus, Keypad_ArmDisarm_Permissions_plus);

            Shortcuts_Disabling(KeypadShortcuts_ArmDisarmTile, KeypadShortcuts_ArmDisarmTileDISABLED);
        }
        private void KeypadShortcuts_FbuttonsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Fbuttons_minus, Fbuttons_plus);

            Shortcuts_Disabling(KeypadShortcuts_FbuttonsTile, KeypadShortcuts_FbuttonsTileDISABLED);
        }
        private void KeypadShortcuts_AudioButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Keypad_Audio_minus, Keypad_Audio_plus);

            Shortcuts_Disabling(KeypadShortcuts_AudioTile, KeypadShortcuts_AudioTileDISABLED);
        }
        #endregion

        #region Expanders
        private void ExpanderShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(Expander_Active_Column);
            Shortcuts_SingleFilter(Expander_Enable_Tamper_Column);
            Shortcuts_SingleFilter(Expander_Config_Tamper_Column);

            Shortcuts_Disabling(ExpanderShortcuts_SettingsTile, ExpanderShortcuts_SettingsTileDISABLED);
        }
        private void ExpanderShortcuts_ConfigPinsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Expander_ConfigPins_minus, Expander_ConfigPins_plus);

            Shortcuts_Disabling(ExpanderShortcuts_ConfigPinsTile, ExpanderShortcuts_ConfigPinsTileDISABLED);
        }

        #endregion

        #region Outputs
        private void OutputShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(cleanassociationsColumn);
            Shortcuts_SingleFilter(invertedColumn);

            Shortcuts_MinusToPlus(TimeConfig_minus, TimeConfig_plus);
            Shortcuts_MinusToPlus(Output_ChimeConfig_minus, Output_ChimeConfig_plus);
            Shortcuts_MinusToPlus(Output_PulseConfig_minus, Output_PulseConfig_plus);
            Shortcuts_MinusToPlus(Output_Beeps_minus, Output_Beeps_plus);

            Shortcuts_Disabling(OutputShortcuts_SettingsTile, OutputShortcuts_SettingsTileDISABLED);
        }
        private void OutputShortcuts_TimezonesButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Output_Timezones_minus, Output_Timezones_plus);

            Shortcuts_Disabling(OutputShortcuts_TimezonesTile, OutputShortcuts_TimezonesTileDISABLED);
        }
        #endregion

        #region Timezones
        private void TimezoneShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(Timezone_InitialHour);
            Shortcuts_SingleFilter(Timezone_FinalHour);

            Shortcuts_MinusToPlus(Timezone_Weekdays_minus, Timezone_Weekdays_plus);

            Shortcuts_Disabling(TimezoneShortcuts_SettingsTile, TimezoneShortcuts_SettingsTileDISABLED);
        }
        private void TimezoneShortcuts_ExceptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Timezone_Exception_1_minus, Timezone_Exception_1_plus);
            Shortcuts_MinusToPlus(Timezone_Exception_2_minus, Timezone_Exception_2_plus);
            Shortcuts_MinusToPlus(Timezone_Exception_3_minus, Timezone_Exception_3_plus);
            Shortcuts_MinusToPlus(Timezone_Exception_4_minus, Timezone_Exception_4_plus);

            Shortcuts_Disabling(TimezoneShortcuts_ExceptionsTile, TimezoneShortcuts_ExceptionsTileDISABLED);
        }
        #endregion

        #region Phones
        private void PhoneShortcuts_SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Phones_ActiveColumn.Visibility = Phones_ActiveColumn.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            prefix_column.Visibility = prefix_column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            phone_numberColumn.Visibility = phone_numberColumn.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            ComAttempts_Column.Visibility = ComAttempts_Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;

            Shortcuts_MinusToPlus(Phones_Area_minus, Phones_Area_plus);
            Shortcuts_MinusToPlus(Phone_KissOff_minus, Phone_KissOff_plus);

            Shortcuts_Disabling(PhoneShortcuts_SettingsTile, PhoneShortcuts_SettingsTileDISABLED);
        }
        private void PhoneShortcuts_ReportsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_MinusToPlus(Phone_Reports_minus, Phone_Reports_plus);

            Shortcuts_Disabling(PhoneShortcuts_ReportsTile, PhoneShortcuts_ReportsTileDISABLED);
        }
        private void PhoneShortcuts_TestsButton_Click(object sender, RoutedEventArgs e)
        {
            Shortcuts_SingleFilter(Phone_TestCalls);
            Shortcuts_SingleFilter(Phone_VoiceCallType);

            Shortcuts_MinusToPlus(Phone_TestConfig_minus, Phone_TestConfig_plus);

            Shortcuts_Disabling(PhoneShortcuts_TestsTile, PhoneShortcuts_TestsTileDISABLED);
        }
        #endregion


        #endregion

        #region HeaderConfigure Clicks

        #region Zones

        private void EntryTimes_minus_Click(object sender, RoutedEventArgs e)
        {
            EntryTimes_minus.Visibility = Visibility.Hidden;
            EntryTimes_plus.Visibility = Visibility.Visible;
        }
        private void EntryTimes_plus_Click(object sender, RoutedEventArgs e)
        {
            EntryTimes_minus.Visibility = Visibility.Visible;
            EntryTimes_plus.Visibility = Visibility.Hidden;
        }

        private void _24H_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            _24H_Config_minus.Visibility = Visibility.Hidden;
            _24H_Config_plus.Visibility = Visibility.Visible;
        }
        private void _24H_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            _24H_Config_minus.Visibility = Visibility.Visible;
            _24H_Config_plus.Visibility = Visibility.Hidden;
        }

        private void Keyswitch_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            Keyswitch_Config_minus.Visibility = Visibility.Hidden;
            Keyswitch_Config_plus.Visibility = Visibility.Visible;
        }
        private void Keyswitch_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            Keyswitch_Config_minus.Visibility = Visibility.Visible;
            Keyswitch_Config_plus.Visibility = Visibility.Hidden;
        }

        private void Bypass_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            Bypass_Config_minus.Visibility = Visibility.Hidden;
            Bypass_Config_plus.Visibility = Visibility.Visible;
        }
        private void Bypass_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            Bypass_Config_minus.Visibility = Visibility.Visible;
            Bypass_Config_plus.Visibility = Visibility.Hidden;
        }

        private void Zone_Type_minus_Click(object sender, RoutedEventArgs e)
        {
            Zone_Type_minus.Visibility = Visibility.Hidden;
            Zone_Type_plus.Visibility = Visibility.Visible;
        }
        private void Zone_Type_plus_Click(object sender, RoutedEventArgs e)
        {
            Zone_Type_minus.Visibility = Visibility.Visible;
            Zone_Type_plus.Visibility = Visibility.Hidden;
        }

        private void Chime_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            Chime_Config_minus.Visibility = Visibility.Hidden;
            Chime_Config_plus.Visibility = Visibility.Visible;
        }
        private void Chime_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            Chime_Config_minus.Visibility = Visibility.Visible;
            Chime_Config_plus.Visibility = Visibility.Hidden;
        }

        private void Zones_Areas_Away_minus_Click(object sender, RoutedEventArgs e)
        {
            Zones_Areas_Away_minus.Visibility = Visibility.Hidden;
            Zones_Areas_Away_plus.Visibility = Visibility.Visible;
        }
        private void Zones_Areas_Away_plus_Click(object sender, RoutedEventArgs e)
        {
            Zones_Areas_Away_minus.Visibility = Visibility.Visible;
            Zones_Areas_Away_plus.Visibility = Visibility.Hidden;
        }

        private void Zones_Areas_Stay_minus_Click(object sender, RoutedEventArgs e)
        {
            Zones_Areas_Stay_minus.Visibility = Visibility.Hidden;
            Zones_Areas_Stay_plus.Visibility = Visibility.Visible;
        }
        private void Zones_Areas_Stay_plus_Click(object sender, RoutedEventArgs e)
        {
            Zones_Areas_Stay_minus.Visibility = Visibility.Visible;
            Zones_Areas_Stay_plus.Visibility = Visibility.Hidden;
        }

        private void Zones_ShowKeypad_minus_Click(object sender, RoutedEventArgs e)
        {
            Zones_ShowKeypad_minus.Visibility = Visibility.Hidden;
            Zones_ShowKeypad_plus.Visibility = Visibility.Visible;
        }
        private void Zones_ShowKeypad_plus_Click(object sender, RoutedEventArgs e)
        {
            Zones_ShowKeypad_minus.Visibility = Visibility.Visible;
            Zones_ShowKeypad_plus.Visibility = Visibility.Hidden;
        }

        private void R1_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            R1_Config_minus.Visibility = Visibility.Hidden;
            R1_Config_plus.Visibility = Visibility.Visible;
        }
        private void R1_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            R1_Config_minus.Visibility = Visibility.Visible;
            R1_Config_plus.Visibility = Visibility.Hidden;
        }

        private void R2_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            R2_Config_minus.Visibility = Visibility.Hidden;
            R2_Config_plus.Visibility = Visibility.Visible;
        }
        private void R2_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            R2_Config_minus.Visibility = Visibility.Visible;
            R2_Config_plus.Visibility = Visibility.Hidden;
        }

        private void R3_Config_minus_Click(object sender, RoutedEventArgs e)
        {
            R3_Config_minus.Visibility = Visibility.Hidden;
            R3_Config_plus.Visibility = Visibility.Visible;
        }
        private void R3_Config_plus_Click(object sender, RoutedEventArgs e)
        {
            R3_Config_minus.Visibility = Visibility.Visible;
            R3_Config_plus.Visibility = Visibility.Hidden;
        }

        private void Zone_Alarm_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Zone_Alarm_Output_minus.Visibility = Visibility.Hidden;
            Zone_Alarm_Output_plus.Visibility = Visibility.Visible;
        }
        private void Zone_Alarm_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Zone_Alarm_Output_minus.Visibility = Visibility.Visible;
            Zone_Alarm_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Chime_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Chime_Output_minus.Visibility = Visibility.Hidden;
            Chime_Output_plus.Visibility = Visibility.Visible;
        }
        private void Chime_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Chime_Output_minus.Visibility = Visibility.Visible;
            Chime_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Sensor_Watch_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Sensor_Watch_Output_minus.Visibility = Visibility.Hidden;
            Sensor_Watch_Output_plus.Visibility = Visibility.Visible;
        }
        private void Sensor_Watch_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Sensor_Watch_Output_minus.Visibility = Visibility.Visible;
            Sensor_Watch_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Entry_Time_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Entry_Time_Output_minus.Visibility = Visibility.Hidden;
            Entry_Time_Output_plus.Visibility = Visibility.Visible;
        }
        private void Entry_Time_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Entry_Time_Output_minus.Visibility = Visibility.Visible;
            Entry_Time_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Anti_Mask_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Anti_Mask_Output_minus.Visibility = Visibility.Hidden;
            Anti_Mask_Output_plus.Visibility = Visibility.Visible;
        }
        private void Anti_Mask_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Anti_Mask_Output_minus.Visibility = Visibility.Visible;
            Anti_Mask_Output_plus.Visibility = Visibility.Hidden;
        }

        private void _24H_Alarm_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            _24H_Alarm_Output_minus.Visibility = Visibility.Hidden;
            _24H_Alarm_Output_plus.Visibility = Visibility.Visible;
        }
        private void _24H_Alarm_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            _24H_Alarm_Output_minus.Visibility = Visibility.Visible;
            _24H_Alarm_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Fire_Alarm_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Fire_Alarm_Output_minus.Visibility = Visibility.Hidden;
            Fire_Alarm_Output_plus.Visibility = Visibility.Visible;
        }
        private void Fire_Alarm_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Fire_Alarm_Output_minus.Visibility = Visibility.Visible;
            Fire_Alarm_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Tamper_Alarm_Output_minus_Click(object sender, RoutedEventArgs e)
        {
            Tamper_Alarm_Output_minus.Visibility = Visibility.Hidden;
            Tamper_Alarm_Output_plus.Visibility = Visibility.Visible;
        }
        private void Tamper_Alarm_Output_plus_Click(object sender, RoutedEventArgs e)
        {
            Tamper_Alarm_Output_minus.Visibility = Visibility.Visible;
            Tamper_Alarm_Output_plus.Visibility = Visibility.Hidden;
        }

        private void Zone_Audio_minus_Click(object sender, RoutedEventArgs e)
        {
            Zone_Audio_minus.Visibility = Visibility.Hidden;
            Zone_Audio_plus.Visibility = Visibility.Visible;
        }
        private void Zone_Audio_plus_Click(object sender, RoutedEventArgs e)
        {
            Zone_Audio_minus.Visibility = Visibility.Visible;
            Zone_Audio_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Areas

        private void Area_Code_Required_minus_Click(object sender, RoutedEventArgs e)
        {
            Area_Code_Required_minus.Visibility = Visibility.Hidden;
            Area_Code_Required_plus.Visibility = Visibility.Visible;
        }
        private void Area_Code_Required_plus_Click(object sender, RoutedEventArgs e)
        {
            Area_Code_Required_minus.Visibility = Visibility.Visible;
            Area_Code_Required_plus.Visibility = Visibility.Hidden;
        }

        private void Exit_Times_minus_Click(object sender, RoutedEventArgs e)
        {
            Exit_Times_minus.Visibility = Visibility.Hidden;
            Exit_Times_plus.Visibility = Visibility.Visible;
        }
        private void Exit_Times_plus_Click(object sender, RoutedEventArgs e)
        {
            Exit_Times_minus.Visibility = Visibility.Visible;
            Exit_Times_plus.Visibility = Visibility.Hidden;
        }

        private void Timezones_Start_Arm_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezones_Start_Arm_minus.Visibility = Visibility.Hidden;
            Timezones_Start_Arm_plus.Visibility = Visibility.Visible;
        }
        private void Timezones_Start_Arm_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezones_Start_Arm_minus.Visibility = Visibility.Visible;
            Timezones_Start_Arm_plus.Visibility = Visibility.Hidden;
        }

        private void Timezones_End_Disarm_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezones_End_Disarm_minus.Visibility = Visibility.Hidden;
            Timezones_End_Disarm_plus.Visibility = Visibility.Visible;
        }
        private void Timezones_End_Disarm_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezones_End_Disarm_minus.Visibility = Visibility.Visible;
            Timezones_End_Disarm_plus.Visibility = Visibility.Hidden;
        }

        private void Arm_Away_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Away_Outputs_minus.Visibility = Visibility.Hidden;
            Arm_Away_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Arm_Away_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Away_Outputs_minus.Visibility = Visibility.Visible;
            Arm_Away_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Arm_Stay_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Stay_Outputs_minus.Visibility = Visibility.Hidden;
            Arm_Stay_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Arm_Stay_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Stay_Outputs_minus.Visibility = Visibility.Visible;
            Arm_Stay_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Disarm_Away_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Away_Outputs_minus.Visibility = Visibility.Hidden;
            Disarm_Away_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Disarm_Away_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Away_Outputs_minus.Visibility = Visibility.Visible;
            Disarm_Away_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Disarm_Stay_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Stay_Outputs_minus.Visibility = Visibility.Hidden;
            Disarm_Stay_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Disarm_Stay_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Stay_Outputs_minus.Visibility = Visibility.Visible;
            Disarm_Stay_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Arm_Away_Pulsed_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Away_Pulsed_Outputs_minus.Visibility = Visibility.Hidden;
            Arm_Away_Pulsed_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Arm_Away_Pulsed_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Away_Pulsed_Outputs_minus.Visibility = Visibility.Visible;
            Arm_Away_Pulsed_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Arm_Stay_Pulsed_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Stay_Pulsed_Outputs_minus.Visibility = Visibility.Hidden;
            Arm_Stay_Pulsed_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Arm_Stay_Pulsed_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Stay_Pulsed_Outputs_minus.Visibility = Visibility.Visible;
            Arm_Stay_Pulsed_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Disarm_Away_Pulsed_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Away_Pulsed_Outputs_minus.Visibility = Visibility.Hidden;
            Disarm_Away_Pulsed_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Disarm_Away_Pulsed_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Away_Pulsed_Outputs_minus.Visibility = Visibility.Visible;
            Disarm_Away_Pulsed_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Disarm_Stay_Pulsed_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Stay_Pulsed_Outputs_minus.Visibility = Visibility.Hidden;
            Disarm_Stay_Pulsed_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Disarm_Stay_Pulsed_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Stay_Pulsed_Outputs_minus.Visibility = Visibility.Visible;
            Disarm_Stay_Pulsed_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Arm_Away_Beeps_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Away_Beeps_Outputs_minus.Visibility = Visibility.Hidden;
            Arm_Away_Beeps_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Arm_Away_Beeps_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Away_Beeps_Outputs_minus.Visibility = Visibility.Visible;
            Arm_Away_Beeps_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Arm_Stay_Beeps_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Stay_Beeps_Outputs_minus.Visibility = Visibility.Hidden;
            Arm_Stay_Beeps_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Arm_Stay_Beeps_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Arm_Stay_Beeps_Outputs_minus.Visibility = Visibility.Visible;
            Arm_Stay_Beeps_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Disarm_Away_Beeps_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Away_Beeps_Outputs_minus.Visibility = Visibility.Hidden;
            Disarm_Away_Beeps_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Disarm_Away_Beeps_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Away_Beeps_Outputs_minus.Visibility = Visibility.Visible;
            Disarm_Away_Beeps_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Disarm_Stay_Beeps_Outputs_minus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Stay_Beeps_Outputs_minus.Visibility = Visibility.Hidden;
            Disarm_Stay_Beeps_Outputs_plus.Visibility = Visibility.Visible;
        }
        private void Disarm_Stay_Beeps_Outputs_plus_Click(object sender, RoutedEventArgs e)
        {
            Disarm_Stay_Beeps_Outputs_minus.Visibility = Visibility.Visible;
            Disarm_Stay_Beeps_Outputs_plus.Visibility = Visibility.Hidden;
        }

        private void Area_Audio_minus_Click(object sender, RoutedEventArgs e)
        {
            Area_Audio_minus.Visibility = Visibility.Hidden;
            Area_Audio_plus.Visibility = Visibility.Visible;
        }
        private void Area_Audio_plus_Click(object sender, RoutedEventArgs e)
        {
            Area_Audio_minus.Visibility = Visibility.Visible;
            Area_Audio_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Keypads
        private void Keypad_Areas_minus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_Areas_minus.Visibility = Visibility.Hidden;
            Keypad_Areas_plus.Visibility = Visibility.Visible;
        }
        private void Keypad_Areas_plus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_Areas_minus.Visibility = Visibility.Visible;
            Keypad_Areas_plus.Visibility = Visibility.Hidden;
        }

        private void Keypad_Beeps_minus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_Beeps_minus.Visibility = Visibility.Hidden;
            Keypad_Beeps_plus.Visibility = Visibility.Visible;
        }
        private void Keypad_Beeps_plus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_Beeps_minus.Visibility = Visibility.Visible;
            Keypad_Beeps_plus.Visibility = Visibility.Hidden;
        }

        private void Keypad_ArmDisarm_Permissions_minus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_ArmDisarm_Permissions_minus.Visibility = Visibility.Hidden;
            Keypad_ArmDisarm_Permissions_plus.Visibility = Visibility.Visible;
        }
        private void Keypad_ArmDisarm_Permissions_plus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_ArmDisarm_Permissions_minus.Visibility = Visibility.Visible;
            Keypad_ArmDisarm_Permissions_plus.Visibility = Visibility.Hidden;
        }

        private void Fbuttons_minus_Click(object sender, RoutedEventArgs e)
        {
            Fbuttons_minus.Visibility = Visibility.Hidden;
            Fbuttons_plus.Visibility = Visibility.Visible;
        }
        private void Fbuttons_plus_Click(object sender, RoutedEventArgs e)
        {
            Fbuttons_minus.Visibility = Visibility.Visible;
            Fbuttons_plus.Visibility = Visibility.Hidden;
        }

        private void Keypad_Audio_minus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_Audio_minus.Visibility = Visibility.Hidden;
            Keypad_Audio_plus.Visibility = Visibility.Visible;
        }
        private void Keypad_Audio_plus_Click(object sender, RoutedEventArgs e)
        {
            Keypad_Audio_minus.Visibility = Visibility.Visible;
            Keypad_Audio_plus.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Expanders
        private void Expander_ConfigPins_minus_Click(object sender, RoutedEventArgs e)
        {
            Expander_ConfigPins_minus.Visibility = Visibility.Hidden;
            Expander_ConfigPins_plus.Visibility = Visibility.Visible;
        }
        private void Expander_ConfigPins_plus_Click(object sender, RoutedEventArgs e)
        {
            Expander_ConfigPins_minus.Visibility = Visibility.Visible;
            Expander_ConfigPins_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Outputs

        private void TimeConfig_minus_Click(object sender, RoutedEventArgs e)
        {
            TimeConfig_minus.Visibility = Visibility.Hidden;
            TimeConfig_plus.Visibility = Visibility.Visible;
        }
        private void TimeConfig_plus_Click(object sender, RoutedEventArgs e)
        {
            TimeConfig_minus.Visibility = Visibility.Visible;
            TimeConfig_plus.Visibility = Visibility.Hidden;
        }

        private void Output_ChimeConfig_minus_Click(object sender, RoutedEventArgs e)
        {
            Output_ChimeConfig_minus.Visibility = Visibility.Hidden;
            Output_ChimeConfig_plus.Visibility = Visibility.Visible;
        }
        private void Output_ChimeConfig_plus_Click(object sender, RoutedEventArgs e)
        {
            Output_ChimeConfig_minus.Visibility = Visibility.Visible;
            Output_ChimeConfig_plus.Visibility = Visibility.Hidden;
        }

        private void Output_PulseConfig_minus_Click(object sender, RoutedEventArgs e)
        {
            Output_PulseConfig_minus.Visibility = Visibility.Hidden;
            Output_PulseConfig_plus.Visibility = Visibility.Visible;
        }
        private void Output_PulseConfig_plus_Click(object sender, RoutedEventArgs e)
        {
            Output_PulseConfig_minus.Visibility = Visibility.Visible;
            Output_PulseConfig_plus.Visibility = Visibility.Hidden;
        }

        private void Output_Beeps_minus_Click(object sender, RoutedEventArgs e)
        {
            Output_Beeps_minus.Visibility = Visibility.Hidden;
            Output_Beeps_plus.Visibility = Visibility.Visible;
        }
        private void Output_Beeps_plus_Click(object sender, RoutedEventArgs e)
        {
            Output_Beeps_minus.Visibility = Visibility.Visible;
            Output_Beeps_plus.Visibility = Visibility.Hidden;
        }

        private void Output_Timezones_minus_Click(object sender, RoutedEventArgs e)
        {
            Output_Timezones_minus.Visibility = Visibility.Hidden;
            Output_Timezones_plus.Visibility = Visibility.Visible;
        }
        private void Output_Timezones_plus_Click(object sender, RoutedEventArgs e)
        {
            Output_Timezones_minus.Visibility = Visibility.Visible;
            Output_Timezones_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Users

        private void User_ArmDisarm_Permissions_minus_Click(object sender, RoutedEventArgs e)
        {
            User_ArmDisarm_Permissions_minus.Visibility = Visibility.Hidden;
            User_ArmDisarm_Permissions_plus.Visibility = Visibility.Visible;
        }
        private void User_ArmDisarm_Permissions_plus_Click(object sender, RoutedEventArgs e)
        {
            User_ArmDisarm_Permissions_minus.Visibility = Visibility.Visible;
            User_ArmDisarm_Permissions_plus.Visibility = Visibility.Hidden;
        }

        private void User_Area_Permissions_minus_Click(object sender, RoutedEventArgs e)
        {
            User_Area_Permissions_minus.Visibility = Visibility.Hidden;
            User_Area_Permissions_plus.Visibility = Visibility.Visible;
        }
        private void User_Area_Permissions_plus_Click(object sender, RoutedEventArgs e)
        {
            User_Area_Permissions_minus.Visibility = Visibility.Visible;
            User_Area_Permissions_plus.Visibility = Visibility.Hidden;
        }

        private void User_Output_Permissions_minus_Click(object sender, RoutedEventArgs e)
        {
            User_Output_Permissions_minus.Visibility = Visibility.Hidden;
            User_Output_Permissions_plus.Visibility = Visibility.Visible;
        }
        private void User_Output_Permissions_plus_Click(object sender, RoutedEventArgs e)
        {
            User_Output_Permissions_minus.Visibility = Visibility.Visible;
            User_Output_Permissions_plus.Visibility = Visibility.Hidden;
        }

        private void User_Timezone_Permissions_minus_Click(object sender, RoutedEventArgs e)
        {
            User_Timezone_Permissions_minus.Visibility = Visibility.Hidden;
            User_Timezone_Permissions_plus.Visibility = Visibility.Visible;
        }
        private void User_Timezone_Permissions_plus_Click(object sender, RoutedEventArgs e)
        {
            User_Timezone_Permissions_minus.Visibility = Visibility.Visible;
            User_Timezone_Permissions_plus.Visibility = Visibility.Hidden;
        }

        private void Button_A_AreaAway_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_A_AreaAway_minus.Visibility = Visibility.Hidden;
            Button_A_AreaAway_plus.Visibility = Visibility.Visible;
        }
        private void Button_A_AreaAway_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_A_AreaAway_minus.Visibility = Visibility.Visible;
            Button_A_AreaAway_plus.Visibility = Visibility.Hidden;
        }

        private void Button_A_AreaStay_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_A_AreaStay_minus.Visibility = Visibility.Hidden;
            Button_A_AreaStay_plus.Visibility = Visibility.Visible;
        }
        private void Button_A_AreaStay_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_A_AreaStay_minus.Visibility = Visibility.Visible;
            Button_A_AreaStay_plus.Visibility = Visibility.Hidden;
        }

        private void Button_B_AreaAway_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_B_AreaAway_minus.Visibility = Visibility.Hidden;
            Button_B_AreaAway_plus.Visibility = Visibility.Visible;
        }
        private void Button_B_AreaAway_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_B_AreaAway_minus.Visibility = Visibility.Visible;
            Button_B_AreaAway_plus.Visibility = Visibility.Hidden;
        }

        private void Button_B_AreaStay_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_B_AreaStay_minus.Visibility = Visibility.Hidden;
            Button_B_AreaStay_plus.Visibility = Visibility.Visible;
        }
        private void Button_B_AreaStay_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_B_AreaStay_minus.Visibility = Visibility.Visible;
            Button_B_AreaStay_plus.Visibility = Visibility.Hidden;
        }

        private void Button_C_AreaAway_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_C_AreaAway_minus.Visibility = Visibility.Hidden;
            Button_C_AreaAway_plus.Visibility = Visibility.Visible;
        }
        private void Button_C_AreaAway_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_C_AreaAway_minus.Visibility = Visibility.Visible;
            Button_C_AreaAway_plus.Visibility = Visibility.Hidden;
        }

        private void Button_C_AreaStay_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_C_AreaStay_minus.Visibility = Visibility.Hidden;
            Button_C_AreaStay_plus.Visibility = Visibility.Visible;
        }
        private void Button_C_AreaStay_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_C_AreaStay_minus.Visibility = Visibility.Visible;
            Button_C_AreaStay_plus.Visibility = Visibility.Hidden;
        }

        private void Button_D_AreaAway_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_D_AreaAway_minus.Visibility = Visibility.Hidden;
            Button_D_AreaAway_plus.Visibility = Visibility.Visible;
        }
        private void Button_D_AreaAway_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_D_AreaAway_minus.Visibility = Visibility.Visible;
            Button_D_AreaAway_plus.Visibility = Visibility.Hidden;
        }

        private void Button_D_AreaStay_minus_Click(object sender, RoutedEventArgs e)
        {
            Button_D_AreaStay_minus.Visibility = Visibility.Hidden;
            Button_D_AreaStay_plus.Visibility = Visibility.Visible;
        }
        private void Button_D_AreaStay_plus_Click(object sender, RoutedEventArgs e)
        {
            Button_D_AreaStay_minus.Visibility = Visibility.Visible;
            Button_D_AreaStay_plus.Visibility = Visibility.Hidden;
        }

        private void User_Audio_minus_Click(object sender, RoutedEventArgs e)
        {
            User_Audio_minus.Visibility = Visibility.Hidden;
            User_Audio_plus.Visibility = Visibility.Visible;
        }
        private void User_Audio_plus_Click(object sender, RoutedEventArgs e)
        {
            User_Audio_minus.Visibility = Visibility.Visible;
            User_Audio_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Timezones

        private void Timezone_Weekdays_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Weekdays_minus.Visibility = Visibility.Hidden;
            Timezone_Weekdays_plus.Visibility = Visibility.Visible;
        }
        private void Timezone_Weekdays_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Weekdays_minus.Visibility = Visibility.Visible;
            Timezone_Weekdays_plus.Visibility = Visibility.Hidden;
        }

        private void Timezone_Exception_1_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_1_minus.Visibility = Visibility.Hidden;
            Timezone_Exception_1_plus.Visibility = Visibility.Visible;
        }
        private void Timezone_Exception_1_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_1_minus.Visibility = Visibility.Visible;
            Timezone_Exception_1_plus.Visibility = Visibility.Hidden;
        }

        private void Timezone_Exception_2_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_2_minus.Visibility = Visibility.Hidden;
            Timezone_Exception_2_plus.Visibility = Visibility.Visible;
        }
        private void Timezone_Exception_2_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_2_minus.Visibility = Visibility.Visible;
            Timezone_Exception_2_plus.Visibility = Visibility.Hidden;
        }

        private void Timezone_Exception_3_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_3_minus.Visibility = Visibility.Hidden;
            Timezone_Exception_3_plus.Visibility = Visibility.Visible;
        }
        private void Timezone_Exception_3_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_3_minus.Visibility = Visibility.Visible;
            Timezone_Exception_3_plus.Visibility = Visibility.Hidden;
        }

        private void Timezone_Exception_4_minus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_4_minus.Visibility = Visibility.Hidden;
            Timezone_Exception_4_plus.Visibility = Visibility.Visible;
        }
        private void Timezone_Exception_4_plus_Click(object sender, RoutedEventArgs e)
        {
            Timezone_Exception_4_minus.Visibility = Visibility.Visible;
            Timezone_Exception_4_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Phones
        private void Phones_Area_minus_Click(object sender, RoutedEventArgs e)
        {
            Phones_Area_minus.Visibility = Visibility.Hidden;
            Phones_Area_plus.Visibility = Visibility.Visible;
        }
        private void Phones_Area_plus_Click(object sender, RoutedEventArgs e)
        {
            Phones_Area_minus.Visibility = Visibility.Visible;
            Phones_Area_plus.Visibility = Visibility.Hidden;
        }

        private void Phone_KissOff_minus_Click(object sender, RoutedEventArgs e)
        {
            Phone_KissOff_minus.Visibility = Visibility.Hidden;
            Phone_KissOff_plus.Visibility = Visibility.Visible;
        }
        private void Phone_KissOff_plus_Click(object sender, RoutedEventArgs e)
        {
            Phone_KissOff_minus.Visibility = Visibility.Visible;
            Phone_KissOff_plus.Visibility = Visibility.Hidden;
        }

        private void Phone_Reports_minus_Click(object sender, RoutedEventArgs e)
        {
            Phone_Reports_minus.Visibility = Visibility.Hidden;
            Phone_Reports_plus.Visibility = Visibility.Visible;
        }
        private void Phone_Reports_plus_Click(object sender, RoutedEventArgs e)
        {
            Phone_Reports_minus.Visibility = Visibility.Visible;
            Phone_Reports_plus.Visibility = Visibility.Hidden;
        }

        private void Phone_TestConfig_minus_Click(object sender, RoutedEventArgs e)
        {
            Phone_TestConfig_minus.Visibility = Visibility.Hidden;
            Phone_TestConfig_plus.Visibility = Visibility.Visible;
        }
        private void Phone_TestConfig_plus_Click(object sender, RoutedEventArgs e)
        {
            Phone_TestConfig_minus.Visibility = Visibility.Visible;
            Phone_TestConfig_plus.Visibility = Visibility.Hidden;
        }

        #endregion

        #region Audio

        #endregion

        #region DoubleClick to avoid Bugs

        private void Open_Areas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Areas_Click(sender, e);
        }
        private void Open_Zones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Zones_Click(sender, e);
        }
        private void Open_Keypads_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Keypads_Click(sender, e);
        }
        private void Open_Expanders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Expanders_Click(sender, e);
        }
        private void Open_Outputs_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Outputs_Click(sender, e);
        }
        private void Open_Timezones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Timezones_Click(sender, e);
        }
        private void Open_Users_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Users_Click(sender, e);
        }
        private void Open_Phones_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Phones_Click(sender, e);
        }
        private void Open_Dialer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Dialer_Click(sender, e);
        }
        private void Open_ClientInfo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_ClientInfo_Click(sender, e);
        }
        private void Open_Events_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Events_Click(sender, e);
        }
        private void Open_Audio_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Audio_Click(sender, e);
        }
        private void Open_Status_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Status_Click(sender, e);
        }
        private void Open_Debug_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Debug_Click(sender, e);
        }

        private void Open_Memory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_Memory_Click(sender, e);
        }


        private void Open_FWUpdate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_FWUpdate_Click(sender, e);
        }
        private void Open_GlobalConfig_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Open_GlobalConfig_Click(sender, e);
        }
        #endregion

        #endregion

        private void dialerDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void PartitionComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int id = PartitionComboBox.SelectedIndex;
            string clearfilter = string.Empty;
            DataTable dtEvent = new DataTable();
            dtEvent = databaseDataSet.Event;

            switch (id)
            {
                case 0://All Partitions
                    dtEvent.DefaultView.RowFilter = clearfilter;
                    break;

                case 1: //partition 1
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "1" + ")";
                    break;

                case 2://partition 2
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "2" + ")";
                    break;

                case 3://partition 3
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "3" + ")";
                    break;

                case 4://partition 4
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "4" + ")";
                    break;

                case 5://partition 5
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "5" + ")";
                    break;

                case 6://partition 6
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "6" + ")";
                    break;

                case 7://partition 7
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "7" + ")";
                    break;

                case 8://partition 8
                    dtEvent.DefaultView.RowFilter = databaseDataSet.Event.AreaColumn.ColumnName + " IN(" + "8" + ")";
                    break;

                default:
                    break;
            }

        }

        private void HelpFlyoutImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //HelpFlyout.IsOpen = false;
        }

        private void FlyComImage_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FlyCom.IsOpen = false;
        }

        private void AudioCustomButtonAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        public void DebugTable(DataTable table)
        {
            Debug.WriteLine("--- DebugTable(" + table.TableName + ") ---");
            int zeilen = table.Rows.Count;
            int spalten = table.Columns.Count;

            // Header
            for (int i = 0; i < table.Columns.Count; i++)
            {
                string s = table.Columns[i].ToString();
                Debug.Write(String.Format("{0,-20} | ", s));
            }
            Debug.Write(Environment.NewLine);
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Debug.Write("---------------------|-");
            }
            Debug.Write(Environment.NewLine);

            // Data
            for (int i = 0; i < zeilen; i++)
            {
                DataRow row = table.Rows[i];
                //Debug.WriteLine("{0} {1} ", row[0], row[1]);
                for (int j = 0; j < spalten; j++)
                {
                    string s = row[j].ToString();
                    if (s.Length > 20) s = s.Substring(0, 17) + "...";
                    Debug.Write(String.Format("{0,-20} | ", s));
                }
                Debug.Write(Environment.NewLine);
            }
            for (int i = 0; i < table.Columns.Count; i++)
            {
                Debug.Write("---------------------|-");
            }
            Debug.Write(Environment.NewLine);
        }

        private void ButtonPlayAudioConfig_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseFile_Click(object sender, RoutedEventArgs e)
        {
            var filemanagerwindow = new FileManager(AppLocale, AppRole, this);
            this.Close();
            filemanagerwindow.Show();
        }

        private void SettingsTile_Click(object sender, RoutedEventArgs e)
        {
            var preferences_window = new Settings(AppLocale, this, AppDbFile, default_restore_is_set, AppRole);
            preferences_window.Show();
        }

        #region Only Active Filter

        private void OnlyActiveClick(bool active, DataGrid dg, DataTable dt, Tile tile, string columnActiveName)
        {
            if (active)
            {
                dg.ItemsSource = new DataView(dt);
                DebugTable(dt);

                tile.Background = Brushes.WhiteSmoke;
                active = false;
            }
            else
            {
                dg.ItemsSource = new DataView(dt.AsEnumerable().Where(row => row.Field<bool>(columnActiveName) == true).CopyToDataTable());
                DebugTable(dt.AsEnumerable().Where(row => row.Field<bool>(columnActiveName) == true).CopyToDataTable());

                tile.Background = Brushes.DarkSeaGreen;
                active = true;
            }
        }



        #endregion

        public void ValidatePassword(string password)
        {
            string patternPassword = @"^(?=.*\d).{4,8}$";
            if (!string.IsNullOrEmpty(password))
            {
                if (!Regex.IsMatch(password, patternPassword))
                {
                    MessageBox.Show(" Password must be at least 4 characters and no more than 8 characters");
                }
            }
        }

        private void UserCodePasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PasswordBox pw = (PasswordBox)sender;
            //@"\d{4}$"
            Regex regex = new Regex(@"^([0-9]){4,8}$");
            //Regex regex = new Regex(@"^(?=.*[0-9]).{4,8}$");
            if (regex.IsMatch(pw.Password))
                e.Handled = true;
            else
            {
                e.Handled = false;
                MessageBox.Show("Password must be at least 4 characters and no more than 8 characters");
                pw.Password = "\0";
            }
        }

        public static byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            if (NumberChars % 2 != 0)
                NumberChars++;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }

        private async void ReadMemory_button_Click(object sender, RoutedEventArgs e)
        {
            Protocol.Memory memory = new Protocol.Memory();

            string path_memory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\memory";
            string path_dataRX_file = path_memory + "\\dataRX_" + DateTime.Now.ToString("yyMMdd-HHmm") + ".txt";

            if (!Directory.Exists(path_memory))
                Directory.CreateDirectory(path_memory);

            if (!File.Exists(path_dataRX_file))
                File.Create(path_dataRX_file);

            string addr_init = Address1_textbox.Text;
            string addr_final = Address2_textbox.Text;
            string DataRX_HEX;

            byte[] addr1_byte = StringToByteArray(addr_init.ToString().PadLeft(6, '0'));
            byte[] addr2_byte = StringToByteArray(addr_final.ToString().PadLeft(6, '0'));

            uint addr1 = uint.Parse(addr_init, System.Globalization.NumberStyles.HexNumber);//Convert.ToUInt32(addr1_byte); //
            uint addr2 = uint.Parse(addr_final, System.Globalization.NumberStyles.HexNumber);//Convert.ToUInt32(addr2_byte); //uint.Parse(addr_final, System.Globalization.NumberStyles.HexNumber);

            int msg_size = 235;
            int size = (int)addr2 - (int)addr1;
            int max_msg = size / msg_size;
            
            byte[] DataRX = new byte[size];

            var controller = await this.ShowProgressAsync(Properties.Resources.PleaseWait, "");
            controller.Maximum = 100.0;
            controller.Minimum = 0.0;
           
            await Task.Run(() =>
            {
                //int error_cnt = 0;
                for (int i = 0; i < max_msg; i++)
                {
                    onlyDebug = true;
                    if (!RX_ACK)
                    { 
                        if (i == 0)
                            i = 0;
                        else i--;
                    }

                    Dispatcher.Invoke(() => memory.read(this, (uint)i, addr1));
                    System.Threading.Thread.Sleep(intervalsleeptime);
                    System.Buffer.BlockCopy(rx_buffer, 10, DataRX, i * msg_size, msg_size);
                    System.Threading.Thread.Sleep(intervalsleeptime);
                    controller.SetProgress(i * (100 / size));

                }
                onlyDebug = false;
                controller.CloseAsync();
            });

            if (ASCII_HEX_ComboBox.SelectedIndex == 1) //ASCII
            {
                DataRX_HEX = ByteArrayToString(DataRX, size, 1);
                if(RX_ACK)
                    File.WriteAllText(path_dataRX_file, DataRX_HEX);
            }
            else 
                if(RX_ACK)
                    File.WriteAllBytes(path_dataRX_file, DataRX);

            DataRX_info.Visibility = Visibility.Visible;
            DataRX_savedfile.Content = path_dataRX_file;
            DataRX_savedfile.Visibility = Visibility.Visible;

            MemoryContentRX.Text = BitConverter.ToString(DataRX);
            await DialogManager.ShowMessageAsync(this, Properties.Resources.ReadWithSuccess, "");
        }

        //void ConvertToHEX(byte[] buf, byte[] buf2, int size)
        //{
        //    for (int i = 0; i < size; i++)
        //    {
        //        if ((buf&0x0f)>9)
        //        {

        //        }
        //        else

                    
        //    }
        //}

    }

    public static class PasswordBoxAssistant
    {
        public static readonly DependencyProperty BoundPassword =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(string), typeof(PasswordBoxAssistant), new PropertyMetadata(string.Empty, OnBoundPasswordChanged));

        public static readonly DependencyProperty BindPassword = DependencyProperty.RegisterAttached(
            "BindPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false, OnBindPasswordChanged));

        public static readonly DependencyProperty UpdatingPassword =
            DependencyProperty.RegisterAttached("UpdatingPassword", typeof(bool), typeof(PasswordBoxAssistant), new PropertyMetadata(false));

        public static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox box = d as PasswordBox;

            // only handle this event when the property is attached to a PasswordBox
            // and when the BindPassword attached property has been set to true
            if (d == null || !GetBindPassword(d))
            {
                return;
            }

            // avoid recursive updating by ignoring the box's changed event
            box.PasswordChanged -= HandlePasswordChanged;

            string newPassword = (string)e.NewValue;

            if (!GetUpdatingPassword(box))
            {
                box.Password = newPassword;
            }

            box.PasswordChanged += HandlePasswordChanged;
        }

        public static void OnBindPasswordChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            // when the BindPassword attached property is set on a PasswordBox,
            // start listening to its PasswordChanged event

            PasswordBox box = dp as PasswordBox;

            if (box == null)
            {
                return;
            }

            bool wasBound = (bool)(e.OldValue);
            bool needToBind = (bool)(e.NewValue);

            if (wasBound)
            {
                box.PasswordChanged -= HandlePasswordChanged;
            }

            if (needToBind)
            {
                box.PasswordChanged += HandlePasswordChanged;
            }
        }

        public static void HandlePasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox box = sender as PasswordBox;

            // set a flag to indicate that we're updating the password
            SetUpdatingPassword(box, true);
            // push the new password into the BoundPassword property
            SetBoundPassword(box, box.Password);
            SetUpdatingPassword(box, false);
        }

        public static void SetBindPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(BindPassword, value);
        }

        public static bool GetBindPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(BindPassword);
        }

        public static string GetBoundPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(BoundPassword);
        }

        public static void SetBoundPassword(DependencyObject dp, string value)
        {
            dp.SetValue(BoundPassword, value);
        }

        public static bool GetUpdatingPassword(DependencyObject dp)
        {
            return (bool)dp.GetValue(UpdatingPassword);
        }

        public static void SetUpdatingPassword(DependencyObject dp, bool value)
        {
            dp.SetValue(UpdatingPassword, value);
        }
    }
}

