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
    /// Логика взаимодействия для EditRoomsPage.xaml
    /// </summary>
    public partial class EditRoomsPage : Page
    {
        public EditRoomsPage()
        {
            InitializeComponent();
            AddData();
            RoomsPage.RoomCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = RoomsPage.RoomCurrent.room_id.ToString();
                RoomTextBox.Text = RoomsPage.RoomCurrent.room_name;
                FloorTextBox.Text = RoomsPage.RoomCurrent.floor.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string RoomName = RoomTextBox.Text;
            string Floor = FloorTextBox.Text;
            UpdateRoom(IdStr, RoomName, Floor);
        }

        private bool UpdateRoom(string IdStr, string RoomName, string Floor)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(RoomName) ||
                    string.IsNullOrEmpty(Floor))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int FloorInt = Convert.ToInt32(Floor);
                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.Rooms.Any(u => u.room_name == Name && u.room_id != Id))
                {
                    MessageBox.Show("Комната с таким именем уже существует");
                    return false;
                }

                var room = Core.DB.Rooms.FirstOrDefault(h => h.room_id == Id);
                if (room == null)
                {
                    MessageBox.Show("Комната не найдена");
                    return false;
                }

                room.room_name = RoomName;
                room.floor = FloorInt;

                Core.DB.SaveChanges();
                MessageBox.Show("Комната успешно обновлена");

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
