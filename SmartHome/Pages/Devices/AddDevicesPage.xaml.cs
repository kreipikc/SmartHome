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
    /// Логика взаимодействия для AddDevicesPage.xaml
    /// </summary>
    public partial class AddDevicesPage : Page
    {
        public AddDevicesPage()
        {
            InitializeComponent();
            Devices.Utils.LoadRoomsToComboBox(this.RoomsComboBox);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            int RoomsId = (int)RoomsComboBox.SelectedValue;
            bool Status = (bool)StatusCheckBox.IsChecked;

            CreateDevices(Name, Status, RoomsId);
        }

        public bool CreateDevices(string Name, bool Status, int RoomsId)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (Core.DB.Devices.Any(u => u.device_name == Name))
                {
                    MessageBox.Show("Девайс с таким названием уже существует");
                    return false;
                }

                var newUser = new Database.Devices
                {
                    device_name = Name,
                    status = Status,
                    room_id = RoomsId,
                    created_at = DateTime.Now
                };

                Core.DB.Devices.Add(newUser);
                Core.DB.SaveChanges();

                MessageBox.Show("Девайс успешно создан");
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
