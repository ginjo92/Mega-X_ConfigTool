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

        private static string PathPDF = System.AppDomain.CurrentDomain.BaseDirectory + "manuals\\";

        private static string PathPDF_Home =           PathPDF + "home_01.pdf";
        private static string PathPDF_Area =           PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Zone =           PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Keypad =         PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Output =         PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Timezone =       PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_User =           PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Phone =          PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Dialer =         PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_GlobalSystem =   PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Client =         PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Event =          PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Audio =          PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Status =         PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_Debug =          PathPDF + "TutorialFlavioVeloso.pdf";
        private static string PathPDF_FWUpdate =       PathPDF + "TutorialFlavioVeloso.pdf";

        Uri homePath = new Uri(PathPDF_Home, UriKind.RelativeOrAbsolute);
        Uri areaPath = new Uri(PathPDF_Area, UriKind.RelativeOrAbsolute);
        Uri zonePath = new Uri(PathPDF_Zone, UriKind.RelativeOrAbsolute);
        Uri keypadPath = new Uri(PathPDF_Keypad, UriKind.RelativeOrAbsolute);
        Uri outputPath = new Uri(PathPDF_Output, UriKind.RelativeOrAbsolute);
        Uri timezonePath = new Uri(PathPDF_Timezone, UriKind.RelativeOrAbsolute);
        Uri userPath = new Uri(PathPDF_User, UriKind.RelativeOrAbsolute);
        Uri phonePath = new Uri(PathPDF_Phone, UriKind.RelativeOrAbsolute);
        Uri dialerPath = new Uri(PathPDF_Dialer, UriKind.RelativeOrAbsolute);
        Uri globalsystemPath = new Uri(PathPDF_GlobalSystem, UriKind.RelativeOrAbsolute);
        Uri clientPath = new Uri(PathPDF_Client, UriKind.RelativeOrAbsolute);
        Uri eventPath = new Uri(PathPDF_Event, UriKind.RelativeOrAbsolute);
        Uri audioPath = new Uri(PathPDF_Audio, UriKind.RelativeOrAbsolute);
        Uri statusPath = new Uri(PathPDF_Status, UriKind.RelativeOrAbsolute);
        Uri debugPath = new Uri(PathPDF_Debug, UriKind.RelativeOrAbsolute);
        Uri fwupdatePath = new Uri(PathPDF_FWUpdate, UriKind.RelativeOrAbsolute);


        public Help()
        {
            InitializeComponent();
            this.Loaded += HelpWindow_Loaded;
        }
        
    
        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            MainHomeTab.IsSelected = true;
        }
        
        private void Open_Home_Click(object sender, RoutedEventArgs e)
        {
            HomeHelpViewer.Navigate(homePath);
            MainHomeTab.IsSelected = true;
        }
        private void Open_Areas_Click(object sender, RoutedEventArgs e)
        {
            AreaHelpViewer.Navigate(areaPath);
            MainAreasTab.IsSelected = true;
        }
        private void Open_Zones_Click(object sender, RoutedEventArgs e)
        {
            ZonesHelpViewer.Navigate(zonePath);
            MainZonesTab.IsSelected = true;
        }
        private void Open_Keypads_Click(object sender, RoutedEventArgs e)
        {
            KeypadsHelpViewer.Navigate(keypadPath);
            MainKeypadsTab.IsSelected = true;
        }
        private void Open_Outputs_Click(object sender, RoutedEventArgs e)
        {
            OutputHelpViewer.Navigate(outputPath);
            MainOutputsTab.IsSelected = true;
        }
        private void Open_Timezones_Click(object sender, RoutedEventArgs e)
        {
            TimezoneHelpViewer.Navigate(timezonePath);
            MainTimezonesTab.IsSelected = true;
        }
        private void Open_Users_Click(object sender, RoutedEventArgs e)
        {
            UsersHelpViewer.Navigate(userPath);
            MainAreasTab.IsSelected = true;
        }
        private void Open_Phones_Click(object sender, RoutedEventArgs e)
        {
            PhonesHelpViewer.Navigate(phonePath);
            MainPhonesTab.IsSelected = true;
        }
        private void Open_Dialer_Click(object sender, RoutedEventArgs e)
        {
            DialerHelpViewer.Navigate(dialerPath);
            MainDialerTab.IsSelected = true;
        }
        private void Open_GlobalSystem_Click(object sender, RoutedEventArgs e)
        {
            GlobalSystemHelpViewer.Navigate(globalsystemPath);
            MainGlobalSystemTab.IsSelected = true;
        }
        private void Open_Client_Click(object sender, RoutedEventArgs e)
        {
            ClientHelpViewer.Navigate(clientPath);
            MainClientTab.IsSelected = true;
        }
        private void Open_Events_Click(object sender, RoutedEventArgs e)
        {
            EventsHelpViewer.Navigate(eventPath);
            MainEventsTab.IsSelected = true;
        }
        private void Open_Audio_Click(object sender, RoutedEventArgs e)
        {
            AudioHelpViewer.Navigate(audioPath);
            MainAudioTab.IsSelected = true;
        }
        private void Open_Status_Click(object sender, RoutedEventArgs e)
        {
            StatusHelpViewer.Navigate(statusPath);
            MainStatusTab.IsSelected = true;
        }
        private void Open_Debug_Click(object sender, RoutedEventArgs e)
        {
            DebugHelpViewer.Navigate(debugPath);
            MainDebugTab.IsSelected = true;
        }
        private void Open_FWUpdate_Click(object sender, RoutedEventArgs e)
        {
            FWUpdateHelpViewer.Navigate(fwupdatePath);
            MainFWUpdateTab.IsSelected = true;
        }
    }
}
