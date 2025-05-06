using SmartHome.Pages.AutomationRules;
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
    /// Логика взаимодействия для EditEventsPage.xaml
    /// </summary>
    public partial class EditEventsPage : Page
    {
        public EditEventsPage()
        {
            InitializeComponent();
            Events.Utils.LoadTypeActionToComboBox(TypeActionsComboBox);
            AddData();
            EventsPage.EventsCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = Convert.ToString(EventsPage.EventsCurrent.event_id);
                NameTextBox.Text = EventsPage.EventsCurrent.event_name;

                var currentTypeId = EventsPage.EventsCurrent.event_type_id;
                var selectedType = TypeActionsComboBox.Items
                    .OfType<Database.TypeAction>()
                    .FirstOrDefault(r => r.type_action_id == currentTypeId);

                if (selectedType != null)
                {
                    TypeActionsComboBox.SelectedItem = selectedType;
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            string IdStr = IdTextBox.Text;
            string Name = NameTextBox.Text;
            int TypeId = (int)TypeActionsComboBox.SelectedValue;

            UpdateEvent(IdStr, Name, TypeId);
        }

        public bool UpdateEvent(string IdStr, string Name, int TypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.Events.Any(u => u.event_name == Name && u.event_id != Id))
                {
                    MessageBox.Show("Событие с таким именем уже существует");
                    return false;
                }

                var events = Core.DB.Events.FirstOrDefault(h => h.event_id == Id);
                if (events == null)
                {
                    MessageBox.Show("Событие не найден");
                    return false;
                }

                events.event_name = Name;
                events.event_type_id = TypeId;

                Core.DB.SaveChanges();
                MessageBox.Show("Событие успешно обновлено");

                NavigationService.GoBack();
                return true;
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex);
                return false;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
