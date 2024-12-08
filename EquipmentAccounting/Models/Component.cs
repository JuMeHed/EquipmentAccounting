namespace EquipmentAccounting.Models
{
    using System;
    using System.Collections.Generic;

    public partial class Component
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Component()
        {
            this.ComponentCharacteristic = new HashSet<ComponentCharacteristic>();
            this.EquipmentComponent = new HashSet<EquipmentComponent>();
        }

        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int ReleaseYear { get; set; }
        public int ComponentTypeId { get; set; }
        public string Note { get; set; }
        public virtual string ImagePath { get; set; }
        public virtual bool IsActive { get; set; }

        public virtual string GetImagePath()
        {
            switch (ComponentTypeId)
            {
                case 1:
                    return "pack://application:,,,/Resources/processor.png";
                case 2:
                    return "pack://application:,,,/Resources/computer.png";
                case 3:
                    return "pack://application:,,,/Resources/ram.png";
                case 4:
                    return "pack://application:,,,/Resources/computer-case.png";
                case 5:
                    return "pack://application:,,,/Resources/cooler.png";
                case 6:
                    return "pack://application:,,,/Resources/ssd.png";
                case 7:
                    return "pack://application:,,,/Resources/hdd.png";
                case 8:
                    return "pack://application:,,,/Resources/gpu-mining.png";
                case 9:
                    return "pack://application:,,,/Resources/power.png";
                case 10:
                    return "pack://application:,,,/Resources/network-interface-card.png";
                case 11:
                    return "pack://application:,,,/Resources/sound-card.png";
                default:
                    return null;
            }
        }

        public virtual ComponentType ComponentType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ComponentCharacteristic> ComponentCharacteristic { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EquipmentComponent> EquipmentComponent { get; set; }
    }
}
