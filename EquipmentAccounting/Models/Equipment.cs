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
    
    public partial class Equipment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Equipment()
        {
            this.EquipmentComponent = new HashSet<EquipmentComponent>();
            this.EquipmentLocation = new HashSet<EquipmentLocation>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public System.DateTime InventoryDate { get; set; }
        public string InventoryNumber { get; set; }
        public string SerialNumber { get; set; }
        public int StateId { get; set; }
        public int EquipmentTypeId { get; set; }
        public string Note { get; set; }
        public virtual string ImagePath { get; set; }

        public virtual string GetImagePath()
        {
            switch (EquipmentTypeId)
            {
                case 1:
                    return "pack://application:,,,/Resources/computer-case1.png";
                case 2:
                    return "pack://application:,,,/Resources/monitor1.png";
                case 3:
                    return "pack://application:,,,/Resources/computer-keyboard1.png";
                case 4:
                    return "pack://application:,,,/Resources/mouse1.png";
                case 5:
                    return "pack://application:,,,/Resources/acoustic1.png";
                case 6:
                    return "pack://application:,,,/Resources/projector1.png";
                case 7:
                    return "pack://application:,,,/Resources/television1.png";
                default:
                    return null;
            }
        }
        public virtual EquipmentType EquipmentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EquipmentComponent> EquipmentComponent { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EquipmentLocation> EquipmentLocation { get; set; }
        public virtual State State { get; set; }
    }
}
