using MahApps.Metro.Controls;
using ProdigyConfigToolWPF.SqliteLoginDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for PasswordChange.xaml
    /// </summary>
    public partial class PasswordChange : MetroWindow
    {
        //private string UserName;
        private int Role = 0;
        private MainWindow mainWindow;
        private Boolean loginsuccessfull;

        SqliteLoginDataSet login_dataset = new SqliteLoginDataSet();
        UserLoginTableAdapter LoginTableAdapter = new UserLoginTableAdapter();

        public PasswordChange(string locale, int role, MainWindow main)
        {
            
            mainWindow = main;
            Role = role;
            mainWindow.IsEnabled = false;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(locale);
            //QueriesTableAdapter("attachdbfilename=|DataDirectory|\\Database\\Login\\SqliteLogin.prgy;data source=Database\\Login\\SqliteLogin.prgy;Password=idsancoprodigy2017");

            string sancoDBfolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Sanco S.A\\Mega-X Config Tool\\db\\";
            QueriesTableAdapter("attachdbfilename =" + sancoDBfolder + "SqliteLogin.prgy" + "; data source = " + sancoDBfolder + "SqliteLogin.prgy");

            InitializeComponent();
        }

      
        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            LoginTableAdapter.Fill(login_dataset.UserLogin);
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mainWindow.IsEnabled = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            //Get all needed information from Form
            string user_password = this.UserPasswordValue.Password;
            string new_user_password = this.UserNewPasswordValue.Password;
            string confirm_new_user_password = this.UserRepeatNewPasswordValue.Password;
                        
            foreach (DataRow dr in login_dataset.UserLogin.Rows)
            {
                if (Convert.ToInt32(dr["Role"]) == Role)
                {
                    if (dr["Password"].ToString() == user_password)
                    {
                        loginsuccessfull = true;

                        if(new_user_password.Equals(confirm_new_user_password))
                        {
                            dr["Password"] = new_user_password;
                            MessageBox.Show(Properties.Resources.PasswordChanged, "", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve
                           
                            LoginTableAdapter.Update(login_dataset.UserLogin);
                            DebugTable(login_dataset.UserLogin);
                            this.Close();
                        }else
                        {
                            MessageBox.Show(Properties.Resources.NewPasswordConfirmationNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                        }                        
                    }
                }
            }
            

            if (!loginsuccessfull)
            {
                MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                
            }
            else
            {
                
                this.Close();
                mainWindow.Close();
                AppLogin newapplogin = new AppLogin();
                newapplogin.Show();
            }
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
            Properties.Settings.Default["defaultConnectionString"] = connectionString + ";password = idsancoprodigy2017";
        }

        private void UserRepeatNewPasswordValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //Get all needed information from Form
                //Get all needed information from Form
                string user_password = this.UserPasswordValue.Password;
                string new_user_password = this.UserNewPasswordValue.Password;
                string confirm_new_user_password = this.UserRepeatNewPasswordValue.Password;

                foreach (DataRow dr in login_dataset.UserLogin.Rows)
                {
                    if (Convert.ToInt32(dr["Role"]) == Role)
                    {
                        if (dr["Password"].ToString() == user_password)
                        {
                            loginsuccessfull = true;

                            if (new_user_password.Equals(confirm_new_user_password))
                            {
                                dr["Password"] = new_user_password;
                                MessageBox.Show(Properties.Resources.PasswordChanged, "", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve

                                LoginTableAdapter.Update(login_dataset.UserLogin);
                                DebugTable(login_dataset.UserLogin);
                                this.Close();
                            }
                            else
                            {
                                MessageBox.Show(Properties.Resources.NewPasswordConfirmationNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
                            }
                        }
                    }
                }


                if (!loginsuccessfull)
                {
                    MessageBox.Show(Properties.Resources.PasswordUserNotMatch, "", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve

                }
                else
                {

                    this.Close();
                    mainWindow.Close();
                    AppLogin newapplogin = new AppLogin();
                    newapplogin.Show();
                }
            }
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
    }
}
