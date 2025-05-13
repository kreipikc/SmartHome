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
    /// Логика взаимодействия для AutomationRulesPage.xaml
    /// </summary>
    public partial class AutomationRulesPage : Page
    {
        public static Database.Automation_Rules AutomationRulesCurrent;
        private List<Database.Automation_Rules> allAutomationRules;

        public AutomationRulesPage()
        {
            InitializeComponent();
            UpdateData();
            SortAutomationRulesCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID правила" },
                new Category { NameOfCategory = "По названию правила" },
                new Category { NameOfCategory = "По ID события" },
                new Category { NameOfCategory = "По ID девайса" },
                new Category { NameOfCategory = "По действию" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allAutomationRules = Core.DB.Automation_Rules.ToList();
            FilterAndSortAutomationRules();
        }

        private void FilterAndSortAutomationRules()
        {
            var filteredData = allAutomationRules.AsQueryable();

            if (!string.IsNullOrEmpty(SearchAutomationRulesName.Text))
            {
                filteredData = filteredData.Where(d => d.rule_name.IndexOf(SearchAutomationRulesName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortAutomationRulesCategory.SelectedIndex)
            {
                case 0: // По ID правила
                    filteredData = filteredData.OrderBy(d => d.rule_id);
                    break;
                case 1: // По названию правила
                    filteredData = filteredData.OrderBy(d => d.rule_name);
                    break;
                case 2: // По ID события
                    filteredData = filteredData.OrderBy(d => d.trigger_event_id);
                    break;
                case 3: // По ID девайса
                    filteredData = filteredData.OrderBy(d => d.action_device_id);
                    break;
                case 4: // По действию
                    filteredData = filteredData.OrderBy(d => d.action);
                    break;
                case 5: // По дате создания
                    filteredData = filteredData.OrderBy(d => d.created_at);
                    break;
            }

            DataGridAutomationRules.ItemsSource = filteredData.ToList();
        }

        private void AddAutomationRules_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAutomationRulesPage());
        }

        private void EditAutomationRules_Click(object sender, RoutedEventArgs e)
        {
            AutomationRulesCurrent = DataGridAutomationRules.SelectedItem as Database.Automation_Rules;
            if (AutomationRulesCurrent != null)
            {
                NavigationService.Navigate(new EditAutomationRulesPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteAutomationRules_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Database.Automation_Rules AutomationRule = DataGridAutomationRules.SelectedItem as Database.Automation_Rules;
                if (AutomationRule != null)
                {
                    try
                    {
                        Core.DB.Automation_Rules.Remove(AutomationRule);
                        Core.DB.SaveChanges();
                        UpdateData();
                    }
                    catch (Exception ex)
                    {
                        SmartHome.Utils.PrintError(ex);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите поле с записью");
                }
            }
        }

        private void RefreshAutomationRules_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchAutomationRulesName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortAutomationRules();
        }

        private void SortAutomationRulesCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortAutomationRules();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchAutomationRulesName.Text = string.Empty;
            SortAutomationRulesCategory.SelectedIndex = 0;
            FilterAndSortAutomationRules();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите выйти из аккаунта?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                LoginPage.UserNow = null;
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.NavigateToPage(new LoginPage());
            }
        }
    }
}
