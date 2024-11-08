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
    internal class RAMAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private int _selectedCountOfRAM;
        private int _selectedMemoryCapacity;

        private string _frequency;
        private string _selectedRAMType;
        private string _selectedFormFactor;
        private string _timings;
        private string _connectionInterfaces;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        public List<int> AvailableCountsOfRAM => ConstLists.NUMBER_OF_RAM;
        public List<int> AvailableMemoryCapacities => ConstLists.RAM_CAPACITIES;
        public List<string> AvailableRAMTypes => ConstLists.RAM_TYPES;
        public List<string> AvailableRAMFormFactors => ConstLists.RAM_FORM_FACTORS;

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

        public int SelectedCountOfRAM
        {
            get => _selectedCountOfRAM;
            set
            {
                _selectedCountOfRAM = value;
                OnPropertyChanged();
            }
        }

        public int SelectedMemoryCapacity
        {
            get => _selectedMemoryCapacity;
            set
            {
                _selectedMemoryCapacity = value;
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

        public string SelectedRAMType
        {
            get => _selectedRAMType;
            set
            {
                _selectedRAMType = value;
                OnPropertyChanged();
            }
        }

        public string SelectedFormFactor
        {
            get => _selectedFormFactor;
            set
            {
                _selectedFormFactor = value;
                OnPropertyChanged();
            }
        }

        public string Timings
        {
            get => _timings;
            set
            {
                _timings = value;
                OnPropertyChanged();
            }
        }

        public string ConnectionInterfaces
        {
            get => _connectionInterfaces;
            set
            {
                _connectionInterfaces = value;
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
                        ComponentTypeId = 3
                    };
                }
                else
                {
                    _currentComponent = value;
                    MessageBox.Show(_currentComponent.ComponentTypeId + " ");
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

        public RAMAddEditViewModel()
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

                if (SelectedMemoryCapacity > 0)
                {
                    var memoryCapacityCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 16,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedMemoryCapacity.ToString()
                    };
                    AddCharacteristic(memoryCapacityCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedRAMType))
                {
                    var typeRAMCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 17,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedRAMType
                    };
                    AddCharacteristic(typeRAMCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(ConnectionInterfaces))
                {
                    var connectionInterfacesCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 18,
                        ComponentId = CurrentComponent.Id,
                        Value = ConnectionInterfaces
                    };
                    AddCharacteristic(connectionInterfacesCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Frequency))
                {
                    var frequencyCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 19,
                        ComponentId = CurrentComponent.Id,
                        Value = Frequency
                    };
                    AddCharacteristic(frequencyCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedFormFactor))
                {
                    var formFactorCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 20,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedFormFactor
                    };
                    AddCharacteristic(formFactorCharacteristic);
                }


                if (SelectedCountOfRAM > 0)
                {
                    var countOfRAMCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 21,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedCountOfRAM.ToString()
                    };
                    AddCharacteristic(countOfRAMCharacteristic);
                }

                if (!string.IsNullOrEmpty(Timings))
                {
                    var timingsCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 22,
                        ComponentId = CurrentComponent.Id,
                        Value = Timings
                    };
                    AddCharacteristic(timingsCharacteristic);
                }
            }
        }
        private void LoadCharacteristics()
        {
            if (CurrentComponent != null)
            {
                MessageBox.Show("АУ");
                var characteristics = EquipmentEntities.GetContext().ComponentCharacteristic
                    .Where(cc => cc.ComponentId == CurrentComponent.Id).ToList();

                foreach (var characteristic in characteristics)
                {

                    switch (characteristic.ComponentTypeCharacteristicId)
                    {
                        case 16:
                            SelectedMemoryCapacity = int.Parse(characteristic.Value);
                            break;
                        case 17:
                            SelectedRAMType = characteristic.Value;
                            break;
                        case 18:
                            ConnectionInterfaces = characteristic.Value;
                            break;
                        case 19:
                            Frequency = characteristic.Value;
                            break;
                        case 20:
                            SelectedFormFactor = characteristic.Value;
                            break;
                        case 21:
                            SelectedCountOfRAM = int.Parse(characteristic.Value);
                            break;
                        case 22:
                            Timings = characteristic.Value;
                            break;
                    }
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
