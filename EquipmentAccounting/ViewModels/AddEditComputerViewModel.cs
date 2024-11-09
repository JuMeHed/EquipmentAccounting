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

        private Equipment _currentEquipment;
        private State _selectedState;
        private Location _selectedLocation;
        private Models.Component _componentToEdit;
        private Models.Component _previousComponent;
        private Models.Location _previousLocation;

        private ObservableCollection<State> _states;
        private ObservableCollection<Location> _locations;
        private ObservableCollection<Models.Component> _availableComponents;

        private BitmapSource _qrCodeImage;

        private Models.Component _centralProcessor;
        private Models.Component _motherBoard;
        private Models.Component _case;
        private Models.Component _cooling;
        private Models.Component _videocard;
        private Models.Component _ram;
        private Models.Component _power;
        private List<Models.Component> _storages;
        private Models.Component _networkCard;
        private Models.Component _soundCard;

        public List<string> TypesOfQR => ConstLists.TYPES_OF_QR;

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
                if (value == null)
                {
                    _currentEquipment = new Equipment
                    {
                        EquipmentTypeId = 1
                    };
                }
                else
                {
                    _currentEquipment = value;
                    IsEditing = true;
                    LoadComponents();
                    SelectedLocation = EquipmentEntities.GetContext().EquipmentLocation
                                        .FirstOrDefault(x => x.EquipmentId == _currentEquipment.Id
                                        && x.IsActual == true).Location;
                    PreviousLocation = SelectedLocation;
                    OnPropertyChanged();
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
        public List<Models.Component> Storages
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
       
        public AddEditComputerViewModel()
        {
            Storages = new List<Models.Component>();
            LoadLocations();
            LoadStates();
        }

        private void SaveChanges()
        {
            // Убедитесь, что CurrentEquipment инициализирован
            if (CurrentEquipment == null)
            {
                // Если CurrentEquipment равен null, создайте новый объект
                CurrentEquipment = new Equipment();
            }

            try
            {
                if (CurrentEquipment.Id != 0)
                {
                    // Если Id не равен 0, обновляем существующий компьютер
                    UpdateExistingComputer();
                }
                else
                {
                    // Если Id равен 0, добавляем новый компьютер
                    AddNewComputer();
                }

                EquipmentEntities.GetContext().SaveChanges();
                CloseDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
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

                EquipmentEntities.GetContext().EquipmentLocation.Add(newEquipmentLocation);
            }
        }

        private void AddNewComputer()
        {
            // Создание нового объекта Equipment
            var newEquipment = new Equipment
            {
                InventoryNumber = CurrentEquipment.InventoryNumber,
                SerialNumber = CurrentEquipment.SerialNumber,
                EquipmentTypeId = CurrentEquipment.EquipmentTypeId,
                // Заполните другие необходимые поля здесь
            };

            EquipmentEntities.GetContext().Equipment.Add(newEquipment);
            EquipmentEntities.GetContext().SaveChanges(); // Сохраняем новый объект, чтобы получить его Id

            // Устанавливаем текущий объект на только что добавленный
            CurrentEquipment = newEquipment;

            // Добавляем новое местоположение
            EquipmentLocation newEquipmentLocation = new EquipmentLocation
            {
                EquipmentId = CurrentEquipment.Id,
                LocationId = SelectedLocation.Id,
                Date = DateTime.Today,
                IsActual = true
            };

            EquipmentEntities.GetContext().EquipmentLocation.Add(newEquipmentLocation);

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

            // Обработка компонентов, если они есть
            foreach (var component in Storages) // Или другие компоненты, которые вы хотите добавить
            {
                EquipmentComponent newEquipmentComponent = new EquipmentComponent
                {
                    ComponentId = component.Id,
                    EquipmentId = CurrentEquipment.Id,
                    IsActual = true
                };

                EquipmentEntities.GetContext().EquipmentComponent.Add(newEquipmentComponent);
            }

            EquipmentEntities.GetContext().SaveChanges();
        }

        private void LoadComponents()
        {
            if (CurrentEquipment != null)
            {
                try
                {
                    var components = EquipmentEntities.GetContext().EquipmentComponent
                            .Where(x => x.EquipmentId == CurrentEquipment.Id && x.IsActual == true).ToList();

                    foreach (var component in components)
                    {
                        if (component != null && component.Component != null)
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
                                    Storages.Add(component.Component);
                                    break;
                                case 7:
                                    Storages.Add(component.Component);
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
            AvailableComponents = new ObservableCollection<Models.Component>(components);

            IsEditComponentOpen = true;
        }

        private void ChangeComponentInPC()
        {
            if (CurrentEquipment != null)
            {
                if (PreviousComponent != null)
                {
                    var equipmentComponent = EquipmentEntities.GetContext().EquipmentComponent
                                            .Where(x => x.ComponentId == PreviousComponent.Id 
                                            && x.EquipmentId == CurrentEquipment.Id)
                                            .OrderByDescending(x => x.Id)
                                            .FirstOrDefault();

                    equipmentComponent.IsActual = false;
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
        private void OnSaveChanges()
        {

        }
        private void CloseDialog()
        {
            IsExitDialogOpen = false;
            IsSaveDialogOpen = false;
            IsEditComponentOpen = false;
        }
        private void Exit()
        {
            CloseDialog();
            Views.EquipmentView componentsPage = new Views.EquipmentView();
            Manager.MenuPage.CurrentPage = componentsPage;
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
