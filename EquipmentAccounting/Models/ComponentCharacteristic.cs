//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EquipmentAccounting.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class ComponentCharacteristic
    {
        public int Id { get; set; }
        public int ComponentTypeCharacteristicId { get; set; }
        public int ComponentId { get; set; }
        public string Value { get; set; }
    
        public virtual Component Component { get; set; }
        public virtual ComponentTypeCharacteristic ComponentTypeCharacteristic { get; set; }
    }
}
