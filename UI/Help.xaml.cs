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
        private static string PathPDF = System.AppDomain.CurrentDomain.BaseDirectory + "manuals\\";

        private string PathPDF_Home =           PathPDF + "home_01.pdf";
        private string PathPDF_Area =           PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Zone =           PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Keypad =         PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Output =         PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Timezone =       PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_User =           PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Phone =          PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Dialer =         PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_GlobalSystem =   PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Client =         PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Event =          PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Audio =          PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Status =         PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_Debug =          PathPDF + "TutorialFlavioVeloso.pdf";
        private string PathPDF_FWUpdate =       PathPDF + "TutorialFlavioVeloso.pdf";


        public Help()
        {
            InitializeComponent();
            MainHomeTab.IsSelected = true;

            
            System.Diagnostics.Debug.WriteLine("GINJO - Home: " + PathPDF_Home);

            this.Loaded += HelpWindow_Loaded;
            
        }
        
    
        private void HelpWindow_Loaded(object sender, RoutedEventArgs e)
        {
            HomeHelpViewer.Navigate(PathPDF_Home);
            AreaHelpViewer.Navigate(PathPDF_Area);
            ZonesHelpViewer.Navigate(PathPDF_Zone);
            KeypadsHelpViewer.Navigate(PathPDF_Keypad);
            OutputHelpViewer.Navigate(PathPDF_Output);
            TimezoneHelpViewer.Navigate(PathPDF_Timezone);
            UsersHelpViewer.Navigate(PathPDF_User);
            PhonesHelpViewer.Navigate(PathPDF_Phone);
            DialerHelpViewer.Navigate(PathPDF_Dialer);
            GlobalSystemHelpViewer.Navigate(PathPDF_GlobalSystem);
            ClientHelpViewer.Navigate(PathPDF_Client);
            EventsHelpViewer.Navigate(PathPDF_Event);
            AudioHelpViewer.Navigate(PathPDF_Audio);
            StatusHelpViewer.Navigate(PathPDF_Status);
            DebugHelpViewer.Navigate(PathPDF_Debug);
            FWUpdateHelpViewer.Navigate(PathPDF_FWUpdate);
            AreaHelpViewer.Navigate(PathPDF_Area);
        }


        private void Open_Home_Click(object sender, RoutedEventArgs e)
        {
            MainHomeTab.IsSelected = true;
        }

        private void Open_Areas_Click(object sender, RoutedEventArgs e)
        {
            MainAreasTab.IsSelected = true;
        }

        private void Open_Zones_Click(object sender, RoutedEventArgs e)
        {
            MainZonesTab.IsSelected = true;
        }

        private void Open_Keypads_Click(object sender, RoutedEventArgs e)
        {
            MainKeypadsTab.IsSelected = true;
        }

        private void Open_Outputs_Click(object sender, RoutedEventArgs e)
        {
            MainOutputsTab.IsSelected = true;
        }

        private void Open_Timezones_Click(object sender, RoutedEventArgs e)
        {
            MainTimezonesTab.IsSelected = true;
        }

        private void Open_Users_Click(object sender, RoutedEventArgs e)
        {
            MainAreasTab.IsSelected = true;
        }

        private void Open_Phones_Click(object sender, RoutedEventArgs e)
        {
            MainPhonesTab.IsSelected = true;
        }

        private void Open_Dialer_Click(object sender, RoutedEventArgs e)
        {
            MainDialerTab.IsSelected = true;
        }

        private void Open_GlobalSystem_Click(object sender, RoutedEventArgs e)
        {
            MainGlobalSystemTab.IsSelected = true;
        }

        private void Open_Client_Click(object sender, RoutedEventArgs e)
        {
            MainClientTab.IsSelected = true;
        }

        private void Open_Events_Click(object sender, RoutedEventArgs e)
        {
            MainEventsTab.IsSelected = true;
        }

        private void Open_Audio_Click(object sender, RoutedEventArgs e)
        {
            MainAudioTab.IsSelected = true;
        }

        private void Open_Status_Click(object sender, RoutedEventArgs e)
        {
            MainStatusTab.IsSelected = true;
        }

        private void Open_Debug_Click(object sender, RoutedEventArgs e)
        {
            MainDebugTab.IsSelected = true;
        }

        private void Open_FWUpdate_Click(object sender, RoutedEventArgs e)
        {
            MainFWUpdateTab.IsSelected = true;
        }

    }
}
