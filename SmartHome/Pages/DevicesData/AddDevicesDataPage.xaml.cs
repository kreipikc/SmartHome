using System;
using System.Collections.Generic;
using System.Data;
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

namespace SmartHome.Pages.DevicesData
{
    /// <summary>
    /// Логика взаимодействия для AddDevicesDataPage.xaml
    /// </summary>
    public partial class AddDevicesDataPage : Page
    {
        public AddDevicesDataPage()
        {
            InitializeComponent();
            SmartHome.Utils.LoadDataToComboBox(DeviceComboBox, Utils.ComboBoxName.Devices);
            SmartHome.Utils.LoadDataToComboBox(SensorDataComboBox, Utils.ComboBoxName.SensorData);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            int DeviceID = (int)DeviceComboBox.SelectedValue;
            int SensorDataID = (int)SensorDataComboBox.SelectedValue;
            CreateClassDeviceData(DeviceID, SensorDataID);
        }

        private bool CreateClassDeviceData(int DeviceID, int SensorDataID)
        {
            try
            {
                if (Core.DB.Device_Data.Any(u => u.device_id == DeviceID && u.data_id == SensorDataID))
                {
                    MessageBox.Show("Такая связь Device_Data уже существует");
                    return false;
                }

                var newDeviceData = new Database.Device_Data
                {
                    device_id = DeviceID,
                    data_id = SensorDataID,
                    created_at = DateTime.Now
                };

                Core.DB.Device_Data.Add(newDeviceData);
                Core.DB.SaveChanges();

                MessageBox.Show("Связь Device_Data успешно создана");
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
