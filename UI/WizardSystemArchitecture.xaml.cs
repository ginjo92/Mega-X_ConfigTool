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

namespace ProdigyConfigToolWPF
{
    /// <summary>
    /// Interaction logic for WizardSystemArchitecture.xaml
    /// </summary>
    public partial class WizardSystemArchitecture : MetroWindow
    {
        private string AppLocale;
        private int AppRole;
        private string config_file_name;

        public WizardSystemArchitecture(string locale, int role, string config_file_name)
        {
            this.AppLocale = locale;
            this.AppRole = role;
            this.config_file_name = config_file_name;
            InitializeComponent();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            var prodigy_configtool_window = new MainWindow(AppLocale, AppRole, config_file_name, null, null, null, null, null);
            prodigy_configtool_window.Show();

            this.Close();
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            Dictionary<int, bool> KeypadConfiguration = new Dictionary<int, bool>();
            KeypadConfiguration.Add(0, SelectedKeypad1.IsVisible);
            KeypadConfiguration.Add(1, SelectedKeypad2.IsVisible);
            KeypadConfiguration.Add(2, SelectedKeypad3.IsVisible);
            KeypadConfiguration.Add(3, SelectedKeypad4.IsVisible);
            KeypadConfiguration.Add(4, SelectedKeypad5.IsVisible);
            KeypadConfiguration.Add(5, SelectedKeypad6.IsVisible);
            KeypadConfiguration.Add(6, SelectedKeypad7.IsVisible);
            KeypadConfiguration.Add(7, SelectedKeypad8.IsVisible);

            Dictionary<int, bool> DialerConfiguration = new Dictionary<int, bool>();
            DialerConfiguration.Add(0, SelectedDialer.IsVisible);

            Dictionary<int, bool> ZonesConfiguration = new Dictionary<int, bool>();
            ZonesConfiguration.Add(0, SelectedZone1.IsVisible);
            ZonesConfiguration.Add(1, SelectedZone2.IsVisible);
            ZonesConfiguration.Add(2, SelectedZone3.IsVisible);
            ZonesConfiguration.Add(3, SelectedZone4.IsVisible);
            ZonesConfiguration.Add(4, SelectedZone5.IsVisible);
            ZonesConfiguration.Add(5, SelectedZone6.IsVisible);
            ZonesConfiguration.Add(6, SelectedZone7.IsVisible);
            ZonesConfiguration.Add(7, SelectedZone8.IsVisible);
            ZonesConfiguration.Add(8, SelectedZone9.IsVisible);
            ZonesConfiguration.Add(9, SelectedZone10.IsVisible);
            ZonesConfiguration.Add(10, SelectedZone11.IsVisible);
            ZonesConfiguration.Add(11, SelectedZone12.IsVisible);
            ZonesConfiguration.Add(12, SelectedZone13.IsVisible);
            ZonesConfiguration.Add(13, SelectedZone14.IsVisible);
            ZonesConfiguration.Add(14, SelectedZone15.IsVisible);
            ZonesConfiguration.Add(15, SelectedZone16.IsVisible);
            ZonesConfiguration.Add(16, SelectedZone17.IsVisible);
            ZonesConfiguration.Add(17, SelectedZone18.IsVisible);
            ZonesConfiguration.Add(18, SelectedZone19.IsVisible);
            ZonesConfiguration.Add(19, SelectedZone20.IsVisible);
            ZonesConfiguration.Add(20, SelectedZone21.IsVisible);
            ZonesConfiguration.Add(21, SelectedZone22.IsVisible);
            ZonesConfiguration.Add(22, SelectedZone23.IsVisible);
            ZonesConfiguration.Add(23, SelectedZone24.IsVisible);
            ZonesConfiguration.Add(24, SelectedZone25.IsVisible);
            ZonesConfiguration.Add(25, SelectedZone26.IsVisible);
            ZonesConfiguration.Add(26, SelectedZone27.IsVisible);
            ZonesConfiguration.Add(27, SelectedZone28.IsVisible);
            ZonesConfiguration.Add(28, SelectedZone29.IsVisible);
            ZonesConfiguration.Add(29, SelectedZone30.IsVisible);
            ZonesConfiguration.Add(30, SelectedZone31.IsVisible);
            ZonesConfiguration.Add(31, SelectedZone32.IsVisible);

            Dictionary<int, bool> PhonesConfiguration = new Dictionary<int, bool>();
            PhonesConfiguration.Add(0, SelectedPhone1.IsVisible);
            PhonesConfiguration.Add(1, SelectedPhone2.IsVisible);
            PhonesConfiguration.Add(2, SelectedPhone3.IsVisible);
            PhonesConfiguration.Add(3, SelectedPhone4.IsVisible);
            PhonesConfiguration.Add(4, SelectedPhone5.IsVisible);
            PhonesConfiguration.Add(5, SelectedPhone6.IsVisible);
            PhonesConfiguration.Add(6, SelectedPhone7.IsVisible);
            PhonesConfiguration.Add(7, SelectedPhone8.IsVisible);
            PhonesConfiguration.Add(8, SelectedPhone9.IsVisible);
            PhonesConfiguration.Add(9, SelectedPhone10.IsVisible);
            PhonesConfiguration.Add(10, SelectedPhone11.IsVisible);
            PhonesConfiguration.Add(11, SelectedPhone12.IsVisible);
            PhonesConfiguration.Add(12, SelectedPhone13.IsVisible);
            PhonesConfiguration.Add(13, SelectedPhone14.IsVisible);
            PhonesConfiguration.Add(14, SelectedPhone15.IsVisible);
            PhonesConfiguration.Add(15, SelectedPhone16.IsVisible);

            Dictionary<int, bool> UsersConfiguration = new Dictionary<int, bool>();
            UsersConfiguration.Add(0, SelectedUser1.IsVisible);
            UsersConfiguration.Add(1, SelectedUser2.IsVisible);
            UsersConfiguration.Add(2, SelectedUser3.IsVisible);
            UsersConfiguration.Add(3, SelectedUser4.IsVisible);
            UsersConfiguration.Add(4, SelectedUser5.IsVisible);
            UsersConfiguration.Add(5, SelectedUser6.IsVisible);
            UsersConfiguration.Add(6, SelectedUser7.IsVisible);
            UsersConfiguration.Add(7, SelectedUser8.IsVisible);
            UsersConfiguration.Add(8, SelectedUser9.IsVisible);
            UsersConfiguration.Add(9, SelectedUser10.IsVisible);
            UsersConfiguration.Add(10, SelectedUser11.IsVisible);
            UsersConfiguration.Add(11, SelectedUser12.IsVisible);
            UsersConfiguration.Add(12, SelectedUser13.IsVisible);
            UsersConfiguration.Add(13, SelectedUser14.IsVisible);
            UsersConfiguration.Add(14, SelectedUser15.IsVisible);
            UsersConfiguration.Add(15, SelectedUser16.IsVisible);

            var prodigy_configtool_window = new MainWindow(AppLocale, AppRole, config_file_name, KeypadConfiguration, DialerConfiguration, ZonesConfiguration, PhonesConfiguration, UsersConfiguration);
            prodigy_configtool_window.Show();

            this.Close();
        }

        #region KEYPADS
        private void DragKeypad1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad1.Visibility = Visibility.Hidden;
            SelectedKeypad1.Visibility = Visibility.Visible;
        }
        private void DragKeypad2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad2.Visibility = Visibility.Hidden;
            SelectedKeypad2.Visibility = Visibility.Visible;
        }
        private void DragKeypad3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad3.Visibility = Visibility.Hidden;
            SelectedKeypad3.Visibility = Visibility.Visible;
        }
        private void DragKeypad4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad4.Visibility = Visibility.Hidden;
            SelectedKeypad4.Visibility = Visibility.Visible;
        }
        private void DragKeypad5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad5.Visibility = Visibility.Hidden;
            SelectedKeypad5.Visibility = Visibility.Visible;
        }
        private void DragKeypad6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad6.Visibility = Visibility.Hidden;
            SelectedKeypad6.Visibility = Visibility.Visible;
        }
        private void DragKeypad7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad7.Visibility = Visibility.Hidden;
            SelectedKeypad7.Visibility = Visibility.Visible;
        }
        private void DragKeypad8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragKeypad8.Visibility = Visibility.Hidden;
            SelectedKeypad8.Visibility = Visibility.Visible;
        }

        private void SelectedKeypad1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad1.Visibility = Visibility.Hidden;
            DragKeypad1.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad2.Visibility = Visibility.Hidden;
            DragKeypad2.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad3.Visibility = Visibility.Hidden;
            DragKeypad3.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad4.Visibility = Visibility.Hidden;
            DragKeypad4.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad5.Visibility = Visibility.Hidden;
            DragKeypad5.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad6.Visibility = Visibility.Hidden;
            DragKeypad6.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad7.Visibility = Visibility.Hidden;
            DragKeypad7.Visibility = Visibility.Visible;
        }
        private void SelectedKeypad8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedKeypad8.Visibility = Visibility.Hidden;
            DragKeypad8.Visibility = Visibility.Visible;
        }

        #endregion

        #region DIALER
        private void SelectedDialer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SelectedDialer.Visibility = Visibility.Hidden;
            DragDialer.Visibility = Visibility.Visible;
        }
        private void DragDialer_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragDialer.Visibility = Visibility.Hidden;
            SelectedDialer.Visibility = Visibility.Visible;
        }
        #endregion

        #region ZONES
        private void DragZone1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone1.Visibility = Visibility.Hidden;
            SelectedZone1.Visibility = Visibility.Visible;
        }
        private void DragZone2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone2.Visibility = Visibility.Hidden;
            SelectedZone2.Visibility = Visibility.Visible;
        }
        private void DragZone3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone3.Visibility = Visibility.Hidden;
            SelectedZone3.Visibility = Visibility.Visible;
        }
        private void DragZone4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone4.Visibility = Visibility.Hidden;
            SelectedZone4.Visibility = Visibility.Visible;
        }
        private void DragZone5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone5.Visibility = Visibility.Hidden;
            SelectedZone5.Visibility = Visibility.Visible;
        }
        private void DragZone6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone6.Visibility = Visibility.Hidden;
            SelectedZone6.Visibility = Visibility.Visible;
        }
        private void DragZone7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone7.Visibility = Visibility.Hidden;
            SelectedZone7.Visibility = Visibility.Visible;
        }
        private void DragZone8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone8.Visibility = Visibility.Hidden;
            SelectedZone8.Visibility = Visibility.Visible;
        }
        private void DragZone9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone9.Visibility = Visibility.Hidden;
            SelectedZone9.Visibility = Visibility.Visible;
        }
        private void DragZone10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone10.Visibility = Visibility.Hidden;
            SelectedZone10.Visibility = Visibility.Visible;
        }
        private void DragZone11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone11.Visibility = Visibility.Hidden;
            SelectedZone11.Visibility = Visibility.Visible;
        }
        private void DragZone12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone12.Visibility = Visibility.Hidden;
            SelectedZone12.Visibility = Visibility.Visible;
        }
        private void DragZone13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone13.Visibility = Visibility.Hidden;
            SelectedZone13.Visibility = Visibility.Visible;
        }
        private void DragZone14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone14.Visibility = Visibility.Hidden;
            SelectedZone14.Visibility = Visibility.Visible;
        }
        private void DragZone15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone15.Visibility = Visibility.Hidden;
            SelectedZone15.Visibility = Visibility.Visible;
        }
        private void DragZone16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone16.Visibility = Visibility.Hidden;
            SelectedZone16.Visibility = Visibility.Visible;
        }
        private void DragZone17_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone17.Visibility = Visibility.Hidden;
            SelectedZone17.Visibility = Visibility.Visible;
        }
        private void DragZone18_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone18.Visibility = Visibility.Hidden;
            SelectedZone18.Visibility = Visibility.Visible;
        }
        private void DragZone19_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone19.Visibility = Visibility.Hidden;
            SelectedZone19.Visibility = Visibility.Visible;
        }
        private void DragZone20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone20.Visibility = Visibility.Hidden;
            SelectedZone20.Visibility = Visibility.Visible;
        }
        private void DragZone21_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone21.Visibility = Visibility.Hidden;
            SelectedZone21.Visibility = Visibility.Visible;
        }
        private void DragZone22_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone22.Visibility = Visibility.Hidden;
            SelectedZone22.Visibility = Visibility.Visible;
        }
        private void DragZone23_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone23.Visibility = Visibility.Hidden;
            SelectedZone23.Visibility = Visibility.Visible;
        }
        private void DragZone24_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone24.Visibility = Visibility.Hidden;
            SelectedZone24.Visibility = Visibility.Visible;
        }
        private void DragZone25_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone25.Visibility = Visibility.Hidden;
            SelectedZone25.Visibility = Visibility.Visible;
        }
        private void DragZone26_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone26.Visibility = Visibility.Hidden;
            SelectedZone26.Visibility = Visibility.Visible;
        }
        private void DragZone27_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone27.Visibility = Visibility.Hidden;
            SelectedZone27.Visibility = Visibility.Visible;
        }
        private void DragZone28_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone28.Visibility = Visibility.Hidden;
            SelectedZone28.Visibility = Visibility.Visible;
        }
        private void DragZone29_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone29.Visibility = Visibility.Hidden;
            SelectedZone29.Visibility = Visibility.Visible;
        }
        private void DragZone30_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone30.Visibility = Visibility.Hidden;
            SelectedZone30.Visibility = Visibility.Visible;
        }
        private void DragZone31_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone31.Visibility = Visibility.Hidden;
            SelectedZone31.Visibility = Visibility.Visible;
        }
        private void DragZone32_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone32.Visibility = Visibility.Hidden;
            SelectedZone32.Visibility = Visibility.Visible;
        }

        private void SelectedZone1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone1.Visibility = Visibility.Visible;
            SelectedZone1.Visibility = Visibility.Hidden;
        }
        private void SelectedZone2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone2.Visibility = Visibility.Visible;
            SelectedZone2.Visibility = Visibility.Hidden;
        }
        private void SelectedZone3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone3.Visibility = Visibility.Visible;
            SelectedZone3.Visibility = Visibility.Hidden;
        }
        private void SelectedZone4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone4.Visibility = Visibility.Visible;
            SelectedZone4.Visibility = Visibility.Hidden;
        }
        private void SelectedZone5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone5.Visibility = Visibility.Visible;
            SelectedZone5.Visibility = Visibility.Hidden;
        }
        private void SelectedZone6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone6.Visibility = Visibility.Visible;
            SelectedZone6.Visibility = Visibility.Hidden;
        }
        private void SelectedZone7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone7.Visibility = Visibility.Visible;
            SelectedZone7.Visibility = Visibility.Hidden;
        }
        private void SelectedZone8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone8.Visibility = Visibility.Visible;
            SelectedZone8.Visibility = Visibility.Hidden;
        }
        private void SelectedZone9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone9.Visibility = Visibility.Visible;
            SelectedZone9.Visibility = Visibility.Hidden;
        }
        private void SelectedZone10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone10.Visibility = Visibility.Visible;
            SelectedZone10.Visibility = Visibility.Hidden;
        }
        private void SelectedZone11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone11.Visibility = Visibility.Visible;
            SelectedZone11.Visibility = Visibility.Hidden;
        }
        private void SelectedZone12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone12.Visibility = Visibility.Visible;
            SelectedZone12.Visibility = Visibility.Hidden;
        }
        private void SelectedZone13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone13.Visibility = Visibility.Visible;
            SelectedZone13.Visibility = Visibility.Hidden;
        }
        private void SelectedZone14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone14.Visibility = Visibility.Visible;
            SelectedZone14.Visibility = Visibility.Hidden;
        }
        private void SelectedZone15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone15.Visibility = Visibility.Visible;
            SelectedZone15.Visibility = Visibility.Hidden;
        }
        private void SelectedZone16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone16.Visibility = Visibility.Visible;
            SelectedZone16.Visibility = Visibility.Hidden;
        }
        private void SelectedZone17_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone17.Visibility = Visibility.Visible;
            SelectedZone17.Visibility = Visibility.Hidden;
        }
        private void SelectedZone18_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone18.Visibility = Visibility.Visible;
            SelectedZone18.Visibility = Visibility.Hidden;
        }
        private void SelectedZone19_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone19.Visibility = Visibility.Visible;
            SelectedZone19.Visibility = Visibility.Hidden;
        }
        private void SelectedZone20_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone20.Visibility = Visibility.Visible;
            SelectedZone20.Visibility = Visibility.Hidden;
        }
        private void SelectedZone21_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone21.Visibility = Visibility.Visible;
            SelectedZone21.Visibility = Visibility.Hidden;
        }
        private void SelectedZone22_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone22.Visibility = Visibility.Visible;
            SelectedZone22.Visibility = Visibility.Hidden;
        }
        private void SelectedZone23_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone23.Visibility = Visibility.Visible;
            SelectedZone23.Visibility = Visibility.Hidden;
        }
        private void SelectedZone24_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone24.Visibility = Visibility.Visible;
            SelectedZone24.Visibility = Visibility.Hidden;
        }
        private void SelectedZone25_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone25.Visibility = Visibility.Visible;
            SelectedZone25.Visibility = Visibility.Hidden;
        }
        private void SelectedZone26_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone26.Visibility = Visibility.Visible;
            SelectedZone26.Visibility = Visibility.Hidden;
        }
        private void SelectedZone27_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone27.Visibility = Visibility.Visible;
            SelectedZone27.Visibility = Visibility.Hidden;
        }
        private void SelectedZone28_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone28.Visibility = Visibility.Visible;
            SelectedZone28.Visibility = Visibility.Hidden;
        }
        private void SelectedZone29_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone29.Visibility = Visibility.Visible;
            SelectedZone29.Visibility = Visibility.Hidden;
        }
        private void SelectedZone30_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone30.Visibility = Visibility.Visible;
            SelectedZone30.Visibility = Visibility.Hidden;
        }
        private void SelectedZone31_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone31.Visibility = Visibility.Visible;
            SelectedZone31.Visibility = Visibility.Hidden;
        }
        private void SelectedZone32_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragZone32.Visibility = Visibility.Visible;
            SelectedZone32.Visibility = Visibility.Hidden;
        }

        #endregion

        #region PHONES
        private void DragPhone1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone1.Visibility = Visibility.Hidden;
            SelectedPhone1.Visibility = Visibility.Visible;
        }
        private void DragPhone2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone2.Visibility = Visibility.Hidden;
            SelectedPhone2.Visibility = Visibility.Visible;
        }
        private void DragPhone3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone3.Visibility = Visibility.Hidden;
            SelectedPhone3.Visibility = Visibility.Visible;
        }
        private void DragPhone4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone4.Visibility = Visibility.Hidden;
            SelectedPhone4.Visibility = Visibility.Visible;
        }
        private void DragPhone5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone5.Visibility = Visibility.Hidden;
            SelectedPhone5.Visibility = Visibility.Visible;
        }
        private void DragPhone6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone6.Visibility = Visibility.Hidden;
            SelectedPhone6.Visibility = Visibility.Visible;
        }
        private void DragPhone7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone7.Visibility = Visibility.Hidden;
            SelectedPhone7.Visibility = Visibility.Visible;
        }
        private void DragPhone8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone8.Visibility = Visibility.Hidden;
            SelectedPhone8.Visibility = Visibility.Visible;
        }
        private void DragPhone9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone9.Visibility = Visibility.Hidden;
            SelectedPhone9.Visibility = Visibility.Visible;
        }
        private void DragPhone10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone10.Visibility = Visibility.Hidden;
            SelectedPhone10.Visibility = Visibility.Visible;
        }
        private void DragPhone11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone11.Visibility = Visibility.Hidden;
            SelectedPhone11.Visibility = Visibility.Visible;
        }
        private void DragPhone12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone12.Visibility = Visibility.Hidden;
            SelectedPhone12.Visibility = Visibility.Visible;
        }
        private void DragPhone13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone13.Visibility = Visibility.Hidden;
            SelectedPhone13.Visibility = Visibility.Visible;
        }
        private void DragPhone14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone14.Visibility = Visibility.Hidden;
            SelectedPhone14.Visibility = Visibility.Visible;
        }
        private void DragPhone15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone15.Visibility = Visibility.Hidden;
            SelectedPhone15.Visibility = Visibility.Visible;
        }
        private void DragPhone16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone16.Visibility = Visibility.Hidden;
            SelectedPhone16.Visibility = Visibility.Visible;
        }


        private void SelectedPhone1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone1.Visibility = Visibility.Visible;
            SelectedPhone1.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone2.Visibility = Visibility.Visible;
            SelectedPhone2.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone3.Visibility = Visibility.Visible;
            SelectedPhone3.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone4.Visibility = Visibility.Visible;
            SelectedPhone4.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone5.Visibility = Visibility.Visible;
            SelectedPhone5.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone6.Visibility = Visibility.Visible;
            SelectedPhone6.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone7.Visibility = Visibility.Visible;
            SelectedPhone7.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone8.Visibility = Visibility.Visible;
            SelectedPhone8.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone9.Visibility = Visibility.Visible;
            SelectedPhone9.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone10.Visibility = Visibility.Visible;
            SelectedPhone10.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone11.Visibility = Visibility.Visible;
            SelectedPhone11.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone12.Visibility = Visibility.Visible;
            SelectedPhone12.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone13.Visibility = Visibility.Visible;
            SelectedPhone13.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone14.Visibility = Visibility.Visible;
            SelectedPhone14.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone15.Visibility = Visibility.Visible;
            SelectedPhone15.Visibility = Visibility.Hidden;
        }
        private void SelectedPhone16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragPhone16.Visibility = Visibility.Visible;
            SelectedPhone16.Visibility = Visibility.Hidden;
        }

        #endregion

        #region USERS
        private void DragUser1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser1.Visibility = Visibility.Hidden;
            SelectedUser1.Visibility = Visibility.Visible;
        }
        private void DragUser2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser2.Visibility = Visibility.Hidden;
            SelectedUser2.Visibility = Visibility.Visible;
        }
        private void DragUser3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser3.Visibility = Visibility.Hidden;
            SelectedUser3.Visibility = Visibility.Visible;
        }
        private void DragUser4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser4.Visibility = Visibility.Hidden;
            SelectedUser4.Visibility = Visibility.Visible;
        }
        private void DragUser5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser5.Visibility = Visibility.Hidden;
            SelectedUser5.Visibility = Visibility.Visible;
        }
        private void DragUser6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser6.Visibility = Visibility.Hidden;
            SelectedUser6.Visibility = Visibility.Visible;
        }
        private void DragUser7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser7.Visibility = Visibility.Hidden;
            SelectedUser7.Visibility = Visibility.Visible;
        }
        private void DragUser8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser8.Visibility = Visibility.Hidden;
            SelectedUser8.Visibility = Visibility.Visible;
        }
        private void DragUser9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser9.Visibility = Visibility.Hidden;
            SelectedUser9.Visibility = Visibility.Visible;
        }
        private void DragUser10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser10.Visibility = Visibility.Hidden;
            SelectedUser10.Visibility = Visibility.Visible;
        }
        private void DragUser11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser11.Visibility = Visibility.Hidden;
            SelectedUser11.Visibility = Visibility.Visible;
        }
        private void DragUser12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser12.Visibility = Visibility.Hidden;
            SelectedUser12.Visibility = Visibility.Visible;
        }
        private void DragUser13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser13.Visibility = Visibility.Hidden;
            SelectedUser13.Visibility = Visibility.Visible;
        }
        private void DragUser14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser14.Visibility = Visibility.Hidden;
            SelectedUser14.Visibility = Visibility.Visible;
        }
        private void DragUser15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser15.Visibility = Visibility.Hidden;
            SelectedUser15.Visibility = Visibility.Visible;
        }
        private void DragUser16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser16.Visibility = Visibility.Hidden;
            SelectedUser16.Visibility = Visibility.Visible;
        }

        private void SelectedUser1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser1.Visibility = Visibility.Visible;
            SelectedUser1.Visibility = Visibility.Hidden;
        }
        private void SelectedUser2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser2.Visibility = Visibility.Visible;
            SelectedUser2.Visibility = Visibility.Hidden;
        }
        private void SelectedUser3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser3.Visibility = Visibility.Visible;
            SelectedUser3.Visibility = Visibility.Hidden;
        }
        private void SelectedUser4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser4.Visibility = Visibility.Visible;
            SelectedUser4.Visibility = Visibility.Hidden;
        }
        private void SelectedUser5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser5.Visibility = Visibility.Visible;
            SelectedUser5.Visibility = Visibility.Hidden;
        }
        private void SelectedUser6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser6.Visibility = Visibility.Visible;
            SelectedUser6.Visibility = Visibility.Hidden;
        }
        private void SelectedUser7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser7.Visibility = Visibility.Visible;
            SelectedUser7.Visibility = Visibility.Hidden;
        }
        private void SelectedUser8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser8.Visibility = Visibility.Visible;
            SelectedUser8.Visibility = Visibility.Hidden;
        }
        private void SelectedUser9_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser9.Visibility = Visibility.Visible;
            SelectedUser9.Visibility = Visibility.Hidden;
        }
        private void SelectedUser10_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser10.Visibility = Visibility.Visible;
            SelectedUser10.Visibility = Visibility.Hidden;
        }
        private void SelectedUser11_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser11.Visibility = Visibility.Visible;
            SelectedUser11.Visibility = Visibility.Hidden;
        }
        private void SelectedUser12_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser12.Visibility = Visibility.Visible;
            SelectedUser12.Visibility = Visibility.Hidden;
        }
        private void SelectedUser13_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser13.Visibility = Visibility.Visible;
            SelectedUser13.Visibility = Visibility.Hidden;
        }
        private void SelectedUser14_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser14.Visibility = Visibility.Visible;
            SelectedUser14.Visibility = Visibility.Hidden;
        }
        private void SelectedUser15_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser15.Visibility = Visibility.Visible;
            SelectedUser15.Visibility = Visibility.Hidden;
        }
        private void SelectedUser16_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragUser16.Visibility = Visibility.Visible;
            SelectedUser16.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
