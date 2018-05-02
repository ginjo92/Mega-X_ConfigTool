using MahApps.Metro.Controls;
using ProdigyConfigToolWPF.defaultDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Data.SQLite;
using System.ComponentModel;
using ProdigyConfigToolWPF.SqliteLoginDataSetTableAdapters;
using System.Data.SQLite;
using System.Collections;
using System.Windows.Data;
using MahApps.Metro.Controls.Dialogs;
using System.Data;

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : MetroWindow
    {
        public string AppDbFile;
        private string locale;
        private MainWindow mainWindow;
        private string version;
        private int language = 0;
        Boolean restore = false;

        public defaultDataSet databaseDataSet { get; set; }

        public Preferences(string ChoosenLocale, MainWindow main, string ChoosenDbFile, Boolean default_restore_is_set)
        {
            restore = default_restore_is_set;
            locale = ChoosenLocale;
            mainWindow = main;
            version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            AppDbFile = ChoosenDbFile;
            mainWindow.IsEnabled = false;

            SoftwareVersion2.Content = version;

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Properties.Settings.Default.DefaultCulture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.DefaultCulture);
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(locale);

           
            XmlLanguageConverter c = new XmlLanguageConverter();
            XmlLanguage c2 = (XmlLanguage)c.ConvertFrom(System.Threading.Thread.CurrentThread.CurrentUICulture.ToString());
            this.Resources.Add("GetSystemCulture", c2);
            
            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version + "\\"; //My documents folder
            QueriesTableAdapter("attachdbfilename =" + configurations_folder + ChoosenDbFile + "; data source = " + configurations_folder + ChoosenDbFile);

            this.Language = XmlLanguage.GetLanguage(
                       Properties.Settings.Default.DefaultCulture);

            try
            {
                InitializeComponent();
                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); //TODO: delete/improve
            }
            
            switch (Properties.Settings.Default.DefaultCulture)
            {
                case "EN-US":
                    InitializeComponent();
                    language = 0;
                    ChoseLanguageToChangeFlags(language);
                    break;

                case "PT-PT":
                    InitializeComponent();
                    language = 1;
                    ChoseLanguageToChangeFlags(language);
                    break;

                default:
                    InitializeComponent();
                    language = 0;
                    ChoseLanguageToChangeFlags(language);
                    break;
            }

        }

        private async void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("PT-PT"))
                language = 0;
            else if (Thread.CurrentThread.CurrentCulture.Name.Equals("EN-US"))
                language = 1;

            try
            {
                //Dataset data
                //Initialize all data structures TODO: Put it in a function
                defaultDataSet databaseDataSet = ((defaultDataSet)(this.FindResource("databaseDataSet")));

                // ZONE
                ZoneTableAdapter databaseDataSetZoneTableAdapter = new ZoneTableAdapter();
                databaseDataSetZoneTableAdapter.Fill(databaseDataSet.Zone);
                CollectionViewSource zoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("zoneViewSource")));
                zoneViewSource.View.MoveCurrentToFirst();

                // AREA
                AreaTableAdapter databaseDataSetAreaTableAdapter = new AreaTableAdapter();
                databaseDataSetAreaTableAdapter.Fill(databaseDataSet.Area);
                CollectionViewSource AreaViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("areaViewSource")));
                AreaViewSource.View.MoveCurrentToFirst();

                // USER
                UserTableAdapter databaseDataSetUserTableAdapter = new UserTableAdapter();
                databaseDataSetUserTableAdapter.Fill(databaseDataSet.User);
                CollectionViewSource userViewSource = ((CollectionViewSource)(this.FindResource("userViewSource")));
                userViewSource.View.MoveCurrentToFirst();

                // KEYPAD
                KeypadTableAdapter databaseDataSetKeypadTableAdapter = new KeypadTableAdapter();
                databaseDataSetKeypadTableAdapter.Fill(databaseDataSet.Keypad);
                CollectionViewSource keypadViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("keypadViewSource")));
                keypadViewSource.View.MoveCurrentToFirst();

                // OUTPUT
                OutputTableAdapter databaseDataSetOutputTableAdapter = new OutputTableAdapter();
                databaseDataSetOutputTableAdapter.Fill(databaseDataSet.Output);
                CollectionViewSource outputViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("outputViewSource")));
                outputViewSource.View.MoveCurrentToFirst();

                // TIMEZONE
                TimezoneTableAdapter databaseDataSetTimezoneTableAdapter = new TimezoneTableAdapter();
                databaseDataSetTimezoneTableAdapter.Fill(databaseDataSet.Timezone);
                CollectionViewSource timezoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("timezoneViewSource")));
                timezoneViewSource.View.MoveCurrentToFirst();

                // PHONE
                PhoneTableAdapter databaseDataSetPhoneTableAdapter = new PhoneTableAdapter();
                databaseDataSetPhoneTableAdapter.Fill(databaseDataSet.Phone);
                CollectionViewSource phoneViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("phoneViewSource")));
                phoneViewSource.View.MoveCurrentToFirst();

                // DIALER
                DialerTableAdapter databaseDataSetDialerTableAdapter = new DialerTableAdapter();
                databaseDataSetDialerTableAdapter.Fill(databaseDataSet.Dialer);
                CollectionViewSource dialerViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("dialerViewSource")));
                dialerViewSource.View.MoveCurrentToFirst();

                // GLOBAL SYSTEM
                GlobalSystemTableAdapter databaseDataSetGlobalSystemTableAdapter = new GlobalSystemTableAdapter();
                databaseDataSetGlobalSystemTableAdapter.Fill(databaseDataSet.GlobalSystem);
                CollectionViewSource globalSystemViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("globalSystemViewSource")));
                globalSystemViewSource.View.MoveCurrentToFirst();

                // CLIENT INFO
                MainInfoTableAdapter databaseDataSetMainInfoTableAdapter = new MainInfoTableAdapter();
                databaseDataSetMainInfoTableAdapter.Fill(databaseDataSet.MainInfo);
                System.Windows.Data.CollectionViewSource mainInfoViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientInfoViewSource")));
                mainInfoViewSource.View.MoveCurrentToFirst();

                
                //AUDIO
                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version + "\\";


                //databaseDataSetAudioDefaultTableAdapter.Fill(databaseDataSet.Audio);
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
                //AudioTableAdapter databaseDataSetAudioCustomizedTableAdapter = new AudioTableAdapter();
                //databaseDataSetAudioCustomizedTableAdapter.Fill(databaseDataSet.Audio);
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
                
                AudioCustomizedViewSource.View.MoveCurrentToFirst();
                AudioCustomizedViewSource.Source = audio_customized_table;
                
                databaseDataSet.Audio.AcceptChanges();

                AudioTableAdapter databaseDataSetAudioTableAdapter = new defaultDataSetTableAdapters.AudioTableAdapter();
                System.Windows.Data.CollectionViewSource AudioViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioViewSource")));
                AudioViewSource.View.MoveCurrentToFirst();


                //AUDIO SYSTEM
                AudioSystemConfigurationTableAdapter databaseDataSetAudioSystemConfigurationTableAdapter = new defaultDataSetTableAdapters.AudioSystemConfigurationTableAdapter();
                databaseDataSetAudioSystemConfigurationTableAdapter.Fill(databaseDataSet.AudioSystemConfiguration);
                System.Windows.Data.CollectionViewSource AudioSystemConfigurationViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("AudioSystemConfigurationViewSource")));
                AudioSystemConfigurationViewSource.View.MoveCurrentToFirst();

            }
            catch (Exception ex)
            {
                await DialogManager.ShowMessageAsync(this, ex.Message, "");
            }


        }

        private void QueriesTableAdapter(object p)
        {
            throw new NotImplementedException();
        }

        private int sanitize_locale(Preferences pref)
        {
            if (locale.Equals("EN-US")) //EN-US
                pref.language = 0;
            else if (locale.Equals("PT-PT")) //PT-PT
                pref.language = 1;

            return pref.language;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.IsEnabled = true;
        }

        private void ChoseLanguageToChangeFlags(int culture)
        {
            if (culture == 0) //EN-US
            {
                FlagPT.Source = new BitmapImage(new Uri("/images/flags/off/pt.png", UriKind.Relative));
                RadioLocalePT.IsEnabled = true;
                FlagPT.Opacity = 0.75;

                FlagEN.Source = new BitmapImage(new Uri("/images/flags/uk.png", UriKind.Relative));
                RadioLocaleEN.IsEnabled = false;
                FlagEN.Opacity = 1;
            }
            if (culture == 1) //PT-PT
            {
                FlagPT.Source = new BitmapImage(new Uri("/images/flags/pt.png", UriKind.Relative));
                RadioLocalePT.IsEnabled = false;
                FlagPT.Opacity = 1;

                FlagEN.Source = new BitmapImage(new Uri("/images/flags/off/uk.png", UriKind.Relative));
                RadioLocaleEN.IsEnabled = true;
                FlagEN.Opacity = 0.75;
            }
        }

        private void ChangeLanguage(string culture)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);

            var oldWindow = Application.Current.MainWindow;

            Properties.Settings.Default.DefaultCulture = culture;
            Properties.Settings.Default.Save();

            Application.Current.MainWindow = new AppLogin();
            Application.Current.MainWindow.Show();

            oldWindow.Close();
        }

        private void RadioLocalePT_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("PT-PT");
        }

        private void RadioLocaleEN_Click(object sender, RoutedEventArgs e)
        {
            ChangeLanguage("EN-US");
        }
        
        private void RestoreTile_Click(object sender, RoutedEventArgs e)
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

                restore = true;
            }
            else if (messageBox == MessageBoxResult.No)
            {
                return;
            }
        }
    }
}
