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

        public UsersPage()
        {
            InitializeComponent();
            UpdateData();
        }

        private void UpdateData()
        {
            DataGridUsers.ItemsSource = Core.DB.Users.ToList();
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
    }
}
