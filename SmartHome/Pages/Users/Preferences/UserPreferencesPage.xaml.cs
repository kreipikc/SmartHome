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
        private List<Database.User_Preferences> allUsersPreferences;

        public UserPreferencesPage()
        {
            InitializeComponent();
            UsersCurrentText.Text = UsersPage.UserCurrent.email;
            UpdateData();
            SortUsersPreferencesCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID" },
                new Category { NameOfCategory = "По названию" },
                new Category { NameOfCategory = "По значению" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allUsersPreferences = Core.DB.User_Preferences.Where(p => p.user_id == UsersPage.UserCurrent.user_id).ToList();
            FilterAndSortUsersPreferences();
        }

        private void FilterAndSortUsersPreferences()
        {
            var filteredDevices = allUsersPreferences.AsQueryable();

            // Фильтрация по названию
            if (!string.IsNullOrEmpty(SearchUsersPreferencesName.Text))
            {
                filteredDevices = filteredDevices.Where(d => d.preference_name.IndexOf(SearchUsersPreferencesName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Сортировка
            switch (SortUsersPreferencesCategory.SelectedIndex)
            {
                case 0: // По ID
                    filteredDevices = filteredDevices.OrderBy(d => d.user_id);
                    break;
                case 1: // По названию
                    filteredDevices = filteredDevices.OrderBy(d => d.preference_name);
                    break;
                case 2: // По значению
                    filteredDevices = filteredDevices.OrderBy(d => d.preference_value);
                    break;
                case 3: // По дате создания
                    filteredDevices = filteredDevices.OrderBy(d => d.created_at);
                    break;
            }

            ListViewUsersPreferences.ItemsSource = filteredDevices.ToList();
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
                    SmartHome.Utils.PrintError(ex);
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

        private void SearchUsersPreferencesName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortUsersPreferences();
        }

        private void SortUsersPreferencesCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortUsersPreferences();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchUsersPreferencesName.Text = string.Empty;
            SortUsersPreferencesCategory.SelectedIndex = 0;
            FilterAndSortUsersPreferences();
        }
    }
}
