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
            var prodigy_configtool_window = new MainWindow(AppLocale, AppRole, config_file_name, null, null, null, null);
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

            Dictionary<int, bool> PartitionsConfiguration = new Dictionary<int, bool>();
            PartitionsConfiguration.Add(0, SelectedArea1.IsVisible);
            PartitionsConfiguration.Add(1, SelectedArea2.IsVisible);
            PartitionsConfiguration.Add(2, SelectedArea3.IsVisible);
            PartitionsConfiguration.Add(3, SelectedArea4.IsVisible);
            PartitionsConfiguration.Add(4, SelectedArea5.IsVisible);
            PartitionsConfiguration.Add(5, SelectedArea6.IsVisible);
            PartitionsConfiguration.Add(6, SelectedArea7.IsVisible);
            PartitionsConfiguration.Add(7, SelectedArea8.IsVisible);

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

            var prodigy_configtool_window = new MainWindow(AppLocale, AppRole, config_file_name, KeypadConfiguration, DialerConfiguration, PartitionsConfiguration, PhonesConfiguration);
            prodigy_configtool_window.Show();

            this.Close();
    }

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

        private void DragArea1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea1.Visibility = Visibility.Hidden;
            SelectedArea1.Visibility = Visibility.Visible;
        }
        private void DragArea2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea2.Visibility = Visibility.Hidden;
            SelectedArea2.Visibility = Visibility.Visible;
        }
        private void DragArea3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea3.Visibility = Visibility.Hidden;
            SelectedArea3.Visibility = Visibility.Visible;
        }
        private void DragArea4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea4.Visibility = Visibility.Hidden;
            SelectedArea4.Visibility = Visibility.Visible;
        }
        private void DragArea5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea5.Visibility = Visibility.Hidden;
            SelectedArea5.Visibility = Visibility.Visible;
        }
        private void DragArea6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea6.Visibility = Visibility.Hidden;
            SelectedArea6.Visibility = Visibility.Visible;
        }
        private void DragArea7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea7.Visibility = Visibility.Hidden;
            SelectedArea7.Visibility = Visibility.Visible;
        }
        private void DragArea8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea8.Visibility = Visibility.Hidden;
            SelectedArea8.Visibility = Visibility.Visible;
        }

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

        private void SelectedArea1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea1.Visibility = Visibility.Visible;
            SelectedArea1.Visibility = Visibility.Hidden;
        }
        private void SelectedArea2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea2.Visibility = Visibility.Visible;
            SelectedArea2.Visibility = Visibility.Hidden;
        }
        private void SelectedArea3_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea3.Visibility = Visibility.Visible;
            SelectedArea3.Visibility = Visibility.Hidden;
        }
        private void SelectedArea4_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea4.Visibility = Visibility.Visible;
            SelectedArea4.Visibility = Visibility.Hidden;
        }
        private void SelectedArea5_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea5.Visibility = Visibility.Visible;
            SelectedArea5.Visibility = Visibility.Hidden;
        }
        private void SelectedArea6_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea6.Visibility = Visibility.Visible;
            SelectedArea6.Visibility = Visibility.Hidden;
        }
        private void SelectedArea7_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea7.Visibility = Visibility.Visible;
            SelectedArea7.Visibility = Visibility.Hidden;
        }
        private void SelectedArea8_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragArea8.Visibility = Visibility.Visible;
            SelectedArea8.Visibility = Visibility.Hidden;
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

    }
}
