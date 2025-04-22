using SmartHome.Database;
using SmartHome.Pages.Devices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
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

namespace SmartHome.Pages.Rooms
{
    /// <summary>
    /// Логика взаимодействия для RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : Page
    {
        public static Database.Rooms RoomCurrent;

        public RoomsPage()
        {
            InitializeComponent();
            UpdateData();
        }

        private void UpdateData()
        {
            DataGridRooms.ItemsSource = Core.DB.Rooms.ToList();
        }

        private void AddRooms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRoomsPage());
        }

        private void EditRooms_Click(object sender, RoutedEventArgs e)
        {
            RoomCurrent = DataGridRooms.SelectedItem as Database.Rooms;
            if (RoomCurrent != null)
            {
                NavigationService.Navigate(new EditRoomsPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteRooms_Click(object sender, RoutedEventArgs e)
        {
            Database.Rooms Room = DataGridRooms.SelectedItem as Database.Rooms;
            if (Room != null)
            {
                try
                {
                    Core.DB.Rooms.Remove(Room);
                    Core.DB.SaveChanges();
                    UpdateData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"{ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void RefreshRooms_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }
    }
}
