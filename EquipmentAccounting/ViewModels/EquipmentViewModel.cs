using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using EquipmentAccounting.Views;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

        private Equipment _selectedEquipment;
        private EquipmentType _selectedEquipmentType;

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

        public ICommand OpenAddEditComputerViewCommand => new RelayCommand(OpenAddEditComputerView);
        public ICommand OpenAddEditMonitorViewCommand => new RelayCommand(OnAddEditMonitorOpen);
        public ICommand OpenAddEditKeyboardCommand => new RelayCommand(OnAddEditKeyBoardOpen);
        public ICommand OpenAddEditMouseCommand => new RelayCommand(OnAddEditMouseOpen);
        public ICommand OpenAddEditAudioCommand => new RelayCommand(OnAddEditAudioOpen);
        public ICommand OpenAddEditProjectorCommand => new RelayCommand(OnAddEditProjectorOpen);
        public ICommand OpenAddEditSmartBoardCommand => new RelayCommand(OnAddEditSmartBoardOpen);
        public ICommand OpenAddEditPageCommand => new RelayCommand<Models.Equipment>(OnAddEditPageOpen);
        public EquipmentViewModel()
        {
            LoadEquipment();   
            LoadEquipmentTypes();
        }

        private void OnAddEditPageOpen(Models.Equipment equipment)
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

        private void OnAddEditComputerOpen()
        {
            AddEditComputerView view = new AddEditComputerView();
            AddEditComputerViewModel viewModel = new AddEditComputerViewModel();
            viewModel.CurrentEquipment = SelectedEquipment;
            view.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = view;
        }

        private void OnAddEditMonitorOpen()
        {

        }
        private void OnAddEditKeyBoardOpen()
        {

        }

        private void OnAddEditMouseOpen()
        {

        }
        private void OnAddEditAudioOpen()
        {

        }
        private void OnAddEditProjectorOpen()
        {

        }
        private void OnAddEditSmartBoardOpen()
        {

        }
        private void LoadEquipment()
        {
            var equipment = new ObservableCollection<Equipment>(EquipmentEntities.GetContext().Equipment.ToList());

            foreach (var item in equipment)
            {
                item.ImagePath = item.GetImagePath();
            }

            Equipments = equipment;
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
                if (Equipments == null ||  Equipments.Count == 0)
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

        private void OpenAddEditComputerView()
        {
            AddEditComputerView view = new AddEditComputerView();
            AddEditComputerViewModel viewModel = new AddEditComputerViewModel();
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
