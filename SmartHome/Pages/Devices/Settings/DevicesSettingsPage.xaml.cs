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
using System.Xml.Serialization;

namespace SmartHome.Pages.Devices.Settings
{
    /// <summary>
    /// Логика взаимодействия для DevicesSettingsPage.xaml
    /// </summary>
    public partial class DevicesSettingsPage : Page
    {
        public static Database.Device_Settings Device_SettingsCurrent;
        private List<Database.Device_Settings> allDevicesSettings;

        public DevicesSettingsPage()
        {
            InitializeComponent();
            DevicesCurrent.Text = DevicesPage.DevicesCurrent.device_name;
            UpdateData();

            SortDevicesSettingsCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По названию" },
                new Category { NameOfCategory = "По значению" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allDevicesSettings = Core.DB.Device_Settings.Where(p => p.device_id == DevicesPage.DevicesCurrent.device_id).ToList();
            FilterAndSortDevicesSettings();
        }

        private void FilterAndSortDevicesSettings()
        {
            var filteredDevicesSettings = allDevicesSettings.AsQueryable();

            // Фильтрация по названию
            if (!string.IsNullOrEmpty(SearchDevicesSettingsName.Text))
            {
                filteredDevicesSettings = filteredDevicesSettings.Where(d => d.setting_name.IndexOf(SearchDevicesSettingsName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            // Сортировка
            switch (SortDevicesSettingsCategory.SelectedIndex)
            {
                case 0: // По названию
                    filteredDevicesSettings = filteredDevicesSettings.OrderBy(d => d.setting_name);
                    break;
                case 1: // По значению
                    filteredDevicesSettings = filteredDevicesSettings.OrderBy(d => d.setting_value);
                    break;
                case 2: // По дате создания
                    filteredDevicesSettings = filteredDevicesSettings.OrderBy(d => d.created_at);
                    break;
            }

            ListViewDevicesSettings.ItemsSource = filteredDevicesSettings.ToList();
        }

        private void AddDevicesSettings_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDevicesSettingsPage());
        }

        private void EditDevicesSettings_Click(object sender, RoutedEventArgs e)
        {
            Device_SettingsCurrent = ListViewDevicesSettings.SelectedItem as Database.Device_Settings;
            if (Device_SettingsCurrent != null)
            {
                NavigationService.Navigate(new EditDevicesSettingsPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteDevicesSettings_Click(object sender, RoutedEventArgs e)
        {
            Database.Device_Settings Settings = ListViewDevicesSettings.SelectedItem as Database.Device_Settings;
            if (Settings != null)
            {
                try
                {
                    Core.DB.Device_Settings.Remove(Settings);
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

        private void RefreshDevicesSettings_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchDevicesSettingsName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortDevicesSettings();
        }

        private void SortDevicesSettingsCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortDevicesSettings();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchDevicesSettingsName.Text = string.Empty;
            SortDevicesSettingsCategory.SelectedIndex = 0;
            FilterAndSortDevicesSettings();
        }
    }
}
