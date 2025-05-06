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

namespace SmartHome.Pages.AutomationRules
{
    /// <summary>
    /// Логика взаимодействия для AddAutomationRulesPage.xaml
    /// </summary>
    public partial class AddAutomationRulesPage : Page
    {
        public AddAutomationRulesPage()
        {
            InitializeComponent();
            AutomationRules.Utils.LoadEventsToComboBox(EventComboBox);
            AutomationRules.Utils.LoadDevicesToComboBox(DeviceComboBox);
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            string Name = NameTextBox.Text;
            int EventId = (int)EventComboBox.SelectedValue;
            int DeviceId = (int)DeviceComboBox.SelectedValue;
            string Action = ActionTextBox.Text;

            CreateRules(Name, EventId, DeviceId, Action);
        }

        public bool CreateRules(string Name, int EventId, int DeviceId, string Action)
        {
            try
            {
                if (string.IsNullOrEmpty(Name) ||
                    string.IsNullOrEmpty(Action))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                if (EventId <= 0 || DeviceId <= 0)
                {
                    MessageBox.Show("Некорректный ID события или устройства");
                    return false;
                }

                if (Core.DB.Automation_Rules.Any(u => u.rule_name == Name))
                {
                    MessageBox.Show("Правило с таким названием уже существует");
                    return false;
                }

                var newRule = new Database.Automation_Rules
                {
                    rule_name = Name,
                    trigger_event_id = EventId,
                    action_device_id = DeviceId,
                    action = Action,
                    created_at = DateTime.Now
                };

                Core.DB.Automation_Rules.Add(newRule);
                Core.DB.SaveChanges();

                MessageBox.Show("Правило успешно создано");
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
