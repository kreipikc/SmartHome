using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SmartHome
{
    /// <summary>
    /// Класс с общими утилитами
    /// </summary>
    class Utils
    {
        public enum ComboBoxName
        {
            Users,
            Devices,
            TypeActions,
            SensorData,
            Rooms,
            Events
        }

        public static void LoadDataToComboBox(ComboBox ComboBox, ComboBoxName comboBoxName)
        {
            try
            {
                switch (comboBoxName)
                {
                    case ComboBoxName.TypeActions:
                        var types = Core.DB.TypeAction.ToList();

                        ComboBox.ItemsSource = types;

                        if (types.Any())
                        {
                            ComboBox.SelectedIndex = 0;
                        }
                        break;
                    case ComboBoxName.Devices:
                        var dev = Core.DB.Devices.ToList();

                        ComboBox.ItemsSource = dev;

                        if (dev.Any())
                        {
                            ComboBox.SelectedIndex = 0;
                        }
                        break;
                    case ComboBoxName.Users:
                        var us = Core.DB.Users.ToList();

                        ComboBox.ItemsSource = us;

                        if (us.Any())
                        {
                            ComboBox.SelectedIndex = 0;
                        }
                        break;
                    case ComboBoxName.SensorData:
                        var data = Core.DB.Sensor_Data.ToList();

                        ComboBox.ItemsSource = data;

                        if (data.Any())
                        {
                            ComboBox.SelectedIndex = 0;
                        }
                        break;
                    case ComboBoxName.Rooms:
                        var rooms = Core.DB.Rooms.ToList();

                        ComboBox.ItemsSource = rooms;

                        if (rooms.Any())
                        {
                            ComboBox.SelectedIndex = 0;
                        }
                        break;
                    case ComboBoxName.Events:
                        var events = Core.DB.Events.ToList();

                        ComboBox.ItemsSource = events;

                        if (events.Any())
                        {
                            ComboBox.SelectedIndex = 0;
                        }
                        break;
                    default:
                        throw new Exception($"Ошибка в ComboBoxName: {comboBoxName} нет в данном контексте");
                }
            }
            catch (Exception ex)
            {
                SmartHome.Utils.PrintError(ex, "Ошибка при загрузке данных: ");
            }
        }
        public static string GetHash(string password)
        {
            using (var hash = SHA1.Create())
            {
                return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
            }
        }

        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(emailPattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }

        public static void PrintError(Exception ex, string Message = null)
        {
            if (Message == null)
            {
                MessageBox.Show(
                    $"Message: {ex.Message}",
                    "Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
                return;
            }

            MessageBox.Show(
                $"{Message}: {ex.Message}",
                "Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }
    }
}
