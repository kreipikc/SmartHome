using SmartHome.Pages.Events;
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
    /// Логика взаимодействия для SensorDataPage.xaml
    /// </summary>
    public partial class SensorDataPage : Page
    {
        public static Database.Sensor_Data SensorDataCurrent;
        private List<Database.Sensor_Data> allSensorData;

        public SensorDataPage()
        {
            InitializeComponent();
            UpdateData();
            SortSensorDataCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID" },
                new Category { NameOfCategory = "По ID типа сенсора" },
                new Category { NameOfCategory = "По дате данным" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allSensorData = Core.DB.Sensor_Data.ToList();
            FilterAndSortSensorData();
        }

        private void FilterAndSortSensorData()
        {
            var filteredData = allSensorData.AsQueryable();

            switch (SortSensorDataCategory.SelectedIndex)
            {
                case 0:
                    filteredData = filteredData.OrderBy(d => d.data_id);
                    break;
                case 1:
                    filteredData = filteredData.OrderBy(d => d.sensor_type_id);
                    break;
                case 2:
                    filteredData = filteredData.OrderBy(d => d.data);
                    break;
                case 3:
                    filteredData = filteredData.OrderBy(d => d.created_at);
                    break;
            }

            DataGridSensorData.ItemsSource = filteredData.ToList();
        }

        private void AddSensorData_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddSensorDataPage());
        }

        private void EditSensorData_Click(object sender, RoutedEventArgs e)
        {
            SensorDataCurrent = DataGridSensorData.SelectedItem as Database.Sensor_Data;
            if (SensorDataCurrent != null)
            {
                NavigationService.Navigate(new EditSensorDataPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteSensorData_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Database.Sensor_Data Data = DataGridSensorData.SelectedItem as Database.Sensor_Data;
                if (Data != null)
                {
                    try
                    {
                        Core.DB.Sensor_Data.Remove(Data);
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

        private void RefreshSensorData_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SortSensorDataCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortSensorData();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SortSensorDataCategory.SelectedIndex = 0;
            FilterAndSortSensorData();
        }
    }
}
