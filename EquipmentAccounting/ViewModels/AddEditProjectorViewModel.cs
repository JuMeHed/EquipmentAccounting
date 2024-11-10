using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace EquipmentAccounting.ViewModels
{
    internal class AddEditProjectorViewModel : INotifyPropertyChanged
    {
        private string _selectedTypeOfQR;

        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;
        private bool _isLocationHistoryOpen;

        private Equipment _currentEquipment;
        private State _selectedState;
        private Location _selectedLocation;
        private Models.Location _previousLocation;

        private ObservableCollection<State> _states;
        private ObservableCollection<Location> _locations;
        private ObservableCollection<EquipmentLocation> _locationHistory;

        private BitmapSource _qrCodeImage;
        public List<string> TypesOfQR => ConstLists.TYPES_OF_QR;

        public bool IsSaveDialogOpen
        {
            get => _isSaveDialogOpen;
            set
            {
                _isSaveDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsLocationHistoryOpen
        {
            get => _isLocationHistoryOpen;
            set
            {
                _isLocationHistoryOpen = value;
                OnPropertyChanged();
            }
        }
        public bool IsExitDialogOpen
        {
            get => _isExitDialogOpen;
            set
            {
                _isExitDialogOpen = value;
                OnPropertyChanged();
            }
        }
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        public Models.Equipment CurrentEquipment
        {
            get => _currentEquipment;
            set
            {
                if (value != null && value.Id != 0)
                {
                    _currentEquipment = value;
                    IsEditing = true;

                    var equipmentLocation = EquipmentEntities.GetContext().EquipmentLocation
                                  .FirstOrDefault(x => x.EquipmentId == CurrentEquipment.Id
                                  && x.IsActual == true);

                    // Проверяем, что equipmentLocation не null
                    if (equipmentLocation != null)
                    {
                        SelectedLocation = equipmentLocation.Location;
                        PreviousLocation = SelectedLocation;
                    }
                    else
                    {
                        // Если equipmentLocation null, сбрасываем SelectedLocation и PreviousLocation
                        SelectedLocation = null;
                        PreviousLocation = null;
                    }
                    OnPropertyChanged();
                }
                else if (value != null && value.Id == 0)
                {
                    _currentEquipment = value;
                }
            }
        }

        public Models.State SelectedState
        {
            get => _selectedState;
            set
            {
                _selectedState = value;
                OnPropertyChanged();
            }
        }
        public Models.Location SelectedLocation
        {
            get => _selectedLocation;
            set
            {
                _selectedLocation = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<State> States
        {
            get => _states;
            set
            {
                _states = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Location> Locations
        {
            get => _locations;
            set
            {
                _locations = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EquipmentLocation> LocationHistory
        {
            get => _locationHistory;
            set
            {
                _locationHistory = value;
                OnPropertyChanged();
            }
        }
        public Models.Location PreviousLocation
        {
            get => _previousLocation;
            set
            {
                _previousLocation = value;
                OnPropertyChanged();
            }
        }
        public string SelectedTypeOfQr
        {
            get => _selectedTypeOfQR;
            set
            {
                _selectedTypeOfQR = value;
                OnPropertyChanged();
                GenerateQRCode();
            }
        }
        public BitmapSource QrCodeImage
        {
            get => _qrCodeImage;
            set
            {
                _qrCodeImage = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand GenerateQRCodeCommand => new RelayCommand(GenerateQRCode);
        public ICommand PrintQRCodeCommand => new RelayCommand(PrintQRCode);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);
        public ICommand OpenLocationHistoryCommand => new RelayCommand(OpenLocationHistory);
        public ICommand SaveCommand => new RelayCommand(SaveChanges);
        public AddEditProjectorViewModel()
        {
            LoadLocations();
            LoadStates();

            if (CurrentEquipment == null)
            {
                CurrentEquipment = new Equipment();
                CurrentEquipment.EquipmentTypeId = 6;
                CurrentEquipment.InventoryDate = DateTime.Now;
            }
        }

        private void SaveChanges()
        {
            try
            {
                if (IsEditing)
                {
                    UpdateExistingMonitor();
                }
                else
                {
                    AddNewMonitor();
                }

                EquipmentEntities.GetContext().SaveChanges();
                CloseDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка. Проверьте правильность заполнения полей.");
            }
        }

        private void OpenLocationHistory()
        {
            if (CurrentEquipment != null)
            {
                var locations = EquipmentEntities.GetContext().EquipmentLocation.Where(x => x.EquipmentId == CurrentEquipment.Id).ToList();
                locations.OrderByDescending(x => x.Date);

                LocationHistory = new ObservableCollection<EquipmentLocation>(locations);

                IsLocationHistoryOpen = true;
            }
        }
        private void UpdateExistingMonitor()
        {
            if (SelectedLocation != PreviousLocation && PreviousLocation != null)
            {
                var equipmentLocation = EquipmentEntities.GetContext().EquipmentLocation
                                          .Where(x => x.LocationId == PreviousLocation.Id
                                          && x.EquipmentId == CurrentEquipment.Id)
                                          .OrderByDescending(x => x.Id)
                                          .FirstOrDefault();

                equipmentLocation.IsActual = false;
                equipmentLocation.Date = DateTime.Today;

                EquipmentLocation newEquipmentLocation = new EquipmentLocation
                {
                    EquipmentId = CurrentEquipment.Id,
                    LocationId = SelectedLocation.Id,
                    Date = DateTime.Today,
                    IsActual = true
                };

                PreviousLocation = EquipmentEntities.GetContext().Location
                    .FirstOrDefault(x => x.Id == newEquipmentLocation.LocationId);
                EquipmentEntities.GetContext().EquipmentLocation.Add(newEquipmentLocation);
            }
        }

        private void AddNewMonitor()
        {
            EquipmentEntities.GetContext().Equipment.Add(CurrentEquipment);
            EquipmentEntities.GetContext().SaveChanges();

            if (SelectedLocation != null)
            {
                EquipmentLocation newEquipmentLocation = new EquipmentLocation
                {
                    EquipmentId = CurrentEquipment.Id,
                    LocationId = SelectedLocation.Id,
                    Date = DateTime.Today,
                    IsActual = true
                };

                PreviousLocation = EquipmentEntities.GetContext().Location
                     .FirstOrDefault(x => x.Id == newEquipmentLocation.LocationId);
                EquipmentEntities.GetContext().EquipmentLocation.Add(newEquipmentLocation);
                EquipmentEntities.GetContext().SaveChanges();
            }
            else
            {
                MessageBox.Show("Не выбрано местоположение для нового оборудования.");
                return;
            }
        }
        private void LoadLocations()
        {
            var locations = new ObservableCollection<Location>(EquipmentEntities.GetContext().Location.ToList());
            Locations = locations;
        }

        private void LoadStates()
        {
            var states = new ObservableCollection<State>(EquipmentEntities.GetContext().State.ToList());
            States = states;
        }

        private void CloseDialog()
        {
            IsExitDialogOpen = false;
            IsSaveDialogOpen = false;
            IsLocationHistoryOpen = false;
        }
        private void Exit()
        {
            CloseDialog();
            Views.EquipmentView equipmentsPage = new Views.EquipmentView();
            EquipmentViewModel viewModel = new EquipmentViewModel();
            viewModel.IsReadOnly = false;
            equipmentsPage.DataContext = viewModel;
            Manager.MenuPage.CurrentPage = equipmentsPage;
        }
        private void GoBack()
        {
            IsExitDialogOpen = true;
        }

        private void OpenSaveDialog()
        {
            IsSaveDialogOpen = true;
        }

        private void GenerateQRCode()
        {
            if (CurrentEquipment != null)
            {
                if (SelectedTypeOfQr == "Сгенерировать по инвентарному номеру")
                {
                    QrCodeImage = QRCodeGenerator.GenerateQRCode(CurrentEquipment.InventoryNumber);
                }
                else if (SelectedTypeOfQr == "Сгенерировать по серийному номеру")
                {
                    QrCodeImage = QRCodeGenerator.GenerateQRCode(CurrentEquipment.SerialNumber);
                }
            }
        }

        private void PrintQRCode()
        {
            if (QrCodeImage != null)
                QRCodeGenerator.PrintQRCode(QrCodeImage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
