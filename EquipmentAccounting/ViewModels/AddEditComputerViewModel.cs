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
    internal class AddEditComputerViewModel : INotifyPropertyChanged
    {
        private string _selectedTypeOfQR;
        private string _typeToEdit;

        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;
        private bool _isEditComponentOpen;
        private bool _isLocationHistoryOpen;
        private bool _isComponentHistoryOpen;

        private Equipment _currentEquipment;
        private State _selectedState;
        private Location _selectedLocation;
        private Models.Component _componentToEdit;
        private Models.Component _previousComponent;
        private Models.Location _previousLocation;

        private ObservableCollection<State> _states;
        private ObservableCollection<Location> _locations;
        private ObservableCollection<Models.Component> _availableComponents;
        private ObservableCollection<EquipmentLocation> _locationHistory;
        private ObservableCollection<EquipmentComponent> _processorHistory;
        private ObservableCollection<EquipmentComponent> _motherboardHistory;
        private ObservableCollection<EquipmentComponent> _caseHistory;
        private ObservableCollection<EquipmentComponent> _coolingHistory;
        private ObservableCollection<EquipmentComponent> _videocardHistory;
        private ObservableCollection<EquipmentComponent> _ramHistory;
        private ObservableCollection<EquipmentComponent> _powerHistory;
        private ObservableCollection<EquipmentComponent> _networkcardHistory;
        private ObservableCollection<EquipmentComponent> _soundcardHistory;

        private BitmapSource _qrCodeImage;

        private Models.Component _centralProcessor;
        private Models.Component _motherBoard;
        private Models.Component _case;
        private Models.Component _cooling;
        private Models.Component _videocard;
        private Models.Component _ram;
        private Models.Component _power;
        private Models.Component _storages;
        private Models.Component _networkCard;
        private Models.Component _soundCard;

        public List<string> TypesOfQR => ConstLists.TYPES_OF_QR;

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
        public Models.Component PreviousComponent
        {
            get => _previousComponent;
            set
            {
                _previousComponent = value;
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

        public string TypeToEdit
        {
            get => _typeToEdit;
            set
            {
                _typeToEdit = value;
                OnPropertyChanged();
            }
        }
        public bool IsSaveDialogOpen
        {
            get => _isSaveDialogOpen;
            set
            {
                _isSaveDialogOpen = value;
                OnPropertyChanged();
            }
        }

        public bool IsComponentsHistoryOpen
        {
            get => _isComponentHistoryOpen;
            set
            {
                _isComponentHistoryOpen = value;
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

        public ObservableCollection<EquipmentComponent> ProcessorHistory
        {
            get => _processorHistory;
            set
            {
                _processorHistory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipmentComponent> MotherboardHistory
        {
            get => _motherboardHistory;
            set
            {
                _motherboardHistory = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EquipmentComponent> CaseHistory
        {
            get => _caseHistory;
            set
            {
                _caseHistory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipmentComponent> CoolingHistory
        {
            get => _coolingHistory;
            set
            {
                _coolingHistory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipmentComponent> VideocardHistory
        {
            get => _videocardHistory;
            set
            {
                _videocardHistory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipmentComponent> RAMHistory
        {
            get => _ramHistory;
            set
            {
                _ramHistory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipmentComponent> PowerHistory
        {
            get => _powerHistory;
            set
            {
                _powerHistory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<EquipmentComponent> NetworkcardHistory
        {
            get => _networkcardHistory;
            set
            {
                _networkcardHistory = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<EquipmentComponent> SoundcardHistory
        {
            get => _soundcardHistory;
            set
            {
                _soundcardHistory = value;
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

        public bool IsEditComponentOpen
        {
            get => _isEditComponentOpen;
            set
            {
                _isEditComponentOpen = value;
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
                    LoadComponents();

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

        public Models.Component ComponentToEdit
        {
            get => _componentToEdit;
            set
            {
                _componentToEdit = value;
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

        public Models.Component CentralProcessor
        {
            get => _centralProcessor;
            set
            {
                _centralProcessor = value;
                OnPropertyChanged();
            }
        }
        public Models.Component Motherboard
        {
            get => _motherBoard;
            set
            {
                _motherBoard = value;
                OnPropertyChanged();
            }
        }

        public Models.Component RAM
        {
            get => _ram;
            set
            {
                _ram = value;
                OnPropertyChanged();
            }
        }
        public Models.Component Case
        {
            get => _case;
            set
            {
                _case = value;
                OnPropertyChanged();
            }
        }
        public Models.Component Cooling
        {
            get => _cooling;
            set
            {
                _cooling = value;
                OnPropertyChanged();
            }
        }
        public Models.Component Videocard
        {
            get => _videocard;
            set
            {
                _videocard = value;
                OnPropertyChanged();
            }
        }
        public Models.Component Power
        {
            get => _power;
            set
            {
                _power = value;
                OnPropertyChanged();
            }
        }
        public Models.Component Storages
        {
            get => _storages;
            set
            {
                _storages = value;
                OnPropertyChanged();
            }
        }
        public Models.Component NetworkCard
        {
            get => _networkCard;
            set
            {
                _networkCard = value;
                OnPropertyChanged();
            }
        }
        public Models.Component SoundCard
        {
            get => _soundCard;
            set
            {
                _soundCard = value;
                OnPropertyChanged();
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

        public ObservableCollection<Models.Component> AvailableComponents
        {
            get => _availableComponents;
            set
            {
                _availableComponents = value;
                OnPropertyChanged();
            }
        }
        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(SaveChanges);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);
        public ICommand GenerateQRCodeCommand => new RelayCommand(GenerateQRCode);
        public ICommand ChangeComponentCommand => new RelayCommand<string>(ChangeComponentOpen);
        public ICommand SaveChangeComponentCommand => new RelayCommand(ChangeComponentInPC);
        public ICommand PrintQRCodeCommand => new RelayCommand(PrintQRCode);
        public ICommand OpenLocationHistoryCommand => new RelayCommand(OpenLocationHistory);
        public ICommand OpenComponentsHistoryCommand => new RelayCommand(OpenComponentHistory);
        public AddEditComputerViewModel()
        {
            LoadLocations();
            LoadStates();

            if (CurrentEquipment == null)
            {
                CurrentEquipment = new Equipment();
                CurrentEquipment.EquipmentTypeId = 1;
                CurrentEquipment.InventoryDate = DateTime.Now;
            }
        }

        private void OpenComponentHistory()
        {
            if (CurrentEquipment != null && CurrentEquipment.Id != 0)
            {
                var equipmentComponent = EquipmentEntities.GetContext().EquipmentComponent.ToList();

                ProcessorHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 1).ToList());

                MotherboardHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 2).ToList());

                RAMHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 3).ToList());

                CaseHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 4).ToList());

                CoolingHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 5).ToList());

                VideocardHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 8).ToList());

                PowerHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                   .Where(x => x.Component.ComponentTypeId == 9).ToList());

                NetworkcardHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 10).ToList());

                SoundcardHistory = new ObservableCollection<EquipmentComponent>(equipmentComponent
                    .Where(x => x.Component.ComponentTypeId == 11).ToList());

                IsComponentsHistoryOpen = true;
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
        private void SaveChanges()
        {
            try
            {
                if (IsEditing)
                {
                    UpdateExistingComputer();
                }
                else
                {
                    AddNewComputer();
                }

                EquipmentEntities.GetContext().SaveChanges();
                CloseDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка. Проверьте правильность заполнения полей.");
            }
        }

        private void PrintQRCode()
        {
            if (QrCodeImage != null)
                QRCodeGenerator.PrintQRCode(QrCodeImage);
        }

        private void UpdateExistingComputer()
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

        private void AddNewComputer()
        {
            EquipmentEntities.GetContext().Equipment.Add(CurrentEquipment);
            EquipmentEntities.GetContext().SaveChanges(); // Сохраняем новый объект, чтобы получить его Id

            // Добавляем новое местоположение
            if (SelectedLocation != null) // Проверьте, что SelectedLocation не null
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
            }
            else
            {
                MessageBox.Show("Не выбрано местоположение для нового оборудования.");
                return;
            }

            // Добавление компонентов
            AddComponentsToEquipment();

            EquipmentEntities.GetContext().SaveChanges();
        }

        private void AddComponentsToEquipment()
        {
            if (CentralProcessor != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = CentralProcessor.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (Motherboard != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = Motherboard.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (Case != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = Case.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (Cooling != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = Cooling.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (RAM != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = RAM.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (Power != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = Power.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (Storages != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = Storages.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            } 

            if (NetworkCard != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = NetworkCard.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

            if (SoundCard != null)
            {
                EquipmentComponent equipmentComponent = new EquipmentComponent
                {
                    EquipmentId = CurrentEquipment.Id,
                    ComponentId = SoundCard.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(equipmentComponent);
            }

        }
        private void LoadComponents()
        {
            if (CurrentEquipment != null)
            {
                try
                {
                    var components = EquipmentEntities.GetContext().EquipmentComponent
                            .Where(x => x.EquipmentId == CurrentEquipment.Id && x.IsActual).ToList();

                    foreach (var component in components)
                    {
                        if (component != null && component.Component != null && component.IsActual)
                        {
                            switch (component.Component.ComponentTypeId)
                            {
                                case 1:
                                    CentralProcessor = component.Component;
                                    break;
                                case 2:
                                    Motherboard = component.Component;
                                    break;
                                case 3:
                                    RAM = component.Component;
                                    break;
                                case 4:
                                    Case = component.Component;
                                    break;
                                case 5:
                                    Cooling = component.Component;
                                    break;
                                case 6:
                                    Storages = component.Component;
                                    break;
                                case 7:
                                    Storages = component.Component;
                                    break;
                                case 8:
                                    Videocard = component.Component;
                                    break;
                                case 9:
                                    Power = component.Component;
                                    break;
                                case 10:
                                    NetworkCard = component.Component;
                                    break;
                                case 11:
                                    SoundCard = component.Component;
                                    break;
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading components: {ex.Message}");
                }
            }
        }

        private void ChangeComponentOpen(string componentType)
        {
            if (string.IsNullOrWhiteSpace(componentType) || CurrentEquipment == null)
            {
                // Обработка случая, когда componentType пустой или CurrentEquipment равен null
                return;
            }

            // Преобразуем строку в int с проверкой
            if (!int.TryParse(componentType, out int typeId))
            {
                // Обработка случая, когда не удалось преобразовать componentType в int
                return;
            }

            // Получение текущего компонента
            var currentComponentEntry = EquipmentEntities.GetContext().EquipmentComponent
                .FirstOrDefault(x => x.EquipmentId == CurrentEquipment.Id && x.Component.ComponentTypeId == typeId);

            if (currentComponentEntry != null)
            {
                // Если компонент уже установлен, заменяем его
                var currentComponent = currentComponentEntry.Component;

                // Логика замены компонента
                // Например, если вы хотите просто обновить его или изменить какие-то свойства
                ComponentToEdit = currentComponent; // Здесь можно заменить на нужный компонент
                                                    // Дополнительная логика для обновления текущего компонента, если нужно
                PreviousComponent = currentComponent;
            }
            else
            {
                PreviousComponent = new Models.Component();
                PreviousComponent.ComponentTypeId = typeId;
            }

            // Получение типа компонента
            var componentTypeEntity = EquipmentEntities.GetContext().ComponentType.FirstOrDefault(x => x.Id == typeId);
            if (componentTypeEntity != null)
            {
                TypeToEdit = componentTypeEntity.Title;
            }
            else
            {
                // Обработка случая, когда тип компонента не найден
                return;
            }

            // Получение доступных компонентов
            var components = EquipmentEntities.GetContext().Component.Where(x => x.ComponentTypeId == typeId
                                                                    && (string.IsNullOrEmpty(x.Note)
                                                                    || !x.Note.Contains("Снят с учёта"))).ToList();
            components = components.Where(x => x.IsActive == false).ToList();
            components.Insert(0, new Models.Component
            {
                Model = "Убрать"
            });

            AvailableComponents = new ObservableCollection<Models.Component>(components);

            IsEditComponentOpen = true;
        }

        private void ChangeComponentInPC()
        {
            if (CurrentEquipment.Id != 0)
            {
                if ((PreviousComponent.Id != 0 && PreviousComponent != null) || ComponentToEdit.Model == "Убрать")
                {
                    var equipmentComponent = EquipmentEntities.GetContext().EquipmentComponent
                                            .Where(x => x.ComponentId == PreviousComponent.Id
                                            && x.EquipmentId == CurrentEquipment.Id)
                                            .OrderByDescending(x => x.Id)
                                            .FirstOrDefault();

                    equipmentComponent.IsActual = false;
                }

                if (ComponentToEdit.Model == "Убрать")
                {
                    switch (PreviousComponent.ComponentTypeId)
                    {
                        case 1:
                            CentralProcessor = null;
                            break;
                        case 2:
                            Motherboard = null;
                            break;
                        case 3:
                            RAM = null;
                            break;
                        case 4:
                            Case = null;
                            break;
                        case 5:
                            Cooling = null;
                            break;
                        case 6:
                            Storages = null;
                            break;
                        case 7:
                            Storages = null;
                            break;
                        case 8:
                            Videocard = null;
                            break;
                        case 9:
                            Power = null;
                            break;
                        case 10:
                            NetworkCard = null;
                            break;
                        case 11:
                            SoundCard = null;
                            break;
                    }

                    //LoadComponents();
                    CloseDialog();
                    EquipmentEntities.GetContext().SaveChanges();
                    return;
                }

                EquipmentComponent newEquipmentComponent = new EquipmentComponent
                {
                    ComponentId = ComponentToEdit.Id,
                    EquipmentId = CurrentEquipment.Id,
                    IsActual = true
                };

                try
                {
                    EquipmentEntities.GetContext().EquipmentComponent.Add(newEquipmentComponent);
                    EquipmentEntities.GetContext().SaveChanges();
                    LoadComponents();
                    CloseDialog();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                switch (PreviousComponent.ComponentTypeId)
                {
                    case 1:
                        CentralProcessor = ComponentToEdit;
                        break;
                    case 2:
                        Motherboard = ComponentToEdit;
                        break;
                    case 3:
                        Case = ComponentToEdit;
                        break;
                    case 4:
                        Cooling = ComponentToEdit;
                        break;
                    case 5:
                        Videocard = ComponentToEdit;
                        break;
                    case 6:
                        RAM = ComponentToEdit;
                        break;
                    case 7:
                        Power = ComponentToEdit;
                        break;
                    case 8:
                        break;
                    case 9:
                        NetworkCard = ComponentToEdit;
                        break;
                    case 10:
                        SoundCard = ComponentToEdit;
                        break;
                }

                LoadComponents();
                CloseDialog();
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
            IsComponentsHistoryOpen = false;
            IsEditComponentOpen = false;
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
