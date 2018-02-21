using MahApps.Metro.Controls;
using ProdigyConfigToolWPF.SqliteLoginDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Data;
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
        private string UserName;
        private AppLogin PageParent;
        private Boolean loginsuccessfull;
        public PasswordChange(string locale, string user, AppLogin parent)
        {
            PageParent = parent;
            UserName = user;
            PageParent.IsEnabled = false;
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(locale);
            QueriesTableAdapter("attachdbfilename=|DataDirectory|\\Database\\Login\\SqliteLogin.prgy;data source=Database\\Login\\SqliteLogin.prgy");

            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UserLoginValue.Text = UserName;
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            PageParent.IsEnabled = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            //Get all needed information from Form
            string user_login = this.UserLoginValue.Text;
            string user_password = this.UserPasswordValue.Password;
            string new_user_password = this.UserNewPasswordValue.Password;
            string confirm_new_user_password = this.UserRepeatNewPasswordValue.Password;

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

                        if(new_user_password.Equals(confirm_new_user_password))
                        {
                            dr["Password"] = new_user_password;
                            user_login_table_adapter.Update(dr);
                            MessageBox.Show(Properties.Resources.PasswordChanged, "", MessageBoxButton.OK, MessageBoxImage.Information); // TODO: delete/improve
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
                this.Close();
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
    }
}
