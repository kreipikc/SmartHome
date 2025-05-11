using SmartHome.Pages.Events;
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

namespace SmartHome.Pages.TypeAction
{
    /// <summary>
    /// Логика взаимодействия для TypeActionPage.xaml
    /// </summary>
    public partial class TypeActionPage : Page
    {
        public static Database.TypeAction TypeCurrent;
        private List<Database.TypeAction> allType;

        public TypeActionPage()
        {
            InitializeComponent();
            UpdateData();
            SortTypeActionCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По ID" },
                new Category { NameOfCategory = "По названию" },
            };
        }

        private void UpdateData()
        {
            allType = Core.DB.TypeAction.ToList();
            FilterAndSortTypeAction();
        }

        private void FilterAndSortTypeAction()
        {
            var filteredData = allType.AsQueryable();

            if (!string.IsNullOrEmpty(SearchTypeActionName.Text))
            {
                filteredData = filteredData.Where(d => d.type_name.IndexOf(SearchTypeActionName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortTypeActionCategory.SelectedIndex)
            {
                case 0: // По Id
                    filteredData = filteredData.OrderBy(d => d.type_action_id);
                    break;
                case 1: // По названию
                    filteredData = filteredData.OrderBy(d => d.type_name);
                    break;
            }

            DataGridTypeAction.ItemsSource = filteredData.ToList();
        }

        private void AddTypeAction_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddTypeActionPage());
        }

        private void EditTypeAction_Click(object sender, RoutedEventArgs e)
        {
            TypeCurrent = DataGridTypeAction.SelectedItem as Database.TypeAction;
            if (TypeCurrent != null)
            {
                NavigationService.Navigate(new EditTypeActionPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteTypeAction_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Database.TypeAction Type = DataGridTypeAction.SelectedItem as Database.TypeAction;
                if (Type != null)
                {
                    try
                    {
                        Core.DB.TypeAction.Remove(Type);
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

        private void RefreshTypeAction_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchTypeActionName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortTypeAction();
        }

        private void SortTypeActionCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortTypeAction();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchTypeActionName.Text = string.Empty;
            SortTypeActionCategory.SelectedIndex = 0;
            FilterAndSortTypeAction();
        }
    }
}
