using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartHome
{
    /// <summary>
    /// Класс для работы с базой данных (Singleton)
    /// </summary>
    class Core
    {
        private static Database.SmartHomeEntities _smartHomeEntities;

        public static Database.SmartHomeEntities DB = GetContext();

        public static Database.SmartHomeEntities GetContext()
        {
            if (_smartHomeEntities == null)
            {
                _smartHomeEntities = new Database.SmartHomeEntities();
            }
            return _smartHomeEntities;
        }
    }
}
