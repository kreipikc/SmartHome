using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SmartHome.Pages.Events
{
    class Utils
    {
        public static void LoadTypeActionToComboBox(ComboBox ComboBox)
        {
            try
            {
                var types = Core.DB.TypeAction.ToList();

                ComboBox.ItemsSource = types;

                if (types.Any())
                {
                    ComboBox.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при загрузке данных: ");
            }
        }
    }
}
