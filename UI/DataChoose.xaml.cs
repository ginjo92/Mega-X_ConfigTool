using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Windows;

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for DataChoose.xaml
    /// </summary>
    public partial class DataChoose : MetroWindow
    {
        public MainWindow WindowParent;
        public bool ActionType;
        public int role;
        public DataChoose(MainWindow window_parent, bool action_type, string pre_selected_structure, int AppRole)
        {
            role = AppRole;
            WindowParent = window_parent;
            ActionType = action_type;
            InitializeComponent();

            if (action_type)
            {
                ButtonRead.Visibility = Visibility.Collapsed;
                ButtonWrite.Visibility = Visibility.Visible;
                DataChooseDialog.Text = Properties.Resources.DataChooseDialogWrite;
                
            }
            else
            {
                ButtonRead.Visibility = Visibility.Visible;
                ButtonWrite.Visibility = Visibility.Collapsed;
                DataChooseDialog.Text = Properties.Resources.DataChooseDialogRead;
            }

            if(role==0)
            {
                CheckBoxAreas.Visibility = Visibility.Collapsed;
                CheckBoxZones.Visibility = Visibility.Collapsed;
                CheckBoxKeypads.Visibility = Visibility.Collapsed;
                CheckBoxOutputs.Visibility = Visibility.Collapsed;
                CheckBoxSystem.Visibility = Visibility.Collapsed;
                CheckBoxDialer.Visibility = Visibility.Collapsed;
                CheckBoxAudioSystemConfiguration.Visibility = Visibility.Collapsed;
            }

            if (!pre_selected_structure.Equals(String.Empty))
            {
                CheckBoxAreas.IsChecked = false;
                CheckBoxZones.IsChecked = false;
                CheckBoxUsers.IsChecked = false;
                CheckBoxKeypads.IsChecked = false;
                CheckBoxOutputs.IsChecked = false;
                CheckBoxTimezones.IsChecked = false;
                CheckBoxPhones.IsChecked = false;
                CheckBoxSystem.IsChecked = false;
                CheckBoxDialer.IsChecked = false;
                CheckBoxAudioSystemConfiguration.IsChecked = false;

                switch (pre_selected_structure)
                {
                    case "zones":
                        CheckBoxZones.IsChecked = true;
                        break;

                    case "areas":
                        CheckBoxAreas.IsChecked = true;
                        break;

                    case "keypads":
                        CheckBoxKeypads.IsChecked = true;
                        break;

                    case "outputs":
                        CheckBoxOutputs.IsChecked = true;
                        break;

                    case "users":
                        CheckBoxUsers.IsChecked = true;
                        break;

                    case "timezones":
                        CheckBoxTimezones.IsChecked = true;
                        break;

                    case "dialer":
                        CheckBoxDialer.IsChecked = true;
                        break;

                    case "phones":
                        CheckBoxPhones.IsChecked = true;
                        break;

                    case "system":
                        CheckBoxSystem.IsChecked = true;
                        break;
                    case "audio_system_configuration":
                        CheckBoxAudioSystemConfiguration.IsChecked = true;
                        break;

                }
            }
        }

        private void ButtonReadOrWrite_Click(object sender, RoutedEventArgs e)
        {
            var CheckboxesValues = new List<KeyValuePair<string, bool>>();
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Areas", (bool)CheckBoxAreas.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Zones", (bool)CheckBoxZones.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Users", (bool)CheckBoxUsers.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Keypads", (bool)CheckBoxKeypads.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Outputs", (bool)CheckBoxOutputs.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Timezones", (bool)CheckBoxTimezones.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Phones", (bool)CheckBoxPhones.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("System", (bool)CheckBoxSystem.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("Dialer", (bool)CheckBoxDialer.IsChecked));
            CheckboxesValues.Add(new KeyValuePair<string, bool>("AudioSystemConfiguration", (bool)CheckBoxAudioSystemConfiguration.IsChecked));

            this.Close();

            if (ActionType.Equals(false))
                WindowParent.RequestDataFromProdigy(CheckboxesValues);
            else
                WindowParent.SendDataToProdigy(CheckboxesValues);

            WindowParent.IsEnabled = true;

        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            WindowParent.IsEnabled = true;
        }

        private void ButtonChooseAll_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxAreas.IsChecked = true;
            CheckBoxZones.IsChecked = true;
            CheckBoxUsers.IsChecked = true;
            CheckBoxKeypads.IsChecked = true;
            CheckBoxOutputs.IsChecked = true;
            CheckBoxTimezones.IsChecked = true;
            CheckBoxPhones.IsChecked = true;
            CheckBoxSystem.IsChecked = true;
            CheckBoxDialer.IsChecked = true;
            CheckBoxAudioSystemConfiguration.IsChecked = true;
        }

        private void ButtonChooseNothing_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxAreas.IsChecked = false;
            CheckBoxZones.IsChecked = false;
            CheckBoxUsers.IsChecked = false;
            CheckBoxKeypads.IsChecked = false;
            CheckBoxOutputs.IsChecked = false;
            CheckBoxTimezones.IsChecked = false;
            CheckBoxPhones.IsChecked = false;
            CheckBoxSystem.IsChecked = false;
            CheckBoxDialer.IsChecked = false;
            CheckBoxAudioSystemConfiguration.IsChecked = false;
        }
    }
}
