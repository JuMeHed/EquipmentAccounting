using EquipmentAccounting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentAccounting.ViewModels
{
    internal class ComponentsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EquipmentAccounting.Models.Component> _components;
        private ObservableCollection<ComponentType> _componentTypes;
        private string _nameFilter;
        private ComponentType _selectedComponentType;
        private bool _isActive;

        public ObservableCollection<EquipmentAccounting.Models.Component> Components
        {
            get => _components;
            set
            {
                _components = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ComponentType> ComponentTypes
        {
            get => _componentTypes;
            set
            {
                _componentTypes = value;
                OnPropertyChanged();
            }
        }
        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                OnPropertyChanged();
            }
        }
        public ComponentType SelectedComponentType
        {
            get => _selectedComponentType;
            set
            {
                _selectedComponentType = value;
                OnPropertyChanged();
            }
        }
        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
            }
        }

        public ComponentsViewModel()
        {
            LoadComponents();
            LoadComponentTypes();

        }

        private void LoadComponentTypes()
        {
            var types = new ObservableCollection<ComponentType>(EquipmentEntities.GetContext().ComponentType.ToList());
            types.Insert(0, new ComponentType
            {
                Title = "Все типы"
            });

            ComponentTypes = types;
        }
        private void LoadComponents()
        {
            var components = new ObservableCollection<EquipmentAccounting.Models.Component>(EquipmentEntities.GetContext().Component.ToList());

            foreach (var component in components)
            {
                component.ImagePath = component.GetImagePath();
                var componentInEquipment = EquipmentEntities.GetContext().EquipmentComponent.FirstOrDefault(x => x.ComponentId == component.Id);

                if (componentInEquipment != null && componentInEquipment.IsActual)
                    component.IsActive = true;
                else 
                    component.IsActive = false;
            }

            Components = components;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
