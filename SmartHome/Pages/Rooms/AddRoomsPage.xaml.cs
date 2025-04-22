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

namespace SmartHome.Pages.Rooms
{
    /// <summary>
    /// Логика взаимодействия для AddRoomsPage.xaml
    /// </summary>
    public partial class AddRoomsPage : Page
    {
        public AddRoomsPage()
        {
            InitializeComponent();
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = RoomTextBox.Text;
            string Floor = FloorTextBox.Text;
            CreateRoom(Name, Floor);
        }

        private bool CreateRoom(string Name, string Floor)
        {
            try
            {
                if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Floor))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int FloorInt = Convert.ToInt32(Floor);

                if (Core.DB.Rooms.Any(u => u.room_name == Name))
                {
                    MessageBox.Show("Комната с таким названием уже существует");
                    return false;
                }

                var newRoom = new Database.Rooms
                {
                    room_name = Name,
                    floor = FloorInt,
                    created_at = DateTime.Now
                };

                Core.DB.Rooms.Add(newRoom);
                Core.DB.SaveChanges();

                MessageBox.Show("Комната успешно создана");
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
