CREATE TABLE Users (
  user_id INT PRIMARY KEY IDENTITY,
  username VARCHAR(255) NOT NULL,
  password VARCHAR(255) NOT NULL,
  email VARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE Rooms (
  room_id INT PRIMARY KEY IDENTITY,
  room_name VARCHAR(255) NOT NULL,
  floor INT NOT NULL,
  created_at DATETIME DEFAULT GETDATE()
);

CREATE TABLE Devices (
  device_id INT PRIMARY KEY IDENTITY,
  device_name VARCHAR(255) NOT NULL,
  status BIT NOT NULL,
  room_id INT,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (room_id) REFERENCES Rooms(room_id)
);

CREATE TABLE UsersToDevices (
  id INT PRIMARY KEY IDENTITY,
  device_id INT NOT NULL,
  user_id INT NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (device_id) REFERENCES Devices(device_id),
  FOREIGN KEY (user_id) REFERENCES Users(user_id),
  CONSTRAINT UQ_UsersToDevices_DeviceUser UNIQUE (device_id, user_id)
);

CREATE TABLE Device_Settings (
  setting_id INT PRIMARY KEY IDENTITY,
  device_id INT NOT NULL,
  setting_name VARCHAR(255) NOT NULL,
  setting_value VARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (device_id) REFERENCES Devices(device_id)
);

CREATE TABLE TypeAction (
  type_action_id INT PRIMARY KEY IDENTITY,
  type_name VARCHAR(255) NOT NULL
);

CREATE TABLE Sensor_Data (
  data_id INT PRIMARY KEY IDENTITY,
  sensor_type_id INT NOT NULL,
  data FLOAT NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (sensor_type_id) REFERENCES TypeAction(type_action_id)
);

CREATE TABLE Device_Data (
  id INT PRIMARY KEY IDENTITY,
  device_id INT NOT NULL,
  data_id INT NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (device_id) REFERENCES Devices(device_id),
  FOREIGN KEY (data_id) REFERENCES Sensor_Data(data_id),
  CONSTRAINT UQ_DeviceData_DeviceData UNIQUE (device_id, data_id)
);

CREATE TABLE Events (
  event_id INT PRIMARY KEY IDENTITY,
  event_name VARCHAR(255) NOT NULL,
  event_type_id INT NOT NULL,
  timestamp DATETIME NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (event_type_id) REFERENCES TypeAction(type_action_id)
);

CREATE TABLE Automation_Rules (
  rule_id INT PRIMARY KEY IDENTITY,
  rule_name VARCHAR(255) NOT NULL,
  trigger_event_id INT NOT NULL,
  action_device_id INT NOT NULL,
  action VARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (trigger_event_id) REFERENCES Events(event_id),
  FOREIGN KEY (action_device_id) REFERENCES Devices(device_id)
);

CREATE TABLE User_Preferences (
  preference_id INT PRIMARY KEY IDENTITY,
  user_id INT NOT NULL,
  preference_name VARCHAR(255) NOT NULL,
  preference_value VARCHAR(255) NOT NULL,
  created_at DATETIME DEFAULT GETDATE(),
  FOREIGN KEY (user_id) REFERENCES Users(user_id)
);