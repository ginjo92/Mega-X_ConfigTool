using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : MetroWindow
    {
        private MainWindow ParentWindow;
        public Settings(MainWindow parent_window)
        {
            ParentWindow = parent_window;
            InitializeComponent();

            if (Thread.CurrentThread.CurrentCulture.Name.Equals("en-US"))
            {
                RadioLocaleEN.IsChecked = true;
                RadioLocalePT.IsChecked = false;
            }
            else if (Thread.CurrentThread.CurrentCulture.Name.Equals("pt-PT"))
            {
                RadioLocaleEN.IsChecked = false;
                RadioLocalePT.IsChecked = true;
            }
        }

        private void TitleBarHelpButton_Click(object sender, RoutedEventArgs e)
        {
            if (HelpFlyout.IsOpen)
                HelpFlyout.IsOpen = false;
            else
                HelpFlyout.IsOpen = true;
        }

        private void RadioLocalePT_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("PT-PT");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("PT-PT");
            Properties.Settings.Default.DefaultCulture = "pt-PT";
            Properties.Settings.Default.Save();
            if (ParentWindow.databaseDataSet.HasChanges())
            {
                var messageBox = MessageBox.Show(Properties.Resources.QuestionSaveChangesExtended + "\n" + "\n" + Properties.Resources.InfoDataWillBeLost, Properties.Resources.QuestionSaveChanges, MessageBoxButton.YesNoCancel);
                if (messageBox == MessageBoxResult.Cancel)
                {
                    RadioLocaleEN.IsChecked = true;
                    RadioLocalePT.IsChecked = false;
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
                    Properties.Settings.Default.DefaultCulture = "en-US";
                    Properties.Settings.Default.Save();
                    return;
                }
                else if (messageBox == MessageBoxResult.Yes)
                {
                    ParentWindow.Save_Database_data();

                }
                else if (messageBox == MessageBoxResult.No)
                {
                    ParentWindow.Closing -= ParentWindow.BaseWindow_Closing;

                }
                    
            }

            

            ParentWindow.Close();
            ParentWindow.Closing += ParentWindow.BaseWindow_Closing;
            MainWindow window1 = new MainWindow("pt-PT", ParentWindow.AppRole, ParentWindow.AppDbFile, null, null, null, null);
            window1.Show();
            
            this.Close();


        }

        private void RadioLocaleEN_Click(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("en-US");
            Properties.Settings.Default.DefaultCulture = "en-US";
            Properties.Settings.Default.Save();

            if (ParentWindow.databaseDataSet.HasChanges())
            {
                var messageBox = MessageBox.Show(Properties.Resources.QuestionSaveChangesExtended + "\n" + "\n" + Properties.Resources.InfoDataWillBeLost, Properties.Resources.QuestionSaveChanges, MessageBoxButton.YesNoCancel);
                if (messageBox == MessageBoxResult.Cancel)
                {
                    RadioLocaleEN.IsChecked = false;
                    RadioLocalePT.IsChecked = true;
                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("PT-PT");
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("PT-PT");
                    Properties.Settings.Default.DefaultCulture = "pt-PT";
                    Properties.Settings.Default.Save();
                    return;
                }
                else if (messageBox == MessageBoxResult.Yes)
                {
                    ParentWindow.Save_Database_data();

                }
            }

            Properties.Settings.Default.DefaultCulture = "en-US";
            Properties.Settings.Default.Save();

            
            
            ParentWindow.Close();
            MainWindow window1 = new MainWindow("en-US", ParentWindow.AppRole, ParentWindow.AppDbFile, null, null, null, null);
            window1.Show();
            
            this.Close();

        }
    }
}
