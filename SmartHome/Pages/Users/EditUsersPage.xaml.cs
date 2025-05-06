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
    /// Логика взаимодействия для EditUsersPage.xaml
    /// </summary>
    public partial class EditUsersPage : Page
    {
        public EditUsersPage()
        {
            InitializeComponent();
            AddData();
            UsersPage.UserCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = UsersPage.UserCurrent.user_id.ToString();
                EmailTextBox.Text = UsersPage.UserCurrent.email;
                UsernameTextBox.Text = UsersPage.UserCurrent.username;
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Email = EmailTextBox.Text;
            string Username = UsernameTextBox.Text;
            UpdateUser(IdStr, Email, Username);
        }

        private bool UpdateUser(string IdStr, string Email, string Username)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(Email) ||
                    string.IsNullOrEmpty(Username))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.Users.Any(u => u.email == Email && u.user_id != Id))
                {
                    MessageBox.Show("Пользователь с такой почтой уже существует");
                    return false;
                }

                var user = Core.DB.Users.FirstOrDefault(h => h.user_id == Id);
                if (user == null)
                {
                    MessageBox.Show("Пользователь не найден");
                    return false;
                }

                user.email = Email;
                user.username = Username;

                Core.DB.SaveChanges();
                MessageBox.Show("Пользователь успешно обновлен");

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
