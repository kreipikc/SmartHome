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
    /// Логика взаимодействия для EditSensorDataPage.xaml
    /// </summary>
    public partial class EditSensorDataPage : Page
    {
        public EditSensorDataPage()
        {
            InitializeComponent();
            SmartHome.Utils.LoadDataToComboBox(TypeActionsComboBox, Utils.ComboBoxName.TypeActions);
            AddData();
            SensorDataPage.SensorDataCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = Convert.ToString(SensorDataPage.SensorDataCurrent.data_id);
                DataTextBox.Text = SensorDataPage.SensorDataCurrent.data.ToString();

                var currentTypeId = SensorDataPage.SensorDataCurrent.sensor_type_id;
                var selectedType = TypeActionsComboBox.Items
                    .OfType<Database.TypeAction>()
                    .FirstOrDefault(r => r.type_action_id == currentTypeId);

                if (selectedType != null)
                {
                    TypeActionsComboBox.SelectedItem = selectedType;
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Data = DataTextBox.Text;
            int TypeId = (int)TypeActionsComboBox.SelectedValue;

            UpdateEvent(IdStr, Data, TypeId);
        }

        public bool UpdateEvent(string IdStr, string Data, int TypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(Data))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);
                double DataDouble = Convert.ToDouble(Data);

                var sens_data = Core.DB.Sensor_Data.FirstOrDefault(h => h.data_id == Id);
                if (sens_data == null)
                {
                    MessageBox.Show("Сеснор данные не найдены");
                    return false;
                }

                sens_data.data = DataDouble;
                sens_data.sensor_type_id = TypeId;

                Core.DB.SaveChanges();
                MessageBox.Show("Сеснор данные успешно обновлены");

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
