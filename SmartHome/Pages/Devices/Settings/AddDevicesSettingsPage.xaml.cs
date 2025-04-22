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

namespace SmartHome.Pages.Devices.Settings
{
    /// <summary>
    /// Логика взаимодействия для AddDevicesSettingsPage.xaml
    /// </summary>
    public partial class AddDevicesSettingsPage : Page
    {
        public AddDevicesSettingsPage()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            string Value = ValueTextBox.Text;

            CreateDevicesSettings(Name, Value);
        }

        public bool CreateDevicesSettings(string Name, string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Value))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (Core.DB.Device_Settings.Any(u => u.setting_name == Name && u.device_id == DevicesPage.DevicesCurrent.device_id))
                {
                    MessageBox.Show("Настройка с таким названием уже существует");
                    return false;
                }

                var newSetting = new Database.Device_Settings
                {
                    device_id = DevicesPage.DevicesCurrent.device_id,
                    setting_name = Name,
                    setting_value = Value,
                    created_at = DateTime.Now
                };

                Core.DB.Device_Settings.Add(newSetting);
                Core.DB.SaveChanges();

                MessageBox.Show("Настройка для девайса успешно создана");
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
