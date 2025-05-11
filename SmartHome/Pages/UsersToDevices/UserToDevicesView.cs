using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome.Pages.UsersToDevices
{
    public class UserToDevicesView
    {
        public int Id { get; set; }
        public string DeviceName { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
