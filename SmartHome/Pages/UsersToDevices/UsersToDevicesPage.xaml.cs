using SmartHome.Pages.DevicesData;
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

namespace SmartHome.Pages.UsersToDevices
{
    /// <summary>
    /// Логика взаимодействия для UsersToDevicesPage.xaml
    /// </summary>
    public partial class UsersToDevicesPage : Page
    {
        public static List<UserToDevicesView> AllUserToDevices;

        public UsersToDevicesPage()
        {
            InitializeComponent();
            UpdateData();
            SortUsersToDevicesCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID связи" },
                new Category { NameOfCategory = "По почте пользователя" },
                new Category { NameOfCategory = "По девайсу" },
                new Category { NameOfCategory = "По дате" }
            };
        }

        private void UpdateData()
        {
            AllUserToDevices = Core.DB.UsersToDevices
                .Join(
                    Core.DB.Devices,
                    utd => utd.device_id,
                    d => d.device_id,
                    (utd, d) => new { utd, d.device_name }
                )
                .Join(
                    Core.DB.Users,
                    temp => temp.utd.user_id,
                    u => u.user_id,
                    (temp, u) => new UserToDevicesView
                    {
                        Id = temp.utd.id,
                        DeviceName = temp.device_name,
                        Email = u.email,
                        CreatedAt = (DateTime)temp.utd.created_at
                    }
                ).ToList();
            SortAndFilterUsersToDevices();
        }

        private void SortAndFilterUsersToDevices()
        {
            var filteredData = AllUserToDevices.AsQueryable();

            if (!string.IsNullOrEmpty(SearchUsers.Text))
            {
                filteredData = filteredData.Where(d => d.Email.IndexOf(SearchUsers.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            if (!string.IsNullOrEmpty(SearchDevices.Text))
            {
                filteredData = filteredData.Where(d => d.DeviceName.IndexOf(SearchDevices.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortUsersToDevicesCategory.SelectedIndex)
            {
                case 0:
                    filteredData = filteredData.OrderBy(d => d.Id);
                    break;
                case 1:
                    filteredData = filteredData.OrderBy(d => d.Email);
                    break;
                case 2:
                    filteredData = filteredData.OrderBy(d => d.DeviceName);
                    break;
                case 3:
                    filteredData = filteredData.OrderBy(d => d.CreatedAt);
                    break;
            }

            DataGridUsersToDevices.ItemsSource = filteredData.ToList();
        }
        private void AddUsersToDevices_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddUsersToDevicesPage());
        }

        private void DeleteUsersToDevices_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                UserToDevicesView ud = DataGridUsersToDevices.SelectedItem as UserToDevicesView;
                if (ud != null)
                {
                    try
                    {
                        var udContext = Core.DB.UsersToDevices.FirstOrDefault(p => p.id == ud.Id);

                        Core.DB.UsersToDevices.Remove(udContext);
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
        }

        private void RefreshUsersToDevices_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchUsersToDevices_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortAndFilterUsersToDevices();
        }

        private void SortUsersToDevicesCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortAndFilterUsersToDevices();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchUsers.Text = string.Empty;
            SearchDevices.Text = string.Empty;
            SortUsersToDevicesCategory.SelectedIndex = 0;
            SortAndFilterUsersToDevices();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите выйти из аккаунта?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                LoginPage.UserNow = null;
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.NavigateToPage(new LoginPage());
            }
        }
    }
}
