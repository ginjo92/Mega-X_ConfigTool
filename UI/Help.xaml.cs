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

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : MetroWindow
    {
        private MainWindow mainWindow;
        
        public Help()
        {
            InitializeComponent();
            this.Loaded += HelpWindow_Loaded;
        }
            
        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
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
