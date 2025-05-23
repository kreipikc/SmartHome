﻿using SmartHome.Database;
using SmartHome.Pages.Devices;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
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
    /// Логика взаимодействия для RoomsPage.xaml
    /// </summary>
    public partial class RoomsPage : Page
    {
        public static Database.Rooms RoomCurrent;
        private List<Database.Rooms> allRooms;

        public RoomsPage()
        {
            InitializeComponent();
            UpdateData();
            SortRoomsCategory.ItemsSource = new List<Category>
            {
                new Category { NameOfCategory = "По названию" },
                new Category { NameOfCategory = "По этажу" },
                new Category { NameOfCategory = "По дате создания" }
            };
        }

        private void UpdateData()
        {
            allRooms = Core.DB.Rooms.ToList();
            FilterAndSortRooms();
        }

        private void FilterAndSortRooms()
        {
            var filteredData = allRooms.AsQueryable();

            if (!string.IsNullOrEmpty(SearchRoomsName.Text))
            {
                filteredData = filteredData.Where(d => d.room_name.IndexOf(SearchRoomsName.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }

            switch (SortRoomsCategory.SelectedIndex)
            {
                case 0: // По названию
                    filteredData = filteredData.OrderBy(d => d.room_name);
                    break;
                case 1: // По этажу
                    filteredData = filteredData.OrderBy(d => d.floor);
                    break;
                case 2: // По дате создания
                    filteredData = filteredData.OrderBy(d => d.created_at);
                    break;
            }

            DataGridRooms.ItemsSource = filteredData.ToList();
        }

        private void AddRooms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddRoomsPage());
        }

        private void EditRooms_Click(object sender, RoutedEventArgs e)
        {
            RoomCurrent = DataGridRooms.SelectedItem as Database.Rooms;
            if (RoomCurrent != null)
            {
                NavigationService.Navigate(new EditRoomsPage());
            }
            else
            {
                MessageBox.Show("Выберите поле с записью");
            }
        }

        private void DeleteRooms_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(
                "Вы уверены что хотите удалить запись?",
                "Подтверждение",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                Database.Rooms Room = DataGridRooms.SelectedItem as Database.Rooms;
                if (Room != null)
                {
                    try
                    {
                        Core.DB.Rooms.Remove(Room);
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

        private void RefreshRooms_Click(object sender, RoutedEventArgs e)
        {
            UpdateData();
        }

        private void SearchRoomsName_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterAndSortRooms();
        }

        private void SortRoomsCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterAndSortRooms();
        }

        private void CleanFilter_OnClick(object sender, RoutedEventArgs e)
        {
            SearchRoomsName.Text = string.Empty;
            SortRoomsCategory.SelectedIndex = 0;
            FilterAndSortRooms();
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
