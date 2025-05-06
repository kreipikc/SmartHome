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

namespace SmartHome.Pages.Devices.Settings
{
    /// <summary>
    /// Логика взаимодействия для EditDevicesSettingsPage.xaml
    /// </summary>
    public partial class EditDevicesSettingsPage : Page
    {
        public EditDevicesSettingsPage()
        {
            InitializeComponent();
            AddData();
            DevicesSettingsPage.Device_SettingsCurrent = null;
        }

        private void AddData()
        {
            try
            {
                DeviceTextBox.Text = DevicesPage.DevicesCurrent.device_name;
                IdTextBox.Text = DevicesSettingsPage.Device_SettingsCurrent.setting_id.ToString();
                NameTextBox.Text = DevicesSettingsPage.Device_SettingsCurrent.setting_name;
                ValueTextBox.Text = DevicesSettingsPage.Device_SettingsCurrent.setting_value;
            }
            catch (Exception e)
            {
                SmartHome.Utils.PrintError(e);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Name = NameTextBox.Text;
            string Value = ValueTextBox.Text;
            UpdateSetting(IdStr, Name, Value);
        }

        private bool UpdateSetting(string IdStr, string Name, string Value)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(Name) ||
                    string.IsNullOrEmpty(Value))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.Device_Settings.Any(u => u.setting_name == Name && u.device_id == DevicesPage.DevicesCurrent.device_id && u.setting_id != Id))
                {
                    MessageBox.Show($"Настройка для девайса '{DevicesPage.DevicesCurrent.device_name}' с таким именем уже существует");
                    return false;
                }

                var setting = Core.DB.Device_Settings.FirstOrDefault(h => h.setting_id == Id);
                if (setting == null)
                {
                    MessageBox.Show("Настройка не найдена");
                    return false;
                }

                setting.setting_name = Name;
                setting.setting_value = Value;

                Core.DB.SaveChanges();
                MessageBox.Show("Настройка успешно обновлена");

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
