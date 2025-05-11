using SmartHome.Pages.Devices;
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
    /// Логика взаимодействия для EditAutomationRulesPage.xaml
    /// </summary>
    public partial class EditAutomationRulesPage : Page
    {
        public EditAutomationRulesPage()
        {
            InitializeComponent();
            SmartHome.Utils.LoadDataToComboBox(EventComboBox, SmartHome.Utils.ComboBoxName.Events);
            SmartHome.Utils.LoadDataToComboBox(DeviceComboBox, SmartHome.Utils.ComboBoxName.Devices);
            AddData();
            AutomationRulesPage.AutomationRulesCurrent = null;
        }

        private void AddData()
        {
            try
            {
                IdTextBox.Text = Convert.ToString(AutomationRulesPage.AutomationRulesCurrent.rule_id);
                NameTextBox.Text = AutomationRulesPage.AutomationRulesCurrent.rule_name;
                ActionTextBox.Text = AutomationRulesPage.AutomationRulesCurrent.action;

                var currentEventId = AutomationRulesPage.AutomationRulesCurrent.trigger_event_id;
                var selectedEvent = EventComboBox.Items
                    .OfType<Database.Events>()
                    .FirstOrDefault(r => r.event_id == currentEventId);

                if (selectedEvent != null)
                {
                    EventComboBox.SelectedItem = selectedEvent;
                }

                var currentDevicesId = AutomationRulesPage.AutomationRulesCurrent.action_device_id;
                var selectedDevices = DeviceComboBox.Items
                    .OfType<Database.Devices>()
                    .FirstOrDefault(r => r.device_id == currentDevicesId);

                if (selectedDevices != null)
                {
                    DeviceComboBox.SelectedItem = selectedDevices;
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
            int EventId = (int)EventComboBox.SelectedValue;
            int DeviceID = (int)DeviceComboBox.SelectedValue;
            string Action = ActionTextBox.Text;

            UpdateRule(IdStr, Name, EventId, DeviceID, Action);
        }

        public bool UpdateRule(string IdStr, string Name, int EventId, int DeviceID, string Action)
        {
            try
            {
                if (string.IsNullOrEmpty(IdStr) ||
                    string.IsNullOrEmpty(Action) ||
                    string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return false;
                }

                int Id = Convert.ToInt32(IdStr);

                if (Core.DB.Automation_Rules.Any(u => u.rule_name == Name && u.rule_id != Id))
                {
                    MessageBox.Show("Правило с таким именем уже существует");
                    return false;
                }

                var rule = Core.DB.Automation_Rules.FirstOrDefault(h => h.rule_id == Id);
                if (rule == null)
                {
                    MessageBox.Show("Правило не найден");
                    return false;
                }

                rule.rule_name = Name;
                rule.trigger_event_id = EventId;
                rule.action_device_id = DeviceID;
                rule.action = Action;

                Core.DB.SaveChanges();
                MessageBox.Show("Правило успешно обновлено");

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
