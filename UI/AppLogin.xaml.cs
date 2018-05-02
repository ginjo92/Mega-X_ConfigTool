using MahApps.Metro.Controls;
using ProdigyConfigToolWPF.SqliteLoginDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
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
using System.Windows.Shapes;

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for AppLogin.xaml
    /// </summary>
    
    public partial class AppLogin : MetroWindow
    {
        private int language = 0;
        private bool loginsuccessfull = false;
        public AppLogin()
        {
            string [] args = Environment.GetCommandLineArgs();

            //Read from configuration
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Properties.Settings.Default.DefaultCulture);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.DefaultCulture);

            QueriesTableAdapter("attachdbfilename=|DataDirectory|\\Database\\Login\\SqliteLogin.prgy;data source=Database\\Login\\SqliteLogin.prgy");

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

                default:
                    InitializeComponent();
                    language = 0;
                    ChoseLanguageToChangeFlags(language);
                    break;
            }

            if (args.Length == 2)
            {
                //Get all needed information from Form
                //string user_login = this.UserLoginValue.Text;
                string user_password = args[1];

                //TODO: Create DB and process it - return and pass to mainWindow - role, and username
                SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
                UserLoginTableAdapter user_login_table_adapter = new UserLoginTableAdapter();
                user_login_table_adapter.Fill(login_dataset.UserLogin);

                foreach (DataRow dr in login_dataset.UserLogin.Rows)
                {
                    if (dr["Password"].ToString() == user_password)
                    {
                        loginsuccessfull = true;

                        //sanitize locale
                        string locale = sanitize_locale(this);

                        var prodigy_configtool_window = new FileManager(locale, Convert.ToInt32(dr["Role"]), null);
                        prodigy_configtool_window.Show();
                        this.Close();

                    }
                }
                if (loginsuccessfull == false)
                {
                    MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                    this.Close();
                }
            }
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

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            //Get all needed information from Form
            //string user_login = this.UserLoginValue.Text;          
            string user_password = this.UserPasswordValue.Password;

            //TODO: Create DB and process it - return and pass to mainWindow - role, and username
            SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
            UserLoginTableAdapter user_login_table_adapter = new UserLoginTableAdapter();
            user_login_table_adapter.Fill(login_dataset.UserLogin);

            foreach (DataRow dr in login_dataset.UserLogin.Rows)
            {
                 if (dr["Password"].ToString() == user_password)
                    {
                        loginsuccessfull = true;

                        //sanitize locale
                        string locale = sanitize_locale(this);

                        var prodigy_configtool_window = new FileManager(locale, Convert.ToInt32(dr["Role"]), null);
                        prodigy_configtool_window.Show();
                        this.Close();
                    }
            }
            if (loginsuccessfull == false)
            {
                MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                this.Close();
            }
        }

        private string sanitize_locale(AppLogin appLogin)
        {
            string languageculture = "PT-PT";

            if (appLogin.language == 0) //EN-US
                languageculture = "EN-US";
            else if (appLogin.language == 1) //PT-PT
                languageculture = "PT-PT";
           
            return languageculture;
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

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("PT-PT"))
                language = 0;
            else if (Thread.CurrentThread.CurrentCulture.Name.Equals("EN-US"))
                language = 1;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void TitleBarHelpButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (HelpFlyout.IsOpen)
        //        HelpFlyout.IsOpen = false;
        //    else
        //        HelpFlyout.IsOpen = true;
        //}
                
        public void QueriesTableAdapter(string connectionString)
        {
            Properties.Settings.Default["SqliteLoginConnectionString"] = connectionString + ";password = idsancoprodigy2017";
        }

        private void UserPasswordValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ////Get all needed information from Form
                //string user_login = this.UserLoginValue.Text;
                string user_password = this.UserPasswordValue.Password;

                //TODO: Create DB and process it - return and pass to mainWindow - role, and username
                SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
                UserLoginTableAdapter user_login_table_adapter = new UserLoginTableAdapter();
                user_login_table_adapter.Fill(login_dataset.UserLogin);

                foreach (DataRow dr in login_dataset.UserLogin.Rows)
                {
                    if (dr["Password"].ToString() == user_password)
                    {
                        loginsuccessfull = true;

                        //sanitize locale
                        string locale = sanitize_locale(this);

                        var prodigy_configtool_window = new FileManager(locale, Convert.ToInt32(dr["Role"]), null);
                        prodigy_configtool_window.Show();
                        this.Close();

                    }
                }
                if (loginsuccessfull == false)
                {
                    MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                    this.Close();
                }
            }
        }
    }
}
