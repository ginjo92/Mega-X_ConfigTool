using ProdigyConfigToolWPF.defaultDataSetTableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProdigyConfigToolWPF.Controls
{
    /// <summary>
    /// Interaction logic for UserControlUsers.xaml
    /// </summary>
    public partial class UserControlUsers : UserControl
    {
        public UserControlUsers()
        {
            InitializeComponent();
        }

        private void UsersDownloadTile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UsersUploadTile_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UsersColumnHeaderClickedHandler(object sender, RoutedEventArgs e)
        {
            DataGridColumnHeader a = e.OriginalSource as DataGridColumnHeader;

            try
            {
                //ARM/DISARM Permissions
                if (a.Column.Header.Equals(Properties.Resources.User_arm_disarm_permissions_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_arm_disarm_permissions_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_arm_disarm_permissions_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_arm_disarm_permissions_minus).Visibility = Visibility.Visible;
                }

                //Areas
                if (a.Column.Header.Equals(Properties.Resources.User_areas_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_areas_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_areas_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_areas_minus).Visibility = Visibility.Visible;
                }


                //Timezones config
                if (a.Column.Header.Equals(Properties.Resources.User_timezone_config_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_timezone_config_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_timezone_config_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_timezone_config_minus).Visibility = Visibility.Visible;
                }

                //Button A - Away
                if (a.Column.Header.Equals(Properties.Resources.User_button_a_areas_away_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_a_areas_away_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_a_areas_away_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_a_areas_away_minus).Visibility = Visibility.Visible;
                }

                //Button B - Away
                if (a.Column.Header.Equals(Properties.Resources.User_button_b_areas_away_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_b_areas_away_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_b_areas_away_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_b_areas_away_minus).Visibility = Visibility.Visible;
                }

                //Button C - Away
                if (a.Column.Header.Equals(Properties.Resources.User_button_c_areas_away_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_c_areas_away_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_c_areas_away_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_c_areas_away_minus).Visibility = Visibility.Visible;
                }

                //Button D - Away
                if (a.Column.Header.Equals(Properties.Resources.User_button_d_areas_away_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_d_areas_away_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_d_areas_away_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_d_areas_away_minus).Visibility = Visibility.Visible;
                }


                //Button A - STAY
                if (a.Column.Header.Equals(Properties.Resources.User_button_a_areas_stay_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_a_areas_stay_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_a_areas_stay_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_a_areas_stay_minus).Visibility = Visibility.Visible;
                }

                //Button B - STAY
                if (a.Column.Header.Equals(Properties.Resources.User_button_b_areas_stay_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_b_areas_stay_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_b_areas_stay_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_b_areas_stay_minus).Visibility = Visibility.Visible;
                }

                //Button C - STAY
                if (a.Column.Header.Equals(Properties.Resources.User_button_c_areas_stay_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_c_areas_stay_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_c_areas_stay_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_c_areas_stay_minus).Visibility = Visibility.Visible;
                }

                //Button D - STAY
                if (a.Column.Header.Equals(Properties.Resources.User_button_d_areas_stay_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_d_areas_stay_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.User_button_d_areas_stay_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.User_button_d_areas_stay_minus).Visibility = Visibility.Visible;
                }

                //AUDIO tracks 
                if (a.Column.Header.Equals(Properties.Resources.Zone_audio_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.Zone_audio_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.Zone_audio_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.Zone_audio_minus).Visibility = Visibility.Visible;
                }

                //Output permissions 
                if (a.Column.Header.Equals(Properties.Resources.Output_Permissions_Call_minus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.Output_Permissions_Call_plus).Visibility = Visibility.Visible;
                }
                else if (a.Column.Header.Equals(Properties.Resources.Output_Permissions_Call_plus))
                {
                    a.Column.Visibility = a.Column.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                    userDataGrid.Columns.Single(c => c.Header.ToString() == Properties.Resources.Output_Permissions_Call_minus).Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Message", MessageBoxButton.OK, MessageBoxImage.Error); // TODO: delete/improve
            }

        }
        
    }
}
