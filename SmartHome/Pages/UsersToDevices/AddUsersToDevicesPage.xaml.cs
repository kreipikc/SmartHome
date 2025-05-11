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
    /// Логика взаимодействия для AddUsersToDevicesPage.xaml
    /// </summary>
    public partial class AddUsersToDevicesPage : Page
    {
        public AddUsersToDevicesPage()
        {
            InitializeComponent();
            SmartHome.Utils.LoadDataToComboBox(UserComboBox, Utils.ComboBoxName.Users);
            SmartHome.Utils.LoadDataToComboBox(DeviceComboBox, Utils.ComboBoxName.Devices);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            int DeviceID = (int)DeviceComboBox.SelectedValue;
            int UserID = (int)UserComboBox.SelectedValue;
            CreateClassUsersToDevices(DeviceID, UserID);
        }

        private bool CreateClassUsersToDevices(int DeviceID, int UserID)
        {
            try
            {
                if (Core.DB.UsersToDevices.Any(u => u.device_id == DeviceID && u.user_id == UserID))
                {
                    MessageBox.Show("Такая связь UsersToDevices уже существует");
                    return false;
                }

                var newUsersToDevices = new Database.UsersToDevices
                {
                    device_id = DeviceID,
                    user_id = UserID,
                    created_at = DateTime.Now
                };

                Core.DB.UsersToDevices.Add(newUsersToDevices);
                Core.DB.SaveChanges();

                MessageBox.Show("Связь UsersToDevices успешно создана");
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
