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

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for FileManager.xaml
    /// </summary>
    public partial class FileManager : MetroWindow
    {
        private string locale;
        private int role;
        private MainWindow mainWindow;

        public FileManager(string ChoosenLocale, int UserRole, MainWindow mainWindow)
        {
            locale = ChoosenLocale;
            role = UserRole;
            this.mainWindow = mainWindow;
            InitializeComponent();

            string version_part = (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 4) + "X";
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version_part + "\\"; //My documents folder
            string old_configs_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version_part + "\\old\\";

            Console.WriteLine("File Manager: " + role);

            if (!Directory.Exists(configurations_folder))
            {
                Directory.CreateDirectory(configurations_folder);
            }

            if (!Directory.Exists(old_configs_folder))
            {
                Directory.CreateDirectory(old_configs_folder);
            }

            //string folder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\database\";
            string filter = "*.prgy";
            string[] files = Directory.GetFiles(configurations_folder, filter);

            List<File> Files = new List<File>();
            int file_id = 0;
            foreach (string file in files)
            {
                Files.Add(new File() { Id = file_id, Name = System.IO.Path.GetFileName(file), LastChanged = System.IO.File.GetLastWriteTime(file) });
                file_id++;
            }
            file_id = 0;

            foreach (File file in Files)
            {
                this.DBListBox.Items.Add(file);
            }

            if (DBListBox.Items != null)
            {
                DBListBox.SelectedIndex = 0;
            }
            else
            {
                DeleteTile.IsEnabled = false;
            }

            //if (mainWindow != null)
            //    DeleteTile.Visibility = Visibility.Collapsed;

        }

        private void FileManagerWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public class File
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public DateTime LastChanged { get; set; }
        }

        private void ListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            while (obj != null && obj != this.DBListBox)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    File DbFile = (File)DBListBox.SelectedItem;
                    var prodigy_configtool_window = new MainWindow(locale, role, DbFile.Name, null, null, null, null, null);

                    if (mainWindow != null)
                    {
                        mainWindow.Close();
                    }

                    prodigy_configtool_window.Show();
                    this.Close();
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        public void QueriesTableAdapter(string connectionString)
        {
            Properties.Settings.Default["defaultConnectionString"] = connectionString + ";password = idsancoprodigy2017";
        }

        private void ListViewClick(object sender, SelectionChangedEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            File DbFile = (File)DBListBox.SelectedItem;

            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string version_part = (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 4) + "X";

            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version_part + "\\"; //My documents folder
            QueriesTableAdapter("attachdbfilename =" + configurations_folder + DbFile.Name + "; data source = " + configurations_folder + DbFile.Name);

            

            //QueriesTableAdapter("attachdbfilename =| DataDirectory |\\Database\\" + DbFile.Name + "; data source = Database\\" + DbFile.Name);

            // Opens an unencrypted database
            //SQLiteConnection cnn = new SQLiteConnection("Data Source=" + System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\database\sqlitetest.prgy");
            //cnn.SetPassword("");
            //cnn.Open();
            ////cnn.ChangePassword("1234");

            defaultDataSet databaseDataSet = ((defaultDataSet)(this.FindResource("databaseDataSet")));

            // Load data into the table Zone. You can modify this code as needed.
            MainInfoTableAdapter databaseDataSetMainInfoTableAdapter = new MainInfoTableAdapter();
            databaseDataSetMainInfoTableAdapter.Fill(databaseDataSet.MainInfo);

            this.ProjectName.Text = DbFile.Name;

            this.ClientName.Text = databaseDataSet.MainInfo.Rows[0]["ClientName"].ToString();
            this.ClientAddr1.Text = databaseDataSet.MainInfo.Rows[0]["Address_1"].ToString();
            this.ClientAddr2.Text = databaseDataSet.MainInfo.Rows[0]["Address_2"].ToString();
            this.ClientCity.Text = databaseDataSet.MainInfo.Rows[0]["City"].ToString();
            this.ClientPostalCode.Text = databaseDataSet.MainInfo.Rows[0]["PostalCode"].ToString();
            this.ClientDistrict.Text = databaseDataSet.MainInfo.Rows[0]["District"].ToString();
            this.ClientPhone.Text = databaseDataSet.MainInfo.Rows[0]["PhoneNumber"].ToString();
            this.ClientFax.Text = databaseDataSet.MainInfo.Rows[0]["FaxNumber"].ToString();
            this.ClientEmail.Text = databaseDataSet.MainInfo.Rows[0]["Email"].ToString();
            this.ClientWebSite.Text = databaseDataSet.MainInfo.Rows[0]["WebPage"].ToString();

        }

        private void NewConfigTile_Click(object sender, RoutedEventArgs e)
        {
            var CreateNewFileWindow = new CreateNewFile(DBListBox.Items, locale, role, this);

            CreateNewFileWindow.ShowDialog();
        }

        private void DeleteConfigTile_Click(object sender, RoutedEventArgs e)
        {
            DependencyObject obj = (DependencyObject)e.OriginalSource;

            File DbFile = (File)DBListBox.SelectedItem;

            if (DBListBox.SelectedIndex == -1)
            {
                MessageBox.Show(Properties.Resources.ChooseFile, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }
            else
            {

                if (mainWindow != null)
                {
                    if (DbFile.Name.Equals(mainWindow.AppDbFile))
                    {
                        MessageBox.Show(Properties.Resources.File_in_use, Properties.Resources.Error, MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                    }
                    else
                    {
                        var messageBox = MessageBox.Show(Properties.Resources.DeleteConfigFile, Properties.Resources.SureToDeleteFile, MessageBoxButton.YesNo);
                        if (messageBox == MessageBoxResult.No)
                        {
                            
                        }
                        else if (messageBox == MessageBoxResult.Yes)
                        {
                           MoveDeletedFileToOldFolder(DbFile);
                        }
                    }
                }
                else
                {
                    var messageBox = MessageBox.Show(Properties.Resources.DeleteConfigFile, Properties.Resources.SureToDeleteFile, MessageBoxButton.YesNo);
                    if (messageBox == MessageBoxResult.No)
                    {

                    }
                    else if (messageBox == MessageBoxResult.Yes)
                    {
                        MoveDeletedFileToOldFolder(DbFile);
                    }
                   
                }
            }
            
            
        }

        private void MoveDeletedFileToOldFolder(File DbFile)
        {
            DBListBox.SelectedItem = DBListBox.Items[0];
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string version_part = (System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()).Substring(0, 4) + "X";
            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version_part + "\\"; //My documents folder

            string sourcePath = configurations_folder + DbFile.Name;
            
            string targetPath = configurations_folder + "\\old\\" + DbFile.Name;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            try
            {
                System.IO.File.Move(sourcePath, targetPath);
            }
            catch (Exception ex)
            {
                if (System.IO.File.Exists(targetPath))
                {
                    System.IO.File.Delete(targetPath);
                }
                System.IO.File.Move(sourcePath, targetPath);
            }

            DBListBox.SelectionChanged -= ListViewClick;
            DBListBox.Items.Remove(DbFile);
            DBListBox.SelectionChanged += ListViewClick;
            DBListBox.InvalidateArrange();
            DBListBox.UpdateLayout();
        }
    }

}
