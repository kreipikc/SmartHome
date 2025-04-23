using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class Core
    {
        private static Database.smart_homeEntities _smart_homeEntities;

        public static Database.smart_homeEntities DB = GetContext();

        public static Database.smart_homeEntities GetContext()
        {
            if (_smart_homeEntities == null)
            {
                _smart_homeEntities = new Database.smart_homeEntities();
            }
            return _smart_homeEntities;
        }
    }
}
