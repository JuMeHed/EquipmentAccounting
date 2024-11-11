using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using EquipmentAccounting.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class EquipmentViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Equipment> _equipments;
        private ObservableCollection<EquipmentType> _equipmentTypes;
        private ObservableCollection<Equipment> _filteredEquipments;

        private string _serachText;
        private string _message;

        private Equipment _selectedEquipment;
        private EquipmentType _selectedEquipmentType;

        private bool _isReadOnly;
        private bool _isMessageOpen;
        public bool IsMessageOpen
        {
            get => _isMessageOpen;
            set
            {
                _isMessageOpen = value;
                OnPropertyChanged();
            }
        }
        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            set
            {
                _equipments = value;
                OnPropertyChanged();
                FilterEquipment();
            }
        }

        public ObservableCollection<EquipmentType> EquipmentTypes
        {
            get => _equipmentTypes;
            set
            {
                _equipmentTypes = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Equipment> FilteredEquipments
        {
            get => _filteredEquipments;
            set
            {
                _filteredEquipments = value;
                OnPropertyChanged();
            }
        }

        public string SearchText
        {
            get => _serachText;
            set
            {
                _serachText = value;
                OnPropertyChanged();
                FilterEquipment();
            }
        }

        public Equipment SelectedEquipment
        {
            get => _selectedEquipment;
            set
            {
                _selectedEquipment = value;
                OnPropertyChanged();
            }
        }
        public EquipmentType SelectedEquipmentType
        {
            get => _selectedEquipmentType;
            set
            {
                _selectedEquipmentType = value;
                OnPropertyChanged();
                FilterEquipment();
            }
        }

        public bool IsReadOnly
        {
            get => _isReadOnly;
            set
            {
                _isReadOnly = !value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenAddEditComputerViewCommand => new RelayCommand(OnAddEditComputerOpen);
        public ICommand OpenAddEditMonitorViewCommand => new RelayCommand(OnAddEditMonitorOpen);
        public ICommand OpenAddEditKeyboardCommand => new RelayCommand(OnAddEditKeyBoardOpen);
        public ICommand OpenAddEditMouseCommand => new RelayCommand(OnAddEditMouseOpen);
        public ICommand OpenAddEditAudioCommand => new RelayCommand(OnAddEditAudioOpen);
        public ICommand OpenAddEditProjectorCommand => new RelayCommand(OnAddEditProjectorOpen);
        public ICommand OpenAddEditSmartBoardCommand => new RelayCommand(OnAddEditSmartBoardOpen);
        public ICommand OpenAddEditPageCommand => new RelayCommand<Models.Equipment>(OnAddEditPageOpen);
        public ICommand DeregisterEquipmentCommand => new RelayCommand<Models.Equipment>(DeregisterEquipment);
        public EquipmentViewModel()
        {
            LoadEquipment();
            LoadEquipmentTypes();
        }

        private void OnAddEditPageOpen(Models.Equipment equipment)
        {
            if (IsReadOnly)
            {
                if (equipment == null) return;

                switch (equipment.EquipmentTypeId)
                {
                    case 1:
                        OnAddEditComputerOpen();
                        break;
                    case 2:
                        OnAddEditMonitorOpen();
                        break;
                    case 3:
                        OnAddEditKeyBoardOpen();
                        break;
                    case 4:
                        OnAddEditMouseOpen();
                        break;
                    case 5:
                        OnAddEditAudioOpen();
                        break;
                    case 6:
                        OnAddEditProjectorOpen();
                        break;
                    case 7:
                        OnAddEditSmartBoardOpen();
                        break;

                }

            }
        }

        private void OnAddEditComputerOpen()
        {
            AddEditComputerView view = new AddEditComputerView();
            AddEditComputerViewModel viewModel = new AddEditComputerViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnAddEditMonitorOpen()
        {
            AddEditMonitorView view = new AddEditMonitorView();
            AddEditMonitorViewModel viewModel = new AddEditMonitorViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }
        private void OnAddEditKeyBoardOpen()
        {
            AddEditKeyBoardView view = new AddEditKeyBoardView();
            AddEditKeyboardViewModel viewModel = new AddEditKeyboardViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnAddEditMouseOpen()
        {
            AddEditMouseView view = new AddEditMouseView();
            AddEditMouseViewModel viewModel = new AddEditMouseViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }
        private void OnAddEditAudioOpen()
        {
            AddEditAudioView view = new AddEditAudioView();
            AddEditAudioViewModel viewModel = new AddEditAudioViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }
        private void OnAddEditProjectorOpen()
        {
            AddEditProjectorView view = new AddEditProjectorView();
            AddEditProjectorViewModel viewModel = new AddEditProjectorViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }
        private void OnAddEditSmartBoardOpen()
        {
            AddEditSmartboardView view = new AddEditSmartboardView();
            AddEditSmartboardViewModel viewModel = new AddEditSmartboardViewModel();
            view.DataContext = viewModel;
            viewModel.CurrentEquipment = SelectedEquipment;
            Manager.MenuPage.CurrentPage = view;
        }
        private void LoadEquipment()
        {
            if (Classes.User.CurrentUser.AccessLevelId == 1)
            {
                var equipment = new ObservableCollection<Equipment>(EquipmentEntities.GetContext().Equipment
                        .Where(x => x.StateId != 4));

                foreach (var item in equipment)
                {
                    item.ImagePath = item.GetImagePath();
                }

                Equipments = equipment;
            }
            else
            {
                var usersLocations = EquipmentEntities.GetContext().Location
                                                    .Where(x => x.ResponsibleId == Classes.User.CurrentUser.Id)
                                                    .Select(x => x.Id) // Получаем только Id кабинетов
                                                    .ToList();

                var equipmentInLocations = EquipmentEntities.GetContext().EquipmentLocation
                    .Where(x => usersLocations.Contains(x.LocationId) && x.IsActual) 
                    .Select(x => x.Equipment)
                    .ToList();

                // Создаем ObservableCollection из списка оборудования
                ObservableCollection<Equipment> equipment = new ObservableCollection<Equipment>(equipmentInLocations);

                foreach (var item in equipment)
                {
                    item.ImagePath = item.GetImagePath();
                }

                Equipments = equipment;
            }
        }

        private void LoadEquipmentTypes()
        {
            var types = new ObservableCollection<EquipmentType>(EquipmentEntities.GetContext().EquipmentType.ToList());
            types.Insert(0, new EquipmentType { Title = "Все типы" });
            EquipmentTypes = types;
        }
        private void FilterEquipment()
        {
            try
            {
                if (Equipments == null || Equipments.Count == 0)
                {
                    FilteredEquipments = new ObservableCollection<Equipment>();
                    return;
                }

                var filteredEquipment = Equipments.AsEnumerable();

                if (!string.IsNullOrEmpty(SearchText))
                {
                    filteredEquipment = filteredEquipment
                                        .Where(x => x.Title.ToLower().Contains(SearchText.ToLower())
                                        || x.InventoryNumber.ToLower().Contains(SearchText.ToLower())
                                        || x.SerialNumber.ToLower().Contains(SearchText.ToLower()));
                }

                if (SelectedEquipmentType != null && SelectedEquipmentType != EquipmentTypes[0])
                {
                    filteredEquipment = filteredEquipment
                                        .Where(x => x.EquipmentTypeId == SelectedEquipmentType.Id).ToList();
                }

                FilteredEquipments = new ObservableCollection<Equipment>(filteredEquipment);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DeregisterEquipment(Equipment selectedEquipment)
        {
            try
            {
                selectedEquipment.StateId = 4;

                var equipmentComponent = EquipmentEntities.GetContext().EquipmentComponent
                    .Where(x => x.IsActual && x.EquipmentId == selectedEquipment.Id).ToList();
                foreach (var equipment in equipmentComponent)
                {
                    equipment.IsActual = false;
                }

                EquipmentEntities.GetContext().SaveChanges();
                LoadEquipment();
            }
            catch
            {
                DisplayMessage($"{selectedEquipment.Title} не удалось снять с учета.");
                return;
            }
            DisplayMessage($"{selectedEquipment.Title} снят с учета.");
        }

        private async Task DisplayMessage(string message)
        {
            Message = message;
            IsMessageOpen = true;

            await Task.Delay(3000);


            IsMessageOpen = false;
            Message = "";
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
