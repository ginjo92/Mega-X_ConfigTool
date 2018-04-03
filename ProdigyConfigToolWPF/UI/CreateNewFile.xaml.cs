using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;


namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for CreateNewFile.xaml
    /// </summary>
    public partial class CreateNewFile : MetroWindow
    {
        private ItemCollection Fileitems;
        private string AppLocale;
        private int AppRole;
        private object fileManager;

        public CreateNewFile(ItemCollection Fileitems, string locale, int role, object fileManager)
        {
            this.Fileitems = Fileitems;
            this.AppLocale = locale;
            this.AppRole = role;
            this.fileManager = fileManager;

            InitializeComponent();

            //Fill combobox with existent files
            this.ComboBoxFiles.ItemsSource = Fileitems;
            this.ComboBoxFiles.DisplayMemberPath = "Name";
            this.ComboBoxFiles.SelectedValuePath = "Name";
        }
        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonCreateNewFile_Click(object sender, RoutedEventArgs e)
        {

            ///Validate name
            //Remove name extension
            if (Path.GetExtension(NewConfigName.Text) != "")
            {
                NewConfigName.Text = Path.GetFileNameWithoutExtension(NewConfigName.Text);
            }
            //Check if name is empty
            if (String.IsNullOrEmpty(NewConfigName.Text)) {
                System.Windows.MessageBox.Show(Properties.Resources.FileNameCantBeEmpty, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve  
                return;
            }

            //Chec if name already exists
            //string folder = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\database\";
            string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            string configurations_folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Configurator\\V" + version + "\\"; //My documents folder
            
            string filter = "*.prgy";
            string[] files = Directory.GetFiles(configurations_folder, filter);

            foreach (string file in files)
            {
                if (System.IO.Path.GetFileNameWithoutExtension(file).Equals(NewConfigName.Text))
                {
                    System.Windows.MessageBox.Show(Properties.Resources.FileAlreadyExists, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve  
                    return;
                }
            }

            ///Check radio button
            if (DefaultRadioButton.IsChecked.Equals(true))
            {
                try {
                    System.IO.File.Copy(System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\configuration\setup\default\default.prgy",
                    configurations_folder + NewConfigName.Text + ".prgy");
                    
                }
                catch
                {
                    System.IO.File.Copy(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86) + @"\default.prgy",
                    configurations_folder + NewConfigName.Text + ".prgy");
                }
            }
            else if(TemplateRadioButton.IsChecked.Equals(true))
            {
                System.IO.File.Copy(configurations_folder + ComboBoxFiles.SelectedValue,
                                    configurations_folder + NewConfigName.Text + ".prgy");
                
            }

            //Initialize APP with new DB
            if (this.fileManager.GetType() == typeof(FileManager))
                ((FileManager)this.fileManager).Close();

            if (this.fileManager.GetType() == typeof(MainWindow))
                ((MainWindow)this.fileManager).Close();

            if (DefaultRadioButton.IsChecked.Equals(true))
            {
                var setup_wizard = new WizardSystemArchitecture(AppLocale, AppRole, (NewConfigName.Text + ".prgy"));
                setup_wizard.Show();
            }
            else
            {
                var prodigy_configtool_window = new MainWindow(AppLocale, AppRole, (NewConfigName.Text + ".prgy"), null, null, null, null);
                prodigy_configtool_window.Show();
            }
            this.Close();
           
        }

        private void TemplateRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            ComboBoxFiles.SelectedIndex = 0;
        }
    }
}
