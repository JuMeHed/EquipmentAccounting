using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace EquipmentAccounting.ViewModels
{
    internal class GPUAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private string _frequency;
        private string _technology;
        private string _maxResolution;
        private string _videoMemoryCapacity;
        private string _videoMemoryType;
        private string _memoryBus;
        private string _connectionInterface;
        private string _power;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        public List<string> AvailableTechnologies => ConstLists.TECHNOLOGIES;
        public List<string> AvailableResolutions => ConstLists.MAX_RESOLUTIONS;
        public List<string> AvailableVideoMemoryCapacities => ConstLists.VIDEO_MEMORY_CAPACITIES;
        public List<string> AvailableVideoMemoryTypes => ConstLists.VIDEO_MEMORY_TYPES;
        public List<string> AvailableMemoryBus => ConstLists.MEMORY_BUSES;

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

        public string Frequency
        {
            get => _frequency;
            set
            {
                _frequency = value;
                OnPropertyChanged();
            }
        }

        public string Technology
        {
            get => _technology;
            set
            {
                _technology = value;
                OnPropertyChanged();
            }
        }

        public string MaxResolution
        {
            get => _maxResolution;
            set
            {
                _maxResolution = value;
                OnPropertyChanged();
            }
        }

        public string VideoMemoryCapacity
        {
            get => _videoMemoryCapacity;
            set
            {
                _videoMemoryCapacity = value;
                OnPropertyChanged();
            }
        }

        public string VideoMemoryType
        {
            get => _videoMemoryType;
            set
            {
                _videoMemoryType = value;
                OnPropertyChanged();
            }
        }

        public string MemoryBus
        {
            get => _memoryBus;
            set
            {
                _memoryBus = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionInterface
        {
            get => _connectionInterface;
            set
            {
                _connectionInterface = value;
                OnPropertyChanged();
            }
        }

        public string Power
        {
            get => _power;
            set
            {
                _power = value;
                OnPropertyChanged();
            }
        }
        public Models.Component CurrentComponent
        {
            get => _currentComponent;
            set
            {
                if (value == null)
                {
                    _currentComponent = new Models.Component
                    {
                        ComponentTypeId = 8
                    };
                }
                else
                {
                    _currentComponent = value;
                    IsEditing = true;
                    LoadCharacteristics();
                    OnPropertyChanged();
                }
            }
        }
        public Models.ComponentCharacteristic Characteristic
        {
            get => _characteristic;
            set
            {
                _characteristic = value;
                OnPropertyChanged();
            }
        }
        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(OnSaveChanges);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);
        public GPUAddEditViewModel()
        {

        }

        private void OnSaveChanges()
        {
            if (IsEditing)
            {
                SetCharacteristics();

                try
                {
                    EquipmentEntities.GetContext().SaveChanges();
                    Exit();
                }
                catch (Exception ex)
                {
                }
            }
            else
            {
                SetCharacteristics();

                try
                {
                    EquipmentEntities.GetContext().Component.Add(CurrentComponent);
                    EquipmentEntities.GetContext().SaveChanges();
                    Exit();
                }
                catch (Exception ex)
                {
                }
            }
        }
        private void CloseDialog()
        {
            IsExitDialogOpen = false;
            IsSaveDialogOpen = false;
        }
        private void Exit()
        {
            CloseDialog();
            Views.AdminViews.ComponentsPage componentsPage = new Views.AdminViews.ComponentsPage();
            Classes.Manager.MenuPage.CurrentPage = componentsPage;
        }
        private void GoBack()
        {
            IsExitDialogOpen = true;
        }

        private void OpenSaveDialog()
        {
            IsSaveDialogOpen = true;
        }
        private void SetCharacteristics()
        {
            if (CurrentComponent != null)
            {
                if (IsEditing)
                {
                    ClearCharacteristics();
                }

                if (!string.IsNullOrWhiteSpace(VideoMemoryCapacity))
                {
                    var videoMemoryCapacityCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 44,
                        ComponentId = CurrentComponent.Id,
                        Value = VideoMemoryCapacity
                    };
                    AddCharacteristic(videoMemoryCapacityCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(MaxResolution))
                {
                    var maxResolutionCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 45,
                        ComponentId = CurrentComponent.Id,
                        Value = MaxResolution
                    };
                    AddCharacteristic(maxResolutionCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Power))
                {
                    var powerCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 46,
                        ComponentId = CurrentComponent.Id,
                        Value = Power
                    };
                    AddCharacteristic(powerCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Technology))
                {
                    var technologyCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 47,
                        ComponentId = CurrentComponent.Id,
                        Value = Technology
                    };
                    AddCharacteristic(technologyCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(VideoMemoryType))
                {
                    var memoryTypeCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 48,
                        ComponentId = CurrentComponent.Id,
                        Value = VideoMemoryType
                    };
                    AddCharacteristic(memoryTypeCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(ConnectionInterface))
                {
                    var connectionInterfaceCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 49,
                        ComponentId = CurrentComponent.Id,
                        Value = ConnectionInterface
                    };
                    AddCharacteristic(connectionInterfaceCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(MemoryBus))
                {
                    var busCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 50,
                        ComponentId = CurrentComponent.Id,
                        Value = MemoryBus
                    };
                    AddCharacteristic(busCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Frequency))
                {
                    var frequencyCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 51,
                        ComponentId = CurrentComponent.Id,
                        Value = Frequency
                    };
                    AddCharacteristic(frequencyCharacteristic);
                }
            }
        }
        private void AddCharacteristic(Models.ComponentCharacteristic characteristic)
        {
            try
            {
                EquipmentEntities.GetContext().ComponentCharacteristic.Add(characteristic);
                EquipmentEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
        }
        private void LoadCharacteristics()
        {
            if (CurrentComponent != null)
            {
                var characteristics = EquipmentEntities.GetContext().ComponentCharacteristic
                    .Where(cc => cc.ComponentId == CurrentComponent.Id).ToList();

                foreach (var characteristic in characteristics)
                {
                    switch (characteristic.ComponentTypeCharacteristicId)
                    {
                        case 44:
                            VideoMemoryCapacity = characteristic.Value;
                            break;
                        case 45:
                            MaxResolution = characteristic.Value;
                            break;
                        case 46:
                            Power = characteristic.Value;
                            break;
                        case 47:
                            Technology = characteristic.Value;
                            break;
                        case 48:
                            VideoMemoryType = characteristic.Value;
                            break;
                        case 49:
                            ConnectionInterface = characteristic.Value;
                            break;
                        case 50:
                            MemoryBus = characteristic.Value;
                            break;
                        case 51:
                            Frequency = characteristic.Value;
                            break;
                    }
                }
            }
        }

        private void ClearCharacteristics()
        {
            try
            {
                var existingCharacteristics = EquipmentEntities.GetContext().ComponentCharacteristic
                    .Where(cc => cc.ComponentId == CurrentComponent.Id).ToList();
                EquipmentEntities.GetContext().ComponentCharacteristic.RemoveRange(existingCharacteristics);
                EquipmentEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
