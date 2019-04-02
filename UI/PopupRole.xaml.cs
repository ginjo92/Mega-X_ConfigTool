using MahApps.Metro.Controls;
using MegaXConfigTool.defaultDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Data.SQLite;

namespace MegaXConfigTool
{
    public partial class PopupRole : MetroWindow
    {
        private string locale;
        private int role;
        

        public PopupRole(string ChoosenLocale, int UserRole)
        {
            locale = ChoosenLocale;
            role = UserRole;

            InitializeComponent();
            StartTimer();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //ADMIN
            if (role == 0)
            {
                UserRoleName.Text = Properties.Resources.User_role_admin_user;
                UserImage.Source = new BitmapImage(new Uri("/images/login/0_user.png", UriKind.Relative));
            }
            //MANUFACTURER
            if (role == 1)
            {
                UserRoleName.Text = Properties.Resources.User_role_manufacturer;
                UserImage.Source = new BitmapImage(new Uri("/images/login/1_manufacturer.png", UriKind.Relative));
            }
            //INSTALLER
            else if (role == 2)
            {
                UserRoleName.Text = Properties.Resources.User_role_installer;
                UserImage.Source = new BitmapImage(new Uri("/images/login/2_installer.png", UriKind.Relative));
            }
        }

        DispatcherTimer timer = null;
        void StartTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(3);
            timer.Tick += new EventHandler(timer_Elapsed);
            timer.Start();
        }

        void timer_Elapsed(object sender, EventArgs e)
        {
            timer.Stop();
            var filemanagerpage = new FileManager(locale, role, null);
            filemanagerpage.Show();
            this.Close();
        }
    }
}
