using SmartHome.Database;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

namespace SmartHome.Pages.Devices
{
    class DeviceViewModel : INotifyPropertyChanged
    {
        private bool _isEditMode;
        private string _id;
        private string _name;
        private bool _status;
        private int _selectedRoomId;
        private Database.Devices _currentDevice;

        public DeviceViewModel(Database.Devices device = null)
        {
            _currentDevice = device;
            IsEditMode = device != null;

            SaveCommand = new RelayCommand(Save, CanSave);
            CancelCommand = new RelayCommand(Cancel);

            if (IsEditMode)
            {
                Id = device.device_id.ToString();
                Name = device.device_name;
                Status = device.status;
                SelectedRoomId = (int)device.room_id;
            }

            Rooms = new ObservableCollection<Rooms>(Core.DB.Rooms.ToList());
        }

        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                _isEditMode = value;
                OnPropertyChanged(nameof(IsEditMode));
            }
        }

        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                OnPropertyChanged(nameof(Id));
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public bool Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public int SelectedRoomId
        {
            get => _selectedRoomId;
            set
            {
                _selectedRoomId = value;
                OnPropertyChanged(nameof(SelectedRoomId));
            }
        }

        public ObservableCollection<Rooms> Rooms { get; set; }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        private void Save(object parameter)
        {
            if (IsEditMode)
            {
                UpdateDevice();
            }
            else
            {
                CreateDevice();
            }
        }

        private bool CanSave(object parameter)
        {
            return !string.IsNullOrEmpty(Name);
        }

        private static void Cancel(object parameter)
        {
            NavigationService.GoBack();
        }

        private void UpdateDevice()
        {
            try
            {
                if (string.IsNullOrEmpty(Id))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }

                int id = Convert.ToInt32(Id);

                if (Core.DB.Devices.Any(u => u.device_name == Name && u.device_id != id))
                {
                    MessageBox.Show("Девайс с таким именем уже существует");
                    return;
                }

                var device = Core.DB.Devices.FirstOrDefault(h => h.device_id == id);
                if (device == null)
                {
                    MessageBox.Show("Девайс не найден");
                    return;
                }

                device.device_name = Name;
                device.room_id = SelectedRoomId;
                device.status = Status;

                Core.DB.SaveChanges();
                MessageBox.Show("Девайс успешно обновлен");

                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        private void CreateDevice()
        {
            try
            {
                if (string.IsNullOrEmpty(Name))
                {
                    MessageBox.Show("Заполните все поля");
                    return;
                }

                if (Core.DB.Devices.Any(u => u.device_name == Name))
                {
                    MessageBox.Show("Девайс с таким названием уже существует");
                    return;
                }

                var newDevice = new Database.Devices
                {
                    device_name = Name,
                    status = Status,
                    room_id = SelectedRoomId,
                    created_at = DateTime.Now
                };

                Core.DB.Devices.Add(newDevice);
                Core.DB.SaveChanges();

                MessageBox.Show("Девайс успешно создан");
                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
