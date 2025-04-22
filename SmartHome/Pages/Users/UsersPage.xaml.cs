using SmartHome.Database;
using SmartHome.Pages.Rooms;
using SmartHome.Pages.Users.Preferences;
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

namespace SmartHome.Pages.Users
{
    /// <summary>
    /// Логика взаимодействия для UsersPage.xaml
    /// </summary>
    public partial class UsersPage : Page
    {
        public static Database.Users UserCurrent;
        private List<Database.Users> allUsers;

        public UsersPage()
        {
            InitializeComponent();
            UpdateData();
            SortUsersCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID" },
                new Category { NameOfCategory = "По имени" },
                new Category { NameOfCategory = "По почте" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allUsers = Core.DB.Users.ToList();
            FilterAndSortUsers();
        }

        private void FilterAndSortUsers()
        {
            var filteredDevices = allUsers.AsQueryable();

            // Фильтрация по названию
            if (!string.IsNullOrEmpty(SearchUsersName.Text))
            {
                filteredDevices = filteredDevices.Where(d => d.username.IndexOf(SearchUsersName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Сортировка
            switch (SortUsersCategory.SelectedIndex)
            {
                case 0: // По ID
                    filteredDevices = filteredDevices.OrderBy(d => d.user_id);
                    break;
                case 1: // По названию
                    filteredDevices = filteredDevices.OrderBy(d => d.username);
                    break;
                case 2: // По почте
                    filteredDevices = filteredDevices.OrderBy(d => d.email);
                    break;
                case 3: // По дате создания
                    filteredDevices = filteredDevices.OrderBy(d => d.created_at);
                    break;
            }

            DataGridUsers.ItemsSource = filteredDevices.ToList();
        }

        private void AddUsers_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUsersPage());
        }

        private void EditUsers_Click(object sender, RoutedEventArgs e)
        {
            UserCurrent = DataGridUsers.SelectedItem as Database.Users;
            if (UserCurrent != null)
            {
                NavigationService.Navigate(new EditUsersPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteUsers_Click(object sender, RoutedEventArgs e)
        {
            Database.Users us = DataGridUsers.SelectedItem as Database.Users;
            if (us != null)
            {
                try
                {
                    Core.DB.Users.Remove(us);
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

        private void RefreshUsers_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void Preferences_Click(object sender, RoutedEventArgs e)
        {
            UserCurrent = DataGridUsers.SelectedItem as Database.Users;
            if (UserCurrent != null)
            {
                NavigationService.Navigate(new UserPreferencesPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void SearchUsersName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortUsers();
        }

        private void SortUsersCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortUsers();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchUsersName.Text = string.Empty;
            SortUsersCategory.SelectedIndex = 0;
            FilterAndSortUsers();
        }
    }
}
