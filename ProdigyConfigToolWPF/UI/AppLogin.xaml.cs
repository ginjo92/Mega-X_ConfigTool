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
                case "pt-PT":
                    InitializeComponent();
                    RadioLocaleEN_Active.Visibility = Visibility.Collapsed;
                    RadioLocalePT_Active.Visibility = Visibility.Visible;
                    break;

                case "en-US":
                    InitializeComponent();
                    RadioLocaleEN_Active.Visibility = Visibility.Visible;
                    RadioLocalePT_Active.Visibility = Visibility.Collapsed;
                    break;

                default:
                    InitializeComponent();
                    RadioLocaleEN_Active.Visibility = Visibility.Visible;
                    RadioLocalePT_Active.Visibility = Visibility.Collapsed;
                    break;
            }


            if (args.Length == 2)
            {
                //Get all needed information from Form
                string user_login = this.UserLoginValue.Text;
                string user_password = args[1];

                //TODO: Create DB and process it - return and pass to mainWindow - role, and username
                SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
                UserLoginTableAdapter user_login_table_adapter = new UserLoginTableAdapter();
                user_login_table_adapter.Fill(login_dataset.UserLogin);

                foreach (DataRow dr in login_dataset.UserLogin.Rows)
                {
                    if (dr["UserName"].ToString() == user_login)
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
                }


                if (!loginsuccessfull)
                {
                    MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                    this.Close();
                }

               
            }
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            //Get all needed information from Form
            string user_login = this.UserLoginValue.Text;          
            string user_password = this.UserPasswordValue.Password;

            //TODO: Create DB and process it - return and pass to mainWindow - role, and username
            SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
            UserLoginTableAdapter user_login_table_adapter = new UserLoginTableAdapter();
            user_login_table_adapter.Fill(login_dataset.UserLogin);

            foreach (DataRow dr in login_dataset.UserLogin.Rows)
            {
                if (dr["UserName"].ToString() == user_login)
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
            }

            if (!loginsuccessfull)
            {
                MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                //this.Close();
            }
        }

        private string sanitize_locale(AppLogin appLogin)
        {
            if (appLogin.RadioLocaleEN.IsChecked.Equals(true))
            {
                appLogin.RadioLocaleEN.BorderThickness.Equals(1);
                appLogin.RadioLocalePT.BorderThickness.Equals(0);
                return "EN-US";
            }
            if (appLogin.RadioLocalePT.IsChecked.Equals(true))
            {
                appLogin.RadioLocalePT.BorderThickness.Equals(1);
                appLogin.RadioLocaleEN.BorderThickness.Equals(0);
                return "PT-PT";
            }

            return "PT-PT";
        }

        private void RadioLocalePT_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("PT-PT");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("PT-PT");
            
            var oldWindow = Application.Current.MainWindow;

            Properties.Settings.Default.DefaultCulture = "pt-PT";
            Properties.Settings.Default.Save();

            Application.Current.MainWindow = new AppLogin();
            Application.Current.MainWindow.Show();

            oldWindow.Close();
        }

        private void RadioLocaleEN_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("EN-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("EN-US");

           
            var oldWindow = Application.Current.MainWindow;


            Properties.Settings.Default.DefaultCulture = "en-US";
            Properties.Settings.Default.Save();

            Application.Current.MainWindow = new AppLogin();
            Application.Current.MainWindow.Show();


            oldWindow.Close();

        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (Thread.CurrentThread.CurrentCulture.Name.Equals("en-US"))
            {
                RadioLocaleEN.IsChecked = true;
                RadioLocalePT.IsChecked = false;
                

            }
            else if (Thread.CurrentThread.CurrentCulture.Name.Equals("pt-PT"))
            {
                RadioLocaleEN.IsChecked = false;
                RadioLocalePT.IsChecked = true;

                
            }
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void TitleBarHelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpFlyout.IsOpen)
                HelpFlyout.IsOpen = false;
            else
                HelpFlyout.IsOpen = true;
        }
        
        

        public void QueriesTableAdapter(string connectionString)
        {
            Properties.Settings.Default["SqliteLoginConnectionString"] = connectionString + ";password = idsancoprodigy2017";
        }

        private void UserPasswordValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Get all needed information from Form
                string user_login = this.UserLoginValue.Text;
                string user_password = this.UserPasswordValue.Password;

                //TODO: Create DB and process it - return and pass to mainWindow - role, and username
                SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
                UserLoginTableAdapter user_login_table_adapter = new UserLoginTableAdapter();
                user_login_table_adapter.Fill(login_dataset.UserLogin);

                foreach (DataRow dr in login_dataset.UserLogin.Rows)
                {
                    if (dr["UserName"].ToString() == user_login)
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
                }
            }
        }
    }
}
