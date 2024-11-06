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
    internal class CoolerAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private string _selectedSocket;
        private string _selectedCoolingType;
        private string _size;
        private string _selectedBaseMaterial;
        private string _selectedRadiatorMaterial;
        private string _dissipatedPower;

        private int _countOfFans;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;

        public List<string> AvailableSockets => ConstLists.SOCKET_TYPES;
        public List<string> AvailableCoolingTypes => ConstLists.COOLING_TYPES;
        public List<string> AvailableBaseMaterials => ConstLists.BASE_MATERIALS;
        public List<string> AvailableRadiatorMaterials => ConstLists.RADIATOR_MATERIALS;
        public List<int> AvailableCoolerCount => ConstLists.NUMBER_OF_SATA;
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
                        ComponentTypeId = 5
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

        public string SelectedSocket
        {
            get => _selectedSocket;
            set
            {
                _selectedSocket = value;
                OnPropertyChanged();
            }
        }
        public string SelecetedCoolingType
        {
            get => _selectedCoolingType;
            set
            {
                _selectedCoolingType = value;
                OnPropertyChanged();
            }
        }
        public string Size
        {
            get => _size;
            set
            {
                _size = value;
                OnPropertyChanged();
            }
        }
        public int CountOfFans
        {
            get => _countOfFans;
            set
            {
                _countOfFans = value;
                OnPropertyChanged();
            }
        }
        public string SelectedBaseMaterial
        {
            get => _selectedBaseMaterial;
            set
            {
                _selectedBaseMaterial = value;
                OnPropertyChanged();
            }
        }

        public string SelectedRadiationMaterial
        {
            get => _selectedRadiatorMaterial;
            set
            {
                _selectedRadiatorMaterial = value;
                OnPropertyChanged();
            }
        }

        public string DissipatedPower
        {
            get => _dissipatedPower;
            set
            {
                _dissipatedPower = value;
                OnPropertyChanged();
            }
        }

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(OnSaveChanges);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);
        public CoolerAddEditViewModel()
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

                if (CountOfFans > -1)
                {
                    var countOfFansCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 27,
                        ComponentId = CurrentComponent.Id,
                        Value = CountOfFans.ToString()
                    };
                    AddCharacteristic(countOfFansCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelecetedCoolingType))
                {
                    var coolingTypeCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 28,
                        ComponentId = CurrentComponent.Id,
                        Value = SelecetedCoolingType
                    };
                    AddCharacteristic(coolingTypeCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(Size))
                {
                    var sizeCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 29,
                        ComponentId = CurrentComponent.Id,
                        Value = Size
                    };
                    AddCharacteristic(sizeCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedBaseMaterial))
                {
                    var baseMaterialCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 30,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedBaseMaterial
                    };
                    AddCharacteristic(baseMaterialCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(DissipatedPower))
                {
                    var dissipatedPowerCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 31,
                        ComponentId = CurrentComponent.Id,
                        Value = DissipatedPower
                    };
                    AddCharacteristic(dissipatedPowerCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedSocket))
                {
                    var socketCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 32,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedSocket
                    };
                    AddCharacteristic(socketCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedRadiationMaterial))
                {
                    var radiatorMaterialCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 33,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedRadiationMaterial
                    };
                    AddCharacteristic(radiatorMaterialCharacteristic);
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
                        case 27:
                            CountOfFans = int.Parse(characteristic.Value);
                            break;
                        case 28:
                            SelecetedCoolingType = characteristic.Value;
                            break;
                        case 29:
                            Size = characteristic.Value;
                            break;
                        case 30:
                            SelectedBaseMaterial = characteristic.Value;
                            break;
                        case 31:
                            DissipatedPower = characteristic.Value;
                            break;
                        case 32:
                            SelectedSocket = characteristic.Value;
                            break;
                        case 33:
                            SelectedRadiationMaterial = characteristic.Value;
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
