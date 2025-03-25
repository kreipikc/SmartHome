using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

namespace SmartHome.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = emailTextBox.Text;
            string password = passwordBox.Password;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                loginMessage.Text = "Пожалуйста, заполните все поля";
                return;
            }

            string hashPass = Utils.GetHash(password);
            var user = Core.DB.Users.FirstOrDefault(u => u.email == email && u.password == hashPass);
            if (user != null)
            {
                loginMessage.Text = "Авторизация успешна";
                loginMessage.Foreground = new SolidColorBrush(Colors.Green);
                MainWindow.userNow = user;
                NavigationService.Navigate(new Uri("Pages\\HomePage.xaml", UriKind.Relative));
            }
            else
            {
                loginMessage.Text = "Неверный логин или пароль";
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string login = regLoginTextBox.Text;
            string email = regEmailTextBox.Text;
            string password = regPasswordBox.Password;
            string confirmPassword = confirmPasswordBox.Password;

            if (string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(password))
            {
                registerMessage.Text = "Пожалуйста, заполните все поля";
                return;
            }

            if (password.Length < 6)
            {
                registerMessage.Text = "Пароль слишком простой (меньше 6 символов)";
                return;
            }

            if (password != confirmPassword)
            {
                registerMessage.Text = "Пароли не совпадают";
                return;
            }

            if (Core.DB.Users.Any(u => u.email == email))
            {
                registerMessage.Text = "Пользователь с таким почтой уже существует";
                return;
            }

            var newUser = new Users
            {
                username = login,
                password = Utils.GetHash(password),
                email = email,
                created_at = DateTime.Now
            };

            Core.DB.Users.Add(newUser);
            Core.DB.SaveChanges();

            registerMessage.Text = "Регистрация успешна";
            registerMessage.Foreground = new SolidColorBrush(Colors.Green);
            AuthAndRegisrt.SelectedIndex = 0;
        }
    }
}
