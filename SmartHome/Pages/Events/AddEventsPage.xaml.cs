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
    /// Логика взаимодействия для AddEventsPage.xaml
    /// </summary>
    public partial class AddEventsPage : Page
    {
        public AddEventsPage()
        {
            InitializeComponent();
            Events.Utils.LoadTypeActionToComboBox(TypeActionsComboBox);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            int TypeId = (int)TypeActionsComboBox.SelectedValue;

            CreateEvents(Name, TypeId);
        }

        public bool CreateEvents(string Name, int TypeId)
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (Core.DB.Events.Any(u => u.event_name == Name))
                {
                    MessageBox.Show("Событие с таким названием уже существует");
                    return false;
                }

                var newEvent = new Database.Events
                {
                    event_name = Name,
                    event_type_id = TypeId,
                    timestamp = DateTime.Now,
                    created_at = DateTime.Now
                };

                Core.DB.Events.Add(newEvent);
                Core.DB.SaveChanges();

                MessageBox.Show("Событие успешно создано");
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
