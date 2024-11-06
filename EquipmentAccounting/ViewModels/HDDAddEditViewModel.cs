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
    internal class HDDAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private string _selectedMemoryCapacity;
        private string _readingSpeed;
        private string _writingSpeed;
        private string _power;
        private string _connectionInterface;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        public List<string> AvailableMemoryCapacities => ConstLists.STORAGE_CAPACITY;
        public string SelectedMemoryCapacity
        {
            get => _selectedMemoryCapacity;
            set
            {
                _selectedMemoryCapacity = value;
                OnPropertyChanged();
            }
        }
        public string ReadingSpeed
        {
            get => _readingSpeed;
            set
            {
                _readingSpeed = value;
                OnPropertyChanged();
            }
        }

        public string WritingSpeed
        {
            get => _writingSpeed;
            set
            {
                _writingSpeed = value;
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
        public string ConnectionInterface
        {
            get => _connectionInterface;
            set
            {
                _connectionInterface = value;
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
        public Models.Component CurrentComponent
        {
            get => _currentComponent;
            set
            {
                if (value == null)
                {
                    _currentComponent = new Models.Component
                    {
                        ComponentTypeId = 7
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
        public HDDAddEditViewModel()
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

                if (!string.IsNullOrWhiteSpace(SelectedMemoryCapacity))
                {
                    var memoryCapacityCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 39,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedMemoryCapacity
                    };
                    AddCharacteristic(memoryCapacityCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(ReadingSpeed))
                {
                    var readingSpeedCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 40,
                        ComponentId = CurrentComponent.Id,
                        Value = ReadingSpeed
                    };
                    AddCharacteristic(readingSpeedCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(WritingSpeed))
                {
                    var writingSpeedCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 41,
                        ComponentId = CurrentComponent.Id,
                        Value = WritingSpeed
                    };
                    AddCharacteristic(writingSpeedCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(ConnectionInterface))
                {
                    var connectionInterfaceCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 42,
                        ComponentId = CurrentComponent.Id,
                        Value = ConnectionInterface
                    };
                    AddCharacteristic(connectionInterfaceCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Power))
                {
                    var powerCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 43,
                        ComponentId = CurrentComponent.Id,
                        Value = Power
                    };
                    AddCharacteristic(powerCharacteristic);
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
                        case 39:
                            SelectedMemoryCapacity = characteristic.Value;
                            break;
                        case 40:
                            ReadingSpeed = characteristic.Value;
                            break;
                        case 41:
                            WritingSpeed = characteristic.Value;
                            break;
                        case 42:
                            ConnectionInterface = characteristic.Value;
                            break;
                        case 43:
                            Power = characteristic.Value;
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
