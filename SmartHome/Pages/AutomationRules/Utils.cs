using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace SmartHome.Pages.AutomationRules
{
    class Utils
    {
        public static void LoadEventsToComboBox(ComboBox ComboBox)
        {
            try
            {
                var events = Core.DB.Events.ToList();

                ComboBox.ItemsSource = events;

                if (events.Any())
                {
                    ComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при загрузке данных: ");
            }
        }

        public static void LoadDevicesToComboBox(ComboBox ComboBox)
        {
            try
            {
                var devices = Core.DB.Devices.ToList();

                ComboBox.ItemsSource = devices;

                if (devices.Any())
                {
                    ComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при загрузке данных");
            }
        }
    }
}
