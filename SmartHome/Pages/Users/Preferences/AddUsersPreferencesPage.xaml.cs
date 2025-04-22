using SmartHome.Pages.Devices;
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
    /// Логика взаимодействия для AddUsersPreferencesPage.xaml
    /// </summary>
    public partial class AddUsersPreferencesPage : Page
    {
        public AddUsersPreferencesPage()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            string Value = ValueTextBox.Text;
            CreatePreference(Name, Value);
        }

        private bool CreatePreference(string Name, string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Value))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (Core.DB.User_Preferences.Any(u => u.preference_name == Name && u.user_id == UsersPage.UserCurrent.user_id))
                {
                    MessageBox.Show($"Настройка с таким названием уже существует у пользователя '{UsersPage.UserCurrent.username}'");
                    return false;
                }

                var newPreference = new Database.User_Preferences
                {
                    user_id = UsersPage.UserCurrent.user_id,
                    preference_name = Name,
                    preference_value = Value,
                    created_at = DateTime.Now
                };

                Core.DB.User_Preferences.Add(newPreference);
                Core.DB.SaveChanges();

                MessageBox.Show("Настройка для пользователя успешно создана");
                NavigationService.GoBack();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
                return false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
