using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using EquipmentAccounting.Views.AdminViews;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class ComponentsViewModel : INotifyPropertyChanged
    {
        //TODO третье состояние кнопки применен (все компоненты)
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
        public ICommand OpenRAMAddEditPageCommand => new RelayCommand(OnRAMAddEditOpen);
        public ICommand OpenCaseAddEditPageCommand => new RelayCommand(OnCaseAddEditOpen);
        public ICommand OpenCoolerAddEditPageCommand => new RelayCommand(OnCoolerAddEditOpen);
        public ICommand OpenSSDAddEditPageCommand => new RelayCommand(OnSSDAddEditOpen);
        public ICommand OpenHDDAddEditPageCommand => new RelayCommand(OnHDDAddEditOpen);
        public ICommand OpenGPUAddEditPageCommand => new RelayCommand(OnGPUAddEditOpen);
        public ICommand OpenPowerAddEditPageCommand => new RelayCommand(OnPowerAddEditOpen);
        public ICommand OpenNetworkCardAddEditPageCommand => new RelayCommand(OnNetworkCardAddEditOpen);
        public ICommand OpenSoundCardAddEditPageCommand => new RelayCommand(OnSoundCardAddEditOpen);
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
                    OnRAMAddEditOpen();
                    break;
                case 4:
                    OnCaseAddEditOpen();
                    break;
                case 5:
                    OnCoolerAddEditOpen();
                    break;
                case 6:
                    OnSSDAddEditOpen();
                    break;
                case 7:
                    OnHDDAddEditOpen();
                    break;
                case 8:
                    OnGPUAddEditOpen();
                    break;
                case 9:
                    OnPowerAddEditOpen();
                    break;
                case 10:
                    OnNetworkCardAddEditOpen();
                    break;
                case 11:
                    OnSoundCardAddEditOpen();
                    break;
                default:
                    MessageBox.Show("Неизвестный тип компонента");
                    break;
            }
        }

        private void OnMotherBoardAddEditOpen()
        {
            MotherboardAddEditView view = new MotherboardAddEditView();
            MotherboardAddEditViewModel viewModel = new MotherboardAddEditViewModel();
            viewModel.CurrentComponent = SelectedComponent;
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnCPUAddEditOpen()
        {
            CPUAddEditView view = new CPUAddEditView();
            CPUAddEditViewModel viewModel = new CPUAddEditViewModel();
            viewModel.CurrentComponent = SelectedComponent;
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnRAMAddEditOpen()
        {
            RAMAddEditView view = new RAMAddEditView();
            RAMAddEditViewModel viewModel = new RAMAddEditViewModel();
            viewModel.CurrentComponent = SelectedComponent;
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnCaseAddEditOpen()
        {
            CaseAddEditView view = new CaseAddEditView();
            CaseAddEditViewModel viewModel = new CaseAddEditViewModel();
            viewModel.CurrentComponent = SelectedComponent;
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnCoolerAddEditOpen()
        {
            CoolerAddEditView view = new CoolerAddEditView();
            CoolerAddEditViewModel viewModel = new CoolerAddEditViewModel();
            viewModel.CurrentComponent = SelectedComponent;
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnSSDAddEditOpen()
        {
            SSDAddEditView view = new SSDAddEditView();
            SSDAddEditViewModel viewModel = new SSDAddEditViewModel();
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnHDDAddEditOpen()
        {
            HDDAddEditView view = new HDDAddEditView();
            HDDAddEditViewModel viewModel = new HDDAddEditViewModel();
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnGPUAddEditOpen()
        {
            GPUAddEditView view = new GPUAddEditView();
            GPUAddEditViewModel viewModel = new GPUAddEditViewModel();
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }
        private void OnPowerAddEditOpen()
        {
            PowerAddEditView view = new PowerAddEditView();
            PowerAddEditViewModel viewModel = new PowerAddEditViewModel();
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnNetworkCardAddEditOpen()
        {
            NetworkCardAddEditView view = new NetworkCardAddEditView();
            NetworCardAddEditViewModel viewModel = new NetworCardAddEditViewModel();
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnSoundCardAddEditOpen()
        {
            SoundCardAddEditView view = new SoundCardAddEditView();
            SoundCardAddEditViewModel viewModel = new SoundCardAddEditViewModel();
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
