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
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms.DataVisualization.Charting;

namespace SmartHome.Pages.DevicesData
{
    /// <summary>
    /// Логика взаимодействия для DevicesDataStatPage.xaml
    /// </summary>
    public partial class DevicesDataStatPage : Page
    {
        public DevicesDataStatPage()
        {
            InitializeComponent();
            InitChart();
            LoadDevices();
        }

        private void InitChart()
        {
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Данные сенсоров")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);
            CmbDiagram.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void LoadDevices()
        {
            var devices = Core.DB.Devices
                .Select(d => new { d.device_id, d.device_name })
                .ToList();

            DeviceComboBox.DisplayMemberPath = "device_name";
            DeviceComboBox.SelectedValuePath = "device_id";
            DeviceComboBox.ItemsSource = devices;

            if (devices.Any())
                DeviceComboBox.SelectedIndex = 0;
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (DeviceComboBox.SelectedItem == null || CmbDiagram.SelectedItem == null)
                return;

            var selectedDeviceId = (int)DeviceComboBox.SelectedValue;
            var currentType = (SeriesChartType)CmbDiagram.SelectedItem;

            Series currentSeries = ChartPayments.Series.FirstOrDefault();
            if (currentSeries == null)
            {
                currentSeries = new Series("Данные сенсоров");
                ChartPayments.Series.Add(currentSeries);
            }

            currentSeries.ChartType = currentType;
            currentSeries.Points.Clear();

            var sensorData = Core.DB.Device_Data
                .Where(dd => dd.device_id == selectedDeviceId)
                .Join(
                    Core.DB.Sensor_Data,
                    dd => dd.data_id,
                    sd => sd.data_id,
                    (dd, sd) => new { sd.data, sd.sensor_type_id }
                )
                .Join(
                    Core.DB.TypeAction,
                    temp => temp.sensor_type_id,
                    ta => ta.type_action_id,
                    (temp, ta) => new { temp.data, ta.type_name }
                )
                .ToList();

            foreach (var data in sensorData)
            {
                currentSeries.Points.AddXY(data.type_name, data.data);
            }

            currentSeries.IsValueShownAsLabel = true;
            currentSeries.LabelFormat = "{#.##}";
            ChartPayments.Legends[0].Enabled = true;
        }

        private void btnExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var excelApp = new Excel.Application();
                Excel.Workbook workbook = excelApp.Workbooks.Add();

                // Получаем все устройства
                var allDevices = Core.DB.Devices.ToList();

                foreach (var device in allDevices)
                {
                    // Создаем лист для каждого устройства
                    Excel.Worksheet worksheet = workbook.Worksheets.Add();
                    worksheet.Name = device.device_name.Length > 31 ?
                        device.device_name.Substring(0, 31) : device.device_name;

                    // Заголовок отчета
                    worksheet.Cells[1, 1] = $"Отчет по устройству: {device.device_name}";
                    Excel.Range titleRange = worksheet.Range["A1:C1"];
                    titleRange.Merge();
                    titleRange.Font.Bold = true;
                    titleRange.Font.Size = 14;
                    titleRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Информация об устройстве
                    worksheet.Cells[2, 1] = $"Статус: {(device.status ? "Включено" : "Выключено")}";
                    worksheet.Cells[3, 1] = $"Комната: {device.Rooms?.room_name ?? "Не указана"}";

                    // Получаем данные сенсоров для устройства
                    var sensorData = Core.DB.Device_Data
                        .Where(dd => dd.device_id == device.device_id)
                        .Join(
                            Core.DB.Sensor_Data,
                            dd => dd.data_id,
                            sd => sd.data_id,
                            (dd, sd) => new { sd.data, sd.sensor_type_id, dd.created_at }
                        )
                        .Join(
                            Core.DB.TypeAction,
                            temp => temp.sensor_type_id,
                            ta => ta.type_action_id,
                            (temp, ta) => new
                            {
                                Type = ta.type_name,
                                Value = temp.data,
                                Date = temp.created_at
                            }
                        )
                        .ToList();

                    // Заголовки таблицы
                    worksheet.Cells[5, 1] = "Тип датчика";
                    worksheet.Cells[5, 2] = "Значение";
                    worksheet.Cells[5, 3] = "Дата измерения";

                    // Форматируем заголовки
                    Excel.Range headerRange = worksheet.Range["A5:C5"];
                    headerRange.Font.Bold = true;
                    headerRange.Interior.Color = Excel.XlRgbColor.rgbLightGray;
                    headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                    // Заполняем данные
                    int row = 6;
                    foreach (var data in sensorData)
                    {
                        worksheet.Cells[row, 1] = data.Type;
                        worksheet.Cells[row, 2] = data.Value;
                        worksheet.Cells[row, 3] = data.Date?.ToString("g");
                        row++;
                    }

                    // Добавляем сводную статистику
                    worksheet.Cells[row + 1, 1] = "Всего показаний:";
                    worksheet.Cells[row + 1, 2] = sensorData.Count;

                    worksheet.Columns["A:C"].AutoFit();
                }

                // Удаляем пустой лист, который создается по умолчанию
                ((Excel.Worksheet)workbook.Worksheets[1]).Delete();

                excelApp.Visible = true;
                ReleaseExcelObjects(workbook, excelApp);
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при создании Excel отчета");
            }
        }

        private void btnWord_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var application = new Word.Application();
                Word.Document document = application.Documents.Add();

                // Общий заголовок отчета
                Word.Paragraph mainTitleParagraph = document.Paragraphs.Add();
                Word.Range mainTitleRange = mainTitleParagraph.Range;
                mainTitleRange.Text = "Полный отчет по всем устройствам";
                mainTitleParagraph.set_Style("Заголовок 1");
                mainTitleRange.InsertParagraphAfter();

                // Получаем все устройства
                var allDevices = Core.DB.Devices.ToList();

                foreach (var device in allDevices)
                {
                    // Заголовок устройства
                    Word.Paragraph deviceTitleParagraph = document.Paragraphs.Add();
                    Word.Range deviceTitleRange = deviceTitleParagraph.Range;
                    deviceTitleRange.Text = $"Устройство: {device.device_name}";
                    deviceTitleParagraph.set_Style("Заголовок 2");
                    deviceTitleRange.InsertParagraphAfter();

                    // Информация об устройстве
                    Word.Paragraph deviceInfoParagraph = document.Paragraphs.Add();
                    Word.Range deviceInfoRange = deviceInfoParagraph.Range;
                    deviceInfoRange.Text = $"Статус: {(device.status ? "Включено" : "Выключено")}\n" +
                                           $"Комната: {device.Rooms?.room_name ?? "Не указана"}";
                    deviceInfoRange.InsertParagraphAfter();

                    // Получаем данные сенсоров для устройства
                    var sensorData = Core.DB.Device_Data
                        .Where(dd => dd.device_id == device.device_id)
                        .Join(
                            Core.DB.Sensor_Data,
                            dd => dd.data_id,
                            sd => sd.data_id,
                            (dd, sd) => new { sd.data, sd.sensor_type_id, dd.created_at }
                        )
                        .Join(
                            Core.DB.TypeAction,
                            temp => temp.sensor_type_id,
                            ta => ta.type_action_id,
                            (temp, ta) => new
                            {
                                Type = ta.type_name,
                                Value = temp.data,
                                Date = temp.created_at
                            }
                        )
                        .ToList();

                    // Добавляем данные сенсоров
                    foreach (var group in sensorData.GroupBy(s => s.Type))
                    {
                        Word.Paragraph sensorHeaderParagraph = document.Paragraphs.Add();
                        Word.Range sensorHeaderRange = sensorHeaderParagraph.Range;
                        sensorHeaderRange.Text = $"{group.Key} - {group.Count()} показаний";
                        sensorHeaderParagraph.set_Style("Заголовок 3");
                        sensorHeaderRange.InsertParagraphAfter();

                        Word.Paragraph sensorDataParagraph = document.Paragraphs.Add();
                        Word.Range sensorDataRange = sensorDataParagraph.Range;

                        foreach (var data in group.OrderByDescending(d => d.Date))
                        {
                            sensorDataRange.Text += $"{data.Value} ({data.Date?.ToString("g")})\n";
                        }
                        sensorDataRange.InsertParagraphAfter();
                    }

                    // Добавляем разделитель между устройствами
                    Word.Paragraph separatorParagraph = document.Paragraphs.Add();
                    Word.Range separatorRange = separatorParagraph.Range;
                    separatorRange.Text = new string('-', 50);
                    separatorRange.InsertParagraphAfter();
                }

                application.Visible = true;
                ReleaseWordObjects(document, application);
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при создании отчета");
            }
        }

        private void ReleaseExcelObjects(Excel.Workbook workbook, Excel.Application excelApp)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(workbook);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(excelApp);
            }
            catch { }
        }

        private void ReleaseWordObjects(Word.Document document, Word.Application application)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(document);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(application);
            }
            catch { }
        }
    }
}
