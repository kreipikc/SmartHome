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

namespace SmartHome.Pages.DevicesData
{
    /// <summary>
    /// Логика взаимодействия для DevicesDataPage.xaml
    /// </summary>
    public partial class DevicesDataPage : Page
    {
        public static List<DeviceDataView> AllDeviceData;

        public DevicesDataPage()
        {
            InitializeComponent();
            UpdateData();
            SortDevicesDataCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID связи" },
                new Category { NameOfCategory = "По девайсу" },
                new Category { NameOfCategory = "По сенсору" },
                new Category { NameOfCategory = "По значению" },
                new Category { NameOfCategory = "По дате" }
            };
        }

        private void UpdateData()
        {
            AllDeviceData = Core.DB.Device_Data
                .Join(
                    Core.DB.Devices,
                    dd => dd.device_id,
                    d => d.device_id,
                    (dd, d) => new { dd, d.device_name }
                )
                .Join(
                    Core.DB.Sensor_Data,
                    temp => temp.dd.data_id,
                    sd => sd.data_id,
                    (temp, sd) => new { temp.dd, temp.device_name, sd.data, sd.sensor_type_id }
                )
                .Join(
                    Core.DB.TypeAction,
                    temp => temp.sensor_type_id,
                    ta => ta.type_action_id,
                    (temp, ta) => new DeviceDataView
                    {
                        Id = temp.dd.id,
                        DeviceName = temp.device_name,
                        TypeName = ta.type_name,
                        Data = temp.data,
                        CreatedAt = (DateTime)temp.dd.created_at
                    }
                ).ToList();
            SortDeviceData();
        }

        private void SortDeviceData()
        {
            var filteredData = AllDeviceData.AsQueryable();

            if (!string.IsNullOrEmpty(SearchDevicesDataName.Text))
            {
                filteredData = filteredData.Where(d => d.DeviceName.IndexOf(SearchDevicesDataName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortDevicesDataCategory.SelectedIndex)
            {
                case 0:
                    filteredData = filteredData.OrderBy(d => d.Id);
                    break;
                case 1:
                    filteredData = filteredData.OrderBy(d => d.DeviceName);
                    break;
                case 2:
                    filteredData = filteredData.OrderBy(d => d.TypeName);
                    break;
                case 3:
                    filteredData = filteredData.OrderBy(d => d.Data);
                    break;
                case 4:
                    filteredData = filteredData.OrderBy(d => d.CreatedAt);
                    break;
            }

            DataGridDevicesData.ItemsSource = filteredData.ToList();
        }

        private void AddDevicesData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddDevicesDataPage());
        }

        private void DeleteDevicesData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                DeviceDataView dd = DataGridDevicesData.SelectedItem as DeviceDataView;
                if (dd != null)
                {
                    try
                    {
                        var ddContext = Core.DB.Device_Data.FirstOrDefault(p => p.id == dd.Id);

                        Core.DB.Device_Data.Remove(ddContext);
                        Core.DB.SaveChanges();
                        UpdateData();
                    }
                    catch (Exception ex)
                    {
                        SmartHome.Utils.PrintError(ex);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите поле с записью");
                }
            }
        }

        private void RefreshDevicesData_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchDevicesDataName_TextChanged(object sender, TextChangedEventArgs e)
        {
            SortDeviceData();
        }

        private void SortDevicesDataCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SortDeviceData();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchDevicesDataName.Text = string.Empty;
            SortDevicesDataCategory.SelectedIndex = 0;
            SortDeviceData();
        }

        private void Diagram_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new DevicesDataStatPage());
        }
    }
}
