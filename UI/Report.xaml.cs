using MahApps.Metro.Controls;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MegaXConfigTool
{
    /// <summary>
    /// Interaction logic for CreateNewFile.xaml
    /// </summary>
    public partial class Report : MetroWindow
    {

        public MainWindow WindowParent;
        private double[] datasent;
        private double[] datatosend;

        public Report(MainWindow window_parent, double[] data_sent, double[] data_to_send)
        {
            WindowParent = window_parent;
            datasent = data_sent;
            datatosend = data_to_send;

            InitializeComponent();

            SetMessage(ReportZones, ReportZones_name, 0);
            SetMessage(ReportAreas, ReportAreas_name, 1);
            SetMessage(ReportUsers, ReportUsers_name, 2);
            SetMessage(ReportKeypads, ReportKeypads_name, 3);
            SetMessage(ReportOutputs, ReportOutputs_name, 4);
            SetMessage(ReportTimezones, ReportTimezones_name, 5);
            SetMessage(ReportPhones, ReportPhones_name, 6);
            SetMessage(ReportGlobalSystemConfig, ReportGlobalSystemConfig_name, 7);
            SetMessage(ReportDialer, ReportDialer_name, 8);
            SetMessage(ReportAudioConfig, ReportAudioConfig_name, 9);
            SetMessage(ReportExpanders, ReportExpanders_name, 10);
        }
        

        private void SetMessage(Label report_label, Label report_label_name, int num)
        {
            if(datatosend[num] == 0)
            {
                report_label.Visibility = Visibility.Collapsed;
                report_label_name.Visibility = Visibility.Collapsed;
            }
            else
            {
                report_label.Visibility = Visibility.Visible;
                report_label_name.Visibility = Visibility.Visible;
            }

            if (datasent[num] == 0)
            {
                report_label.Foreground = Brushes.Red;
                report_label.Content = Properties.Resources.Error;
            }
            if (datasent[num] == 1)
            {
                report_label.Foreground = Brushes.Green;
                report_label.Content = Properties.Resources.Ok;
            }
        }    

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        

    }
}
