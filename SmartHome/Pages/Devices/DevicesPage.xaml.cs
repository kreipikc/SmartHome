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

        public DevicesPage()
        {
            InitializeComponent();
            UpdateData();
        }

        private void UpdateData()
        {
            DataGridDevices.ItemsSource = Core.DB.Devices.ToList();
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
                    MessageBox.Show($"{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
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
    }
}
