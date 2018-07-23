using MahApps.Metro.Controls;
using ProdigyConfigToolWPF.defaultDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.Data;
using MahApps.Metro.Controls.Dialogs;

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        public string AppDbFile;
        private string locale;
        private MainWindow mainWindow;
        private string version;
        private int language = 0;
        int role;
           
        Boolean restore = false;

        public Settings(string ChoosenLocale, MainWindow main, string ChoosenDbFile, Boolean default_restore_is_set, int AppRole)
        {
            InitializeComponent();

            role = AppRole;
            this.Loaded += MetroWindow_Loaded;
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

            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\V" + version + "\\"; //My documents folder
            mainWindow.QueriesTableAdapter("attachdbfilename =" + configurations_folder + ChoosenDbFile + "; data source = " + configurations_folder + ChoosenDbFile);
            
            this.Language = XmlLanguage.GetLanguage(
                       Properties.Settings.Default.DefaultCulture);
            
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
            }
        }
        

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (role == 0)
            {
                RestoreTile.Visibility = Visibility.Collapsed;
                Sep1.Visibility = Visibility.Collapsed;
            }

            //if (Thread.CurrentThread.CurrentCulture.Name.Equals("PT-PT"))
            //    language = 1;
            //else if (Thread.CurrentThread.CurrentCulture.Name.Equals("EN-US"))
            //    language = 0;
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
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("PT-PT");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("PT-PT");
            Properties.Settings.Default.DefaultCulture = "PT-PT";
            Properties.Settings.Default.Save();
            if (mainWindow.databaseDataSet.HasChanges())
            {
                var messageBox = MessageBox.Show(Properties.Resources.QuestionSaveChangesExtended + "\n" + "\n" + Properties.Resources.InfoDataWillBeLost, Properties.Resources.QuestionSaveChanges, MessageBoxButton.YesNoCancel);
                if (messageBox == MessageBoxResult.Cancel)
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("EN-US");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("EN-US");
                    Properties.Settings.Default.DefaultCulture = "EN-US";
                    Properties.Settings.Default.Save();
                    return;
                }
                else if (messageBox == MessageBoxResult.Yes)
                {
                    mainWindow.Save_Database_data();

                }
                else if (messageBox == MessageBoxResult.No)
                {
                    mainWindow.Closing -= mainWindow.BaseWindow_Closing;
                }
            }
            
            mainWindow.Close();
            mainWindow.Closing += mainWindow.BaseWindow_Closing;
            MainWindow window1 = new MainWindow("pt-PT", mainWindow.AppRole, mainWindow.AppDbFile, null, null, null, null, null);
            window1.Show();
            
            this.Close();
        }
        private void RadioLocaleEN_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Properties.Settings.Default.DefaultCulture = "en-US";
            Properties.Settings.Default.Save();

            if (mainWindow.databaseDataSet.HasChanges())
            {
                var messageBox = MessageBox.Show(Properties.Resources.QuestionSaveChangesExtended + "\n" + "\n" + Properties.Resources.InfoDataWillBeLost, Properties.Resources.QuestionSaveChanges, MessageBoxButton.YesNoCancel);
                if (messageBox == MessageBoxResult.Cancel)
                {
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("PT-PT");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("PT-PT");
                    Properties.Settings.Default.DefaultCulture = "PT-PT";
                    Properties.Settings.Default.Save();
                    return;
                }
                else if (messageBox == MessageBoxResult.Yes)
                {
                    mainWindow.Save_Database_data();

                }
            }

            Properties.Settings.Default.DefaultCulture = "EN-US";
            Properties.Settings.Default.Save();



            mainWindow.Close();
            MainWindow window1 = new MainWindow("en-US", mainWindow.AppRole, mainWindow.AppDbFile, null, null, null, null, null);
            window1.Show();
            
            this.Close();

        }

        private async void RestoreTile_Click(object sender, RoutedEventArgs e)
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
                mainWindow.databaseDataSet.Zone.Clear();

                //delete table
                foreach (defaultDataSet.ZoneRow row in (mainWindow.databaseDataSet.Zone.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.ZoneRow row in zone_table)
                {
                    mainWindow.databaseDataSet.Zone.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Area.Clear();

                //delete table
                foreach (defaultDataSet.AreaRow row in (mainWindow.databaseDataSet.Area.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AreaRow row in area_table)
                {
                    mainWindow.databaseDataSet.Area.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.User.Clear();

                //delete table
                foreach (defaultDataSet.UserRow row in (mainWindow.databaseDataSet.User.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.UserRow row in user_table)
                {
                    mainWindow.databaseDataSet.User.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Keypad.Clear();

                //delete table
                foreach (defaultDataSet.KeypadRow row in (mainWindow.databaseDataSet.Keypad.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.KeypadRow row in keypad_table)
                {
                    mainWindow.databaseDataSet.Keypad.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Output.Clear();

                //delete table
                foreach (defaultDataSet.OutputRow row in (mainWindow.databaseDataSet.Output.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.OutputRow row in output_table)
                {
                    mainWindow.databaseDataSet.Output.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Timezone.Clear();

                //delete table
                foreach (defaultDataSet.TimezoneRow row in (mainWindow.databaseDataSet.Timezone.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.TimezoneRow row in timezone_table)
                {
                    mainWindow.databaseDataSet.Timezone.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Phone.Clear();

                //delete table
                foreach (defaultDataSet.PhoneRow row in (mainWindow.databaseDataSet.Phone.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.PhoneRow row in phone_table)
                {
                    mainWindow.databaseDataSet.Phone.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Dialer.Clear();

                //delete table
                foreach (defaultDataSet.DialerRow row in (mainWindow.databaseDataSet.Dialer.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.DialerRow row in dialer_table)
                {
                    mainWindow.databaseDataSet.Dialer.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.GlobalSystem.Clear();

                //delete table
                foreach (defaultDataSet.GlobalSystemRow row in (mainWindow.databaseDataSet.GlobalSystem.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.GlobalSystemRow row in system_table)
                {
                    mainWindow.databaseDataSet.GlobalSystem.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.MainInfo.Clear();

                //delete table
                foreach (defaultDataSet.MainInfoRow row in (mainWindow.databaseDataSet.MainInfo.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.MainInfoRow row in main_info_table)
                {
                    mainWindow.databaseDataSet.MainInfo.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Event.Clear();

                //delete table
                foreach (defaultDataSet.EventRow row in (mainWindow.databaseDataSet.Event.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.EventRow row in event_table)
                {
                    mainWindow.databaseDataSet.Event.Rows.Add(row.ItemArray);
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
                mainWindow.databaseDataSet.Audio.Clear();

                //delete table
                foreach (defaultDataSet.AudioRow row in (mainWindow.databaseDataSet.Audio.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AudioRow row in audio_table)
                {
                    mainWindow.databaseDataSet.Audio.Rows.Add(row.ItemArray);
                }
                #endregion

                #region AUDIO CUSTOMIZED
                //defaultDataSet.AudioDataTable audio_customized_table = new defaultDataSet.AudioDataTable();
                //con = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy;Password=idsancoprodigy2017");
                //con.Open();
                //cmd = con.CreateCommand();
                //cmd.CommandText = string.Format("SELECT * FROM Audio WHERE Type = 1");
                //adapter = new SQLiteDataAdapter(cmd);
                //builder = new SQLiteCommandBuilder(adapter);
                //adapter.Fill(audio_customized_table);
                //con.Close();
                //mainWindow.databaseDataSet.Audio.Clear();

                //DataTable audio_full_table = new DataTable();
                //audio_full_table = audio_table.Copy();
                //audio_full_table.Merge(audio_customized_table);



                //AudioTableAdapter databaseDatasetTableAdapter = new AudioTableAdapter();
                //databaseDatasetTableAdapter.Fill(audio_table);
                //databaseDatasetTableAdapter.Update(mainWindow.databaseDataSet.Audio);

                ////delete table
                //foreach (defaultDataSet.AudioRow row in (mainWindow.databaseDataSet.Audio.Select("Id <> null")))
                //{
                //    row.Delete();
                //}

                //foreach (defaultDataSet.AudioRow row in audio_customized_table)
                //{
                //    mainWindow.databaseDataSet.Audio.Rows.Add(row.ItemArray);
                //}

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
                mainWindow.databaseDataSet.AudioSystemConfiguration.Clear();

                //delete table
                foreach (defaultDataSet.AudioSystemConfigurationRow row in (mainWindow.databaseDataSet.Audio.Select("Id <> null")))
                {
                    row.Delete();
                }

                foreach (defaultDataSet.AudioSystemConfigurationRow row in audio_system_table)
                {
                    mainWindow.databaseDataSet.AudioSystemConfiguration.Rows.Add(row.ItemArray);
                }
                #endregion

                restore = true;
            }
            else if (messageBox == MessageBoxResult.No)
            {
                return;
            }
            this.Close();
            mainWindow.IsEnabled = true;
            await DialogManager.ShowMessageAsync(mainWindow, Properties.Resources.DefaultConfigRestored, "");
        }
    }
}
