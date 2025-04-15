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
    /// Логика взаимодействия для EditDevicesPage.xaml
    /// </summary>
    public partial class EditDevicesPage : Page
    {
        public EditDevicesPage()
        {
            InitializeComponent();
            Devices.Utils.LoadRoomsToComboBox(this.RoomsComboBox);
            AddData();
            DevicesPage.DevicesCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = Convert.ToString(DevicesPage.DevicesCurrent.device_id);
                NameTextBox.Text = DevicesPage.DevicesCurrent.device_name;
                StatusCheckBox.IsChecked = DevicesPage.DevicesCurrent.status;

                var currentRoomId = DevicesPage.DevicesCurrent.room_id;
                var selectedRole = RoomsComboBox.Items
                    .OfType<Rooms>()
                    .FirstOrDefault(r => r.room_id == currentRoomId);

                if (selectedRole != null)
                {
                    RoomsComboBox.SelectedItem = selectedRole;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Name = NameTextBox.Text;
            int RoomsId = (int)RoomsComboBox.SelectedValue;
            bool Status = (bool)StatusCheckBox.IsChecked;

            UpdateDevices(IdStr, Name, RoomsId, Status);
        }

        public bool UpdateDevices(string IdStr, string Name, int RoomsId, bool Status)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.Devices.Any(u => u.device_name == Name && u.device_id != Id))
                {
                    MessageBox.Show("Девайс с таким именем уже существует");
                    return false;
                }

                var user = Core.DB.Devices.FirstOrDefault(h => h.device_id == Id);
                if (user == null)
                {
                    MessageBox.Show("Девайс не найден");
                    return false;
                }

                user.device_name = Name;
                user.room_id = RoomsId;
                user.status = Status;

                Core.DB.SaveChanges();
                MessageBox.Show("Девайс успешно обновлен");

                NavigationService.Navigate(new Uri("Pages\\Devices\\DevicesPage.xaml", UriKind.Relative));
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
            NavigationService.Navigate(new Uri("Pages\\Devices\\DevicesPage.xaml", UriKind.Relative));
        }
    }
}
