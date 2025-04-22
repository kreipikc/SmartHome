using SmartHome.Pages.Devices.Settings;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SmartHome.Pages.Users.Preferences
{
    /// <summary>
    /// Логика взаимодействия для UserPreferencesPage.xaml
    /// </summary>
    public partial class UserPreferencesPage : Page
    {
        public static Database.User_Preferences UserPreferencesCurrent;

        public UserPreferencesPage()
        {
            InitializeComponent();
            UsersCurrentText.Text = UsersPage.UserCurrent.username;
            UpdateData();
        }

        private void UpdateData()
        {
            ListViewUsersPreferences.ItemsSource = Core.DB.User_Preferences.Where(p => p.user_id == UsersPage.UserCurrent.user_id).ToList();
        }

        private void AddUsersPreferences_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUsersPreferencesPage());
        }

        private void EditUsersPreferences_Click(object sender, RoutedEventArgs e)
        {
            UserPreferencesCurrent = ListViewUsersPreferences.SelectedItem as Database.User_Preferences;
            if (UserPreferencesCurrent != null)
            {
                NavigationService.Navigate(new EditUsersPreferencesPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteUsersPreferences_Click(object sender, RoutedEventArgs e)
        {
            Database.User_Preferences Preferences = ListViewUsersPreferences.SelectedItem as Database.User_Preferences;
            if (Preferences != null)
            {
                try
                {
                    Core.DB.User_Preferences.Remove(Preferences);
                    Core.DB.SaveChanges();
                    UpdateData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void RefreshUsersPreferences_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
    }
}
