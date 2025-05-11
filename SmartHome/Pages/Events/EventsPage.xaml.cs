using SmartHome.Pages.Devices;
using SmartHome.Properties;
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

namespace SmartHome.Pages.Events
{
    /// <summary>
    /// Логика взаимодействия для EventsPage.xaml
    /// </summary>
    public partial class EventsPage : Page
    {
        public static Database.Events EventsCurrent;
        private List<Database.Events> allEvents;

        public EventsPage()
        {
            InitializeComponent();
            UpdateData();
            SortEventsCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID" },
                new Category { NameOfCategory = "По названию" },
                new Category { NameOfCategory = "По ID типа действия события" },
                new Category { NameOfCategory = "По дате фиксации" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allEvents = Core.DB.Events.ToList();
            FilterAndSortEvents();
        }

        private void FilterAndSortEvents()
        {
            var filteredData = allEvents.AsQueryable();

            if (!string.IsNullOrEmpty(SearchEventsName.Text))
            {
                filteredData = filteredData.Where(d => d.event_name.IndexOf(SearchEventsName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortEventsCategory.SelectedIndex)
            {
                case 0: // По Id
                    filteredData = filteredData.OrderBy(d => d.event_id);
                    break;
                case 1: // По названию
                    filteredData = filteredData.OrderBy(d => d.event_name);
                    break;
                case 2: // ID типа действия события
                    filteredData = filteredData.OrderBy(d => d.event_type_id);
                    break;
                case 3: // По дате фиксации
                    filteredData = filteredData.OrderBy(d => d.timestamp);
                    break;
                case 4: // По дате создания
                    filteredData = filteredData.OrderBy(d => d.created_at);
                    break;
            }

            DataGridEvents.ItemsSource = filteredData.ToList();
        }

        private void AddEvents_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddEventsPage());
        }

        private void EditEvents_Click(object sender, RoutedEventArgs e)
        {
            EventsCurrent = DataGridEvents.SelectedItem as Database.Events;
            if (EventsCurrent != null)
            {
                NavigationService.Navigate(new EditEventsPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteEvents_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Database.Events Event = DataGridEvents.SelectedItem as Database.Events;
                if (Event != null)
                {
                    try
                    {
                        Core.DB.Events.Remove(Event);
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

        private void RefreshEvents_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchEventsName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortEvents();
        }

        private void SortEventsCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortEvents();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchEventsName.Text = string.Empty;
            SortEventsCategory.SelectedIndex = 0;
            FilterAndSortEvents();
        }
    }
}
