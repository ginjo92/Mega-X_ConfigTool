using MahApps.Metro.Controls;
using System;
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
using System.Windows.Shapes;
using System.IO;
using System.Net;
using System.Windows.Navigation;

namespace MegaXConfigTool
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : MetroWindow
    {

        int role;
        public Help(int AppRole)
        {
            role = AppRole;

            InitializeComponent();
            this.Loaded += HelpWindow_Loaded;
        }
            
        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if(role == 0)
            {
                Open_Areas.Visibility = Visibility.Collapsed;
                Open_Zones.Visibility = Visibility.Collapsed;
                Open_Keypads.Visibility = Visibility.Collapsed;
                Open_Outputs.Visibility = Visibility.Collapsed;
                Open_Dialer.Visibility = Visibility.Collapsed;
                Open_GlobalSystem.Visibility = Visibility.Collapsed;
                Open_Client.Visibility = Visibility.Collapsed;
                Open_Audio.Visibility = Visibility.Collapsed;
                Open_Events.Visibility = Visibility.Collapsed;
                Open_Debug.Visibility = Visibility.Collapsed;
                Open_FWUpdate.Visibility = Visibility.Collapsed;
            }

            else if (role == 2)
            {
                Open_Debug.Visibility = Visibility.Collapsed;
            }
        }

        private void Open_Login_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/login.png", UriKind.Relative));
        }
        private void Open_FileManager_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/filemanager.png", UriKind.Relative));
        }
        private void Open_Home_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/home.png", UriKind.Relative));
        }
        private void Open_Areas_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/areas.png", UriKind.Relative));
        }
        private void Open_Zones_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/zones.png", UriKind.Relative));
        }
        private void Open_Keypads_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/keypads.png", UriKind.Relative));
        }
        private void Open_Outputs_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/outputs.png", UriKind.Relative));
        }
        private void Open_Timezones_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/timezones.png", UriKind.Relative));
        }
        private void Open_Users_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/users.png", UriKind.Relative));
        }
        private void Open_Phones_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/phones.png", UriKind.Relative));
        }
        private void Open_Dialer_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/dialer.png", UriKind.Relative));
        }
        private void Open_GlobalSystem_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/system.png", UriKind.Relative));
        }
        private void Open_Client_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/clientinfo.png", UriKind.Relative));
        }
        private void Open_Events_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/events.png", UriKind.Relative));
        }
        private void Open_Audio_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/audios.png", UriKind.Relative));
        }
        private void Open_Status_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/status.png", UriKind.Relative));
        }
        private void Open_Debug_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/debug.png", UriKind.Relative));
        }
        private void Open_FWUpdate_Click(object sender, RoutedEventArgs e)
        {
            manual.Source = new BitmapImage(new Uri(@"/manuals/images/fwupdate.png", UriKind.Relative));
        }
    }
}
