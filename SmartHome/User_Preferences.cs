//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SmartHome
{
    using System;
    using System.Collections.Generic;
    
    public partial class User_Preferences
    {
        public int preference_id { get; set; }
        public int user_id { get; set; }
        public string preference_name { get; set; }
        public string preference_value { get; set; }
        public Nullable<System.DateTime> created_at { get; set; }
    
        public virtual Users Users { get; set; }
    }
}
