using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using EquipmentAccounting.Classes;
using EquipmentAccounting.Models;

namespace EquipmentAccounting.ViewModels
{
    internal class MotherboardAddEditViewModel : INotifyPropertyChanged
    {
        private bool _isSaveDialogOpen;
        private bool _isExitDialogOpen;
        private bool _isEditing;

        private int _selectedCountOfRAM;
        private int _selectedCountOfM2;
        private int _selectedCountOfSATA;

        private string _connectionInterfaces;
        private string _selectedMotherboardFormFactor;
        private string _selectedRAMType;
        private string _selectedSocket;
        private string _selectedChipset;

        private Models.Component _currentComponent;
        private Models.ComponentCharacteristic _characteristic;
        public int SelectedCountOfRAM
        {
            get => _selectedCountOfRAM;
            set
            {
                _selectedCountOfRAM = value;
                OnPropertyChanged();
            }
        }

        public int SelectedCountOfM2
        {
            get => _selectedCountOfM2;
            set
            {
                _selectedCountOfM2 = value;
                OnPropertyChanged();
            }
        }

        public int SelectedCountOfSATA
        {
            get => _selectedCountOfSATA;
            set
            {
                _selectedCountOfSATA = value;
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

        public string ConnectionInterfaces
        {
            get => _connectionInterfaces;
            set
            {
                _connectionInterfaces = value;
                OnPropertyChanged();
            }
        }

        public string SelectedMotherBoardFormFactor
        {
            get => _selectedMotherboardFormFactor;
            set
            {
                _selectedMotherboardFormFactor = value;
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
        public string SelectedSocket
        {
            get => _selectedSocket;
            set
            {
                _selectedSocket = value;
                OnPropertyChanged();
            }
        }

        public string SelectedChipset
        {
            get => _selectedChipset;
            set
            {
                _selectedChipset = value;
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
                        ComponentTypeId = 2
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

        public List<string> AvailableSockets => ConstLists.SOCKET_TYPES;
        public List<string> AvailableChipsets => ConstLists.CHIPSETS;
        public List<string> AvailableMotherboardFormFactors => ConstLists.MOTHERBOARDS_FORM_FACTORS;
        public List<string> AvailableRAMTypes => ConstLists.RAM_TYPES;
        public List<int> AvailableRAMCounts => ConstLists.NUMBER_OF_RAM;
        public List<int> AvailableSATACounts => ConstLists.NUMBER_OF_SATA;
        public List<int> AvailableM2Counts => ConstLists.NUMBER_OF_M2;

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand ExitCommand => new RelayCommand(Exit);
        public ICommand CloseDialogCommand => new RelayCommand(CloseDialog);
        public ICommand SaveCommand => new RelayCommand(OnSaveChanges);
        public ICommand OpenSaveDialogCommand => new RelayCommand(OpenSaveDialog);
        public MotherboardAddEditViewModel()
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
            _isExitDialogOpen = false;
            _isSaveDialogOpen = false;
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

                if (!string.IsNullOrWhiteSpace(SelectedMotherBoardFormFactor))
                {
                    var motherBoardFormFactorCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 8,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedMotherBoardFormFactor
                    };
                    AddCharacteristic(motherBoardFormFactorCharacteristic);
                }

                if (SelectedCountOfRAM > 0)
                {
                    MessageBox.Show("Добавление характеристики количества памяти");
                    var countOfRamCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 12,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedCountOfRAM.ToString()
                    };
                    AddCharacteristic(countOfRamCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedRAMType))
                {
                    var typeRAMCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 9,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedRAMType
                    };
                    AddCharacteristic(typeRAMCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedSocket))
                {
                    var socketCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 11,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedSocket
                    };
                    AddCharacteristic(socketCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(SelectedChipset))
                {
                    var chipsetCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 14,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedChipset
                    };
                    AddCharacteristic(chipsetCharacteristic);
                }

                if (!string.IsNullOrWhiteSpace(ConnectionInterfaces))
                {
                    var connectionInterfacesCharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 10,
                        ComponentId = CurrentComponent.Id,
                        Value = ConnectionInterfaces
                    };
                    AddCharacteristic(connectionInterfacesCharacteristic);
                }

                if (SelectedCountOfM2 > -1)
                {
                    var countOfM2Characteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 13,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedCountOfM2.ToString()
                    };
                    AddCharacteristic(countOfM2Characteristic);
                }

                if (SelectedCountOfSATA > -1)
                {
                    var countOfSATACharacteristic = new Models.ComponentCharacteristic
                    {
                        ComponentTypeCharacteristicId = 15,
                        ComponentId = CurrentComponent.Id,
                        Value = SelectedCountOfSATA.ToString()
                    };
                    AddCharacteristic(countOfSATACharacteristic);
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
                        case 8:
                            SelectedMotherBoardFormFactor = characteristic.Value;
                            break;
                        case 9:
                            SelectedRAMType = characteristic.Value;
                            break;
                        case 10:
                            ConnectionInterfaces = characteristic.Value;
                            break;
                        case 11:
                            SelectedSocket = characteristic.Value;
                            break;
                        case 12:
                            SelectedCountOfRAM = int.Parse(characteristic.Value);
                            break;
                        case 13:
                            SelectedCountOfM2 = int.Parse(characteristic.Value);
                            break;
                        case 14:
                            SelectedChipset = characteristic.Value;
                            break;
                        case 15:
                            SelectedCountOfSATA = int.Parse(characteristic.Value);
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
                MessageBox.Show($"{existingCharacteristics.Count}");
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
