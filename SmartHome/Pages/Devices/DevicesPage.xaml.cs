using SmartHome.Database;
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

namespace SmartHome.Pages.Devices
{
    /// <summary>
    /// Логика взаимодействия для DevicesPage.xaml
    /// </summary>
    public partial class DevicesPage : Page
    {
        public static Database.Devices DevicesCurrent;
        private List<Database.Devices> allDevices;

        public DevicesPage()
        {
            InitializeComponent();
            UpdateData();

            SortDevicesCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По названию" },
                new Category { NameOfCategory = "По статусу" },
                new Category { NameOfCategory = "По комнате" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allDevices = Core.DB.Devices.ToList();
            FilterAndSortDevices();
        }

        private void FilterAndSortDevices()
        {
            var filteredData = allDevices.AsQueryable();

            if (!string.IsNullOrEmpty(SearchDevicesName.Text))
            {
                filteredData = filteredData.Where(d => d.device_name.IndexOf(SearchDevicesName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortDevicesCategory.SelectedIndex)
            {
                case 0: // По названию
                    filteredData = filteredData.OrderBy(d => d.device_name);
                    break;
                case 1: // По статусу
                    filteredData = filteredData.OrderBy(d => d.status);
                    break;
                case 2: // По комнате
                    filteredData = filteredData.OrderBy(d => d.room_id);
                    break;
                case 3: // По дате создания
                    filteredData = filteredData.OrderBy(d => d.created_at);
                    break;
            }

            DataGridDevices.ItemsSource = filteredData.ToList();
        }

        private void AddDevices_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDevicesPage());
        }

        private void EditDevices_Click(object sender, RoutedEventArgs e)
        {
            DevicesCurrent = DataGridDevices.SelectedItem as Database.Devices;
            if (DevicesCurrent != null)
            {
                NavigationService.Navigate(new EditDevicesPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteDevices_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Database.Devices Device = DataGridDevices.SelectedItem as Database.Devices;
                if (Device != null)
                {
                    try
                    {
                        Core.DB.Devices.Remove(Device);
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

        private void RefreshDevices_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void OpenSetings_Click(object sender, RoutedEventArgs e)
        {
            DevicesCurrent = DataGridDevices.SelectedItem as Database.Devices;
            if (DevicesCurrent != null)
            {
                NavigationService.Navigate(new Settings.DevicesSettingsPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void SearchDevicesName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortDevices();
        }

        private void SortDevicesCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortDevices();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchDevicesName.Text = string.Empty;
            SortDevicesCategory.SelectedIndex = 0;
            FilterAndSortDevices();
        }
    }
}
