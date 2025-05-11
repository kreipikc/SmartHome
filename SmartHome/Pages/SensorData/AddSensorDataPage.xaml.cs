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

namespace SmartHome.Pages.SensorData
{
    /// <summary>
    /// Логика взаимодействия для AddSensorDataPage.xaml
    /// </summary>
    public partial class AddSensorDataPage : Page
    {
        public AddSensorDataPage()
        {
            InitializeComponent();
            SmartHome.Utils.LoadDataToComboBox(TypeActionsComboBox, Utils.ComboBoxName.TypeActions);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Data = DataTextBox.Text;
            int TypeId = (int)TypeActionsComboBox.SelectedValue;

            CreateEvents(Data, TypeId);
        }

        public bool CreateEvents(string Data, int TypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(Data))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                double DataDouble = Convert.ToDouble(Data);

                var newData = new Database.Sensor_Data
                {
                    sensor_type_id = TypeId,
                    data = DataDouble,
                    created_at = DateTime.Now
                };

                Core.DB.Sensor_Data.Add(newData);
                Core.DB.SaveChanges();

                MessageBox.Show("Сенсор данные успешно созданы");
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
