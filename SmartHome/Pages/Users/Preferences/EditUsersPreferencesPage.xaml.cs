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
    /// Логика взаимодействия для EditUsersPreferencesPage.xaml
    /// </summary>
    public partial class EditUsersPreferencesPage : Page
    {
        public EditUsersPreferencesPage()
        {
            InitializeComponent();
            AddData();
            UserPreferencesPage.UserPreferencesCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = UserPreferencesPage.UserPreferencesCurrent.preference_id.ToString();
                NameTextBox.Text = UserPreferencesPage.UserPreferencesCurrent.preference_name;
                ValueTextBox.Text = UserPreferencesPage.UserPreferencesCurrent.preference_value;
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Name = NameTextBox.Text;
            string Value = ValueTextBox.Text;
            UpdateUserPreferences(IdStr, Name, Value);
        }

        private bool UpdateUserPreferences(string IdStr, string Name, string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) || string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Value))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.User_Preferences.Any(u => u.preference_name == Name && u.user_id == UsersPage.UserCurrent.user_id && u.preference_id != Id))
                {
                    MessageBox.Show($"Настройка с таким названием уже существует у пользователя '{UsersPage.UserCurrent.username}'");
                    return false;
                }

                var preference = Core.DB.User_Preferences.FirstOrDefault(h => h.preference_id == Id);
                if (preference == null)
                {
                    MessageBox.Show("Настройка не найдена");
                    return false;
                }

                preference.preference_name = Name;
                preference.preference_value = Value;

                Core.DB.SaveChanges();

                MessageBox.Show("Настройка для пользователя успешно обновлена");
                NavigationService.GoBack();
                return true;
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
                return false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
