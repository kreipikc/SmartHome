using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SmartHome.Pages.Devices
{
    class Utils
    {
        public static void LoadRoomsToComboBox(ComboBox RolesComboBox)
        {
            try
            {
                var rooms = Core.DB.Rooms.ToList();

                RolesComboBox.ItemsSource = rooms;

                if (rooms.Any())
                {
                    RolesComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при загрузке данных");
            }
        }
    }
}
