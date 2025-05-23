﻿using System;
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

namespace SmartHome.Pages
{
    /// <summary>
    /// Логика взаимодействия для HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
            Loaded += (s, e) => NavigateToDevices(null, null);
        }

        private void NavigateToDevices(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Devices.DevicesPage());
        }

        private void NavigateToRooms(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Rooms.RoomsPage());
        }

        private void NavigateToUsers(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Users.UsersPage());
        }

        private void NavigateToAutomationRules(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new AutomationRules.AutomationRulesPage());
        }

        private void NavigateToEvents(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Events.EventsPage());
        }

        private void NavigateToTypeAction(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new TypeAction.TypeActionPage());
        }

        private void NavigateToDevicesData(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new DevicesData.DevicesDataPage());
        }

        private void NavigateToUsersToDevices(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new UsersToDevices.UsersToDevicesPage());
        }

        private void NavigateToSensorData(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new SensorData.SensorDataPage());
        }
    }
}
