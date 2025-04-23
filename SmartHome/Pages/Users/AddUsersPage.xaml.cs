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
    /// Логика взаимодействия для AddUsersPage.xaml
    /// </summary>
    public partial class AddUsersPage : Page
    {
        public AddUsersPage()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Email = EmailTextBox.Text;
            string Username = UsernameTextBox.Text;
            string Pass1 = Password1.Password;
            string Pass2 = Password2.Password;
            CreateUser(Email, Username, Pass1, Pass2);
        }

        private bool CreateUser(string Email, string Username, string Pass1, string Pass2)
        {
            try
            {
                if (string.IsNullOrEmpty(Email) ||
                    string.IsNullOrEmpty(Username) ||
                    string.IsNullOrEmpty(Pass1) ||
                    string.IsNullOrEmpty(Pass2))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (!SmartHome.Utils.IsValidEmail(Email))
                {
                    MessageBox.Show("Почта не соответствует стандартам");
                    return false;
                }

                if (Pass1 != Pass2)
                {
                    MessageBox.Show("Пароли не совпдают, проверьте написание и попробуйте снова");
                    return false;
                }

                if (Core.DB.Users.Any(u => u.email == Email))
                {
                    MessageBox.Show("Пользователь с такой почтой уже существует");
                    return false;
                }

                var newUser = new Database.Users
                {
                    email = Email,
                    username = Username,
                    password = SmartHome.Utils.GetHash(Pass1),
                    created_at = DateTime.Now
                };

                Core.DB.Users.Add(newUser);
                Core.DB.SaveChanges();

                MessageBox.Show("Пользователь успешно создан");
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
