using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Pages.DevicesData
{
    public class DeviceDataView
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string TypeName { get; set; }
        public double Data { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
