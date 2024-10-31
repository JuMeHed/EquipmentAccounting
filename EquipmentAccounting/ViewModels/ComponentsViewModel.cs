using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class ComponentsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<EquipmentAccounting.Models.Component> _components;
        private ObservableCollection<ComponentType> _componentTypes;
        private ObservableCollection<EquipmentAccounting.Models.Component> _filteredComponents;
        private Models.Component _selectedComponent;
        private string _nameFilter;
        private ComponentType _selectedComponentType;
        private bool _isActive;
        private bool _isContextMenuOpen;

        public ObservableCollection<EquipmentAccounting.Models.Component> Components
        {
            get => _components;
            set
            {
                _components = value;
                OnPropertyChanged();
                FilterComponents();
            }
        }

        public ObservableCollection<EquipmentAccounting.Models.Component> FilteredComponents
        {
            get => _filteredComponents;
            private set
            {
                _filteredComponents = value;
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
                FilterComponents();
            }
        }

        public string NameFilter
        {
            get => _nameFilter;
            set
            {
                _nameFilter = value;
                OnPropertyChanged();
                FilterComponents();
            }
        }

        public ComponentType SelectedComponentType
        {
            get => _selectedComponentType;
            set
            {
                _selectedComponentType = value;
                OnPropertyChanged();
                FilterComponents();
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged();
                FilterComponents();
            }
        }

        public bool IsContextMenuOpen 
        {
            get => _isContextMenuOpen;
            set
            {
                _isContextMenuOpen = value;
                OnPropertyChanged();
            }
        }

        public Models.Component SelectedComponent
        {
            get => _selectedComponent;
            set
            {
                _selectedComponent = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenAddEditPageCommand => new RelayCommand<Models.Component>(onAddEditPageOpen);
        public ICommand OpenMotherboardAddEditPageCommand => new RelayCommand(OnMotherBoardAddEditOpen);
        public ICommand OpenCPUAddEdit => new RelayCommand(OnCPUAddEditOpen);
        public ComponentsViewModel()
        {
            LoadComponents();
            LoadComponentTypes();
            FilterComponents(); 
        }

        private void LoadComponentTypes()
        {
            var types = new ObservableCollection<ComponentType>(EquipmentEntities.GetContext().ComponentType.ToList());
            types.Insert(0, new ComponentType { Title = "Все типы" });
            ComponentTypes = types;
        }

        private void LoadComponents()
        {
            var components = new ObservableCollection<EquipmentAccounting.Models.Component>(EquipmentEntities.GetContext().Component.ToList());

            foreach (var component in components)
            {
                component.ImagePath = component.GetImagePath();
                var componentInEquipment = EquipmentEntities.GetContext().EquipmentComponent
                                           .FirstOrDefault(x => x.ComponentId == component.Id);

                component.IsActive = componentInEquipment != null && componentInEquipment.IsActual;
            }

            Components = components;
        }

        private void FilterComponents()
        {
            try
            {
                if (Components == null || Components.Count == 0)
                {
                    FilteredComponents = new ObservableCollection<Models.Component>();
                    return;
                }

                var filteredComponents = Components.AsEnumerable();

                filteredComponents = filteredComponents.Where(x => x.IsActive == IsActive);

                if (!string.IsNullOrEmpty(NameFilter))
                {
                    filteredComponents = filteredComponents
                                         .Where(x => x.Model.ToLower().Contains(NameFilter.ToLower()));
                }
                if (SelectedComponentType != null && SelectedComponentType != ComponentTypes[0])
                {
                    filteredComponents = filteredComponents.Where(x => x.ComponentType == SelectedComponentType);
                }

                FilteredComponents = new ObservableCollection<Models.Component>(filteredComponents);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void onAddEditPageOpen(Models.Component component)
        {
            if (component == null) return;

            switch (component.ComponentTypeId)
            {
                case 1:
                    OnCPUAddEditOpen();
                    break;
                case 2:
                    OnMotherBoardAddEditOpen();
                    break;
                case 3:
                    break;
                default:
                    MessageBox.Show("Неизвестный тип компонента");
                    break;
            }
        }

        private void OnMotherBoardAddEditOpen()
        {
            Views.AdminViews.MotherboardAddEditView motherboardAddEditView = new Views.AdminViews.MotherboardAddEditView();
            MotherboardAddEditViewModel viewModel = new MotherboardAddEditViewModel();
            viewModel.CurrentComponent = SelectedComponent;
            motherboardAddEditView.DataContext = viewModel;
            Classes.Manager.MenuPage.CurrentPage = motherboardAddEditView;
        }

        private void OnCPUAddEditOpen()
        {
            Views.AdminViews.CPUAddEditView cpuAddEdit = new Views.AdminViews.CPUAddEditView();
            CPUAddEditViewModel cPUAddEditViewModel = new CPUAddEditViewModel();
            cPUAddEditViewModel.CurrentComponent = SelectedComponent;
            cpuAddEdit.DataContext = cPUAddEditViewModel;
            Classes.Manager.MenuPage.CurrentPage = cpuAddEdit;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
