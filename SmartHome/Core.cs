using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    class Core
    {
        private static smart_homeEntities _smart_homeEntities;

        public static smart_homeEntities DB = GetContext();

        public static smart_homeEntities GetContext()
        {
            if (_smart_homeEntities == null)
            {
                _smart_homeEntities = new smart_homeEntities();
            }
            return _smart_homeEntities;
        }

    }
}
